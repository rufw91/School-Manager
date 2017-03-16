using Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace UmanyiSMS.ViewModels
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
