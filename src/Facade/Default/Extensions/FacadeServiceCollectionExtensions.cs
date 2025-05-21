using Castle.DynamicProxy;
using Honamic.Framework.Applications.Authorizes;
using Honamic.Framework.Facade.Discovery;
using Honamic.Framework.Facade.Interceptors;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Honamic.Framework.Facade.Extensions;

public static class FacadeServiceCollectionExtensions
{
    public static void AddDefaultFrameworkFacadeServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton(new ProxyGenerator());
        services.AddScoped<IInterceptor, AuthorizeInterceptor>();
        services.AddScoped<IInterceptor, ExceptionHandlingInterceptor>();

        //temporary 
        services.AddDisableFacadeAuthorizationServices();
    }

    public static void AddDisableFacadeAuthorizationServices(this IServiceCollection services)
    {
        services.AddScoped<IAuthorization, DisableAuthorization>();
    }

    public static void AddFacadeScoped<TInterface, TImplementation>(this IServiceCollection services)
           where TInterface : IBaseFacade
           where TImplementation : BaseFacade, TInterface
    {
        services.AddScoped<TImplementation>();
        services.AddScoped(typeof(TInterface), serviceProvider =>
        {
            var proxyGenerator = serviceProvider.GetRequiredService<ProxyGenerator>();
            var actual = serviceProvider.GetRequiredService<TImplementation>();
            var interceptors = serviceProvider.GetServices<IInterceptor>().ToArray();
            return proxyGenerator.CreateInterfaceProxyWithTarget(typeof(TInterface), actual, interceptors);
        });

        FacadeDiscovery.RegisterFacadeType<TImplementation>();
    }


}