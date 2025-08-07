using Honamic.Framework.Applications.Authorizes;

namespace Honamic.Framework.Facade;

internal class DisableAuthorization : IAuthorization
{
    public bool HaveAccess(string permission)
    {
        return true;
    }

    [Obsolete("This method is obsolete. Use HavePermissionAsync instead.")]
    public Task<bool> HaveAccessAsync(string permission)
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
