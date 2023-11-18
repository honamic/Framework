
using Honamic.Framework.EntityFramework.Query;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Hasti.Framework.Queries.EntityFramework.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddQueryDbContext<TContext>(this IServiceCollection serviceCollection, Action<DbContextOptionsBuilder>? optionsAction = null)
        where TContext : QueryDbContext‌Base
    {
        serviceCollection.AddDbContext<TContext>(optionsAction);

        return serviceCollection;
    }
}
