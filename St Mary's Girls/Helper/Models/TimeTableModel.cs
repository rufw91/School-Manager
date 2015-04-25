using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Helper.Models
{
    public class TimeTableModel:ObservableCollection<ClassLessons>
    {
        private TimeTableSettingsModel settings;
        public TimeTableModel()
        {
            settings = new TimeTableSettingsModel();
        }

        public TimeTableSettingsModel Settings
        {
            get { return settings; }
            set
            {
                if (value != settings)
                {
                    settings = value;
                    NotifyPropertyChanged("Settings");
                }
            }
        }

        protected override void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
        }
        private void NotifyPropertyChanged(string propertyName)
        {
            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }
    }
}
