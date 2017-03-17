using System.Collections.ObjectModel;
using System.Security.Permissions;
using System.Windows.Input;
using UmanyiSMS.Lib;
using UmanyiSMS.Lib.Presentation;
using UmanyiSMS.Modules.Purchases.Controller;
using UmanyiSMS.Modules.Purchases.Models;

namespace UmanyiSMS.Modules.Purchases.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "Accounts")]
    public class SupplierListVM : ViewModelBase
    {
        ObservableCollection<SupplierModel> allSuppliers;

        public SupplierListVM()
        {
            InitVars();
            CreateCommands();
        }

        protected override async void InitVars()
        {
            IsBusy = true;
            Title = "SUPPLIERS LIST";
            AllSuppliers = await DataController.GetAllSuppliersFullAsync();
            IsBusy = false;
        }

        protected override void CreateCommands()
        {
            RefreshCommand = new RelayCommand(o => { IsBusy = true; Reset(); IsBusy = false; }, o => !IsBusy);
        }

        public ObservableCollection<SupplierModel> AllSuppliers
        {
            get { return this.allSuppliers; }

            private set
            {
                if (value != this.allSuppliers)
                {
                    this.allSuppliers = value;
                    NotifyPropertyChanged("AllSuppliers");
                }
            }
        }

        public ICommand RefreshCommand
        {
            get;
            private set;
        }

        public async override void Reset()
        {
            AllSuppliers = await DataController.GetAllSuppliersFullAsync();
        }

    }
}
