using Honamic.Framework.Events;

namespace Honamic.Framework.Domain;

public abstract class DomainEvent : IDomianEvent
{
    protected DomainEvent(long aggregateId)
    {
        EventId = Guid.NewGuid();
        AggregateId = aggregateId;
        OccurredOn = DateTimeOffset.Now;
    }

    public Guid EventId { get; }

    public DateTimeOffset OccurredOn { get; }

    public long AggregateId { get; }

    public long AggregateVersion { get; private set; }


    public void SetAggregateVersion(long aggregateVersion)
    {
        AggregateVersion = aggregateVersion;
    }
}