using System;
using System.ComponentModel;

using UmanyiSMS.Lib;

namespace UmanyiSMS.Modules.Purchases.Models
{
    public class ItemBaseModel : ModelBase
    {
        long _itemID;
        string _description;

        public ItemBaseModel()
        {
            ItemID = 0;
            Description = "";
        }

        public ItemBaseModel(long itemID, string description)
        {
            ItemID = itemID;
            Description = description;
        }

        public ItemBaseModel(ItemBaseModel itemModel)
        {
            ItemID = itemModel.ItemID;
            Description = itemModel.Description;
        }

        public long ItemID
        {
            get { return this._itemID; }

            set
            {
                if (value != this._itemID)
                {
                    this._itemID = value;
                    NotifyPropertyChanged("ItemID");
                }
            }
        }

        public string Description
        {
            get { return this._description; }

            set
            {
                if (value != this._description)
                {
                    this._description = value;
                    NotifyPropertyChanged("Description");
                }
            }
        }
        public override void Reset()
        {
            ItemID = 0;
            Description = "";
        }
    }
}

