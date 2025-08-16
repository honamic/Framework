using Honamic.Framework.Applications.Results;
using Microsoft.AspNetCore.Http;

namespace Honamic.Framework.Endpoints.Web.Results;
public static class ResultMinimalApiExtensions
{
    public static IResult ToMinimalApiResult<T>(this Result<T> result)
    {
        return (result as Result).ToMinimalApiResult();
    }

    public static IResult ToMinimalApiResult(this Result result)
    {
        switch (result.Status)
        {
            case ResultStatus.AuthenticationRequired:
                return Microsoft.AspNetCore.Http.Results.Json(result, statusCode: StatusCodes.Status401Unauthorized);
            case ResultStatus.Forbidden:
                return Microsoft.AspNetCore.Http.Results.Json(result, statusCode: StatusCodes.Status403Forbidden);
            case ResultStatus.UnhandledException:
                return Microsoft.AspNetCore.Http.Results.Json(result, statusCode: StatusCodes.Status500InternalServerError);
            case ResultStatus.ValidationFailed:
                return Microsoft.AspNetCore.Http.Results.Json(result, statusCode: StatusCodes.Status400BadRequest);
            case ResultStatus.DomainStateInvalid:
                return Microsoft.AspNetCore.Http.Results.Json(result, statusCode: StatusCodes.Status422UnprocessableEntity);
            case ResultStatus.NotFound:
                return Microsoft.AspNetCore.Http.Results.NotFound(result);
            case ResultStatus.Undefined:
            case ResultStatus.Success:
            default:
                return Microsoft.AspNetCore.Http.Results.Ok(result);
        }
    }
}