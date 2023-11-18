namespace Honamic.Framework.Queries;

public interface IQueryHandler<in TQueryFilter, TQueryResult>
    where TQueryFilter : IQueryFilter
    where TQueryResult : IQueryResult
{
    Task<TQueryResult> HandleAsync(TQueryFilter filter, CancellationToken cancellationToken);

}