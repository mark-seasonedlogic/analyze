using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using BBIReporting;
using Microsoft.Extensions.Logging;


namespace DynamicDashboardWinForm
{
    /// <summary>
    /// The DashboardPresenter class is responsible for coordinating the business logic of loading
    /// AirWatch and Meraki device data, updating the UI, and handling progress reporting.
    /// </summary>
    public class DashboardPresenter
    {
        //Because DashboardForm and DashboardPresenter reference each other
        //  and both are being injected by DI, we need to use Lazy<T> to 
        //  ensure that the view (DashboardForm) is only 
        private readonly IDashboardView _view;
        private readonly BindingList<AndroidDevice> _deviceList;  // Sample data
        private List<string> _selectedColumns;
        private List<AlertRuleBase> _alertRules;  // List of configurable alert rules
        private readonly Dictionary<string, DeviceAlertRuleInfo> _deviceAlertRuleInfoMap = new Dictionary<string, DeviceAlertRuleInfo>();

        // New dependencies for API services and ProgressForm
        private readonly IAirWatchService _airWatchService;
        private readonly IMerakiService _merakiService;
        private readonly ILogger<DashboardPresenter> _log;
        private readonly ProgressForm _progressForm;

        /// <summary>
        /// Initializes a new instance of the <see cref="DashboardPresenter"/> class.
        /// </summary>
        /// <param name="view">The dashboard view interface.</param>
        /// <param name="logger">The logger instance for logging information and errors.</param>
        /// <param name="airWatchService">The AirWatch service for interacting with the AirWatch API.</param>
        /// <param name="merakiService">The Meraki service for interacting with the Meraki API.</param>
        /// <param name="progressForm">The progress form for displaying asynchronous progress.</param>
        public DashboardPresenter(
            IDashboardView view,
            ILogger<DashboardPresenter> logger,
            IAirWatchService airWatchService,
            IMerakiService merakiService,
            ProgressForm progressForm)
        {
            _log = logger;
            _view = view;
            _airWatchService = airWatchService;
            _merakiService = merakiService;
            _progressForm = progressForm;
            _log.LogInformation("Initializing DashboardPresenter");
            // Load device data from CSV
            _deviceList = DataHelper.LoadAndroidDevicesFromCsv("C:\\Users\\MarkYoung\\SourceCode\\POSi Tablet Monitoring Dashboard - Release\\TestAndroidData.csv");

            // Set initial selected columns
            _selectedColumns = DataHelper.GetAvailableColumns();  // All columns by default
            LoadAlertRules();

            // Call APIs to get initial data
            Task.Run(async () => await LoadAirwatchAndMerakiDevicesAsync("570", "f96016dfc0255f5a696374c1c65a3d9633bad616", "700309742056112193", 600000));

            // Bind data to the view
            BindDataToView();
        }

        /// <summary>
        /// Asynchronously loads device data from AirWatch and Meraki, updates the UI, and reports progress.
        /// </summary>
        /// <param name="airwatchOrgId">The AirWatch organization ID to fetch devices for.</param>
        /// <param name="merakiKey">The Meraki API key for authentication.</param>
        /// <param name="merakiOrgId">The Meraki organization ID to fetch devices for.</param>
        /// <param name="merakiDeviceSpan">The time span for retrieving Meraki devices (in seconds).</param>
        public async Task LoadAirwatchAndMerakiDevicesAsync(string airwatchOrgId, string merakiKey, string merakiOrgId, double merakiDeviceSpan)
        {
            try
            {
                // Show the ProgressForm and reset progress
                _progressForm.Show();
                _progressForm.BringToFront();
                _progressForm.Refresh();
                //_progressForm.UpdateProgress(0, 100); // Initialize progress

                // Fetch AirWatch devices
                //var airwatchDevices = await _airWatchService.GetAirWatchDevicesByOrgAsync(airwatchOrgId);

                //var airwatchDevicesDict = _airWatchService.GetAirwatchObjectsDetail(airwatchDevices);

                // Fetch Meraki device URLs
                List<string> merakiDataURLs = await _merakiService.GetAndroidDevicesURLsAsync(/*merakiKey, */merakiOrgId, merakiDeviceSpan);
                //_progressForm.UpdateProgress(0, merakiDataURLs.Count); // Initialize progress

                try
                {
                    // Setup progress tracking for Meraki data processing
                    var progress = new Progress<int>(value =>
                    {
                        if (!_progressForm.IsDisposed && _progressForm.IsHandleCreated)
                        {       // Ensure UI updates happen on the UI thread
                            if (_progressForm.InvokeRequired)
                            {
                                _progressForm.BeginInvoke(new Action(() =>
                {
                                _progressForm.UpdateProgress(value, merakiDataURLs.Count);
                            }));
                            }
                            else
                            {
                                _progressForm.UpdateProgress(value, merakiDataURLs.Count);
                            }
                        }
                    });

                    // Log that processing has started
                    _log.LogInformation("Processing Meraki URLs asynchronously with progress...");

                    // Process Meraki URLs asynchronously with progress
                    var merakiDataPages = await _merakiService.ProcessMerakiUrlsAsync(merakiDataURLs, progress);

                    // Load processed Meraki objects into the view
                    //_view.LoadMerakiObjects(merakiDataPages);

                    // Log that processing has completed
                    _log.LogInformation("Meraki URL processing complete.");

                    // Complete progress and close form
                    if (_progressForm.InvokeRequired)
                    {
                        _progressForm.BeginInvoke(new Action(() => _progressForm.CompleteProgress()));
                    }
                    else
                    {
                        _log.LogInformation("Loading Meraki Data Completed.");
                        _progressForm.CompleteProgress();
                    }
                }
                catch (Exception ex)
                {
                    _log.LogError(ex, "Error occurred during Meraki URL processing.");

                    // Ensure that the progress form is still closed even on error
                    if (_progressForm.InvokeRequired)
                    {
                        _progressForm.BeginInvoke(new Action(() => _progressForm.CompleteProgress()));
                    }
                    else
                    {
                        _progressForm.CompleteProgress();
                    }
                }
                // Log completion
                _log.LogInformation("Data load completed.");

                // Rebind the updated data to the view
                BindDataToView();
            }
            catch (Exception ex)
            {
                _progressForm.CompleteProgress(); // Ensure the progress form is closed on error
                _log.LogError(ex, "Error occurred during data load.", ex);
            }
        }

