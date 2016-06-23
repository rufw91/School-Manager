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
        private DateTime startDate;
        private DateTime endDate;
        private int budgetID;
        public BudgetModel()
        {
            BudgetID = 0;
            accounts = new ObservableCollection<BudgetAccountModel>();
            entries = new ObservableCollection<BudgetEntryModel>();
            StartDate = new DateTime(DateTime.Now.Year, 1, 1);
            EndDate = new DateTime(DateTime.Now.Year, 12, 31);
            PropertyChanged += (o, e) =>
                {
                    if (e.PropertyName=="TotalBudget")
                    {
                        RefreshValues();
                    }
                };

            accounts.CollectionChanged += (o, e) =>
                {
                    RefreshValues();
                };

        }

        public void RefreshValues()
        {
            foreach (var h in accounts)
            {
                h.BudgetAmount = h.BudgetPc * totalBudget / 100;                
            
            }
            var orphans=entries.Where(o => !accounts.Any(a => a.AccountID == o.AccountID));
            if (orphans!=null)
            {
                foreach (var y in orphans)
                    entries.Remove(y);
            }

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

        public DateTime StartDate
        {
            get { return startDate; }

            set
            {
                if (value != startDate)
                {
                    startDate = value;
                    NotifyPropertyChanged("StartDate");
                }
            }
        }

        public DateTime EndDate
        {
            get { return endDate; }

            set
            {
                if (value != endDate)
                {
                    endDate = value;
                    NotifyPropertyChanged("EndDate");
                }
            }
        }

        public int BudgetID
        {
            get { return budgetID; }

            set
            {
                if (value != budgetID)
                {
                    budgetID = value;
                    NotifyPropertyChanged("BudgetID");
                }
            }
        }
    }
}
