using Honamic.Framework.Events;
using Honamic.IdentityPlus.Domain.Users;

namespace Honamic.Todo.Application.TodoItems.EventHandlers;
public class UserLoggedEventHandler : IEventHandler<UserLoggedEvent>
{
    public Task HandleAsync(UserLoggedEvent eventToHandle)
    {
       return Task.CompletedTask;
    }
}
