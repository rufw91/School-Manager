using System;
namespace UmanyiSMS.Modules.Purchases.Models
{
    public class ItemPurchaseModel: ItemBaseModel
    {
        decimal qty;
        decimal cost;
        decimal totalamt;
        public ItemPurchaseModel()
        {
            PropertyChanged += (o, e) =>
            {
                if (e.PropertyName == "Cost" || e.PropertyName == "Quantity")
                    TotalAmt = Cost * Quantity;
            };
            Quantity = 0;
            Cost = 0;
        }
        public ItemPurchaseModel(long itemID, string description, decimal quantity, decimal buyingPrice)
            : base(itemID, description)
        {
            PropertyChanged += (o, e) => 
            {
                if (e.PropertyName == "Cost" || e.PropertyName == "Quantity")
                    TotalAmt = Cost * Quantity;
            };
            Quantity = quantity;
            Cost = buyingPrice;
        }

        public ItemPurchaseModel(ItemModel itemModel)
            : base(itemModel.ItemID, itemModel.Description)
        {
            PropertyChanged += (o, e) =>
            {
                if (e.PropertyName == "Cost" || e.PropertyName == "Quantity")
                    TotalAmt = Cost * Quantity;
            };
            Cost = itemModel.Price;
            Quantity = 1;
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
