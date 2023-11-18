using Honamic.Framework.Queries;
using Honamic.Todo.Query.Domain.TodoItems.Models;
using Honamic.Todo.Query.Domain.TodoItems.Queries;

namespace Honamic.Todo.Query.Domain.TodoItems;

public interface ITodoQueryRepository
{
    Task<GetAllTodoItemsQueryResult> GetAsync(GetAllTodoItemsQueryFilter filter);

    Task<TodoItemQuery> GetAsync(long id, CancellationToken cancellationToken);

    Task<PagedQueryResult<GetAllTodoItemsQueryResult>> GetAllAsync(GetAllTodoItemsQueryFilter filter, CancellationToken cancellationToken);

}