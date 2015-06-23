using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helper.Models
{
    public class BooksPurchaseModel:PurchaseModel
    {
        private ObservableCollection<BookReceiptModel> items;
        public BooksPurchaseModel()
        {
            Items = new ObservableCollection<BookReceiptModel>();
            Items.CollectionChanged += (o, e) =>
            {
                RefreshOrderTotal();
            };
        }

        public new void RefreshOrderTotal()
        {
            OrderTotal = 0;
            foreach (BookReceiptModel ipm in Items)
                OrderTotal += ipm.TotalAmt;
        }

        public new ObservableCollection<BookReceiptModel> Items
        {
            get { return this.items; }

            set
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
            items.Clear();
        }
    }
}
