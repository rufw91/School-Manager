using Helper;
using System.Security.Permissions;

namespace Starehe.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "Principal")]
    public class ImportWizardPage3VM : ViewModelBase
    {
        public ImportWizardPage3VM()
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
