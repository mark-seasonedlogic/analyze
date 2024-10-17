using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace BBIReporting
{
	public abstract class BaseAPIClient
{
    protected readonly HttpClient _httpClient;

    protected BaseAPIClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    // Sets authorization header (can be overridden for custom auth headers)
    protected virtual void SetAuthorizationHeader(string token)
    {
        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
    }

    // Basic GET request
    protected async Task<string> GetAsync(string url)
    {
        try
        {
            HttpResponseMessage response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();  // Throw if not 2xx
            return await response.Content.ReadAsStringAsync();
        }
        catch (HttpRequestException ex)
        {
            // Log the error and rethrow or handle accordingly
            throw new Exception($"Request failed for URL {url}: {ex.Message}");
        }
    }

    // Basic POST request
    protected async Task<string> PostAsync(string url, HttpContent content)
    {
        try
        {
            HttpResponseMessage response = await _httpClient.PostAsync(url, content);
            response.EnsureSuccessStatusCode();  // Throw if not 2xx
            return await response.Content.ReadAsStringAsync();
        }
        catch (HttpRequestException ex)
        {
            throw new Exception($"Request failed for URL {url}: {ex.Message}");
        }
    }
}
}
