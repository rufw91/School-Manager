using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.ComponentModel;

using UmanyiSMS.Lib;

namespace UmanyiSMS.Modules.Fees.Models
{
    public class SaleModel : ModelBase
    {
        int saleID;
        DateTime dateAdded;
        int customerID;
        ObservableCollection<FeesStructureEntryModel> saleItems;
        decimal orderTotal;
        decimal numberOfItems;
        int employeeID;

        public SaleModel()
        {
            SaleID = 0;
            CustomerID = 0;
            OrderTotal = 0;
            NoOfItems = 0;
            EmployeeID = 0;
            DateAdded = DateTime.Now;
            SaleItems = new ObservableCollection<FeesStructureEntryModel>();
            saleItems.CollectionChanged += (s, e) =>
                {
                    RefreshTotal();
                };
        }

        public void RefreshTotal()
        {
            OrderTotal = 0;
            foreach (FeesStructureEntryModel ism in saleItems)
               OrderTotal += ism.Amount;
           
        }

        public int SaleID
        {
            get { return this.saleID; }

            set
            {
                if (value != this.saleID)
                {
                    this.saleID = value;
                    NotifyPropertyChanged("SaleID");
                }
            }
        }
        
        public int CustomerID
        {
            get { return this.customerID; }

            set
            {
                if (value != this.customerID)
                {
                    this.customerID = value;
                    NotifyPropertyChanged("CustomerID");
                }
            }
        }

        public decimal OrderTotal
        {
            get { return this.orderTotal; }

            set
            {
                if (value != this.orderTotal)
                {
                    this.orderTotal = value;
                    NotifyPropertyChanged("OrderTotal");
                }
            }
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

        public int EmployeeID
        {
            get { return this.employeeID; }

            set
            {
                if (value != this.employeeID)
                {
                    this.employeeID = value;
                    NotifyPropertyChanged("EmployeeID");
                }
            }
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

        public ObservableCollection<FeesStructureEntryModel> SaleItems
        {
            get { return this.saleItems; }

            set
            {
                if (value != this.saleItems)
                {
                    this.saleItems = value;
                    NotifyPropertyChanged("SaleItems");
                }
            }
        }

        public override void Reset()
        {
            SaleID = 0;
            NoOfItems = 0;
            CustomerID = 0;
            SaleItems.Clear();
        }
    }
}
