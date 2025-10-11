using Honamic.Framework.Application.Results;
using Honamic.Framework.Queries;
using TodoSample.Application.Contracts.Todos.Queries;
using TodoSample.QueryModels.Todos;

namespace TodoSample.Application.Todos.QueryHandlers;

public class GetTodoQueryHandler : IQueryHandler<GetTodoQuery, Result<GetTodoQueryResult?>>
{
    private readonly ITodoQueryModelRepository _todoQueryModelRepository;

    public GetTodoQueryHandler(ITodoQueryModelRepository todoQueryModelRepository)
    {
        _todoQueryModelRepository = todoQueryModelRepository;
    }

    public async Task<Result<GetTodoQueryResult?>> HandleAsync(GetTodoQuery query, CancellationToken cancellationToken)
    {
        return await _todoQueryModelRepository.GetAsync(query, cancellationToken);
    }
}