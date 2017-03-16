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
        #region Receipt2

        private static void AddRC2Date(DateTime dt, int pageNo)
        {
            AddText(dt.ToString("dd MMM yyyy hh:mm:ss"), 12.5, false, 0, Colors.Black, 300, 220, pageNo);
        }
        private static void AddRC2StudentID(int studentID, int pageNo)
        {
            AddText(studentID.ToString(), 14, true, 0, Colors.Black, 80, 231, pageNo);
        }
        private static void AddRC2StudentName(string studentName, int pageNo)
        {
            AddText(studentName.ToUpperInvariant(), 14, true, 0, Colors.Black, 80, 249, pageNo);
        }
        private static void AddRC2Class(string nameOfclass, int pageNo)
        {
            AddText(nameOfclass, 14, true, 0, Colors.Black, 80, 267, pageNo);
        }
        private static void AddRC2Term(string term, int pageNo)
        {
            AddText(term, 14, true, 0, Colors.Black, 80, 285, pageNo);
        }

        private static void AddRC2FeesItem(FeesStructureEntryModel item, int itemIndex, int pageNo, bool isAggregate)
        {
            double fontsize = 14;
            int pageRelativeIndex = itemIndex - itemsPerPage * pageNo;
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
        private static void AddRC2FeesItems(IList<FeesStructureEntryModel> psi, int pageNo)
        {
            int startIndex = pageNo * itemsPerPage;
            int endIndex = startIndex + itemsPerPage - 1;
            if (startIndex >= psi.Count)
                return;
            if (endIndex >= psi.Count)
                endIndex = psi.Count - 1;

            for (int i = startIndex; i <= endIndex; i++)
                AddRC2FeesItem(psi[i], i, pageNo, (i >= endIndex - 2));

        }

        private static void GenerateReceipt2()
        {
            FeePaymentReceipt2Model si = myWorkObject as FeePaymentReceipt2Model;

            int pageNo;
            for (pageNo = 0; pageNo < noOfPages; pageNo++)
            {
                AddRC2Date(DateTime.Now, pageNo);
                AddRC2StudentID(si.StudentID, pageNo);
                AddRC2StudentName(si.NameOfStudent, pageNo);
                AddRC2Term(GetTerm(), pageNo);
                AddRC2Class(si.NameOfClass, pageNo);
                AddRC2FeesItems(si.Entries, pageNo);
            }
        }

        #endregion
    }
}
