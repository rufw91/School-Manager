﻿using UmanyiSMS.Lib.Controls;
using UmanyiSMS.Modules.MySystem.ViewModels;

namespace UmanyiSMS.Modules.MySystem.Views
{
    public partial class SetupWizard : CustomWindow
    {
        private bool _openedAsChild;

        public SetupWizard(bool openedAsChild)
        {
            var vm = new SetupWizardVM();
            InitializeComponent();
            _openedAsChild = openedAsChild;
            
            vm.CloseAction = () =>
              {
                  vm.UserClosed = false;
                  this.Close();
              };
            DataContext = vm;
            this.Closed += (o, e) =>
              {
                  if (vm.UserClosed && !_openedAsChild)
                  {
                      App.Current.Shutdown();
                      return;
                  }
                  if (!vm.UserClosed)
                      App.Restart();
              };
        }
    }
}
