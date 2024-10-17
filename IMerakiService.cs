using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BBIReporting
{
    /// <summary>
    /// IMerakiService (Business Logic Layer)
    /// A higher-level service that coordinates how Meraki data is fetched and processed within the application.
    /// Uses IMerakiAPIClient to interact with the API but abstracts away the low-level details from the rest of the application.
    /// It might handle retries, complex data manipulation, caching, or transforming data into a format that the application can use.
    /// </summary>
    public interface IMerakiService
    {
        /// <summary>
        /// Retrieves a list of URLs for Android devices from the Meraki API.
        /// </summary>
        /// <param name="apiKey">The API key for authenticating with the Meraki API.</param>
        /// <param name="orgId">The organization ID from which to fetch the Android device URLs.</param>
        /// <param name="timeSpan">The time span (in seconds) for which the API should retrieve recent devices.</param>
        /// <returns>A task representing the asynchronous operation, containing a list of Android device URLs as strings.</returns>
        Task<List<string>> GetAndroidDevicesURLsAsync(string orgId, double timeSpan);
        Task<List<string>> ProcessMerakiUrlsAsync(List<string> urls, IProgress<int> progress);
  

    }
}