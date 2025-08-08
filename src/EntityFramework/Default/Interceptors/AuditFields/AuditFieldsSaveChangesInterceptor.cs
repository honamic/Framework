using Honamic.Framework.Domain;
using Honamic.Framework.Domain.Audits;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

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
        foreach (var addedEntry in addedEntries)
        {
            addedEntry.Entity.CreatedOn = now;
            addedEntry.Entity.CreatedBy = currentUserId;
        }
    }

    private void ApplyUpdateAudits(ChangeTracker changeTracker, string? currentUserId, DateTimeOffset now)
    {
        var addedEntries = changeTracker.Entries<IAuditUpdate>().Where(x => x.State == EntityState.Modified);
        foreach (var addedEntry in addedEntries)
        {
            addedEntry.Entity.ModifiedOn = now;
            addedEntry.Entity.ModifiedBy = currentUserId;
        }
    }

    private string? CreateUserName()
    {
        var userId = _userContext.GetCurrentUserId();
        var username = _userContext.GetCurrentUsername();
        string? result = null;
        result = auditType switch
        {
            AuditType.UserId => userId,
            AuditType.UserName => username,
            AuditType.UserIDAndName => $"{userId} | {username}",
            AuditType.UserNameAndId => $"{username} | {userId}",
            _ => throw new ArgumentOutOfRangeException(nameof(auditType), auditType, "Invalid AuditType specified."),
        };
        if (result?.Length > 100)
        {
            result = result.Substring(0, 100);
        }
        return result;
    }
}