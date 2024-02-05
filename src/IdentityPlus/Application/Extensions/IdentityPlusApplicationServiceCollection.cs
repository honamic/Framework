using Honamic.IdentityPlus.Application.Users.CommandHandlers;
using Honamic.IdentityPlus.Application.Users.Commands;
using Honamic.IdentityPlus.Domain.Roles;
using Honamic.IdentityPlus.Domain.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Honamic.Framework.Applications.Extensions;
using Honamic.Framework.Queries.Extensions;
using Honamic.IdentityPlus.Application.Users.QueryHandlers;
using Honamic.IdentityPlus.Application.Users.Queries;
using Honamic.Framework.Queries;

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

        services.AddCommandHandlers();
        services.AddQueryHandlers();
        services.AddEventHandlers();

        return services;
    }

    private static void AddCommandHandlers(this IServiceCollection services)
    {
        services.AddCommandHandler<LoginCommand, LoginCommandHandler, LoginCommandResult>();
        services.AddCommandHandler<LogoutCommand, LogoutCommandHandler>();
        services.AddCommandHandler<RefreshTokenCommand, RefreshTokenCommandHandler, object>();
    }

    private static void AddQueryHandlers(this IServiceCollection services)
    {
        services.AddQueryHandler<GetAllUsersQueryFilter, PagedQueryResult<GetAllUsersQueryResult>, GetAllUsersQueryHandler>();
    }

    private static void AddEventHandlers(this IServiceCollection services)
    {
        //services.AddEventHandler<UserCreatedEvent, UserCreatedEventHandler>();
    }

}