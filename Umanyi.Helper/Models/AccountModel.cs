﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helper.Models
{
    public class AccountModel : ObservableCollection<AccountModel>, INotifyPropertyChanged
    {
        private string name;
        private int accountID;

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

        public void Reset()
        {
            Name = "";
            AccountID = 0;
            this.Clear();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private decimal balance;
        

        protected void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public override string ToString()
        {
            return name;
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
