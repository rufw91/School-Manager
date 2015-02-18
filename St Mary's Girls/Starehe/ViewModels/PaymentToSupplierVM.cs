using Helper;
using Helper.Models;
using System.Collections.ObjectModel;
using System.Security.Permissions;
using System.Windows;
using System.Windows.Input;

namespace Starehe.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "Accounts")]
    public class PaymentToSupplierVM: ViewModelBase
    {
       SupplierPaymentModel newPayment;
        ObservableCollection<SupplierBaseModel> allSuppliers;
        public PaymentToSupplierVM()
        {
            InitVars();
            CreateCommands();
        }

        protected async override void InitVars()
        {
            IsBusy = true;
            NewPayment = new SupplierPaymentModel();
            AllSuppliers = await DataAccess.GetAllSuppliersAsync();
            IsBusy = false;
        }

        protected override void CreateCommands()
        {
            
            SaveCommand = new RelayCommand(async o => 
            {
                IsBusy = true;
                bool succ = await DataAccess.SaveNewSupplierPaymentAsync(newPayment);
                if (succ)
                {
                    MessageBox.Show("Successfully saved.");
                    Reset();
                }
                else
                {
                    MessageBox.Show("Could not save details please ensure you have filled all entries correctly.");
                    return;
                }
                IsBusy = false;
            }, o => !IsBusy&&CanSave());
        }

        private bool CanSave()
        {
            return newPayment.SupplierID > 0 &&
                    newPayment.Amount > 0 && newPayment.DatePaid != null;
        }

        public ICommand SaveCommand
        {
            get;
            private set;
        }

        public SupplierPaymentModel NewPayment
        {
            get { return this.newPayment; }

            set
            {
                if (value != this.newPayment)
                {
                    this.newPayment = value;
                    NotifyPropertyChanged("NewPayment");
                }
            }
        }

        public ObservableCollection<SupplierBaseModel> AllSuppliers
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

        public async override void Reset()
        {
            NewPayment.Reset();
            AllSuppliers = await DataAccess.GetAllSuppliersAsync();
        }
    }
}
