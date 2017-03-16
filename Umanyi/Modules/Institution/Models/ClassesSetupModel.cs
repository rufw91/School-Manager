using System;
using System.Collections.ObjectModel;
using UmanyiSMS.Lib;

namespace UmanyiSMS.Modules.Institution.Models
{
    public class ClassesSetupModel : ModelBase
    {
        private ObservableCollection<ClassesSetupEntryModel> entries;

        public int ClassSetupID
        {
            get;
            set;
        }

        public bool IsActive
        {
            get;
            set;
        }

        public DateTime StartDate
        {
            get;
            set;
        }

        public DateTime? EndDate
        {
            get;
            set;
        }

        public ObservableCollection<ClassesSetupEntryModel> Entries
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

        public ClassesSetupModel()
        {
            this.ClassSetupID = 0;
            this.IsActive = true;
            this.StartDate = DateTime.Now;
            this.EndDate = null;
            this.Entries = new ObservableCollection<ClassesSetupEntryModel>();
        }

        public override void Reset()
        {
            this.ClassSetupID = 0;
            this.IsActive = true;
            this.StartDate = DateTime.Now;
            this.EndDate = null;
            this.Entries = new ObservableCollection<ClassesSetupEntryModel>();
        }
    }
}
