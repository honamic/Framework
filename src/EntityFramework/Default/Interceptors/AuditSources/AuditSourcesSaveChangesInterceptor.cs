using Honamic.Framework.Domain.Audits;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Honamic.Framework.Persistence.EntityFramework.Interceptors.AuditSources;
public class AuditSourcesSaveChangesInterceptor : SaveChangesInterceptor
{
    private readonly IAuditSourcesProvider _auditSourcesProvider;

    public AuditSourcesSaveChangesInterceptor(IAuditSourcesProvider auditSourcesProvider)
    {
        _auditSourcesProvider = auditSourcesProvider;
    }

    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData,
        InterceptionResult<int> result)
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
        var auditSourceValues = _auditSourcesProvider.GetAuditSourceValues().SerializeJson();

        ApplyCreateAudits(changeTracker, auditSourceValues);
        ApplyUpdateAudits(changeTracker, auditSourceValues);
    }

    private void ApplyCreateAudits(ChangeTracker changeTracker, string auditSourceValues)
    {
        var addedEntries = changeTracker.Entries<IAuditCreateSources>()
                                        .Where(x => x.State == EntityState.Added);

        foreach (var addedEntry in addedEntries)
        {
            addedEntry.Entity.CreatedSources = auditSourceValues;
        }
    }

    private void ApplyUpdateAudits(ChangeTracker changeTracker, string auditSourceValues)
    {
        var modifiedEntries = changeTracker.Entries<IAuditUpdateSources>()
                            .Where(x => x.State == EntityState.Modified);

        foreach (var modifiedEntry in modifiedEntries)
        {
            modifiedEntry.Entity.ModifiedSources = auditSourceValues;
        }
    }
}