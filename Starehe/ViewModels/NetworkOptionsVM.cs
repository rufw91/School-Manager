using Helper;
using Helper.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace UmanyiSMS.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "SystemAdmin")]
    public class NetworkOptionsVM:ViewModelBase
    {
        bool canTest=true;
        string prevServ;
        ApplicationModel newSchool;
        public NetworkOptionsVM()
        {
            InitVars();
            CreateCommands();
        }
        public NetworkOptionsVM(bool canTest)
        {
            this.canTest = canTest;
            InitVars();
            CreateCommands();            
        }
        protected override void InitVars()
        {
            Title = "NETWORK OPTIONS";
            prevServ = App.Info.ServerName;
            newSchool = App.Info;
        }

        public bool CanTest
        { get { return canTest; } }

        public ApplicationModel NewSchool
        {
            get { return this.newSchool; }
        }
        protected override void CreateCommands()
        {
            SaveCommand = new RelayCommand(o =>
            {
                Helper.Properties.Settings.Default.Info = new ApplicationPersistModel(newSchool);
                Helper.Properties.Settings.Default.Save();
                App.Info.CopyFrom(new ApplicationModel(Helper.Properties.Settings.Default.Info));
                MessageBox.Show("Successfully saved settings.","Success", MessageBoxButton.OK, MessageBoxImage.Information);
                if (CloseWindowAction!=null)
                CloseWindowAction.Invoke();
                
            }, o => !IsBusy);

            TestCommand = new RelayCommand(async o =>
            {
                IsBusy = true;
                bool succ = await DataAccessHelper.TestDb(ConnectionStringHelper.CreateTestConnSTr(newSchool.ServerName));
                MessageBox.Show(succ ? "Test Succeeded." : "Test Failed.", "Info", MessageBoxButton.OK,
                    succ ? MessageBoxImage.Information : MessageBoxImage.Warning);
                
                IsBusy = false;
            }, o => !IsBusy);
        }
        public override void Reset()
        {
            newSchool = new ApplicationModel(Helper.Properties.Settings.Default.Info);
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
