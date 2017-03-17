using System.Security.Permissions;
using System.Windows;
using System.Windows.Input;
using UmanyiSMS.Lib;
using UmanyiSMS.Lib.Controllers;
using UmanyiSMS.Lib.Presentation;

namespace UmanyiSMS.Modules.System.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "Deputy")]
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
                IsBusy = true;
                bool succ = await (DataAccessHelper.Helper as SqlServerHelper).CreateBackupAsync(pathToFile);
                if (succ)
                    MessageBox.Show("Successfully completed operation.");
                else
                    MessageBox.Show("Operation failed. Ensure you have sufficient permission to access the current folder."+
                        "\r\nYou can also try saving to a different location e.g. a removable disk.", "Error", MessageBoxButton.OK, 
                        MessageBoxImage.Error);
                Reset();
                IsBusy = false;
            }, o => !IsBusy && !string.IsNullOrEmpty(pathToFile));
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
