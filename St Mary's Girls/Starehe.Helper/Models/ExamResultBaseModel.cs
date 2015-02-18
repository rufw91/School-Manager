using System.Collections.ObjectModel;

namespace Helper.Models
{
    public class ExamResultBaseModel : ModelBase
    {
        int examResultID;
        int examID;

        public ExamResultBaseModel()
        {
            ExamResultID = 0;
            ExamID = 0;
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
        public int ExamID
        {
            get { return this.examID; }

            set
            {
                if (value != this.examID)
                {
                    this.examID = value;
                    NotifyPropertyChanged("ExamID");
                }
            }
        }
        public override void Reset()
        {
            ExamResultID = 0;
            ExamID = 0;
        }
    }
}
