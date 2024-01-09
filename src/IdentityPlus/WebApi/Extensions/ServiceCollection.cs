using Microsoft.Extensions.DependencyInjection;
using Honamic.IdentityPlus.Application.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;
using Honamic.IdentityPlus.Application;
using Microsoft.AspNetCore.Components.Authorization;
using Honamic.IdentityPlus.Razor;
using Honamic.IdentityPlus.WebApi.Components;
using Microsoft.AspNetCore.Mvc.ApplicationParts;

namespace Honamic.IdentityPlus.WebApi.Extensions;
public static class ServiceCollection
{

    public static IServiceCollection AddIdentityPlusApiEndpoint(this IServiceCollection services)
    {
        return services.AddIdentityPlusApiEndpoint(_ => { });
    }

    public static IServiceCollection AddIdentityPlusApiEndpoint(this IServiceCollection services,
        Action<IdentityPlusOptions> configure)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configure);

        services.AddControllers()
                        .PartManager.ApplicationParts.Add(new AssemblyPart(typeof(ServiceCollection).Assembly));

        // blazor
        services.AddCascadingAuthenticationState();
        services.AddScoped<IdentityUserAccessor>();
        services.AddScoped<IdentityRedirectManager>();
        services.AddScoped<AuthenticationStateProvider, PersistingServerAuthenticationStateProvider>();
        // end blazor

        services.AddAuthorization();
        services
            .AddAuthentication(IdentityPlusConstants.BearerAndApplicationScheme)
            .AddScheme<AuthenticationSchemeOptions, CompositeIdentityHandler>(IdentityPlusConstants.BearerAndApplicationScheme, null, compositeOptions =>
            {
                compositeOptions.ForwardDefault = IdentityConstants.BearerScheme;
                compositeOptions.ForwardAuthenticate = IdentityPlusConstants.BearerAndApplicationScheme;
            })
            .AddBearerToken(IdentityConstants.BearerScheme, opt =>
            {
                opt.ClaimsIssuer = "Honamic.Dev";
                opt.BearerTokenExpiration = TimeSpan.FromHours(1);
            })
            //.AddJwtBearer(options =>
            //{
            //    options.Authority = oktaSettings.Authority;
            //    options.Audience = "api://default";
            //    options.RequireHttpsMetadata = false;
            //    options.TokenValidationParameters = new TokenValidationParameters
            //    {
            //        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(oktaSettings.ClientSecret)),
            //        RequireSignedTokens = true,
            //        ClockSkew = TimeSpan.FromMinutes(5),

            //        RequireExpirationTime = true,
            //        ValidateLifetime = true,

            //        ValidateAudience = true,
            //        ValidAudience = "api://default",

            //        ValidateIssuer = true,
            //        ValidIssuer = oktaSettings.Authority

            //    };
            //})
            .AddIdentityCookies();

        if (IdentityPlusApplicationServiceCollection
            .IdentityBuilder is null)
        {
            services.AddIdentityPlusApplication(configure);
        }

        IdentityPlusApplicationServiceCollection
            .IdentityBuilder?.AddApiEndpoints();

        IdentityPlusApplicationServiceCollection
            .IdentityBuilder?.AddSignInManager<IdentityPlusSignInManager>();

        return services;
    }


    private sealed class CompositeIdentityHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder)
    : SignInAuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder)
    {
        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var bearerResult = await Context.AuthenticateAsync(IdentityConstants.BearerScheme);

            // Only try to authenticate with the application cookie if there is no bearer token.
            if (!bearerResult.None)
            {
                return bearerResult;
            }

            // Cookie auth will return AuthenticateResult.NoResult() like bearer auth just did if there is no cookie.
            return await Context.AuthenticateAsync(IdentityConstants.ApplicationScheme);
        }

        protected override Task HandleSignInAsync(ClaimsPrincipal user, AuthenticationProperties? properties)
        {
            throw new NotImplementedException();
        }

        protected override Task HandleSignOutAsync(AuthenticationProperties? properties)
        {
            throw new NotImplementedException();
        }
    }
}
