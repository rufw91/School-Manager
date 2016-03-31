using System;

namespace Helper.Models
{
    public class AggregateResultEntryModel : ModelBase
    {
        public string NameOfSubject
        {
            get;
            set;
        }

        public decimal MeanScore
        {
            get;
            set;
        }

        public string MeanGrade
        {
            get;
            set;
        }

        public int Points
        {
            get;
            set;
        }

        public AggregateResultEntryModel()
        {
            this.NameOfSubject = "";
            this.MeanScore = 0m;
            this.MeanGrade = "NOT DEF";
            this.Points = 0;
        }

        public override void Reset()
        {
            this.NameOfSubject = "";
            this.MeanScore = 0m;
            this.MeanGrade = "NOT DEF";
            this.Points = 0;
        }
    }
}
