using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helper.Models
{
    public class AccountBaseModel:ModelBase
    {
        private string name;
        private int accountID;

        public AccountBaseModel()
        {
            AccountID = 0;
            Name = "";
        }

        public AccountBaseModel(int accountID, string name)
        {
            AccountID = accountID;
            Name = name;
        }

        public int AccountID
        {
            get { return this.accountID; }

            set
            {
                if (value != this.accountID)
                {
                    this.accountID = value;
                    NotifyPropertyChanged("AccountID");
                }
            }
        }

        public string Name
        {
            get { return this.name; }

            set
            {
                if (value != this.name)
                {
                    this.name = value;
                    NotifyPropertyChanged("Name");
                }
            }
        }

        public override void Reset()
        {
            Name = "";
            AccountID = 0;
        }
    }
}
