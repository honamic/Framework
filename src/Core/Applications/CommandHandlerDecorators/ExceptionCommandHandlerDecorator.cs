using Honamic.Framework.Applications.Extensions;
using Honamic.Framework.Commands;

namespace Honamic.Framework.Applications.CommandHandlerDecorators;

public class ExceptionCommandHandlerDecorator<TCommand, TResponse> : ICommandHandler<TCommand, TResponse>
    where TCommand : ICommand<TResponse>
{
    private readonly ICommandHandler<TCommand, TResponse> _commandHandler;

    public ExceptionCommandHandlerDecorator(ICommandHandler<TCommand, TResponse> commandHandler)
    {
        _commandHandler = commandHandler;
    }

    public async Task<TResponse> HandleAsync(TCommand command, CancellationToken cancellationToken)
    {
        TResponse result;
        try
        {
            result = await _commandHandler.HandleAsync(command, cancellationToken);
        }
        catch (Exception ex)
        {
            if (ExceptionDecoratorHelper.IsResultOriented(typeof(TResponse)))
            {
                result = ExceptionDecoratorHelper.CreateResultWithError<TResponse>(typeof(TResponse), ex);
                return result;
            }

            throw;
        }

        return result;
    }
}