using Helper;
using Helper.Models;
using System;
using System.Collections.ObjectModel;
using System.Security.Permissions;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Input;
using UmanyiSMS.Lib;
using UmanyiSMS.Lib.Presentation;
using UmanyiSMS.Modules.Fees.Controller;
using UmanyiSMS.Modules.Fees.Models;
using UmanyiSMS.Modules.Students.Models;

namespace UmanyiSMS.Modules.Fees.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "Accounts")]
    public class ReprintReceiptVM:ViewModelBase
    {
        private FixedDocument fd;
        ObservableCollection<FeePaymentModel> recentPayments;
        private StudentSelectModel selectedStudent;
        private FeePaymentModel selectedPayment;

        public ReprintReceiptVM()
        {
            InitVars();
            CreateCommands();
        }

        protected override void InitVars()
        {
            Title = "REPRINT RECEIPT";
            RecentPayments = new ObservableCollection<FeePaymentModel>();
            SelectedStudent = new StudentSelectModel();
            selectedStudent.PropertyChanged += async (o, e) =>
                {
                    if ((e.PropertyName == "StudentID") && (selectedStudent.StudentID > 0))
                    {
                        selectedStudent.CheckErrors();
                        await RefreshRecentPayments();
                    }

                };
        }

        protected override void CreateCommands()
        {
            FullPreviewCommand = new RelayCommand(async o=>
            {
                var fs = await DataController.GetTermInvoice(selectedPayment.StudentID, selectedPayment.DatePaid);
                var temp = await DataController.GetReceiptAsync(selectedPayment, new ObservableImmutableList<FeesStructureEntryModel>(fs.SaleItems));
                var doc = DocumentHelper.GenerateDocument(temp);
                if (ShowPrintDialogAction != null)
                    ShowPrintDialogAction.Invoke(doc);
            }, o => CanGenerate() && Document!=null);
            GenerateCommand = new RelayCommand(async o =>
            {
                var fs = await DataController.GetTermInvoice(selectedPayment.StudentID, selectedPayment.DatePaid);
                var temp = await DataController.GetReceiptAsync(selectedPayment, new ObservableImmutableList<FeesStructureEntryModel>(fs.SaleItems));
                Document = DocumentHelper.GenerateDocument(temp);
            },
               o => CanGenerate());
        }

        private bool CanGenerate()
        {
            return selectedPayment != null;
        }

        private async Task RefreshRecentPayments()
        {
            recentPayments.Clear();
            RecentPayments = await DataController.GetRecentPaymentsAsync(selectedStudent);
            
        }

        public FeePaymentModel SelectedPayment
        {
            get { return selectedPayment; }
            set
            {
                if (value != selectedPayment)
                {
                    selectedPayment = value;
                    NotifyPropertyChanged("SelectedPayment");
                }
            }
        }


        public StudentSelectModel SelectedStudent
        {
            get { return selectedStudent; }

            set
            {
                if (value != selectedStudent)
                {
                    selectedStudent = value;
                    NotifyPropertyChanged("SelectedStudent");
                }
            }
        }

        public ObservableCollection<FeePaymentModel> RecentPayments
        {
            get { return recentPayments; }

            private set
            {
                if (value != recentPayments)
                {
                    recentPayments = value;
                    NotifyPropertyChanged("RecentPayments");
                }
            }
        }
        public Action<FixedDocument> ShowPrintDialogAction
        {
            get;
            set;
        }
        public ICommand FullPreviewCommand
        {
            get;
            private set;
        }
        public ICommand GenerateCommand
        {
            get;
            private set;
        }

        
        public override void Reset()
        {

        }

        public FixedDocument Document
        {
            get { return this.fd; }

            private set
            {
                if (value != this.fd)
                {
                    this.fd = value;
                    NotifyPropertyChanged("Document");
                }
            }
        }
    }
}
