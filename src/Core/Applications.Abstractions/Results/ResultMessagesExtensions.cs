namespace Honamic.Framework.Applications.Results;

public static class ResultMessagesExtensions
{
    public static void AppendWarningmessage(this Result result, string message)
    {
        result.AddMessage(ResultMessageType.Warning, message);
    }

    public static void AppendInfomessage(this Result result, string message)
    {
        result.AddMessage(ResultMessageType.Info, message);
    }

    public static void AppendErrormessage(this Result result, string message, string? field, string? code)
    {
        result.AddMessage(ResultMessageType.Error, message, field, code);
    }

    public static void AppendError(this Result result, string message, string? field)
    {
        result.AppendErrormessage(message, field, null);
    }

    public static void AppendError(this Result result, string message)
    {
        result.AppendErrormessage(message, null, null);
    }

    public static void AppendSorryErrormessage(this Result result)
    {
        // TODO: localization message
        result.AddMessage(ResultMessageType.Error, "متاسفانه خطایی پیش آمده است.");
    }

    public static void AppendSuccessmessage(this Result result, string message)
    {
        result.AddMessage(ResultMessageType.Success, message);
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

    public static void SetStatusAsAuthenticationRequired(this Result result)
    {
        result.Status = ResultStatus.AuthenticationRequired;
    }

    public static void SetStatusAsAuthenticationRequired(this Result result, string errorMessage)
    {
        result.Status = ResultStatus.AuthenticationRequired;
        result.AppendError(errorMessage);
    }

    public static void SetStatusAsForbidden(this Result result)
    {
        result.Status = ResultStatus.Forbidden;
    }

    public static void SetStatusAsForbidden(this Result result, string errorMessage)
    {
        result.Status = ResultStatus.Forbidden;
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
        result.AppendSorryErrormessage();
    }

    public static void SetStatusAsDomainStateInvalid(this Result result, string errorMessage)
    {
        result.Status = ResultStatus.DomainStateInvalid;
        result.AppendError(errorMessage);
    }

    public static void SetStatusAsValidationFailed(this Result result)
    {
        result.Status = ResultStatus.ValidationFailed;
    }
}