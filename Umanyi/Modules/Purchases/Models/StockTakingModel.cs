using System;
using System.Collections.ObjectModel;

namespace UmanyiSMS.Modules.Purchases.Models
{
    public class StockTakingModel :StockTakingBaseModel
    {
        ObservableCollection<ItemStockTakingModel> items;
        public StockTakingModel()
        {
            Items = new ObservableCollection<ItemStockTakingModel>();
        }

        public ObservableCollection<ItemStockTakingModel> Items
        {
            get { return this.items; }

            private set
            {
                if (value != this.items)
                {
                    this.items = value;
                    NotifyPropertyChanged("Items");
                }
            }
        }

        public override void Reset()
        {
            base.Reset();
            Items.Clear();
        }
    }
}
