using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BBIReporting
{
    /*

     {
          "ApplicationName": "POSi",
          "BundleId": "com.RestaurantDataConceptsInc.POSiterm",
          "AppVersion": "2.6.0.0",
          "ActualFileVersion": "2.6.0",
          "AppType": "Internal",
          "Status": "Active",
          "Platform": 5,
          "SupportedModels": {
            "Model": [
              {
                "ApplicationId": 262,
                "ModelId": 5,
                "ModelName": "Android"
              }
            ]
          },
          "AssignmentStatus": "Assigned",
          "ApplicationSize": "9.52 MB",
          "CategoryList": {
            "Category": []
          },
          "SmartGroups": [
            {
              "Id": 517,
              "Name": "AND-POSI App TEST - 2.6.0"
            },
            {
              "Id": 521,
              "Name": "POSI - 2.6.0 - TEST - 12-11-2023 01-08-2024"
            },
            {
              "Id": 531,
              "Name": "POSI 2.6.0 - 01/29/24"
            },
            {
              "Id": 536,
              "Name": "POSI 2.6.0 - 02/05/24"
            },
            {
              "Id": 544,
              "Name": "POSI 2.6 - 02-20-24"
            },
            {
              "Id": 549,
              "Name": "POSI 2.6.0 - 02-26-24"
            }
          ],
          "IsReimbursable": false,
          "ApplicationSource": 0,
          "LocationGroupId": 570,
          "RootLocationGroupName": "Bloomin Brands",
          "OrganizationGroupUuid": "888b2d6c-0bb7-4664-87f4-d67c2a2d17dd",
          "LargeIconUri": "https://awmdm.bloominbrands.com/DeviceServices/publicblob/8b1523a6-d67e-4689-a927-4e122b790c45/BlobHandler.pblob",
          "MediumIconUri": "https://awmdm.bloominbrands.com/DeviceServices/publicblob/8b1523a6-d67e-4689-a927-4e122b790c45/BlobHandler.pblob",
          "SmallIconUri": "https://awmdm.bloominbrands.com/DeviceServices/publicblob/8b1523a6-d67e-4689-a927-4e122b790c45/BlobHandler.pblob",
          "PushMode": 0,
          "AppRank": 0,
          "AssignedDeviceCount": 7065,
          "InstalledDeviceCount": 6727,
          "NotInstalledDeviceCount": 338,
          "AutoUpdateVersion": false,
          "EnableProvisioning": false,
          "IsDependencyFile": false,
          "ContentGatewayId": 0,
          "IconFileName": "res/kn.png",
          "ApplicationFileName": "POSiterm-release (1).apk",
          "MetadataFileName": "",
          "Id": {
            "Value": 262
          },
          "Uuid": "b1ee45b7-c586-4bc6-aa20-998df218cca5"
        }

     */
    public class AndroidSmartGroup
    {
        public string Uuid { get; set; }
        public string Name { get; set; }
        public AndroidSmartGroup() { }
    }
    public class AndroidApplicationAW
    {
        public string Uuid { get; set; }
        public string ApplicationFileName { get; set; }
        public string ApplicationName { get; set; }
        public string BundleId { get; set; }
        public string AppVersion { get; set; }
        public string ActualFileVersion { get; set; }
        public string AppType { get; set; }
        public string Status { get; set; }
        public List<AndroidSmartGroup> SmartGroups { get; set; }
        public AndroidApplicationAW() {
        SmartGroups = new List<AndroidSmartGroup>();
        }
    }
}
