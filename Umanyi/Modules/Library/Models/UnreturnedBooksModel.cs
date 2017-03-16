using System.Collections.ObjectModel;
using UmanyiSMS.Lib;

namespace UmanyiSMS.Modules.Library.Models
{
    public class UnreturnedBooksModel:ModelBase
    {
        public UnreturnedBooksModel()
        {
            StudentID = 0;
            NameOfStudent = "";
            ClassID = 0;
            NameOfClass = "";
            Entries = new ObservableCollection<BookModel>();
        }
        public override void Reset()
        {
            StudentID = 0;
            NameOfStudent = "";
            ClassID = 0;
            NameOfClass = "";
            Entries.Clear();
        }

        public int StudentID { get; set; }

        public string NameOfStudent { get; set; }

        public int ClassID { get; set; }

        public string NameOfClass { get; set; }

        public ObservableCollection<BookModel> Entries { get; set; }
    }
}
