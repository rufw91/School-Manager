using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helper.Models
{
    public class DisciplineModel:StudentBaseModel
    {
        int disciplineID;
        string issue;
        private byte[] sPhoto;
        private DateTime dateAdded;
        public DisciplineModel()
        {
            DisciplineID = 0;
            Issue = "";
            SPhoto = new byte[0];
            DateAdded = DateTime.Now;
        }
        public override void Reset()
        {
            DisciplineID = 0;
            Issue ="";
            SPhoto = new byte[0];
            DateAdded = DateTime.Now;
        }

        public int DisciplineID
        {
            get { return disciplineID; }
            set
            {
                if (disciplineID != value)
                    disciplineID = value;
                NotifyPropertyChanged("DisciplineID");
            }
        }

        public string Issue
        {
            get { return issue; }
            set
            {
                if (issue != value)
                    issue = value;
                NotifyPropertyChanged("Issue");
            }
        }

        public byte[] SPhoto
        {
            get { return sPhoto; }
            set
            {
                if (sPhoto != value)
                    sPhoto = value;
                NotifyPropertyChanged("SPhoto");
            }
        }

        public DateTime DateAdded
        {
            get { return dateAdded; }
            set
            {
                if (dateAdded != value)
                    dateAdded = value;
                NotifyPropertyChanged("DateAdded");
            }
        }

        public void CopyFrom(DisciplineModel discipline)
        {
            DisciplineID = discipline.DisciplineID;
            Issue = discipline.Issue;
            DateAdded = discipline.DateAdded;
            StudentID= discipline.StudentID;
            NameOfStudent= discipline.NameOfStudent;
            SPhoto=discipline.SPhoto;
        }
    }
}
