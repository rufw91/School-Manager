using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UmanyiSMS.Modules.Institution.Models;

namespace UmanyiSMS.Modules.Exams.Models
{
    public class ReportFormSubjectModel : SubjectBaseModel
    {
        private string _exam1Score;
        private string _exam2Score;
        private string _exam3Score;
        private string _streamRank;

        public ReportFormSubjectModel(int subjectID,string nameOfSubject, string exam1Score, string exam2Score, string exam3Score, string streamRank)
        {
            SubjectID = subjectID;
            NameOfSubject = nameOfSubject;
            Exam1Score = exam1Score;
            Exam1Score = exam2Score;
            Exam3Score = exam3Score;
            StreamRank = streamRank;
        }

        public string Exam1Score
        {
            get { return this._exam1Score; }

            set
            {
                if (value != this._exam1Score)
                {
                    this._exam1Score = value;
                    NotifyPropertyChanged("Exam1Score");
                }
            }
        }

        public string Exam2Score
        {
            get { return this._exam2Score; }

            set
            {
                if (value != this._exam2Score)
                {
                    this._exam2Score = value;
                    NotifyPropertyChanged("Exam2Score");
                }
            }
        }

        public string Exam3Score
        {
            get { return this._exam3Score; }

            set
            {
                if (value != this._exam3Score)
                {
                    this._exam3Score = value;
                    NotifyPropertyChanged("Exam3Score");
                }
            }
        }

        public string StreamRank
        {
            get { return this._streamRank; }

            set
            {
                if (value != this._streamRank)
                {
                    this._streamRank = value;
                    NotifyPropertyChanged("StreamRank");
                }
            }
        }
    }
}
