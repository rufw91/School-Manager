using Helper;
using Helper.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Starehe.ViewModels
{
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
