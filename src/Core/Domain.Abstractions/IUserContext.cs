namespace Honamic.Framework.Domain;

public interface IUserContext
{
    long? GetCurrentUserIdAsNumber();

    string? GetCurrentUserId();

    string? GetCurrentUserEmail();

    string? GetCurrentUsername();

    string? GetCurrentUserMobile();

    Task<List<string>> GetCurrentUserRolesAsync();
}