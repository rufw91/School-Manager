
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Security.Permissions;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using UmanyiSMS.Lib;
using UmanyiSMS.Lib.Presentation;
using UmanyiSMS.Modules.Fees.Controller;
using UmanyiSMS.Modules.Fees.Models;
using UmanyiSMS.Modules.Institution.Models;

namespace UmanyiSMS.Modules.Fees.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "Accounts")]
    public class NewFeesPaymentVM : ViewModelBase
    {
        FeePaymentModel currentPayment;
        ObservableCollection<FeePaymentModel> recentPayments;
        ObservableCollection<TermModel> allTerms;
        private FixedDocument fd;
        private decimal feesStructureTotal;
        private ObservableCollection<string> pm;
        private TermModel selectedTerm;
        public NewFeesPaymentVM()
        {
            InitVars();
            CreateCommands();
        }
        protected override void CreateCommands()
        {
            PreviewCommand  = new RelayCommand(async o=>
            {
                IsBusy = true;
                SaleModel sm = new SaleModel();

                if (!await DataController.HasInvoicedOnTerm(currentPayment.StudentID,selectedTerm))
                {
                    MessageBox.Show("This student has not been billed for the current term. Please bill the student before receiving payment.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    IsBusy = false;
                    return;
                }
                else
                {
                    sm = await DataController.GetTermInvoice(currentPayment.StudentID,selectedTerm);
                    sm.CustomerID = currentPayment.StudentID;
                    sm.DateAdded = currentPayment.DatePaid;
                    sm.EmployeeID = 0;
                    sm.RefreshTotal();
                }
                MessageBox.Show("Dont forget to save the transaction!!!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                currentPayment.FeePaymentID = await DataController.GetLastPaymentIDAsync(currentPayment.StudentID);
                FeePaymentReceiptModel fprm = await DataController.GetReceiptAsync(currentPayment, new ObservableImmutableList<FeesStructureEntryModel>(sm.SaleItems));
                fprm.Entries.Where(o1 => o1.Name == "TOTAL BALANCE").First().Amount = fprm.Entries.Where(o1 => o1.Name == "TOTAL BALANCE").First().Amount - currentPayment.AmountPaid;

                Document = DocumentHelper.GenerateDocument(fprm);
                if (ShowPrintDialogAction != null)
                    ShowPrintDialogAction.Invoke(Document);
                IsBusy = false;
            }, o => CanSavePayment());
            SaveCommand = new RelayCommand(async o =>
            {
                IsBusy = true;
                bool succ=true;
                if (!await DataController.HasInvoicedOnTerm(currentPayment.StudentID,selectedTerm))
                {
                    MessageBox.Show("This student has not been billed for the current term. Please bill the student before receiving payment.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    IsBusy = false;
                    return;
                }
                else
                succ = await DataController.SaveNewFeesPaymentAsync(currentPayment);
                if (succ)
                {
                    MessageBox.Show("Successfully saved details.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    RefreshRecentPayments();
                    Reset();

                }
                else
                    MessageBox.Show("Could not save details.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                IsBusy = false;
            },
               o => CanSavePayment());

            SaveAndPrintCommand = new RelayCommand(async o =>
            {
                IsBusy = true;
                bool succ = true;
                SaleModel sm;
                if (!await DataController.HasInvoicedOnTerm(currentPayment.StudentID,selectedTerm))
                {
                    MessageBox.Show("This student has not been billed for the current term. Please bill the student before receiving payment.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    IsBusy = false;
                    return;
                }
                else
                    succ = await DataController.SaveNewFeesPaymentAsync(currentPayment);
                    sm = await DataController.GetTermInvoice(currentPayment.StudentID,selectedTerm);
                
                if (succ)
                {
                    MessageBox.Show("Successfully saved details.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    currentPayment.FeePaymentID = await DataController.GetLastPaymentIDAsync(currentPayment.StudentID);                    
                    FeePaymentReceiptModel fprm = await DataController.GetReceiptAsync(currentPayment, new ObservableImmutableList<FeesStructureEntryModel>(sm.SaleItems));
                    RefreshRecentPayments();                    
                    
                    Document = DocumentHelper.GenerateDocument(fprm);
                    if (ShowPrintDialogAction != null)
                        ShowPrintDialogAction.Invoke(Document);
                    Reset();
                }
                else
                    MessageBox.Show("Could not save details.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                IsBusy = false;
            },
               o => CanSavePayment());
        }

        public ObservableCollection<string> PaymentMethods
        {
            get { return this.pm; }

            private set
            {
                if (value != this.pm)
                {
                    this.pm = value;
                    NotifyPropertyChanged("PaymentMethods");
                }
            }
        }

        public ObservableCollection<TermModel> AllTerms
        {
            get { return this.allTerms; }

            private set
            {
                if (value != this.allTerms)
                {
                    this.allTerms = value;
                    NotifyPropertyChanged("AllTerms");
                }
            }
        }

        protected async override void InitVars()
        {
            Title = "NEW FEES PAYMENT";
            CurrentPayment = new FeePaymentModel();
            RecentPayments = new ObservableCollection<FeePaymentModel>();
            this.PaymentMethods = new ObservableCollection<string>
            {
                "M-PESA",
                "CASH",
                "CHEQUE",
                "BANK DEPOSIT/TRANSFER",
                "MONEY ORDER",
                "OTHER"
            };

            AllTerms =await Institution.Controller.DataController.GetAllTermsAsync();
            currentPayment.PropertyChanged += OnPropertyChanged;
            this.PropertyChanged += OnPropertyChanged;
        }

        private async void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != "StudentID" && e.PropertyName != "SelectedTerm")
                return;
            if (currentPayment.StudentID > 0)
                currentPayment.CheckErrors();
            if (!currentPayment.HasErrors && selectedTerm != null)
            {
                var s = (await DataController.GetTermInvoice(currentPayment.StudentID, selectedTerm)).SaleItems;
                if (s.Count > 0)
                    foreach (var v in s)
                        FeesStructureTotal += v.Amount;
                else FeesStructureTotal = 0;
                RefreshRecentPayments();
            }
        }



        public override void Reset()
        {
            currentPayment.Reset();
            recentPayments.Clear();
        }

        private async void RefreshRecentPayments()
        {
            RecentPayments = await DataController.GetRecentPaymentsAsync(currentPayment);
        }

        public FeePaymentModel CurrentPayment
        {
            get { return currentPayment; }

            set
            {
                if (value != currentPayment)
                {
                    currentPayment = value;
                    NotifyPropertyChanged("CurrentPayment");
                }
            }
        }

        public decimal FeesStructureTotal
        {
            get { return feesStructureTotal; }

            private set
            {
                if (value != feesStructureTotal)
                {
                    feesStructureTotal = value;
                    NotifyPropertyChanged("FeesStructureTotal");
                }
            }
        }

        public TermModel SelectedTerm
        {
            get { return selectedTerm; }

            set
            {
                if (value != selectedTerm)
                {
                    selectedTerm = value;
                    NotifyPropertyChanged("SelectedTerm");
                }
            }
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

        public ObservableCollection<FeePaymentModel> RecentPayments
        {
            get { return recentPayments; }
            set
            {
                if (value != recentPayments)
                {
                    recentPayments = value;
                    NotifyPropertyChanged("RecentPayments");
                }
            }
        }

        private bool CanSavePayment()
        {
            return !currentPayment.HasErrors && selectedTerm!=null&&
                (currentPayment.AmountPaid > 0)&&!string.IsNullOrWhiteSpace(currentPayment.PaymentMethod);
        }

        public Action<FixedDocument> ShowPrintDialogAction
        {
            get;
            set;
        }

        public ICommand RemoveItemCommand
        { get; private set; }

        public ICommand SaveCommand
        {
            get;
            private set;
        }
        public ICommand PreviewCommand
        {
            get;
            private set;
        }
        public ICommand SaveAndPrintCommand
        {
            get;
            private set;
        }

        public string Error
        {
            get { return null; }
        }

    }
}
