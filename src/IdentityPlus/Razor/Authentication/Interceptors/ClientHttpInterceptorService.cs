using Honamic.IdentityPlus.Razor.Authentication.Services.Contracts;
using System.Net.Http.Headers;

namespace Honamic.IdentityPlus.Razor.Authentication.Interceptors;


public class ClientHttpInterceptorService : DelegatingHandler
{
    private readonly IBearerTokensStore _bearerTokensStore;


    public ClientHttpInterceptorService(IBearerTokensStore bearerTokensStore)
    {
        _bearerTokensStore = bearerTokensStore ?? throw new ArgumentNullException(nameof(bearerTokensStore));
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        await AddAccessTokenToAllRequestsAsync(request);

        var response = await base.SendAsync(request, cancellationToken);
        return response;
    }

    private async Task AddAccessTokenToAllRequestsAsync(HttpRequestMessage request)
    {
        var tokenInfo = await _bearerTokensStore.GetBearerTokenAsync();
        request.Headers.Authorization =
            tokenInfo is not null ? new AuthenticationHeaderValue("Bearer", tokenInfo.AccessToken) : null;
    }
}