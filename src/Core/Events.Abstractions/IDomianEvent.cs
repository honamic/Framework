namespace Honamic.Framework.Events;

public interface IDomianEvent : IEvent
{
    public long AggregateId { get; }

    public long AggregateVersion { get;}
}
