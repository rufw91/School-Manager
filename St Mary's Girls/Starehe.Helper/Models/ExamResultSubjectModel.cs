
using System.Windows;
namespace Helper.Models
{
    public class ExamResultSubjectEntryModel: SubjectModel
    {
        string remarks;
        string tutor;
        decimal score;
        int examResultID;
        public ExamResultSubjectEntryModel()
        {
            ExamResultID = 0;
            Score = 0;
            Remarks = "";
            Tutor = "";
            PropertyChanged += (o, e) =>
                {
                    if ((e.PropertyName == "Score") || (e.PropertyName == "NameOfSubject"))
                        Remarks = GetRemark(score);
                };
        }

        private string GetRemark(decimal score)
        {
            int points = DataAccess.CalculatePoints(DataAccess.CalculateGrade(score));
            
            if (NameOfSubject.ToUpper().Trim() != "KISWAHILI")
                switch (points)
                {
                    case 12: return "EXCELLENT";
                    case 11: return "VERY GOOD";
                    case 10: return "VERY GOOD";
                    case 9: return "GOOD";
                    case 8: return "ABOVE AVERAGE";
                    case 7: return "AVERAGE";
                    case 6: return "AVERAGE";
                    case 5: return "FAIR";
                    case 4: return "BELOW AVERAGE";
                    case 3: return "POOR";
                    case 2: return "VERY POOR";
                    case 1: return "WAKE UP";
                }
            else
                switch (points)
                {
                    case 12: return "HONGERA";
                    case 11: return "PONGEZI";
                    case 10: return "VIZURI SANA";
                    case 9: return "VIZURI";
                    case 8: return "HEKO";
                    case 7: return "WASTANI";
                    case 6: return "CHINI YA WASTANI";
                    case 5: return "TIA BIDII";
                    case 4: return "TIA BIDII";
                    case 3: return "AMKA";
                    case 2: return "PUNGUZA MZAHA";
                    case 1: return "ZINDUKA";
                }
            return "";
        }

        public ExamResultSubjectEntryModel(SubjectModel sm)
        {
            this.NameOfSubject = sm.NameOfSubject;
            this.SubjectID = sm.SubjectID;
        }

        public int ExamResultID
        {
            get { return this.examResultID; }

            set
            {
                if (value != this.examResultID)
                {
                    this.examResultID = value;
                    NotifyPropertyChanged("ExamResultID");
                }
            }
        }

        public decimal Score
        {
            get { return this.score; }

            set
            {
                if (value != this.score)
                {
                    this.score = value;
                    NotifyPropertyChanged("Score");
                }
            }
        }
        
        public string Remarks
        {
            get { return this.remarks; }

            set
            {
                if (value != this.remarks)
                {
                    this.remarks = value;
                    NotifyPropertyChanged("Remarks");
                }
            }
        }

        public string Tutor
        {
            get { return this.tutor; }

            set
            {
                if (value != this.tutor)
                {
                    this.tutor = value;
                    NotifyPropertyChanged("Tutor");
                }
            }
        }

        public override void Reset()
        {
            base.Reset();
            ExamResultID = 0; 
            Score = 0;
            Remarks = "";
            Tutor = "";
        }
    }
}
