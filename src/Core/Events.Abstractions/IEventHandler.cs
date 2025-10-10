namespace Honamic.Framework.Events;

public interface IEventHandler<in TEvent> where TEvent : IEvent
{
    EventExecutionTiming ExecutionTiming { get; }

    Task HandleAsync(TEvent eventToHandle);
}
