using Honamic.Framework.Applications.Authorizes;
using Honamic.Framework.Queries; 

namespace Honamic.Todo.Query.Domain.TodoItems.Queries;

[DynamicAuthorize]
public class GetAllTodoItemsQueryFilter: PagedQueryFilter, IQueryFilter
{
    public GetAllTodoItemsQueryFilter()
    {
        OrderBy = "Id desc";
    }
}
