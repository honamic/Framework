using Honamic.Framework.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;

namespace Honamic.Framework.Persistence.EntityFramework.Interceptors.AggregateRootVersion;
public class AggregateRootVersionInterceptor : SaveChangesInterceptor
{
    private readonly ILogger<AggregateRootVersionInterceptor> _logger;

    public AggregateRootVersionInterceptor(ILogger<AggregateRootVersionInterceptor> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData,
        InterceptionResult<int> result)
    {
        if (eventData == null)
        {
            throw new ArgumentNullException(nameof(eventData));
        }

        BeforeSaveTriggers(eventData.Context);
        return result;
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        if (eventData == null)
        {
            throw new ArgumentNullException(nameof(eventData));
        }

        BeforeSaveTriggers(eventData.Context);
        return ValueTask.FromResult(result);
    }

    private void BeforeSaveTriggers(DbContext context)
    {
        if (context?.ChangeTracker != null)
        {
            ApplyVersion(context.ChangeTracker);
        }
    }

    private void ApplyVersion(ChangeTracker changeTracker)
    {
        var aggregateRoots = changeTracker
            .Entries<AggregateRoot<long>>()
            .Where(c => c.State == EntityState.Added || c.State == EntityState.Modified);

        foreach (var aggregateRoot in aggregateRoots)
        {
            aggregateRoot.Entity.Version =
               aggregateRoot.OriginalValues.GetValue<long>(nameof(AggregateRoot<long>.Version))
               + 1;
        }
    }
}