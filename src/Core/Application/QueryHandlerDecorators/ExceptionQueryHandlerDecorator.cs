using Honamic.Framework.Application.Extensions;
using Honamic.Framework.Queries;
using Microsoft.Extensions.Logging;

namespace Honamic.Framework.Application.QueryHandlerDecorators;

public class ExceptionQueryHandlerDecorator<TQuery, TResponse> : IQueryHandler<TQuery, TResponse>
    where TQuery : class, IQuery<TResponse>
{
    private readonly IQueryHandler<TQuery, TResponse> _queryHandler;
    private readonly ILogger<ExceptionQueryHandlerDecorator<TQuery, TResponse>> _logger;

    public ExceptionQueryHandlerDecorator(
        IQueryHandler<TQuery, TResponse> queryHandler,
        ILogger<ExceptionQueryHandlerDecorator<TQuery, TResponse>> logger)
    {
        _queryHandler = queryHandler;
        _logger = logger;
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
                if (ExceptionDecoratorHelper.IsNonBusinessException(ex))
                {
                    _logger.LogError(ex, "An unhandled exception has been occurred.");
                }

                result = ExceptionDecoratorHelper.CreateResultWithError<TResponse>(typeof(TResponse), ex);
                return result;
            }

            throw;
        }

        return result;
    }
}
