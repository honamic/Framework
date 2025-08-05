using Honamic.Framework.Applications.Authorizes;
using Honamic.Framework.Applications.Results;
using Honamic.Framework.Queries; 

namespace Honamic.Todo.Query.Domain.TodoItems.Queries;

[DynamicAuthorize]
public class GetAllTodoItemsQueryFilter: PagedQueryFilter, IQuery<Result<PagedQueryResult<GetAllTodoItemsQueryResult>>>
{
    public GetAllTodoItemsQueryFilter()
    {
        OrderBy = "Id desc";
    }
}
