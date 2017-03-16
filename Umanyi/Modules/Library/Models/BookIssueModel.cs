using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UmanyiSMS.Lib;

namespace UmanyiSMS.Modules.Library.Models
{
    public class BookIssueModel:ModelBase
    {
        int studentID;
        DateTime dateIssued;
        ObservableCollection<BookModel> entries;
        public BookIssueModel()
        {
            StudentID = 0;
            DateIssued = DateTime.Now;
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
        public DateTime DateIssued
        {
            get { return dateIssued; }

            set
            {
                if (value != this.dateIssued)
                {
                    this.dateIssued = value;
                    NotifyPropertyChanged("DateIssued");
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
            DateIssued = DateTime.Now;
            Entries.Clear();
        }
    }
}
