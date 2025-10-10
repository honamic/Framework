using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Honamic.Framework.Events;

public class EventBus : IEventBus
{
    private readonly IList<(object handler, string eventType)> _subscribers =
        new List<(object handler, string eventType)>();
    private readonly IServiceProvider _serviceProvider;
    private readonly IEventStore _eventStore;

    public EventBus(IServiceProvider serviceProvider, IEventStore eventStore)
    {
        _serviceProvider = serviceProvider;
        _eventStore = eventStore;
    }

    public void On<TEvent>(Action<TEvent> action) where TEvent : IEvent
    {
        var eventType = typeof(TEvent).FullName;

        if (_subscribers.Any(handler => Equals(handler.handler, action)))
        {
            throw new EventAlreadyAddedException();
        }

        _subscribers.Add((action, eventType));
    }

    public async Task PublishAsync<TEvent>(TEvent eventToPublish)
        where TEvent : IEvent
    {
        var eligibleSubscribers = GetActionSubscribers(eventToPublish);

        eligibleSubscribers.ForEach(handler =>
        {
            if (handler is MulticastDelegate action)
            {
                action.DynamicInvoke(eventToPublish);
                return;
            }
        });

        var handlers = GetHandlerSubscribers(eventToPublish);

        if (handlers is null)
            return;

        foreach (var handler in handlers)
        {
            if (handler is null)
                continue;
            var executionTiming= GetExecutionTiming(handler);
            switch (executionTiming)
            {
                case EventExecutionTiming.BeforeCommit:
                    await InvokHandler(eventToPublish, handler);
                    break;
                case EventExecutionTiming.AfterCommit:
                    await _eventStore.AddAsync(eventToPublish, handler.GetType());
                    break;
                default:
                    throw new InvalidOperationException($"Unknown event execution timing.[{executionTiming}]");
            }
        }
    }

    private EventExecutionTiming GetExecutionTiming(object handler)
    {
        var handlerType = handler.GetType();
        var executionTimingProperty = handlerType.GetProperty("ExecutionTiming");
        if (executionTimingProperty == null)
            throw new InvalidOperationException("Handler does not have an ExecutionTiming property.");

        var value = executionTimingProperty.GetValue(handler);
        if (value is EventExecutionTiming timing)
            return timing;

        throw new InvalidOperationException("ExecutionTiming property is not of type EventExecutionTiming.");
    }

    private static Task InvokHandler<TEvent>(TEvent eventToPublish, object handler) where TEvent : IEvent
    {
        Type handlerType = handler.GetType();
        MethodInfo handleAsyncMethodInfo = handlerType.GetMethod("HandleAsync")!;
        var task = (Task)handleAsyncMethodInfo!.Invoke(handler, new object[] { eventToPublish })!;
        return task;
    }



    private IEnumerable<object?>? GetHandlerSubscribers<TEvent>(TEvent eventToPublish) where TEvent : IEvent
    {
        var eventType = eventToPublish.GetType();
        var handlerType = typeof(IEventHandler<>).MakeGenericType(eventType);
        var list = _serviceProvider.GetServices(handlerType);

        return list?.Where(x => x != null);
    }


    private List<object> GetActionSubscribers<TEvent>(TEvent eventToPublish) where TEvent : IEvent
    {
        var eventType = eventToPublish.GetType().FullName;

        var handlers = _subscribers
            .Where(e => e.eventType == eventType)
            .ToList();

        foreach (var handler in handlers)
        {
            _subscribers.Remove(handler);
        }

        return handlers.Select(c => c.handler).ToList();
    }
}