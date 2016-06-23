using Helper;
using Helper.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Input;

namespace UmanyiSMS.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "Accounts")]
    public class ReprintPaymentVoucherVM:ViewModelBase
    {
    
        private SupplierBaseModel selectedSupplier;
        private ObservableCollection<SupplierPaymentModel> recentPayments;
        private FixedDocument fd;
        private SupplierPaymentModel selectedPayment;
        public ReprintPaymentVoucherVM()
        {
            InitVars();
            CreateCommands();
        }

        protected override void InitVars()
        {
            Title = "REPRINT PAYMENT VOUCHER";
            RecentPayments = new ObservableCollection<SupplierPaymentModel>();
            SelectedSupplier = new SupplierBaseModel();
            selectedSupplier.PropertyChanged += async (o, e) =>
            {
                if ((e.PropertyName == "SupplierID") && (selectedSupplier.SupplierID > 0))
                {
                    selectedSupplier.CheckErrors();
                    await RefreshRecentPayments();
                }

            };
        }

        protected override void CreateCommands()
        {
            FullPreviewCommand = new RelayCommand(o =>
            {
                var doc = DocumentHelper.GenerateDocument(selectedPayment);
                if (ShowPrintDialogAction != null)
                    ShowPrintDialogAction.Invoke(doc);
            }, o => CanGenerate() && Document != null);
            GenerateCommand = new RelayCommand(o =>
            {
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
            RecentPayments = await DataAccess.GetRecentSupplierPaymentsAsync(selectedSupplier);
        }

        public SupplierPaymentModel SelectedPayment
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


        public SupplierBaseModel SelectedSupplier
        {
            get { return selectedSupplier; }

            set
            {
                if (value != selectedSupplier)
                {
                    selectedSupplier = value;
                    NotifyPropertyChanged("SelectedSupplier");
                }
            }
        }

        public ObservableCollection<SupplierPaymentModel> RecentPayments
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
