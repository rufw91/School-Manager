using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
namespace Helper.Models
{
    public class ItemCategoryModel : ModelBase
    {
        int _itemCategoryID;
        string description;
    
        public ItemCategoryModel()
        {
            ItemCategoryID = 0;
            Description = "";
        }
        public ItemCategoryModel(int itemCategoryID, string descrption)
        {
            ItemCategoryID = itemCategoryID;
            Description = descrption;
        }


        public int ItemCategoryID
        {
            get { return this._itemCategoryID; }

            set
            {
                if (value != this._itemCategoryID)
                {
                    this._itemCategoryID = value;
                    NotifyPropertyChanged("ItemCategoryID");
                }
            }
        }
        public string Description
        {
            get { return this.description; }

            set
            {
                if (value != this.description)
                {
                    this.description = value;
                    NotifyPropertyChanged("Description");
                }
            }
        }

        public override void Reset()
        {
            ItemCategoryID = 0;
            Description = "";
        }
       
    }
}
