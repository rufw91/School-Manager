using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helper.Models
{
    public class ItemIssueEntryModel:ItemBaseModel
    {
        private decimal quantity;
        public ItemIssueEntryModel()
        {
            Quantity = 0;
        }

        public decimal Quantity
        {
            get { return this.quantity; }

            set
            {
                if (value != this.quantity)
                {
                    this.quantity = value;
                    NotifyPropertyChanged("Quantity");
                }
            }
        }
        public override void Reset()
        {
            base.Reset();
            Quantity = 0;
        }
    }
}
