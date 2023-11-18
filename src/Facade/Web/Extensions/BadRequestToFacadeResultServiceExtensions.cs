using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Honamic.Framework.Endpoints.Web.Extensions;

public static class BadRequestToFacadeResultServiceExtensions
{
    public static void ConfigureBadRequestToFacadeResult(this IServiceCollection services)
    {
        services.PostConfigure<ApiBehaviorOptions>(options =>
        {
            options.InvalidModelStateResponseFactory = actionContext
                       => new BadRequestFacadeResult(actionContext.ModelState);
        });
    }
}