        // Existing methods preserved and integrated

        private List<AndroidDevice> GetOnlineDevices(string restaurantNumber)
        {
            AndroidDeviceFilter filter = new AndroidDeviceFilter
            {
                Restaurant = restaurantNumber,
                IsOnline = true
            };

            List<AndroidDevice> filteredDevices = AndroidDeviceFilterHelper.FilterDevices(_deviceList.ToList(), filter);
#if DEBUG
            foreach (var device in filteredDevices)
            {
                _log.LogInformation($"Device Name: {device.Name}, Online: {device.IsOnline}");
            }
#endif

            return filteredDevices;
        }

        private List<AndroidDevice> GetOfflineDevices(string restaurantNumber)
        {
            AndroidDeviceFilter filter = new AndroidDeviceFilter
            {
                Restaurant = restaurantNumber,
                IsOnline = false
            };

            List<AndroidDevice> filteredDevices = AndroidDeviceFilterHelper.FilterDevices(_deviceList.ToList(), filter);
#if DEBUG
            foreach (var device in filteredDevices)
            {
                _log.LogInformation($"Device Name: {device.Name}, Online: {device.IsOnline}");
            }
#endif

            return filteredDevices;
        }

        // Load alert rules from the configuration file
        private void LoadAlertRules()
        {
            _log.LogDebug("Loading alert rules from configuration...");
            _alertRules = RuleConfigurationManager.LoadRules();
            _log.LogDebug("Loaded {0} alert rules.", _alertRules.Count);
        }

        // Save alert rules to the configuration file
        public void SaveAlertRules()
        {
            var ruleConfigs = ConvertRulesToConfigs(_alertRules);
            RuleConfigurationManager.SaveRules(ruleConfigs);
        }

        // Convert the device list to a DataTable and update the view
        private void BindDataToView()
        {
            foreach (var device in _deviceList)
            {
                // Calculate and store the status color for each device
                GetStatusColor(device);
            }
            // Update the view's data source
            _view.EmailGridDataSource = _deviceList;

            // Configure the columns in the view based on selected columns
            UpdateGridColumns();
        }

        // Configure the columns based on selected columns
        private void UpdateGridColumns()
        {
            // Set mandatory column that should always be displayed (e.g., "Name")
            const string mandatoryColumn = "StatusColumn";
            var columnsToDisplay = new List<string>(_selectedColumns) { mandatoryColumn };

            // Tell the view to update the columns
            _view.SetVisibleColumns(columnsToDisplay);
        }

        // Show column selection dialog and update the grid if columns change
        public void ConfigureColumns()
        {
            // Show column selection dialog in the view
            _view.ShowColumnSelectionDialog(DataHelper.GetAvailableColumns(), _selectedColumns);
        }

        public void UpdateSelectedColumns(List<string> selectedColumns)
        {
            _selectedColumns = selectedColumns;
            _view.SetVisibleColumns(_selectedColumns);  // Call view method to apply column visibility
            BindDataToView();  // Rebind the data to the view
        }

        public void LoadChartData()
        {
            // Simulate loading data for the chart
            var data = new Dictionary<string, int>
            {
                { "Category 1", 10 },
                { "Category 2", 20 },
                { "Category 3", 30 }
            };

            _view.DisplayChartData(data);
        }

