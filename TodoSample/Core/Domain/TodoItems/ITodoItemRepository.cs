using System.Linq.Expressions;

namespace Honamic.Todo.Domain.TodoItems;

public interface ITodoItemRepository
{
    Task<IList<TodoItem>> GetListAsync(Expression<Func<TodoItem, bool>> predicate, CancellationToken cancellationToken = default);

    Task InsertAsync(TodoItem entity, CancellationToken cancellationToken = default);

    void Update(TodoItem entity);

    Task<TodoItem> GetAsync(long id, CancellationToken cancellationToken = default);

    Task<IList<TodoItem>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<TodoItem?> GetAsync(Expression<Func<TodoItem, bool>> predicate, CancellationToken cancellationToken = default);

    void Remove(TodoItem message);
}