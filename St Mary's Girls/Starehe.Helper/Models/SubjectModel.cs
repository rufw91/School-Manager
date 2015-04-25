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
        private string tutor;
        private int code;
        private bool isOptional;

        public SubjectModel()
        {
            MaximumScore = 0;
            Tutor = "";
            Code = 0;
            IsOptional = false;
        }

        public SubjectModel(int subjectID, string nameofsubject, decimal newMaximumScore,bool isOptional)
            : base(subjectID, nameofsubject)
        {
            MaximumScore = newMaximumScore;
            IsOptional = isOptional;
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

        public string Tutor
        {
            get { return this.tutor; }

            set
            {
                if (value != this.tutor)
                {
                    this.tutor = value;
                    NotifyPropertyChanged("Tutor");
                }
            }
        }

        public int Code
        {
            get { return this.code; }

            set
            {
                if (value != this.code)
                {
                    this.code = value;
                    NotifyPropertyChanged("Code");
                }
            }
        }

        public bool IsOptional
        {
            get { return this.isOptional; }

            set
            {
                if (value != this.isOptional)
                {
                    this.isOptional = value;
                    NotifyPropertyChanged("IsOptional");
                }
            }
        }

        public override void Reset()
        {
            base.Reset();
            MaximumScore = 0;
            Tutor = "";
            Code = 0;
            IsOptional = false;
        }
    }
}
