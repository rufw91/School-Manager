using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Helper.Models
{
    public class StudentExamResultModel : StudentSelectModel
    {
        private string nameOfClass;
        private string classPosition;
        private string overAllPosition;
        private decimal totalMarks;
        private int points;
        private string meanGrade;
        private ObservableCollection<StudentTranscriptSubjectModel> entries;
        public StudentExamResultModel()
        {
            NameOfClass = "";
            ClassPosition = "1/1";
            OverAllPosition = "1/1";
            TotalMarks = 0;
            Points = 0;
            MeanGrade = "";
            Entries = new ObservableCollection<StudentTranscriptSubjectModel>();
        }

        public string NameOfClass
        {
            get { return this.nameOfClass; }

            set
            {
                if (value != this.nameOfClass)
                {
                    this.nameOfClass = value;
                    NotifyPropertyChanged("NameOfClass");
                }
            }
        }

        public string ClassPosition
        {
            get { return this.classPosition; }

            set
            {
                if (value != this.classPosition)
                {
                    this.classPosition = value;
                    NotifyPropertyChanged("ClassPosition");
                }
            }
        }

        public string OverAllPosition
        {
            get { return this.overAllPosition; }

            set
            {
                if (value != this.overAllPosition)
                {
                    this.overAllPosition = value;
                    NotifyPropertyChanged("OverAllPosition");
                }
            }
        }

        public decimal TotalMarks
        {
            get { return this.totalMarks; }

            set
            {
                if (value != this.totalMarks)
                {
                    this.totalMarks = value;
                    NotifyPropertyChanged("TotalMarks");
                }
            }
        }

        public int Points
        {
            get { return this.points; }

            set
            {
                if (value != this.points)
                {
                    this.points = value;
                    NotifyPropertyChanged("Points");
                }
            }
        }

        public string MeanGrade
        {
            get { return this.meanGrade; }

            set
            {
                if (value != this.meanGrade)
                {
                    this.meanGrade = value;
                    NotifyPropertyChanged("MeanGrade");
                }
            }
        }

        public ObservableCollection<StudentTranscriptSubjectModel> Entries
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

        internal void CopyFrom(StudentExamResultModel newResult)
        {
            NameOfClass = newResult.NameOfClass;
            ClassPosition = newResult.ClassPosition;
            OverAllPosition = newResult.OverAllPosition;
            TotalMarks = newResult.TotalMarks;
            Points = newResult.Points;
            MeanGrade = newResult.MeanGrade;
            Entries = newResult.Entries;
        }
    }
}
