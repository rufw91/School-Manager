using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helper.Models
{
    public sealed class AccountModel : ObservableCollection<AccountModel>
    {
        private string name;
        private int accountID;
        private decimal balance;

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

        public void Reset()
        {
            Name = "";
            AccountID = 0;
            this.Clear();
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

        public void CopyFrom(AccountModel source)
        {
            this.AccountID = source.AccountID;
            this.Name = source.Name;
            this.Balance = source.Balance;

        }

        public override string ToString()
        {
            return name;
        }

       
    }
}
