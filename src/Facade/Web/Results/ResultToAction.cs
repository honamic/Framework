using Honamic.Framework.Applications.Results;
using Microsoft.AspNetCore.Mvc;

namespace Honamic.Framework.Facade.Web.Results;
public static class ResultExtensions
{

    public static ActionResult<Result<T>> ResultToAction<T>(this ControllerBase controller, Result<T> result)
    {
        switch (result.Status)
        {
            case ResultStatus.Ok:
                return controller.Ok(result);

            case ResultStatus.Unauthorized:
            case ResultStatus.Unauthenticated:
                return controller.Unauthorized(result);

            case ResultStatus.UnhandledException:
                return new ActionResult<Result<T>>(result);

            case ResultStatus.ValidationError:
            case ResultStatus.InvalidDomainState:
                return controller.BadRequest(result);

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
            case ResultStatus.Ok:
                return controller.Ok(result);

            case ResultStatus.Unauthorized:
            case ResultStatus.Unauthenticated:
                return controller.Unauthorized(result);

            case ResultStatus.UnhandledException:
                return new ActionResult<Result>(result);

            case ResultStatus.ValidationError:
            case ResultStatus.InvalidDomainState:
                return controller.BadRequest(result);

            case ResultStatus.NotFound:
                return controller.NotFound(result);

            default:
                return controller.Ok(result);
        }
    }
}
