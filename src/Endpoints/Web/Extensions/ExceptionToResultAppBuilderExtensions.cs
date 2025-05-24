using Honamic.Framework.Endpoints.Web.Middleware;
using Microsoft.AspNetCore.Builder;

namespace Honamic.Framework.Endpoints.Web.Extensions;

public static class ExceptionToResultAppBuilderExtensions
{
    public static IApplicationBuilder UseExceptionToResult(this IApplicationBuilder app)
    {
        return app.UseMiddleware<ExceptionToResultMiddleware>();
    }
}