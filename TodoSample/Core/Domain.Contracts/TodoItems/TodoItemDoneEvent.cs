using Honamic.Framework.Domain;

namespace Honamic.Todo.Domain.TodoItems;

public class TodoItemDoneEvent : DomainEvent
{
    public TodoItemDoneEvent(long aggregateId) : base(aggregateId)
    {

    }
}