using System;
using System.Collections.ObjectModel;

namespace Helper.Models
{
    public class FeesStructureModel: ModelBase
    {
        ObservableCollection<FeesStructureEntryModel> entries;
        public FeesStructureModel()
        {
            FeesStructureID = 0;
            NameOfCombinedClass = "";
            ClassID = 0;
            IsActive = true;
            StartDate = DateTime.Now;
            EndDate = null;
            Entries = new ObservableCollection<FeesStructureEntryModel>();
        }

        public override void Reset()
        {
            NameOfCombinedClass = "";
            FeesStructureID = 0;
            ClassID = 0;
            IsActive = true;
            StartDate = DateTime.Now;
            EndDate = null;
            Entries.Clear();
        }
         
        public int FeesStructureID
        { get; set; }

        public string NameOfCombinedClass
        { get; set; }

        public int ClassID
        { get; set; }

        public bool IsActive
        { get; set; }

        public DateTime StartDate
        { get; set; }

        public DateTime? EndDate
        { get; set; }

        public ObservableCollection<FeesStructureEntryModel> Entries
        {
            get { return entries; }
            set
            {
                if (value != this.entries)
                {
                    this.entries = value;
                    NotifyPropertyChanged("Entries");
                }
            }
        }
        
    }

    public class FeesStructureEntryModel : ModelBase
    {
        string name;
        decimal amount;
        public FeesStructureEntryModel()
        {
            Name = "";
            Amount = 0;
        }
        public string Name
        {
            get { return name; }
            set
            {
                if (value != this.name)
                {
                    this.name = value;
                    NotifyPropertyChanged("Name");
                }
            }
        }
        public decimal Amount
        {
            get { return amount; }

            set
            {
                if (value != this.amount)
                {
                    this.amount = value;
                    NotifyPropertyChanged("Amount");
                }
            }
        }

        public override void Reset()
        {
            Name = "";
            Amount = 0;
        }
    }
}
