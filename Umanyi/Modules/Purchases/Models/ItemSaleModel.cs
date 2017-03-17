using System.Collections.Generic;
using System.Threading.Tasks;

namespace UmanyiSMS.Modules.Purchases.Models
{
    public class ItemSaleModel : ItemBaseModel
    {
        decimal _quantity;
        decimal _price;
        decimal _totalAmt;
                
        public ItemSaleModel()
        {
            PropertyChanged += (o, e) =>
                {
                    if ((e.PropertyName == "Quantity") || (e.PropertyName == "Price"))
                    {
                        Total = Price * Quantity;
                    }
                };
            Quantity = 1;
            Price = 1;
        }
        
        public ItemSaleModel(long itemID,string description, decimal quantity, decimal price)
        {
            PropertyChanged += (o, e) =>
            {
                if ((e.PropertyName == "Quantity") || (e.PropertyName == "Price"))
                {
                    Total = Price * Quantity;
                }
            };
            ItemID = itemID;
            Quantity = quantity;
            Price = price;
            Description = description;
        }

        public ItemSaleModel(ItemModel itemModel):base(itemModel.ItemID,itemModel.Description)
        {
            PropertyChanged += (o, e) =>
            {
                if ((e.PropertyName == "Quantity") || (e.PropertyName == "Price"))
                {
                    Total = Price * Quantity;
                }
            };
            this.Price = itemModel.Price;
            this.Quantity = 1;
        }

        public decimal Price
        {
            get { return this._price; }

            set
            {
                if (value != this._price)
                {
                    this._price = value;
                    NotifyPropertyChanged("Price");
                    Total = Price * Quantity;
                }
            }
        }
        public decimal Quantity
        {
            get { return this._quantity; }

            set
            {
                if (value != this._quantity)
                {
                    this._quantity = value;
                    NotifyPropertyChanged("Quantity");
                    Total = Price * Quantity;
                }
            }
        }
        public decimal Total
        {
            get { return this._totalAmt; }

            private set
            {
                if (value != this._totalAmt)
                {
                    this._totalAmt = value;
                    NotifyPropertyChanged("Total");
                }
            }
        }
        
        public override bool CheckErrors()
        {
            try
            {
                ClearAllErrors();
                if (ItemID == 0)
                {
                    List<string> errors = new List<string>();
                    errors.Add("Item does not exist.");
                    SetErrors("ItemID", errors);
                }
                else
                {
                    ItemModel item = DataController.GetItem(ItemID);
                    if (item.ItemID == 0)
                    {
                        List<string> errors = new List<string>();
                        errors.Add("Item does not exist.");
                        SetErrors("ItemID", errors);
                    }
                    else
                    {
                        ClearErrors("ItemID");
                        this.Description = item.Description;
                    }
                }
            }
            catch { }
                return HasErrors;           
        }


        public override void Reset()
        {
            base.Reset();
            Quantity = 1;
            Price = 1;
        }
    }
}
