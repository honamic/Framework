using Honamic.IdentityPlus.Domain.Roles;
using Honamic.IdentityPlus.Domain.Users;
using Honamic.IdentityPlus.Persistence.Roles;
using Honamic.IdentityPlus.Persistence.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Honamic.IdentityPlus.Persistence.Extensions;

public static class SeviceCollectionExtensions
{

    public static IServiceCollection AddIdentityPlusPersistence(this IServiceCollection services)
    {
        services.TryAddScoped<IUserStore<User>,UserRepository>();
        services.TryAddScoped<IRoleStore<Role>,RoleRepository>();

        return services;
    }

}