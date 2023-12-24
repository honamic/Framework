using Honamic.Framework.Events;

namespace Honamic.Framework.Domain;

public abstract class DomainEvent : IEvent
{
    protected DomainEvent(long aggregateId)
    {
        AggregateId = aggregateId;
        OccurredOn = DateTimeOffset.Now;
    }

    public DateTimeOffset OccurredOn { get; private set; }

    public long AggregateId { get; private set; }

    public long AggregateVersion { get; private set; }

    public IEventUserInfo? UserInfo { get; private set; }

    public void SetUserContextValue(IEventUserInfo userInfo)
    {
        UserInfo = userInfo;
    }

    public void SetAggregateVersion(long aggregateVersion)
    {
        AggregateVersion = aggregateVersion;
    }
}