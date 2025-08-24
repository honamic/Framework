using Honamic.Framework.Events;

namespace Honamic.Framework.Messaging;

public class DisableEventStore : IEventStore
{
    public Task AddAsync(IEvent eventObject, Type handlerType)
    {
        throw new NotImplementedException(nameof(EventExecutionTiming.AfterCommit));
    }
}