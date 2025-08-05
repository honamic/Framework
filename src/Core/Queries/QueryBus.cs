using Microsoft.Extensions.DependencyInjection;

namespace Honamic.Framework.Queries;

internal class QueryBus : IQueryBus
{
    private readonly IServiceProvider _serviceProvider;

    public QueryBus(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task<TQueryResult> Dispatch<TQueryFilter, TQueryResult>(TQueryFilter filter, CancellationToken cancellationToken)
        where TQueryFilter : IQueryFilter
      //  where TQueryResult : IQueryResult
    {
        var handler = _serviceProvider.GetService<IQueryHandler<TQueryFilter, TQueryResult>>();

        if (handler == null)
        {
            throw new InvalidOperationException($"No QueryHandler is registered for {typeof(TQueryFilter).Name}");
        }

        var result = await handler.HandleAsync(filter,cancellationToken);
        return result;
    }

}