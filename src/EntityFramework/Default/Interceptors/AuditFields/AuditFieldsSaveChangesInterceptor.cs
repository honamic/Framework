using Honamic.Framework.Domain;
using Honamic.Framework.Domain.Audits;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Reflection;

namespace Honamic.Framework.EntityFramework.Interceptors.AuditFields;
public class AuditFieldsSaveChangesInterceptor : SaveChangesInterceptor
{
    private readonly IUserContext _userContext;
    private readonly AuditType auditType;

    public AuditFieldsSaveChangesInterceptor(IUserContext userContext, AuditType auditType)
    {
        _userContext = userContext;
        this.auditType = auditType;
    }

    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        ApplyAudits(eventData.Context.ChangeTracker);

        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        ApplyAudits(eventData.Context.ChangeTracker);

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private void ApplyAudits(ChangeTracker changeTracker)
    {
        var currentUser = CreateUserName();
        var now = DateTimeOffset.Now;

        ApplyCreateAudits(changeTracker, currentUser, now);
        ApplyUpdateAudits(changeTracker, currentUser, now);
    }

    private void ApplyCreateAudits(ChangeTracker changeTracker, string? currentUserId, DateTimeOffset now)
    {
        var addedEntries = changeTracker.Entries<IAuditCreate>().Where(x => x.State == EntityState.Added);

        foreach (var entry in addedEntries)
        {
            var entityType = entry.Entity.GetType();

            var createdOnProp = entityType.GetProperty(nameof(IAuditCreate.CreatedOn));
            var createdByProp = entityType.GetProperty(nameof(IAuditCreate.CreatedBy));

            createdOnProp?.SetValue(entry.Entity, now);
            createdByProp?.SetValue(entry.Entity, currentUserId);
        }

    }

    private void ApplyUpdateAudits(ChangeTracker changeTracker, string? currentUserId, DateTimeOffset now)
    {
        var modifiedEntries = changeTracker.Entries<IAuditUpdate>().Where(x => x.State == EntityState.Modified);

        foreach (var entry in modifiedEntries)
        {
            var entityType = entry.Entity.GetType();

            var modifiedOnProp = entityType.GetProperty(nameof(IAuditUpdate.ModifiedOn));
            var modifiedByProp = entityType.GetProperty(nameof(IAuditUpdate.ModifiedBy));

            modifiedOnProp?.SetValue(entry.Entity, now);
            modifiedByProp?.SetValue(entry.Entity, currentUserId);
        }

    }

    private string? CreateUserName()
    {
        var userId = _userContext.GetCurrentUserId();
        var username = _userContext.GetCurrentUsername();

        string? result = auditType switch
        {
            AuditType.UserId => string.IsNullOrWhiteSpace(userId) ? null : userId,
            AuditType.UserName => string.IsNullOrWhiteSpace(username) ? null : username,
            AuditType.UserIDAndName => CombineNonEmpty(userId, username),
            AuditType.UserNameAndId => CombineNonEmpty(username, userId),
            _ => throw new ArgumentOutOfRangeException(nameof(auditType), auditType, "Invalid AuditType specified."),
        };

        if (result?.Length > 100)
        {
            result = result.Substring(0, 100);
        }
        return result;
    }

    private static string? CombineNonEmpty(string? first, string? second)
    {
        if (string.IsNullOrWhiteSpace(first) && string.IsNullOrWhiteSpace(second))
            return null;
        if (string.IsNullOrWhiteSpace(first))
            return second;
        if (string.IsNullOrWhiteSpace(second))
            return first;
        return $"{first} | {second}";
    }
}