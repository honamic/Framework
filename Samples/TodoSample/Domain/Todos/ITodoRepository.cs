namespace TodoSample.Domain.Todos;

public interface ITodoRepository
{
    Task<bool> ExistsByTitleAsync(string title, long? excludeId);
    Task InsertAsync(Todo todo, CancellationToken cancellationToken);
}