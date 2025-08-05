using Honamic.Framework.Applications.Authorizes;
using Honamic.Framework.Applications.Extensions;
using Honamic.Framework.Queries;

namespace Honamic.Framework.Applications.QueryHandlerDecorators;

public class AuthorizeQueryHandlerDecorator<TQueryFilter, TQueryResult> : IQueryHandler<TQueryFilter, TQueryResult>
        where TQueryFilter : IQueryFilter
        //where TQueryResult : IQueryResult
{
    private readonly IQueryHandler<TQueryFilter, TQueryResult> _queryHandler;
    private readonly IAuthorization _authorization;

    public AuthorizeQueryHandlerDecorator(IQueryHandler<TQueryFilter, TQueryResult> queryHandler, IAuthorization authorization)
    {
        _queryHandler = queryHandler;
        _authorization = authorization;
    }

    public async Task<TQueryResult> HandleAsync(TQueryFilter query, CancellationToken cancellationToken)
    {
        await _authorization.AuthorizeAttributes(typeof(TQueryFilter));
        return await _queryHandler.HandleAsync(query, cancellationToken);
    }
}
