using Honamic.IdentityPlus.Application.Users.Commands;

namespace Honamic.IdentityPlus.Razor.Authentication.Services.Contracts;

public interface IBearerTokensStore
{
    Task<BererTokenResult?> GetBearerTokenAsync();

    Task StoreAllTokensAsync(BererTokenResult? tokenResult);

    Task RemoveBearerTokenAsync();

}