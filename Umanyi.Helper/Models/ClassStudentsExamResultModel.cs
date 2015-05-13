using System.Collections.ObjectModel;

namespace Helper.Models
{
    public class ClassStudentsExamResultModel:ModelBase
    {
        private ObservableCollection<StudentExamResultModel> entries;
        public ClassStudentsExamResultModel()
        {
            entries = new ObservableCollection<StudentExamResultModel>();
        }
        public ObservableCollection<StudentExamResultModel> Entries
        {
            get { return this.entries; }

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
