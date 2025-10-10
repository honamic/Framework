namespace Honamic.Framework.Domain;

public class NotFoundBusinessException : BusinessException
{
    public NotFoundBusinessException(string? message = null, string? code = null)
        : base(message ?? "Item not found.", code)
    {
    }
}