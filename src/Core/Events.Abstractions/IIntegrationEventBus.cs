
namespace Honamic.Framework.Events;

public interface IIntegrationEventBus
{
    Task PublishAsync<TEvent>(TEvent eventToPublish) where TEvent : IIntegrationEvent;
}