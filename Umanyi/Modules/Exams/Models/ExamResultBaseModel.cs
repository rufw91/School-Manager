using System;
using UmanyiSMS.Lib;

namespace UmanyiSMS.Modules.Exams.Models
{
    public class ExamResultBaseModel : ModelBase
    {
        private int examResultID;

        private int examID;

        public int ExamResultID
        {
            get
            {
                return this.examResultID;
            }
            set
            {
                if (value != this.examResultID)
                {
                    this.examResultID = value;
                    base.NotifyPropertyChanged("ExamResultID");
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

        public ExamResultBaseModel()
        {
            this.ExamResultID = 0;
            this.ExamID = 0;
        }

        public override void Reset()
        {
            this.ExamResultID = 0;
            this.ExamID = 0;
        }
    }
}
