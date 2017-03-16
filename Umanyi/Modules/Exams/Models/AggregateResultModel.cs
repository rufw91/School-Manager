using System;
using System.Collections.ObjectModel;
using UmanyiSMS.Lib;

namespace UmanyiSMS.Modules.Exams.Models
{
    public class AggregateResultModel : ModelBase
    {
        public string NameOfClass
        {
            get;
            set;
        }

        public string NameOfExam
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

        public string MeanGrade
        {
            get;
            set;
        }

        public ObservableCollection<AggregateResultEntryModel> Entries
        {
            get;
            set;
        }

        public decimal TotalScore
        {
            get;
            set;
        }

        public AggregateResultModel()
        {
            this.NameOfClass = "";
            this.NameOfExam = "";
            this.MeanScore = 0m;
            this.TotalScore = 0m;
            this.Points = 0;
            this.MeanGrade = "NOT DEF";
            this.Entries = new ObservableCollection<AggregateResultEntryModel>();
        }

        public override void Reset()
        {
            this.NameOfClass = "";
            this.NameOfExam = "";
            this.MeanScore = 0m;
            this.Points = 0;
            this.TotalScore = 0m;
            this.MeanGrade = "NOT DEF";
            this.Entries = new ObservableCollection<AggregateResultEntryModel>();
        }
    }
}
