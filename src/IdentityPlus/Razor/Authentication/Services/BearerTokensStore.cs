using Blazored.LocalStorage;
using Honamic.IdentityPlus.Application.Accounts.Commands;
using Honamic.IdentityPlus.Razor.Authentication.Services.Contracts;

namespace Honamic.IdentityPlus.Razor.Authentication.Services;

public class BearerTokensStore : IBearerTokensStore
{
    private readonly ILocalStorageService _localStorage;
    private readonly ILogger<BearerTokensStore> _logger;
    private const string StoreKey = "BearerToken";
    private const string DebugStoreKey = "DebugBearerToken";
    public BearerTokensStore(
        ILocalStorageService localStorage,
        ILogger<BearerTokensStore> logger)
    {
        _localStorage = localStorage ?? throw new ArgumentNullException(nameof(localStorage));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<BererTokenResult?> GetBearerTokenAsync()
    {
        return await _localStorage.GetItemAsync<BererTokenResult?>(StoreKey);
    }

    public async Task RemoveBearerTokenAsync()
    {
        await _localStorage.RemoveItemAsync(StoreKey);
    }

    public async Task StoreAllTokensAsync(BererTokenResult? tokenResult)
    {
        if (tokenResult is null)
        {
            return;
        }

        if (tokenResult.CreateOn is null)
        {
            tokenResult.CreateOn = DateTimeOffset.Now;
        }

        await _localStorage.SetItemAsync(StoreKey, tokenResult);
        await _localStorage.SetItemAsync(DebugStoreKey, new { StoreTime = DateTimeOffset.Now });
    }

}