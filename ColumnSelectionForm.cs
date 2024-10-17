using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace BBIReporting
{
    public partial class ColumnSelectionForm : Form
    {
        public List<string> SelectedColumns { get; private set; }

        // Context menu for Select All and Deselect All
        private ContextMenuStrip contextMenuStrip;

        public ColumnSelectionForm(List<string> availableColumns, List<string> initiallySelectedColumns)
        {
            InitializeComponent();

            // Initialize the context menu
            InitializeContextMenu();

            // Associate the context menu with the CheckedListBox
            checkedListBoxColumns.ContextMenuStrip = contextMenuStrip;

            // Populate the CheckedListBox with available columns
            foreach (var column in availableColumns)
            {
                checkedListBoxColumns.Items.Add(column, initiallySelectedColumns.Contains(column));
            }
        }

        // Initialize the context menu with Select All and Deselect All
        private void InitializeContextMenu()
        {
            contextMenuStrip = new ContextMenuStrip();
            var selectAllMenuItem = new ToolStripMenuItem("Select All", null, SelectAll_Click);
            var deselectAllMenuItem = new ToolStripMenuItem("Deselect All", null, DeselectAll_Click);

            contextMenuStrip.Items.Add(selectAllMenuItem);
            contextMenuStrip.Items.Add(deselectAllMenuItem);
        }

        // Event handler for the Save button
        private void btnSave_Click(object sender, EventArgs e)
        {
            // Save selected columns
            SelectedColumns = checkedListBoxColumns.CheckedItems.Cast<string>().ToList();
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        // Event handler for the Cancel button
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        // Event handler for "Select All" in the context menu
        private void SelectAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < checkedListBoxColumns.Items.Count; i++)
            {
                checkedListBoxColumns.SetItemChecked(i, true);
            }
        }

        // Event handler for "Deselect All" in the context menu
        private void DeselectAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < checkedListBoxColumns.Items.Count; i++)
            {
                checkedListBoxColumns.SetItemChecked(i, false);
            }
        }
    }
}
