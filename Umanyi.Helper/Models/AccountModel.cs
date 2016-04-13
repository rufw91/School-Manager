using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helper.Models
{
    public class AccountModel : ModelBase
    {
        private decimal amount;
        private string name;

        public AccountModel()
        {
            Name = "";
            Amount = 0m;
            SubAccounts = new ObservableCollection<AccountModel>();
        }

        public decimal Amount {
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

        public ObservableCollection<AccountModel> SubAccounts
        { get; private set; }

        public override void Reset()
        {
            
        }
    }
}
