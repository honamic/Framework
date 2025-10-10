namespace Honamic.Framework.Events;

public interface IDomainEventsDispatcher
{
    Task DispatchEventsAsync();
}
