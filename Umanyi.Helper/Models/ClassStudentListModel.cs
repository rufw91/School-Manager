using System;
using System.Collections.ObjectModel;

namespace Helper.Models
{
    public class ClassStudentListModel : ClassModel
    {
        public ObservableCollection<StudentBaseModel> Entries
        {
            get;
            set;
        }

        public DateTime Date
        {
            get;
            set;
        }

        public ClassStudentListModel()
        {
            this.Entries = new ObservableCollection<StudentBaseModel>();
            this.Date = DateTime.Now;
        }
    }
}
