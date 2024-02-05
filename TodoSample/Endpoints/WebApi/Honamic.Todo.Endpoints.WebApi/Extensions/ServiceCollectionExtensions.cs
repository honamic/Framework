using Honamic.Framework.Endpoints.Web.Extensions;
using Honamic.Framework.Utilities.Web.Json;
using Honamic.Todo.Facade.Extensions;
using Honamic.IdentityPlus.WebApi.Extensions; 
using Honamic.IdentityPlus.Facade.Extensions; 
using MudBlazor.Services;

namespace Honamic.Todo.Endpoints.WebApi.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddFacades(configuration);
        services.AddIdentityPlusFacades(configuration);

        services.AddIdentityPlusApiEndpoint();
        services.AddEndpointsServices(configuration);

        return services;
    }

    private static void AddEndpointsServices(this IServiceCollection services, IConfiguration configurations)
    {
        services.ConfigureBadRequestToFacadeResult();
        services.AddControllers()
                    .AddJsonOptions(c =>
                    {
                        c.JsonSerializerOptions
                         .Converters.Add(new CustomLongToStringConverter());
                    });

        services.AddEndpointsApiExplorer();
        services.AddSwaggerAndConfiguration();
        services.AddCors();

        services.AddBlazorServices(configurations)
            ;
    }


    private static void AddBlazorServices(this IServiceCollection services, IConfiguration configurations)
    {

        services.AddRazorComponents()
           .AddInteractiveServerComponents()
           .AddInteractiveWebAssemblyComponents();

        services.AddMudServices();

    }
}
