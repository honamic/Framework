using Honamic.Framework.Commands;
using Honamic.Framework.Domain;

namespace Honamic.Framework.Applications.CommandHandlerDecorators;

public class TransactionalCommandHandlerDecorator<TCommand> : ICommandHandler<TCommand>
    where TCommand : ICommand
{
    private readonly ICommandHandler<TCommand> _commandHandler;
    private readonly IUnitOfWork _unitOfWork;

    public TransactionalCommandHandlerDecorator(IUnitOfWork unitOfWork, ICommandHandler<TCommand> commandHandler)
    {
        _commandHandler = commandHandler;
        _unitOfWork = unitOfWork;
    }

    public async Task HandleAsync(TCommand command, CancellationToken cancellationToken)
    {
        try
        {
            await _commandHandler.HandleAsync(command, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (Exception)
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }
}

public class TransactionalCommandHandlerDecorator<TCommand, TResponse> : ICommandHandler<TCommand, TResponse>
    where TCommand : ICommand<TResponse>
{
    private readonly ICommandHandler<TCommand, TResponse> _commandHandler;
    private readonly IUnitOfWork _unitOfWork;

    public TransactionalCommandHandlerDecorator(IUnitOfWork unitOfWork, ICommandHandler<TCommand, TResponse> commandHandler)
    {
        _commandHandler = commandHandler;
        _unitOfWork = unitOfWork;
    }

    public async Task<TResponse> HandleAsync(TCommand command, CancellationToken cancellationToken)
    {
        TResponse result;
        try
        {

            result = await _commandHandler.HandleAsync(command, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (Exception)
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }

        return result;
    }
}