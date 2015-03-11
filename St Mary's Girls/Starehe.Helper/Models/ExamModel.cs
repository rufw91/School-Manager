using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Helper.Models
{
    public class ExamModel: ModelBase
    {
        public ExamModel()
        {
            ExamID = 0;
            ClassID = 0;
            
            NameOfExam = "";
            Entries = new ObservableCollection<ExamSubjectEntryModel>();
        }
        int examID;
        int classID;
        string nameOfExam;
        ObservableCollection<ExamSubjectEntryModel> entries;
        
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

        public int ClassID
        {
            get { return this.classID; }

            set
            {
                if (value != this.classID)
                {
                    this.classID = value;
                    NotifyPropertyChanged("ClassID");
                }
            }
        }

        public ObservableCollection<ExamSubjectEntryModel> Entries
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

        public override void Reset()
        {
            ExamID = 0;
            ClassID = 0;
            NameOfExam = "";
            Entries = new ObservableCollection<ExamSubjectEntryModel>();
        }
    }

    public class ExamSubjectEntryModel : SubjectModel
    {
        int examID;
        DateTime examDateTime;
        DateTime examDate;
        TimeSpan examTime;
        public ExamSubjectEntryModel()
        {
            PropertyChanged += (o, e) =>
            {
                if ((e.PropertyName == "ExamTime") || (e.PropertyName == "ExamDate"))
                    ExamDateTime = ExamDate + ExamTime;
            }; 
            ExamID = 0;
            ExamDate = DateTime.Now.Date;
            ExamTime = DateTime.Now.TimeOfDay;
            
        }

        public ExamSubjectEntryModel(SubjectModel subjectModel)
        {
            PropertyChanged += (o, e) =>
            {
                if ((e.PropertyName == "ExamTime") || (e.PropertyName == "ExamDate"))
                    ExamDateTime = ExamDate + ExamTime;
            }; 
             ExamID = 0;
             ExamDate = DateTime.Now.Date;
             ExamTime = DateTime.Now.TimeOfDay;
            this.MaximumScore = subjectModel.MaximumScore;
            this.NameOfSubject = subjectModel.NameOfSubject;
            this.SubjectID = subjectModel.SubjectID;
            
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

        public DateTime ExamDate
        {
            get { return examDate; }
            set
            {
                if (examDate != value)
                {
                    examDate = value;
                    NotifyPropertyChanged("ExamDate");
                }
            }
        }

        public TimeSpan ExamTime
        {
            get { return examTime; }
            set
            {
                if (examTime != value)
                {
                    examTime = value;
                    NotifyPropertyChanged("ExamTime");
                }
            }
        }

        public DateTime ExamDateTime
        {
            get { return examDateTime; }
            set
            {
                if (examDateTime != value)
                    examDateTime = value;
                NotifyPropertyChanged("ExamDateTime");
            }
        }

        public override void Reset()
        {
            base.Reset();
            ExamID = 0;
            ExamDate = DateTime.Now.Date;
            ExamTime = DateTime.Now.TimeOfDay;
        }
    }
}
