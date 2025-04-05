using System.Linq.Expressions;

namespace Honamic.Todo.Domain.TodoItems;

public interface ITodoItemRepository
{
    Task<IList<TodoItem>> GetListAsync(Expression<Func<TodoItem, bool>> predicate);

    Task InsertAsync(TodoItem entity);

    Task Update(TodoItem entity);

    Task<TodoItem> GetAsync(long id);

    Task<IList<TodoItem>> GetAllAsync();

    Task<TodoItem?> GetAsync(Expression<Func<TodoItem, bool>> predicate);

    void Remove(TodoItem message);
}