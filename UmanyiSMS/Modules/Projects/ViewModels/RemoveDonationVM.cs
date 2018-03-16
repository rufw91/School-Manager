using System;
using System.Collections.ObjectModel;
using System.Security.Permissions;
using System.Windows;
using System.Windows.Input;
using UmanyiSMS.Lib;
using UmanyiSMS.Modules.Institution.Models;
using UmanyiSMS.Modules.Projects.Models;
using UmanyiSMS.Modules.Projects.Controller;
using UmanyiSMS.Lib.Presentation;
using UmanyiSMS.Lib.Controllers;

namespace UmanyiSMS.Modules.Projects.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "Accounts")]
    public class RemoveDonationVM : ViewModelBase
    {
        int selectedDonorID;
        private DonationModel selectedDonation;
        private ObservableCollection<TermModel> allTerms;
        public RemoveDonationVM()
        {
            InitVars();
            CreateCommands();
        }

        public int SelectedDonorID
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
        

        protected override void InitVars()
        {
            Title = "REMOVE EXAM";
            AllDonations = new ObservableCollection<DonationModel>();
            PropertyChanged += async (o, e) =>
            {
                if (e.PropertyName == "SelectedDonorID")
                {
                    AllDonations = await DataController.GetDonationsAsync(selectedDonorID, null, null);
                        NotifyPropertyChanged("AllDonations");
                    }
                    return;
                
            };
        }

        protected override void CreateCommands()
        {
            RemoveCommand = new RelayCommand(async o =>
            {
                if (MessageBoxResult.Yes == MessageBox.Show("This action is IRREVERSIBLE.\r\nAre you sure you would like to continue?", "Information", MessageBoxButton.YesNo, MessageBoxImage.Warning))
                {
                    if (MessageBoxResult.Yes == MessageBox.Show("Are you ABSOLUTELY sure you would like to delete this donation?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Information))
                    {
                        bool succ = await DataController.RemoveDonationAsync(selectedDonation.DonationID);
                        MessageBox.Show(succ ? "Successfully completed operation" : "Operation failed!", succ ? "Success" : "Error", MessageBoxButton.OK, succ ? MessageBoxImage.Information : MessageBoxImage.Error);
                        if (succ)
                            Reset();
                    }
                }
            }, o => CanRemove());
        }

        private bool CanRemove()
        {
            return selectedDonorID > 0 && selectedDonation!=null;
        }

        public ICommand RemoveCommand
        {
            get;
            private set;
        }

        public override void Reset()
        {
            SelectedDonorID = 0;
        }
        public DonationModel SelectedDonation
        {
            get { return this.selectedDonation; }

            set
            {
                if (value != this.selectedDonation)
                {
                    this.selectedDonation = value;
                    NotifyPropertyChanged("SelectedDonation");
                }
            }
        }
        public ObservableCollection<DonationModel> AllDonations { get; set; }
    }
}
