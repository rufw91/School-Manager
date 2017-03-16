using System;

namespace UmanyiSMS.Modules.Exams.Models
{
    public class StudentTranscriptSubjectModel
    {
        public string Code
        {
            get;
            set;
        }

        public string Tutor
        {
            get;
            set;
        }

        public string NameOfSubject
        {
            get;
            set;
        }

        public string Grade
        {
            get;
            set;
        }

        public string Remarks
        {
            get;
            set;
        }

        public int Score
        {
            get;
            set;
        }

        public int Points
        {
            get;
            set;
        }

        public StudentTranscriptSubjectModel(ExamResultSubjectEntryModel subjectResult)
        {
            this.NameOfSubject = subjectResult.NameOfSubject;
            this.Remarks = subjectResult.Remarks;
            this.Score = Convert.ToInt32(subjectResult.Score);
            this.Grade = DataAccess.CalculateGrade(this.Score);
            this.Points = DataAccess.CalculatePoints(this.Grade);
            this.Tutor = subjectResult.Tutor;
            this.Code = subjectResult.Code.ToString();
        }
    }
}
