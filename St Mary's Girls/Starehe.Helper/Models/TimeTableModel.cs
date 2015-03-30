using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helper.Models
{
    public class TimeTableModel:ModelBase
    {
        List<KeyValuePair<int, List<KeyValuePair<int, List<SubjectBaseModel>>>>> timeTable;
        public TimeTableModel()
        {
            TimeTable = new List<KeyValuePair<int, List<KeyValuePair<int, List<SubjectBaseModel>>>>>();
        }
        public List<KeyValuePair<int, List<KeyValuePair<int, List<SubjectBaseModel>>>>> TimeTable
        {
            get { return timeTable; }
            set
            {
                if (value != this.timeTable)
                {
                    this.timeTable = value;
                    NotifyPropertyChanged("TimeTable");
                }
            }
        }

        public override void Reset()
        {
            timeTable.Clear();
        }
    }
}
