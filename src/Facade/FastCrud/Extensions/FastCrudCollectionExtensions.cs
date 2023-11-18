using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Honamic.Framework.Facade.FastCrud.Mapping;

namespace Honamic.Framework.Facade.FastCrud.Extensions;

public static class FastCrudCollectionExtensions
{
    public static void AddFastCrudFacades(this IServiceCollection services, params Assembly[] scanAssemblies)
    {
        services.AddAutoMapper((config) =>
        {
            config.AddFastCrudMappings(scanAssemblies);
            config.ShouldMapMethod = (info) => false;
        }, scanAssemblies);

        services.Scan(scan =>
          //scan.FromCallingAssembly()
          scan
              .FromAssemblies(scanAssemblies)
              .AddClasses()
              .AsMatchingInterface()
              .WithScopedLifetime());
    }

    public static IServiceCollection AddCrudDbContext<TDbContext>(this IServiceCollection services)
        where TDbContext : FastCrudDbContext
    {
        return services.AddScoped<FastCrudDbContext, TDbContext>();
    }
}