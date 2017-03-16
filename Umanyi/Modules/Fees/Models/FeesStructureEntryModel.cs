using System;

namespace UmanyiSMS.Modules.Fees.Models
{
    public class FeesStructureEntryModel : ModelBase
    {
        private string name;

        private decimal amount;

        public string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                if (value != this.name)
                {
                    this.name = value;
                    base.NotifyPropertyChanged("Name");
                }
            }
        }

        public decimal Amount
        {
            get
            {
                return this.amount;
            }
            set
            {
                if (value != this.amount)
                {
                    this.amount = value;
                    base.NotifyPropertyChanged("Amount");
                }
            }
        }

        public FeesStructureEntryModel()
        {
            this.Name = "";
            this.Amount = 0m;
        }

        public override void Reset()
        {
            this.Name = "";
            this.Amount = 0m;
        }
    }
}
