using Helper.Presentation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helper.Models
{
    public sealed class AccountModel : ObservableCollection<IAccount>, IAccount
    {
        private string name;
        private int accountID;
        private decimal balance;
        private Helper.AccountType accountType;

        public AccountModel()
        {
            AccountID = 0;
            Name = "";
            Balance = 0;
        }

        public AccountModel(int accountID,string name )
        {
            AccountID = accountID;
            Name = name;
            Balance = 0;
        }

        public int AccountID
        {
            get { return this.accountID; }

            set
            {
                if (value != this.accountID)
                {
                    this.accountID = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("AccountID"));
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
                    OnPropertyChanged(new PropertyChangedEventArgs("Name"));
                }
            }
        }

        public AccountType AccountType
        {
            get { return this.accountType; }

            set
            {
                if (value != this.accountType)
                {
                    this.accountType = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("AccountType"));
                }
            }
        }

        public decimal Balance
        {
            get { return this.balance; }

            set
            {
                if (value != this.balance)
                {
                    this.balance = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("Balance"));
                }
            }
        }

        public void Reset()
        {
            Name = "";
            AccountID = 0;
            this.Clear();
        }

        public IAccount this[string accountName]
        {
            get
            {
                if (!this.Any(o => o.Name.ToUpper().Equals(accountName.ToUpper())))
                    throw new IndexOutOfRangeException(string.Format("The account {0} cannot be found.", accountName));
                return this.First(o => o.Name.ToUpper().Equals(accountName.ToUpper()));
            }
        }

        public void CopyFrom(IAccount source)
        {
            this.AccountID = source.AccountID;
            this.Name = source.Name;
            this.Balance = source.Balance;
            this.AccountType = source.AccountType;
        }

        public override string ToString()
        {
            return name;
        }

       
    }
}
