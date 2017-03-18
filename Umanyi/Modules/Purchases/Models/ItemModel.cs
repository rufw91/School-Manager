using System;


namespace UmanyiSMS.Modules.Purchases.Models
{
    public class ItemModel : ItemBaseModel
    {
        DateTime dateAdded;
        int itemCategoryId;
        decimal price;
        decimal cost;
        decimal startQuantity;
        int vatID;

        public ItemModel()
        {
            DateAdded = DateTime.Now;
            ItemCategoryID = 0;
            Price = 1;
            Cost = 1;
            StartQuantity = 0;
            VatID = 0;
        }

        public ItemModel(ItemModel item):base(item)
        {
            DateAdded = item.DateAdded;
            ItemCategoryID = item.ItemCategoryID;
            Price = item.Price;
            Cost = 1;
            StartQuantity = item.StartQuantity;
            VatID = item.VatID;
        }
        public DateTime DateAdded
        {
            get { return this.dateAdded; }

            set
            {
                if (value != this.dateAdded)
                {
                    this.dateAdded = value;
                    NotifyPropertyChanged("DateAdded");
                }
            }
        }
        public int ItemCategoryID
        {
            get { return this.itemCategoryId; }

            set
            {
                if (value != this.itemCategoryId)
                {
                    this.itemCategoryId = value;
                    NotifyPropertyChanged("ItemCategoryID");
                }
            }
        }
        public decimal Price
        {
            get { return this.price; }

            set
            {
                if (value != this.price)
                {
                    this.price = value;
                    NotifyPropertyChanged("Price");
                }
            }
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
        public decimal StartQuantity
        {
            get { return this.startQuantity; }

            set
            {
                if (value != this.startQuantity)
                {
                    this.startQuantity = value;
                    NotifyPropertyChanged("StartQuantity");
                }
            }
        }
        public int VatID
        {
            get { return this.vatID; }

            set
            {
                if (value != this.vatID)
                {
                    this.vatID = value;
                    NotifyPropertyChanged("VatID");
                }
            }
        }
        public override void Reset()
        {
            base.Reset();
            DateAdded = DateTime.Now;
            ItemCategoryID = 0;
            Price = 1;
            Cost = 1;
            StartQuantity = 0;
            VatID = 0;
        }

        
    }

}





