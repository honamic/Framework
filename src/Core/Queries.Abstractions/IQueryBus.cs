namespace Honamic.Framework.Queries;

public interface IQueryBus
{
    [Obsolete("Use DispatchAsync instead.", true)]
    Task<TResponse> Dispatch<TResponse>(IQuery<TResponse> query, CancellationToken cancellationToken);
    Task<TResponse> DispatchAsync<TResponse>(IQuery<TResponse> query, CancellationToken cancellationToken);
}