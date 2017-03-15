using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helper.Models
{
    public class StudentSubjectSelectionModel:StudentBaseModel
    {
        private ObservableCollection<StudentSubjectSelectionEntryModel> entries;
        private int year;

        public StudentSubjectSelectionModel()
        {
            Entries = new ObservableCollection<StudentSubjectSelectionEntryModel>();
            year = DateTime.Now.Year;
        }

        public ObservableCollection<StudentSubjectSelectionEntryModel> Entries
        {
            get { return entries; }
            set
            {
                if (entries != value)
                {
                    entries = value; NotifyPropertyChanged("Entries");
                }
            }
        }
        public int Year
        {
            get { return year; }
            set
            {
                if (year != value)
                {
                    year = value; NotifyPropertyChanged("Entries");
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
