using Honamic.Framework.Endpoints.Web.Extensions;
using Honamic.Framework.Facade.Web.Middleware;

namespace Honamic.Todo.Endpoints.WebApi.Extensions;

public static class WebApplicationExtensions
{
    public static WebApplication UseConfigurations(this WebApplication app)
    {
        app.UseExceptionToResult();
        app.UseFacadeDiscoveryEndpoint();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(); 
        }

        // global cors policy
        app.UseCors(x => x
            .AllowAnyMethod()
            .AllowAnyHeader()
            .SetIsOriginAllowed(origin => true) 
            .AllowCredentials()); 

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        return app;
    }
}
