using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UmanyiSMS.Modules.Purchases.Models
{
    public class ItemFindModel:ItemModel
    {
        bool isSelected;
        private decimal quantity;

        public ItemFindModel()
        {
            IsSelected = false;
            Quantity = 1;
        }
        public ItemFindModel(ItemModel item)
            : base(item)
        {
            IsSelected = false;
            Quantity = 1;
        }
        public override void Reset()
        {
            base.Reset();
            IsSelected = false;
            Quantity = 1;
        }

        public bool IsSelected
        {
            get { return this.isSelected; }

            set
            {
                if (value != this.isSelected)
                {
                    this.isSelected = value;
                    NotifyPropertyChanged("IsSelected");
                }
            }
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
