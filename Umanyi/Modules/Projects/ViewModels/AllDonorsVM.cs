using Helper;
using Helper.Models;
using System;
using System.Collections.ObjectModel;
using System.Security.Permissions;
using System.Windows.Input;

namespace UmanyiSMS.Modules.Projects.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "Accounts")]
    public class AllDonorsVM : ViewModelBase
    {
        public ICommand RefreshCommand
        {
            get;
            private set;
        }

        public ObservableCollection<DonorListModel> AllDonors
        {
            get;
            private set;
        }

        public AllDonorsVM()
        {
            this.InitVars();
            this.CreateCommands();
        }

        protected override async void InitVars()
        {
            base.Title = "ALL DONORS";
            base.IsBusy = true;
            this.AllDonors = await DataAccess.GetAllDonorsAsync();
            base.NotifyPropertyChanged("AllDonors");
            base.IsBusy = false;
        }

        protected override void CreateCommands()
        {
            this.RefreshCommand = new RelayCommand(async delegate(object o)
            {
                base.IsBusy = true;
                this.AllDonors = await DataAccess.GetAllDonorsAsync();
                base.NotifyPropertyChanged("AllDonors");
                base.IsBusy = false;
            }, (object o) => !base.IsBusy);
        }

        public override void Reset()
        {
        }
    }
}
