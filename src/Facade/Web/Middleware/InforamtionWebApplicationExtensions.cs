using Honamic.Framework.Facade.Discovery;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace Honamic.Framework.Facade.Web.Middleware;
public static class FacadeDiscoveryWebApplicationExtensions
{
    public static WebApplication UseFacadeDiscoveryEndpoint(this WebApplication app, string path = "/_system/FacadeDiscovery")
    {
        app.MapGet(path, (HttpContext httpContext) =>
        {
            return FacadeDiscovery.GetFacadesInformation();
        });

        return app;
    }
}