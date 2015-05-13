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
        private static void AddUBStudentID(int studentID, int pageNo)
        {
            AddText(studentID.ToString(), "Arial", 14, true, 0, Colors.Black, 100, 170, pageNo);
        }
        private static void AddUBNameOfStudent(string nameOfStudent, int pageNo)
        {
            AddText(nameOfStudent, "Arial", 14, true, 0, Colors.Black, 250, 170, pageNo);
        }
        private static void AddUBClass(string nameOfClass, int pageNo)
        {
            AddText(nameOfClass, "Arial", 14, true, 0, Colors.Black, 630, 170, pageNo);
        }
        private static void AddUBDate(DateTime dateTime, int pageNo)
        {
            AddText(dateTime.ToString("dd-MMM-yyyy"), "Arial", 14, true, 0, Colors.Black, 630, 85, pageNo);
        }

        private static void AddUBEntry(BookModel item, int itemIndex, int pageNo)
        {
            double fontsize = 14;
            int pageRelativeIndex = itemIndex - itemsPerPage * pageNo;
            double yPos = 245 + pageRelativeIndex * 25;

            AddText(item.ISBN, "Arial", 14, false, 0, Colors.Black, 40, yPos, pageNo);
            AddText(item.Title, "Arial", fontsize, false, 0, Colors.Black, 145, yPos, pageNo);
            AddText(item.Author, "Arial", fontsize, false, 0, Colors.Black, 305, yPos, pageNo);
            AddText(item.Publisher, "Arial", fontsize, false, 0, Colors.Black, 455, yPos, pageNo);
            AddText(item.Price.ToString("N2"), "Arial", fontsize, false, 0, Colors.Black, 605, yPos, pageNo);
        }
        private static void AddUBEntries(ObservableCollection<BookModel> psi, int pageNo)
        {

            int startIndex = pageNo * itemsPerPage;
            int endIndex = startIndex + itemsPerPage - 1;
            if (startIndex >= psi.Count)
                return;
            if (endIndex >= psi.Count)
                endIndex = psi.Count - 1;

            for (int i = startIndex; i <= endIndex; i++)
                AddUBEntry(psi[i], i, pageNo);
        }

        private static void GenerateUnreturnedBooks()
        {
            UnreturnedBooksModel si = myWorkObject as UnreturnedBooksModel;

            int pageNo;
            for (pageNo = 0; pageNo < noOfPages; pageNo++)
            {
                AddUBStudentID(si.StudentID, pageNo);
                AddUBNameOfStudent(si.NameOfStudent, pageNo);
                AddUBClass(si.NameOfClass, pageNo);
                AddUBDate(DateTime.Now, pageNo);
                AddUBEntries(si.Entries, pageNo);
            }
        }
        #endregion
    }
}
