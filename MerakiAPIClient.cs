using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace BBIReporting
{
    public class MerakiAPIClient : IMerakiAPIClient
    {
        private readonly string _apiKey;
        private readonly ILogger<MerakiAPIClient> _log;
        private readonly HttpClient _httpClient;
        public MerakiAPIClient(HttpClient httpClient, string apiKey, ILogger<MerakiAPIClient> log)
        {
            _apiKey = apiKey;
            _httpClient = httpClient;
            _log = log;

            // Set global authorization header from the environment variable
            _httpClient.DefaultRequestHeaders.Add("X-Cisco-Meraki-API-Key", _apiKey);
            _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

        }

        // Override authorization to set the API key
        public void SetAuthorizationHeader(string token)
        {
            _httpClient.DefaultRequestHeaders.Add("X-Cisco-Meraki-API-Key", _apiKey);
        }

        public async Task<string> GetDevicesFromNetworkAsync(string networkId)
        {
            string url = $"https://api.meraki.com/api/v1/networks/{networkId}/devices";
            var response = await _httpClient.GetAsync(url);

            // Ensure success and get the response content
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        public List<string> GetAndroidDevicesURLs(string apiKey, string orgId, double timeSpan)
        {
            throw new NotImplementedException();
        }
        public async Task<string> ProcessUrlAsync(string url)
        {
            string apiKey = Environment.GetEnvironmentVariable("BBIMerakiKey") ?? string.Empty;

            if (string.IsNullOrEmpty(apiKey))
            {
                _log.LogError("Meraki API key is missing from environment variables.");
                throw new InvalidOperationException("Meraki API key is missing.");
            }

            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Add("X-Cisco-Meraki-API-Key", apiKey);
            request.Headers.Add("Accept", "application/json");

            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                _log.LogError("Failed to retrieve URL: {0} - Status: {1}", url, response.StatusCode);
                throw new HttpRequestException($"Failed to retrieve URL: {url}");
            }

            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> GetAsync(string url)
        {
            var response = await _httpClient.GetAsync(url);
            // Ensure success and get the response content
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }


        // Other Meraki-specific methods...
    }
}
