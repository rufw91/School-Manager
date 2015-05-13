using Helper;
using System.Security.Permissions;

namespace UmanyiSMS.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "Accounts")]
    public class InventoryMgmtVM : ParentViewModel
    {
        public InventoryMgmtVM()
        {

            TryAddChild(typeof(ItemsVM));
            TryAddChild(typeof(SuppliersVM));
            TryAddChild(typeof(StockTakingsVM));
            TryAddChild(typeof(VATsVM));
        }
    }
}
