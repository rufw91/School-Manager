using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helper.Models
{
    public class StudentFeesDefaultModel: StudentModel
    {
        decimal balance;
        public StudentFeesDefaultModel()
        {
            Balance = 0;
        }
        public StudentFeesDefaultModel(StudentModel student, decimal balance):base()
        {
            CopyFrom(student);
            Balance = balance;
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
    }
}
