using System;
using System.Collections.ObjectModel;
using UmanyiSMS.Modules.Institution.Models;

namespace UmanyiSMS.Modules.Students.Models
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
