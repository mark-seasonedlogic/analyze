using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Windows.Forms;
using BBIReporting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;


namespace DynamicDashboardWinForm
{
    public partial class DashboardForm : Form, IDashboardView  // Implement the interface
    {
        private readonly ILogger<DashboardForm> _log;
        private Dictionary<string,Restaurant> restaurants;
        private readonly IServiceProvider _serviceProvider;
        private DashboardPresenter _presenter;
        private IAirWatchService _airWatchService;
        private IMerakiService _merakiService;
        private ProgressForm _progressForm;
        private ILogger<Restaurant> _restaurantLogger;
public DashboardForm(IServiceProvider serviceProvider,ILogger<DashboardForm> logger, ILogger<Restaurant> restaurantLogger, IAirWatchService airWatchService, IMerakiService merakiService, ProgressForm progressForm)
{
    _log = logger;
    _restaurantLogger = restaurantLogger;
    _airWatchService = airWatchService;
    _merakiService = merakiService;
    _progressForm = progressForm;
_serviceProvider = serviceProvider;
 _log.LogInformation("Intializing DashboardForm");
 
    InitializeComponent();
    InitializeMasterData();
    this.Load += DashboardForm_Load;

}
// Called when the form is loaded
private void DashboardForm_Load(object sender, EventArgs e)
{
    // Resolve the presenter and initialize logic
    GetPresenter();
}
// Lazy resolution of DashboardPresenter
private DashboardPresenter GetPresenter()
{
    return _presenter = _serviceProvider.GetRequiredService<DashboardPresenter>();
}
        /// <summary>
    /// Displays an error message in a message box.
    /// </summary>
    /// <param name="message">The error message to display.</param>
    public void ShowErrorMessage(string message)
    {
        MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }

    /// <summary>
    /// Loads and displays the Meraki objects in the appropriate UI component.
    /// </summary>
    /// <param name="merakiData">The list of Meraki data to load.</param>
    public void LoadMerakiObjects(List<string> merakiData)
    {
        // This method should handle displaying the Meraki data in the appropriate part of your UI.
        // For example, you could load it into a DataGridView or any other UI component.

        // Example using a DataGridView (you can adapt this to your needs):
        foreach (var data in merakiData)
        {
            // Assuming you have a DataGridView named 'merakiDataGridView'
            _log.LogDebug("Adding meraki data {0}",data);
        }
    }
        // Property to set the data source of the DataGridView
        public object EmailGridDataSource
        {
            set { emailGrid.DataSource = value; }
        }

