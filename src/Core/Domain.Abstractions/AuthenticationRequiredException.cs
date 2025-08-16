namespace Honamic.Framework.Applications.Exceptions;

public class AuthenticationRequiredException : Exception
{
    public override string Message => "Authentication is required";
}