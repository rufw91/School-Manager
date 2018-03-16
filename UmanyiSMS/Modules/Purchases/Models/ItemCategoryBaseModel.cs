
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UmanyiSMS.Lib;

namespace UmanyiSMS.Modules.Purchases.Models
{
    public class ItemCategoryModel:ModelBase
    {
        private string description;
        private int itemCategoryID;

        public ItemCategoryModel()
        {
            ItemCategoryID = 0;
            Description = "";
        }

        public ItemCategoryModel(int itemCategoryID, string description)
        {
            ItemCategoryID = itemCategoryID;
            Description = description;
        }
        
        public int ItemCategoryID
        {
            get { return this.itemCategoryID; }

            set
            {
                if (value != this.itemCategoryID)
                {
                    this.itemCategoryID = value;
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
            Description = "";
            ItemCategoryID = 0;
        }
        
    }
}
