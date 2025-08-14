using Honamic.Framework.Applications.Extensions;
using Honamic.Framework.Domain;
using Honamic.Framework.Queries;

namespace Honamic.Framework.Applications.QueryHandlerDecorators;

public class AuthorizeQueryHandlerDecorator<TQuery, TResponse> : IQueryHandler<TQuery, TResponse>
    where TQuery : class, IQuery<TResponse>
{
    private readonly IQueryHandler<TQuery, TResponse> _queryHandler;
    private readonly IAuthorization _authorization;

    public AuthorizeQueryHandlerDecorator(IQueryHandler<TQuery, TResponse> queryHandler, IAuthorization authorization)
    {
        _queryHandler = queryHandler;
        _authorization = authorization;
    }

    public async Task<TResponse> HandleAsync(TQuery query, CancellationToken cancellationToken)
    {
        await _authorization.AuthorizeAttributes(typeof(TQuery));
        return await _queryHandler.HandleAsync(query, cancellationToken);
    }
}