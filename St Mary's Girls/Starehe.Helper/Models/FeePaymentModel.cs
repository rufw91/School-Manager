using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Helper.Models
{
    public class FeePaymentModel : StudentBaseModel
    {
        int feePaymentID;
        decimal amtPaid;
        DateTime datePaid;
        public FeePaymentModel()
            : base()
        {
            DatePaid = DateTime.Now;
            FeePaymentID = 0;
            AmountPaid = 0;
        }

        public DateTime DatePaid
        {
            get { return this.datePaid; }

            set
            {
                if (value != this.datePaid)
                {
                    this.datePaid = value;
                    NotifyPropertyChanged("DatePaid");
                }
            }
        }
        public int FeePaymentID
        {
            get { return this.feePaymentID; }

            set
            {
                if (value != this.feePaymentID)
                {
                    this.feePaymentID = value;
                    NotifyPropertyChanged("FeePaymentID");
                }
            }
        }
        public decimal AmountPaid
        {
            get { return this.amtPaid; }

            set
            {
                if (value != this.amtPaid)
                {
                    this.amtPaid = value;
                    NotifyPropertyChanged("AmountPaid");
                }
            }
        }
        
        public override void Reset()
        {
            base.Reset();
            DatePaid = DateTime.Now;
            AmountPaid = 0;
            FeePaymentID = 0;
        }

    }
}
