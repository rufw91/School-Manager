using System;

namespace Helper.Models
{
    public class EmployeePaymentModel: ModifyStaffModel
    {
        private decimal amount;
        private int employeePaymentID;
        private DateTime datePaid;
        private string notes;
        public EmployeePaymentModel() 
        {
            Amount = 0;
            DatePaid = DateTime.Now;
            Notes = "";
        }

        public int EmployeePaymentID
        {
            get { return this.employeePaymentID; }

            set
            {
                if (value != this.employeePaymentID)
                {
                    this.employeePaymentID = value;
                    NotifyPropertyChanged("EmployeePaymentID");
                }
            }
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
            base.Reset();
            Amount = 0;
            DatePaid = DateTime.Now;
            Notes = "";
        }
    }
}
