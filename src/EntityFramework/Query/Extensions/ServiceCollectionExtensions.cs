using Honamic.Framework.EntityFramework.Query;
using Honamic.Framework.Queries;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Hasti.Framework.Queries.EntityFramework.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDefaultQueryDbContext<TDbContext>(this IServiceCollection serviceCollection)
        where TDbContext : QueryDbContext‌Base
    {

        serviceCollection.AddKeyedScoped<DbContext>(QueryConstants.QueryDbContextKey, (sp, key) => sp.GetRequiredService<TDbContext>());

        return serviceCollection;
    }
}
