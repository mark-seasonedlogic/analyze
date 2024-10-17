using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

namespace BBIReporting
{
    public class CustomIntConverter : DefaultTypeConverter
    {
        public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
        {
            if (int.TryParse(text, out int result))
            {
                return result;
            }
            throw new Exception($"Cannot convert '{text}' to int.");
        }
    }
    public class AndroidDevice
    {
        private string _name;
        private DateTime _firstSeen;
        private DateTime _lastSeen;
        private DateTime _netFirstSeen;
        private DateTime _netLastSeen;

        private string _ipAddress;
        private string _macAddress;//This could be a spoofed address.  AirWatch contains hardware MAC
        private bool _isOnline;
        private string _serialNumber;
        private string _hardwareMACAddress;
        private int _awId;
        private string _merakiId;
        private int _batteryLevel;
        private int _spaceInternalTotal;
        private int _spaceInternalRemaining;
        private bool _isPosi260 = false;
        private bool _isMRDisabled = false;

        private Dictionary<string, AndroidApplicationAW> _applicationDict = new Dictionary<string, AndroidApplicationAW>();

        public string Name { get { return _name; } set { _name = value; } }
        public DateTime LastSeen { get { return _lastSeen; } set { _lastSeen = value; } }
        public DateTime FirstSeen { get { return _firstSeen; } set { _firstSeen = value; } }
        public DateTime NetLastSeen { get { return _netLastSeen; } set { _netLastSeen = value; } }
        public DateTime NetFirstSeen { get { return _netFirstSeen; } set { _netFirstSeen = value; } }
        public string IpAddress { get { return _ipAddress; } set { _ipAddress = value; } }
        public string MacAddress { get { return _macAddress; } set { _macAddress = value; } }
        public bool IsOnline { get { return _isOnline; } set { _isOnline = value; } }
        public string SerialNumber { get { return _serialNumber; } set { _serialNumber = value; } }
        public string HardwareMACAddress { get { return _hardwareMACAddress; } set { _hardwareMACAddress = value; } }
        public int AirWatchID { get { return _awId; } set { _awId = value; } }
        public string MerakiID { get { return _merakiId; } set { _merakiId = value; } }

        public int BatteryLevel { get { return _batteryLevel; } set { _batteryLevel = value; } }
        public int SpaceInternalRemaining { get { return _spaceInternalRemaining; } set { _spaceInternalRemaining = value; } }
        public int SpaceInternalTotal { get { return _spaceInternalTotal; } set { _spaceInternalTotal = value; } }

        public bool IsPosi2_6_0Installed { get { return _isPosi260; } set { _isPosi260 = value; } }
        public bool IsMACRandomizationDisableInstalled { get { return _isMRDisabled; } set { _isMRDisabled = value; } }
        public enum AWFields
        {
            LastSeen,
            FriendlyName,
            Ownership,
            Username,
            FirstName,
            LastName,
            Email,
            Platform,
            OS,
            Model,
            Phone,
            Tags,
            Enrollment,
            ComplianceStatus,
            DisplayName,
            Management,
            AssetNumber,
            SerialNumber,
            IMEI,
            OrganizationGroup,
            Compromised,
            HomeCarrier,
            CurrentCarrier,
            WiFiIPAddress,
            PublicIPAddress,
            WiFiMACAddress,
            Notes,
            WNSStatus,
            DMLastSeen,
            BuildVersion,
            WiFiSSID,
            AndroidManagement,
            CPUArchitecture
        }


    }




    public class AndroidDeviceMap : ClassMap<AndroidDevice>
    {
        public AndroidDeviceMap()
        {
            Map(m => m.Name).Index(0);
            Map(m => m.LastSeen).Index(1);
            Map(m => m.FirstSeen).Index(2);
            Map(m => m.NetLastSeen).Index(3);
            Map(m => m.NetFirstSeen).Index(4);
            Map(m => m.IpAddress).Index(5);
            Map(m => m.MacAddress).Index(6);
            Map(m => m.IsOnline).Index(7);
            Map(m => m.SerialNumber).Index(8);
            Map(m => m.HardwareMACAddress).Index(9);
            Map(m => m.AirWatchID).Index(10).TypeConverter<CustomIntConverter>();
            Map(m => m.MerakiID).Index(11);
            Map(m => m.BatteryLevel).Index(12).TypeConverter<CustomIntConverter>();
            Map(m => m.SpaceInternalRemaining).Index(13).TypeConverter<CustomIntConverter>();
            Map(m => m.SpaceInternalTotal).Index(14).TypeConverter<CustomIntConverter>();
            Map(m => m.IsPosi2_6_0Installed).Index(15);
            Map(m => m.IsMACRandomizationDisableInstalled).Index(16);
        }
    }

    public class DictionaryConverter : DefaultTypeConverter
    {
        public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
        {
            var dictionary = new Dictionary<string, AndroidApplicationAW>();

            // Parse the dictionary from the string here
            // This assumes the dictionary is serialized in a specific way in the CSV
            // Adjust parsing logic as needed

            return dictionary;
        }
        public static List<AndroidDevice> ImportDevicesFromCsv(string filePath)
        {
            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Context.RegisterClassMap<AndroidDeviceMap>();
                var devices = csv.GetRecords<AndroidDevice>().ToList();
                return devices;
            }
        }
    }


}