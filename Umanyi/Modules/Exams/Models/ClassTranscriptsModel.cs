using System;
using System.Collections.ObjectModel;

namespace UmanyiSMS.Modules.Exams.Models
{
    public class ClassTranscriptsModel : ModelBase
    {
        private ObservableCollection<StudentTranscriptModel> entries;

        public ObservableCollection<StudentTranscriptModel> Entries
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

        public ClassTranscriptsModel()
        {
            this.entries = new ObservableCollection<StudentTranscriptModel>();
        }

        public override void Reset()
        {
            this.entries.Clear();
        }
    }
}
