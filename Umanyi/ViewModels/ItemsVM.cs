using Helper;
using System;
using System.Security.Permissions;

namespace UmanyiSMS.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "Accounts")]
    public class ItemsVM : ParentViewModel
    {
        public ItemsVM()
        {
            Title = "ITEMS";
            TryAddChild(typeof(ReceiveItemsVM));
            TryAddChild(typeof(NewItemVM));
            TryAddChild(typeof(ItemListVM));
            TryAddChild(typeof(ItemReceiptHistoryVM));
            TryAddChild(typeof(ModifyItemVM));
        }
    }
}
