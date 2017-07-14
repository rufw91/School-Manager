using System;
using UmanyiSMS.Modules.Institution.Controller;
using UmanyiSMS.Modules.Institution.Models;

namespace UmanyiSMS.Modules.Exams.Models
{
    public class ReportForm2SubjectModel : SubjectBaseModel
    {
        public string Grade
        {
            get;
            set;
        }

        public decimal? Cat1Score
        {
            get;
            set;
        }

        public decimal? Cat2Score
        {
            get;
            set;
        }

        public decimal? ExamScore
        {
            get;
            set;
        }

        public decimal MeanScore
        {
            get;
            set;
        }

        public int Points
        {
            get;
            set;
        }

        public int Code
        {
            get;
            set;
        }

        public string Tutor
        {
            get;
            set;
        }

        public string Remarks
        {
            get;
            set;
        }

      
        public ReportForm2SubjectModel()
        {
            this.Grade = "";
            this.Points = 0;
            this.MeanScore = 0m;
            this.Remarks = "";
        }
        
    }
}
