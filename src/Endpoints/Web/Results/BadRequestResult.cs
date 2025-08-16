using Honamic.Framework.Applications.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Honamic.Framework.Endpoints.Web.Results;

[DefaultStatusCode(DefaultStatusCode)]
public class BadRequestResult : ObjectResult
{
    private const int DefaultStatusCode = StatusCodes.Status400BadRequest;

    public BadRequestResult(ModelStateDictionary modelState) :
        base(ToResult(modelState))
    {
        if (modelState == null)
        {
            throw new ArgumentNullException(nameof(modelState));
        }

        StatusCode = DefaultStatusCode;
    }

    private static Result ToResult(ModelStateDictionary modelState)
    {
        if (modelState == null)
        {
            throw new ArgumentNullException(nameof(modelState));
        }

        var result = new Result(ResultStatus.ValidationFailed);

        result.AppendError("The parameters sent are not correct.");

        foreach ((string? key, ModelStateEntry? value) in modelState)
        {
            var errors = value.Errors;

            if (errors is { Count: > 0 })
            {
                foreach (var error in errors)
                {
                    var message = string.IsNullOrEmpty(error.ErrorMessage) ? "The input was not valid." : error.ErrorMessage;

                    var field = string.IsNullOrEmpty(key) ? null : key;

                    result.AppendError($"{field}:{message}", "BadRequest");
                }
            }
        }

        return result;
    }
}
