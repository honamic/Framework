using Honamic.Framework.Application.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Honamic.Framework.Endpoints.Web.Results;
public static class ResultExtensions
{
    public static ActionResult<Result<T>> ResultToAction<T>(this ControllerBase controller, Result<T> result)
    {
        switch (result.Status)
        {
            case ResultStatus.Success:
                return controller.Ok(result);

            case ResultStatus.Forbidden:
                return new ObjectResult(result) { StatusCode = StatusCodes.Status403Forbidden };

            case ResultStatus.AuthenticationRequired:
                return controller.Unauthorized(result);

            case ResultStatus.UnhandledException:
                return new ActionResult<Result<T>>(result);

            case ResultStatus.ValidationFailed:
                return controller.BadRequest(result);
            
            case ResultStatus.DomainStateInvalid: 
                return new ObjectResult(result) { StatusCode = StatusCodes.Status422UnprocessableEntity };

            case ResultStatus.NotFound:
                return controller.NotFound(result);

            default:
                return controller.Ok(result);
        }
    }
    public static ActionResult<Result> ResultToAction(this ControllerBase controller, Result result)
    {
        switch (result.Status)
        {
            case ResultStatus.Success:
                return controller.Ok(result);

            case ResultStatus.AuthenticationRequired:
                return controller.Unauthorized(result);

            case ResultStatus.Forbidden:
                return new ObjectResult(result) { StatusCode = StatusCodes.Status403Forbidden };

            case ResultStatus.UnhandledException:
                return new ActionResult<Result>(result);

            case ResultStatus.DomainStateInvalid:
                return new ObjectResult(result) { StatusCode = StatusCodes.Status422UnprocessableEntity };

            case ResultStatus.ValidationFailed: 
                return controller.BadRequest(result);

            case ResultStatus.NotFound:
                return controller.NotFound(result);

            default:
                return controller.Ok(result);
        }
    }
}