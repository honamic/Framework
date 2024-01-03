using Honamic.IdentityPlus.Domain.Users;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace Honamic.IdentityPlus.WebApi.Extensions;

public static class IdentityPlusApiEndpointRouteBuilderExtensions
{
    public static IEndpointConventionBuilder MapIdentityPlusApi(this IEndpointRouteBuilder endpoints)
    {
        // Add additional endpoints required by the Identity /Account Razor components.
        endpoints.MapAdditionalIdentityEndpoints();

        return  endpoints.MapGroup("/identity").MapIdentityApi<User>();
    }

    public static RazorComponentsEndpointConventionBuilder AddIdentityPlusComponents(
    this RazorComponentsEndpointConventionBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.AddAdditionalAssemblies(
              typeof(Razor.IdentityRedirectManager).Assembly);

        return builder;
    }
}