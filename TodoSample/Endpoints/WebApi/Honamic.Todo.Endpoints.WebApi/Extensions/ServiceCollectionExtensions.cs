using Honamic.Framework.Endpoints.Web.Extensions;
using Honamic.Framework.Utilities.Web.Json;
using Honamic.Todo.Facade.Extensions;

namespace Honamic.Todo.Endpoints.WebApi.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddFacades(configuration);
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
        services.AddSwaggerGen();
        services.AddCors();
    }
}
