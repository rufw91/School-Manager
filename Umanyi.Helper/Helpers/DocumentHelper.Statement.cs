using Helper.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Helper
{
    public static partial class DocumentHelper
    {
        #region Statement
        private static void AddSTPageNo(int pageNo, int totalPages)
        {
            AddText("Page " + (pageNo + 1).ToString() + " of " + totalPages, 12.5, false, 0, Colors.Black, 640, 40, pageNo);
        }
        private static void AddSTDate(DateTime dt, int pageNo)
        {
            AddText(dt.ToString("dd MMM yyyy hh:mm:ss"), 12.5, false, 0, Colors.Black, 640, 70, pageNo);
        }
        private static void AddSTStudentID(int studentID, int pageNo)
        {
            AddText(studentID.ToString(), 14, true, 0, Colors.Black, 165, 180, pageNo);
        }
        private static void AddSTCustomerName(string customerName, int pageNo)
        {
            AddText(customerName.ToUpperInvariant(), 14, true, 0, Colors.Black, 165, 210, pageNo);
        }
        private static void AddSTPeriod(string period, int pageNo)
        {
            AddText(period, 16, true, 0, Colors.Black, 165, 240, pageNo);
        }
        private static void AddSTSales(decimal sales, int pageNo)
        {
            if (pageNo == 0)
                AddText(sales.ToString("N2"), 16, true, 0, Colors.Black, 280, 340, pageNo);
            else
                AddText("-", 16, true, 0, Colors.Black, 280, 340, pageNo);
        }
        private static void AddSTPayments(decimal payments, int pageNo)
        {
            if (pageNo == 0)
                AddText(payments.ToString("N2"), 16, true, 0, Colors.Black, 448, 340, pageNo);
            else
                AddText("-", 16, true, 0, Colors.Black, 448, 340, pageNo);
        }
        private static void AddSTTotal(decimal total, int pageNo)
        {
            if (pageNo == 0)
                AddText(total.ToString("N2"), 16, true, 0, Colors.Black, 645, 340, pageNo);
            else
                AddText("-", 16, true, 0, Colors.Black, 645, 340, pageNo);
        }

        private static void AddSTTransaction(TransactionModel item, int itemIndex, int pageNo)
        {
            double fontsize = 14;
            int pageRelativeIndex = itemIndex - itemsPerPage * pageNo;
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
        private static void AddSTTransactions(ObservableCollection<TransactionModel> psi, int pageNo)
        {
            int startIndex = pageNo * itemsPerPage;
            int endIndex = startIndex + itemsPerPage - 1;
            if (startIndex >= psi.Count)
                return;
            if (endIndex >= psi.Count)
                endIndex = psi.Count - 1;

            for (int i = startIndex; i <= endIndex; i++)
                AddSTTransaction(psi[i], i, pageNo);

        }

        private static void GenerateStatement()
        {
            FeesStatementModel si = myWorkObject as FeesStatementModel;
            int pageNo;
            for (pageNo = 0; pageNo < noOfPages; pageNo++)
            {
                AddSTPageNo(pageNo, noOfPages);
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
    }
}
