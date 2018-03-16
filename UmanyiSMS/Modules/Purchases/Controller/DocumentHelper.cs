using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Media;
using UmanyiSMS.Lib;
using UmanyiSMS.Lib.Controllers;
using UmanyiSMS.Modules.Purchases.Models;

namespace UmanyiSMS.Modules.Purchases.Controller
{
    public class DocumentHelper: DocumentHelperBase
    {
        private DocumentHelper(object workObject)
            : base(workObject)
        {
        }

        public static FixedDocument GenerateDocument(object workObject)
        {
            new DocumentHelper(workObject);
            return Document;
        }

        protected override void AddDataToDocument()
        {
            if (MyWorkObject is SupplierPaymentModel)
                GenerateVoucher2();
            else if (MyWorkObject is SupplierStatementModel)
                GenerateSupplierStatement();
            else
            throw new ArgumentException();

        }

        protected override string GetResString()
        {
            if (MyWorkObject is SupplierPaymentModel) 
                return GetResourceString(new Uri("pack://application:,,,/UmanyiSMS;component/Modules/Purchases/Resources/PaymentVoucher2.xaml"));
            if (MyWorkObject is SupplierStatementModel)
                return GetResourceString(new Uri("pack://application:,,,/UmanyiSMS;component/Modules/Purchases/Resources/SupplierStatement.xaml"));

            return "";
        }

        protected override int GetNoOfPages()
        {
            if (MyWorkObject is SupplierPaymentModel)
                return 1;
            if (MyWorkObject is SupplierStatementModel)
                return 1;

            return 0;
        }

        protected override int GetItemsPerPage()
        {
            if (MyWorkObject is SupplierStatementModel)
                return 22;

            return 0;
        }

        #region Payment Voucher2

        private void AddPV2VoucherNo(int voucherNo, int pageNo)
        {
            AddText(voucherNo.ToString(), "Arial", 14, true, 0, Colors.Black, 665, 175, pageNo);
        }
        private void AddPV2Payee(string payee, int pageNo)
        {
            AddText(payee.ToUpper(), "Arial", 14, true, 0, Colors.Black, 115, 220, pageNo);
        }
        private void AddPV2Tel(string tel, int pageNo)
        {
            AddText(tel, "Arial", 14, true, 0, Colors.Black, 115, 250, pageNo);
        }

        private void AddPV2Description(string description, int pageNo)
        {
            AddTextWithWrap(description, "Arial", 415, 375, 14, false, 0, Colors.Black, 90, 370, pageNo);
        }
        private void AddPV2Amount(decimal amount, int pageNo)
        {
            AddText(amount.ToString("N2"), "Arial", 14, true, 0, Colors.Black, 605, 400, pageNo);
        }

        private void AddPV2AmtInWords(int amount, int pageNo)
        {
            //AddTextWithWrap(description, "Arial", 700, 53, 14, false, 0, Colors.Black, 30, 860, pageNo);
        }
        private void GenerateVoucher2()
        {
            SupplierPaymentModel si = MyWorkObject as SupplierPaymentModel;

            int pageNo;
            for (pageNo = 0; pageNo < NoOfPages; pageNo++)
            {
                AddPV2VoucherNo(si.SupplierPaymentID, pageNo);
                AddPV2Payee(si.NameOfSupplier, pageNo);
                AddPV2Description(si.Notes, pageNo);
                AddPV2Amount(si.AmountPaid, pageNo);
                AddPV2AmtInWords((int)si.AmountPaid, pageNo);
            }
        }
        #endregion

        #region Supp Statement

        private void AddSSTransaction(TransactionModel item, int itemIndex, int pageNo)
        {
            double fontsize = 14;
            int pageRelativeIndex = itemIndex - ItemsPerPage * pageNo;
            double yPos = 405 + pageRelativeIndex * 25;
            if ((pageNo == 0) && (itemIndex == 0))
                AddText(item.TransactionAmt.ToString("N2"), 16, true, 0, Colors.Black, 70, 340, pageNo);
            else
            {
                AddText(item.TransactionDateTime.ToString("dd-MM-yyyy"), fontsize, false, 0, Colors.Black, 55, yPos, pageNo);
                AddText(item.TransactionID, fontsize, false, 0, Colors.Black, 220, yPos, pageNo);
                AddText((item.TransactionType == TransactionTypes.Credit) ? "Due" : "Paid",
                    fontsize, false, 0, Colors.Black, 430, yPos, pageNo);
                AddText(item.TransactionAmt.ToString("N2"), fontsize, false, 0, Colors.Black, 665, yPos, pageNo);

            }
        }
        private void AddSSTransactions(ObservableCollection<TransactionModel> psi, int pageNo)
        {
            int startIndex = pageNo * ItemsPerPage;
            int endIndex = startIndex + ItemsPerPage - 1;
            if (startIndex >= psi.Count)
                return;
            if (endIndex >= psi.Count)
                endIndex = psi.Count - 1;

            for (int i = startIndex; i <= endIndex; i++)
                AddSSTransaction(psi[i], i, pageNo);

        }

        private void AddSTPageNo(int pageNo, int totalPages)
        {
            AddText("Page " + (pageNo + 1).ToString() + " of " + totalPages, 12.5, false, 0, Colors.Black, 640, 40, pageNo);
        }
        private void AddSTDate(DateTime dt, int pageNo)
        {
            AddText(dt.ToString("dd MMM yyyy HH:mm:ss"), 12.5, false, 0, Colors.Black, 640, 70, pageNo);
        }
        private void AddSTStudentID(int studentID, int pageNo)
        {
            AddText(studentID.ToString(), 14, true, 0, Colors.Black, 165, 180, pageNo);
        }
        private void AddSTCustomerName(string customerName, int pageNo)
        {
            AddText(customerName.ToUpperInvariant(), 14, true, 0, Colors.Black, 165, 210, pageNo);
        }
        private void AddSTPeriod(string period, int pageNo)
        {
            AddText(period, 16, true, 0, Colors.Black, 165, 240, pageNo);
        }
        private void AddSTSales(decimal sales, int pageNo)
        {
            if (pageNo == 0)
                AddText(sales.ToString("N2"), 16, true, 0, Colors.Black, 280, 340, pageNo);
            else
                AddText("-", 16, true, 0, Colors.Black, 280, 340, pageNo);
        }
        private void AddSTPayments(decimal payments, int pageNo)
        {
            if (pageNo == 0)
                AddText(payments.ToString("N2"), 16, true, 0, Colors.Black, 448, 340, pageNo);
            else
                AddText("-", 16, true, 0, Colors.Black, 448, 340, pageNo);
        }
        private void AddSTTotal(decimal total, int pageNo)
        {
            if (pageNo == 0)
                AddText(total.ToString("N2"), 16, true, 0, Colors.Black, 645, 340, pageNo);
            else
                AddText("-", 16, true, 0, Colors.Black, 645, 340, pageNo);
        }
        
        private void GenerateSupplierStatement()
        {
            SupplierStatementModel si = MyWorkObject as SupplierStatementModel;
            int pageNo;
            for (pageNo = 0; pageNo < NoOfPages; pageNo++)
            {
                AddSTPageNo(pageNo, NoOfPages);
                AddSTDate(si.DateOfStatement, pageNo);
                AddSTCustomerName(si.NameOfSupplier, pageNo);
                AddSTPeriod(si.Period, pageNo);

                AddSTSales(si.TotalSales, pageNo);
                AddSTPayments(si.TotalPayments, pageNo);
                AddSTTotal(si.TotalDue, pageNo);

                AddSSTransactions(si.Transactions, pageNo);
            }
        }
        #endregion
    }
}
