using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Honamic.Framework.Events;

public class EventBus : IEventBus
{
    private readonly IList<(object handler, string eventType)> _subscribers =
        new List<(object handler, string eventType)>();
    private readonly IServiceProvider _serviceProvider;

    public EventBus(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
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

        IEnumerable<object> handlers = GetHandlerSubscribers(eventToPublish);

        foreach (var handler in handlers)
        {
            Type handlerType = handler.GetType();
            MethodInfo handleAsyncMethodInfo = handlerType.GetMethod("HandleAsync");
            var task = (Task)handleAsyncMethodInfo.Invoke(handler, new object[] { eventToPublish });

            await task;
        }
    }

    private IEnumerable<object> GetHandlerSubscribers<TEvent>(TEvent eventToPublish) where TEvent : IEvent
    {
        var eventType = eventToPublish.GetType();
        var handlerType = typeof(IEventHandler<>).MakeGenericType(eventType);
        var list = _serviceProvider.GetServices(handlerType);
        return list;
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