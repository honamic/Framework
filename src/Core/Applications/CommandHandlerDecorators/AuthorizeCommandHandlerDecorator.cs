using Honamic.Framework.Applications.Authorizes;
using Honamic.Framework.Applications.Exceptions;
using Honamic.Framework.Commands;
using System.Reflection;

namespace Honamic.Framework.Applications.CommandHandlerDecorators;

public class AuthorizeCommandHandlerDecorator<TCommand> : ICommandHandler<TCommand>
    where TCommand : ICommand
{
    private readonly ICommandHandler<TCommand> _commandHandler;
    private readonly IAuthorization _authorization;

    public AuthorizeCommandHandlerDecorator(ICommandHandler<TCommand> commandHandler, IAuthorization authorization)
    {
        _commandHandler = commandHandler;
        _authorization = authorization;
    }

    public async Task HandleAsync(TCommand command, CancellationToken cancellationToken)
    {
        await _authorization.AuthorizeCommandAttributes(typeof(TCommand));

        await _commandHandler.HandleAsync(command, cancellationToken);
    }
}

public class AuthorizeCommandHandlerDecorator<TCommand, TResponse> : ICommandHandler<TCommand, TResponse>
    where TCommand : ICommand<TResponse>
{
    private readonly ICommandHandler<TCommand, TResponse> _commandHandler;
    private readonly IAuthorization _authorization;

    public AuthorizeCommandHandlerDecorator(ICommandHandler<TCommand, TResponse> commandHandler, IAuthorization authorization)
    {
        _commandHandler = commandHandler;
        _authorization = authorization;
    }

    public async Task<TResponse> HandleAsync(TCommand command, CancellationToken cancellationToken)
    {
        await _authorization.AuthorizeCommandAttributes(typeof(TCommand));

        return await _commandHandler.HandleAsync(command, cancellationToken);
    }
}

internal static class AuthorizeCommandHandlerDecoratorHelper
{

    public static async Task AuthorizeCommandAttributes(this IAuthorization authorization, Type type)
    {
        await authorization.AuthorizeWithAttributes(type);

        await authorization.AuthorizeWithDynamicPermissions(type);
    }

    public static async Task AuthorizeWithDynamicPermissions(this IAuthorization authorization, Type type)
    {
        var dynamicAuthorizeAttribute = type.GetCustomAttribute<DynamicAuthorizeAttribute>();

        if (dynamicAuthorizeAttribute is not null)
        {
            if (!authorization.IsAuthenticated())
            {
                throw new UnauthenticatedException();
            }

            string dynamicPermission = CalculatePermissionName(type);

            if (!await authorization.HaveAccessAsync(dynamicPermission))
            {
                throw new UnauthorizedException(dynamicPermission);
            }
        }
    }

    public static async Task AuthorizeWithAttributes(this IAuthorization authorization, Type type)
    {
        var authorizeAttribute = type.GetCustomAttribute<AuthorizeAttribute>();

        if (authorizeAttribute is not null)
        {
            if (!authorization.IsAuthenticated())
            {
                throw new UnauthenticatedException();
            }

            if (authorizeAttribute.Permissions?.Length > 0)
            {
                foreach (var permission in authorizeAttribute.Permissions)
                {
                    if (!await authorization.HaveAccessAsync(permission))
                    {
                        throw new UnauthorizedException(permission);
                    }
                }
            }
        }
    }

    private static string CalculatePermissionName(Type type)
    {
        return type.Name;
    }
}