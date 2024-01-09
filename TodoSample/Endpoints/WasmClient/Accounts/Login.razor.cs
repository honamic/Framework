using Honamic.IdentityPlus.Application.Users.Commands;
using Honamic.Todo.Endpoints.WasmClient.Authentication.Components;
using Honamic.Todo.Endpoints.WasmClient.Authentication.HttpClients;
using Honamic.Todo.Endpoints.WasmClient.Authentication.Services.Contracts;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;

namespace Honamic.Todo.Endpoints.WasmClient.Accounts;
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
        UserName = "mohammadi4net@gmail.com",
        Password = "Iman@2951"
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