using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using Honamic.IdentityPlus.Razor.Authentication.Services.Contracts;
using Honamic.IdentityPlus.Razor.Authentication.Components;
using Honamic.IdentityPlus.Razor.Authentication.HttpClients;

namespace Honamic.IdentityPlus.Razor.Accounts;
public partial class Logout
{
    [Inject] private NavigationManager NavigationManager { set; get; } = default!;

    [Inject] private IBearerTokensStore BearerTokensStore { set; get; } = default!;

    [Inject] private AuthenticationStateProvider AuthStateProvider { set; get; } = default!;

    [Inject] private IHttpClientService HttpClientService { set; get; } = default!;

    [Inject] private IClientRefreshTokenTimer RefreshTokenTimer { set; get; } = default!;

    protected override async Task OnInitializedAsync()
    {
        var response = await HttpClientService.PostDataAsJsonAsync<bool?>("api/account/Logout", new object());
        if (response == true)
        {
            await BearerTokensStore.RemoveBearerTokenAsync();
            await RefreshTokenTimer.StopRefreshTimerAsync();
            ((ClientAuthenticationStateProvider)AuthStateProvider).NotifyUserLogout();
            NavigationManager.NavigateTo("");
        }
    }
}