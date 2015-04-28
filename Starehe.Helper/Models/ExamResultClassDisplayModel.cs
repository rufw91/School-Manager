using System.Collections.ObjectModel;

namespace Helper.Models
{
    public class ExamResultClassDisplayModel : ExamResultClassModel
    {
        private string nameOfExam;

        public ExamResultClassDisplayModel()
        {
            NameOfExam = "";
        }

        public ExamResultClassDisplayModel(ExamResultClassModel classExamResult)
        {
            NameOfExam = "";
            this.ClassID = classExamResult.ClassID;
            this.Entries = classExamResult.Entries;
            this.ExamID = classExamResult.ExamID;
            this.ExamResultID = classExamResult.ExamResultID;            
        }

        public string NameOfExam
        {
            get { return this.nameOfExam; }

            set
            {
                if (value != this.nameOfExam)
                {
                    this.nameOfExam = value;
                    NotifyPropertyChanged("NameOfExam");
                }
            }
        }

        public override void Reset()
        {
            base.Reset();
            NameOfExam = "";
        }
    }
}