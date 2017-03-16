using System;

namespace UmanyiSMS.Modules.Fees.Models
{
    public class StudentFeesDefaultModel : StudentBaseModel
    {
        private decimal balance;

        private string guardianPhoneNo;

        public decimal Balance
        {
            get
            {
                return this.balance;
            }
            set
            {
                if (value != this.balance)
                {
                    this.balance = value;
                    base.NotifyPropertyChanged("Balance");
                }
            }
        }

        public string GuardianPhoneNo
        {
            get
            {
                return this.guardianPhoneNo;
            }
            set
            {
                if (value != this.guardianPhoneNo)
                {
                    this.guardianPhoneNo = value;
                    base.NotifyPropertyChanged("GuardianPhoneNo");
                }
            }
        }

        public StudentFeesDefaultModel()
        {
            this.Balance = 0m;
            this.GuardianPhoneNo = "";
        }
    }
}
