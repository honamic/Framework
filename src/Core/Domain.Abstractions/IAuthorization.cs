namespace Honamic.Framework.Domain;

public interface IAuthorization
{
    Task<bool> HaveRoleAsync(string roleName);

    Task<bool> HavePermissionAsync(string permission, string? module = null);

    bool IsAuthenticated();
}
