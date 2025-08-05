namespace Honamic.Framework.Queries;

public interface IQueryBus
{
    Task<TQueryResult> Dispatch<TQueryFilter, TQueryResult>(TQueryFilter filter, CancellationToken cancellationToken)
        where TQueryFilter : IQueryFilter;
    // where TQueryResult : IQueryResult;


}