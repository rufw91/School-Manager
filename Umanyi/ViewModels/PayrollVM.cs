using Helper;
using System.Security.Permissions;

namespace UmanyiSMS.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "Accounts")]
    public class PayrollVM : ParentViewModel
    {
        public PayrollVM()
        {            
            TryAddChild(typeof(NewPaySlipVM));
            TryAddChild(typeof(ReprintPaySlipVM));
            TryAddChild(typeof(PayslipsHistoryVM));
            TryAddChild(typeof(PAYEInfoVM));
        }
    }
}
