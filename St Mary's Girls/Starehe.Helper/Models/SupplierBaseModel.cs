
namespace Helper.Models
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
        public override void Reset()
        {
            SupplierID = 0;
            NameOfSupplier = "";
        }
    }
}
