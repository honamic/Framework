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

    public Task<TResponse> DispatchAsync<TCommand, TResponse>(TCommand command, CancellationToken cancellationToken)
        where TCommand : ICommand<TResponse>
    {
        var handler = _serviceProvider.GetService<ICommandHandler<TCommand, TResponse>>();

        if (handler == null)
        {
            throw new InvalidOperationException($"No CommandHandler is registered for {typeof(ICommandHandler<TCommand, TResponse>).Name}");
        }

        return handler.HandleAsync(command, cancellationToken);
    }
}