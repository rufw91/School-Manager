using System;

namespace UmanyiSMS.Lib.Models
{
    public class ComparisonModel
    {
        public string Description
        {
            get;
            set;
        }

        public int Value
        {
            get;
            set;
        }

        public ComparisonModel()
        {
        }

        public ComparisonModel(string description, int value)
        {
            this.Description = description;
            this.Value = value;
        }
    }
}
