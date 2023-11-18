namespace Honamic.Framework.Facade;

[AttributeUsage(AttributeTargets.All, AllowMultiple = false)]
public class FacadeAuthorizeAttribute : Attribute
{

}

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class FacadeAllowAnonymousAttribute : Attribute
{

}

[AttributeUsage(AttributeTargets.All, AllowMultiple = false)]
public class FacadeDynamicAuthorizeAttribute : FacadeAuthorizeAttribute
{

}

public class FacadeStaticAuthorizeAttribute : Attribute
{

    public FacadeStaticAuthorizeAttribute(string permission)
    {
        Permission = permission;
    }

    public string Permission { get; }
}