        // Show column selection dialog to the user
        public void ShowColumnSelectionDialog(List<string> availableColumns, List<string> selectedColumns)
        {
            // Show the column selection dialog
            using (var form = new BBIReporting.ColumnSelectionForm(availableColumns, selectedColumns))
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    // Update the columns in the presenter
                    _presenter.UpdateSelectedColumns(form.SelectedColumns);
                }
            }
        }
        private void InitializeMasterData()
        {
            //First Restaurant info:
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Choose Restaurant Directory Master File from BBI Connect";
            openFileDialog.ShowDialog();
            string master = openFileDialog.FileName;
            if (!String.IsNullOrEmpty(master) && File.Exists(master))
            {
                restaurants = Restaurant.GetMasterRestaurantList(master, _restaurantLogger);

            }
            else
            {
                //Log an error here
                _log.LogError(String.Format("File {0} does not exist.", master));
            }

        }        // Button click event to show column configuration
        private void btnConfigureColumns_Click(object sender, EventArgs e)
        {
            _presenter.ConfigureColumns();  // Delegate to presenter
        }
        public string ChartTitle
        {
            get => chartLabel.Text;
            set => chartLabel.Text = value;
        }

        public void DisplayChartData(Dictionary<string, int> data)
        {
            chart.Series["Series1"].Points.Clear();
            foreach (var point in data)
            {
                chart.Series["Series1"].Points.AddXY(point.Key, point.Value);
            }
        }

        public void ShowEmailList(List<string> emails)
        {
            // Check if the DataGridView is currently using a DataSource
            if (emailGrid.DataSource != null)
            {
                // Reset the DataSource before clearing or updating the grid
                emailGrid.DataSource = null;
            }

            // Create a DataTable to store email data
            var dataTable = new DataTable();
            dataTable.Columns.Add("Email", typeof(string));

            // Populate the DataTable with the email list
            foreach (var email in emails)
            {
                dataTable.Rows.Add(email);
            }

            // Set the DataSource of the DataGridView to the new DataTable
            emailGrid.DataSource = dataTable;
        }

        public void DisplayServiceNowItems(List<string> items)
        {
            serviceList.Items.Clear();
            foreach (var item in items)
            {
                serviceList.Items.Add(item);
            }
        }
        private void emailGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0 && e.RowIndex >= 0)
            {
                var device = emailGrid.Rows[e.RowIndex].DataBoundItem as AndroidDevice;

                if (device != null)
                {
                    var failedInfo = _presenter.GetAlertRuleInfo(device.AirWatchID);

                    if (failedInfo != null)
                    {
                        _log.LogError($"Failed Rules:\n{failedInfo.GetTooltip()}", "Device Alert Details", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
        }

        private void emailGrid_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            try
            { // Use the column name to ensure this is applied to the "Status" column
                if (e.ColumnIndex >= 0 && e.RowIndex >= 0 && emailGrid.Columns[e.ColumnIndex].Name == "StatusColumn")
                {
                    e.PaintBackground(e.ClipBounds, true);

                    var device = emailGrid.Rows[e.RowIndex].DataBoundItem as AndroidDevice;

                    if (device != null)
                    {
                        // Use the presenter's method to get the rule info based on MACAddress
                        var failedInfo = _presenter.GetAlertRuleInfo(device.AirWatchID);

                        // Use the stored status color
                        Color bulletColor = failedInfo.StatusColor;
                        _log.LogDebug("Returning color {0} for device {1}", bulletColor, device.Name);

                        // Calculate the position for the bullet
                        int bulletSize = 14;  // Size of the bullet circle
                        int x = e.CellBounds.Left + (e.CellBounds.Width - bulletSize) / 2;
                        int y = e.CellBounds.Top + (e.CellBounds.Height - bulletSize) / 2;

                        // Draw the bullet circle
                        using (SolidBrush brush = new SolidBrush(bulletColor))
                        {
                            e.Graphics.FillEllipse(brush, x, y, bulletSize, bulletSize);
                        }

                        // Set tooltip for the current cell
                        emailGrid.Rows[e.RowIndex].Cells[e.ColumnIndex].ToolTipText = failedInfo.GetTooltip();

                        // Prevent the default painting
                        e.Handled = true;
                    }

                }

            }
            catch (Exception ex)
            {
                _log.LogError("Exception happened: {0}\nStackTrace: {1}", ex.Message, ex);
            }
        }


        // New implementation for setting visible columns in the DataGridView
        public void SetVisibleColumns(List<string> selectedColumns)
        {
            // Ensure that "Status" column is always visible and set as the first column.
            if (!emailGrid.Columns.Contains("StatusColumn"))
            {
                DataGridViewColumn statusColumn = new DataGridViewTextBoxColumn();
                statusColumn.Name = "StatusColumn";
                statusColumn.HeaderText = "Status";
                statusColumn.Width = 50;
                statusColumn.ReadOnly = true;
                statusColumn.Resizable = DataGridViewTriState.False;
                
                emailGrid.Columns.Insert(0, statusColumn);
            }

            // Loop through all columns in the DataGridView
            foreach (DataGridViewColumn column in emailGrid.Columns)
            {
                if (selectedColumns.Contains(column.Name))
                {
                    // Ensure "Status" is the first column
                    if (column.Name == "Status")
                    {
                        column.DisplayIndex = 0;
                    }
                    column.Visible = true;
                }
                else
                {
                    column.Visible = false;
                }
            }
        }

    }
}
