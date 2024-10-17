using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;


namespace BBIReporting
{
public class AirWatchService : IAirWatchService
{
     private readonly Dictionary<string, AndroidDevice> airwatchDevices = new Dictionary<string, AndroidDevice>();

    private readonly IAirWatchAPIClient _airWatchApiClient;
    private readonly ILogger<AirWatchService> _log;
public Dictionary<string,AndroidDevice> GetAirwatchObjectsDetail(List<string> data)
    {
        int deviceCount = 0;
        Dictionary<string,AndroidDevice> androidDevicesDict = new Dictionary<string, AndroidDevice>();
        foreach (string page in data)
        {
          try{

           // Parse the page as a JObject
            JObject pageObject = JObject.Parse(page);

            // Parse the "Devices" array in the response
            foreach (JObject device in JArray.Parse(pageObject["Devices"].ToString()))
            {
                var currDevice = new AndroidDevice();
                deviceCount++;

                // Parse and log essential device information
                int airwatchId = int.Parse(device["DeviceId"].ToString());
                _log.LogDebug($"Device #{deviceCount}:\n\tUDID: {device["Udid"]}\n\tFriendly Name: {device["DeviceFriendlyName"]}\n\tDevice ID: {device["DeviceId"]}");

                try
                {
                    // Get restaurant ID from device friendly name
                    string deviceFriendlyName = device["DeviceFriendlyName"]?.ToString() ?? string.Empty;
                    string restId = string.IsNullOrEmpty(deviceFriendlyName) ? string.Empty : deviceFriendlyName.Substring(0, 7);
                    
                    // Parse date fields
                    string firstSeenDate = device["FirstSeen"]?.ToString() ?? string.Empty;
                    string lastSeenDate = device["LastSeen"]?.ToString() ?? string.Empty;
                    string serialNumber = device["SerialNumber"]?.ToString() ?? string.Empty;

                    // Parse network information
                    JArray networkInfo = (JArray)device["DeviceNetworkInfo"];
                    foreach (JObject attribute in networkInfo)
                    {
                        currDevice.MacAddress = attribute.GetValue("MACAddress")?.ToString();
                        currDevice.IpAddress = attribute.GetValue("IPAddress")?.ToString();
                    }

                    // Set device fields
                    currDevice.Name = deviceFriendlyName;
                    currDevice.SerialNumber = serialNumber;
                    currDevice.AirWatchID = airwatchId;

                    // Parse and set FirstSeen and LastSeen dates
                    if (DateTime.TryParse(firstSeenDate, out DateTime firstSeenDt))
                    {
                        currDevice.FirstSeen = firstSeenDt;
                    }
                    else
                    {
                        _log.LogInformation($"Invalid FirstSeen date detected for device: {device["DeviceFriendlyName"]}");
                    }

                    if (DateTime.TryParse(lastSeenDate, out DateTime lastSeenDt))
                    {
                        currDevice.LastSeen = lastSeenDt;
                    }
                    else
                    {
                        _log.LogInformation($"Invalid LastSeen date detected for device: {device["DeviceFriendlyName"]}");
                    }

                    // Parse custom attributes
                    JArray customAttributes = (JArray)device["CustomAttributes"];
                    foreach (JObject attribute in customAttributes)
                    {
                        string attributeName = attribute["Name"]?.ToString() ?? string.Empty;
                        string attributeValue = attribute["Value"]?.ToString() ?? string.Empty;

                        switch (attributeName)
                        {
                            case "miscellaneous.batteryLevel":
                                currDevice.BatteryLevel = int.Parse(attributeValue);
                                _log.LogDebug($"Battery Level: {currDevice.BatteryLevel}");
                                break;
                            case "memory.totalInternalMemory":
                                currDevice.SpaceInternalTotal = int.Parse(attributeValue);
                                _log.LogDebug($"Total Space: {currDevice.SpaceInternalTotal}");
                                break;
                            case "memory.availableInternalMemory":
                                currDevice.SpaceInternalRemaining = int.Parse(attributeValue);
                                _log.LogDebug($"Remaining Space: {currDevice.SpaceInternalRemaining}");
                                break;
                            default:
                                break;
                        }
                    }

                    // Add device to the collection (airwatchDevices) if not a duplicate
                    androidDevicesDict.Add(currDevice.MacAddress, currDevice);
                }
                catch (ArgumentException argEx)
                {
                    _log.LogError($"This device is a duplicate MAC address: {currDevice.Name}, {currDevice.MacAddress}, {currDevice.SerialNumber}");
                }
                catch (Exception ex)
                {
                    _log.LogError($"Exception processing device: {device}\nMessage: {ex.Message}\nStackTrace: {ex.StackTrace}");
                }
            }
          }
          catch(Exception ex)
          {
            _log.LogError(ex,"An exception occurred parsing a page {0}",page);
            continue;
          }
        }
        return androidDevicesDict;
        
    }
    public AirWatchService(IAirWatchAPIClient airWatchApiClient, ILogger<AirWatchService> log)
    {
        _airWatchApiClient = airWatchApiClient;
        _log = log;
    }

        public Task<List<string>> GetAirWatchDevicesByOrgAsync(string orgId)
        {
            throw new System.NotImplementedException();
        }

        public async Task<List<string>> GetAndroidDevicesByOrgAsync(string orgId)
    {
        try
        {
            var devices = await _airWatchApiClient.GetAndroidDevicesByOrgAsString(orgId);
            _log.LogDebug($"Successfully fetched {devices.Count} pages of devices.");
            return devices;
        }
        catch (Exception ex)
        {
            _log.LogError($"Error fetching Android devices: {ex.Message}");
            return new List<string>(); // Return an empty list in case of error
        }
    }        
}
}
