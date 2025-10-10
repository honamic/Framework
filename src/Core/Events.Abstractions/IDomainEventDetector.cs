namespace Honamic.Framework.Events;

public interface IDomainEventDetector
{
    IEnumerable<IEvent> GetAndClearDomainEvents();
}
