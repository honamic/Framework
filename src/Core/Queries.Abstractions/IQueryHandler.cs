namespace Honamic.Framework.Queries;

public interface IQueryHandler<in TQuery, TResponse>
    where TQuery : class, IQuery<TResponse>
{
    Task<TResponse> HandleAsync(TQuery filter, CancellationToken cancellationToken);
}