using Microsoft.Extensions.DependencyInjection;

namespace Honamic.Framework.Commands;

public class CommandBus : ICommandBus
{
    private readonly IServiceProvider _serviceProvider;
    public CommandBus(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public Task DispatchAsync<TCommand>(TCommand command, CancellationToken cancellationToken)
        where TCommand : ICommand
    {
        var handler = _serviceProvider.GetService<ICommandHandler<TCommand>>();

        if (handler == null)
        {
            throw new InvalidOperationException($"No CommandHandler is registered for {typeof(TCommand).Name}");
        }

        return handler.HandleAsync(command, cancellationToken);
    }

    public Task<TResponse> DispatchAsync<TResponse>(ICommand<TResponse> command, CancellationToken cancellationToken)
    {
        var commandType = command.GetType();
        var handlerType = typeof(ICommandHandler<,>).MakeGenericType(commandType, typeof(TResponse));
        var handler = _serviceProvider.GetService(handlerType);

        if (handler == null)
        {
            throw new InvalidOperationException($"No CommandHandler is registered for {command.GetType().Name}");
        }

        return ((Task<TResponse>)handlerType
            .GetMethod("HandleAsync")!
            .Invoke(handler, new object[] { command, cancellationToken })!);
    }
}