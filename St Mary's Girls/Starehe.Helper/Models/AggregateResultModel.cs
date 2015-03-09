using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helper.Models
{
    public class AggregateResultModel: ModelBase
    {
        public AggregateResultModel()
        {
            NameOfClass = "";
            NameOfExam = "";
            MeanScore = 0;
            TotalScore = 0;
            Points = 0;
            MeanGrade = "NOT DEF";
            Entries = new ObservableCollection<AggregateResultEntryModel>();
        }
        public string NameOfClass
        { get; set; }
        public string NameOfExam
        { get; set; }
        public decimal MeanScore
        { get; set; }
        public int Points
        { get; set; }
        public string MeanGrade
        { get; set; }
        public ObservableCollection<AggregateResultEntryModel> Entries
        { get; set; }
        public decimal TotalScore { get; set; }
        public override void Reset()
        {
            NameOfClass = "";
            NameOfExam = "";
            MeanScore = 0;
            Points = 0;
            TotalScore = 0;
            MeanGrade = "NOT DEF";
            Entries = new ObservableCollection<AggregateResultEntryModel>();
        }

        
    }
}
