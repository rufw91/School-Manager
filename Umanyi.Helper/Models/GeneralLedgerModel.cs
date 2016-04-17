using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helper.Models
{
    public class GeneralLedgerModel
    {
        public GeneralLedgerModel()
        {
            Entries = new ObservableCollection<TransactionModel>();
        }

        public string AccountName
        { get; set; }

        public DateTime Date
        { get; set; }

        public ObservableCollection<TransactionModel> Entries
        {
            get; private set;
        }

        public decimal Total
        { get { return CalculateTotal(); } }

        private decimal CalculateTotal()
        {
            decimal debs = 0;
            decimal creds = 0;
            foreach(var t in Entries)
            {
                if (t.TransactionType == TransactionTypes.Credit)
                    creds += t.TransactionAmt;
                else
                    debs += t.TransactionAmt;
            }
            return creds - debs;
        }
    }
}
