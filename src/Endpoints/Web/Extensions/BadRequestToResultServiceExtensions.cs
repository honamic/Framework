using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Honamic.Framework.Endpoints.Web.Extensions;

public static class BadRequestToResultServiceExtensions
{
    public static void ConfigureBadRequestToResult(this IServiceCollection services)
    {
        services.PostConfigure<ApiBehaviorOptions>(options =>
        {
            options.InvalidModelStateResponseFactory = actionContext
                       => new Results.BadRequestResult(actionContext.ModelState);
        });
    }
}
