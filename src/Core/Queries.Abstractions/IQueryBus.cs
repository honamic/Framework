namespace Honamic.Framework.Queries;

public interface IQueryBus
{
     Task<TResponse> Dispatch<TResponse>(IQuery<TResponse> query, CancellationToken cancellationToken);
 }