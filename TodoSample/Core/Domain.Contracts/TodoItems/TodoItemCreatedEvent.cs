using Honamic.Framework.Domain;

namespace Honamic.Todo.Domain.TodoItems;

public class TodoItemCreatedEvent : DomainEvent
{
    public TodoItemCreatedEvent(long aggregateId) : base(aggregateId)
    {

    }
}