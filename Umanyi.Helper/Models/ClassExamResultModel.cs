using System;
using System.Data;

namespace Helper.Models
{
    public class ClassExamResultModel : ClassModel
    {
        private DataTable entries;

        public DataTable Entries
        {
            get
            {
                return this.entries;
            }
            set
            {
                if (value != this.entries)
                {
                    this.entries = value;
                    base.NotifyPropertyChanged("Entries");
                }
            }
        }

        public ClassExamResultModel()
        {
            this.Entries = new DataTable();
        }

        public override void Reset()
        {
            this.entries.Clear();
        }
    }
}
