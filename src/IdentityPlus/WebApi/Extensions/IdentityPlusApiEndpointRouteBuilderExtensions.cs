using Honamic.IdentityPlus.Domain.Users;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace Honamic.IdentityPlus.WebApi.Extensions;
public static class IdentityPlusApiEndpointRouteBuilderExtensions
{
    public static IEndpointConventionBuilder MapIdentityPlusApi(this IEndpointRouteBuilder endpoints)
    {
      return  endpoints.MapGroup("/identity").MapIdentityApi<User>();

    }

}