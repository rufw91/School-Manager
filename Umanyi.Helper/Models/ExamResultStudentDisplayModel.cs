
namespace Helper.Models
{
    public class ExamResultStudentDisplayModel : ExamResultStudentModel
    {
        string nameOfClass = "";
        string nameOfExam = "";
        public ExamResultStudentDisplayModel()
        {
            NameOfClass = "";
            NameOfExam = "";
        }

        public ExamResultStudentDisplayModel(ExamResultStudentModel examResultModel)
        {
            NameOfClass = "";
            NameOfExam = "";
            this.Entries = examResultModel.Entries;
            this.ExamID = examResultModel.ExamID;
            this.ExamResultID = examResultModel.ExamResultID;
            this.StudentID = examResultModel.StudentID;
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
            NameOfClass = "";
            NameOfExam = "";
        }
    }
}
