using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helper.Models
{
    public class ItemListModel: ItemModel
    {
        decimal currentQuantity;
        public ItemListModel()
        {
            CurrentQuantity = 0;
        }

        public decimal CurrentQuantity
        {
            get { return this.currentQuantity; }

            set
            {
                if (value != this.currentQuantity)
                {
                    this.currentQuantity = value;
                    NotifyPropertyChanged("CurrentQuantity");
                }
            }
        }

        public override void Reset()
        {
            base.Reset();
            CurrentQuantity = 0;
        }
    }
}
