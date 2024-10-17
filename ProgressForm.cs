using System;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace DynamicDashboardWinForm
{
public partial class ProgressForm : Form
{
    public ProgressForm()
    {
        InitializeComponent();
    }

    /// <summary>
    /// Updates the progress bar and label asynchronously with the current progress.
    /// </summary>
    /// <param name="progressValue">The current progress value.</param>
    /// <param name="maximumValue">The maximum value of the progress bar.</param>
public void UpdateProgress(int progressValue, int maximumValue)
{
    if (IsHandleCreated && !IsDisposed)
    {
        if (InvokeRequired)
        {
            try
            {
                // Invoke the update on the UI thread if the form is still valid
                Invoke((Action)(() => UpdateProgress(progressValue, maximumValue)));
            }
            catch (ObjectDisposedException)
            {
                // Handle case where form is disposed between check and invocation
                
            }
        }
        else
        {
            // Set the maximum value first
            progressBar.Maximum = maximumValue;

            // Ensure the progress value is within bounds
            if (progressValue <= maximumValue)
            {
                progressBar.Value = progressValue;
                progressLabel.Text = $"Processing... {progressValue} of {maximumValue}";
            }
            else
            {
                progressBar.Value = maximumValue;
                progressLabel.Text = $"Processing... {maximumValue} of {maximumValue}";
            }
        }
    }
}
   /// <summary>
    /// Closes the form when the operation is complete.
    /// </summary>
    public void CompleteProgress()
    {
        if (InvokeRequired)
        {
            Invoke((Action)(() => Close()));
        }
        else
        {
            Close();
        }
    }
}
}
