using Honamic.IdentityPlus.Application.Accounts.Commands;

namespace Honamic.IdentityPlus.Razor.Authentication.Services.Contracts;

public interface IBearerTokensStore
{
    Task<BererTokenResult?> GetBearerTokenAsync();

    Task StoreAllTokensAsync(BererTokenResult? tokenResult);

    Task RemoveBearerTokenAsync();

}