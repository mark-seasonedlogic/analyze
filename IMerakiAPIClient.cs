using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BBIReporting
{
/// <summary>
/// IMerakiAPIClient (Low-level API Interaction)
/// Directly responsible for making requests to the Meraki API.
/// Handles the specifics of the API such as URL construction, query parameters, and making GET or POST requests.
/// Should be light and focus only on communication with the external service.
/// </summary>
public interface IMerakiAPIClient
{
    /// <summary>
    /// Retrieves URLs of Android devices from Meraki API.
    /// </summary>
    /// <param name="apiKey">The API key for authentication.</param>
    /// <param name="orgId">The organization ID to filter devices.</param>
    /// <param name="timeSpan">The time span to fetch recent devices.</param>
    /// <returns>A list of device URLs.</returns>
    List<string> GetAndroidDevicesURLs(string apiKey, string orgId, double timeSpan);

Task<string> ProcessUrlAsync(string url);
    // Other low-level methods to interact with the Meraki API
    Task<string> GetAsync(string url);
    void SetAuthorizationHeader(string token);
}

}