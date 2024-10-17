using System.Collections.Generic;
using System.Drawing;

namespace BBIReporting
{
public class DeviceAlertRuleInfo
{
    public int AirwatchUniqueId { get; set; } // Unique identifier for the AndroidDevice
    public List<string> DeviceAlertDescriptions { get; set; } = new List<string>(); // Stores failed rule descriptions
    public Color StatusColor { get; set; } // Stores the resulting color based on rule evaluations

    public DeviceAlertRuleInfo(int AirwatchId)
    {
        AirwatchUniqueId = AirwatchId;
    }

    public string GetTooltip()
    {
        return string.Join("; ", DeviceAlertDescriptions);
    }
}
}
