using System.Collections.ObjectModel;

namespace Helper.Models
{
    public class ClassLeavingCertificatesModel : ModelBase
    {
        private ObservableCollection<LeavingCertificateModel> entries;
        public ClassLeavingCertificatesModel()
        {
            entries = new ObservableCollection<LeavingCertificateModel>();
        }

        public ObservableCollection<LeavingCertificateModel> Entries
        {
            get { return entries; }
            set
            {
                if (value != entries)
                {
                    entries = value;
                    NotifyPropertyChanged("Entries");

                }
            }
        }

        public override void Reset()
        {
            entries.Clear();
        }
    }
}

