using Helper;
using System.Security.Permissions;

namespace UmanyiSMS.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "Principal")]
    public class ImportWizardPage4VM : ViewModelBase
    {
        public ImportWizardPage4VM()
        {
            InitVars();
            CreateCommands();
        }
        protected override void InitVars()
        {
        }

        protected override void CreateCommands()
        {
        }

        public override void Reset()
        {
        }
    }
}
