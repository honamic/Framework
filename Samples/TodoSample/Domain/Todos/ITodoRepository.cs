namespace TodoSample.Domain.Todos;

public interface ITodoRepository
{
    Task<bool> ExistsByTitleAsync(string title, long? excludeId);
    Task<Todo> GetAsync(long id, CancellationToken cancellationToken);
    Task<Todo?> GetByIdAsync(long id, CancellationToken cancellationToken);
    Task InsertAsync(Todo todo, CancellationToken cancellationToken);
}