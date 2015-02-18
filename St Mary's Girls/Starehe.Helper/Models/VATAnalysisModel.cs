using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helper.Models
{
    public class VATAnalysisModel: VATRateModel
    {
        decimal salesTaxable;
        decimal totalVATCollected;
        public VATAnalysisModel()
        {
            TotalVATCollected = 0;
            SalesTaxable = 0;
        }

        public decimal SalesTaxable
        {
            get { return salesTaxable; }
            set
            {
                if (salesTaxable != value)
                {
                    salesTaxable = value;
                    NotifyPropertyChanged("SalesTaxable");
                }
            }
        }

        public decimal TotalVATCollected
        {
            get { return totalVATCollected; }
            set
            {
                if (totalVATCollected != value)
                {
                    totalVATCollected = value;
                    NotifyPropertyChanged("TotalVATCollected");
                }
            }
        }
        public override void Reset()
        {
            base.Reset();
            SalesTaxable = 0;
            TotalVATCollected = 0;
        }
    }
}
