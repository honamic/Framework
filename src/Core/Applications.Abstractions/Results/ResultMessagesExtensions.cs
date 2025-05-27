namespace Honamic.Framework.Applications.Results;

public static class ResultMessagesExtensions
{
    public static void AppendWarning(this Result result, string message)
    {
        result.Messages.Add(new ResultMessage(ResultMessageType.Warning, message));
    }

    public static void AppendInfo(this Result result, string message)
    {
        result.Messages.Add(new ResultMessage(ResultMessageType.Info, message));
    }

    public static void AppendError(this Result result, string message, string? field, string? code)
    {
        result.Messages.Add(new ResultMessage(ResultMessageType.Error, message, field, code));
    }

    public static void AppendError(this Result result, string message, string? field)
    {
        result.AppendError(message, field, null);
    }

    public static void AppendError(this Result result, string message)
    {
        result.AppendError(message, null, null);
    }

    public static void AppendSorryError(this Result result)
    {
        //todo: localization message
        result.Messages.Add(new ResultMessage(ResultMessageType.Error, "متاسفانه خطایی پیش آمده است."));
    }

    public static void AppendSuccess(this Result result, string message)
    {
        result.Messages.Add(new ResultMessage(ResultMessageType.Success, message));
    }

    public static void SetStatusAsNotFound(this Result result)
    {
        result.Status = ResultStatus.NotFound;
    }

    public static void SetStatusAsNotFound(this Result result, string errorMessage)
    {
        result.Status = ResultStatus.NotFound;
        result.AppendError(errorMessage);
    }

    public static void SetStatusAsUnauthenticated(this Result result)
    {
        result.Status = ResultStatus.Unauthenticated;
    }

    public static void SetStatusAsUnauthenticated(this Result result, string errorMessage)
    {
        result.Status = ResultStatus.Unauthenticated;
        result.AppendError(errorMessage);
    }

    public static void SetStatusAsUnauthorized(this Result result)
    {
        result.Status = ResultStatus.Unauthorized;
    }

    public static void SetStatusAsUnauthorized(this Result result, string errorMessage)
    {
        result.Status = ResultStatus.Unauthorized;
        result.AppendError(errorMessage);
    }

    public static void SetStatusAsUnhandledException(this Result result, string errorMessage)
    {
        result.Status = ResultStatus.UnhandledException;
        result.AppendError(errorMessage);
    }

    public static void SetStatusAsUnhandledExceptionWithSorryError(this Result result)
    {
        result.Status = ResultStatus.UnhandledException;
        result.AppendSorryError();
    }

    public static void SetStatusAsInvalidDomainState(this Result result, string errorMessage)
    {
        result.Status = ResultStatus.InvalidDomainState;
        result.AppendError(errorMessage);
    }
}
