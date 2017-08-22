

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using UmanyiSMS.Lib;
using UmanyiSMS.Lib.Presentation;
using UmanyiSMS.Modules.Projects.Models;
using UmanyiSMS.Modules.Projects.Controller;
namespace UmanyiSMS.Modules.Projects.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "Accounts")]
    public class DonationsHistoryVM:ViewModelBase
    {
        private DateTime from;
        private DateTime to;
        private ObservableCollection<DonationModel> items;
            private ObservableCollection<DonorListModel> allDonors;
        private int? selectedDonorID;
        public DonationsHistoryVM()
        {
            InitVars();
            CreateCommands();
        }
        protected async override void InitVars()
        {
            Title = "DONATIONS HISTORY";
            IsBusy = true;
            From = DateTime.Now.Date.AddDays(-5);
            To = DateTime.Now.Date;
            AllDonors = await DataController.GetAllDonorsAsync();
            NotifyPropertyChanged("AllDonors");
            Items = new ObservableCollection<DonationModel>();
            IsBusy = false;
        }

        protected override void CreateCommands()
        {
            ClearCommand = new RelayCommand(o => { SelectedDonorID = null; }, o => true);
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
            Items =  await DataController.GetDonationsAsync(selectedDonorID, from, to);
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
            SelectedDonorID = null;
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

        public int? SelectedDonorID
        {
            get { return selectedDonorID; }
            set
            {
                if (value != this.selectedDonorID)
                {
                    this.selectedDonorID = value;
                    NotifyPropertyChanged("SelectedDonorID");
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

        public ObservableCollection<DonationModel> Items
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
        public ICommand ClearCommand { get; private set; }
        public ICommand RefreshCommand { get; private set; }
        public ICommand TodayCommand { get; private set; }
        public ICommand ThisMonthCommand { get; private set; }

        public ObservableCollection<DonorListModel> AllDonors
        {
            get
            {
                return allDonors;
            }

            set
            {
                allDonors = value;
            }
        }
    }
}


