namespace Honamic.Framework.Events;

public class DomainEventsDispatcher : IDomainEventsDispatcher
{
    private readonly IDomainEventDetector _domainEventDetector;
    private readonly IEventBus _eventBus;

    public DomainEventsDispatcher(IDomainEventDetector domainEventDetector, IEventBus eventBus)
    {
        _domainEventDetector = domainEventDetector;
        _eventBus = eventBus;
    }

    public async Task DispatchEventsAsync()
    {
        var domainEvents = _domainEventDetector.GetAndClearDomainEvents();

        foreach (var domainEvent in domainEvents)
        {
            await _eventBus.PublishAsync(domainEvent);
        }
    }
}
