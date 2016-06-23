using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helper.Models
{
    public class BudgetAccountModel:AccountBaseModel
    {
        private decimal budgetPc;
        private decimal budgetAmount;
        public BudgetAccountModel()
        {
            BudgetPc = 0;
            BudgetAmount = 0;
            PropertyChanged += (o, e) =>
                {
                    if (e.PropertyName=="BudgetPc")
                        if (budgetPc>100||budgetPc<0)
                        {
                            BudgetPc = 0;
                        }
                };
        }

        public BudgetAccountModel(ItemCategoryModel account)
        {
            this.AccountID = account.ItemCategoryID;
            this.Name = account.Description;
            BudgetPc = 0;
            BudgetAmount = 0;
            PropertyChanged += (o, e) =>
            {
                if (e.PropertyName == "BudgetPc")
                    if (budgetPc > 100 || budgetPc < 0)
                    {
                        BudgetPc = 0;
                    }
            };
        }

        public BudgetAccountModel(int accountID,string name,decimal amount,decimal budgetPC)
        {
            this.AccountID = accountID;
            this.Name = name;
            BudgetAmount = amount;
            BudgetPc = budgetPC;
            PropertyChanged += (o, e) =>
            {
                if (e.PropertyName == "BudgetPc")
                    if (budgetPc > 100 || budgetPc < 0)
                    {
                        BudgetPc = 0;
                    }
            };
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

        public decimal BudgetPc
        {
            get { return this.budgetPc; }

            set
            {
                if (value != this.budgetPc)
                {
                    this.budgetPc = value;
                    NotifyPropertyChanged("BudgetPc");
                }
            }
        }

        public override void Reset()
        {
            base.Reset();
            BudgetPc = 0;
            BudgetAmount = 0;
        }
    }
}
