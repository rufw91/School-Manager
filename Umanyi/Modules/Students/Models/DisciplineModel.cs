using System;

namespace UmanyiSMS.Modules.Students.Models
{
    public class DisciplineModel : StudentBaseModel
    {
        private int disciplineID;

        private string issue;

        private byte[] sPhoto;

        private DateTime dateAdded;

        public int DisciplineID
        {
            get
            {
                return this.disciplineID;
            }
            set
            {
                if (this.disciplineID != value)
                {
                    this.disciplineID = value;
                }
                base.NotifyPropertyChanged("DisciplineID");
            }
        }

        public string Issue
        {
            get
            {
                return this.issue;
            }
            set
            {
                if (this.issue != value)
                {
                    this.issue = value;
                }
                base.NotifyPropertyChanged("Issue");
            }
        }

        public byte[] SPhoto
        {
            get
            {
                return this.sPhoto;
            }
            set
            {
                if (this.sPhoto != value)
                {
                    this.sPhoto = value;
                }
                base.NotifyPropertyChanged("SPhoto");
            }
        }

        public DateTime DateAdded
        {
            get
            {
                return this.dateAdded;
            }
            set
            {
                if (this.dateAdded != value)
                {
                    this.dateAdded = value;
                }
                base.NotifyPropertyChanged("DateAdded");
            }
        }

        public DisciplineModel()
        {
            this.DisciplineID = 0;
            this.Issue = "";
            this.SPhoto = new byte[0];
            this.DateAdded = DateTime.Now;
        }

        public override void Reset()
        {
            this.DisciplineID = 0;
            this.Issue = "";
            this.SPhoto = new byte[0];
            this.DateAdded = DateTime.Now;
        }

        public void CopyFrom(DisciplineModel discipline)
        {
            this.DisciplineID = discipline.DisciplineID;
            this.Issue = discipline.Issue;
            this.DateAdded = discipline.DateAdded;
            base.StudentID = discipline.StudentID;
            base.NameOfStudent = discipline.NameOfStudent;
            this.SPhoto = discipline.SPhoto;
        }
    }
}
