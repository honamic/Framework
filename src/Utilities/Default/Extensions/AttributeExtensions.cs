using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Honamic.Framework.Utilities.Extensions;

public static class AttributeExtensions
{
    public static string GetDisplayName(this MemberInfo type)
    {
        var displayNameAttribute = type
            .GetCustomAttributes<DisplayNameAttribute>(true)
            .Cast<DisplayNameAttribute>()
            .FirstOrDefault();

        if (displayNameAttribute != null)
        {
            return displayNameAttribute.DisplayName;
        }

        var displayAttribute = type
            .GetCustomAttributes<DisplayAttribute>(true)
            .Cast<DisplayAttribute>()
            .FirstOrDefault();

        if (displayAttribute != null && displayAttribute.Name is not null)
        {
            return displayAttribute.Name;
        }

        return type.Name.SeparateCamelCase();
    }

}
