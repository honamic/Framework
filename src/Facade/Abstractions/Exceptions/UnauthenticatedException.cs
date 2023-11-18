namespace Honamic.Framework.Facade.Exceptions;

public class UnauthenticatedException : Exception
{
    public override string Message => "Authentication is required";
}