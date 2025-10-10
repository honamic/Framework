using System.Reflection;

namespace Honamic.Framework.Application.Authorizes;

public static class DynamicPermissionExtractor
{
    public static List<DynamicPermissionModel> AggregatePermissions()
    {
        return AggregatePermissions(DynamicPermissionRegistry.GetRegisteredAssemblies());
    }

    public static List<DynamicPermissionModel> AggregatePermissions(IEnumerable<Assembly> assemblies)
    {
        var permissions = new List<DynamicPermissionModel>();

        foreach (var assembly in assemblies)
        {
            foreach (var type in assembly.GetTypes())
            {
                var dynamicAttr = type.GetCustomAttribute<DynamicPermissionAttribute>();
                if (dynamicAttr == null) continue;

                var scopeAttrs = type.GetCustomAttributes<ScopeDynamicPermissionAttribute>().ToList();
                var fieldAttrs = type.GetCustomAttributes<FieldDynamicPermissionAttribute>().ToList();

                var permission = new DynamicPermissionModel
                {
                    Name = dynamicAttr.Name ?? type.FullName!,
                    DisplayName = dynamicAttr.DisplayName ?? dynamicAttr.Name ?? type.Name,
                    Group = dynamicAttr.Group,
                    Module = dynamicAttr.Module ?? assembly.GetName().Name,
                    Description = dynamicAttr.Description,
                    GeneratedFromTypeName = type.Assembly.FullName,
                    ScopePermissions = scopeAttrs.Select(scope => new DynamicScopePermissionModel
                    {
                        Name = scope.Name,
                        DisplayName = scope.DisplayName ?? scope.Name,
                        Description = scope.Description
                    }).ToList(),
                    FieldPermissions = fieldAttrs.Select(field => new DynamicFieldPermissionModel
                    {
                        Name = field.Name,
                        DisplayName = field.DisplayName ?? field.Name,
                        Description = field.Description
                    }).ToList()
                };

                permissions.Add(permission);
            }
        }

        return permissions;
    }
}