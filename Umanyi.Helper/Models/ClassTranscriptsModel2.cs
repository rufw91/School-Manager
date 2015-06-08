using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helper.Models
{
    public class ClassTranscriptsModel2 : ModelBase
    {
        private ObservableCollection<StudentTranscriptModel2> entries;
        public ClassTranscriptsModel2()
        {
            entries = new ObservableCollection<StudentTranscriptModel2>();
        }

        public ObservableCollection<StudentTranscriptModel2> Entries
        {
            get { return entries; }
            set
            {
                if (value != entries)
                {
                    entries = value;
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
