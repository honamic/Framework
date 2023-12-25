using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;

namespace Honamic.Todo.Endpoints.WebApi.Extensions;

public static class SwaggerExtentions
{
    public static IServiceCollection AddSwaggerAndConfiguration(this IServiceCollection services)
    {
        services.AddSwaggerGen(op =>
        {
            op.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
            {
                In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "bearer"
            });

            op.OperationFilter<SecurityRequirementsOperationFilter>();
        });

        return services;

    }
}