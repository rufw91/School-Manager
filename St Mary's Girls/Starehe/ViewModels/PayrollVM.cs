﻿using Helper;
using System.Security.Permissions;

namespace Starehe.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "Accounts")]
    public class PayrollVM : ParentViewModel
    {
        public PayrollVM()
        {
            TryAddChild(typeof(NewEmployeePaymentVM));
            TryAddChild(typeof(PaymentHistoryVM));
        }
    }
}
