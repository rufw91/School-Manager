using System;
using System.Security.Permissions;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using UmanyiSMS.Lib;
using UmanyiSMS.Lib.Controllers;
using UmanyiSMS.Lib.Models;
using UmanyiSMS.Lib.Presentation;

namespace UmanyiSMS.Modules.MySystem.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "Deputy")]
    public class AdvancedSettingsVM: ViewModelBase
    {
        bool isTested;
        string prevServ;
        ApplicationModel newSchool;
        public AdvancedSettingsVM()
        {
            InitVars();
            CreateCommands();
        }

        protected override void InitVars()
        {            
            Title = "ADVANCED";
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
            SaveServerCommand = new RelayCommand(o =>
            {
                App.SaveInfo(newSchool);
                MessageBox.Show("Successfully saved settings.\r\nYou need to RESTART the system for these changes to take effect.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                App.Restart();
            }, o => !IsBusy && isTested);

            TestServerCommand = new RelayCommand(async o =>
            {
                IsBusy = true;
               
                bool succ = await (DataAccessHelper.Helper as SqlServerHelper).TestDb(ConnectionStringHelper.GetConnectionString(newSchool.ServerName,false));
                MessageBox.Show(succ ? "Test Succeeded." : "Test Failed. The server may not exist or the logon credentials used may be invalid.\r\nContact you system admin for more info.", "Info", MessageBoxButton.OK,
                    succ ? MessageBoxImage.Information : MessageBoxImage.Warning);
                isTested = succ;
                IsBusy = false;
            }, o => !IsBusy);
            CleanDbCommand = new RelayCommand(async o =>
                {
                    IsBusy = true;
                    await (DataAccessHelper.Helper as SqlServerHelper).CleanDb();
                    IsBusy = false;
                }, o => !IsBusy);
            OpenTaskWindowCommand = new RelayCommand(o =>
            {
                if (OpenTaskWindowAction != null)
                {
                    OpenTaskWindowAction.Invoke(o as ViewModelBase);
                }
            }, o => true);
            TestDbCommand = new RelayCommand(async o =>
            {
                IsBusy = true;
                bool succ = await (DataAccessHelper.Helper as SqlServerHelper).TestDb(ConnectionStringHelper.GetConnectionString(newSchool.ServerName, false));
                MessageBox.Show(succ ? "Test Succeeded." : "Test Failed.", "Info", MessageBoxButton.OK,
                    succ ? MessageBoxImage.Information : MessageBoxImage.Warning);
                IsBusy = false;
            }, o => !IsBusy);
            ChangeDbCommand = new RelayCommand(o =>
            {
                IsBusy = true;
                if (MessageBoxResult.Yes != MessageBox.Show("This action is will require your application to RESTART. Are you sure you would like to continue.",
                       "Info", MessageBoxButton.YesNo, MessageBoxImage.Warning))
                    IsBusy = false;
                else
                OpenTaskWindowCommand.Execute(new ChangeDbVM());
                IsBusy = false;
            }, o => !IsBusy);
            BackupDbCommand = new RelayCommand(o =>
            {
                IsBusy = true;
                OpenTaskWindowCommand.Execute(new BackupVM());
                IsBusy = false;
            }, o => !IsBusy);
            RestoreDbCommand = new RelayCommand(async o =>
            {
                IsBusy = true;
                if (MessageBoxResult.Yes == MessageBox.Show("This action is IRREVERSIBLE. Are you sure you would like to continue.",
                    "Info", MessageBoxButton.YesNo, MessageBoxImage.Warning))
                {
                    string filePath = await FileHelper.BrowseBAKFileAsString();
                    bool succ = await (DataAccessHelper.Helper as SqlServerHelper).RestoreDb(filePath);
                    MessageBox.Show(succ ? "Action Succeeded." : "Action Failed.", "Info", MessageBoxButton.OK,
                    succ ? MessageBoxImage.Information : MessageBoxImage.Warning);
                }
                IsBusy = false;
            }, o => !IsBusy);
            ClearDbCommand = new RelayCommand(async o =>
            {
                IsBusy = true;
                if (MessageBoxResult.Yes == MessageBox.Show("This action is IRREVERSIBLE. Are you sure you would like to continue.",
                    "Info", MessageBoxButton.YesNo, MessageBoxImage.Warning))
                {
                    bool succ = await (DataAccessHelper.Helper as SqlServerHelper).ClearDb();
                    MessageBox.Show(succ ? "Action Succeeded." : "Action Failed.", "Info", MessageBoxButton.OK,
                    succ ? MessageBoxImage.Information : MessageBoxImage.Warning);
                }
                IsBusy = false;
            }, o => !IsBusy);

            DeleteDbCommand = new RelayCommand( o =>
            {
                IsBusy = true;
                if (MessageBoxResult.Yes != MessageBox.Show("This action is will require your application to RESTART. Are you sure you would like to continue.",
                    "Info", MessageBoxButton.YesNo, MessageBoxImage.Warning))
                    IsBusy = false;
                else if (MessageBoxResult.Yes == MessageBox.Show("This action is IRREVERSIBLE. Are you sure you would like to continue.",
                    "Info", MessageBoxButton.YesNo, MessageBoxImage.Warning))
                {
                    bool succ = false;// await (DataAccessHelper.Helper as SqlServerHelper).DeleteDb();
                    MessageBox.Show(succ ? "Action Succeeded." : "Action Failed.", "Info", MessageBoxButton.OK,
                    succ ? MessageBoxImage.Information : MessageBoxImage.Warning);
                    App.Restart();
                }
                IsBusy = false;
            }, o => !IsBusy && Thread.CurrentPrincipal.IsInRole(UserRole.SystemAdmin.ToString()));
            OpenQueryEditorCommand = new RelayCommand(o =>
            {
                IsBusy = true;
                OpenTaskWindowCommand.Execute(new QueryEditorVM());
                IsBusy = false;
            }, o => true);

            ShowLogCommand = new RelayCommand(o =>
            {
                IsBusy = true;
                if (OpenTaskWindowAction2 != null)
                {
                    OpenTaskWindowAction2.Invoke(new LogWindowVM());
                }
                IsBusy = false;
            }, o => true);
        }
        public Action BackupAction
        { get; internal set; }

        public Action<ViewModelBase> OpenTaskWindowAction
        { get; internal set; }

        public Action<ViewModelBase> OpenTaskWindowAction2
        { get; internal set; }

        public ICommand SaveServerCommand
        {
            get;
            private set;
        }

        public ICommand TestServerCommand
        {
            get;
            private set;
        }

        public ICommand ShowLogCommand
        { get; private set; }

        public ICommand CleanDbCommand
        { get; private set; }

        public ICommand OpenTaskWindowCommand
        { get; private set; }

        public ICommand BackupDbCommand
        { get; private set; }

        public ICommand RestoreDbCommand
        { get; private set; }

        public ICommand TestDbCommand
        { get; private set; }

        public ICommand ChangeDbCommand
        { get; private set; }

        public ICommand ClearDbCommand
        { get; private set; }

        public ICommand DeleteDbCommand
        { get; private set; }

        public ICommand OpenQueryEditorCommand
        { get; private set; }

        public override void Reset()
        {
           
        }
    }
}
