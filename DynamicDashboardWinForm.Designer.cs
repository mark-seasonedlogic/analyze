namespace DynamicDashboardWinForm
{
    partial class DashboardForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label chartLabel;
        private System.Windows.Forms.DataGridView emailGrid;
        private System.Windows.Forms.ListBox serviceList;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart;
        private System.Windows.Forms.TableLayoutPanel mainLayout;
        private System.Windows.Forms.TableLayoutPanel subLayout;
        private System.Windows.Forms.Button btnConfigureColumns;
        private System.Windows.Forms.DataGridViewTextBoxColumn statusColumn;
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.chartLabel = new System.Windows.Forms.Label();
            this.emailGrid = new System.Windows.Forms.DataGridView();
            this.serviceList = new System.Windows.Forms.ListBox();
            this.chart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.mainLayout = new System.Windows.Forms.TableLayoutPanel();
            this.subLayout = new System.Windows.Forms.TableLayoutPanel();

            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();

            ((System.ComponentModel.ISupportInitialize)(this.emailGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart)).BeginInit();
            this.SuspendLayout();

            // 
            // chartLabel
            // 
            this.chartLabel.AutoSize = true;
            this.chartLabel.Location = new System.Drawing.Point(12, 9);
            this.chartLabel.Name = "chartLabel";
            this.chartLabel.Size = new System.Drawing.Size(105, 13);
            this.chartLabel.TabIndex = 0;
            this.chartLabel.Text = "Endpoint Reboot Status";

            // 
            // emailGrid
            // 
            this.emailGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.emailGrid.Columns.Add("Email", "Email");
            this.emailGrid.Columns.Add("RestaurantID", "Restaurant ID");
            this.emailGrid.Columns.Add("DeviceType", "Device Type");
            this.emailGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.emailGrid.Location = new System.Drawing.Point(3, 3);
            this.emailGrid.Name = "emailGrid";
            this.emailGrid.Size = new System.Drawing.Size(1000, 250);  // Full width of the form in the first row
            this.emailGrid.TabIndex = 1;
            this.statusColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.statusColumn.Name = "StatusColumn";
            this.statusColumn.HeaderText = "Status";
            this.statusColumn.Width = 50;  // Set an appropriate width for the bullet
            this.statusColumn.ReadOnly = true;  // No editing for this column
            this.statusColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;

            // Add the custom status column to the beginning of the grid
            this.emailGrid.Columns.Insert(0, this.statusColumn);

            // Attach the CellPainting event handler for custom rendering
            this.emailGrid.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.emailGrid_CellPainting);

            //
            // emailList Column Selector Button
            //
            this.btnConfigureColumns = new System.Windows.Forms.Button();
            this.btnConfigureColumns.Text = "Configure Columns";
            this.btnConfigureColumns.Location = new System.Drawing.Point(330, 210);  // Position it below the email grid
            this.btnConfigureColumns.Click += new System.EventHandler(this.btnConfigureColumns_Click);
            this.Controls.Add(this.btnConfigureColumns);
            // 
            // serviceList
            // 
            this.serviceList.FormattingEnabled = true;
            this.serviceList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.serviceList.Location = new System.Drawing.Point(504, 3);
            this.serviceList.Name = "serviceList";
            this.serviceList.Size = new System.Drawing.Size(495, 300);  // Half-width in the second row
            this.serviceList.TabIndex = 2;

            // 
            // chart
            // 
            chartArea1.Name = "ChartArea1";
            this.chart.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.chart.Legends.Add(legend1);
            this.chart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chart.Location = new System.Drawing.Point(3, 3);
            this.chart.Name = "chart";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.chart.Series.Add(series1);
            this.chart.Size = new System.Drawing.Size(495, 300);  // Half-width in the second row
            this.chart.TabIndex = 3;
            this.chart.Text = "chart";

            // 
            // mainLayout
            // 
            this.mainLayout.ColumnCount = 1;
            this.mainLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));  // Single column
            this.mainLayout.RowCount = 2;
            this.mainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));  // Top row height for emailGrid (50%)
            this.mainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));  // Remaining height for the second row (50%)
            this.mainLayout.Controls.Add(this.emailGrid, 0, 0);  // Add emailGrid to top row
            this.mainLayout.Controls.Add(this.subLayout, 0, 1);  // Add sub-layout for chart and serviceList
            this.mainLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainLayout.Location = new System.Drawing.Point(0, 0);
            this.mainLayout.Name = "mainLayout";
            this.mainLayout.Size = new System.Drawing.Size(1024, 768);  // Full size of the form
            this.mainLayout.TabIndex = 4;

            // 
            // subLayout
            // 
            this.subLayout.ColumnCount = 2;
            this.subLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));  // Left column for chart
            this.subLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));  // Right column for serviceList
            this.subLayout.Controls.Add(this.chart, 0, 0);  // Add chart to left cell
            this.subLayout.Controls.Add(this.serviceList, 1, 0);  // Add serviceList to right cell
            this.subLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.subLayout.Location = new System.Drawing.Point(3, 253);
            this.subLayout.Name = "subLayout";
            this.subLayout.RowCount = 1;
            this.subLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));  // Single row
            this.subLayout.Size = new System.Drawing.Size(1000, 300);  // Spans full width in the second row
            this.subLayout.TabIndex = 5;

            // 
            // DashboardForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1024, 768);
            this.Controls.Add(this.mainLayout);
            this.Name = "DashboardForm";
            this.Text = "Proactive Monitoring Dashboard";

            ((System.ComponentModel.ISupportInitialize)(this.emailGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart)).EndInit();
            this.ResumeLayout(false);
        }
    }
}
