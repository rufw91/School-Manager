using System;
using System.Collections.ObjectModel;
using UmanyiSMS.Lib;

namespace UmanyiSMS.Modules.Exams.Models
{
    public class ExamResultClassDisplayModel : ModelBase
    {
        private ObservableCollection<ExamResultStudentDisplayModel> entries;

        public ObservableCollection<ExamResultStudentDisplayModel> Entries
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

        public ExamResultClassDisplayModel()
        {
            this.entries = new ObservableCollection<ExamResultStudentDisplayModel>();
        }

        public override void Reset()
        {
            this.entries.Clear();
        }
    }
}
