using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helper.Models
{
    public class TermModel:ModelBase
    {
        private int termID;
        private string description;
        private DateTime startDate;
        private DateTime endDate;
        public TermModel()
        {
            Description = "";
            TermID = 0;
        }
        public int TermID
        {
            get { return this.termID; }

            set
            {
                if (value != this.termID)
                {
                    this.termID = value;
                    NotifyPropertyChanged("TermID");
                }
            }
        }

        public string Description
        {
            get { return this.description; }

            set
            {
                if (value != this.description)
                {
                    this.description = value;
                    NotifyPropertyChanged("Description");
                }
            }
        }

        public DateTime StartDate
        {
            get { return this.startDate; }

            set
            {
                if (value != this.startDate)
                {
                    this.startDate = value;
                    NotifyPropertyChanged("StartDate");
                }
            }
        }

        public DateTime EndDate
        {
            get { return this.endDate; }

            set
            {
                if (value != this.endDate)
                {
                    this.endDate = value;
                    NotifyPropertyChanged("EndDate");
                }
            }
        }

        public override void Reset()
        {
            Description = "";
            TermID = 0;
        }
    }
}
