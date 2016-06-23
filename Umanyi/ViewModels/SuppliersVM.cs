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
    public class SuppliersVM : ParentViewModel
    {
        public SuppliersVM()
        {
            Title = "SUPPLIERS";
            TryAddChild(typeof(NewSupplierVM));
            TryAddChild(typeof(PaymentToSupplierVM));
            TryAddChild(typeof(PaymentToSupplierHistoryVM));
            TryAddChild(typeof(SupplierStatementVM));
            TryAddChild(typeof(SupplierListVM));
            TryAddChild(typeof(ModifySupplierVM));
            TryAddChild(typeof(ReprintPaymentVoucherVM));
        }
    }
}