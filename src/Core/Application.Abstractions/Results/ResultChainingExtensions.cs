namespace Honamic.Framework.Application.Results;

public static class ResultChainingExtensions
{
    public static Result WithErrorMessage(this Result result, string message, string? field = null, string? code = null)
    {
        result.AddMessage(ResultMessageType.Error, message, field, code);
        return result;
    }

    public static Result WithWarningMessage(this Result result, string message)
    {
        result.AddMessage(ResultMessageType.Warning, message);
        return result;
    }

    public static Result WithInfoMessage(this Result result, string message)
    {
        result.AddMessage(ResultMessageType.Info, message);
        return result;
    }

    public static Result WithSuccessMessage(this Result result, string message)
    {
        result.AddMessage(ResultMessageType.Success, message);
        return result;
    }

    public static Result WithStatus(this Result result, ResultStatus status)
    {
        result.Status = status;
        return result;
    }

    public static Result WithSuccessStatus(this Result result)
    {
        result.Status = ResultStatus.Success;
        return result;
    }

    public static Result WithFailure(this Result result, ResultStatus status, string message)
    {
        result.Status = status;
        result.AddMessage(ResultMessageType.Error, message);
        return result;
    }
}