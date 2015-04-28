using Helper;
using Helper.Models;
using System.Security.Permissions;

namespace UmanyiSMS.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "User")]
    public class InstitutionInfoVM: ViewModelBase
    {
        public InstitutionInfoVM()
        {
            Title = "INSTITUTION INFO";
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
