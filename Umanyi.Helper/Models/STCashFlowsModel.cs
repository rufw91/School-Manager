using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helper.Models
{
    public class STCashFlowsModel:ModelBase
    {
        private DateTime endTime;
        private DateTime startTime;
        private List<TransactionModel> operatingActivitiesEntries;
        private decimal startBalance;
        private decimal totRev;
        private decimal totExp;
        public STCashFlowsModel()
        {
            operatingActivitiesEntries = new List<TransactionModel>();
        }

        public STCashFlowsModel(IncomeStatementModel incomeStatementModel)
        {
            CopyFromIS(incomeStatementModel);
        }

        private void CopyFromIS(IncomeStatementModel incomeStatementModel)
        {
            operatingActivitiesEntries = new List<TransactionModel>();
            operatingActivitiesEntries.Add(incomeStatementModel.RevenueEntries[0]);
            operatingActivitiesEntries.Add(incomeStatementModel.RevenueEntries[1]);

            totRev = incomeStatementModel.TotalRevenue;

            operatingActivitiesEntries.Add(incomeStatementModel.ExpenseEntries[0]);
            operatingActivitiesEntries.Add(incomeStatementModel.ExpenseEntries[4]);
            operatingActivitiesEntries.Add(incomeStatementModel.ExpenseEntries[2]);
            operatingActivitiesEntries.Add(incomeStatementModel.ExpenseEntries[3]);
            operatingActivitiesEntries.Add(incomeStatementModel.ExpenseEntries[1]);
            operatingActivitiesEntries.Add(incomeStatementModel.ExpenseEntries[5]);

            totExp = incomeStatementModel.TotalExpense;
            this.StartTime = incomeStatementModel.StartTime;
            this.EndTime = incomeStatementModel.EndTime;
        }

        public DateTime StartTime { get { return startTime; } set { startTime = value; } }

        public DateTime EndTime { get { return endTime; } set { endTime = value; } }

        public List<TransactionModel> OperatingActivitiesEntries
        {
            get { return operatingActivitiesEntries; }
        }


        public decimal StartBalance
        {
            get { return startBalance; }
            set { startBalance = value; }
        }

        public decimal EndBalance
        {
            get { return GetEndBalance(); }
        }

        private decimal GetEndBalance()
        {
            return startBalance + OperatingActivitiesTotal;
        }

        public override void Reset()
        {
            
        }

        public decimal OperatingActivitiesTotal { get { return totRev - totExp; } }

    }
}
