

using System;
using System.Collections.ObjectModel;
using System.Security.Permissions;
using System.Windows.Input;
using UmanyiSMS.Lib;
using UmanyiSMS.Lib.Presentation;
using UmanyiSMS.Modules.Projects.Models;
using UmanyiSMS.Modules.Projects.Controller;

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
            this.AllDonors = await DataController.GetAllDonorsAsync();
            base.NotifyPropertyChanged("AllDonors");
            base.IsBusy = false;
        }

        protected override void CreateCommands()
        {
            this.RefreshCommand = new RelayCommand(async delegate(object o)
            {
                base.IsBusy = true;
                this.AllDonors =  await DataController.GetAllDonorsAsync();
                base.NotifyPropertyChanged("AllDonors");
                base.IsBusy = false;
            }, (object o) => !base.IsBusy);
        }

        public override void Reset()
        {
        }
    }
}
