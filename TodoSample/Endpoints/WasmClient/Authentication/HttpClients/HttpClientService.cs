using System.Net.Http.Json;
using System.Text.Json;

namespace Honamic.Todo.Endpoints.WasmClient.Authentication.HttpClients;

/// <summary>
///     A custom  HttpClient Service
/// </summary>
public class HttpClientService : IHttpClientService
{
    private readonly HttpClient _httpClient;

    /// <summary>
    ///     A custom  HttpClient Service
    /// </summary>
    public HttpClientService(HttpClient httpClient)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }

    /// <summary>
    ///     Posts data as json to the endpoint
    /// </summary>
    public async Task<TResult?> PostDataAsJsonAsync<TResult>(string requestUri, object? value = null, bool ensureSuccessStatus = true)
    {
        var response = await _httpClient.PostAsJsonAsync(GetUri(requestUri), value);

        if (ensureSuccessStatus)
        {
            response.EnsureSuccessStatusCode();
        }
        var responseContent = await response.Content.ReadAsStringAsync();

        if (string.IsNullOrEmpty(responseContent))
        {
            return default;
        }

        return JsonSerializer.Deserialize<TResult?>(responseContent,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
    }


    /// <summary>
    ///     Posts data as json to the endpoint
    /// </summary>
    public async Task<TResult?> GetDataAsJsonAsync<TResult>(string requestUri)
    {
        return await _httpClient.GetFromJsonAsync<TResult?>(GetUri(requestUri));
    }

    private Uri GetUri(string requestUri)
    {
        if (requestUri == null)
        {
            throw new ArgumentNullException(nameof(requestUri));
        }

        var baseAddress = _httpClient.BaseAddress;
        if (baseAddress is null)
        {
            throw new InvalidOperationException("`baseAddress` is null.");
        }

        return new Uri(baseAddress, requestUri);
    }
}