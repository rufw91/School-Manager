using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helper.Models
{
    public class ReportFormModel:StudentBaseModel
    {
        private List<ReportFormSubjectModel> subjectEntries;
        private string className;
        private string classRank;
        private string streamRank;

        public ReportFormModel()
        {
            subjectEntries = new List<ReportFormSubjectModel>();
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
        public List<ReportFormSubjectModel> SubjectEntries
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
    }
}
