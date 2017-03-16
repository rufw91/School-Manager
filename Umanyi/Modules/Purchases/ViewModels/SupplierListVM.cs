using Helper;
using Helper.Models;
using System.Collections.ObjectModel;
using System.Security.Permissions;
using System.Windows.Input;

namespace UmanyiSMS.ViewModels
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
            AllSuppliers = await DataAccess.GetAllSuppliersFullAsync();
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
            AllSuppliers = await DataAccess.GetAllSuppliersFullAsync();
        }

    }
}
