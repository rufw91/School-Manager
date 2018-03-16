using System.ComponentModel;
using System.Diagnostics;
using System.Security.Permissions;
using System.Security.Principal;
using System.Windows;
using System.Windows.Input;
using UmanyiSMS.Lib;
using UmanyiSMS.Lib.Controllers;
using UmanyiSMS.Lib.Presentation;

namespace UmanyiSMS.Modules.MySystem.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "Teacher")]
    public class BackupVM : ViewModelBase
    {
        private string pathToFile;
        public BackupVM()
        {
            InitVars();
            CreateCommands();
        }

        protected override void InitVars()
        {

        }

        protected override void CreateCommands()
        {
            BrowseCommand = new RelayCommand(o =>
            {
                IsBusy = true;
                PathToFile = FileHelper.SaveFileAsBak();
                IsBusy = false;
            }, o => !IsBusy);
            BackupCommand = new RelayCommand(async o =>
            {
                bool succ = false;
                IsBusy = true;
                if (!SqlServerHelper.IsServerMachine)
                {
                    MessageBox.Show("Backup can only be run from the server machine ("+UmanyiSMS.Lib.Properties.Settings.Default.Info.ServerName+")", "Error", MessageBoxButton.OK,
                            MessageBoxImage.Error);
                    return;
                }
                if (IsAdmin())
                    succ = await (DataAccessHelper.Helper as SqlServerHelper).CreateBackupAsync(pathToFile);
                else
                {
                    RunAsAdmin(Application.ResourceAssembly.Location, "/b " + pathToFile);
                    succ = true;
                }

                if (succ)
                    MessageBox.Show("Successfully completed operation.");
                else
                    MessageBox.Show("Operation failed. Ensure you have sufficient permission to access the current folder." +
                        "\r\nYou can also try saving to a different location e.g. a removable disk.", "Error", MessageBoxButton.OK,
                        MessageBoxImage.Error);
                Reset();
                IsBusy = false;
            }, o => !IsBusy && !string.IsNullOrEmpty(pathToFile));
        }

        public bool IsAdmin()
        {
            var pricipal = new WindowsPrincipal(WindowsIdentity.GetCurrent());

            return pricipal != null && pricipal.IsInRole(WindowsBuiltInRole.Administrator);
        }

        private void RunAsAdmin(string fileName, string args)
        {
            var processInfo = new ProcessStartInfo
            {
                Verb = "runas",
                FileName = fileName,
                Arguments = args,
            };

            try
            {
                Process.Start(processInfo).WaitForExit();
            }
            catch (Win32Exception)
            {
                // Do nothing...
            }
        }

        public string PathToFile
        {
            get { return pathToFile; }
            set
            {
                if (value != this.pathToFile)
                {
                    this.pathToFile = value;
                    NotifyPropertyChanged("PathToFile");
                }
            }
        }

        public ICommand BrowseCommand
        { get; private set; }

        public ICommand BackupCommand
        { get; private set; }

        public override void Reset()
        {
            PathToFile = "";
        }
    }
}
