using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Helper.Models.Sync
{
    public class SyncStatementModel
    {
        public SyncStatementModel()
        {
        }
        public List<SyncTransactionModel> Transactions { get; set; }
        
        public string TotalDue { get; set; }

        public string TotalPayments { get; set; }

        public string TotalSales { get; set; }
    }
}
