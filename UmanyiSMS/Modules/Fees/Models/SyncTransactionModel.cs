using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UmanyiSMS.Modules.Fees.Sync.Models
{
    public class SyncTransactionModel
    {
        public SyncTransactionModel()
        {
            TransactionType = "";
            TransactionDateTime = "";
            TransactionAmt = "";
        }

        public string TransactionType { get; set; }

        public string TransactionDateTime { get; set; }

        public string TransactionAmt { get; set; }
    }
}
