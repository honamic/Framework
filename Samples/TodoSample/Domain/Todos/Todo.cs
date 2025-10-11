using Honamic.Framework.Domain;
using TodoSample.Todos;

namespace TodoSample.Domain.Todos;

public class Todo : AggregateRoot<long>
{
    public string Title { get; private set; } = default!;
    public string? Description { get; private set; }
    public TodoStatus Status { get; private set; }
    public DateTime? DoneAt { get; private set; }

    public static Todo Create(long id, string title, string? description)
    {
        var newTodo = new Todo
        {
            Id = id,
            Title = title ?? throw new ArgumentNullException(nameof(title)),
            Description = description,
            Status = TodoStatus.Pending,
        };

        return newTodo;
    }

    public void MarkAsDone()
    {
        if (Status == TodoStatus.Completed)
            return;

        Status = TodoStatus.Completed;
        DoneAt = DateTime.UtcNow;
    }
}