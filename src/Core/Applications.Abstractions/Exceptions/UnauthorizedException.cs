namespace Honamic.Framework.Applications.Exceptions;

public class UnauthorizedException : Exception
{
    public UnauthorizedException(string permissionName)
    {
        PermissionName = permissionName;
    }
    public override string Message => "You do not have permission to perform this operation";

    public string PermissionName { get; }
}
