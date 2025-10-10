namespace Honamic.Framework.Domain;

public interface IBusinessException
{
    string? GetCode();
    string GetMessage();
}
