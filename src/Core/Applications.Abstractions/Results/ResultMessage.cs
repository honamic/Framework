namespace Honamic.Framework.Applications.Results;

public class ResultMessage
{
    public ResultMessage(ResultMessageType type, string message, string? code = null, string? field = null)
    {
        Type = type;
        Message = message;
        Field = field;
        Code = code;
    }

    public string Message { get; set; }

    public string? Field { get; set; }

    public string? Code { get; set; }

    public ResultMessageType Type { get; set; }
}

public enum ResultMessageType
{
    Success = 1,
    Info = 2,
    Warning = 4,
    Error = 8
}
