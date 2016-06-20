using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helper.Models
{
    public class IncomeStatementModel : ModelBase
    {
        private AccountModel revenueEntries;
        private AccountModel expenseEntries;
        private List<TransactionModel> gainEntries;
        private List<TransactionModel> lossEntries;
        private DateTime endTime;
        private DateTime startTime;

        public IncomeStatementModel()
        {
            startTime = new DateTime(DateTime.Now.Year, 1, 1);
            endTime = new DateTime(DateTime.Now.Year, 12, 31);
            revenueEntries = new AccountModel();
            expenseEntries = new AccountModel();
            gainEntries = new List<TransactionModel>();
            lossEntries = new List<TransactionModel>();
        }

        public AccountModel RevenueEntries
        {
            get { return revenueEntries; }
            set { revenueEntries = value; }
        }

        public AccountModel ExpenseEntries
        {
            get { return expenseEntries; }
            set { expenseEntries = value; }
        }

        public List<TransactionModel> GainEntries
        {
            get { return gainEntries; }
            set { gainEntries = value; }
        }

        public List<TransactionModel> LossEntries
        {
            get { return lossEntries; }
            set { lossEntries = value; }
        }

        public override void Reset()
        {
            revenueEntries.Clear();
            expenseEntries.Clear();
            gainEntries.Clear();
            lossEntries.Clear();
        }

        public DateTime StartTime { get { return startTime; } set { startTime = value; } }

        public DateTime EndTime { get { return endTime; } set { endTime = value; } }

        public decimal NetIncome { get { return (TotalRevenue - TotalExpense); } }

        public decimal TotalRevenue
        {
            get { return GetTotalRevenue(); }
        }

        private decimal GetTotalExpense()
        {
            decimal j = 0;
            foreach (var t in expenseEntries)
                j += t.Balance;
            return j;
        }
        private decimal GetTotalRevenue()
        {
            decimal j = 0;
            foreach (var t in revenueEntries)
                j += t.Balance;
            return j;
        }

        public decimal TotalExpense
        {
            get { return GetTotalExpense(); }
        }
    }
}
