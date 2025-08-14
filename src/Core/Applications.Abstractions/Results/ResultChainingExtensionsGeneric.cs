
namespace Honamic.Framework.Applications.Results;
public static class ResultChainingExtensionsGeneric
{
    public static Result<T> WithError<T>(this Result<T> result, string message, string? field = null, string? code = null)
    {
        result.AddMessage(ResultMessageType.Error, message, field, code);
        return result;
    }

    public static Result<T> WithWarning<T>(this Result<T> result, string message)
    {
        result.AddMessage(ResultMessageType.Warning, message);
        return result;
    }

    public static Result<T> WithInfo<T>(this Result<T> result, string message)
    {
        result.AddMessage(ResultMessageType.Info, message);
        return result;
    }

    public static Result<T> WithSuccess<T>(this Result<T> result, string message)
    {
        result.AddMessage(ResultMessageType.Success, message);
        return result;
    }

    public static Result<T> WithStatus<T>(this Result<T> result, ResultStatus status)
    {
        result.Status = status;
        return result;
    }

    public static Result<T> WithData<T>(this Result<T> result, T data)
    {
        result.Data = data;
        return result;
    }

    public static Result<T> WithFailure<T>(this Result<T> result, ResultStatus status, string message)
    {
        result.Status = status;
        result.AddMessage(ResultMessageType.Error, message);
        return result;
    }
}

