using Castle.DynamicProxy;
using Honamic.Framework.Facade.Exceptions;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace Honamic.Framework.Facade.Interceptors;

internal class AuthorizeInterceptor : IInterceptor
{
    private readonly ILogger<AuthorizeInterceptor> _logger;
    private readonly IFacadeAuthorization _facadeAuthorization;

    public AuthorizeInterceptor(ILogger<AuthorizeInterceptor> logger,
        IFacadeAuthorization facadeAuthorization)
    {
        _logger = logger;
        _facadeAuthorization = facadeAuthorization;
    }
    public void Intercept(IInvocation invocation)
    {
        _logger.LogTrace($"Authorizing method {invocation.TargetType}.{invocation.Method.Name}.");

        Authorize(invocation);

        invocation.Proceed();
    }

    private void Authorize(IInvocation invocation)
    {
        var classDynamicAuthorize = invocation.TargetType
            .GetCustomAttribute<FacadeDynamicAuthorizeAttribute>();

        var methodDynamicAuthorize = invocation.MethodInvocationTarget
            .GetCustomAttribute<FacadeDynamicAuthorizeAttribute>();

        if (classDynamicAuthorize is null
            && methodDynamicAuthorize is null)
        {
            Authentication(invocation);
            return;
        }

        if (methodDynamicAuthorize is null)
        {
            var AllowAnonymous = invocation.MethodInvocationTarget
                         .GetCustomAttribute<FacadeAllowAnonymousAttribute>();
            if (AllowAnonymous is not null)
            {
                return;
            }
        }

        string permission = CalculatePermissionName(invocation.Method);

        if (!_facadeAuthorization.IsAuthenticated())
        {
            throw new UnauthenticatedException();
        }

        if (!_facadeAuthorization.HaveAccess(permission))
        {
            throw new UnauthorizedException(permission);
        }
    }

    private void Authentication(IInvocation invocation)
    {
        var classAuthorize = invocation.TargetType
            .GetCustomAttribute<FacadeAuthorizeAttribute>();

        var methodAuthorize = invocation.Method
            .GetCustomAttribute<FacadeAuthorizeAttribute>();

        if (classAuthorize is null
            && methodAuthorize is null)
        {
            return;
        }

        if (methodAuthorize is null)
        {
            var AllowAnonymous = invocation.Method
                         .GetCustomAttribute<FacadeAllowAnonymousAttribute>();
            if (AllowAnonymous is not null)
            {
                return;
            }
        }

        if (!_facadeAuthorization.IsAuthenticated())
        {
            throw new UnauthenticatedException();
        }
    }

    private static string CalculatePermissionName(MethodInfo method)
    {
        return method.Name;
    }
}