using Microsoft.Extensions.DependencyInjection;

namespace Honamic.Framework.Queries.Extensions;

public static class QueriesServiceCollectionExtensions
{
    public static void AddDefaultQueriesServices(this IServiceCollection services)
    {
        services.AddScoped<IQueryBus, QueryBus>();
    }

    public static void AddQueryHandler<TQueryFilter, TQueryResult, TQueryHandler>(this IServiceCollection services)
    where TQueryFilter : IQueryFilter
    where TQueryResult : IQueryResult
    where TQueryHandler : class, IQueryHandler<TQueryFilter, TQueryResult>
    {
        services.AddTransient<IQueryHandler<TQueryFilter, TQueryResult>, TQueryHandler>();
    }
}