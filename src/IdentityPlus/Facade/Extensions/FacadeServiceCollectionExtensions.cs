using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Honamic.Framework.Facade.Extensions;
using Honamic.IdentityPlus.Facade.Accounts;

namespace Honamic.IdentityPlus.Facade.Extensions;

public static class FacadeServiceCollectionExtensions
{
    public static void AddIdentityPlusFacades(this IServiceCollection services, IConfiguration configuration)
    {
         services.AddDefaultFrameworkFacadeServices(configuration);
 

        services.AddFacades();
    }

    private static void AddFacades(this IServiceCollection services)
    {
        services.AddFacadeScoped<IUserFacade, UserFacade>();
        services.AddFacadeScoped<IAccountFacade, AccountFacade>();
    }
}