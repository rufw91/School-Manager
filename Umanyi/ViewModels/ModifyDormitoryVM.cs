using Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace UmanyiSMS.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "Deputy")]
    public class ModifyDormitoryVM: ViewModelBase
    {
        public ModifyDormitoryVM()
        {
            InitVars();
            CreateCommands();
        }
        protected override void InitVars()
        {
            Title = "Modify Dormitory";
        }

        protected override void CreateCommands()
        {
        }

        public override void Reset()
        {
        }
    }
}
