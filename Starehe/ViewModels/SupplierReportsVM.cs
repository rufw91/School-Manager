using Helper;
using System.Collections.ObjectModel;
using System.Security.Permissions;

namespace UmanyiSMS.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "Accounts")]
    public class SupplierReportsVM:ViewModelBase
    {
        public SupplierReportsVM()
        {
            InitVars();
            CreateCommands();
        }

        protected override void InitVars()
        {
            MenuItems = new ObservableCollection<ViewModelBase>();
            MenuItems.Add(new SupplierListVM());
            MenuItems.Add(new PaymentToSupplierHistoryVM());
        }

        protected override void CreateCommands()
        {
           
        }

        public ObservableCollection<ViewModelBase> MenuItems
        { get; private set; }

        public override void Reset()
        {
            
        }
    }
}
