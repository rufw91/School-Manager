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
            Classes = new ObservableCollection<ClassModel>();
            NameOfExam = "";
            Entries = new ObservableCollection<ExamSubjectEntryModel>();
            OutOf = 100;
            ExamDateTime = DateTime.Now;
        }
        ObservableCollection<ClassModel> classes;
        int examID;        
        string nameOfExam;
        ObservableCollection<ExamSubjectEntryModel> entries;
        private decimal outOf;
        private bool isRemovingInvalid;
        
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

        public ObservableCollection<ClassModel> Classes
        {
            get { return this.classes; }

            set
            {
                if (value != this.classes)
                {
                    this.classes = value;
                    NotifyPropertyChanged("Classes");
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

        public decimal OutOf
        {
            get { return this.outOf; }

            set
            {
                if (value != this.outOf)
                {
                    if ((decimal.Ceiling(value) > 100) || (decimal.Ceiling(value) < 0))
                        throw new ArgumentOutOfRangeException("OutOf", "Out Of value [" + value + "] is invalid. Should be non negative number greater than zero and less than or equal to 100");
                    this.outOf = value;
                    NotifyPropertyChanged("OutOf");
                }
            }
        }

        public override void Reset()
        {
            ExamID = 0;
            classes.Clear();
            NameOfExam = "";
            ExamDateTime = DateTime.Now;
            Entries = new ObservableCollection<ExamSubjectEntryModel>();
        }

        public DateTime ExamDateTime { get; set; }
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
