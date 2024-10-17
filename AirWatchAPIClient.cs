using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using NLog;
namespace BBIReporting
{
public class AirWatchAPIClient : IAirWatchAPIClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<AirWatchAPIClient> _log;
    private readonly string _baseUrl;

    public AirWatchAPIClient(HttpClient httpClient, string baseUrl, ILogger<AirWatchAPIClient> log)
    {
        _httpClient = httpClient;
        _baseUrl = baseUrl;
        _log = log;
    }

    public async Task<List<string>> GetAndroidDevicesByOrgAsString(string orgId)
    {
        List<string> returnVal = new List<string>();
        int requestPage = 0;
        string url = $"{_baseUrl}/API/mdm/devices/extensivesearch?organizationgroupid={orgId}&platform=Android";
        
        try
        {
            // Use HttpClient to fetch data asynchronously
            string response = await _httpClient.GetStringAsync(url);

            if (!string.IsNullOrEmpty(response))
            {
                returnVal.Add(response);

                // Use Newtonsoft.Json to parse the response
                JObject jsonObject = JObject.Parse(response);
                
                int pageSize = (int)jsonObject["PageSize"];
                int total = (int)jsonObject["Total"];
                int numOfPages = (int)Math.Ceiling((double)total / pageSize);

                // Fetch subsequent pages if needed
                while (++requestPage < numOfPages)
                {
                    string nextUrl = $"{url}&page={requestPage}";
                    string pageResponse = await _httpClient.GetStringAsync(nextUrl);
                    returnVal.Add(pageResponse);
                }
            }
        }
        catch (Exception ex)
        {
            _log.LogError($"Error fetching data from AirWatch: {ex.Message}");
        }

        return returnVal;
    }

        public Task<string> SearchDevicesAsync(string modelIdentifier, string deviceType)
        {
            throw new NotImplementedException();
        }
    }
}
