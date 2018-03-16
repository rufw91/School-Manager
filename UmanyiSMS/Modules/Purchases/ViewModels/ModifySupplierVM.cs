using System.Security.Permissions;
using System.Windows;
using System.Windows.Input;
using UmanyiSMS.Lib;
using UmanyiSMS.Lib.Presentation;
using UmanyiSMS.Modules.Purchases.Controller;
using UmanyiSMS.Modules.Purchases.Models;

namespace UmanyiSMS.Modules.Purchases.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "Accounts")]
    public class ModifySupplierVM : ViewModelBase
    {
        ModifySupplierModel newSupplier;
        public ModifySupplierVM()
        {
            InitVars();
            CreateCommands();
        }

        protected override void InitVars()
        {
            IsBusy = true;
            Title = "MODIFY SUPPLIER";            
            NewSupplier = new ModifySupplierModel();
            IsBusy = false;
        }

        protected override void CreateCommands()
        {
            UpdateCommand = new RelayCommand(async o =>
            {
                IsBusy = true;
                if (MessageBoxResult.Yes == MessageBox.Show("WARNING: This action cannt be reversed. All the supplier details will be lost. ",
                    "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning))
                {
                    bool sq = await DataController.UpdateSupplierAsync(newSupplier);
                    if (sq)
                    {
                        MessageBox.Show("Succesfully Updated Supplier.", "", MessageBoxButton.OK,
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
            newSupplier.CheckErrors();
            return !newSupplier.HasErrors&&!string.IsNullOrWhiteSpace(newSupplier.NameOfSupplier) &&
                    !string.IsNullOrWhiteSpace(newSupplier.PhoneNo);
        }

        public ICommand UpdateCommand
        { get; private set; }

        public ModifySupplierModel NewSupplier
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
            NewSupplier.Reset();
        }

    }
}
