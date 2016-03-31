using System;
using System.Collections.ObjectModel;

namespace Helper.Models
{
    public class CombinedClassModel : ModelBase
    {
        private string description;

        private ObservableCollection<ClassModel> entries;

        public string Description
        {
            get
            {
                return this.description;
            }
            set
            {
                if (this.description != value)
                {
                    this.description = value;
                    base.NotifyPropertyChanged("Description");
                }
            }
        }

        public ObservableCollection<ClassModel> Entries
        {
            get
            {
                return this.entries;
            }
            set
            {
                if (this.entries != value)
                {
                    this.entries = value;
                    base.NotifyPropertyChanged("Entries");
                }
            }
        }

        public CombinedClassModel()
        {
            this.Description = "";
            this.Entries = new ObservableCollection<ClassModel>();
        }

        public override void Reset()
        {
            this.entries.Clear();
            this.Description = "";
        }
    }
}
