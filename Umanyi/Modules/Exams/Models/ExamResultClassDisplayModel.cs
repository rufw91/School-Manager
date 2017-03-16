using System;

namespace UmanyiSMS.Modules.Exams.Models
{
    public class ExamResultClassDisplayModel : ExamResultClassModel
    {
        private string nameOfExam;

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

        public ExamResultClassDisplayModel()
        {
            this.NameOfExam = "";
        }

        public ExamResultClassDisplayModel(ExamResultClassModel classExamResult)
        {
            this.NameOfExam = "";
            base.ClassID = classExamResult.ClassID;
            base.Entries = classExamResult.Entries;
            base.ExamID = classExamResult.ExamID;
            base.ExamResultID = classExamResult.ExamResultID;
        }

        public override void Reset()
        {
            base.Reset();
            this.NameOfExam = "";
        }
    }
}
