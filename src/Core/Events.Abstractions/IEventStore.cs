
namespace Honamic.Framework.Events;

public interface IEventStore
{
    Task AddAsync(IEvent eventObject, Type handlerType);
}