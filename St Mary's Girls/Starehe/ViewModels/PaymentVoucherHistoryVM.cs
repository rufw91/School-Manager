using Helper;
using Helper.Models;
using System.Collections.ObjectModel;
using System.Security.Permissions;
using System.Windows.Input;

namespace Starehe.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "Accounts")]
    public class PaymentVoucherHistoryVM: ViewModelBase
    {
        ObservableCollection<PaymentVoucherModel> allPaymentVouchers;
        public PaymentVoucherHistoryVM()
        {
            InitVars();
            CreateCommands();
        }
        protected async override void InitVars()
        {
            Title = "VOUCHER HISTORY";
            AllPaymentVouchers = await DataAccess.GetAllPaymentVouchersAsync();
        }

        protected override void CreateCommands()
        {
            RefreshCommand = new RelayCommand(async o => 
            {
                AllPaymentVouchers = await DataAccess.GetAllPaymentVouchersAsync();
            }, 
            o => true);
        }
        public ObservableCollection<PaymentVoucherModel> AllPaymentVouchers
        {
            get { return this.allPaymentVouchers; }

            set
            {
                if (value != this.allPaymentVouchers)
                {
                    this.allPaymentVouchers = value;
                    NotifyPropertyChanged("AllPaymentVouchers");
                }
            }
        }
        public ICommand RefreshCommand
        {
            get;
            private set;
        }
        public override void Reset()
        {
            
        }
    }
}
