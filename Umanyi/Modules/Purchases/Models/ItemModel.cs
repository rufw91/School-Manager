using System;


namespace UmanyiSMS.Modules.Purchases.Models
{
    public class ItemModel : ItemBaseModel
    {
        DateTime dateAdded;
        int itemCategoryId;
        decimal cost;

        public ItemModel()
        {
            DateAdded = DateTime.Now;
            ItemCategoryID = 0;
            Cost = 1;
        }

        public ItemModel(ItemModel item):base(item)
        {
            DateAdded = item.DateAdded;
            ItemCategoryID = item.ItemCategoryID;
            Cost = 1;
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
                
        public override void Reset()
        {
            base.Reset();
            DateAdded = DateTime.Now;
            ItemCategoryID = 0;
            Cost = 1;
        }

        
    }

}





