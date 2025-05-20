namespace Honamic.Framework.Applications.Exceptions;

public class UnauthenticatedException : Exception
{
    public override string Message => "Authentication is required";
}