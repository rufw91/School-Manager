using Helper;
using System;
using System.Security.Permissions;
using System.Threading;
using System.Windows;
using System.Windows.Input;

namespace UmanyiSMS.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "SystemAdmin")]
    public class AdvancedSettingsVM: ViewModelBase
    {
        public AdvancedSettingsVM()
        {
            InitVars();
            CreateCommands();
        }

        protected override void InitVars()
        {
            
            Title = "ADVANCED";
        }

        protected override void CreateCommands()
        {
            CleanDbCommand = new RelayCommand(async o =>
                {
                    IsBusy = true;
                    await DataAccessHelper.CleanDb();
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
                bool succ = await DataAccessHelper.TestDb();
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
                    bool succ = await DataAccessHelper.RestoreDb(filePath);
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
                    bool succ = await DataAccessHelper.ClearDb();
                    MessageBox.Show(succ ? "Action Succeeded." : "Action Failed.", "Info", MessageBoxButton.OK,
                    succ ? MessageBoxImage.Information : MessageBoxImage.Warning);
                }
                IsBusy = false;
            }, o => !IsBusy);

            DeleteDbCommand = new RelayCommand(async o =>
            {
                IsBusy = true;
                if (MessageBoxResult.Yes != MessageBox.Show("This action is will require your application to RESTART. Are you sure you would like to continue.",
                    "Info", MessageBoxButton.YesNo, MessageBoxImage.Warning))
                    IsBusy = false;
                else if (MessageBoxResult.Yes == MessageBox.Show("This action is IRREVERSIBLE. Are you sure you would like to continue.",
                    "Info", MessageBoxButton.YesNo, MessageBoxImage.Warning))
                {
                    bool succ = await DataAccessHelper.DeleteDb();
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
        }
        public Action BackupAction
        { get; internal set; }

        public Action<ViewModelBase> OpenTaskWindowAction
        { get; internal set; }

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
