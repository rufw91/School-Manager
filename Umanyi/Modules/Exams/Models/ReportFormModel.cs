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

        public string ClassName
        {
            get { return this.className; }

            set
            {
                if (value != this.className)
                {
                    this.className = value;
                    NotifyPropertyChanged("ClassName");
                }
            }
        }
        public ObservableCollection<ReportFormSubjectModel> SubjectEntries
        {
            get { return this.subjectEntries; }
            
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

        public void Clean()
        {
            ClassName = "";
            StreamRank = "";
            ClassRank = "";
            subjectEntries.Clear();
        }

        public void CopyFrom(ReportFormModel reportForm)
        {
            ClassName = reportForm.ClassName;
            StreamRank = reportForm.ClassName;
            ClassRank = reportForm.ClassRank;
            subjectEntries.Clear();
            foreach (var t in reportForm.subjectEntries)
                subjectEntries.Add(t);
        }
    }
}
