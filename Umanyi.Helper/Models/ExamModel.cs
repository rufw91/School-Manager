using System;
using System.Collections.ObjectModel;

namespace Helper.Models
{
    public class ExamModel : ModelBase
    {
        private ObservableCollection<ClassModel> classes;

        private int examID;

        private string nameOfExam;

        private ObservableCollection<ExamSubjectEntryModel> entries;

        private decimal outOf;

        private bool isRemovingInvalid;

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

        public int ExamID
        {
            get
            {
                return this.examID;
            }
            set
            {
                if (value != this.examID)
                {
                    this.examID = value;
                    base.NotifyPropertyChanged("ExamID");
                }
            }
        }

        public ObservableCollection<ClassModel> Classes
        {
            get
            {
                return this.classes;
            }
            set
            {
                if (value != this.classes)
                {
                    this.classes = value;
                    base.NotifyPropertyChanged("Classes");
                }
            }
        }

        public ObservableCollection<ExamSubjectEntryModel> Entries
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

        public decimal OutOf
        {
            get
            {
                return this.outOf;
            }
            set
            {
                if (value != this.outOf)
                {
                    if (decimal.Ceiling(value) > 100m || decimal.Ceiling(value) < 0m)
                    {
                        throw new ArgumentOutOfRangeException("OutOf", "Out Of value [" + value + "] is invalid. Should be non negative number greater than zero and less than or equal to 100");
                    }
                    this.outOf = value;
                    base.NotifyPropertyChanged("OutOf");
                }
            }
        }

        public DateTime ExamDateTime
        {
            get;
            set;
        }

        public ExamModel()
        {
            this.ExamID = 0;
            this.Classes = new ObservableCollection<ClassModel>();
            this.NameOfExam = "";
            this.Entries = new ObservableCollection<ExamSubjectEntryModel>();
            this.OutOf = 100m;
            this.ExamDateTime = DateTime.Now;
        }

        public override void Reset()
        {
            this.ExamID = 0;
            this.classes.Clear();
            this.NameOfExam = "";
            this.ExamDateTime = DateTime.Now;
            this.Entries = new ObservableCollection<ExamSubjectEntryModel>();
        }
    }
}
