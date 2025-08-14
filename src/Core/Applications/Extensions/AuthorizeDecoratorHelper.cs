using Honamic.Framework.Applications.Authorizes;
using Honamic.Framework.Applications.Exceptions;
using Honamic.Framework.Domain;
using System.Reflection;

namespace Honamic.Framework.Applications.Extensions;

internal static class AuthorizeDecoratorHelper
{
    public static async Task AuthorizeAttributes(this IAuthorization authorization, Type type)
    {
        await authorization.AuthorizeWithAttributes(type);

        await authorization.AuthorizeWithDynamicPermissions(type);
    }

    public static async Task AuthorizeWithDynamicPermissions(this IAuthorization authorization, Type type)
    {
        var dynamicAuthorizeAttribute = type.GetCustomAttribute<DynamicPermissionAttribute>();

        if (dynamicAuthorizeAttribute is not null)
        {
            if (!authorization.IsAuthenticated())
            {
                throw new UnauthenticatedException();
            }

            string dynamicPermission = CalculatePermissionName(dynamicAuthorizeAttribute, type);

            if (!await authorization.HavePermissionAsync(dynamicPermission, dynamicAuthorizeAttribute.Module))
            {
                throw new UnauthorizedException(dynamicPermission);
            }
        }
    }

    public static async Task AuthorizeWithAttributes(this IAuthorization authorization, Type type)
    {
        var authorizeAttribute = type.GetCustomAttribute<AuthorizeAttribute>();

        if (authorizeAttribute is not null)
        {
            if (!authorization.IsAuthenticated())
            {
                throw new UnauthenticatedException();
            }

            if (authorizeAttribute.Roles?.Length > 0)
            {
                foreach (var permission in authorizeAttribute.Roles)
                {
                    if (!await authorization.HaveRoleAsync(permission))
                    {
                        throw new UnauthorizedException(permission);
                    }
                }
            }
        }
    }

    private static string CalculatePermissionName(DynamicPermissionAttribute dynamicAuthorizeAttribute, Type type)
    {
        return dynamicAuthorizeAttribute.Name 
            ?? type.FullName 
            ?? throw new InvalidOperationException("Permission key and type full name are both null.");
    }
}