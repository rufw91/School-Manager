using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helper.Models
{
    public class BudgetAccountModel:AccountBaseModel
    {
        private decimal budgetAmount;
        public BudgetAccountModel()
        {
            BudgetAmount = 0;
        }

        public BudgetAccountModel(ItemCategoryModel account)
        {
            this.AccountID = account.ItemCategoryID;
            this.Name = account.Description;
            BudgetAmount = 0;
        }

        public BudgetAccountModel(int accountID,string name,decimal amount)
        {
            this.AccountID = accountID;
            this.Name = name;
            BudgetAmount = amount;
        }

        public decimal BudgetAmount
        {
            get { return this.budgetAmount; }

            set
            {
                if (value != this.budgetAmount)
                {
                    this.budgetAmount = value;
                    NotifyPropertyChanged("BudgetAmount");
                }
            }
        }

        public override void Reset()
        {
            base.Reset();
            BudgetAmount = 0;
        }
    }
}
