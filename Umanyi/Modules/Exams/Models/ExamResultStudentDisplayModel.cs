using System;
using System.Collections.ObjectModel;

namespace UmanyiSMS.Modules.Exams.Models
{
    public class ExamResultStudentDisplayModel : ExamResultStudentModel
    {
        private string nameOfClass = "";

        private string nameOfExam = "";
        private string overAllPosition;
        private string classPosition;

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

        public ExamResultStudentDisplayModel()
        {
            this.NameOfClass = "";
            this.NameOfExam = "";
        }

        public ExamResultStudentDisplayModel(ExamResultStudentDisplayModel examResultModel)
        {
            this.CopyFrom(examResultModel);
        }

        public override void Reset()
        {
            base.Reset();
            this.NameOfClass = "";
            this.NameOfExam = "";
            this.ClassPosition = "1/1";
            this.OverAllPosition = "1/1";
        }
        
        internal virtual void CopyFrom(ExamResultStudentDisplayModel newResult)
        {
            this.NameOfStudent = newResult.NameOfStudent;
            base.ExamID = newResult.ExamID;
            base.ExamResultID = newResult.ExamResultID;
            this.NameOfClass = newResult.NameOfClass;
            this.ClassPosition = newResult.ClassPosition;
            this.OverAllPosition = newResult.OverAllPosition;
            this.Total = newResult.Total;
            this.TotalPoints = newResult.TotalPoints;
            this.MeanGrade = newResult.MeanGrade;
            this.Entries = newResult.Entries;
        }

        public virtual void Clean()
        {
            this.NameOfClass = "";
            this.ClassPosition = "1/1";
            this.OverAllPosition = "1/1";
            this.Total = 0m;
            this.TotalPoints = 0m;
            this.MeanGrade = "";
            this.NameOfExam = "";
            this.Entries = new ObservableCollection<ExamResultSubjectEntryModel>();
        }
    }
}
