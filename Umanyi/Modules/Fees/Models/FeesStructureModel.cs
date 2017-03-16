using System;
using System.Collections.ObjectModel;

namespace UmanyiSMS.Modules.Fees.Models
{
    public class FeesStructureModel : ModelBase
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

}
