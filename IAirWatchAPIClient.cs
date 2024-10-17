using System.Collections.Generic;
using System.Threading.Tasks;

namespace BBIReporting
{
/// <summary>
/// IAirWatchAPIClient (Low-level API Interaction)
/// Directly responsible for making requests to the WorkspaceOne UEM (formerly AirWatch) API.
/// Handles the specifics of the API such as URL construction, query parameters, and making GET or POST requests.
/// Should be light and focus only on communication with the external service.
/// </summary>

public interface IAirWatchAPIClient
{
    Task<string> SearchDevicesAsync(string modelIdentifier, string deviceType);
    Task<List<string>> GetAndroidDevicesByOrgAsString(string orgId);
}
}