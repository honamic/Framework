using Honamic.Framework.Persistence.EntityFramework;
using Honamic.Todo.Domain.TodoItems;
using System.Linq.Expressions;

namespace Honamic.Todo.Persistence.EntityFramework.TodoItems;

internal class TodoItemRepository : RepositoryBase<TodoItem, long>, ITodoItemRepository
{
    public TodoItemRepository(TodoDbContext context) : base(context)
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