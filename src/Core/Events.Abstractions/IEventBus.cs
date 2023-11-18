namespace Honamic.Framework.Events;

public interface IEventBus
{
    Task PublishAsync<TEvent>(TEvent eventToPublish)
        where TEvent : IEvent;

    void On<TEvent>(Action<TEvent> action) where TEvent : IEvent;
}
