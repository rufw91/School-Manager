using System;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Input;
using UmanyiSMS.Lib;
using UmanyiSMS.Lib.Controllers;
using UmanyiSMS.Lib.Models;
using UmanyiSMS.Lib.Presentation;

namespace UmanyiSMS.Modules.MySystem.ViewModels
{
    public class NetworkOptionsVM:ViewModelBase
    {
        bool isTested;
        string prevServ;
        ApplicationModel newSchool;
        SqlCredential _credential;
        public NetworkOptionsVM(SqlCredential credential)
        {
            _credential = credential;
            InitVars();
            CreateCommands();
        }

        protected override void InitVars()
        {
            Title = "NETWORK OPTIONS";
            isTested = false;
            prevServ = App.Info.ServerName;
            newSchool = App.Info;
            newSchool.PropertyChanged += (o, e) =>
            {
                if (e.PropertyName == "ServerName")
                    isTested = false;
            };
        }
        
        public ApplicationModel NewSchool
        {
            get { return this.newSchool; }
        }
        protected override void CreateCommands()
        {
            SaveCommand = new RelayCommand(o =>
            {
                App.SaveInfo(newSchool);
                MessageBox.Show("Successfully saved settings.\r\nYou need to RESTART the system for these changes to take effect.","Success", MessageBoxButton.OK, MessageBoxImage.Information);
                App.Restart();
            }, o => !IsBusy && isTested);

            TestCommand = new RelayCommand(async o =>
            {
                IsBusy = true;
                bool succ = await (DataAccessHelper.Helper as SqlServerHelper).TestDb(ConnectionStringHelper.GetConnectionString(newSchool.ServerName, false),_credential);
                MessageBox.Show(succ ? "Test Succeeded." : "Test Failed. The server may not exist or the logon credentials used may be invalid. Close this window and re-enter the credentials then click 'Login problems' to open this window.\r\nFor more assistance, contact you system admin.", "Info", MessageBoxButton.OK,
                     succ ? MessageBoxImage.Information : MessageBoxImage.Warning);
                isTested = succ;
                IsBusy = false;
            }, o => !IsBusy);
        }
        public override void Reset()
        {
            newSchool = new ApplicationModel(Lib.Properties.Settings.Default.Info);
        }

        public Action CloseWindowAction
        { get; set; }
        public ICommand SaveCommand
        {
            get;
            private set;
        }

        public ICommand TestCommand
        {
            get;
            private set;
        }
    }
}
