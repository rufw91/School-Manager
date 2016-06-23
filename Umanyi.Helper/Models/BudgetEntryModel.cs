using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helper.Models
{
    public class BudgetEntryModel:ModelBase
    {
        private int accountID;
        private string description;
        private decimal budgetedAmount;
        private decimal expenditure;
        private decimal balance;
        private decimal budgetedPrice;
        private decimal budgetedQuantity;
        private decimal actualPrice;
        private decimal actualQuantity;
        private long itemID;
        public BudgetEntryModel()
        {
            PropertyChanged += (o, e) =>
            {
                if ((e.PropertyName == "BudgetedQuantity") || (e.PropertyName == "BudgetedPrice"))
                    BudgetedAmount = BudgetedPrice * BudgetedQuantity;
            };
        }

        
        public BudgetEntryModel(ItemFindModel item)
        {
            this.ItemID = item.ItemID;
            this.AccountID = item.ItemCategoryID;
            this.Description = item.Description;
            PropertyChanged += (o, e) =>
            {
                if ((e.PropertyName == "BudgetedQuantity") || (e.PropertyName == "BudgetedPrice"))
                    BudgetedAmount = BudgetedPrice * BudgetedQuantity;
            };
        }

        public int AccountID
        {
            get { return accountID; }

            set
            {
                if (value != accountID)
                {
                    accountID = value;
                    NotifyPropertyChanged("AccountID");
                }
            }
        }

        public string Description
        {
            get { return description; }

            set
            {
                if (value != description)
                {
                    description = value;
                    NotifyPropertyChanged("Description");
                }
            }
        }

        public decimal BudgetedQuantity
        {
            get { return budgetedQuantity; }

            set
            {
                if (value != budgetedQuantity)
                {
                    budgetedQuantity = value;
                    NotifyPropertyChanged("BudgetedQuantity");
                }
            }
        }

        public decimal ActualQuantity
        {
            get { return actualQuantity; }

            set
            {
                if (value != actualQuantity)
                {
                    actualQuantity = value;
                    NotifyPropertyChanged("ActualQuantity");
                }
            }
        }

        public decimal BudgetedPrice
        {
            get { return budgetedPrice; }

            set
            {
                if (value != budgetedPrice)
                {
                    budgetedPrice = value;
                    NotifyPropertyChanged("BudgetedPrice");
                }
            }
        }

        public decimal ActualPrice
        {
            get { return actualPrice; }

            set
            {
                if (value != actualPrice)
                {
                    actualPrice = value;
                    NotifyPropertyChanged("ActualPrice");
                }
            }
        }

        public decimal BudgetedAmount
        {
            get { return budgetedAmount; }

            set
            {
                if (value != budgetedAmount)
                {
                    budgetedAmount = value;
                    NotifyPropertyChanged("BudgetedAmount");
                }
            }
        }

        public decimal Expenditure
        {
            get { return expenditure; }

            set
            {
                if (value != expenditure)
                {
                    expenditure = value;
                    NotifyPropertyChanged("Expenditure");
                }
            }
        }

        public decimal Balance
        {
            get { return balance; }

            set
            {
                if (value != balance)
                {
                    balance = value;
                    NotifyPropertyChanged("Balance");
                }
            }
        }

        public override void Reset()
        {
            
        }

        public long ItemID
        {
            get { return itemID; }

            set
            {
                if (value != itemID)
                {
                    itemID = value;
                    NotifyPropertyChanged("ItemID");
                }
            } }
    }
}
