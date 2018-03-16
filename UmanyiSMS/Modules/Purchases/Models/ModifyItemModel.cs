using System;
using System.Collections.Generic;
using UmanyiSMS.Modules.Purchases.Controller;

namespace UmanyiSMS.Modules.Purchases.Models
{
    public class ModifyItemModel: ItemModel
    {
        public override bool CheckErrors()
        {
            ErrorCheckingStatus = UmanyiSMS.Lib.ErrorCheckingStatus.Incomplete;
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
                        this.ItemID = item.ItemID;
                        this.Description = item.Description;
                        this.Cost = item.Cost;
                        this.DateAdded = item.DateAdded;
                        this.ItemCategoryID = item.ItemCategoryID;
                    }
                }

            }
            catch (Exception e)
            {
                List<string> errors = new List<string>();
                errors.Add(e.Message);
                SetErrors("", errors);
            }
            NotifyPropertyChanged("HasErrors");
            ErrorCheckingStatus = UmanyiSMS.Lib.ErrorCheckingStatus.Complete;
            return HasErrors;
        }
    }
}
