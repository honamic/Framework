using Microsoft.Extensions.DependencyInjection;

namespace Honamic.Framework.Queries.Extensions;

public static class QueriesServiceCollectionExtensions
{
    public static void AddDefaultQueriesServices(this IServiceCollection services)
    {
        services.AddScoped<IQueryBus, QueryBus>();
    }
}