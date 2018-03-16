using System.Security.Permissions;
using UmanyiSMS.Lib;

namespace UmanyiSMS.Modules.MySystem.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "SystemAdmin")]
    public class ChangeSAPasswordVM : ViewModelBase
    {
        public ChangeSAPasswordVM()
        {
            InitVars();
            CreateCommands();
        }
        protected override void InitVars()
        {
            Title = "CHANGE SA PASSWORD";
        }

        protected override void CreateCommands()
        {

        }

        public override void Reset()
        {
        }
    }
}
