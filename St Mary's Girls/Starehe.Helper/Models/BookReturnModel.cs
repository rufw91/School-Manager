using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helper.Models
{
    public class BookReturnModel:ModelBase
    {
        int studentID;
        DateTime dateReturned;
        ObservableCollection<BookModel> entries;
        public BookReturnModel()
        {
            StudentID = 0;
            DateReturned = DateTime.Now;
            Entries = new ObservableCollection<BookModel>();
        }
        public int StudentID
        {
            get { return this.studentID; }

            set
            {
                if (value != this.studentID)
                {
                    this.studentID = value;
                    NotifyPropertyChanged("StudentID");
                }
            }
        }
        public DateTime DateReturned
        {
            get { return dateReturned; }

            set
            {
                if (value != this.dateReturned)
                {
                    this.dateReturned = value;
                    NotifyPropertyChanged("DateReturned");
                }
            }
        }
        public ObservableCollection<BookModel> Entries
        {
            get { return entries; }

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
            StudentID = 0;
            DateReturned = DateTime.Now;
            Entries.Clear();
        }
    }
}
