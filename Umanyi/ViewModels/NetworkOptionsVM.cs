﻿using Helper;
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
    public class NetworkOptionsVM:ViewModelBase
    {
        bool canTest=true;
        bool isTested;
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
            isTested = true;
            CreateCommands();            
        }
        protected override void InitVars()
        {
            Title = "NETWORK OPTIONS";
            isTested = false;
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
                MessageBox.Show("Successfully saved settings.\r\nYou need to RESTART the system for these changes to take effect.","Success", MessageBoxButton.OK, MessageBoxImage.Information);
                App.Restart();
            }, o => !IsBusy && isTested);

            TestCommand = new RelayCommand(async o =>
            {
                IsBusy = true;
                bool succ = await (DataAccessHelper.Helper as SqlServerHelper).TestDb(ConnectionStringHelper.CreateTestConnSTr(newSchool.ServerName));
                MessageBox.Show(succ ? "Test Succeeded." : "Test Failed.", "Info", MessageBoxButton.OK,
                    succ ? MessageBoxImage.Information : MessageBoxImage.Warning);
                isTested = succ;
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
