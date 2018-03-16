using System;
using UmanyiSMS.Lib;

namespace UmanyiSMS.Modules.Purchases.Models
{
    public class TransactionModel : ModelBase
    {
        TransactionTypes tranType;
        string tranID;
        DateTime tranDatetime;
        decimal tranAmt;
        public TransactionModel()
        {
            TransactionType = TransactionTypes.All;
            TransactionID = "";
            TransactionDateTime = DateTime.Now;
            TransactionAmt = 0;
        }
        public TransactionModel(TransactionTypes transactionType, string transactionID, DateTime transactionDateTime,
            decimal transactionAmt)
        {
            TransactionType = transactionType;
            TransactionID = transactionID;
            TransactionDateTime = transactionDateTime;
            TransactionAmt = transactionAmt;
        }

        public TransactionModel(TransactionModel tm)
        {
            TransactionType = tm.TransactionType;
            TransactionID = tm.TransactionID;
            TransactionDateTime = tm.TransactionDateTime;
            TransactionAmt = tm.TransactionAmt;
        }
        public TransactionTypes TransactionType
        {
            get { return tranType; }
            set
            {
                if (value != this.tranType)
                {
                    this.tranType = value;
                    NotifyPropertyChanged("TransactionType");
                }
            }
        }

        public string TransactionID
        {
            get { return tranID; }
            set
            {
                if (value != this.tranID)
                {
                    this.tranID = value;
                    NotifyPropertyChanged("TransactionID");
                }
            }
        }

        public DateTime TransactionDateTime
        {
            get { return tranDatetime; }
            set
            {
                if (value != this.tranDatetime)
                {
                    this.tranDatetime = value;
                    NotifyPropertyChanged("TransactionDateTime");
                }
            }
        }

        public decimal TransactionAmt
        {
            get { return tranAmt; }
            set
            {
                if (value != this.tranAmt)
                {
                    this.tranAmt = value;
                    NotifyPropertyChanged("TransactionAmt");
                }
            }
        }

        public override void Reset()
        {
            TransactionType = TransactionTypes.All;
            TransactionID = "";
            TransactionDateTime = DateTime.Now;
            TransactionAmt = 0;
        }
    }
}
