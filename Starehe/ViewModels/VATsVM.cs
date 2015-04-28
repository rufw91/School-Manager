using Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace UmanyiSMS.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "Accounts")]
    public class VATsVM:ViewModelBase
    {
        public VATsVM()
        {
            InitVars();
            CreateCommands();
        }
        protected override void InitVars()
        {
            Title = "VAT";
        }

        protected override void CreateCommands()
        {
        }

        public override void Reset()
        {
        }
    }
}
