using System;

namespace BBIReporting
{
    public class AndroidDeviceFilter
    {
        public string Name { get; set; }
        public string Restaurant { get; set; }
        public DateTime? FirstSeen { get; set; }
        public DateTime? LastSeen { get; set; }
        public DateTime? NetFirstSeen { get; set; }
        public DateTime? NetLastSeen { get; set; }
        public string IpAddress { get; set; }
        public string MacAddress { get; set; }
        public bool? IsOnline { get; set; }
        public string SerialNumber { get; set; }
        public string HardwareMACAddress { get; set; }
        public int? AirWatchID { get; set; }
        public string MerakiID { get; set; }
        public bool? IsPosi2_6_0Installed { get; set; }
        public bool? IsMACRandomizationDisableInstalled { get; set; }
        public DateTime? NetLastSeenStart { get; set; } // New property for the start date
        public DateTime? NetLastSeenEnd { get; set; }   // New property for the end date
    }
}
