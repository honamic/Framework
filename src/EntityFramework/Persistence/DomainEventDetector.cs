using Honamic.Framework.Domain;
using Honamic.Framework.Events;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Honamic.Framework.Persistence.EntityFramework;

internal class DomainEventDetector : IDomainEventDetector
{
    private readonly DbContext _dbContext;

    public DomainEventDetector(DbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IEnumerable<IEvent> GetAndClearDomainEvents()
    {
        var domainEntities = GetDomainEntities();

        var domainEvents = domainEntities
            .SelectMany(x => x.Entity.Events)
            .Where(@event => @event is DomainEvent)
            .ToList();

        foreach (var entity in domainEntities)
        {
            entity.Entity.ClearEvents();
        }

        return domainEvents;
    }

    private List<EntityEntry<IAggregateRoot>> GetDomainEntities()
    {
        return _dbContext.ChangeTracker
        .Entries<IAggregateRoot>()
        .Where(x => x.Entity.Events != null && x.Entity.Events.Any()).ToList();
    }
}