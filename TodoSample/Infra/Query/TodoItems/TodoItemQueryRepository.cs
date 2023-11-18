using Honamic.Framework.Queries;
using Honamic.Framework.Utilities.Extensions;
using Honamic.Todo.Query.Domain.TodoItems.Queries;
using Microsoft.EntityFrameworkCore;
using Honamic.Todo.Query.Domain.TodoItems;
using Honamic.Todo.Query.Domain.TodoItems.Models;

namespace Honamic.Todo.Query.EntityFramework.TodoItems;

internal class TodoItemQueryRepository : ITodoItemQueryRepository
{
    private readonly TodoQueryDbContext _TodoQueryDbContext;

    public TodoItemQueryRepository(TodoQueryDbContext TodoQueryDbContext)
    {
        _TodoQueryDbContext = TodoQueryDbContext;
    }

    public Task<GetAllTodoItemsQueryResult> GetAsync(GetAllTodoItemsQueryFilter filter)
    {
        throw new NotImplementedException();
    }

    public Task<TodoItemQuery> GetAsync(long id, CancellationToken cancellationToken)
    {
        return _TodoQueryDbContext.TodoItems.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public Task<PagedQueryResult<GetAllTodoItemsQueryResult>> GetAllAsync(GetAllTodoItemsQueryFilter filter, CancellationToken cancellationToken)
    {
        var query = _TodoQueryDbContext.TodoItems.AsQueryable();

        if (filter.Keyword.HasValue())
        {
            query = query.Where(c =>
                 c.Content.Contains(filter.Keyword)
              || c.Tags.Contains(filter.Keyword)
              || c.Title.Contains(filter.Keyword)
            );
        }


        return query.Select(x => new GetAllTodoItemsQueryResult()
        {
            Title = x.Title,
            Content = x.Content,
            Tags = x.Tags,
            Done = x.Done,
        }).ToPagedListAsync(filter, cancellationToken);
    }
}