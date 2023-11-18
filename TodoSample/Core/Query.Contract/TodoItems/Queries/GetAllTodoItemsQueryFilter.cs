using Honamic.Framework.Queries;

namespace Honamic.Todo.Query.Domain.TodoItems.Queries;
public class GetAllTodoItemsQueryFilter: PagedQueryFilter, IQueryFilter
{
    public GetAllTodoItemsQueryFilter()
    {
        OrderBy = "Id desc";
    }
}
