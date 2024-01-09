using Honamic.IdentityPlus.Razor.Authentication.Components;
using Honamic.IdentityPlus.Razor.Authentication.HttpClients;
using Honamic.IdentityPlus.Razor.Authentication.Services.Contracts;
using Honamic.IdentityPlus.Razor.Authentication.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Honamic.IdentityPlus.Razor.Authentication.Interceptors;
using Polly;
using MudBlazor.Services;
using Blazored.LocalStorage;

namespace Honamic.IdentityPlus.Razor.Extentions;
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddIdentityPlus(this IServiceCollection services, string baseAddress)
    {
        services.AddBlazoredLocalStorage();
        services.AddLocalization();
        services.AddMudServices();

        services.AddAuthorizationCore();
        services.AddCascadingAuthenticationState();

        services.AddScoped<IBearerTokensStore, BearerTokensStore>();
        services.AddScoped<IClientRefreshTokenTimer, ClientRefreshTokenTimer>();
        services.AddScoped<IClientRefreshTokenService, ClientRefreshTokenService>();
        services.AddScoped<IHttpClientService, HttpClientService>();

        services.AddScoped<AuthenticationStateProvider, ClientAuthenticationStateProvider>();


        services.AddScoped<ClientHttpInterceptorService>();
        services.AddHttpClient(
                "ServerAPI",
                client =>
                {
                    client.BaseAddress = new Uri(baseAddress);
                    client.DefaultRequestHeaders.Add("User-Agent", "Client 1.0");
                    client.Timeout = TimeSpan.FromMinutes(20);
                }
            )
            .AddHttpMessageHandler<ClientHttpInterceptorService>()
            // transient errors: network failures and HTTP 5xx/InternalServerError and HTTP 408/Timeout errors
            .AddTransientHttpErrorPolicy(policy => policy.WaitAndRetryAsync(new[] { TimeSpan.FromSeconds(5) }))
            .AddPolicyHandler((serviceProvider, request) =>
                ClientRefreshTokenRetryPolicy.RefreshToken(serviceProvider, request));

        services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("ServerAPI"));


        return services;
    }
}
