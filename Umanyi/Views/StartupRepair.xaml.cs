﻿using Helper;
using Helper.Controls;
using UmanyiSMS.ViewModels;
using System;

namespace UmanyiSMS.Views
{
    /// <summary>
    /// Interaction logic for Startup_Repair.xaml
    /// </summary>
    public partial class StartupRepair : CustomWindow
    {
        private StartUp startUp;

        public StartupRepair(StartUp startUp)
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