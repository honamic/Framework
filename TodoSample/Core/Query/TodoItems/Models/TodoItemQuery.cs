using Honamic.Framework.Queries;

namespace Honamic.Todo.Query.Domain.TodoItems.Models;
public class TodoItemQuery : AggregateQueryBase<long>
{
    public string Title { get; set; }

    public string Content { get; set; }

    public bool Done { get; set; }

    public List<string> Tags { get; set; }
}