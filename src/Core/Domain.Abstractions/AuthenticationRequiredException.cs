namespace Honamic.Framework.Domain;

public class AuthenticationRequiredException : Exception
{
    public override string Message => "Authentication is required";
}