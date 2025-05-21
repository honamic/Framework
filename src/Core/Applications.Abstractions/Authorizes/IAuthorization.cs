namespace Honamic.Framework.Applications.Authorizes;

public interface IAuthorization
{
    [Obsolete("Use HaveAccessAsync instead. This method will be removed in future versions.")]
    bool HaveAccess(string permission);

    Task<bool> HaveAccessAsync(string permission);

    bool IsAuthenticated();
}
