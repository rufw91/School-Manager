using System;
using System.ComponentModel;

namespace Helper.Models
{
    public class ExamSubjectEntryModel : SubjectModel
    {
        private int examID;

        private DateTime examDateTime;

        private DateTime examDate;

        private TimeSpan examTime;

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

        public DateTime ExamDate
        {
            get
            {
                return this.examDate;
            }
            set
            {
                if (this.examDate != value)
                {
                    this.examDate = value;
                    base.NotifyPropertyChanged("ExamDate");
                }
            }
        }

        public TimeSpan ExamTime
        {
            get
            {
                return this.examTime;
            }
            set
            {
                if (this.examTime != value)
                {
                    this.examTime = value;
                    base.NotifyPropertyChanged("ExamTime");
                }
            }
        }

        public DateTime ExamDateTime
        {
            get
            {
                return this.examDateTime;
            }
            set
            {
                if (this.examDateTime != value)
                {
                    this.examDateTime = value;
                }
                base.NotifyPropertyChanged("ExamDateTime");
            }
        }

        public ExamSubjectEntryModel()
        {
            base.PropertyChanged += delegate (object o, PropertyChangedEventArgs e)
            {
                if (e.PropertyName == "ExamTime" || e.PropertyName == "ExamDate")
                {
                    this.ExamDateTime = this.ExamDate + this.ExamTime;
                }
            };
            this.ExamID = 0;
            this.ExamDate = DateTime.Now.Date;
            this.ExamTime = DateTime.Now.TimeOfDay;
        }

        public ExamSubjectEntryModel(SubjectModel subjectModel)
        {
            base.PropertyChanged += delegate (object o, PropertyChangedEventArgs e)
            {
                if (e.PropertyName == "ExamTime" || e.PropertyName == "ExamDate")
                {
                    this.ExamDateTime = this.ExamDate + this.ExamTime;
                }
            };
            this.ExamID = 0;
            this.ExamDate = DateTime.Now.Date;
            this.ExamTime = DateTime.Now.TimeOfDay;
            base.MaximumScore = subjectModel.MaximumScore;
            base.NameOfSubject = subjectModel.NameOfSubject;
            base.SubjectID = subjectModel.SubjectID;
        }

        public override void Reset()
        {
            base.Reset();
            this.ExamID = 0;
            this.ExamDate = DateTime.Now.Date;
            this.ExamTime = DateTime.Now.TimeOfDay;
        }
    }
}
