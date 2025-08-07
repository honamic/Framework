namespace Honamic.Framework.Applications.Authorizes;

public class DynamicPermissionModel
{
    public string Name { get; set; } = default!;
    public string DisplayName { get; set; } = default!;
    public string? Group { get; set; }
    public string? Module { get; set; }
    public string? Description { get; set; }
    public string? GeneratedFromTypeName { get; set; }
    
    public List<DynamicScopePermissionModel> ScopePermissions { get; set; } = new();
    public List<DynamicFieldPermissionModel> FieldPermissions { get; set; } = new();
}

public class DynamicScopePermissionModel
{
    public string Scope { get; set; } = default!;
    public ValuesType ValuesType { get; set; }
    public string DisplayName { get; set; } = default!;
}

public class DynamicFieldPermissionModel
{
    public string Name { get; set; } = default!;
    public string DisplayName { get; set; } = default!;
}