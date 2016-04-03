using Helper;
using System.Collections.ObjectModel;
using System.Security.Permissions;

namespace UmanyiSMS.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "User")]
    public class FinanceVM : ParentViewModel
    {
        public FinanceVM()
        {
            Title = "FINANCE";
            TryAddChild(typeof(NewFeesPaymentVM));
            TryAddChild(typeof(BillStudentVM));
            TryAddChild(typeof(FeesStatementVM));
            TryAddChild(typeof(ViewFeesStructureVM));
            TryAddChild(typeof(ViewFullFeesStructureVM));
            TryAddChild(typeof(SetFeesStructureVM));
            TryAddChild(typeof(ReprintReceiptVM));
            TryAddChild(typeof(BalancesListVM));
            TryAddChild(typeof(RemovePaymentVM));
            TryAddChild(typeof(RemoveBillVM));
            TryAddChild(typeof(NewPaymentVoucherVM));
            TryAddChild(typeof(PaymentVoucherHistoryVM));
            TryAddChild(typeof(PaymentsByVoteHeadVM));
        }
    }
}
