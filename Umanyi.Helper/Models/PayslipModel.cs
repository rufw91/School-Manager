using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helper.Models
{
    public class PayslipModel:StaffSelectModel
    {
        private ObservableCollection<FeesStructureEntryModel> entries;
        
        public PayslipModel()
        {
            Entries = new ObservableCollection<FeesStructureEntryModel>();
            DatePaid = DateTime.Now;
        }

        public ObservableCollection<FeesStructureEntryModel> Entries
        { get { return entries; }
            set
            {
                if (value != entries)
                {
                    entries = value;
                    NotifyPropertyChanged("Entries");
                }
            }
        }

        public void RefreshTotal()
        {
            decimal tot = 0;
            foreach (var e in entries)
                tot += e.Amount;
            tot = AmountPaid - tot;
            if (!entries.Any(o => o.Name == "TOTAL"))
                Entries.Add(new FeesStructureEntryModel() { Name = "TOTAL", Amount = tot });
            else
                entries.First(o => o.Name == "TOTAL").Amount = tot;
        }

        public decimal AmountPaid { get; set; }

        public DateTime DatePaid
        { get; set; }

        public string Designation { get; set; }

        public int PayslipID { get; set; }
    }
}
