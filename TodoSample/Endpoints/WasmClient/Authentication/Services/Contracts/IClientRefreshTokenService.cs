using Honamic.IdentityPlus.Application.Users.Commands;

namespace Honamic.Todo.Endpoints.WasmClient.Authentication.Services.Contracts;

public interface IClientRefreshTokenService
{
    /// <summary>
    ///     Server-side validation url
    /// </summary>
    string? ValidateAccessTokenUrl { set; get; }

    /// <summary>
    ///     The server-side refresh token creator's url
    /// </summary>
    string? RefreshTokenUrl { set; get; }

    /// <summary>
    ///     Client-side and server-side validation of the current token
    /// </summary>
    Task ValidateAndRefreshTokenOnErrorsAsync(HttpRequestMessage request);

    Task<BererTokenResult?> ValidateAndRefreshTokenOnStartupAsync();

    void SetAccessTokenHeader(HttpRequestMessage? request, string? token);

    Task<BererTokenResult?> TryRefreshTokenAsync();

    Task<bool> IsAccessTokenStillValidAsync();
}