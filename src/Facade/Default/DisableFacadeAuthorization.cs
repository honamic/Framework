namespace Honamic.Framework.Facade;

internal class DisableFacadeAuthorization : IFacadeAuthorization
{
    public bool HaveAccess(string permission)
    {
        return true;
    }

    public bool IsAuthenticated()
    {
        return true;
    }
}
