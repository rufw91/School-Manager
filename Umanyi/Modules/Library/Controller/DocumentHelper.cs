using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Media;
using UmanyiSMS.Lib.Controllers;
using UmanyiSMS.Modules.Library.Models;

namespace UmanyiSMS.Modules.Library.Controller
{
    public class DocumentHelper:DocumentHelperBase
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
            if (MyWorkObject is AllUnreturnedBooksModel)
                GenerateUnreturnedBooks2();
            if (MyWorkObject is UnreturnedBooksModel)
                GenerateUnreturnedBooks();

            throw new ArgumentException();

        }

        protected override string GetResString()
        {
            if (MyWorkObject is AllUnreturnedBooksModel)
                return GetResourceString(null);
            if (MyWorkObject is UnreturnedBooksModel)
                return GetResourceString(null);

            return "";
        }

        protected override int GetNoOfPages()
        {
            if (MyWorkObject is AllUnreturnedBooksModel)
                return 1;
            if (MyWorkObject is UnreturnedBooksModel)
                return 1;

            return 0;
        }

        protected override int GetItemsPerPage()
        {            
            if (MyWorkObject is AllUnreturnedBooksModel)
                return 34;
            if (MyWorkObject is UnreturnedBooksModel)
                return 34;

            return 0;
        }

        #region Unreturned Books List

        private void AddUB2Date(DateTime dateTime, int pageNo)
        {
            AddText(dateTime.ToString("dd-MMM-yyyy"), "Arial", 14, true, 0, Colors.Black, 630, 85, pageNo);
        }

        private void AddUB2Entry(UnreturnedBookModel item, int itemIndex, int pageNo)
        {
            double fontsize = 12;
            int pageRelativeIndex = itemIndex - ItemsPerPage * pageNo;
            double yPos = 245 + pageRelativeIndex * 25;

            AddText(item.ISBN, "Arial Narrow", fontsize, false, 0, Colors.Black, 18, yPos, pageNo);
            AddText(item.Title, "Arial Narrow", fontsize, false, 0, Colors.Black, 200, yPos, pageNo);
            AddText(item.Publisher, "Arial Narrow", fontsize, false, 0, Colors.Black, 540, yPos, pageNo);
            AddText(item.UnreturnedCopies.ToString("N0"), "Arial Narrow", fontsize, false, 0, Colors.Black, 700, yPos, pageNo);
        }
        private void AddUB2Entries(ObservableCollection<UnreturnedBookModel> psi, int pageNo)
        {

            int startIndex = pageNo * ItemsPerPage;
            int endIndex = startIndex + ItemsPerPage - 1;
            if (startIndex >= psi.Count)
                return;
            if (endIndex >= psi.Count)
                endIndex = psi.Count - 1;

            for (int i = startIndex; i <= endIndex; i++)
                AddUB2Entry(psi[i], i, pageNo);
        }

        private void GenerateUnreturnedBooks2()
        {
            AllUnreturnedBooksModel si = MyWorkObject as AllUnreturnedBooksModel;

            int pageNo;
            for (pageNo = 0; pageNo < NoOfPages; pageNo++)
            {
                AddUB2Date(DateTime.Now, pageNo);
                AddUB2Entries(si, pageNo);
            }
        }
        #endregion

        #region Unreturned Books List
        private void AddUBStudentID(int studentID, int pageNo)
        {
            AddText(studentID.ToString(), "Arial", 14, true, 0, Colors.Black, 100, 170, pageNo);
        }
        private void AddUBNameOfStudent(string nameOfStudent, int pageNo)
        {
            AddText(nameOfStudent, "Arial", 14, true, 0, Colors.Black, 250, 170, pageNo);
        }
        private void AddUBClass(string nameOfClass, int pageNo)
        {
            AddText(nameOfClass, "Arial", 14, true, 0, Colors.Black, 630, 170, pageNo);
        }
        private void AddUBDate(DateTime dateTime, int pageNo)
        {
            AddText(dateTime.ToString("dd-MMM-yyyy"), "Arial", 14, true, 0, Colors.Black, 630, 85, pageNo);
        }

        private void AddUBEntry(BookModel item, int itemIndex, int pageNo)
        {
            double fontsize = 12;
            int pageRelativeIndex = itemIndex - ItemsPerPage * pageNo;
            double yPos = 245 + pageRelativeIndex * 25;

            AddText(item.ISBN, "Arial Narrow", fontsize, false, 0, Colors.Black, 20, yPos, pageNo);
            AddText(item.Title, "Arial Narrow", fontsize, false, 0, Colors.Black, 200, yPos, pageNo);
            AddText(item.Publisher, "Arial Narrow", fontsize, false, 0, Colors.Black, 520, yPos, pageNo);
        }
        private void AddUBEntries(ObservableCollection<BookModel> psi, int pageNo)
        {

            int startIndex = pageNo * ItemsPerPage;
            int endIndex = startIndex + ItemsPerPage - 1;
            if (startIndex >= psi.Count)
                return;
            if (endIndex >= psi.Count)
                endIndex = psi.Count - 1;

            for (int i = startIndex; i <= endIndex; i++)
                AddUBEntry(psi[i], i, pageNo);
        }

        private void GenerateUnreturnedBooks()
        {
            UnreturnedBooksModel si = MyWorkObject as UnreturnedBooksModel;

            int pageNo;
            for (pageNo = 0; pageNo < NoOfPages; pageNo++)
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
