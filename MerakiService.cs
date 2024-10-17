using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;


namespace BBIReporting
{
public class MerakiService : IMerakiService
{
    private readonly IMerakiAPIClient _merakiApiClient;
    private readonly ILogger<MerakiService> _log;

    public MerakiService(IMerakiAPIClient merakiApiClient, ILogger<MerakiService> log)  
    {
        _merakiApiClient = merakiApiClient;
        _log = log;
    }


    public async Task<List<string>> GetAndroidDevicesURLsAsync(string apiOrgId, double merakiTimespan)
    {
        _log.LogDebug($"Retrieving Meraki orgId: {apiOrgId}");

        // Call the API client to get the list of networks
        string networksUrl = $"https://api.meraki.com/api/v1/organizations/{apiOrgId}/networks?perPage=5000";
        var networksResponse = await _merakiApiClient.GetAsync(networksUrl);
        if (string.IsNullOrEmpty(networksResponse))
        {
            _log.LogError("Failed to retrieve networks from Meraki API.");
            return new List<string>();
        }

        _log.LogDebug($"Received networks: {networksResponse}");

        // Deserialize JSON response into a JArray
        var networkArray = JArray.Parse(networksResponse);
        var urlList = new List<string>();

        foreach (var network in networkArray)
        {
            try
            {
                // Extract product types and skip switch-only networks
                var productTypesArray = (JArray)network["productTypes"];
                var productTypes = productTypesArray.ToObject<string[]>();

                if (!(productTypes.Contains("switch") && !productTypes.Contains("wireless")))
                {
                    string networkId = (string)network["id"];
                    string networkUrl = $"https://api.meraki.com/api/v1/networks/{networkId}/clients?timespan={merakiTimespan}&perPage=5000&vlan=4";
                    urlList.Add(networkUrl);
                    _log.LogInformation($"Added URL: {networkUrl}");
                }
            }
            catch (Exception ex)
            {
                _log.LogError($"Error processing network: {ex.Message}\n{ex.StackTrace}");
            }
        }

        _log.LogInformation($"Returning {urlList.Count} URLs for network clients.");
        return urlList;
    }



  public async Task<List<string>> ProcessMerakiUrlsAsync(List<string> urls, IProgress<int> progress)
    {
        int urlCount = 0;
        _log.LogInformation("Started processing {UrlCount} Meraki API URLs...", urls.Count);

        var returnVal = new List<string>();
        int maxConcurrentRequests = 6;
        var semaphore = new SemaphoreSlim(maxConcurrentRequests);
        var tasks = new List<Task>();

        object lockObject = new object();  // Lock for thread safety

        foreach (string url in urls)
        {
            tasks.Add(Task.Run(async () =>
            {
                await semaphore.WaitAsync();
                try
                {
                    _log.LogInformation("Processing URL: {0}", url);

                    var response = await _merakiApiClient.ProcessUrlAsync(url);

                    // Thread-safe increment and progress reporting
                    lock (lockObject)
                    {
                        urlCount++;
                        // Update every 5 URLs
                        if (urlCount % 5 == 0)
                        {
                            progress.Report(urlCount);
                        }
                    }

                    // Store the response
                    lock (returnVal)
                    {
                        returnVal.Add(response);
                    }
                }
                catch (Exception ex)
                {
                    _log.LogError("Error processing URL {0}: {1}", url, ex.Message);
                }
                finally
                {
                    semaphore.Release();
                }
            }));
        }

        await Task.WhenAll(tasks);
        return returnVal;
    }
}

}