using Honamic.Framework.Endpoints.Web.Middleware;
using Microsoft.AspNetCore.Builder;

namespace Honamic.Framework.Endpoints.Web.Extensions;

public static class ExceptionToFacadeResultAppBuilderExtensions
{
    public static IApplicationBuilder UseExceptionToFacadeResult(this IApplicationBuilder app)
    {
        return app.UseMiddleware<ExceptionToFacadeResultMiddleware>();
    }
}