        public void LoadEmailList()
        {
            // Example email list for testing purposes
            var emailList = new List<string>
            {
                "jason@bloominbrands.com",
                "william@bloominbrands.com",
                "james@bloominbrands.com"
            };
            _log.LogDebug("Removing ShowEmailList method!");
            // _view.ShowEmailList(emailList); // Commented out as per original code
        }

        // Load predefined or test data into the device list
        public void LoadTestData(List<AndroidDevice> testData)
        {
            // Clear existing items and add the new ones
            _deviceList.Clear();
            foreach (var device in testData)
            {
                _deviceList.Add(device);  // Add test data dynamically
            }
            BindDataToView();
        }

        public void LoadServiceNowItems()
        {
            var items = new List<string>
            {
                "INC4686404: Term 1 CLP not printing - Open",
                "INC4679386: NC Missing Labor Hours - In Progress"
            };
            _view.DisplayServiceNowItems(items);
        }

        /// <summary>
        /// Calculates and returns the status color for a device based on alert rules.
        /// </summary>
        /// <param name="device">The Android device to evaluate.</param>
        /// <returns>The status color representing the device's alert level.</returns>
        public Color GetStatusColor(AndroidDevice device)
        {
            // If AirwatchID is not available, skip
            if (string.IsNullOrEmpty(device.AirWatchID.ToString()))
                return Color.Gray;

            // Create or update the DeviceAlertRuleInfo object for the current device
            if (!_deviceAlertRuleInfoMap.TryGetValue(device.AirWatchID.ToString(), out DeviceAlertRuleInfo alertInfo))
            {
                alertInfo = new DeviceAlertRuleInfo(device.AirWatchID);
                _deviceAlertRuleInfoMap[device.AirWatchID.ToString()] = alertInfo;
            }

            // Clear previous failed descriptions
            alertInfo.DeviceAlertDescriptions.Clear();

            var alertRules = new List<AlertRuleBase>();
            AlertRuleBase mostSevereAlert = null;

            foreach (var rule in _alertRules)
            {
                if (rule.Evaluate(device))
                {
                    if (mostSevereAlert == null || rule.Severity > mostSevereAlert.Severity)
                        mostSevereAlert = rule;
                    alertRules.Add(rule);
                    alertInfo.DeviceAlertDescriptions.Add(rule.Description);  // Accumulate descriptions
                }
            }

            // Determine the color based on rule failures
            Color statusColor = Color.Green;

            // Store the final status color in the DeviceAlertRuleInfo object
            alertInfo.StatusColor = mostSevereAlert != null ? mostSevereAlert.AlertColor : statusColor;

            return alertInfo.StatusColor;
        }

        /// <summary>
        /// Retrieves the alert rule information for a specific device.
        /// </summary>
        /// <param name="airwatchId">The AirWatch ID of the device.</param>
        /// <returns>The alert rule information for the device.</returns>
        public DeviceAlertRuleInfo GetAlertRuleInfo(int airwatchId)
        {
            if (airwatchId == 0) return null;

            // Retrieve the DeviceAlertRuleInfo based on AirWatchID
            _deviceAlertRuleInfoMap.TryGetValue(airwatchId.ToString(), out var alertInfo);
            return alertInfo;
        }

        // Converts alert rules to configurations for saving
        private List<AlertRuleConfig> ConvertRulesToConfigs(List<AlertRuleBase> rules)
        {
            var configs = new List<AlertRuleConfig>();

            foreach (var rule in rules)
            {
                var config = new AlertRuleConfig
                {
                    PropertyName = rule.PropertyName,
                    AlertColor = rule.AlertColor,
                    Description = rule.Description
                };

                // Set the comparison type and value based on the specific rule type
                if (rule is EqualsRule equalsRule)
                {
                    config.ComparisonValue = equalsRule.ComparisonValue;
                    config.ComparisonType = "equals";
                }
                else if (rule is GreaterThanRule greaterThanRule)
                {
                    config.ComparisonValue = greaterThanRule.ComparisonValue;
                    config.ComparisonType = "greaterthan";
                }
                else if (rule is LessThanRule lessThanRule)
                {
                    config.ComparisonValue = lessThanRule.ComparisonValue;
                    config.ComparisonType = "lessthan";
                }
                else if (rule is DateGreaterThanRule dateGreaterThanRule)
                {
                    // Access the comparison date using the provider function instead of a direct property
                    config.ComparisonValue = dateGreaterThanRule._comparisonDateProvider().ToString("yyyy-MM-dd");  // Format the date as a string
                    config.ComparisonType = "dategreaterthan";
                }
                else
                {
                    throw new InvalidOperationException($"Unsupported rule type: {rule.GetType().Name}");
                }

                configs.Add(config);
            }

            return configs;
        }

        // Other methods like CreateConditions, DetermineDeviceColor, and CreateCondition can remain as is or be updated as needed.
    }
}
