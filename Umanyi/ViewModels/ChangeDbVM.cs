using Helper;
using System.Collections.ObjectModel;
using System.Security.Permissions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace UmanyiSMS.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "SystemAdmin")]
    public class ChangeDbVM: ViewModelBase
    {
        private ObservableCollection<string> allDatabases;
        private string selectedDB;
        public ChangeDbVM()
        {
            InitVars();
            CreateCommands();
        }
        protected async override void InitVars()
        {
            IsBusy = true;
            SelectedDB= Helper.Properties.Settings.Default.DBName;
            string selectString = "SELECT name FROM sys.databases WHERE name NOT IN (N'master',N'tempdb',N'model',N'msdb')";
            await Task.Factory.StartNew(() => { AllDatabases = DataAccessHelper.CopyFromDBtoObservableCollection(selectString); });
            IsBusy = false;
        }

        protected override void CreateCommands()
        {
            SaveCommand = new RelayCommand(o => 
            {
                if (MessageBoxResult.Yes == MessageBox.Show("This action may leave your database inaccessible. Are you sure you would like to proceed?", "Info",
                     MessageBoxButton.YesNo, MessageBoxImage.Warning))
                {
                    Helper.Properties.Settings.Default.DBName = selectedDB;
                    Helper.Properties.Settings.Default.Save();
                    App.Restart();
                }
            }, o => true);

            ResetCommand = new RelayCommand(o =>
            {
                if (MessageBoxResult.Yes == MessageBox.Show("This action may leave your database inaccessible.. Are you sure you would like to proceed?", "Info",
                     MessageBoxButton.YesNo, MessageBoxImage.Warning))
                {
                    Helper.Properties.Settings.Default.DBName = "UmanyiSMS";
                    Helper.Properties.Settings.Default.Save();
                }
            }, o => true);
        }

        public override void Reset()
        {
          
        }

        public string SelectedDB
        {
            get { return selectedDB; }

            set
            {
                if (value != this.selectedDB)
                {
                    this.selectedDB = value;
                    NotifyPropertyChanged("SelectedDB");
                }
            }
        }

        public ICommand SaveCommand
        { get; private set; }

        public ICommand ResetCommand
        { get; private set; }

        public ObservableCollection<string> AllDatabases
        {
            get { return allDatabases; }

            private set
            {
                if (value != this.allDatabases)
                {
                    this.allDatabases = value;
                    NotifyPropertyChanged("AllDatabases");
                }
            }
        }
    }
}
