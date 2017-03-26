using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UmanyiSMS.Modules.Students.Models;

namespace UmanyiSMS.Modules.Exams.Models
{
    public class ReportFormModel:StudentSelectModel
    {
        private ObservableCollection<ReportFormSubjectModel> subjectEntries;
        private string className;
        private string classRank;
        private string streamRank;

        public ReportFormModel()
        {
            subjectEntries = new ObservableCollection<ReportFormSubjectModel>();
        }

        public string NameOfClass
        {
            get { return this.className; }

            set
            {
                if (value != this.className)
                {
                    this.className = value;
                    NotifyPropertyChanged("NameOfClass");
                }
            }
        }
        public ObservableCollection<ReportFormSubjectModel> SubjectEntries
        {
            get { return this.subjectEntries; }

            set
            {
                if (value != this.subjectEntries)
                {
                    this.subjectEntries = value;
                    NotifyPropertyChanged("SubjectEntries");
                }
            }

        }
        public string ClassRank
        {
            get { return this.classRank; }

            set
            {
                if (value != this.classRank)
                {
                    this.classRank = value;
                    NotifyPropertyChanged("ClassRank");
                }
            }
        }

        public string StreamRank
        {
            get { return this.streamRank; }

            set
            {
                if (value != this.streamRank)
                {
                    this.streamRank = value;
                    NotifyPropertyChanged("StreamRank");
                }
            }
        }

        public decimal TotalMarks { get; internal set; }
        public decimal MeanScore { get; internal set; }
        public string MeanGrade { get; internal set; }
        public decimal TotalPoints { get; internal set; }
        public decimal AvgPoints { get; internal set; }
        public DateTime OpeningDay { get; internal set; }
        public DateTime ClosingDay { get; internal set; }
        public string PrincipalComments { get; internal set; }
        public string ClassTeacherComments { get; internal set; }
        public byte[] SPhoto { get; internal set; }

        public void Clean()
        {
            NameOfClass = "";
            StreamRank = "";
            ClassRank = "";
            subjectEntries.Clear();
        }

        public void CopyFrom(ReportFormModel reportForm)
        {
            NameOfClass = reportForm.NameOfClass;
            StreamRank = reportForm.NameOfClass;
            ClassRank = reportForm.ClassRank;
            subjectEntries.Clear();
            foreach (var t in reportForm.subjectEntries)
                subjectEntries.Add(t);
        }
    }
}
