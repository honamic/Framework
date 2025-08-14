using Honamic.Framework.Domain;

namespace Honamic.Framework.Facade;

internal class DisableAuthorization : IAuthorization
{
    public Task<bool> HaveRoleAsync(string roleName)
    {
        return Task.FromResult(true);
    }

    public Task<bool> HavePermissionAsync(string permission, string? module)
    {
        return Task.FromResult(true);
    }

    public bool IsAuthenticated()
    {
        return true;
    }
}
