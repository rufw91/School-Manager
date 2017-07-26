using Helper;
using Helper.Models;
using System;
using System.Security.Permissions;
using System.Windows;
using System.Windows.Input;

namespace UmanyiSMS.Modules.Projects.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "Accounts")]
    public class NewDonorVM : ViewModelBase
    {
        private DonorModel newDonor;

        public ICommand SaveCommand
        {
            get;
            private set;
        }

        public DonorModel NewDonor
        {
            get
            {
                return this.newDonor;
            }
            set
            {
                if (value != this.newDonor)
                {
                    this.newDonor = value;
                    base.NotifyPropertyChanged("NewDonor");
                }
            }
        }

        public NewDonorVM()
        {
            this.InitVars();
            this.CreateCommands();
        }

        protected override void InitVars()
        {
            base.Title = "NEW DONOR";
            base.IsBusy = true;
            this.NewDonor = new DonorModel();
            base.IsBusy = false;
        }

        protected override void CreateCommands()
        {
            this.SaveCommand = new RelayCommand(async delegate(object o)
            {
                base.IsBusy = true;
                bool flag = await DataAccess.SaveNewDonorAsync(this.newDonor);
                if (flag)
                {
                    MessageBox.Show("Succesfully saved data.", "", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                    this.Reset();
                }
                else
                {
                    MessageBox.Show("An error occured. Could not save details at this Time.\r\n Error: Database Access Failure.", "", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                }
                base.IsBusy = false;
            }, (object o) => this.CanSave());
        }

        private bool CanSave()
        {
            return !base.IsBusy && !string.IsNullOrWhiteSpace(this.newDonor.NameOfDonor) && !string.IsNullOrWhiteSpace(this.newDonor.PhoneNo);
        }

        public override void Reset()
        {
            this.newDonor.Reset();
        }
    }
}
