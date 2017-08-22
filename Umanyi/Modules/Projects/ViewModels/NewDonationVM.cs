


using System;
using System.ComponentModel;
using System.Security.Permissions;
using System.Windows;
using System.Windows.Input;
using UmanyiSMS.Lib;
using UmanyiSMS.Lib.Presentation;
using UmanyiSMS.Modules.Projects.Models;
using UmanyiSMS.Modules.Projects.Controller;
using System.Collections.ObjectModel;

namespace UmanyiSMS.Modules.Projects.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "Accounts")]
    public class NewDonationVM : ViewModelBase
    {
        private DonationModel donation;

        private ObservableCollection<DonorListModel> allDonors;
        private int? selectedDonorID;
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

        public int? SelectedDonorID
        {
            get
            {
                return selectedDonorID;// new string[6](); // return Enum.GetValues(typeof(DonateTo));
            }
            private set
            {
                if (value != this.selectedDonorID)
                {
                    this.selectedDonorID = value;
                    NotifyPropertyChanged("SelectedDonorID");
                }
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

        public NewDonationVM()
        {
            this.InitVars();
            this.CreateCommands();
        }

        protected async override void InitVars()
        {
            base.Title = "ENTER NEW DONATION";
            this.donation = new DonationModel();
            AllDonors = await DataController.GetAllDonorsAsync();
            NotifyPropertyChanged("AllDonors");
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
                this.donation.DonorID = selectedDonorID.Value;
                bool flag =  await DataController.SaveNewDonation(this.donation, "D");
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
                bool flag = await DataController.SaveNewDonation(this.donation, "D");
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
            return !base.IsBusy && !this.donation.HasErrors && this.donation.Amount > 0m&&selectedDonorID.HasValue&&selectedDonorID>0;
        }

        public override void Reset()
        {
            SelectedDonorID = null;
            this.donation.Reset();
        }
    }
}
