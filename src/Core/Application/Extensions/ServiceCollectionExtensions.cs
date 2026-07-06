using Honamic.Framework.Application.Authorizes;
using Honamic.Framework.Commands.Extensions;
using Honamic.Framework.Domain.Extensions;
using Honamic.Framework.Events.Extensions;
using Honamic.Framework.Queries.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Honamic.Framework.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDefaultApplicationsServices(this IServiceCollection services)
    {
        services.AddDefaultDomainServices();
        services.AddDefaultEventsServices();
        services.AddDefaultCommandsServices();
        services.AddDefaultQueriesServices();
        services.AddScopeValueProviders();

        return services;
    }

    public static IServiceCollection AddScopeValueProviders(this IServiceCollection services)
    {
        services.AddScoped<IScopeValueProviderRegistry, ScopeValueProviderRegistry>();

        return services;
    }
}