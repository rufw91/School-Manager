using Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace Starehe.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "Accounts")]
    public class SuppliersVM:ViewModelBase
    {
        public SuppliersVM()
        {
            InitVars();
            CreateCommands();
        }
        protected override void InitVars()
        {
            Title = "Suppliers";
        }

        protected override void CreateCommands()
        {
        }

        public override void Reset()
        {
        }
    }
}
