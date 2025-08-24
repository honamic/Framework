using Honamic.Framework.Events;
using Honamic.Todo.Domain.TodoItems;

namespace Honamic.Todo.Application.TodoItems.EventHandlers;
internal class TodoItemCreatedEventHandler : IEventHandler<TodoItemCreatedEvent>
{
    public EventExecutionTiming ExecutionTiming => EventExecutionTiming.BeforeCommit;

    public Task HandleAsync(TodoItemCreatedEvent eventToHandle)
    {
        
        return Task.CompletedTask;
    }
}
