using System.Windows.Forms;

namespace DynamicDashboardWinForm
{
    partial class ProgressForm
    {
        private System.ComponentModel.IContainer components = null;
        private ProgressBar progressBar;
        private Label progressLabel;

        private void InitializeComponent()
        {
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.progressLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            
            // ProgressBar
            this.progressBar.Location = new System.Drawing.Point(12, 12);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(360, 23);
            this.progressBar.TabIndex = 0;
            
            // ProgressLabel
            this.progressLabel.AutoSize = true;
            this.progressLabel.Location = new System.Drawing.Point(12, 45);
            this.progressLabel.Name = "progressLabel";
            this.progressLabel.Size = new System.Drawing.Size(110, 13);
            this.progressLabel.TabIndex = 1;
            this.progressLabel.Text = "Processing... 0 of 100";
            
            // ProgressForm
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 71);
            this.Controls.Add(this.progressLabel);
            this.Controls.Add(this.progressBar);
            this.Name = "ProgressForm";
            this.Text = "Progress";
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
