using Honamic.IdentityPlus.Application.Users.Commands;
using Honamic.IdentityPlus.Razor.Authentication.Components;
using Honamic.IdentityPlus.Razor.Authentication.HttpClients;
using Honamic.IdentityPlus.Razor.Authentication.Services.Contracts;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;

namespace Honamic.IdentityPlus.Razor.Accounts;
public partial class Login
{
    private string _title = "Sign In";
    private MudForm? _form;
    private bool _success;
    private bool _loading;
    [Inject] public AuthenticationStateProvider AuthenticationStateProvider { get; set; } = null!;
    [Inject] private IHttpClientService HttpClientService { set; get; } = default!;

    [Inject] private IBearerTokensStore BearerTokensStore { set; get; } = default!;

    [Inject] private NavigationManager NavigationManager { set; get; } = default!;

    [Inject] private AuthenticationStateProvider AuthStateProvider { set; get; } = default!;

    [Inject] private IClientRefreshTokenTimer RefreshTokenTimer { set; get; } = default!;

    [Parameter, SupplyParameterFromQuery] public string? ReturnUrl { set; get; }

    private readonly LoginCommand _model = new()
    {
        UserName = "",
        Password = ""
    };

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        _title = L["Sign In"];
    }
    private async Task LoginUserAsync()
    {

    }

    private void RedirectAfterLogin()
    {
        NavigationManager.NavigateTo(string.IsNullOrEmpty(ReturnUrl) ? "" : $"{ReturnUrl}");
    }


    private async Task OnSubmit()
    {
        try
        {
            _loading = true;
            await _form!.Validate();

            if (_form!.IsValid)
            {
                var response = await HttpClientService.PostDataAsJsonAsync<BererTokenResult>("api/Account/login", _model);
                await BearerTokensStore.StoreAllTokensAsync(response);
                await ((ClientAuthenticationStateProvider)AuthStateProvider).NotifyUserLoggedInAsync();
                await RefreshTokenTimer.StartRefreshTimerAsync();
                //  RedirectAfterLogin();
            }
        }
        finally
        {
            _loading = false;
        }
    }



    public class LoginFormModel
    {
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public bool RememberMe { get; set; } = false;
    }

}