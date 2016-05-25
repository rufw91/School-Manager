using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helper.Models
{
    public class TrialBalanceModel:ModelBase
    {
        private List<TransactionModel> accounts;
        public TrialBalanceModel()
        {
            accounts = new List<TransactionModel>();
            StartTime = new DateTime(DateTime.Now.Year, 1, 1);
            EndTime = new DateTime(DateTime.Now.Year, 12, 31);
            DebitTotals = 0;
            CreditTotals = 0;
        }
        
        public DateTime EndTime { get; set; }

        public DateTime StartTime { get; set; }

        public List<TransactionModel>Accounts { get { return accounts; } }

        public override void Reset()
        {

        }

        public decimal CreditTotals { get; set; }

        public decimal DebitTotals { get; set; }
    }
}
