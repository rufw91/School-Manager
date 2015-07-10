using Helper;
using Helper.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Input;

namespace UmanyiSMS.ViewModels
{
    public class ReprintPaySlipVM:ViewModelBase
    {
        private StaffSelectModel selectedStaff;
        private ObservableCollection<PayslipModel> recentPayments;
        private FixedDocument fd;
        private PayslipModel selectedPayment;
        public ReprintPaySlipVM()
        {
            InitVars();
            CreateCommands();
        }

        protected override void InitVars()
        {
            Title = "REPRINT PAYSLIP";
            RecentPayments = new ObservableCollection<PayslipModel>();
            SelectedStaff = new StaffSelectModel();
            selectedStaff.PropertyChanged += async (o, e) =>
            {
                if ((e.PropertyName == "StaffID") && (selectedStaff.StaffID > 0))
                {
                    selectedStaff.CheckErrors();
                    await RefreshRecentPayments();
                }

            };
        }

        protected override void CreateCommands()
        {
            FullPreviewCommand = new RelayCommand(o =>
            {
                selectedPayment.RefreshTotal();
                var doc = DocumentHelper.GenerateDocument(selectedPayment);
                if (ShowPrintDialogAction != null)
                    ShowPrintDialogAction.Invoke(doc);
            }, o => CanGenerate() && Document != null);
            GenerateCommand = new RelayCommand(o =>
            {
                selectedPayment.RefreshTotal();
                Document = DocumentHelper.GenerateDocument(selectedPayment);
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
            RecentPayments = await DataAccess.GetRecentPayslipsAsync(selectedStaff);
        }

        public PayslipModel SelectedPayment
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


        public StaffSelectModel SelectedStaff
        {
            get { return selectedStaff; }

            set
            {
                if (value != selectedStaff)
                {
                    selectedStaff = value;
                    NotifyPropertyChanged("SelectedStaff");
                }
            }
        }

        public ObservableCollection<PayslipModel> RecentPayments
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
