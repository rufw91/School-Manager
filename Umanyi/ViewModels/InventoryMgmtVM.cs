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
            TryAddChild(typeof(BooksVM));
            TryAddChild(typeof(SuppliersVM));
            TryAddChild(typeof(StockTakingsVM));
            ActiveView = new ItemsVM();
            PersistViews = true;
        }
    }
}
