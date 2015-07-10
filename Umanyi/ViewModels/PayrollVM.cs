using Helper;
using System.Security.Permissions;

namespace UmanyiSMS.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "Accounts")]
    public class PayrollVM : ParentViewModel
    {
        public PayrollVM()
        {
            TryAddChild(typeof(NewEmployeePaymentVM));
            TryAddChild(typeof(PaymentHistoryVM));
            TryAddChild(typeof(NewPaySlipVM));
            TryAddChild(typeof(ReprintPaySlipVM));
            TryAddChild(typeof(PAYEInfoVM));
        }
    }
}
