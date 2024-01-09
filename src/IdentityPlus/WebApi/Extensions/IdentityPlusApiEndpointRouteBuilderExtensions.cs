using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace Honamic.IdentityPlus.WebApi.Extensions;

public static class IdentityPlusApiEndpointRouteBuilderExtensions
{
    public static IEndpointRouteBuilder MapIdentityPlusApi(this IEndpointRouteBuilder endpoints)
    {
        //regiser [minimal api] here

        return  endpoints;
    }



    public static RazorComponentsEndpointConventionBuilder AddIdentityPlusComponents(
this RazorComponentsEndpointConventionBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.AddAdditionalAssemblies(
              typeof(IdentityPlusApiEndpointRouteBuilderExtensions).Assembly);

        return builder;
    }
}