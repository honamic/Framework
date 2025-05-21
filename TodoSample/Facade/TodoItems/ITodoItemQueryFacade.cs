using Honamic.Framework.Applications.Results;
using Honamic.Framework.Facade;
using Honamic.Framework.Queries;
using Honamic.Todo.Query.Domain.TodoItems.Models;
using Honamic.Todo.Query.Domain.TodoItems.Queries;

namespace Honamic.Todo.Facade.TodoItems;

public interface ITodoItemQueryFacade : IBaseFacade
{
    Task<Result<TodoItemQuery>> GetTodoItem(long id, CancellationToken cancellationToken);

    Task<Result<PagedQueryResult<GetAllTodoItemsQueryResult>>> GetAllTodoItem(GetAllTodoItemsQueryFilter filter, CancellationToken cancellationToken);
}
