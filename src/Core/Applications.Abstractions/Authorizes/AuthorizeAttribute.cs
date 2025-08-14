namespace Honamic.Framework.Applications.Authorizes;

[AttributeUsage(AttributeTargets.All, AllowMultiple = false)]
public class AuthorizeAttribute : Attribute
{
    public AuthorizeAttribute()
    {
        Roles = null;
    }

    public AuthorizeAttribute(params string[] roles)
    {
        Roles = roles;
    }

    public string[]? Roles { get; }
}