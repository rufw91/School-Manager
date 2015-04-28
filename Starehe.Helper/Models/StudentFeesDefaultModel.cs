using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helper.Models
{
    public class StudentFeesDefaultModel: StudentBaseModel
    {
        decimal balance;
        private string guardianPhoneNo;
        public StudentFeesDefaultModel()
        {
            Balance = 0;
            GuardianPhoneNo = "";
        }

        public decimal Balance
        {
            get { return this.balance; }

            set
            {
                if (value != this.balance)
                {
                    this.balance = value;
                    NotifyPropertyChanged("Balance");
                }
            }
        }

        public string GuardianPhoneNo
        {
            get { return this.guardianPhoneNo; }

            set
            {
                if (value != this.guardianPhoneNo)
                {
                    this.guardianPhoneNo = value;
                    NotifyPropertyChanged("GuardianPhoneNo");
                }
            }
        }
    }
}
