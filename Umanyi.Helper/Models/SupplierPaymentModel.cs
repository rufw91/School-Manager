using System;
using System.ComponentModel;

namespace Helper.Models
{
    public class SupplierPaymentModel : SupplierBaseModel
    {
        int paymentID;
        decimal iAmount;
        DateTime iDatePaid;
        string notes;
        public SupplierPaymentModel()
        {
            SupplierPaymentID = 0;
            Amount = 0;
            DatePaid = DateTime.Now;
            Notes = "";
        }

        public SupplierPaymentModel(int supplierPaymentID, int supplierId, string nameOfSupplier, decimal amount, DateTime datePaid, string notes)
            :base(supplierId,nameOfSupplier)
        {
            this.SupplierPaymentID = supplierPaymentID;
            this.Amount = amount;
            this.DatePaid = datePaid;
            this.Notes = notes;
        }

        public int SupplierPaymentID
        {
            get { return this.paymentID; }

            set
            {
                if (value != this.paymentID)
                {
                    this.paymentID = value;
                    NotifyPropertyChanged("SupplierPaymentID");
                }
            }
        }
        
        public decimal Amount
        {
            get { return this.iAmount; }

            set
            {
                if (value != this.iAmount)
                {
                    this.iAmount = value;
                    NotifyPropertyChanged("Amount");
                }
            }
        }

        public DateTime DatePaid
        {
            get { return this.iDatePaid; }

            set
            {
                if (value != this.iDatePaid)
                {
                    this.iDatePaid = value;
                    NotifyPropertyChanged("DatePaid");
                }
            }
        }

        public string Notes
        {
            get { return this.notes; }

            set
            {
                if (value != this.notes)
                {
                    this.notes = value;
                    NotifyPropertyChanged("Notes");
                }
            }
        }

        public override void Reset()
        {
            base.Reset();
            SupplierPaymentID = 0;
            Amount = 0;
            DatePaid = DateTime.Now;
            Notes = "";
        }

        
    }
}
