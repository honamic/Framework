namespace Honamic.Framework.Application.Authorizes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
public sealed class DynamicPermissionAttribute : Attribute
{
    public string? Name { get; set; }
    public string? DisplayName { get; set; }
    public string? Group { get; set; }
    public string? Module { get; set; }
    public string? Description { get; set; }
}

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
public sealed class ScopeDynamicPermissionAttribute : Attribute
{
    public string Name { get; }
    public string? DisplayName { get; }
    public string? Description { get; set; }

    public ScopeDynamicPermissionAttribute(string name, string? displayName = null, string? description = null)
    {
        Name = name;
        DisplayName = displayName;
        Description = description;
    }
}

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
public sealed class FieldDynamicPermissionAttribute : Attribute
{
    public string Name { get; }

    public string? DisplayName { get; }
    
    public string? Description { get; set; }

    public FieldDynamicPermissionAttribute(string name, string? displayName = null, string? description = null)
    {
        Name = name;
        DisplayName = displayName;
        Description = description;
    }
}
