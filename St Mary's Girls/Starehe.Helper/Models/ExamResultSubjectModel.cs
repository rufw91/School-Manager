
namespace Helper.Models
{
    public class ExamResultSubjectEntryModel: SubjectModel
    {
        string remarks;
        string tutor;
        decimal score;
        int examResultID;
        public ExamResultSubjectEntryModel()
        {
            ExamResultID = 0;
            Score = 0;
            Remarks = "";
            Tutor = "";
        }

        public ExamResultSubjectEntryModel(SubjectModel sm)
        {
            this.NameOfSubject = sm.NameOfSubject;
            this.SubjectID = sm.SubjectID;
        }

        public int ExamResultID
        {
            get { return this.examResultID; }

            set
            {
                if (value != this.examResultID)
                {
                    this.examResultID = value;
                    NotifyPropertyChanged("ExamResultID");
                }
            }
        }

        public decimal Score
        {
            get { return this.score; }

            set
            {
                if (value != this.score)
                {
                    this.score = value;
                    NotifyPropertyChanged("Score");
                }
            }
        }
        
        public string Remarks
        {
            get { return this.remarks; }

            set
            {
                if (value != this.remarks)
                {
                    this.remarks = value;
                    NotifyPropertyChanged("Remarks");
                }
            }
        }

        public string Tutor
        {
            get { return this.tutor; }

            set
            {
                if (value != this.tutor)
                {
                    this.tutor = value;
                    NotifyPropertyChanged("Tutor");
                }
            }
        }

        public override void Reset()
        {
            base.Reset();
            ExamResultID = 0; 
            Score = 0;
            Remarks = "";
            Tutor = "";
        }
    }
}
