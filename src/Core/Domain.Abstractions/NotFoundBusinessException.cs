namespace Honamic.Framework.Domain;

public class NotFoundBusinessException : BusinessException
{
    public NotFoundBusinessException(string? message = null, string code = "404", Exception? innerException = null)
        : base(message ?? "Item not found.", code, innerException)
    {
    }
}