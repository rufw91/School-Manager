using System;

namespace UmanyiSMS.Modules.Fees.Models
{
    public class FeePaymentModel : StudentSelectModel
    {
        private int feePaymentID;

        private decimal amtPaid;

        private DateTime datePaid;

        private string paymentMethod;

        public DateTime DatePaid
        {
            get
            {
                return this.datePaid;
            }
            set
            {
                if (value != this.datePaid)
                {
                    this.datePaid = value;
                    base.NotifyPropertyChanged("DatePaid");
                }
            }
        }

        public int FeePaymentID
        {
            get
            {
                return this.feePaymentID;
            }
            set
            {
                if (value != this.feePaymentID)
                {
                    this.feePaymentID = value;
                    base.NotifyPropertyChanged("FeePaymentID");
                }
            }
        }

        public decimal AmountPaid
        {
            get
            {
                return this.amtPaid;
            }
            set
            {
                if (value != this.amtPaid)
                {
                    this.amtPaid = value;
                    base.NotifyPropertyChanged("AmountPaid");
                }
            }
        }

        public string PaymentMethod
        {
            get
            {
                return this.paymentMethod;
            }
            set
            {
                if (value != this.paymentMethod)
                {
                    this.paymentMethod = value;
                    base.NotifyPropertyChanged("PaymentMethod");
                }
            }
        }

        public FeePaymentModel()
        {
            this.DatePaid = DateTime.Now;
            this.FeePaymentID = 0;
            this.AmountPaid = 0m;
            this.PaymentMethod = "";
        }

        public override void Reset()
        {
            base.Reset();
            this.DatePaid = DateTime.Now;
            this.AmountPaid = 0m;
            this.FeePaymentID = 0;
            this.PaymentMethod = "";
        }
    }
}
