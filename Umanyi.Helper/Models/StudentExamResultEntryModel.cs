using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helper.Models
{
    public class StudentExamResultEntryModel:SubjectBaseModel
    {
        public StudentExamResultEntryModel()
        {
            Grade = "";
            Points = 0;
            MeanScore = 0;
            Remarks = "";
        }

        internal string GetRemark(decimal score)
        {
            int points = DataAccess.CalculatePoints(DataAccess.CalculateGrade(DataAccess.ConvertScoreToOutOf(score, 100, 100)));

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

        public string Grade { get; set; }

        public decimal? Cat1Score { get; set; }

        public decimal? Cat2Score { get; set; }

        public decimal? ExamScore { get; set; }

        public decimal MeanScore { get; set; }

        public int Points { get; set; }

        public int Code { get; set; }

        public string Tutor { get; set; }

        public string Remarks { get; set; }
    }
}
