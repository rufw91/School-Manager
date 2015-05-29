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
