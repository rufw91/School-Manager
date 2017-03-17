using System;
using UmanyiSMS.Modules.Institution.Models;

namespace UmanyiSMS.Modules.Exams.Models
{
    public class StudentExamResultEntryModel : SubjectBaseModel
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

      
        public StudentExamResultEntryModel()
        {
            this.Grade = "";
            this.Points = 0;
            this.MeanScore = 0m;
            this.Remarks = "";
        }

        internal string GetRemark(decimal score)
        {
            int num = DataController.CalculatePoints(DataController.CalculateGrade(DataController.ConvertScoreToOutOf(score, 100m, 100m)));
            string result;
            if (base.NameOfSubject.ToUpper().Trim() != "KISWAHILI")
            {
                switch (num)
                {
                    case 1:
                        result = "WAKE UP";
                        return result;
                    case 2:
                        result = "VERY POOR";
                        return result;
                    case 3:
                        result = "POOR";
                        return result;
                    case 4:
                        result = "BELOW AVERAGE";
                        return result;
                    case 5:
                        result = "FAIR";
                        return result;
                    case 6:
                        result = "AVERAGE";
                        return result;
                    case 7:
                        result = "AVERAGE";
                        return result;
                    case 8:
                        result = "ABOVE AVERAGE";
                        return result;
                    case 9:
                        result = "GOOD";
                        return result;
                    case 10:
                        result = "VERY GOOD";
                        return result;
                    case 11:
                        result = "VERY GOOD";
                        return result;
                    case 12:
                        result = "EXCELLENT";
                        return result;
                }
            }
            else
            {
                switch (num)
                {
                    case 1:
                        result = "ZINDUKA";
                        return result;
                    case 2:
                        result = "PUNGUZA MZAHA";
                        return result;
                    case 3:
                        result = "AMKA";
                        return result;
                    case 4:
                        result = "TIA BIDII";
                        return result;
                    case 5:
                        result = "TIA BIDII";
                        return result;
                    case 6:
                        result = "CHINI YA WASTANI";
                        return result;
                    case 7:
                        result = "WASTANI";
                        return result;
                    case 8:
                        result = "HEKO";
                        return result;
                    case 9:
                        result = "VIZURI";
                        return result;
                    case 10:
                        result = "VIZURI SANA";
                        return result;
                    case 11:
                        result = "PONGEZI";
                        return result;
                    case 12:
                        result = "HONGERA";
                        return result;
                }
            }
            result = "";
            return result;
        }
    }
}
