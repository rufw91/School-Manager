using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helper.Models
{
    public class ItemIssueModel:ItemBaseModel
    {
        
        private decimal quantity;
        public ItemIssueModel()
        {
            Quantity = 0;
        }
        public ItemIssueModel(ItemBaseModel item):base(item)
        {
            Quantity = 0;
        }
        public override void Reset()
        {
            base.Reset();
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
        
    }
}
