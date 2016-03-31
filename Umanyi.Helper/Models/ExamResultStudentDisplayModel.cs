using System;

namespace Helper.Models
{
    public class ExamResultStudentDisplayModel : ExamResultStudentModel
    {
        private string nameOfClass = "";

        private string nameOfExam = "";

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

        public ExamResultStudentDisplayModel()
        {
            this.NameOfClass = "";
            this.NameOfExam = "";
        }

        public ExamResultStudentDisplayModel(ExamResultStudentModel examResultModel)
        {
            this.NameOfClass = "";
            this.NameOfExam = "";
            base.Entries = examResultModel.Entries;
            base.ExamID = examResultModel.ExamID;
            base.ExamResultID = examResultModel.ExamResultID;
            base.StudentID = examResultModel.StudentID;
        }

        public override void Reset()
        {
            base.Reset();
            this.NameOfClass = "";
            this.NameOfExam = "";
        }
    }
}
