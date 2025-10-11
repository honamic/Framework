using Honamic.Framework.Application.Results;
using Honamic.Framework.EntityFramework.QueryModels;
using Honamic.Framework.Queries;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TodoSample.Application.Contracts.Todos.Queries;
using TodoSample.QueryModels.Todos;

namespace TodoSample.QueryModels.EntityFramework.Todos;

public class TodoQueryModelRepository : ITodoQueryModelRepository
{
    private readonly DbContext _context;

    public TodoQueryModelRepository([FromKeyedServices(QueryConstants.QueryDbContextKey)] DbContext context)
    {
        _context = context;
    }

    public Task<GetTodoQueryResult?> GetAsync(GetTodoQuery query, CancellationToken cancellationToken)
    {
        return _context.Set<TodoQueryModel>()
                    .Select(c => new GetTodoQueryResult
                    {
                        Id = c.Id,
                        Title = c.Title,
                        Description = c.Description,
                    })
                    .FirstOrDefaultAsync(c=>c.Id== query.Id, cancellationToken);
    }

    public Task<PagedQueryResult<GetAllTodosQueryResult>> GetAll(GetAllTodosQuery query, CancellationToken cancellationToken)
    {
        return _context.Set<TodoQueryModel>()
             .Select(c => new GetAllTodosQueryResult
             {
                 Id = c.Id,
                 Title = c.Title,
                 Description = c.Description,
             })
             .ToFilteredPagedListAsync(query, cancellationToken);
    }
}
