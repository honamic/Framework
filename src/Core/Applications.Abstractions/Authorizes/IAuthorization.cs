namespace Honamic.Framework.Applications.Authorizes;

public interface IAuthorization
{
    Task<bool> HaveAccessAsync(string permission);
    
    Task<bool> HavePermissionAsync(string permission, string? module);

    bool IsAuthenticated();
}
