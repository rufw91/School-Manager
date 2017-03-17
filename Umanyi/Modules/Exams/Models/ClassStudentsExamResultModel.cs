using System;
using System.Collections.ObjectModel;
using UmanyiSMS.Lib;

namespace UmanyiSMS.Modules.Exams.Models
{
    public class ClassStudentsExamResultModel : ModelBase
    {
        private ObservableCollection<StudentExamResultModel> entries;

        public ObservableCollection<StudentExamResultModel> Entries
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

        public ClassStudentsExamResultModel()
        {
            this.entries = new ObservableCollection<StudentExamResultModel>();
        }

        public override void Reset()
        {
            this.entries.Clear();
        }
    }
}
