using System;
using System.Collections.ObjectModel;
using UmanyiSMS.Lib;

namespace UmanyiSMS.Modules.Purchases.Models
{
    public class PurchaseModel : ModelBase
    {
        decimal orderTotal;
        decimal numberOfItems;
        ObservableCollection<ItemPurchaseModel> items;
        DateTime orderDate;
        int purchaseID;
        int vendorID;
        bool isCancelled;
        private string refNo;
        public PurchaseModel()
        {
            PurchaseID = 0;
            SupplierID = 0;
            orderTotal = 0;
            NoOfItems = 0;
            Items = new ObservableCollection<ItemPurchaseModel>();
            OrderDate = DateTime.Now;
            RefNo = "";
            IsCancelled = false;
            items.CollectionChanged+=(o,e)=>
            {
                RefreshOrderTotal();
                NotifyPropertyChanged("OrderTotal");
            };
        }

        public void RefreshOrderTotal()
        {
            orderTotal = 0;
            foreach (ItemPurchaseModel ipm in Items)
                orderTotal += ipm.TotalAmt;
        }
        
        public int PurchaseID
        {
            get { return this.purchaseID; }

            set
            {
                if (value != this.purchaseID)
                {
                    this.purchaseID = value;
                    NotifyPropertyChanged("PurchaseID");
                }
            }
        }

        public string RefNo
        {
            get { return this.refNo; }

            set
            {
                if (value != this.refNo)
                {
                    this.refNo = value;
                    NotifyPropertyChanged("RefNo");
                }
            }
        }

        public bool IsCancelled
        {
            get { return this.isCancelled; }

            set
            {
                if (value != this.isCancelled)
                {
                    this.isCancelled = value;
                    NotifyPropertyChanged("IsCancelled");
                }
            }
        }

        public int SupplierID
        {
            get { return this.vendorID; }

            set
            {
                if (value != this.vendorID)
                {
                    this.vendorID = value;
                    NotifyPropertyChanged("SupplierID");
                }
            }
        }
        public decimal OrderTotal
        {
            get { RefreshOrderTotal(); return this.orderTotal; }            
        }
        public decimal NoOfItems
        {
            get { return this.numberOfItems; }

            set
            {
                if (value != this.numberOfItems)
                {
                    this.numberOfItems = value;
                    NotifyPropertyChanged("NoOfItems");
                }
            }
        }
        public ObservableCollection<ItemPurchaseModel> Items
        {
            get { return this.items; }

            set
            {
                if (value != this.items)
                {
                    this.items = value;
                    NotifyPropertyChanged("Items");
                }
            }
        }
        public DateTime OrderDate
        {
            get { return this.orderDate; }

            set
            {
                if (value != this.orderDate)
                {
                    this.orderDate = value;
                    NotifyPropertyChanged("OrderDate");
                }
            }
        }

        public override void Reset()
        {
            PurchaseID = 0;
            RefNo = "";
            SupplierID = 0;
            NoOfItems = 0;
            Items = new ObservableCollection<ItemPurchaseModel>();
            OrderDate = DateTime.Now;
        }

    }
}
