﻿


using System;
using UmanyiSMS.Lib.Controls;
using UmanyiSMS.Modules.MySystem.Models;
using UmanyiSMS.Modules.MySystem.ViewModels;

namespace UmanyiSMS.Modules.MySystem.Views
{
    /// <summary>
    /// Interaction logic for Startup_Repair.xaml
    /// </summary>
    public partial class StartupRepair : CustomWindow
    {
        private StartUpModel startUp;

        public StartupRepair(StartUpModel startUp)
        {
            this.startUp = startUp;
            InitializeComponent();
            StartupRepairVM vm = new StartupRepairVM(startUp);
            this.DataContext = vm;

            if (vm.CloseAction==null)
                vm.CloseAction = new Action(()=>this.Close());

        }
    }
}