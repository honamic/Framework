using Honamic.Framework.Application.Extensions;
using Honamic.Framework.Commands;
using Honamic.Framework.Domain;

namespace Honamic.Framework.Application.CommandHandlerDecorators;

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
        await _authorization.AuthorizeAttributes(typeof(TCommand));

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
        await _authorization.AuthorizeAttributes(typeof(TCommand));

        return await _commandHandler.HandleAsync(command, cancellationToken);
    }
}
