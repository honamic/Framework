namespace Honamic.IdentityPlus.Razor.Authentication.HttpClients;

/// <summary>
///     A custom  HttpClient Service
/// </summary>
public interface IHttpClientService
{
    /// <summary>
    ///     Posts data as json to the endpoint
    /// </summary>
    Task<TResult?> PostDataAsJsonAsync<TResult>(string requestUri, object? value = null, bool ensureSuccessStatus = true);

    /// <summary>
    ///     Posts data as json to the endpoint
    /// </summary>
    Task<TResult?> GetDataAsJsonAsync<TResult>(string requestUri);
}