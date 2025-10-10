using Honamic.Framework.Domain;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Honamic.Framework.Endpoints.Web.Authorization;

public class DefaultUserContext : IUserContext
{
    protected readonly IHttpContextAccessor _httpContext;

    public DefaultUserContext(IHttpContextAccessor httpContext)
    {
        _httpContext = httpContext;
    }

    public virtual string? GetCurrentUserId()
    {
        return GetUserId(_httpContext.HttpContext?.User);
    }

    public virtual long? GetCurrentUserIdAsNumber()
    {
        var currentUserId = GetUserId(_httpContext.HttpContext?.User);

        if (long.TryParse(currentUserId, out var userId))
            return userId;

        return null;
    }

    public virtual string? GetCurrentUserEmail()
    {
        return GetClaimValue(ClaimTypes.Email);
    }

    public virtual string? GetCurrentUserMobile()
    {
        return GetClaimValue(ClaimTypes.MobilePhone);
    }

    public virtual string? GetCurrentUsername()
    {
        return GetClaimValue(ClaimTypes.Name);
    }

    public virtual Task<List<string>> GetCurrentUserRolesAsync()
    {
        return Task.FromResult(GetUserRoles(_httpContext.HttpContext?.User));
    }

    public virtual string? GetClaimValue(string claimType)
    {
        var userIdentity = _httpContext.HttpContext?.User;

        return userIdentity?.Claims
            .Where(c => c.Type == claimType)
            .Select(c => c.Value)
            .FirstOrDefault();
    }

    private string? GetUserId(ClaimsPrincipal? user)
    {
        if (user == null)
            return null;

        var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier) ?? user.FindFirst("sub");

        if (userIdClaim != null)
            return userIdClaim.Value;

        return null;
    }

    private List<string> GetUserRoles(ClaimsPrincipal? user)
    {
        if (user == null)
            return new();

        var roles = user.FindAll(ClaimTypes.Role);

        return roles.Select(c => c.Value).ToList();
    }
}