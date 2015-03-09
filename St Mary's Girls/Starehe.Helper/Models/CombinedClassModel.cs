using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helper.Models
{
    public class CombinedClassModel:ModelBase
    {
        string description;
        ObservableCollection<ClassModel> entries;
        public CombinedClassModel()
        {
            Description = "";
            Entries = new ObservableCollection<ClassModel>();
        }

        public string Description
        {
            get { return description; }
            set
            {
                if (description != value)
                {
                    description = value;
                    NotifyPropertyChanged("Description");
                }
            }
        }

        public ObservableCollection<ClassModel> Entries
        {
            get { return entries; }
            set
            {
                if (entries != value)
                {
                    entries = value;
                    NotifyPropertyChanged("Entries");
                }
            }
        }

        public override void Reset()
        {
            entries.Clear();
            Description = "";
        }
    }
}
