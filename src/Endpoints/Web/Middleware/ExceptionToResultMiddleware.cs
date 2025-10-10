using Honamic.Framework.Application.Results;
using Honamic.Framework.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;

namespace Honamic.Framework.Endpoints.Web.Middleware;

internal class ExceptionToResultMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionToResultMiddleware> _logger;
    
    //todo: move to options
    private readonly bool _errorExceptionInResult;

    public ExceptionToResultMiddleware(RequestDelegate next,
        ILogger<ExceptionToResultMiddleware> logger,
        IConfiguration configuration)
    {
        _next = next;
        _logger = logger;
        _errorExceptionInResult = configuration.GetSection("General")
            .GetValue<bool?>("ErrorExceptionInResult") ?? false;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(httpContext, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        await RollBackTransaction(context);

        switch (exception)
        {
            case AuthenticationRequiredException:
            case ForbiddenException:
                break;
            case BusinessException:
                _logger.LogWarning(exception, Constants.UnhandledBusinessExceptionMessage);
                break;
            default:
                _logger.LogError(exception, Constants.UnhandledExceptionMessage);
                break;
        }

        Result errorResult = CreateErrorResult(exception);

        await ReturnApiResultMessage(context, errorResult);
    }

    private async Task RollBackTransaction(HttpContext context)
    {
        try
        {
            var unitOfWork = context.RequestServices.GetService<IUnitOfWork>();
            if (unitOfWork != null)
                await unitOfWork.RollbackTransactionAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, Constants.UnhandledExceptionMessage);
        }
    }

    private async Task ReturnApiResultMessage(HttpContext context, Result errorResult)
    {
        switch (errorResult.Status)
        {
            case ResultStatus.Undefined:
                break;
            case ResultStatus.Success:
                context.Response.StatusCode = (int)HttpStatusCode.OK;
                break;
            case ResultStatus.Forbidden:
                context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                break;
            case ResultStatus.AuthenticationRequired:
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                break;
            case ResultStatus.UnhandledException:
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                break;
            case ResultStatus.ValidationFailed:
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                break;
            case ResultStatus.DomainStateInvalid:
                context.Response.StatusCode = (int)HttpStatusCode.UnprocessableEntity;
                break;
            case ResultStatus.NotFound:
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                break;

            default:
                break;
        }

        var serializedResult = SerializeResult(errorResult);
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsync(serializedResult);
    }

    private Result CreateErrorResult(Exception exception)
    {
        var errorResult = new Result();

        if (exception is BusinessException businessException)
        {
            errorResult.Status = ResultStatus.ValidationFailed;
            var code = businessException.GetCode();
            var message = businessException.GetMessage();
            errorResult.AppendErrormessage(message, null, code);
        }
        else if (exception is ForbiddenException unauthorizedException)
        {
            errorResult.Status = ResultStatus.Forbidden;
            errorResult.AppendError(unauthorizedException.Message);
        }
        else if (exception is AuthenticationRequiredException unauthenticatedException)
        {
            errorResult.Status = ResultStatus.AuthenticationRequired;
            errorResult.AppendError(unauthenticatedException.Message);
        }
        else
        {
            errorResult.SetStatusAsUnhandledException(Constants.UnhandledExceptionMessage);
        }

        if (_errorExceptionInResult)
        {
            errorResult.AppendError(exception.ToString(), "exception");
        }

        return errorResult;
    }

    private static string SerializeResult(Result result)
    {
        return JsonSerializer.Serialize(result, result.GetType(), new JsonSerializerOptions
        {
            WriteIndented = false,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        });
    }
}
