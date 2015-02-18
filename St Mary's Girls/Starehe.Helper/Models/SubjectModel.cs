using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Helper.Models
{
    public class SubjectModel : SubjectBaseModel
    {
        decimal maximumScore;

        public SubjectModel()
        {
            MaximumScore = 0;
        }

        public SubjectModel(int subjectID, string nameofsubject, decimal newMaximumScore)
            : base(subjectID, nameofsubject)
        {
            MaximumScore = newMaximumScore;
        }
        
        public decimal MaximumScore
        {
            get { return this.maximumScore; }

            set
            {
                if (value != this.maximumScore)
                {
                    this.maximumScore = value;
                    NotifyPropertyChanged("MaximumScore");
                }
            }
        }

        public override void Reset()
        {
            base.Reset();
            MaximumScore = 0;
        }
    }
}
