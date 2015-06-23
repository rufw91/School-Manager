using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helper.Models
{
    public class BookReceiptModel:BookModel
    {
        private decimal totalamt;
        private decimal qty;
        private decimal cost;
        public BookReceiptModel()
        {
            Cost = 0;
            Quantity = 0;
            TotalAmt = 0;
            PropertyChanged += (o, e) =>
            {
                if (e.PropertyName == "Cost" || e.PropertyName == "Quantity")
                    TotalAmt = Cost * Quantity;
            };
        }

        public BookReceiptModel(BookModel book):
            base(book)
        {
            Cost = 0;
            Quantity = 1;
            TotalAmt = 0;
            PropertyChanged += (o, e) =>
            {
                if (e.PropertyName == "Cost" || e.PropertyName == "Quantity")
                    TotalAmt = Cost * Quantity;
            };
        }

        public decimal Cost
        {
            get { return this.cost; }

            set
            {
                if (value != this.cost)
                {
                    this.cost = value;
                    NotifyPropertyChanged("Cost");
                }
            }
        }
        public decimal Quantity
        {
            get { return this.qty; }

            set
            {
                if (value != this.qty)
                {
                    this.qty = value;
                    NotifyPropertyChanged("Quantity");
                }
            }
        }
        public decimal TotalAmt
        {
            get { return this.totalamt; }

            set
            {
                if (value != this.totalamt)
                {
                    this.totalamt = value;
                    NotifyPropertyChanged("TotalAmt");
                }
            }
        }
        public override void Reset()
        {
            base.Reset();
            Cost = 0;
            Quantity = 0;
            TotalAmt = 0;
        }

        
    }
}
