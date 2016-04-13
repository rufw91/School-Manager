using Helper;
using Helper.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace UmanyiSMS.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "Accounts")]
    public class AccountsTransactionsVM : ViewModelBase
    {
        TransactionTypes selectedTransactionType;
        DateTime from;
        DateTime to;
        ObservableCollection<TransactionModel> items;
        public AccountsTransactionsVM()
        {
            InitVars();
            CreateCommands();
        }
        protected override void InitVars()
        {
            Title = "ALL TRANSACTIONS";
            IsBusy = true;
            From = DateTime.Now.Date.AddDays(-5);
            To = DateTime.Now.Date;
            Items = new ObservableCollection<TransactionModel>();
            IsBusy = false;
            AllTransactionTypes =  Enum.GetValues(typeof(TransactionTypes));
            NotifyPropertyChanged("AllTransactionTypes");
        }


        protected override void CreateCommands()
        {
            RefreshCommand = new RelayCommand(async o => { IsBusy = true; await RefreshItems(); IsBusy = false; }, o => !IsBusy && CanRefresh());
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
            Items = await DataAccess.GetAccountsTransactionHistoryAsync(selectedTransactionType,from, to);
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

        public TransactionTypes SelectedTransactionType
        {
            get { return selectedTransactionType; }
            private set
            {
                if (value != this.selectedTransactionType)
                {
                    this.selectedTransactionType = value;
                    NotifyPropertyChanged("SelectedTransactionType");
                }
            }
        }

        public Array AllTransactionTypes
        {
            get;
            private set;
        }

        public DateTime From
        {
            get { return from; }
             set
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
             set
            {
                if (value != this.to)
                {
                    this.to = value;
                    NotifyPropertyChanged("To");
                }
            }
        }

        public ObservableCollection<TransactionModel> Items
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
