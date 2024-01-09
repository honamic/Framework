using System.Security.Claims;
using Honamic.IdentityPlus.Application.Users.Commands;
using Honamic.IdentityPlus.Application.Users.Queries;
using Honamic.IdentityPlus.Razor.Authentication.HttpClients;
using Honamic.IdentityPlus.Razor.Authentication.Services.Contracts;
using Microsoft.AspNetCore.Components.Authorization;

namespace Honamic.IdentityPlus.Razor.Authentication.Components;


public class ClientAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly IBearerTokensStore _bearerTokensStore;
    private readonly ILogger<ClientAuthenticationStateProvider> _logger;
    private readonly IClientRefreshTokenService _refreshTokenService;
    private readonly IClientRefreshTokenTimer _refreshTokenTimer;
    private readonly IHttpClientService httpClientService;

    public ClientAuthenticationStateProvider(
        IClientRefreshTokenService refreshTokenService,
        IBearerTokensStore bearerTokensStore,
        IClientRefreshTokenTimer refreshTokenTimer,
        IHttpClientService httpClientService,
        ILogger<ClientAuthenticationStateProvider> logger)
    {
        _refreshTokenService = refreshTokenService ?? throw new ArgumentNullException(nameof(refreshTokenService));
        _bearerTokensStore = bearerTokensStore ?? throw new ArgumentNullException(nameof(bearerTokensStore));
        _refreshTokenTimer = refreshTokenTimer ?? throw new ArgumentNullException(nameof(refreshTokenTimer));
        this.httpClientService = httpClientService;
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }


    public override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        _logger.LogInformation("Get authentication state.");
        return GetCurrentAuthenticationStateAsync(true);
    }

    public async Task NotifyUserLoggedInAsync()
    {
        var authState = Task.FromResult(await GetCurrentAuthenticationStateAsync(false));
        NotifyAuthenticationStateChanged(authState);
    }


    public void NotifyUserLogout()
    {
        var authState = Task.FromResult(GetEmptyAuthenticationState());
        NotifyAuthenticationStateChanged(authState);
    }

    private async Task<AuthenticationState> GetCurrentAuthenticationStateAsync(bool validateTokens)
    {
        BererTokenResult? bererToken;
        if (validateTokens)
        {
            bererToken = await _refreshTokenService.ValidateAndRefreshTokenOnStartupAsync();
            if (bererToken is not null)
            {
                await _refreshTokenTimer.StartRefreshTimerAsync();
            }
        }
        else
        {
            bererToken = await _bearerTokensStore.GetBearerTokenAsync();
        }

        if (bererToken is null)
        {
            return GetEmptyAuthenticationState();
        }

        var claimValues = await httpClientService.GetDataAsJsonAsync<List<ClaimValue>>("/api/Account/MyClientClaims");

        var claims = claimValues.Select(claimValue => new Claim(claimValue.Type, claimValue.Value)).ToArray();

        return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(claims,
                authenticationType: nameof(ClientAuthenticationStateProvider))));

    }

    private static AuthenticationState GetEmptyAuthenticationState()
    {
        return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
    }
}