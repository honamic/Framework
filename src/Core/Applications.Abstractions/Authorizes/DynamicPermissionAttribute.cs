namespace Honamic.Framework.Applications.Authorizes;

[AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = false)]
public sealed class DynamicPermissionAttribute : Attribute
{
    public string? Name { get; set; }
    public string? DisplayName { get; set; }
    public string? Group { get; set; }
    public string? Module { get; set; }
    public string? Description { get; set; }
}

[AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
public sealed class ScopeDynamicPermissionAttribute : Attribute
{
    public string Scope { get; }
    public ValuesType ValuesType { get; }
    public string? DisplayName { get; }

    public ScopeDynamicPermissionAttribute(string scope, ValuesType valuesType, string? displayName = null)
    {
        Scope = scope;
        ValuesType = valuesType;
        DisplayName = displayName;
    }
}

[AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
public sealed class FieldDynamicPermissionAttribute : Attribute
{
    public string Name { get; }
    public string? DisplayName { get; }

    public FieldDynamicPermissionAttribute(string name, string? displayName = null)
    {
        Name = name;
        DisplayName = displayName;
    }
}

public enum ValuesType
{
    My = 0,
    Value = 1,
    List = 2,
}