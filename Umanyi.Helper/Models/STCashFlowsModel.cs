using Helper.Presentation;
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
        private List<IAccount> operatingActivitiesEntries;
        private decimal startBalance;
        private decimal totRev;
        private decimal totExp;
        public STCashFlowsModel()
        {
            operatingActivitiesEntries = new List<IAccount>();
        }

        public STCashFlowsModel(IncomeStatementModel incomeStatementModel)
        {
            CopyFromIS(incomeStatementModel);
        }

        private void CopyFromIS(IncomeStatementModel incomeStatementModel)
        {
            operatingActivitiesEntries = new List<IAccount>();
            foreach (var r in incomeStatementModel.RevenueEntries)
            {
                r.Name = "CASH COLLECTED FROM " + r.Name;
                operatingActivitiesEntries.Add(r);
            }

            totRev = incomeStatementModel.TotalRevenue;

            foreach (var r in incomeStatementModel.ExpenseEntries)
            {
                r.Name = "CASH PAID FOR " + r.Name;
                operatingActivitiesEntries.Add(r);
            }

            totExp = incomeStatementModel.TotalExpense;
            this.StartTime = incomeStatementModel.StartTime;
            this.EndTime = incomeStatementModel.EndTime;
        }

        public DateTime StartTime { get { return startTime; } set { startTime = value; } }

        public DateTime EndTime { get { return endTime; } set { endTime = value; } }

        public List<IAccount> OperatingActivitiesEntries
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
            get { return startBalance + OperatingActivitiesTotal; }
        }

        public override void Reset()
        {
            
        }

        public decimal OperatingActivitiesTotal { get { return totRev - totExp; } }

    }
}
