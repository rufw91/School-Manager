using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helper.Models
{
    public class BudgetModel:ModelBase
    {
        private ObservableCollection<BudgetAccountModel> accounts;
        private ObservableCollection<BudgetEntryModel> entries;
        private decimal totalBudget;
        public BudgetModel()
        {
            accounts = new ObservableCollection<BudgetAccountModel>();
            PropertyChanged += (o, e) =>
                {
                    if (e.PropertyName=="TotalBudget")
                    {
                        foreach (var h in accounts)
                            h.BudgetAmount = h.BudgetPc * totalBudget / 100; 
                    }
                };

            accounts.CollectionChanged += (o, e) =>
                {
                    foreach (var h in accounts)
                        h.BudgetAmount = h.BudgetPc * totalBudget / 100; 
                };

        }

        public ObservableCollection<BudgetAccountModel> Accounts
        {
            get { return accounts; }

            set
            {
                if (value != accounts)
                {
                    accounts = value;
                    NotifyPropertyChanged("Accounts");
                }
            }
        }

        public ObservableCollection<BudgetEntryModel> Entries
        {
            get { return entries; }

            set
            {
                if (value != entries)
                {
                    entries = value;
                    NotifyPropertyChanged("Entries");
                }
            }
        }

        public decimal TotalBudget
        {
            get { return totalBudget; }

            set
            {
                if (value != totalBudget)
                {
                    totalBudget = value;
                    NotifyPropertyChanged("TotalBudget");
                }
            }
        }

        public override void Reset()
        {
            
        }
    }
}
