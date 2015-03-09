using Helper;
using Helper.Models;
using System;
using System.Security.Permissions;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;

namespace Starehe.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "Accounts")]
    public class NewPaymentVoucherVM:ViewModelBase
    {
        PaymentVoucherModel currentVoucher;
        PaymentVoucherEntryModel newEntry;
        public NewPaymentVoucherVM()
        {
            InitVars();
            CreateCommands();
        }
        protected override void InitVars()
        {
            Title = "NEW PAYMENT VOUCHER";
            currentVoucher = new PaymentVoucherModel();
            NewEntry = new PaymentVoucherEntryModel();
        }

        protected override void CreateCommands()
        {
            AddEntryCommand = new RelayCommand(o =>
            {
                currentVoucher.Entries.Add(newEntry);
                NewEntry = new PaymentVoucherEntryModel();
            },
            o => CanAddNewEntry());
            SaveCommand = new RelayCommand(async o =>
            {
                bool succ = await DataAccess.SaveNewPaymentVoucher(currentVoucher);
                MessageBox.Show(succ ? "Successfully saved details" : "Could not save details.", succ ? "Success" : "Error",
                    MessageBoxButton.OK, succ ? MessageBoxImage.Information : MessageBoxImage.Warning);
                if (succ)
                    Reset();
            }, o => CanSave());

            SaveAndPrintCommand = new RelayCommand(async o =>
            {
                bool succ = await DataAccess.SaveNewPaymentVoucher(currentVoucher);
                MessageBox.Show(succ ? "Successfully saved details" : "Could not save details.", succ ? "Success" : "Error",
                    MessageBoxButton.OK, succ ? MessageBoxImage.Information : MessageBoxImage.Warning);
                if (succ)
                {Reset();

                    Document = DocumentHelper.GenerateDocument(currentVoucher);
                    if (ShowPrintDialogAction != null)
                        ShowPrintDialogAction.Invoke(Document);

                }
            },
               o => CanSave());
        }
        private FixedDocument Document
        {
            get;
            set;
        }
        private bool CanAddNewEntry()
        {
            return !string.IsNullOrWhiteSpace(newEntry.Description)
                && newEntry.Amount > 0
                && newEntry.DatePaid != null ;
        }
        private bool CanSave()
        {
            return currentVoucher.Entries.Count > 0;
        }
        

        public PaymentVoucherEntryModel NewEntry
        {
            get { return newEntry; }
            set
            {
                if (value != newEntry)
                {
                    newEntry = value;
                    NotifyPropertyChanged("NewEntry");
                }
            }
        }

        public PaymentVoucherModel CurrentVoucher
        { get { return currentVoucher; } }
        public Action<FixedDocument> ShowPrintDialogAction
        {
            get;
            set;
        }
        public ICommand AddEntryCommand
        {
            get;
            private set;
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
        public override void Reset()
        {
            currentVoucher.Reset();
            newEntry.Reset();
        }
    }
}
