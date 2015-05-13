using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helper.Models
{
    public class ItemStockTakingModel: ItemBaseModel
    {
        decimal availableQuantity;
        public ItemStockTakingModel()
        {
            AvailableQuantity = 0;
        }
        public ItemStockTakingModel(ItemBaseModel itemModel)
            : base(itemModel)
        {            
            AvailableQuantity = 0;
        }

        public decimal AvailableQuantity
        {
            get { return this.availableQuantity; }

            set
            {
                if (value != this.availableQuantity)
                {
                    this.availableQuantity = value;
                    NotifyPropertyChanged("AvailableQuantity");
                }
            }
        }

        public override void Reset()
        {
            base.Reset();
            AvailableQuantity = 0;
        }
    }
}
