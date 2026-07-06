namespace Honamic.Framework.Application.Extensions;

using global::Honamic.Framework.Application.Authorizes; 
using Microsoft.Extensions.DependencyInjection; 

public static class ScopeValueServiceCollectionExtensions
{
    /// <summary>
    /// Registers a scope value provider. Each provider serves one value-source key
    /// and is resolved by <see cref="IScopeValueProviderRegistry"/>.
    /// </summary>
    public static IServiceCollection AddScopeValueProvider<TProvider>(this IServiceCollection services)
        where TProvider : class, IScopeValueProvider
    {
        services.AddScoped<IScopeValueProvider, TProvider>();
        return services;
    }
}