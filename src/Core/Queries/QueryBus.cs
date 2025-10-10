namespace Honamic.Framework.Queries;

internal class QueryBus : IQueryBus
{
    private readonly IServiceProvider _serviceProvider;

    public QueryBus(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public Task<TResponse> DispatchAsync<TResponse>(IQuery<TResponse> query, CancellationToken cancellationToken)
    {
        var queryType = query.GetType();
        var handlerType = typeof(IQueryHandler<,>).MakeGenericType(queryType, typeof(TResponse));
        var handler = _serviceProvider.GetService(handlerType);

        if (handler == null)
        {
            throw new InvalidOperationException($"No QueryHandler is registered for {queryType.Name}");
        }

        return (Task<TResponse>)handlerType
            .GetMethod("HandleAsync")!
            .Invoke(handler, new object[] { query, cancellationToken })!;
    }

    public Task<TResponse> Dispatch<TResponse>(IQuery<TResponse> query, CancellationToken cancellationToken)
    {
        return DispatchAsync(query, cancellationToken);
    }
}