namespace Honamic.Framework.Applications.Authorizes;

public interface IAuthorization
{
    Task<bool> HaveAccessAsync(string permission);

    bool IsAuthenticated();
}
