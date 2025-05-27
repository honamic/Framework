using Honamic.Framework.Applications.Exceptions;
using Honamic.Framework.Applications.Results;
using Honamic.Framework.Commands;
using Honamic.Framework.Domain;

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
            if (IsResultOriented(typeof(TResponse)))
            {
                result = CreateResultWithError(typeof(TResponse), ex);
                return result;
            }

            throw;
        }

        return result;
    }

    private TResponse CreateResultWithError(Type type, Exception ex)
    {
        var resultObject = CreateResult(type);

        if (resultObject is Result result)
        {
            switch (ex)
            {
                case UnauthenticatedException:
                    result.SetStatusAsUnauthenticated();
                    result.AppendError(ex.Message);
                    break;
                case UnauthorizedException:
                    result.SetStatusAsUnauthorized();
                    result.AppendError(ex.Message);
                    break;
                case NotFoundBusinessException notFoundEx:
                    result.Status = ResultStatus.NotFound;
                    result.AppendError(notFoundEx.GetMessage(), null, notFoundEx.GetCode());
                    break;
                case BusinessException businessException:
                    result.Status = ResultStatus.ValidationError;
                    var code = businessException.GetCode();
                    var message = businessException.GetMessage();
                    result.AppendError(message, null, code);
                    break;
                default:
                    result.SetStatusAsUnhandledExceptionWithSorryError();
                    result.AppendError(ex.ToString(), "Exception");
                    break;
            }
            return resultObject;
        }

        // If we can't cast to Result, we have a serious error
        throw new ArgumentException($"Expected a Result type but got {type.FullName}");
    }

    private TResponse CreateResult(Type type)
    {
        // For non-generic Result
        if (type == typeof(Result))
        {
            return (TResponse)(object)new Result();
        }

        // For Result<T>
        if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Result<>))
        {
            var genericArgType = type.GenericTypeArguments[0];
            var resultType = typeof(Result<>).MakeGenericType(genericArgType);
            return (TResponse)Activator.CreateInstance(resultType);
        }

        return default;
    }

    private bool IsResultOriented(Type type)
    {
        if (type == typeof(Result))
        {
            return true;
        }

        if (type.IsGenericType
            && type.GetGenericTypeDefinition() == typeof(Result<>))
        {
            return true;
        }

        return false;
    }
}