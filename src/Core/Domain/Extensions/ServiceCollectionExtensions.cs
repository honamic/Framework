using Microsoft.Extensions.DependencyInjection;
using Honamic.Framework.Domain.Defaults;
using Honamic.Framework.Domain;

namespace Honamic.Framework.Applications.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDefaultDomainsServices(this IServiceCollection services)
    {
        services.AddSystemClock();
        return services;
    }

    public static IServiceCollection AddSystemClock(this IServiceCollection services)
    {
        services.AddScoped<IClock, SystemClock>();

        return services;

    }
}