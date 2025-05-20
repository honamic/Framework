namespace Honamic.Framework.Applications.Exceptions;

public class UnauthorizedException : Exception
{
    public UnauthorizedException(string permissionName)
    {
        PermissionName = permissionName;
    }
    public override string Message => "the request does not have valid authentication credentials for the operation";

    public string PermissionName { get; }
}
