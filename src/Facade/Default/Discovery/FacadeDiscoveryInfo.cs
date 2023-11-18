namespace Honamic.Framework.Facade.Discovery;

public class FacadeDiscoveryInfo
{
    public FacadeDiscoveryInfo()
    {
        Methods = new List<FacadeDiscoveryMethodInfo>();
    }

    public required string FullName { get; set; }
    public required string Name { get; set; }
    public required string DisplayName { get; set; }

    public List<FacadeDiscoveryMethodInfo> Methods { get; }
}