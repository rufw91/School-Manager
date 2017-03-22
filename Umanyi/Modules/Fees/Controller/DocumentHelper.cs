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
using UmanyiSMS.Modules.Fees.Models;
using UmanyiSMS.Modules.Purchases.Models;

namespace UmanyiSMS.Modules.Fees.Controller
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
            if (MyWorkObject is ClassBalancesListModel)
                GenerateBalanceList();
            else if (MyWorkObject is FeesStatementModel)
                GenerateStatement();
            else if (MyWorkObject is FeePaymentReceipt2Model)
                GenerateReceipt2();
            else if (MyWorkObject is FeePaymentReceiptModel)
                GenerateReceipt();
            else if (MyWorkObject is FullFeesStructureModel)
                GenerateFeesStructure();
            else
            throw new ArgumentException();

        }

        protected override string GetResString()
        {
            if (MyWorkObject is ClassBalancesListModel)
                return GetResourceString(new Uri("pack://application:,,,/UmanyiSMS;component/Modules/Fees/Resources/Balances.xaml"));
            if (MyWorkObject is FeesStatementModel)
                return GetResourceString(new Uri("pack://application:,,,/UmanyiSMS;component/Modules/Fees/Resources/Statement.xaml"));
            if (MyWorkObject is FeePaymentReceipt2Model)
                return GetResourceString(new Uri("pack://application:,,,/UmanyiSMS;component/Modules/Fees/Resources/Receipt2.xaml"));
            if (MyWorkObject is FeePaymentReceiptModel)
                return GetResourceString(new Uri("pack://application:,,,/UmanyiSMS;component/Modules/Fees/Resources/Receipt.xaml"));
            if (MyWorkObject is FullFeesStructureModel)
                return GetResourceString(new Uri("pack://application:,,,/UmanyiSMS;component/Modules/Fees/Resources/FullFeesStructure.xaml"));

            return "";
        }

        protected override int GetNoOfPages()
        {
            if (MyWorkObject is ClassBalancesListModel)
                return 1;
            if (MyWorkObject is FeesStatementModel)
                return 1;
            if (MyWorkObject is FeePaymentReceipt2Model)
                return 1;
            if (MyWorkObject is FeePaymentReceiptModel)
                return 1;
            if (MyWorkObject is FullFeesStructureModel)
                return 1;

            return 0;
        }

        protected override int GetItemsPerPage()
        {
            if (MyWorkObject is ClassBalancesListModel)
                return 34;
            if (MyWorkObject is FeesStatementModel)
                return 22;
            return 0;
        }

        #region FeesStructure
        private void AddFSDate(DateTime dt, int pageNo)
        {
            AddText(dt.ToString("dd MMM yyyy"), 14, false, 0, Colors.Black, 660, 95, pageNo);
        }
        private void AddFSTerm(int term, int pageNo)
        {
            AddText("TERM " + term, 16, true, 0, Colors.Black, 365, 165, pageNo);
        }
        private void AddFSClassName(string nameOfClass, int pageNo)
        {
            AddText(nameOfClass, 16, true, 0, Colors.Black, 365, 205, pageNo);
        }

        private void AddFSTotal(decimal total, int pageNo)
        {
            AddText(total.ToString("N2"), 16, true, 0, Colors.Black, 630, 875, pageNo);
        }

        private void AddFSEntry(FeesStructureEntryModel item, int itemIndex, int pageNo)
        {
            int pageRelativeIndex = itemIndex;
            double yPos = 290 + pageRelativeIndex * 40;
            AddText((itemIndex + 1).ToString(), 16, true, 0, Colors.Black, 40, yPos, pageNo);
            AddText(item.Name, 16, true, 0, Colors.Black, 130, yPos, pageNo);
            AddText(item.Amount.ToString("N2"), 16, true, 0, Colors.Black, 630, yPos, pageNo);
        }
        private void AddFSEntries(ObservableCollection<FeesStructureEntryModel> psi, int pageNo)
        {
            int startIndex = 0;
            int endIndex = startIndex + ItemsPerPage - 1;
            if (startIndex >= psi.Count)
                return;
            if (endIndex >= psi.Count)
                endIndex = psi.Count - 1;

            for (int i = startIndex; i <= endIndex; i++)
                AddFSEntry(psi[i], i, pageNo);

        }

        private void GenerateFeesStructure()
        {
            FullFeesStructureModel si = MyWorkObject as FullFeesStructureModel;
            int pageNo;
            for (pageNo = 0; pageNo < NoOfPages; pageNo++)
            {
                AddFSDate(DateTime.Now, pageNo);
                AddFSClassName(si[pageNo].NameOfCombinedClass, pageNo);
                AddFSEntries(si[pageNo].Entries, pageNo);
                decimal tot = 0;
                foreach (var t in si[pageNo].Entries)
                    tot += t.Amount;
                AddFSTotal(tot, pageNo);
            }
        }
        #endregion

        #region Receipt

        double page2Offset = 561.28;
        private void AddRCDate(DateTime dt, int pageNo)
        {
            AddText(dt.ToString("dd MMM yyyy hh:mm:ss"), 12.5, false, 0, Colors.Black, 300, 220, pageNo);
            AddText(dt.ToString("dd MMM yyyy hh:mm:ss"), 12.5, false, 0, Colors.Black, 300 + page2Offset, 220, pageNo);
        }
        private void AddRCStudentID(int studentID, int pageNo)
        {
            AddText(studentID.ToString(), 14, true, 0, Colors.Black, 80, 231, pageNo);
            AddText(studentID.ToString(), 14, true, 0, Colors.Black, 80 + page2Offset, 231, pageNo);
        }
        private void AddRCStudentName(string studentName, int pageNo)
        {
            AddText(studentName.ToUpperInvariant(), 14, true, 0, Colors.Black, 80, 249, pageNo);
            AddText(studentName.ToUpperInvariant(), 14, true, 0, Colors.Black, 80 + page2Offset, 249, pageNo);
        }
        private void AddRCClass(string nameOfclass, int pageNo)
        {
            AddText(nameOfclass, 14, true, 0, Colors.Black, 80, 267, pageNo);
            AddText(nameOfclass, 14, true, 0, Colors.Black, 80 + page2Offset, 267, pageNo);
        }
        private void AddRCTerm(string term, int pageNo)
        {
            AddText(term, 14, true, 0, Colors.Black, 80, 285, pageNo);
            AddText(term, 14, true, 0, Colors.Black, 80 + page2Offset, 285, pageNo);
        }

        private void AddRCPaymentMethod(string method, int pageNo)
        {
            AddText(method, 14, true, 0, Colors.Black, 121, 301, pageNo);
            AddText(method, 14, true, 0, Colors.Black, 121 + page2Offset, 301, pageNo);
        }

        private void AddRCReceiptNo(string receiptNo, int pageNo)
        {
            AddText(receiptNo, 14, true, 0, Colors.Black, 452, 24, pageNo);
            AddText(receiptNo, 14, true, 0, Colors.Black, 452 + page2Offset, 24, pageNo);
        }

        private void AddRCFeesItem(FeesStructureEntryModel item, int itemIndex, int pageNo, bool isAggregate)
        {
            double fontsize = 14;
            int pageRelativeIndex = itemIndex - ItemsPerPage * pageNo;
            double yPos = 337 + pageRelativeIndex * 20;

            if (isAggregate)
            {
                AddText(item.Name, 16, true, 0, Colors.Black, 30, yPos, pageNo);
                AddText(item.Amount.ToString("N2"), 16, true, 0, Colors.Black, 250, yPos, pageNo);
                AddText(item.Name, 16, true, 0, Colors.Black, 30 + page2Offset, yPos, pageNo);
                AddText(item.Amount.ToString("N2"), 16, true, 0, Colors.Black, 250 + page2Offset, yPos, pageNo);
            }
            else
            {
                AddText(item.Name, fontsize, false, 0, Colors.Black, 30, yPos, pageNo);
                AddText(item.Amount.ToString("N2"), fontsize, false, 0, Colors.Black, 250, yPos, pageNo);
                AddText(item.Name, fontsize, false, 0, Colors.Black, 30 + page2Offset, yPos, pageNo);
                AddText(item.Amount.ToString("N2"), fontsize, false, 0, Colors.Black, 250 + page2Offset, yPos, pageNo);
            }
        }
        private void AddRCFeesItems(IList<FeesStructureEntryModel> psi, int pageNo)
        {
            int startIndex = pageNo * ItemsPerPage;
            int endIndex = startIndex + ItemsPerPage - 1;
            if (startIndex >= psi.Count)
                return;
            if (endIndex >= psi.Count)
                endIndex = psi.Count - 1;

            for (int i = startIndex; i <= endIndex; i++)
                AddRCFeesItem(psi[i], i, pageNo, (i >= endIndex - 3));

        }

        private void GenerateReceipt()
        {
            FeePaymentReceiptModel si = MyWorkObject as FeePaymentReceiptModel;

            int pageNo;
            for (pageNo = 0; pageNo < NoOfPages; pageNo++)
            {
                AddRCDate(DateTime.Now, pageNo);
                AddRCStudentID(si.StudentID, pageNo);
                AddRCStudentName(si.NameOfStudent, pageNo);
                AddRCClass(si.NameOfClass, pageNo);
                AddRCPaymentMethod(si.PaymentMethod, pageNo);
                AddRCReceiptNo(si.FeePaymentID.ToString(), pageNo);
                AddRCFeesItems(si.Entries, pageNo);
            }
        }


        #endregion

        #region Receipt2

        private void AddRC2Date(DateTime dt, int pageNo)
        {
            AddText(dt.ToString("dd MMM yyyy hh:mm:ss"), 12.5, false, 0, Colors.Black, 300, 220, pageNo);
        }
        private void AddRC2StudentID(int studentID, int pageNo)
        {
            AddText(studentID.ToString(), 14, true, 0, Colors.Black, 80, 231, pageNo);
        }
        private void AddRC2StudentName(string studentName, int pageNo)
        {
            AddText(studentName.ToUpperInvariant(), 14, true, 0, Colors.Black, 80, 249, pageNo);
        }
        private void AddRC2Class(string nameOfclass, int pageNo)
        {
            AddText(nameOfclass, 14, true, 0, Colors.Black, 80, 267, pageNo);
        }
        private void AddRC2Term(string term, int pageNo)
        {
            AddText(term, 14, true, 0, Colors.Black, 80, 285, pageNo);
        }

        private void AddRC2FeesItem(FeesStructureEntryModel item, int itemIndex, int pageNo, bool isAggregate)
        {
            double fontsize = 14;
            int pageRelativeIndex = itemIndex - ItemsPerPage * pageNo;
            double yPos = 337 + pageRelativeIndex * 20;

            if (isAggregate)
            {
                AddText(item.Name, 16, true, 0, Colors.Black, 30, yPos, pageNo);
                AddText(item.Amount.ToString("N2"), 16, true, 0, Colors.Black, 250, yPos, pageNo);
            }
            else
            {
                AddText(item.Name, fontsize, false, 0, Colors.Black, 30, yPos, pageNo);
                AddText(item.Amount.ToString("N2"), fontsize, false, 0, Colors.Black, 250, yPos, pageNo);
            }
        }
        private void AddRC2FeesItems(IList<FeesStructureEntryModel> psi, int pageNo)
        {
            int startIndex = pageNo * ItemsPerPage;
            int endIndex = startIndex + ItemsPerPage - 1;
            if (startIndex >= psi.Count)
                return;
            if (endIndex >= psi.Count)
                endIndex = psi.Count - 1;

            for (int i = startIndex; i <= endIndex; i++)
                AddRC2FeesItem(psi[i], i, pageNo, (i >= endIndex - 2));

        }

        private void GenerateReceipt2()
        {
            FeePaymentReceipt2Model si = MyWorkObject as FeePaymentReceipt2Model;

            int pageNo;
            for (pageNo = 0; pageNo < NoOfPages; pageNo++)
            {
                AddRC2Date(DateTime.Now, pageNo);
                AddRC2StudentID(si.StudentID, pageNo);
                AddRC2StudentName(si.NameOfStudent, pageNo);
                AddRC2Class(si.NameOfClass, pageNo);
                AddRC2FeesItems(si.Entries, pageNo);
            }
        }

        #endregion

        #region Statement
        private void AddSTPageNo(int pageNo, int totalPages)
        {
            AddText("Page " + (pageNo + 1).ToString() + " of " + totalPages, 12.5, false, 0, Colors.Black, 640, 40, pageNo);
        }
        private void AddSTDate(DateTime dt, int pageNo)
        {
            AddText(dt.ToString("dd MMM yyyy hh:mm:ss"), 12.5, false, 0, Colors.Black, 640, 70, pageNo);
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

        private void AddSTTransaction(TransactionModel item, int itemIndex, int pageNo)
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
                AddText((item.TransactionType == TransactionTypes.Credit) ? "Fees Paid-" + item.TransactionID : "Fees Due-" + item.TransactionID,
                    fontsize, false, 0, Colors.Black, 430, yPos, pageNo);
                AddText(item.TransactionAmt.ToString("N2"), fontsize, false, 0, Colors.Black, 665, yPos, pageNo);

            }
        }
        private void AddSTTransactions(ObservableCollection<TransactionModel> psi, int pageNo)
        {
            int startIndex = pageNo * ItemsPerPage;
            int endIndex = startIndex + ItemsPerPage - 1;
            if (startIndex >= psi.Count)
                return;
            if (endIndex >= psi.Count)
                endIndex = psi.Count - 1;

            for (int i = startIndex; i <= endIndex; i++)
                AddSTTransaction(psi[i], i, pageNo);

        }

        private void GenerateStatement()
        {
            FeesStatementModel si = MyWorkObject as FeesStatementModel;
            int pageNo;
            for (pageNo = 0; pageNo < NoOfPages; pageNo++)
            {
                AddSTPageNo(pageNo, NoOfPages);
                AddSTDate(si.DateOfStatement, pageNo);
                AddSTStudentID(si.StudentID, pageNo);
                AddSTCustomerName(si.NameOfStudent, pageNo);
                AddSTPeriod(si.Period, pageNo);

                AddSTSales(si.TotalSales, pageNo);
                AddSTPayments(si.TotalPayments, pageNo);
                AddSTTotal(si.TotalDue, pageNo);

                AddSTTransactions(si.Transactions, pageNo);
            }
        }
        #endregion

        #region Balances

        private void AddBLClass(string nameOfClass, int pageNo)
        {
            AddText(nameOfClass, "Arial", 14, true, 0, Colors.Black, 100, 85, pageNo);
        }
        private void AddBLDate(DateTime dateTime, int pageNo)
        {
            AddText(dateTime.ToString("dd MMM yyyy"), "Arial", 14, true, 0, Colors.Black, 600, 85, pageNo);
        }

        private void AddBLTotal(decimal total, int pageNo)
        {
            AddText(total.ToString("N2"), "Arial", 16, true, 0, Colors.Black, 80, 1050, pageNo);
        }

        private void AddBLTotalUnpaid(decimal total, int pageNo)
        {
            AddText(total.ToString("N2"), "Arial", 16, true, 0, Colors.Black, 310, 1050, pageNo);
        }

        private void AddBLStudentBalance(StudentFeesDefaultModel item, int itemIndex, int pageNo)
        {
            double fontsize = 14;
            int pageRelativeIndex = itemIndex - ItemsPerPage * pageNo;
            double yPos = 165 + pageRelativeIndex * 25;

            AddText(item.StudentID.ToString(), "Arial", 14, false, 0, Colors.Black, 45, yPos, pageNo);
            AddText(item.NameOfStudent, "Arial", fontsize, false, 0, Colors.Black, 175, yPos, pageNo);
            AddText(item.Balance.ToString("N2"), "Arial", fontsize, false, 0, Colors.Black, 445, yPos, pageNo);
            AddText(item.GuardianPhoneNo, "Arial", fontsize, false, 0, Colors.Black, 580, yPos, pageNo);
        }
        private void AddBLStudentBalances(ObservableCollection<StudentFeesDefaultModel> psi, int pageNo)
        {

            int startIndex = pageNo * ItemsPerPage;
            int endIndex = startIndex + ItemsPerPage - 1;
            if (startIndex >= psi.Count)
                return;
            if (endIndex >= psi.Count)
                endIndex = psi.Count - 1;

            for (int i = startIndex; i <= endIndex; i++)
                AddBLStudentBalance(psi[i], i, pageNo);
        }

        private void GenerateBalanceList()
        {
            ClassBalancesListModel si = MyWorkObject as ClassBalancesListModel;

            int pageNo;
            for (pageNo = 0; pageNo < NoOfPages; pageNo++)
            {
                AddBLClass(si.NameOfClass, pageNo);
                AddBLDate(si.Date, pageNo);
                if (pageNo == 0)
                {
                    AddBLTotal(si.Total, pageNo);
                    AddBLTotalUnpaid(si.TotalUnpaid, pageNo);
                }
                AddBLStudentBalances(si.Entries, pageNo);
            }
        }
        #endregion
    }
}
