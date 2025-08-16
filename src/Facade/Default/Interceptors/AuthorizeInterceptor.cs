using Castle.DynamicProxy;
using Honamic.Framework.Applications.Authorizes;
using Honamic.Framework.Applications.Exceptions;
using Honamic.Framework.Domain;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace Honamic.Framework.Facade.Interceptors;

internal class AuthorizeInterceptor : IInterceptor
{
    private readonly ILogger<AuthorizeInterceptor> _logger;
    private readonly IAuthorization _facadeAuthorization;

    public AuthorizeInterceptor(ILogger<AuthorizeInterceptor> logger,
        IAuthorization facadeAuthorization)
    {
        _logger = logger;
        _facadeAuthorization = facadeAuthorization;
    }
    public void Intercept(IInvocation invocation)
    {
        _logger.LogTrace($"Authorizing method {invocation.TargetType}.{invocation.Method.Name}.");

        Authorize(invocation).GetAwaiter().GetResult();

        invocation.Proceed();
    }

    private async Task Authorize(IInvocation invocation)
    {
        var classDynamicAuthorize = invocation.TargetType
            .GetCustomAttribute<DynamicAuthorizeAttribute>();

        var methodDynamicAuthorize = invocation.MethodInvocationTarget
            .GetCustomAttribute<DynamicAuthorizeAttribute>();

        if (classDynamicAuthorize is null
            && methodDynamicAuthorize is null)
        {
            Authentication(invocation);
            return;
        }

        if (methodDynamicAuthorize is null)
        {
            var AllowAnonymous = invocation.MethodInvocationTarget
                         .GetCustomAttribute<AllowAnonymousAttribute>();
            if (AllowAnonymous is not null)
            {
                return;
            }
        }

        string permission = CalculatePermissionName(invocation.Method);

        if (!_facadeAuthorization.IsAuthenticated())
        {
            throw new AuthenticationRequiredException();
        }

        if (!await _facadeAuthorization.HavePermissionAsync(permission))
        {
            throw new ForbiddenException(permission);
        }
    }

    private void Authentication(IInvocation invocation)
    {
        var classAuthorize = invocation.TargetType
            .GetCustomAttribute<AuthorizeAttribute>();

        var methodAuthorize = invocation.Method
            .GetCustomAttribute<AuthorizeAttribute>();

        if (classAuthorize is null
            && methodAuthorize is null)
        {
            return;
        }

        if (methodAuthorize is null)
        {
            var AllowAnonymous = invocation.Method
                         .GetCustomAttribute<AllowAnonymousAttribute>();
            if (AllowAnonymous is not null)
            {
                return;
            }
        }

        if (!_facadeAuthorization.IsAuthenticated())
        {
            throw new AuthenticationRequiredException();
        }
    }

    private static string CalculatePermissionName(MethodInfo method)
    {
        return method.Name;
    }
}