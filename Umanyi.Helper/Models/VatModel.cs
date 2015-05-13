using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helper.Models
{
    public class VATRateModel : ModelBase
    {
        string description;
        int vatID;
        decimal rate;
        public VATRateModel()
        {
            Description = "";
            VatID = 0;
            Rate =0;
        }
        public string Description
        {
            get { return description; }
            set
            {
                if (description != value)
                {
                    description = value;
                    NotifyPropertyChanged("Description");
                }
            }
        }

        public int VatID
        {
            get { return vatID; }
            set
            {
                if (vatID != value)
                {
                    vatID = value;
                    NotifyPropertyChanged("VatID");
                }
            }
        }
        public decimal Rate
        {
            get { return rate; }
            set
            {
                if (rate != value)
                {
                    rate = value;
                    NotifyPropertyChanged("Rate");
                }
            }
        }
        public override void Reset()
        {
            VatID = 0;
            Description = "";
            Rate = 0;
        }
    }
}
