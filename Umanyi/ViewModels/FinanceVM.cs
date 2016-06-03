﻿using Helper;
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
            TryAddChild(typeof(FeesStatementVM),false);
            TryAddChild(typeof(ViewFeesStructureVM));
            TryAddChild(typeof(ViewFullFeesStructureVM));
            TryAddChild(typeof(SetFeesStructureVM));
            TryAddChild(typeof(ReprintReceiptVM));
            TryAddChild(typeof(BalancesListVM),false);
            TryAddChild(typeof(RemovePaymentVM));
            TryAddChild(typeof(RemoveBillVM));
            TryAddChild(typeof(NewPaymentVoucherVM));
            TryAddChild(typeof(PaymentVoucherHistoryVM));
            TryAddChild(typeof(FeesPaymentHistoryVM),false);
            TryAddChild(typeof(PaymentsByVoteHeadVM),false);
        }

        internal void ToggleDonationsVisible(bool isVisible)
        {
            if (isVisible)
            {
               // TryAddChild(typeof(NewDonorVM), false);25.5.2016
                //TryAddChild(typeof(PaymentsByVoteHeadVM), false);
            }
        }
    }
}
