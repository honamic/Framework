using Honamic.IdentityPlus.Domain;
using Honamic.IdentityPlus.Domain.Roles;
using Honamic.IdentityPlus.Domain.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Honamic.IdentityPlus.Application.Extensions;

public static class IdentityPlusApplicationServiceCollection
{
    public static IdentityBuilder? IdentityBuilder { get; private set; }
    public static IServiceCollection AddIdentityPlusApplication(this IServiceCollection services)
    {
        services.AddIdentityPlusApplication(_ => { });

        return services;
    }

    public static IServiceCollection AddIdentityPlusApplication(this IServiceCollection services,
        Action<IdentityPlusOptions> configure)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configure);

        services.AddScoped<UserManager<User>, IdentityPlusUserManager>();
        services.AddScoped<IdentityPlusUserManager>();
        
        services.AddScoped<RoleManager<Role>, IdentityPlusRoleManager>();
        services.AddScoped<IdentityPlusRoleManager>();

        IdentityBuilder = services.AddIdentityCore<User>((opt) =>
        {

        });

        return services;
    }
}
