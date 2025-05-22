using Honamic.Framework.Applications.Authorizes;

namespace Honamic.Framework.Facade;

internal class DisableAuthorization : IAuthorization
{
    public bool HaveAccess(string permission)
    {
        return true;
    }

    public Task<bool> HaveAccessAsync(string permission)
    {
        return Task.FromResult(true);
    }

    public bool IsAuthenticated()
    {
        return true;
    }
}
