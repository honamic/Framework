using Honamic.Framework.Applications.Authorizes;
using Honamic.Framework.Applications.Extensions;
using Honamic.Framework.Queries;

namespace Honamic.Framework.Applications.QueryHandlerDecorators;

public class ExceptionQueryHandlerDecorator<TQueryFilter, TQueryResult> : IQueryHandler<TQueryFilter, TQueryResult>
        where TQueryFilter : IQueryFilter
       // where TQueryResult : IQueryResult
{
    private readonly IQueryHandler<TQueryFilter, TQueryResult> _queryHandler;
    private readonly IAuthorization _authorization;

    public ExceptionQueryHandlerDecorator(IQueryHandler<TQueryFilter, TQueryResult> queryHandler, IAuthorization authorization)
    {
        _queryHandler = queryHandler;
        _authorization = authorization;
    }

    public async Task<TQueryResult> HandleAsync(TQueryFilter query, CancellationToken cancellationToken)
    {
        TQueryResult result;
        try
        {
            result = await _queryHandler.HandleAsync(query, cancellationToken);
        }
        catch (Exception ex)
        {
            if (ExceptionDecoratorHelper.IsResultOriented(typeof(TQueryResult)))
            {
                result = ExceptionDecoratorHelper.CreateResultWithError<TQueryResult>(typeof(TQueryResult), ex);
                return result;
            }

            throw;
        }

        return result;
    }
}
