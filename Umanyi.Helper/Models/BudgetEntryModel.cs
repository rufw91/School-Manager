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
        public BudgetEntryModel()
        {

        }

        public BudgetEntryModel(ItemFindModel item)
        {
            this.AccountID = item.ItemCategoryID;
            this.Description = item.Description;
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
                    NotifyPropertyChanged("Price");
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
    }
}
