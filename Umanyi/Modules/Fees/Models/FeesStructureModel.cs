using System;
using System.Collections.ObjectModel;
using UmanyiSMS.Lib;

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
            Term = Institution.Controller.DataController.GetTerm(DateTime.Now);
            Year = DateTime.Now.Year;
            Entries = new ObservableCollection<FeesStructureEntryModel>();
        }

        public override void Reset()
        {
            NameOfCombinedClass = "";
            FeesStructureID = 0;
            ClassID = 0;
            IsActive = true;
            Term = Institution.Controller.DataController.GetTerm(DateTime.Now);
            Year = DateTime.Now.Year;
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

        public int Term
        { get; set; }

        public int Year
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
