namespace Honamic.Framework.Events;

public class DisableEventStore : IEventStore
{
    public Task AddAsync(IEvent eventObject, Type handlerType)
    {
        throw new NotImplementedException(nameof(EventExecutionTiming.AfterCommit));
    }
}