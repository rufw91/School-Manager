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
        #region FeesStructure
        private static void AddFSDate(DateTime dt, int pageNo)
        {
            //AddText(dt.ToString("dd MMM yyyy hh:mm:ss"), 12.5, false, 0, Colors.Black, 640, 70, pageNo);
        }
        private static void AddFSPeriod(string period, int pageNo)
        {
         //   AddText(period, 16, true, 0, Colors.Black, 165, 240, pageNo);
        }
        
        private static void AddFSEntry(FeesStructureEntryModel item, int itemIndex, int pageNo)
        {
            /*double fontsize = 14;
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

            }*/
        }
        private static void AddFSEntries(ObservableCollection<FeesStructureEntryModel> psi, int pageNo)
        {
           /* int startIndex = pageNo * itemsPerPage;
            int endIndex = startIndex + itemsPerPage - 1;
            if (startIndex >= psi.Count)
                return;
            if (endIndex >= psi.Count)
                endIndex = psi.Count - 1;

            for (int i = startIndex; i <= endIndex; i++)
                AddFSEntry(psi[i], i, pageNo);*/

        }

        private static void GenerateFeesStructure()
        {
            FullFeesStructureModel si = myWorkObject as FullFeesStructureModel;
            int pageNo;
            for (pageNo = 0; pageNo < noOfPages; pageNo++)
            {
                //AddFSDate(si.DateOfStatement, pageNo);
                //AddFSPeriod(si.StudentID, pageNo);
                //AddFSEntries(si.Transactions, pageNo);
            }
        }
        #endregion
    }
}
