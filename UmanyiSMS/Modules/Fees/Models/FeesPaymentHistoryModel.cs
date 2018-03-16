using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UmanyiSMS.Lib;

namespace UmanyiSMS.Modules.Fees.Models
{
    public class FeesPaymentHistoryModel : ModelBase
    {
        private decimal amount;
        private string paymentMode;
        public FeesPaymentHistoryModel()
        {
            Amount = 0;
            PaymentMode = "";
        }
        
        public string PaymentMode
        {
            get { return this.paymentMode; }

            set
            {
                if (value != this.paymentMode)
                {
                    this.paymentMode = value;
                    NotifyPropertyChanged("PaymentMode");
                }
            }
        }

        public decimal Amount
        {
            get { return this.amount; }

            set
            {
                if (value != this.amount)
                {
                    this.amount = value;
                    NotifyPropertyChanged("Amount");
                }
            }
        }

        public override void Reset()
        {
            Amount = 0;
            PaymentMode = "";
        }
    }
}
            