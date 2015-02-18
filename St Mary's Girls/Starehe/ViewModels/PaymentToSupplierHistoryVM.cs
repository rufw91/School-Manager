using Helper;
using Helper.Models;
using System;
using System.Collections.ObjectModel;
using System.Security.Permissions;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Starehe.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "Accounts")]
    public class PaymentToSupplierHistoryVM: ViewModelBase
    {
        DateTime from;
        DateTime to;
        ObservableCollection<SupplierPaymentModel> items;
        public PaymentToSupplierHistoryVM()
        {
            InitVars();
            CreateCommands();
        }
        protected override void InitVars()
        {
            IsBusy = true;
            Title = "Suppliers Payments History";
            From = DateTime.Now.Date.AddDays(-5);
            To = DateTime.Now.Date;
            Items = new ObservableCollection<SupplierPaymentModel>();
            IsBusy = false;
        }

     
        protected override void CreateCommands()
        {
            RefreshCommand = new RelayCommand(async o => { IsBusy = true; await RefreshItems(); IsBusy = false; }, o => !IsBusy&&CanRefresh());
            TodayCommand = new RelayCommand(async o => { IsBusy = true; From = DateTime.Now.Date; To = DateTime.Now.Date.AddDays(1); await RefreshItems(); IsBusy = false; }, o => !IsBusy);
            ThisMonthCommand = new RelayCommand(async o =>
            {
                IsBusy = true;
                From = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            To = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month)); await RefreshItems(); IsBusy = false;
            IsBusy = false;
            }, o => true);
        }

        private async Task RefreshItems()
        {
            Items = await DataAccess.GetSupplierPaymentsAsync(null, from, to);
        }

        private bool CanRefresh()
        {
            return this.From < this.To;
        }

        public override void Reset()
        {
            From = DateTime.Now.Date.AddDays(-5);
            To = DateTime.Now.Date;
            Items.Clear();
        }

        public DateTime From
        {
            get { return from; }
            private set
            {
                if (value != this.from)
                {
                    this.from = value;
                    NotifyPropertyChanged("From");
                }
            }
        }

        public DateTime To
        {
            get { return to; }
            private set
            {
                if (value != this.to)
                {
                    this.to = value;
                    NotifyPropertyChanged("To");
                }
            }
        }

        public ObservableCollection<SupplierPaymentModel> Items
        {
            get { return items; }
            private set
            {
                if (value != this.items)
                {
                    this.items = value;
                    NotifyPropertyChanged("Items");
                }
            }
        }

        public ICommand RefreshCommand { get; private set; }
        public ICommand TodayCommand { get; private set; }
        public ICommand ThisMonthCommand { get; private set; }
    }
}
