using Microsoft.Extensions.DependencyInjection;

namespace Honamic.Framework.Domain.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDefaultDomainServices(this IServiceCollection services)
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