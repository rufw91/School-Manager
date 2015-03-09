using Helper;
using Helper.Models;
using System.Security.Permissions;
using System.Windows;
using System.Windows.Input;

namespace Starehe.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "Deputy")]
    public class InstitutionSetupVM: ViewModelBase
    {
        ApplicationModel newSchool;

        public InstitutionSetupVM()
        {
            InitVars();
            CreateCommands();
        }

        protected override void InitVars()
        {
            Title = "INSTITUTION SETUP";
            newSchool =new ApplicationModel( Helper.Properties.Settings.Default.Info);
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
                Helper.Properties.Settings.Default.Info = new ApplicationPersistModel(newSchool);
                Helper.Properties.Settings.Default.Save();
                App.Info.CopyFrom(new ApplicationModel(Helper.Properties.Settings.Default.Info));
                if (MessageBoxResult.Yes == MessageBox.Show("You may need to restart the School Management system for all changes to be saved. Do you want to restart now? ",
                    "Warning", MessageBoxButton.YesNo, MessageBoxImage.Information))
                {
                    App.Restart();
                }
            }, o => !IsBusy);
        }

        public override void Reset()
        {
            newSchool = new ApplicationModel(Helper.Properties.Settings.Default.Info);
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
