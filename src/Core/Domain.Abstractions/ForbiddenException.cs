namespace Honamic.Framework.Domain;

public class ForbiddenException : Exception
{
    public ForbiddenException(string permissionName)
    {
        PermissionName = permissionName;
    }
    public override string Message => "You do not have permission to perform this operation";

    public string PermissionName { get; }
}
