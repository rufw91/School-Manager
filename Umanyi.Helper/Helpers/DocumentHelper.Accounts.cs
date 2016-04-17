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
        #region General Ledger
        private static int CalculatePagesAccountsGenLedger(ObservableCollection<AccountModel> counts)
        {
            return 1;
        }
        
        private static void AddAGLDate(DateTime dateTime, int pageNo)
        {
            AddText(dateTime.ToString("dd-MMM-yyyy"), "Arial", 14, true, 0, Colors.Black, 650, 65, pageNo);
        }
       
        private static void AddAGLTotal(decimal meanScore, int pageNo)
        {
            AddText(meanScore.ToString("N2"), "Arial", 14, true, 0, Colors.Black, 670, 1050, pageNo);
        }
        private static void AddAGLAccount(string meanGrade, int pageNo)
        {
            AddText(meanGrade, "Arial", 14, true, 0, Colors.Black, 330, 115, pageNo);
        }
        private static void AddAGLEntry(TransactionModel item, int itemIndex, int pageNo)
        {
            double fontsize = 14;
            int pageRelativeIndex = itemIndex - itemsPerPage * pageNo;
            double yPos = 190 + pageRelativeIndex * 25;

            AddText(item.TransactionDateTime.ToString("dd-MM-yyyy"), "Arial", 14, false, 0, Colors.Black, 30, yPos, pageNo);
            AddText(item.TransactionID, "Arial", fontsize, false, 0, Colors.Black, 125, yPos, pageNo);
            AddText(item.TransactionAmt.ToString("N2"), "Arial", fontsize, false, 0, Colors.Black, item.TransactionType == TransactionTypes.Debit ? 485 : 585, yPos, pageNo);
            
        }
        private static void AddAGLEntries(ObservableCollection<TransactionModel> psi, int pageNo)
        {

            int startIndex = pageNo * itemsPerPage;
            int endIndex = startIndex + itemsPerPage - 1;
            if (startIndex >= psi.Count)
                return;
            if (endIndex >= psi.Count)
                endIndex = psi.Count - 1;

            for (int i = startIndex; i <= endIndex; i++)
                AddAGLEntry(psi[i], i, pageNo);
        }

        private static void GenerateAccountsGeneralLedger()
        {
            GeneralLedgerModel si = myWorkObject as GeneralLedgerModel;

            int pageNo;
            for (pageNo = 0; pageNo < noOfPages; pageNo++)
            {
                AddAGLDate(si.Date, pageNo);
                AddAGLAccount(si.AccountName, pageNo);
                AddAGLTotal(si.Total, pageNo);
                AddAGLEntries(si.Entries, pageNo);
            }
        }
        #endregion
    }
}
