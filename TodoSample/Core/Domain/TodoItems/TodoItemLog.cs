using Honamic.Framework.Domain;

namespace Honamic.Todo.Domain.TodoItems;

public class TodoItemLog : Entity<long>
{
    public long TodoItemRef { get; set; }

    public string Type { get; set; }
    
    public string Description { get; set; }
}