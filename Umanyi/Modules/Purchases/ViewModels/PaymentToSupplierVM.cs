
using System;
using System.Collections.ObjectModel;
using System.Security.Permissions;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using UmanyiSMS.Lib;
using UmanyiSMS.Lib.Presentation;
using UmanyiSMS.Modules.Purchases.Models;
using UmanyiSMS.Modules.Purchases.Controller;

namespace UmanyiSMS.Modules.Purchases.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "Accounts")]
    public class PaymentToSupplierVM: ViewModelBase
    {
       SupplierPaymentModel newPayment;
        ObservableCollection<SupplierBaseModel> allSuppliers;
        private FixedDocument fd;
        private SupplierBaseModel selectedSupplier;
        public PaymentToSupplierVM()
        {
            InitVars();
            CreateCommands();
        }

        protected async override void InitVars()
        {
            Title = "NEW SUPPLIER PAYMENT";
            IsBusy = true;
            NewPayment = new SupplierPaymentModel();
            AllSuppliers = await DataController.GetAllSuppliersAsync();
            IsBusy = false;
        }

        protected override void CreateCommands()
        {
            
            SaveCommand = new RelayCommand(async o => 
            {
                IsBusy = true;
                bool succ = await DataController.SaveNewSupplierPaymentAsync(newPayment);
                if (succ)
                {
                    MessageBox.Show("Successfully saved details.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    Reset();
                }
                else
                    MessageBox.Show("Could not save details.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                IsBusy = false;
            }, o =>CanSave());


            SaveAndPrintCommand = new RelayCommand(async o =>
            {
                IsBusy = true;
                bool succ = await DataController.SaveNewSupplierPaymentAsync(newPayment);
                if (succ)
                {
                    MessageBox.Show("Successfully saved details.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    newPayment.SupplierPaymentID = await DataController.GetLastSupplierPaymentIDAsync(newPayment.SupplierID, newPayment.DatePaid);

                    Document = DocumentHelper.GenerateDocument(newPayment);
                    if (ShowPrintDialogAction != null)
                        ShowPrintDialogAction.Invoke(Document);
                    Reset();
                }
                else
                    MessageBox.Show("Could not save details.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                IsBusy = false;
            },
               o => CanSave());
        }

        public FixedDocument Document
        {
            get { return this.fd; }

            set
            {
                if (value != this.fd)
                {
                    this.fd = value;
                    NotifyPropertyChanged("Document");
                }
            }
        }

        private bool CanSave()
        {
            return !IsBusy&&newPayment.SupplierID > 0 &&
                    newPayment.AmountPaid > 0 && newPayment.DatePaid != null &&!string.IsNullOrWhiteSpace(newPayment.Notes);
        }

        public Action<FixedDocument> ShowPrintDialogAction
        {
            get;
            set;
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

        public SupplierBaseModel SelectedSupplier
        {
            get { return this.selectedSupplier; }

            set
            {
                if (value != this.selectedSupplier)
                {
                    this.selectedSupplier = value;
                    if (selectedSupplier!=null)
                    {
                        newPayment.SupplierID = selectedSupplier.SupplierID;
                        newPayment.NameOfSupplier = selectedSupplier.NameOfSupplier;
                    }
                    NotifyPropertyChanged("SelectedSupplier");
                }
            }
        } 

        public SupplierPaymentModel NewPayment
        {
            get { return this.newPayment; }

            set
            {
                if (value != this.newPayment)
                {
                    this.newPayment = value;
                    NotifyPropertyChanged("NewPayment");
                }
            }
        }

        public ObservableCollection<SupplierBaseModel> AllSuppliers
        {
            get { return this.allSuppliers; }

            private set
            {
                if (value != this.allSuppliers)
                {
                    this.allSuppliers = value;
                    NotifyPropertyChanged("AllSuppliers");
                }
            }
        }

        public async override void Reset()
        {
            NewPayment.Reset();
            AllSuppliers = await DataController.GetAllSuppliersAsync();
        }
    }
}
