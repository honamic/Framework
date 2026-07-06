
namespace Honamic.Framework.Application.Authorizes;

/// <summary>
/// Resolves the registered <see cref="IScopeValueProvider"/> for a given value-source key.
/// </summary>
public interface IScopeValueProviderRegistry
{
    IScopeValueProvider? Find(string key);
}