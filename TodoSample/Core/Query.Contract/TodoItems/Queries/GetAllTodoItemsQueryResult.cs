using Honamic.Framework.Queries;

namespace Honamic.Todo.Query.Domain.TodoItems.Queries;

public class GetAllTodoItemsQueryResult: IQueryResult
{
    public string Title { get; set; }

    public string Content { get; set; }

    public bool Done { get; set; }

    public List<string> Tags { get; set; }
}