using System;
using System.Collections.ObjectModel;
using UmanyiSMS.Lib;

namespace UmanyiSMS.Modules.Exams.Models
{
    public class ClassTranscriptsModel2 : ModelBase
    {
        private ObservableCollection<StudentTranscriptModel2> entries;

        public ObservableCollection<StudentTranscriptModel2> Entries
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

        public ClassTranscriptsModel2()
        {
            this.entries = new ObservableCollection<StudentTranscriptModel2>();
        }

        public override void Reset()
        {
            this.entries.Clear();
        }
    }
}
