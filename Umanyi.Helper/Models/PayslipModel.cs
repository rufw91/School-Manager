using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace Helper.Models
{
    public class PayslipModel : StaffSelectModel
    {
        private ObservableCollection<FeesStructureEntryModel> entries;

        public ObservableCollection<FeesStructureEntryModel> Entries
        {
            get
            {
                return this.entries;
            }
            set
            {
                if (value != this.entries)
                {
                    this.entries = value;
                    base.NotifyPropertyChanged("Entries");
                }
            }
        }

        public decimal AmountPaid
        {
            get;
            set;
        }

        public DateTime DatePaid
        {
            get;
            set;
        }

        public string Designation
        {
            get;
            set;
        }

        public int PayslipID
        {
            get;
            set;
        }

        public PayslipModel()
        {
            this.Entries = new ObservableCollection<FeesStructureEntryModel>();
            this.DatePaid = DateTime.Now;
            this.Designation = "";
        }

        public void RefreshTotal()
        {
            decimal num = 0m;
            foreach (FeesStructureEntryModel current in this.entries)
            {
                num += current.Amount;
            }
            num = this.AmountPaid - num;
            if (!this.entries.Any((FeesStructureEntryModel o) => o.Name == "TOTAL"))
            {
                this.Entries.Add(new FeesStructureEntryModel
                {
                    Name = "TOTAL",
                    Amount = num
                });
            }
            else
            {
                this.entries.First((FeesStructureEntryModel o) => o.Name == "TOTAL").Amount = num;
            }
        }
    }
}
