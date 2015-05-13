using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helper.Models
{
    public class ExamWeightModel:ExamModel
    {
        private decimal weight;
        private int index;
        private bool showInTranscript;
        public ExamWeightModel()
        {
            NameOfExam = "";
            ExamID = 0;
            Weight = 0;
            Index = 3;
            ShowInTranscript = true;
        }

        public decimal Weight
        {
            get { return this.weight; }

            set
            {
                if (value != this.weight)
                {
                    this.weight = value;
                    NotifyPropertyChanged("Weight");
                }
            }
        }

        public int Index
        {
            get { return this.index; }

            set
            {
                if (value != this.index)
                {
                    if ((value > 3) || (value < 1))                    
                        throw new ArgumentOutOfRangeException("Index", "Allowed vales are [1],[2],[3]");
                    
                    this.index = value;
                    NotifyPropertyChanged("Index");
                }
            }
        }

        public bool ShowInTranscript
        {
            get { return this.showInTranscript; }

            set
            {
                if (value != this.showInTranscript)
                {
                    this.showInTranscript = value;
                    NotifyPropertyChanged("ShowInTranscript");
                }
            }
        }
    }
}
