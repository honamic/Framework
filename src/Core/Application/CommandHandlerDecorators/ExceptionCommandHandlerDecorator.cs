using Honamic.Framework.Application.Extensions;
using Honamic.Framework.Commands;
using Microsoft.Extensions.Logging;

namespace Honamic.Framework.Application.CommandHandlerDecorators;

public class ExceptionCommandHandlerDecorator<TCommand, TResponse> : ICommandHandler<TCommand, TResponse>
    where TCommand : ICommand<TResponse>
{
    private readonly ICommandHandler<TCommand, TResponse> _commandHandler;
    private readonly ILogger<ExceptionCommandHandlerDecorator<TCommand, TResponse>> _logger;

    public ExceptionCommandHandlerDecorator(
        ICommandHandler<TCommand, TResponse> commandHandler,
        ILogger<ExceptionCommandHandlerDecorator<TCommand, TResponse>> logger)
    {
        _commandHandler = commandHandler;
        _logger = logger;
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
                if (ExceptionDecoratorHelper.IsNonBusinessException(ex))
                {
                    _logger.LogError(ex, "An unhandled exception has been occurred.");
                }

                result = ExceptionDecoratorHelper.CreateResultWithError<TResponse>(typeof(TResponse), ex);
                return result;
            }

            throw;
        }

        return result;
    }
}