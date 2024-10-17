using System;
using System.Collections.Generic;
using System.Linq;

namespace BBIReporting
{
    internal class AndroidDeviceFilterHelper
    {
public static List<AndroidDevice> FilterDevices(List<AndroidDevice> devices, AndroidDeviceFilter filter)
{
    List<AndroidDevice> filteredDevices = new List<AndroidDevice>();

    foreach (var device in devices)
    {
        bool matchesFilter = true;

        if (!string.IsNullOrEmpty(filter.Name) && !device.Name.Contains(filter.Name))
            matchesFilter = false;

        if (!string.IsNullOrEmpty(filter.Restaurant) && (string.IsNullOrEmpty(device.Name) || !device.Name.StartsWith(filter.Restaurant)))
            matchesFilter = false;

        if (filter.FirstSeen.HasValue && device.FirstSeen != filter.FirstSeen)
            matchesFilter = false;

        if (filter.LastSeen.HasValue && device.LastSeen != filter.LastSeen)
            matchesFilter = false;

        if (filter.NetFirstSeen.HasValue && device.NetFirstSeen != filter.NetFirstSeen)
            matchesFilter = false;

        if (filter.NetLastSeen.HasValue && device.NetLastSeen != filter.NetLastSeen)
            matchesFilter = false;

        if (filter.NetLastSeenStart.HasValue && device.NetLastSeen < filter.NetLastSeenStart)
            matchesFilter = false;

        if (filter.NetLastSeenEnd.HasValue && device.NetLastSeen > filter.NetLastSeenEnd)
            matchesFilter = false;

        if (!string.IsNullOrEmpty(filter.IpAddress) && device.IpAddress != filter.IpAddress)
            matchesFilter = false;

        if (!string.IsNullOrEmpty(filter.MacAddress) && device.MacAddress != filter.MacAddress)
            matchesFilter = false;

        if (filter.IsOnline.HasValue && device.IsOnline != filter.IsOnline)
            matchesFilter = false;

        if (!string.IsNullOrEmpty(filter.SerialNumber) && device.SerialNumber != filter.SerialNumber)
            matchesFilter = false;

        if (!string.IsNullOrEmpty(filter.HardwareMACAddress) && device.HardwareMACAddress != filter.HardwareMACAddress)
            matchesFilter = false;

        if (filter.AirWatchID.HasValue && device.AirWatchID != filter.AirWatchID)
            matchesFilter = false;

        if (!string.IsNullOrEmpty(filter.MerakiID) && device.MerakiID != filter.MerakiID)
            matchesFilter = false;

        if (filter.IsPosi2_6_0Installed.HasValue && device.IsPosi2_6_0Installed != filter.IsPosi2_6_0Installed)
            matchesFilter = false;

        if (filter.IsMACRandomizationDisableInstalled.HasValue && device.IsMACRandomizationDisableInstalled != filter.IsMACRandomizationDisableInstalled)
            matchesFilter = false;

        if (matchesFilter)
            filteredDevices.Add(device);
    }

    return filteredDevices;
}

    }
}
