
using System.Collections.Generic;
using UmanyiSMS.Lib;

namespace UmanyiSMS.Modules.Purchases.Models
{
    public class SupplierBaseModel: ModelBase
    {
        int _supplierID;
        string _nameOfSupplier;
        public SupplierBaseModel()
        {
        }
        public SupplierBaseModel(int supplierID, string nameOfSupplier)
        {
            SupplierID = supplierID;
            NameOfSupplier = nameOfSupplier;
        }

        public int SupplierID
        {
            get { return this._supplierID; }

            set
            {
                if (value != this._supplierID)
                {
                    this._supplierID = value;
                    NotifyPropertyChanged("SupplierID");
                }
            }
        }

        public string NameOfSupplier
        {
            get { return this._nameOfSupplier; }

            set
            {
                if (value != this._nameOfSupplier)
                {
                    this._nameOfSupplier = value;
                    NotifyPropertyChanged("NameOfSupplier");
                }
            }
        }

        public override bool CheckErrors()
        {
            base.ClearAllErrors();
            if (this.SupplierID == 0)
            {
                base.SetErrors("SupplierID", new List<string>
                {
                    "Supplier does not exist."
                });
                this.NameOfSupplier = "";
            }
            else
            {
                SupplierBaseModel supplier = DataController.GetSupplier(this.SupplierID);
                if (supplier.SupplierID == 0)
                {
                    base.SetErrors("SupplierID", new List<string>
                    {
                        "Supplier does not exist."
                    });
                    this.NameOfSupplier = "";
                }
                else
                {
                    base.ClearErrors("SupplierID");
                    this.NameOfSupplier = supplier.NameOfSupplier;
                }
            }
            base.NotifyPropertyChanged("HasErrors");
            return base.HasErrors;
        }

        public override void Reset()
        {
            SupplierID = 0;
            NameOfSupplier = "";
        }
    }
}
