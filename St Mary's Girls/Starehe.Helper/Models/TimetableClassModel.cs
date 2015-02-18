using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helper.Models
{
    public class TimetableClassModel: ClassModel
    {
        ObservableCollection<TimetableClassEntryModel> entries;
        public TimetableClassModel()
        {
            Entries = new ObservableCollection<TimetableClassEntryModel>();
        }
        public TimetableClassModel(ClassModel newClass)
            : base(newClass.ClassID, newClass.NameOfClass)
        {
            Entries = new ObservableCollection<TimetableClassEntryModel>();
        }
        
        public ObservableCollection<TimetableClassEntryModel> Entries
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
            base.Reset();
            entries.Clear();
        }

    }
}
