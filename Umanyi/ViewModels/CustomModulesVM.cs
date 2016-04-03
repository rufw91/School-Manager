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
    public class CustomModulesVM : ParentViewModel
    {
        public CustomModulesVM()
        {
            TryAddChild(typeof(SyncCloudVM));
            TryAddChild(typeof(QuickBooksSyncVM));
            TryAddChild(typeof(AccountsVM));
        }
    }
}
