using System;

namespace UmanyiSMS.Modules.Exams.Models
{
    public class ExamWeightModel : ExamModel
    {
        private decimal weight;

        private int index;

        private bool showInTranscript;

        public decimal Weight
        {
            get
            {
                return this.weight;
            }
            set
            {
                if (value != this.weight)
                {
                    this.weight = value;
                    base.NotifyPropertyChanged("Weight");
                }
            }
        }

        public int Index
        {
            get
            {
                return this.index;
            }
            set
            {
                if (value != this.index)
                {
                    if (value > 3 || value < 1)
                    {
                        throw new ArgumentOutOfRangeException("Index", "Allowed vales are [1],[2],[3]");
                    }
                    this.index = value;
                    base.NotifyPropertyChanged("Index");
                }
            }
        }

        public bool ShowInTranscript
        {
            get
            {
                return this.showInTranscript;
            }
            set
            {
                if (value != this.showInTranscript)
                {
                    this.showInTranscript = value;
                    base.NotifyPropertyChanged("ShowInTranscript");
                }
            }
        }

        public ExamWeightModel()
        {
            base.NameOfExam = "";
            base.ExamID = 0;
            this.Weight = 0m;
            this.Index = 3;
            this.ShowInTranscript = true;
        }
    }
}
