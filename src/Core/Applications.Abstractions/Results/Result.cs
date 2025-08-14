using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;

namespace Honamic.Framework.Applications.Results;

public class Result
{
    public ResultStatus Status { get; set; }

    public ICollection<ResultMessage> Messages { get; set; }

    public bool IsSuccess => Status == ResultStatus.Success;
    
    public bool IsFailure => !IsSuccess;

    public bool HasMessages => Messages.Any();


    [Obsolete("Use SetSuccess instead.", true)]
    public void Succeed(string message = null)
    {
        Status = ResultStatus.Success;

        if (message != null)
        {
            Messages.Add(new ResultMessage(ResultMessageType.Success, message));
        }
    }

    public void SetSuccess(string? message = null)
    {
        Status = ResultStatus.Success;
        if (message != null)
            AddMessage(ResultMessageType.Success, message);
    }

    public void SetFailure(ResultStatus failureStatus, string? message = null)
    {
        Status = failureStatus;
        if (message != null)
            AddMessage(ResultMessageType.Error, message);
    }

    public void AddMessage(ResultMessageType type, string message)
    {
        Messages.Add(new ResultMessage(type, message));
    }

    public void AddMessage(ResultMessageType type, string message, string? field = null, string? code = null)
    {
        Messages.Add(new ResultMessage(type, message, field, code));
    }

    public Result()
    {
        Messages = new Collection<ResultMessage>();
    }

    public Result(ResultStatus status) : this()
    {
        Status = status;
    }

    public static implicit operator Result(ResultStatus status)
    {
        return new Result(status);
    }

    public static implicit operator Result(bool boolValue)
    {
        return new Result(boolValue ? ResultStatus.Success : ResultStatus.None);
    }

    public static Result Success(string? message = null)
    {
        var result = new Result();
        result.SetSuccess(message);
        return result;
    }

    public static Result Failure(ResultStatus status, string? message = null)
    {
        var result = new Result();
        result.SetFailure(status, message);
        return result;
    }

    public static Result Failure(string message, string? field = null, string? code = null)
    {
        var result = new Result();
        result.AddMessage(ResultMessageType.Error, message, field, code);
        return result;
    }
}

public class Result<TData> : Result
{
    public TData? Data { get; set; }

    public Result()
    {

    }

    public Result(TData data)
    {
        Data = data;
    }

    [MemberNotNullWhen(true, nameof(Data))]
    public bool IsSuccessWithData => Status == ResultStatus.Success && Data is not null;

    public new static Result<TData> Success(string? message = null)
    {
        var result = new Result<TData>();
        result.SetSuccess(message);
        return result;
    }

    public static Result<TData> Success(TData data, string? message = null)
    {
        var result = new Result<TData>();
        result.Data = data;
        result.SetSuccess(message);
        return result;
    }

    public new static Result<TData> Failure(ResultStatus status, string? message = null)
    {
        var result = new Result<TData>();
        result.SetFailure(status, message);
        return result;
    }

    public static implicit operator Result<TData>(TData date)
    {
        return new Result<TData> { Data = date, Status = ResultStatus.Success };
    }
}