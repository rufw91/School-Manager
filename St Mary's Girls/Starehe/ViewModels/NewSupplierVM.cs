using Helper;
using Helper.Models;
using System;
using System.Security.Permissions;
using System.Windows;
using System.Windows.Input;

namespace Starehe.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "Accounts")]
    public class NewSupplierVM: ViewModelBase
    {
        SupplierModel newSupplier;
        public NewSupplierVM()
        {
            InitVars();
            CreateCommands();
        }

        protected override void InitVars()
        {
            Title = "New Supplier";
            IsBusy = true;
            NewSupplier = new SupplierModel();
            IsBusy = false;
        }

        protected override void CreateCommands()
        {
            
            SaveCommand = new RelayCommand(async o =>
            {
                IsBusy = true;
                bool sq = await DataAccess.SaveNewSupplierAsync(newSupplier);
                if (sq)
                {
                    MessageBox.Show("Succesfully saved supplier.", "", MessageBoxButton.OK,
                        MessageBoxImage.Information);
                    Reset();
                }
                else
                {
                    MessageBox.Show("An error occured. Could not save details at this Time.\r\n Error: Database Access Failure.", "", MessageBoxButton.OK,
                        MessageBoxImage.Information);
                }
                IsBusy = false;
            }, o => !IsBusy&&CanSave());

        }

        private bool CanSave()
        {
            return !string.IsNullOrWhiteSpace(newSupplier.NameOfSupplier) &&
                    !string.IsNullOrWhiteSpace(newSupplier.PhoneNo);
        }

        public ICommand SaveCommand
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
            NewSupplier.Reset();
        }

    }
}
