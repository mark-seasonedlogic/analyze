using CsvHelper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;

namespace BBIReporting
{
    public static class DataHelper
    {
        public static BindingList<AndroidDevice> LoadAndroidDevicesFromCsv(string filePath)
        {
            try
            {
                using (var reader = new StreamReader(filePath))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    var records = new BindingList<AndroidDevice>();

                    // Read header to map the columns correctly
                    csv.Read();
                    csv.ReadHeader();

                    while (csv.Read())
                    {
                        var device = new AndroidDevice
                        {
                            Name = csv.GetField<string>("Name"),
                            LastSeen = ParseDateTime(csv.GetField<string>("LastSeen")),
                            FirstSeen = ParseDateTime(csv.GetField<string>("FirstSeen")),
                            NetLastSeen = ParseDateTime(csv.GetField<string>("NetLastSeen")),
                            NetFirstSeen = ParseDateTime(csv.GetField<string>("NetFirstSeen")),
                            IpAddress = csv.GetField<string>("IpAddress"),
                            MacAddress = csv.GetField<string>("MacAddress"),
                            IsOnline = csv.GetField<bool>("IsOnline"),
                            SerialNumber = csv.GetField<string>("SerialNumber"),
                            HardwareMACAddress = csv.GetField<string>("HardwareMACAddress"),
                            AirWatchID = csv.GetField<int>("AirWatchID"),
                            MerakiID = csv.GetField<string>("MerakiID"),
                            BatteryLevel = csv.GetField<int>("BatteryLevel"),
                            SpaceInternalRemaining = csv.GetField<int>("SpaceInternalRemaining"),
                            SpaceInternalTotal = csv.GetField<int>("SpaceInternalTotal"),
                            IsPosi2_6_0Installed = csv.GetField<bool>("IsPosi2_6_0Installed"),
                            IsMACRandomizationDisableInstalled = csv.GetField<bool>("IsMACRandomizationDisableInstalled")
                        };

                        records.Add(device);
                    }

                    return records;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error reading CSV file: " + ex.Message);
                return new BindingList<AndroidDevice>();
            }
        }

        private static DateTime ParseDateTime(string dateTimeStr)
        {
            if (DateTime.TryParse(dateTimeStr, out DateTime result))
            {
                return result;
            }
            return DateTime.MinValue;
        }
        // Generate DataTable dynamically based on properties in AndroidDevice
        public static DataTable ToDataTable(List<AndroidDevice> devices, List<string> selectedColumns)
        {
            DataTable table = new DataTable();

            // Add columns based on selected properties
            foreach (var column in selectedColumns)
            {
                table.Columns.Add(column);
            }

            // Add rows based on selected properties
            foreach (var device in devices)
            {
                var row = table.NewRow();

                // Use reflection to get property values dynamically
                foreach (var column in selectedColumns)
                {
                    var propInfo = device.GetType().GetProperty(column);
                    if (propInfo != null)
                    {
                        row[column] = propInfo.GetValue(device) ?? DBNull.Value;
                    }
                }
                table.Rows.Add(row);
            }

            return table;
        }

        // Get a list of available properties from the AndroidDevice class
        public static List<string> GetAvailableColumns()
        {
            return typeof(AndroidDevice).GetProperties().Select(p => p.Name).ToList();
        }
    }
}
