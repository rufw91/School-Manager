using Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace UmanyiSMS.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "Teacher")]
    public class MarkListsVM: ViewModelBase    
    {
        public MarkListsVM()
        {
            InitVars();
            CreateCommands();
        }
        protected override void InitVars()
        {
            Title = "MARK LIST(S)";
        }

        protected override void CreateCommands()
        {
        }

        public override void Reset()
        {
        }
    }
}
