using Honamic.Todo.Endpoints.WasmClient.Authentication.Services.Contracts;
using Polly;
using Polly.Retry;
using System.Net;

namespace Honamic.Todo.Endpoints.WasmClient.Authentication.Interceptors;

public static class ClientRefreshTokenRetryPolicy
{
    public static AsyncRetryPolicy<HttpResponseMessage> RefreshToken(
        IServiceProvider serviceProvider, HttpRequestMessage request)
    {
        return Policy
            .HandleResult<HttpResponseMessage>(ex =>
               ex.StatusCode is HttpStatusCode.Unauthorized or HttpStatusCode.Forbidden
               && !(request?.RequestUri?.PathAndQuery
                    .StartsWith("/api/account/refresh", StringComparison.InvariantCultureIgnoreCase) ?? false)
                )
            .Or<HttpRequestException>(ex =>
                ex.StatusCode is HttpStatusCode.Unauthorized or HttpStatusCode.Forbidden)
            .WaitAndRetryAsync(
                new[] { TimeSpan.FromSeconds(1) },
                async (response, delay, retryCount, context) =>
                {
                    //#if DEBUG
                    //                    WriteLine(
                    //                        $"Running the `Client RefreshToken Retry Policy` for StatusCode: {response.Result.StatusCode} & Exception: {response.Exception}.");
                    //#endif
                    var refreshTokenService = serviceProvider.GetRequiredService<IClientRefreshTokenService>();
                    await refreshTokenService.ValidateAndRefreshTokenOnErrorsAsync(request);
                });
    }
}