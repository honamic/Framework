using Honamic.Framework.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Honamic.Framework.Persistence.EntityFramework.Interceptors.IsDeletedInfos;

public class MarkAsDeletedSaveChangesInterceptor : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData,
        InterceptionResult<int> result)
    {
        RemoveIsDeletedItems(eventData.Context.ChangeTracker);

        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        RemoveIsDeletedItems(eventData.Context.ChangeTracker);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    public void RemoveIsDeletedItems(ChangeTracker changeTracker)
    {
        var entries = changeTracker.Entries();
        var aggregateRoots = entries.Where(x => x.Entity is IAggregateRoot);
        foreach (var aggregateRoot in aggregateRoots)
        {
            if ((aggregateRoot.Entity as IAggregateRoot)!.IsMarkAsDeleted())
                aggregateRoot.State = EntityState.Deleted;
        }
    }
}