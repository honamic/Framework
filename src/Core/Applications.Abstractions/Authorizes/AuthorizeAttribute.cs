namespace Honamic.Framework.Applications.Authorizes;

[AttributeUsage(AttributeTargets.All, AllowMultiple = false)]
public class AuthorizeAttribute : Attribute
{
    public AuthorizeAttribute()
    {
        Permissions = null;
    }

    public AuthorizeAttribute(params string[] permissions)
    {
        Permissions = permissions;
    }

    public string[]? Permissions { get; }
}