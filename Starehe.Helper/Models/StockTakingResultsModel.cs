
using System.Collections.ObjectModel;
namespace Helper.Models
{
    public class StockTakingResultsModel:StockTakingBaseModel
    {
        ObservableCollection<ItemStockTakingResultsModel> items;
        public StockTakingResultsModel()
        {
            Items = new ObservableCollection<ItemStockTakingResultsModel>();
        }

        public ObservableCollection<ItemStockTakingResultsModel> Items
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
