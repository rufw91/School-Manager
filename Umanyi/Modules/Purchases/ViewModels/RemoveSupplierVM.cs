
using System.Security.Permissions;
using System.Windows;
using System.Windows.Input;
using UmanyiSMS.Lib;
using UmanyiSMS.Lib.Presentation;
using UmanyiSMS.Modules.Purchases.Models;
using UmanyiSMS.Modules.Purchases.Controller;

namespace UmanyiSMS.Modules.Purchases.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "Accounts")]
    public class RemoveSupplierVM : ViewModelBase
    {
        int selectedSupplierID;
        SupplierModel newSupplier;
        public RemoveSupplierVM()
        {
            InitVars();
            CreateCommands();
        }

        protected override void InitVars()
        {
            IsBusy = true;
            SelectedSupplierID = 0;
            NewSupplier = new SupplierModel();
            IsBusy = false;
        }

        public int SelectedSupplierID
        {
            get { return selectedSupplierID; }
            set
            {
                if (selectedSupplierID != value)
                {
                    selectedSupplierID = value;
                    NotifyPropertyChanged("SelectedSupplierID");
                    RefreshSupplier();
                }
            }
        }
        
        private async void RefreshSupplier()
        {
            newSupplier.Reset();
            SupplierModel cs = await DataController.GetSupplierAsync(selectedSupplierID);
            NewSupplier.Address = cs.Address;
            NewSupplier.AltPhoneNo = cs.AltPhoneNo;
            NewSupplier.City = cs.City;
            NewSupplier.SupplierID = cs.SupplierID;
            NewSupplier.NameOfSupplier = cs.NameOfSupplier;
            NewSupplier.PhoneNo = cs.PhoneNo;
            NewSupplier.Email = cs.Email;
            NewSupplier.PostalCode = cs.PostalCode;
            NewSupplier.PINNo = cs.PINNo;
        }

        protected override void CreateCommands()
        {
            
            RemoveCommand = new RelayCommand(async o =>
            {
                IsBusy = true;
                if (MessageBoxResult.Yes == MessageBox.Show("WARNING: This action cannt be reversed. All the supplier details will be lost. ",
                    "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning))
                {
                    bool sq = await DataController.RemoveSupplierAsync(newSupplier.SupplierID);
                    if (sq)
                    {
                        MessageBox.Show("Succesfully Removed Supplier.", "", MessageBoxButton.OK,
                            MessageBoxImage.Information);
                        Reset();
                    }
                    else
                    {
                        MessageBox.Show("An error occured. Could not save details at this Time.\r\n Error: Database Access Failure.", "", MessageBoxButton.OK,
                            MessageBoxImage.Information);
                    }
                }
                IsBusy = false;
            }, o => !IsBusy&&CanSave());

        }

        private bool CanSave()
        {
            return !string.IsNullOrWhiteSpace(newSupplier.NameOfSupplier) &&
                    !string.IsNullOrWhiteSpace(newSupplier.PhoneNo);
        }

        public ICommand RemoveCommand
        { get; private set; }

        public SupplierModel NewSupplier
        {
            get { return this.newSupplier; }

            set
            {
                if (value != this.newSupplier)
                {
                    this.newSupplier = value;
                    NotifyPropertyChanged("NewSupplier");
                }
            }
        }

        public override void Reset()
        {
            SelectedSupplierID = 0;
            NewSupplier.Reset();
        }

    }
}
