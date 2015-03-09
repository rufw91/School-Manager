using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helper.Models
{
    public class ClassTranscriptsModel:ModelBase
    {
        private ObservableCollection<StudentTranscriptModel> entries;
        public ClassTranscriptsModel()
        {
            entries = new ObservableCollection<StudentTranscriptModel>();
        }

        public ObservableCollection<StudentTranscriptModel> Entries
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
