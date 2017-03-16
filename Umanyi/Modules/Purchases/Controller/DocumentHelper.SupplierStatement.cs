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
        #region Supp Statement

        private static void AddSSTransaction(TransactionModel item, int itemIndex, int pageNo)
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
                AddText((item.TransactionType == TransactionTypes.Credit) ? "Due" : "Paid",
                    fontsize, false, 0, Colors.Black, 430, yPos, pageNo);
                AddText(item.TransactionAmt.ToString("N2"), fontsize, false, 0, Colors.Black, 665, yPos, pageNo);

            }
        }
        private static void AddSSTransactions(ObservableCollection<TransactionModel> psi, int pageNo)
        {
            int startIndex = pageNo * itemsPerPage;
            int endIndex = startIndex + itemsPerPage - 1;
            if (startIndex >= psi.Count)
                return;
            if (endIndex >= psi.Count)
                endIndex = psi.Count - 1;

            for (int i = startIndex; i <= endIndex; i++)
                AddSSTransaction(psi[i], i, pageNo);

        }

        private static void GenerateSupplierStatement()
        {
            SupplierStatementModel si = myWorkObject as SupplierStatementModel;
            int pageNo;
            for (pageNo = 0; pageNo < noOfPages; pageNo++)
            {
                AddSTPageNo(pageNo, noOfPages);
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
