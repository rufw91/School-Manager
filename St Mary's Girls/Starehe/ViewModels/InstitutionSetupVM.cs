using Helper;
using Helper.Models;
using System.Security.Permissions;
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
            newSchool = Helper.Properties.Settings.Default.Info;
        }

        public ApplicationModel NewSchool
        {
            get { return this.newSchool; }
        }

        protected override void CreateCommands()
        {
            SaveCommand = new RelayCommand(o => 
            {
                Helper.Properties.Settings.Default.Info = newSchool;
                Helper.Properties.Settings.Default.Save();
            }, o => !IsBusy);
        }

        public override void Reset()
        {
            newSchool = Helper.Properties.Settings.Default.Info;
        }

        public ICommand SaveCommand
        {
            get;
            private set;
        }
    }
}
