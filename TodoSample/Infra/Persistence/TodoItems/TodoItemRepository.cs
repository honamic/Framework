using System.Linq.Expressions;
using Honamic.Framework.Persistence.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Honamic.Todo.Domain.TodoItems;

namespace Honamic.Todo.Persistence.EntityFramework.TodoItems;

internal class TodoItemRepository : RepositoryBase<TodoItem, long>, ITodoItemRepository
{
    public TodoItemRepository(DbContext context) : base(context)
    {

    }

    protected override IList<Expression<Func<TodoItem, object?>>> GetIncludes()
    {
        return new List<Expression<Func<TodoItem, object?>>>
        {
            c=>c.Logs
        };
    }
}