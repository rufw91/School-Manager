using System;
using System.Collections.ObjectModel;
using UmanyiSMS.Lib;

namespace UmanyiSMS.Modules.Exams.Models
{
    public class ClassReportForm2Model : ModelBase
    {
        private ObservableCollection<ReportForm2Model> entries;

        public ObservableCollection<ReportForm2Model> Entries
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

        public ClassReportForm2Model()
        {
            this.entries = new ObservableCollection<ReportForm2Model>();
        }

        public override void Reset()
        {
            this.entries.Clear();
        }
    }
}
