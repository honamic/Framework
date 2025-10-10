namespace Honamic.Framework.Events;

public interface IEvent
{
    Guid EventId { get; }
    public DateTimeOffset OccurredOn { get;}
}
