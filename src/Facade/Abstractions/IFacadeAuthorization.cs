namespace Honamic.Framework.Facade;

public interface IFacadeAuthorization
{
    bool HaveAccess(string permission);
    
    bool IsAuthenticated();
}
