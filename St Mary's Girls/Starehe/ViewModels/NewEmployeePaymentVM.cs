using Helper;
using Helper.Models;
using System;
using System.Security.Permissions;
using System.Windows;
using System.Windows.Input;

namespace Starehe.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "Accounts")]
    public class NewEmployeePaymentVM :ViewModelBase
    {
        EmployeePaymentModel newPayment;
        public NewEmployeePaymentVM()
        {
            InitVars();
            CreateCommands();
        }
        protected override void InitVars()
        {
            Title = "NEW PAYMENT";
            NewPayment = new EmployeePaymentModel();
        }

        protected override void CreateCommands()
        {
            SaveCommand = new RelayCommand(async o => 
            {
                bool succ = await DataAccess.SaveNewEmployeePaymentAsync(newPayment);
                if (succ)
                {
                    MessageBox.Show("Successfully saved details.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);                    
                    Reset();

                }
                else
                    MessageBox.Show("Could not save details.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }, o => !IsBusy && CanSave());
        }

        private bool CanSave()
        {
            newPayment.CheckErrors();
            return newPayment.StaffID > 0 && newPayment.Amount > 0 && !newPayment.HasErrors;
        }

        public EmployeePaymentModel NewPayment
        {
            get { return newPayment; }
            private set
            {
                if (value != this.newPayment)
                {
                    this.newPayment = value;
                    NotifyPropertyChanged("NewPayment");
                }
            }
        }

        public override void Reset()
        {
            newPayment.Reset();
        }

        public ICommand SaveCommand
        { get; private set; }
    }
}
