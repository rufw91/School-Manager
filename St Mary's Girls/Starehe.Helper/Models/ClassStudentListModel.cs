using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helper.Models
{
    public class ClassStudentListModel: ClassModel
    {
        public ClassStudentListModel()
        {
            Entries = new ObservableCollection<StudentBaseModel>();
            Date = DateTime.Now;
        }
        public ObservableCollection<StudentBaseModel> Entries { get; set; }

        public DateTime Date { get; set; }
    }
}
