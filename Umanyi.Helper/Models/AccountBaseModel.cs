using Helper.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helper.Models
{
    public class AccountBaseModel:ModelBase, IAccountBase
    {
        private string name;
        private int accountID;
        private decimal balance;
        private Helper.AccountType accountType;

        public AccountBaseModel()
        {
            AccountID = 0;
            Name = "";
            Balance = 0;
        }

        public AccountBaseModel(int accountID, string name)
        {
            AccountID = accountID;
            Name = name;
            Balance = 0;
        }

        public AccountBaseModel(int accountID, string name, decimal balance, AccountType type)
        {
            AccountID = accountID;
            Name = name;
            Balance = balance;
            AccountType = type;
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
            Balance = 0;
        }


        public AccountType AccountType
        {
            get { return this.accountType; }

            set
            {
                if (value != this.accountType)
                {
                    this.accountType = value;
                    NotifyPropertyChanged("AccountType");
                }
            }
        }



        public new IAccount this[string accountName]
        {
            get { return null; }
        }
    }
}
