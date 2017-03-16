using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using UmanyiSMS.Lib;

namespace UmanyiSMS.Modules.Purchases.Models
{
    public class ItemCategoryModel : ModelBase
    {
        int _itemCategoryID;
        int parentCategoryID;
        string description;
    
        public ItemCategoryModel()
        {
            ItemCategoryID = 0;
            Description = "";
            ParentCategoryID = 0;
        }
        public ItemCategoryModel(int itemCategoryID, string descrption, int parentCategoryID)
        {
            ItemCategoryID = itemCategoryID;
            Description = descrption;
            ParentCategoryID = parentCategoryID;
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

        public int ParentCategoryID
        {
            get { return this.parentCategoryID; }

            set
            {
                if (value != this.parentCategoryID)
                {
                    this.parentCategoryID = value;
                    NotifyPropertyChanged("ParentCategoryID");
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
            ParentCategoryID = 0;
        }
       
    }
}
