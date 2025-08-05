namespace Honamic.Framework.Queries;

internal class QueryBus : IQueryBus
{
    private readonly IServiceProvider _serviceProvider;

    public QueryBus(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task<TResponse> Dispatch<TResponse>(IQuery<TResponse> query, CancellationToken cancellationToken)
    {
        var queryType = query.GetType();
        var handlerType = typeof(IQueryHandler<,>).MakeGenericType(queryType, typeof(TResponse));
        var handler = _serviceProvider.GetService(handlerType);

        if (handler == null)
        {
            throw new InvalidOperationException($"No QueryHandler is registered for {queryType.Name}");
        }

        var method = handlerType.GetMethod("HandleAsync");
        if (method == null)
            throw new InvalidOperationException("HandleAsync method not found on handler.");

        var task = (Task)method.Invoke(handler, new object[] { query, cancellationToken })!;
        await task.ConfigureAwait(false);


        // گرفتن نتیجه از Task<T>
        var resultProperty = task.GetType().GetProperty("Result");

        if (resultProperty == null)
            throw new InvalidOperationException("Result property not found on Task.");

        return (TResponse)resultProperty.GetValue(task)!;
    }
}