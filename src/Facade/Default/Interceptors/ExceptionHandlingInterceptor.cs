using Castle.DynamicProxy;
using Honamic.Framework.Applications.Exceptions;
using Honamic.Framework.Applications.Results;
using Honamic.Framework.Domain;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Reflection;

namespace Honamic.Framework.Facade.Interceptors;

internal class ExceptionHandlingInterceptor : IInterceptor
{
    private readonly ILogger<ExceptionHandlingInterceptor> _logger;

    public ExceptionHandlingInterceptor(ILogger<ExceptionHandlingInterceptor> logger)
    {
        _logger = logger;
    }
    public void Intercept(IInvocation invocation)
    {
        _logger.LogTrace($"Calling method {invocation.TargetType}.{invocation.Method.Name}.");

        try
        {
            invocation.Proceed(); // continue

            if (invocation.ReturnValue is Task task)
            {
                if (!task.IsCompleted)
                {
                    task.GetAwaiter().GetResult();
                }
            }

        }
        catch (Exception ex)
        {
            if (invocation.Proxy is IBaseFacade baseFacade)
            {
                baseFacade.Logger.LogError(ex, "Unhandled exception");
            }

            var result = GetNewResult(invocation, ex);

            if (result != null)
            {
                invocation.ReturnValue = result;
            }

        }

        _logger.LogTrace($"Finished method {invocation.TargetType}.{invocation.Method.Name}.");
    }

    private object? GetNewResult(IInvocation invocation, Exception ex)
    {
        var ReturnValueType = invocation.Method.ReturnType;
        Result? rawResult = null;
        object? result = null;

        if (ReturnValueType == typeof(Result))
        {
            rawResult = new Result();
            result = rawResult;
        }
        else if (ReturnValueType.IsGenericType &&
            ReturnValueType.BaseType == typeof(Result))
        {
            var type = typeof(Result<>).MakeGenericType(ReturnValueType.GenericTypeArguments[0]);

            result = Activator.CreateInstance(type);
            rawResult = (Result)result;

        }
        else if (ReturnValueType.IsGenericType &&
            ReturnValueType.GetGenericTypeDefinition() == typeof(Task<>))
        {
            if (ReturnValueType.GenericTypeArguments[0] == typeof(Result))
            {
                rawResult = new Result(ResultStatus.UnhandledException);
                result = Task.FromResult(rawResult);
            }

            else if (ReturnValueType.GenericTypeArguments[0].GetGenericTypeDefinition() == typeof(Result<>))
            {
                var genericResult = ReturnValueType.GenericTypeArguments[0];
                var type = typeof(Result<>).MakeGenericType(genericResult.GenericTypeArguments[0]);
                result = Activator.CreateInstance(type);
                rawResult = (Result)result;

                // this is not working => Unable to cast object of type ...
                // result = Task.FromResult(rawResult);
                if (invocation.ReturnValue is null)
                {

                }
                else
                {
                    result = SetResultToCurrentTaskByReflection(invocation.ReturnValue, rawResult);
                }
            }
            else
            {
                Debugger.Break();
            }
        }
        else
        {
            Debugger.Break();
        }

        switch (ex)
        {
            case AuthenticationRequiredException:
                rawResult?.SetStatusAsAuthenticationRequired(ex.Message);
                break;
            case ForbiddenException:
                rawResult?.SetStatusAsForbidden(ex.Message);
                break;
            case NotFoundBusinessException notFoundEx:
                rawResult?.SetStatusAsNotFound();
                rawResult?.AppendErrormessage(notFoundEx.GetMessage(), null, notFoundEx.GetCode());
                break;
            case BusinessException businessException:
                rawResult?.SetStatusAsValidationFailed();
                var code = businessException.GetCode();
                var message = businessException.GetMessage();
                rawResult?.AppendErrormessage(message, null, code);
                break;
            default:
                rawResult?.SetStatusAsUnhandledExceptionWithSorryError();
                rawResult?.AppendError(ex.Message, "Exception");
                break;
        }

        return result;
    }

    private static object? SetResultToCurrentTaskByReflection(object? returnValue, Result? rawResult)
    {
        var task = returnValue as Task;
        if (task == null)
        {
            return null;
        }

        try
        {
            var taskStatusProp = task.GetType()
                .GetField("m_stateFlags", BindingFlags.Instance | BindingFlags.NonPublic);

            var taskResultProperty = task.GetType()
                .GetField("m_result", BindingFlags.Instance | BindingFlags.NonPublic);

            if (taskStatusProp is null || taskResultProperty is null)
            {
                return null;
            }

            // set status to RanToCompletion
            taskStatusProp.SetValue(task, 0x1000000);

            //set result of Generic task
            taskResultProperty.SetValue(task, rawResult);
            return task;
        }
        catch (Exception ex)
        {
            Debugger.Break();
            return null;
        }
    }
}