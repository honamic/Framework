using Honamic.Framework.Applications.Results;

namespace Honamic.Framework.Applications;
public static class ResultChainingExtensions
{
    public static Result WithError(this Result result, string message, string? field = null, string? code = null)
    {
        result.AddMessage(ResultMessageType.Error, message, field, code);
        return result;
    }

    public static Result WithWarning(this Result result, string message)
    {
        result.AddMessage(ResultMessageType.Warning, message);
        return result;
    }

    public static Result WithInfo(this Result result, string message)
    {
        result.AddMessage(ResultMessageType.Info, message);
        return result;
    }

    public static Result WithSuccess(this Result result, string message)
    {
        result.AddMessage(ResultMessageType.Success, message);
        return result;
    }

    public static Result WithStatus(this Result result, ResultStatus status)
    {
        result.Status = status;
        return result;
    }

    public static Result WithSorryError(this Result result)
    {
        result.AddMessage(ResultMessageType.Error, "متاسفانه خطایی پیش آمده است.");
        return result;
    }

    public static Result WithFailure(this Result result, ResultStatus status, string message)
    {
        result.Status = status;
        result.AddMessage(ResultMessageType.Error, message);
        return result;
    }
}