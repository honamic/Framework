using Honamic.Framework.Applications.Exceptions;
using Honamic.Framework.Applications.Results;
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
            case UnauthenticatedException:
            case UnauthorizedException:
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
            case ResultStatus.None:
                break;
            case ResultStatus.Success:
                context.Response.StatusCode = (int)HttpStatusCode.OK;
                break;
            case ResultStatus.Unauthorized:
                context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                break;
            case ResultStatus.Unauthenticated:
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                break;
            case ResultStatus.UnhandledException:
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                break;
            case ResultStatus.ValidationError:
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                break;
            case ResultStatus.InvalidDomainState:
                context.Response.StatusCode = (int)HttpStatusCode.UnprocessableEntity;
                break;
            case ResultStatus.NotFound:
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                break;
            case ResultStatus.Failed:
                context.Response.StatusCode = (int)HttpStatusCode.UnprocessableEntity;
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
            errorResult.Status = ResultStatus.ValidationError;
            var code = businessException.GetCode();
            var message = businessException.GetMessage();
            errorResult.AppendError(message, null, code);
        }
        else if (exception is UnauthorizedException unauthorizedException)
        {
            errorResult.Status = ResultStatus.Unauthorized;
            errorResult.AppendError(unauthorizedException.Message);
        }
        else if (exception is UnauthenticatedException unauthenticatedException)
        {
            errorResult.Status = ResultStatus.Unauthenticated;
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
