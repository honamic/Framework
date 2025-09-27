using Honamic.Framework.Queries;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Honamic.Framework.EntityFramework.Query.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDefaultQueryDbContext<TDbContext>(this IServiceCollection serviceCollection)
        where TDbContext : QueryDbContext‌Base
    {

        serviceCollection.AddKeyedScoped<DbContext>(QueryConstants.QueryDbContextKey, (sp, key) => sp.GetRequiredService<TDbContext>());

        return serviceCollection;
    }
}
