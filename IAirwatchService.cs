using System.Collections.Generic;
using System.Threading.Tasks;

namespace BBIReporting
{

public interface IAirWatchService
{
    Task<List<string>> GetAirWatchDevicesByOrgAsync(string orgId);
    Dictionary<string,AndroidDevice> GetAirwatchObjectsDetail(List<string> airwatchDevices);

}
}
