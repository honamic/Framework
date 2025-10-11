using Honamic.Framework.Application.Results;
using Honamic.Framework.Queries;
using TodoSample.Application.Contracts.Todos.Queries;

namespace TodoSample.QueryModels.Todos;

public interface ITodoQueryModelRepository
{
    Task<GetTodoQueryResult?> GetAsync(GetTodoQuery query, CancellationToken cancellationToken);
    Task<PagedQueryResult<GetAllTodosQueryResult>> GetAll(GetAllTodosQuery query, CancellationToken cancellationToken);
}
