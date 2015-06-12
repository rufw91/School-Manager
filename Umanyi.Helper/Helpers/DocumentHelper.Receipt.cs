using Helper.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Helper
{
    public static partial class DocumentHelper
    {
        #region Receipt

        static double page2Offset = 561.28;
        private static void AddRCDate(DateTime dt, int pageNo)
        {
            AddText(dt.ToString("dd MMM yyyy hh:mm:ss"), 12.5, false, 0, Colors.Black, 300, 220, pageNo);
            AddText(dt.ToString("dd MMM yyyy hh:mm:ss"), 12.5, false, 0, Colors.Black, 300 + page2Offset, 220, pageNo);
        }
        private static void AddRCStudentID(int studentID, int pageNo)
        {
            AddText(studentID.ToString(), 14, true, 0, Colors.Black, 80, 231, pageNo);
            AddText(studentID.ToString(), 14, true, 0, Colors.Black, 80 + page2Offset, 231, pageNo);
        }
        private static void AddRCStudentName(string studentName, int pageNo)
        {
            AddText(studentName.ToUpperInvariant(), 14, true, 0, Colors.Black, 80, 249, pageNo);
            AddText(studentName.ToUpperInvariant(), 14, true, 0, Colors.Black, 80 + page2Offset, 249, pageNo);
        }
        private static void AddRCClass(string nameOfclass, int pageNo)
        {
            AddText(nameOfclass, 14, true, 0, Colors.Black, 80, 267, pageNo);
            AddText(nameOfclass, 14, true, 0, Colors.Black, 80 + page2Offset, 267, pageNo);
        }
        private static void AddRCTerm(string term, int pageNo)
        {
            AddText(term, 14, true, 0, Colors.Black, 80, 285, pageNo);
            AddText(term, 14, true, 0, Colors.Black, 80 + page2Offset, 285, pageNo);
        }

        private static void AddRCFeesItem(FeesStructureEntryModel item, int itemIndex, int pageNo, bool isAggregate)
        {
            double fontsize = 14;
            int pageRelativeIndex = itemIndex - itemsPerPage * pageNo;
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
        private static void AddRCFeesItems(IList<FeesStructureEntryModel> psi, int pageNo)
        {
            int startIndex = pageNo * itemsPerPage;
            int endIndex = startIndex + itemsPerPage - 1;
            if (startIndex >= psi.Count)
                return;
            if (endIndex >= psi.Count)
                endIndex = psi.Count - 1;

            for (int i = startIndex; i <= endIndex; i++)
                AddRCFeesItem(psi[i], i, pageNo, (i >= endIndex - 3));

        }

        private static void GenerateReceipt()
        {
            FeePaymentReceiptModel si = myWorkObject as FeePaymentReceiptModel;

            int pageNo;
            for (pageNo = 0; pageNo < noOfPages; pageNo++)
            {
                AddRCDate(DateTime.Now, pageNo);
                AddRCStudentID(si.StudentID, pageNo);
                AddRCStudentName(si.NameOfStudent, pageNo);
                AddRCTerm(GetTerm(), pageNo);
                AddRCClass(si.NameOfClass, pageNo);
                AddRCFeesItems(si.Entries, pageNo);
            }
        }

        
        #endregion
    }
}
