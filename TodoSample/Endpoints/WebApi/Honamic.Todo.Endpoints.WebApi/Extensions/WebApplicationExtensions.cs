using Honamic.Framework.Endpoints.Web.Extensions;
using Honamic.Framework.Facade.Web.Middleware;
using Honamic.IdentityPlus.WebApi.Extensions;
using Microsoft.AspNetCore.Builder;

namespace Honamic.Todo.Endpoints.WebApi.Extensions;

public static class WebApplicationExtensions
{
    public static WebApplication UseConfigurations(this WebApplication app)
    {
        app.UseExceptionToFacadeResult();
        app.UseFacadeDiscoveryEndpoint();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseWebAssemblyDebugging();
        }

        // global cors policy
        app.UseCors(x => x
            .AllowAnyMethod()
            .AllowAnyHeader()
            .SetIsOriginAllowed(origin => true) // allow any origin
                                                //.WithOrigins("https://localhost:44351")); // Allow only this origin can also have multiple origins separated with comma
            .AllowCredentials()); // allow credentials

        app.UseHttpsRedirection();

        app.UseAuthorization();



        app.MapIdentityPlusApi();

        app.MapControllers();

        app.UseStaticFiles();
        app.UseAntiforgery();

        app.MapRazorComponents<Components.App>()
            .AddInteractiveWebAssemblyRenderMode()
           .AddIdentityPlusComponents()
           .AddAdditionalAssemblies(
              typeof(Honamic.Todo.Endpoints.WasmClient.Pages.Counter).Assembly);


        return app;
    }
}
