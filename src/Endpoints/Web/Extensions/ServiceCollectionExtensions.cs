using Honamic.Framework.Domain;
using Honamic.Framework.Endpoints.Web.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace Honamic.Framework.Endpoints.Web.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDefaultUserContextService(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddScoped<IUserContext, DefaultUserContext>();

        return services;
    }
    public static IServiceCollection AddDefaultUserContextService<TUserContext>(this IServiceCollection services)
         where TUserContext : class, IUserContext
    {
        services.AddHttpContextAccessor();
        services.AddScoped<IUserContext, TUserContext>();

        return services;
    }
}