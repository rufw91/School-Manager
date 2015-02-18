using Helper;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace Starehe
{
    /// <summary>
    /// Interaction logic for Restore.xaml
    /// </summary>
    public partial class Restore : UserControl
    {
        private string restorePath = "";
        private string dataPath = "";
        public Restore()
        {
            InitializeComponent();
        }
        private void RestoreAsync()
        {
            BackgroundWorker bgWorker = new BackgroundWorker();
            bgWorker.WorkerSupportsCancellation = true;
            bgWorker.WorkerReportsProgress = false;
            bgWorker.DoWork += delegate(object o1, DoWorkEventArgs e1)
            {
                string[] paramtrs = (string[])e1.Argument;
                e1.Result = null;//DataAccessHelper.RestoreDefaultDbBackup(paramtrs[0], paramtrs[1]);
            };
            bgWorker.RunWorkerCompleted += delegate(object o2, RunWorkerCompletedEventArgs e2)
            {
                if (!(bool)e2.Result)

                    return;
                else
                    MessageBox.Show("Restore Completed Succesfully!",
                   "", MessageBoxButton.OK, MessageBoxImage.Information);
            };
            bgWorker.RunWorkerAsync(new string[] { restorePath, passwordBox1.Password });
        }
        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            RestoreAsync();
        }
        private void btnBrowse_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog saveDialog = new Microsoft.Win32.OpenFileDialog();
            saveDialog.ValidateNames = true;
            saveDialog.Title = "Select location to open backup file";
            saveDialog.Filter = "Backup Files|*.gz";
            if ((saveDialog.ShowDialog() == true) &&
                (new System.IO.FileInfo(saveDialog.FileName).Extension.ToLower() == ".gz"))
            {
                txtBrowse.Text = saveDialog.FileName;
            }
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            restorePath = Helper.Properties.Settings.Default.MostRecentBackup;
            dataPath = Constants.DataFilePath;
            if (restorePath == "")
            {
                MessageBox.Show("No backup file found in the default location. Please choose another backup file.",
                    "", MessageBoxButton.OK, MessageBoxImage.Information);
                radioButton1.IsEnabled = false;
                radioButton1.IsChecked = false;
                radioButton2.IsChecked = true;
            }
        }

    }
}
