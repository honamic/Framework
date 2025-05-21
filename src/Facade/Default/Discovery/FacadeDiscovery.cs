using Honamic.Framework.Applications.Results;
using Honamic.Framework.Utilities.Extensions;
using System.Reflection;

namespace Honamic.Framework.Facade.Discovery;

public static class FacadeDiscovery
{
    private static List<Type> FacadeTypes = new List<Type>();
    internal static void RegisterFacadeType<TImplementation>() where TImplementation : BaseFacade, IBaseFacade
    {
        FacadeTypes.Add(typeof(TImplementation));
    }

    public static List<string> GetFacadeNames()
    {
        return FacadeTypes.Select(t => t.FullName ?? t.Name).ToList();
    }

    public static List<FacadeDiscoveryInfo> GetFacadesInformation()
    {
        var result = new List<FacadeDiscoveryInfo>();

        foreach (var facadeType in FacadeTypes)
        {
            var discoveryInfo = new FacadeDiscoveryInfo()
            {
                DisplayName = facadeType.GetDisplayName(),
                Name = facadeType.Name,
                FullName = facadeType.FullName ?? facadeType.Name
            };

            foreach (var method in
                facadeType.GetMethods(BindingFlags.Public | BindingFlags.Instance))
            {
                if (IsReturnResult(method))
                {
                    discoveryInfo.Methods.Add(new FacadeDiscoveryMethodInfo
                    {
                        DisplayName = method.GetDisplayName(),
                        Name = method.Name,
                    });
                }
            }

            result.Add(discoveryInfo);
        }

        return result;

    }

    private static bool IsReturnResult(MethodInfo method)
    {
        if (method.ReturnType == typeof(Result))
            return true;

        if (method.ReturnType == typeof(Result<>))
            return true;


        if (method.ReturnType.IsGenericType
            && method.ReturnType.GetGenericTypeDefinition() == typeof(Task<>))
        {
            if (method.ReturnType.GenericTypeArguments[0] == typeof(Result))
            {
                return true;
            }

            if (method.ReturnType.GenericTypeArguments[0]
                .GetGenericTypeDefinition() == typeof(Result<>))
            {
                return true;
            }
        }
        return false;
    }

}