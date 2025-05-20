using System.Collections.ObjectModel;

namespace Honamic.Framework.Applications.Results;

public class Result
{
    public ResultStatus Status { get; set; }

    public void Succeed(string message = null)
    {
        Status = ResultStatus.Ok;

        if (message != null)
        {
            Messages.Add(new ResultMessage(ResultMessageType.Success, message));
        }
    }

    public ICollection<ResultMessage> Messages { get; set; }

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
        return new Result(boolValue ? ResultStatus.Ok : ResultStatus.None);
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

    public static implicit operator Result<TData>(TData date)
    {
        return new Result<TData> { Data = date, Status = ResultStatus.Ok };
    }
}