using Helper;
using System.Security.Permissions;

namespace UmanyiSMS.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "Accounts")]
    public class InventoryMgmtVM : ParentViewModel
    {
        private ViewModelBase activeView;
        public InventoryMgmtVM()
        {

            TryAddChild(typeof(ItemsVM));
            TryAddChild(typeof(BooksVM));
            TryAddChild(typeof(SuppliersVM));
            TryAddChild(typeof(StockTakingsVM));
            //TryAddChild(typeof(VATsVM));
            ActiveView = new ItemsVM();
        }

        public new ViewModelBase ActiveView
        {
            get
            {
                return activeView;
            }
            set
            {
                if (activeView != value)
                {
                    if (activeView != null)
                        activeView.IsActive = false;
                    activeView = value;
                    if (activeView != null)
                        activeView.IsActive = true;
                    NotifyPropertyChanged("ActiveView");
                }
            }
        }
    }
}
