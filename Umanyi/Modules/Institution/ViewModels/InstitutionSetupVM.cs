using Helper;
using Helper.Models;
using System;
using System.Security.Permissions;
using System.Windows;
using System.Windows.Input;
using UmanyiSMS.Lib;
using UmanyiSMS.Lib.Controllers;
using UmanyiSMS.Lib.Models;
using UmanyiSMS.Lib.Presentation;

namespace UmanyiSMS.Modules.Institution.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "Deputy")]
    public class InstitutionSetupVM: ViewModelBase
    {
        ApplicationModel newSchool;
        bool isInStartup=false;
        public InstitutionSetupVM()
        {
            InitVars();
            CreateCommands();
        }

        protected override void InitVars()
        {
            Title = "INSTITUTION SETUP";
            newSchool =new ApplicationModel( Lib.Properties.Settings.Default.Info);
        }

        public InstitutionSetupVM(bool isInStartup)
        {
            this.isInStartup = isInStartup;
            newSchool = new ApplicationModel();
            Title = "INSTITUTION SETUP";
            CreateCommands();
        }

        public ApplicationModel NewSchool
        {
            get { return this.newSchool; }
        }

        protected override void CreateCommands()
        {
            BrowseCommand = new RelayCommand(o =>
            {
                newSchool.SPhoto = FileHelper.BrowseImageAsByteArray();
            }, o => true);

            SaveCommand = new RelayCommand(o =>
            {
                Lib.Properties.Settings.Default.Info = new ApplicationPersistModel(newSchool);
                Lib.Properties.Settings.Default.Save();

                App.Info.CopyFrom(new ApplicationModel(Lib.Properties.Settings.Default.Info));
                if (MessageBoxResult.Yes == MessageBox.Show("You need to restart the School Management system for the changes to be saved. Do you want to restart now? ",
                    "Warning", MessageBoxButton.YesNo, MessageBoxImage.Information))
                {
                    App.Restart();
                }
            }, o => !IsBusy && CanSave());
        }

        private bool CanSave()
        {
            return !string.IsNullOrWhiteSpace(newSchool.Name) && !string.IsNullOrWhiteSpace(newSchool.Address)
                && !string.IsNullOrWhiteSpace(newSchool.AltInfo) && !string.IsNullOrWhiteSpace(newSchool.City)
                && !string.IsNullOrWhiteSpace(newSchool.Email) && !string.IsNullOrWhiteSpace(newSchool.FullName)
                && !string.IsNullOrWhiteSpace(newSchool.FullNameAlt) && !string.IsNullOrWhiteSpace(newSchool.Motto)
                && !string.IsNullOrWhiteSpace(newSchool.PhoneNo);
        }

        public override void Reset()
        {
            newSchool = new ApplicationModel(Lib.Properties.Settings.Default.Info);
        }

        public Action CloseWindowAction
        {
            get;
            set;
        }

        public ICommand SaveCommand
        {
            get;
            private set;
        }
        public ICommand BrowseCommand
        {
            get;
            private set;
        }
    }
}
