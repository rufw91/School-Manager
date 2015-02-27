using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;

namespace Helper.Models
{
    public class ClassExamResultModel:ClassModel
    {
        DataTable entries;
        public ClassExamResultModel()
        {
            Entries = new DataTable();
        }
        public DataTable Entries
        {
            get { return entries; }

            set
            {
                if (value != this.entries)
                {
                    this.entries = value;
                    NotifyPropertyChanged("Entries");
                }
            }
        }

        public override void Reset()
        {
            entries.Clear();
        }
    }
}
