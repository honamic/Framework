using System.Reflection;

namespace Honamic.Framework.Application.Authorizes;
public static class DynamicPermissionRegistry
{
    private static readonly List<Assembly> _assemblies = new();

    public static void Register(Assembly assembly)
    {
        if (!_assemblies.Contains(assembly))
            _assemblies.Add(assembly);
    }
    public static void RegisterAll(IEnumerable<Assembly> assemblies)
    {
        foreach (var assembly in assemblies)
        {
            Register(assembly);
        }
    }
    public static void RegisterAll(params Assembly[] assemblies)
    {
        RegisterAll((IEnumerable<Assembly>)assemblies);
    }
    public static IReadOnlyList<Assembly> GetRegisteredAssemblies() => _assemblies;
}