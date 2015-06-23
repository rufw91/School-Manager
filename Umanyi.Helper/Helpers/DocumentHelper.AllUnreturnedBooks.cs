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
        #region Unreturned Books List
        
        private static void AddUB2Date(DateTime dateTime, int pageNo)
        {
            AddText(dateTime.ToString("dd-MMM-yyyy"), "Arial", 14, true, 0, Colors.Black, 630, 85, pageNo);
        }

        private static void AddUB2Entry(UnreturnedBookModel item, int itemIndex, int pageNo)
        {
            double fontsize = 14;
            int pageRelativeIndex = itemIndex - itemsPerPage * pageNo;
            double yPos = 245 + pageRelativeIndex * 25;

            AddText(item.ISBN, "Arial", 14, false, 0, Colors.Black, 40, yPos, pageNo);
            AddText(item.Title, "Arial", fontsize, false, 0, Colors.Black, 145, yPos, pageNo);
            AddText(item.Author, "Arial", fontsize, false, 0, Colors.Black, 305, yPos, pageNo);
            AddText(item.Publisher, "Arial", fontsize, false, 0, Colors.Black, 455, yPos, pageNo);
            AddText(item.UnreturnedCopies.ToString("N0"), "Arial", fontsize, false, 0, Colors.Black, 605, yPos, pageNo);
        }
        private static void AddUB2Entries(ObservableCollection<UnreturnedBookModel> psi, int pageNo)
        {

            int startIndex = pageNo * itemsPerPage;
            int endIndex = startIndex + itemsPerPage - 1;
            if (startIndex >= psi.Count)
                return;
            if (endIndex >= psi.Count)
                endIndex = psi.Count - 1;

            for (int i = startIndex; i <= endIndex; i++)
                AddUB2Entry(psi[i], i, pageNo);
        }

        private static void GenerateUnreturnedBooks2()
        {
            AllUnreturnedBooksModel si = myWorkObject as AllUnreturnedBooksModel;

            int pageNo;
            for (pageNo = 0; pageNo < noOfPages; pageNo++)
            {
                AddUB2Date(DateTime.Now, pageNo);
                AddUB2Entries(si, pageNo);
            }
        }
        #endregion
    }
}
