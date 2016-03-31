using System;
using System.Collections.ObjectModel;

namespace Helper.Models
{
    public class StudentExamResultModel : StudentSelectModel
    {
        private string nameOfClass;

        private string classPosition;

        private string overAllPosition;

        private decimal totalMarks;

        private decimal points;

        private string meanGrade;

        private ObservableCollection<StudentTranscriptSubjectModel> entries;

        private string nameOfExam;

        public string NameOfClass
        {
            get
            {
                return this.nameOfClass;
            }
            set
            {
                if (value != this.nameOfClass)
                {
                    this.nameOfClass = value;
                    base.NotifyPropertyChanged("NameOfClass");
                }
            }
        }

        public string ClassPosition
        {
            get
            {
                return this.classPosition;
            }
            set
            {
                if (value != this.classPosition)
                {
                    this.classPosition = value;
                    base.NotifyPropertyChanged("ClassPosition");
                }
            }
        }

        public string OverAllPosition
        {
            get
            {
                return this.overAllPosition;
            }
            set
            {
                if (value != this.overAllPosition)
                {
                    this.overAllPosition = value;
                    base.NotifyPropertyChanged("OverAllPosition");
                }
            }
        }

        public decimal TotalMarks
        {
            get
            {
                return this.totalMarks;
            }
            set
            {
                if (value != this.totalMarks)
                {
                    this.totalMarks = value;
                    base.NotifyPropertyChanged("TotalMarks");
                }
            }
        }

        public decimal Points
        {
            get
            {
                return this.points;
            }
            set
            {
                if (value != this.points)
                {
                    this.points = value;
                    base.NotifyPropertyChanged("Points");
                }
            }
        }

        public string MeanGrade
        {
            get
            {
                return this.meanGrade;
            }
            set
            {
                if (value != this.meanGrade)
                {
                    this.meanGrade = value;
                    base.NotifyPropertyChanged("MeanGrade");
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

        public ObservableCollection<StudentTranscriptSubjectModel> Entries
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

        public StudentExamResultModel()
        {
            this.NameOfClass = "";
            this.ClassPosition = "1/1";
            this.OverAllPosition = "1/1";
            this.TotalMarks = 0m;
            this.Points = 0m;
            this.MeanGrade = "";
            this.NameOfExam = "";
            this.Entries = new ObservableCollection<StudentTranscriptSubjectModel>();
        }

        internal virtual void CopyFrom(StudentExamResultModel newResult)
        {
            this.NameOfClass = newResult.NameOfClass;
            this.ClassPosition = newResult.ClassPosition;
            this.OverAllPosition = newResult.OverAllPosition;
            this.TotalMarks = newResult.TotalMarks;
            this.Points = newResult.Points;
            this.MeanGrade = newResult.MeanGrade;
            this.Entries = newResult.Entries;
        }

        public virtual void Clean()
        {
            this.NameOfClass = "";
            this.ClassPosition = "1/1";
            this.OverAllPosition = "1/1";
            this.TotalMarks = 0m;
            this.Points = 0m;
            this.MeanGrade = "";
            this.NameOfExam = "";
            this.Entries = new ObservableCollection<StudentTranscriptSubjectModel>();
        }
    }
}
