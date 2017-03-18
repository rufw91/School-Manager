using System;
using System.Data;
using UmanyiSMS.Modules.Institution.Models;

namespace UmanyiSMS.Modules.Exams.Models
{
    public class ClassExamResultModel : ClassModel
    {
        private DataTable entries;
        private string nameOfExam;

        public DataTable Entries
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

        public string NameOfExam
        {
            get
            {
                return this.nameOfExam;
            }
            set
            {
                if (value != this.nameOfExam)
                {
                    this.nameOfExam = value;
                    base.NotifyPropertyChanged("NameOfExam");
                }
            }
        }

        public ClassExamResultModel()
        {
            this.Entries = new DataTable();
            NameOfExam = "";
        }

        public override void Reset()
        {
            this.entries.Clear();
            NameOfExam = "";
        }
    }
}
