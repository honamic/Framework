using Honamic.Todo.Endpoints.WasmClient.Authentication.Services.Contracts;
using Honamic.Todo.Endpoints.WasmClient.Authentication.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using BlazorWasmDynamicPermissions.Client.ClientAuthorization.Services;
using Polly;
using Blazored.LocalStorage;
using Honamic.Todo.Endpoints.WasmClient.Authentication.Interceptors;
using Honamic.Todo.Endpoints.WasmClient.Authentication.HttpClients;
using Honamic.Todo.Endpoints.WasmClient.Authentication.Components;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState();



builder.Services.AddScoped<IBearerTokensStore, BearerTokensStore>();
builder.Services.AddScoped<IClientRefreshTokenTimer, ClientRefreshTokenTimer>();
builder.Services.AddScoped<IClientRefreshTokenService, ClientRefreshTokenService>();
builder.Services.AddScoped<IHttpClientService, HttpClientService>();

builder.Services.AddScoped<AuthenticationStateProvider, ClientAuthenticationStateProvider>();

builder.Services.AddScoped<ClientHttpInterceptorService>();
builder.Services.AddHttpClient(
        "ServerAPI",
        client =>
        {
            client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress);
            client.DefaultRequestHeaders.Add("User-Agent", "Client 1.0");
            client.Timeout = TimeSpan.FromMinutes(20);
        }
    )
    .AddHttpMessageHandler<ClientHttpInterceptorService>()
    // transient errors: network failures and HTTP 5xx/InternalServerError and HTTP 408/Timeout errors
    .AddTransientHttpErrorPolicy(policy => policy.WaitAndRetryAsync(new[] { TimeSpan.FromSeconds(5) }))
    .AddPolicyHandler((serviceProvider, request) =>
        ClientRefreshTokenRetryPolicy.RefreshToken(serviceProvider, request));

builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("ServerAPI"));

builder.Services.AddBlazoredLocalStorage();
builder.Services.AddLocalization();
builder.Services.AddMudServices();




await builder.Build().RunAsync();
