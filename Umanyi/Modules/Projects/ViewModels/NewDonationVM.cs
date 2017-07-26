


using System;
using System.ComponentModel;
using System.Security.Permissions;
using System.Windows;
using System.Windows.Input;
using UmanyiSMS.Lib;
using UmanyiSMS.Lib.Presentation;
using UmanyiSMS.Modules.Projects.Models;

namespace UmanyiSMS.Modules.Projects.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "Accounts")]
    public class NewDonationVM : ViewModelBase
    {
        private DonationModel donation;

        public DonationModel Donation
        {
            get
            {
                return this.donation;
            }
            set
            {
                if (value != this.donation)
                {
                    this.donation = value;
                    base.NotifyPropertyChanged("Donation");
                }
            }
        }

        public string DonateToValues
        {
            get
            {
                return "";// new string[6](); // return Enum.GetValues(typeof(DonateTo));
            }
        }

        public ICommand SaveCommand
        {
            get;
            private set;
        }

        public ICommand SaveAndPrintCommand
        {
            get;
            private set;
        }

        public NewDonationVM()
        {
            this.InitVars();
            this.CreateCommands();
        }

        protected override void InitVars()
        {
            base.Title = "ENTER NEW DONATION";
            this.donation = new DonationModel();
            this.donation.PropertyChanged += delegate(object o, PropertyChangedEventArgs e)
            {
                if (e.PropertyName == "DonorID")
                {
                    this.donation.CheckErrors();
                }
            };
        }

        protected override void CreateCommands()
        {
            this.SaveCommand = new RelayCommand(async delegate(object o)
            {
                base.IsBusy = true;
                bool flag = true;// await DataAccess.SaveNewDonation(this.donation, "D");
                if (flag)
                {
                    MessageBox.Show("Successfully saved details.", "Success", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                    this.Reset();
                }
                else
                {
                    MessageBox.Show("Could not save details.", "Error", MessageBoxButton.OK, MessageBoxImage.Hand);
                }
                base.IsBusy = false;
            }, (object o) => this.CanSave());
            this.SaveAndPrintCommand = new RelayCommand(async delegate(object o)
            {
                base.IsBusy = true;
                bool flag = true;// await DataAccess.SaveNewDonation(this.donation, "D");
                if (flag)
                {
                    MessageBox.Show("Successfully saved details.", "Success", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                    this.Reset();
                }
                else
                {
                    MessageBox.Show("Could not save details.", "Error", MessageBoxButton.OK, MessageBoxImage.Hand);
                }
                base.IsBusy = false;
            }, (object o) => false);
        }

        private bool CanSave()
        {
            return !base.IsBusy && !this.donation.HasErrors && this.donation.Amount > 0m;
        }

        public override void Reset()
        {
            this.donation.Reset();
        }
    }
}
