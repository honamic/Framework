using Honamic.Framework.Application.Extensions;
using Honamic.Framework.Queries;

namespace Honamic.Framework.Application.QueryHandlerDecorators;

public class ExceptionQueryHandlerDecorator<TQuery, TResponse> : IQueryHandler<TQuery, TResponse>
    where TQuery : class, IQuery<TResponse>
{
    private readonly IQueryHandler<TQuery, TResponse> _queryHandler;

    public ExceptionQueryHandlerDecorator(IQueryHandler<TQuery, TResponse> queryHandler)
    {
        _queryHandler = queryHandler;
    }

    public async Task<TResponse> HandleAsync(TQuery query, CancellationToken cancellationToken)
    {
        TResponse result;
        try
        {
            result = await _queryHandler.HandleAsync(query, cancellationToken);
        }
        catch (Exception ex)
        {
            if (ExceptionDecoratorHelper.IsResultOriented(typeof(TResponse)))
            {
                result = ExceptionDecoratorHelper.CreateResultWithError<TResponse>(typeof(TResponse), ex);
                return result;
            }

            throw;
        }

        return result;
    }
}
