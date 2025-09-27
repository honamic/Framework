using Honamic.Framework.Applications.Results;
using Honamic.Framework.EntityFramework.Query;
using Honamic.Framework.Queries;
using Honamic.Framework.Utilities.Extensions;
using Honamic.Todo.Query.Domain.TodoItems.Queries;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Honamic.IdentityPlus.Application.Users.QueryHandlers;
public class GetAllTodoItemsQueryHandler([FromKeyedServices(QueryConstants.QueryDbContextKey)] DbContext dbContext)
    : IQueryHandler<GetAllTodoItemsQueryFilter, Result<PagedQueryResult<GetAllTodoItemsQueryResult>>>
{
    public async Task<Result<PagedQueryResult<GetAllTodoItemsQueryResult>>>
        HandleAsync(GetAllTodoItemsQueryFilter filter, CancellationToken cancellationToken)
    {
        var query = dbContext.Set<Todo.Domain.TodoItems.TodoItem>().AsQueryable();

        if (filter.Keyword.HasValue())
        {
            query = query.Where(c =>
                 c.Content.Contains(filter.Keyword)
              || c.Tags.Contains(filter.Keyword)
              || c.Title.Contains(filter.Keyword)
            );
        }

        var result = await query.Select(x => new GetAllTodoItemsQueryResult()
        {
            Id = x.Id,
            Title = x.Title,
            Content = x.Content,
            Tags = x.Tags,
            Done = x.Done,
        }).ToPagedListAsync(filter, cancellationToken);

        // Uncomment the following lines if you want to return a Result object
        //return new Result<PagedQueryResult<GetAllTodoItemsQueryResult>>()
        //{
        //    Data = result,
        //    Status = ResultStatus.Ok
        //};

        return result;
    }
}