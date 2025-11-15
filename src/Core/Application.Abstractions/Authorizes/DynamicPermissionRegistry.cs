using System.Reflection;

namespace Honamic.Framework.Application.Authorizes;

public static class DynamicPermissionRegistry
{
    private static readonly List<Assembly> _assemblies = new();

    private static void Register(Assembly assembly)
    {
        if (!_assemblies.Contains(assembly))
            _assemblies.Add(assembly);
    }

    /// <summary>
    /// Registers the specified assemblies for dynamic permission discovery.
    /// If no assemblies are provided, it defaults to registering the calling assembly.
    /// </summary>
    /// <param name="assemblies">The assemblies to register. If empty, uses the calling assembly.</param>
    public static void RegisterAssemblies(params Assembly[] assemblies)
    {
        if (assemblies.Length == 0)
        {
            assemblies = new[] { Assembly.GetCallingAssembly() };
        }

        foreach (var assembly in assemblies)
        {
            Register(assembly);
        }
    }
    public static IReadOnlyList<Assembly> GetRegisteredAssemblies() => _assemblies;
}