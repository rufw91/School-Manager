using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helper.Models
{
    public class AggregateResultEntryModel:ModelBase
    {
        public AggregateResultEntryModel()
        {
            NameOfSubject = "";
            MeanScore = 0;
            MeanGrade = "NOT DEF";
            Points = 0;
        }

        public string NameOfSubject
        { get; set; }
        public decimal MeanScore
        { get; set; }
        public string MeanGrade
        { get; set; }
        public int Points
        { get; set; }

        public override void Reset()
        {
            NameOfSubject = "";
            MeanScore = 0;
            MeanGrade = "NOT DEF";
            Points = 0;
        }
    }
}
