using System.Collections.Generic;

public interface IDashboardView
{
    string ChartTitle { get; set; }
    void DisplayChartData(Dictionary<string, int> data);
    void ShowEmailList(List<string> emails);
    void DisplayServiceNowItems(List<string> items);
    // Property to set the data source for the DataGridView
    object EmailGridDataSource { set; }

    // Show column selection dialog
    void ShowColumnSelectionDialog(List<string> availableColumns, List<string> selectedColumns);
// New method to set visible columns
    void SetVisibleColumns(List<string> selectedColumns);
    /// <summary>
    /// Displays an error message to the user.
    /// </summary>
    /// <param name="message">The error message to display.</param>
    void ShowErrorMessage(string message);

    /// <summary>
    /// Loads and displays the Meraki objects (data).
    /// </summary>
    /// <param name="merakiData">The data to load.</param>
    void LoadMerakiObjects(List<string> merakiData);
}
