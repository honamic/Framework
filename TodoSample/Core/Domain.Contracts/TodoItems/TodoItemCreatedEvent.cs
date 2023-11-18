using Honamic.Framework.Domain;

namespace Honamic.Todo.Domain.Messages;

public class TodoItemCreatedEvent : DomainEvent
{
    public TodoItemCreatedEvent(long aggregateId) : base(aggregateId)
    {

    }
}