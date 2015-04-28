using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helper.Models
{
    public class ComparisonModel
    {
        public ComparisonModel()
        {
            
        }

        public ComparisonModel(string description, int value)
        {
            Description = description;
            Value = value;
        }

        public string Description
        { get; set; }

        public int Value
        { get; set; }
    }
}
