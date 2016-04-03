using Helper;
using Helper.Models;
using System;
using System.Linq;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Security.Permissions;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Collections.Generic;

namespace UmanyiSMS.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "Accounts")]
    public class NewFeesPaymentVM : ViewModelBase
    {
        FeePaymentModel currentPayment;
        ObservableCollection<FeePaymentModel> recentPayments;
        private FeesStructureModel currentFeesStructure;
        private FixedDocument fd;
        private decimal feesStructureTotal;
        public NewFeesPaymentVM()
        {
            InitVars();
            CreateCommands();
        }
        protected override void CreateCommands()
        {
            RemoveItemCommand = new RelayCommand(o => 
            {
                currentFeesStructure.Entries.RemoveAt((int)o);
            }, o => 
                {
                    if (o!=null)
                    return ((int)o) > -1;
                    return false;
                });
            PreviewCommand  = new RelayCommand(async o=>
            {
                IsBusy = true;
                SaleModel sm = new SaleModel();

                if (!await DataAccess.HasInvoicedThisTerm(currentPayment.StudentID))
                {
                    sm = new SaleModel();
                    sm.CustomerID = currentPayment.StudentID;
                    sm.DateAdded = currentPayment.DatePaid;
                    sm.EmployeeID = 0;
                    sm.SaleItems = currentFeesStructure.Entries;
                    sm.RefreshTotal();
                }
                else
                {
                    sm = await DataAccess.GetThisTermInvoice(currentPayment.StudentID);
                    sm.CustomerID = currentPayment.StudentID;
                    sm.DateAdded = currentPayment.DatePaid;
                    sm.EmployeeID = 0;
                    sm.SaleItems = currentFeesStructure.Entries;
                    sm.RefreshTotal();
                }
                MessageBox.Show("Dont forget to save the transaction!!!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                currentPayment.FeePaymentID = await DataAccess.GetLastPaymentIDAsync(currentPayment.StudentID, currentPayment.DatePaid);
                FeePaymentReceiptModel fprm = await DataAccess.GetReceiptAsync(currentPayment, new ObservableImmutableList<FeesStructureEntryModel>(sm.SaleItems));
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
                if (!await DataAccess.HasInvoicedThisTerm(currentPayment.StudentID))
                {
                    SaleModel sm = new SaleModel();
                    sm.CustomerID = currentPayment.StudentID;
                    sm.DateAdded = currentPayment.DatePaid;
                    sm.EmployeeID = 0;
                    sm.SaleItems = currentFeesStructure.Entries;
                    sm.RefreshTotal();
                    
                    succ = await DataAccess.SaveNewFeesPaymentAsync(currentPayment,sm);
                }
                else
                succ = await DataAccess.SaveNewFeesPaymentAsync(currentPayment);
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
                if (!await DataAccess.HasInvoicedThisTerm(currentPayment.StudentID))
                {
                    sm = new SaleModel();
                    sm.CustomerID = currentPayment.StudentID;
                    sm.DateAdded = currentPayment.DatePaid;
                    sm.EmployeeID = 0;
                    sm.SaleItems = currentFeesStructure.Entries;
                    sm.RefreshTotal();
                    succ = await DataAccess.SaveNewFeesPaymentAsync(currentPayment, sm);
                }
                else
                    succ = await DataAccess.SaveNewFeesPaymentAsync(currentPayment);
                    sm = await DataAccess.GetThisTermInvoice(currentPayment.StudentID);
                
                if (succ)
                {
                    MessageBox.Show("Successfully saved details.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    currentPayment.FeePaymentID = await DataAccess.GetLastPaymentIDAsync(currentPayment.StudentID,currentPayment.DatePaid);                    
                    FeePaymentReceiptModel fprm = await DataAccess.GetReceiptAsync(currentPayment, new ObservableImmutableList<FeesStructureEntryModel>(sm.SaleItems));
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

        public List<string> PaymentMethods
        {
            get;
            set;
        }

        protected override void InitVars()
        {
            Title = "NEW FEES PAYMENT";
            CurrentPayment = new FeePaymentModel();
            RecentPayments = new ObservableCollection<FeePaymentModel>();
            this.PaymentMethods = new List<string>
            {
                "M-PESA",
                "CASH",
                "CHEQUE",
                "BANK DEPOSIT/TRANSFER",
                "MONEY ORDER",
                "OTHER"
            };
            base.NotifyPropertyChanged("PaymentMethods");

            currentPayment.PropertyChanged += async (o, e) =>
            {
                if (e.PropertyName != "StudentID")
                    return;
                if (currentPayment.StudentID > 0)
                    currentPayment.CheckErrors();
                if (!currentPayment.HasErrors)
                {
                    bool hasInv = await DataAccess.HasInvoicedThisTerm(currentPayment.StudentID);
                    if (hasInv)
                        CurrentFeesStructure = new FeesStructureModel() { Entries = (await DataAccess.GetThisTermInvoice(currentPayment.StudentID)).SaleItems };
                    else
                    {
                        var s = await DataAccess.GetClassIDFromStudentID(currentPayment.StudentID);
                        CurrentFeesStructure = await DataAccess.GetFeesStructureAsync(s, DateTime.Now);
                    }
                    RefreshRecentPayments();
                    FeesStructureTotal = 0;
                    foreach (var v in currentFeesStructure.Entries)
                        FeesStructureTotal += v.Amount;
                    currentFeesStructure.Entries.CollectionChanged += (o1, e1) =>
                    {
                        FeesStructureTotal = 0;
                        foreach (var v in currentFeesStructure.Entries)
                            FeesStructureTotal += v.Amount;
                    };
                }

            };
        }

        public override void Reset()
        {
            currentPayment.Reset();
            recentPayments.Clear();
        }

        private async void RefreshRecentPayments()
        {
            RecentPayments = await DataAccess.GetRecentPaymentsAsync(currentPayment);
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

        public FeesStructureModel CurrentFeesStructure
        {
            get { return currentFeesStructure; }

            set
            {
                if (value != currentFeesStructure)
                {
                    currentFeesStructure = value;
                    NotifyPropertyChanged("CurrentFeesStructure");
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
            return !currentPayment.HasErrors &&
                (currentPayment.AmountPaid > 0);
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
