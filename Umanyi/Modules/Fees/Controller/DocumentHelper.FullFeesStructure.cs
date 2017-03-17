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
            AddText(dt.ToString("dd MMM yyyy"), 14, false, 0, Colors.Black, 660, 95, pageNo);
        }
        private static void AddFSTerm(int term, int pageNo)
        {
            AddText("TERM " + term, 16, true, 0, Colors.Black, 365, 165, pageNo);
        }
        private static void AddFSClassName(string nameOfClass, int pageNo)
        {
            AddText(nameOfClass, 16, true, 0, Colors.Black, 365, 205, pageNo);
        }
        
        private static void AddFSTotal(decimal total, int pageNo)
        {
            AddText(total.ToString("N2"), 16, true, 0, Colors.Black, 630, 875, pageNo);
        }

        private static void AddFSEntry(FeesStructureEntryModel item, int itemIndex, int pageNo)
        {
            int pageRelativeIndex = itemIndex;
            double yPos = 290 + pageRelativeIndex * 40;
            AddText((itemIndex+1).ToString(), 16, true, 0, Colors.Black, 40, yPos, pageNo);
            AddText(item.Name, 16, true, 0, Colors.Black, 130, yPos, pageNo);
            AddText(item.Amount.ToString("N2"), 16, true, 0, Colors.Black, 630, yPos, pageNo);
        }
        private static void AddFSEntries(ObservableCollection<FeesStructureEntryModel> psi, int pageNo)
        {
            int startIndex = 0;
            int endIndex = startIndex + itemsPerPage - 1;
            if (startIndex >= psi.Count)
                return;
            if (endIndex >= psi.Count)
                endIndex = psi.Count - 1;

            for (int i = startIndex; i <= endIndex; i++)
                AddFSEntry(psi[i], i, pageNo);

        }

        private static void GenerateFeesStructure()
        {
            FullFeesStructureModel si = myWorkObject as FullFeesStructureModel;
            int pageNo;
            for (pageNo = 0; pageNo < noOfPages; pageNo++)
            {
                AddFSDate(DateTime.Now, pageNo);
                AddFSClassName(si[pageNo].NameOfCombinedClass, pageNo);
                AddFSTerm(DataController.GetTerm(si[pageNo].StartDate), pageNo);
                AddFSEntries(si[pageNo].Entries, pageNo);
                decimal tot = 0;
                foreach (var t in si[pageNo].Entries)
                    tot += t.Amount;
                AddFSTotal(tot, pageNo);
            }
        }
        #endregion
    }
}
