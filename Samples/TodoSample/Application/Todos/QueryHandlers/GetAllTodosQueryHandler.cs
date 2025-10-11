using Honamic.Framework.Application.Results;
using Honamic.Framework.Queries;
using TodoSample.Application.Contracts.Todos.Queries;
using TodoSample.QueryModels.Todos;

namespace TodoSample.Application.Todos.QueryHandlers;

public class GetAllTodosQueryHandler : IQueryHandler<GetAllTodosQuery, Result<PagedQueryResult<GetAllTodosQueryResult>>>
{
    private readonly ITodoQueryModelRepository _todoQueryModelRepository;

    public GetAllTodosQueryHandler(ITodoQueryModelRepository todoQueryModelRepository)
    {
        _todoQueryModelRepository = todoQueryModelRepository;
    }

    public async Task<Result<PagedQueryResult<GetAllTodosQueryResult>>> HandleAsync(GetAllTodosQuery query, CancellationToken cancellationToken)
    {
       return await  _todoQueryModelRepository.GetAll(query, cancellationToken);
    }
}