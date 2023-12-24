using Honamic.Framework.Events;
using Honamic.IdentityPlus.Domain.Users;

namespace Honamic.Todo.Application.TodoItems.EventHandlers;
public class UserCreatedEventHandler : IEventHandler<UserCreatedEvent>
{
    public Task HandleAsync(UserCreatedEvent eventToHandle)
    {
       return Task.CompletedTask;
    }
}
