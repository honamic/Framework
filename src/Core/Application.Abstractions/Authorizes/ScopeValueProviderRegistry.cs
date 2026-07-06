
namespace Honamic.Framework.Application.Authorizes;

public class ScopeValueProviderRegistry : IScopeValueProviderRegistry
{
    private readonly Dictionary<string, IScopeValueProvider> _providers;

    public ScopeValueProviderRegistry(IEnumerable<IScopeValueProvider> providers)
    {
        _providers = providers
            .GroupBy(p => p.Key, StringComparer.OrdinalIgnoreCase)
            .ToDictionary(g => g.Key, g => g.Last(), StringComparer.OrdinalIgnoreCase);
    }

    public IScopeValueProvider? Find(string key)
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            return null;
        }

        return _providers.TryGetValue(key, out var provider) ? provider : null;
    }
}