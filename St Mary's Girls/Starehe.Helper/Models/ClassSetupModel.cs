using System;
using System.Collections.ObjectModel;

namespace Helper.Models
{
    public class ClassesSetupModel: ModelBase
    {
        ObservableCollection<ClassesSetupEntryModel> entries;
        public ClassesSetupModel()
        {
            ClassSetupID = 0;
            IsActive = true;
            StartDate = DateTime.Now;
            EndDate = null;
            Entries = new ObservableCollection<ClassesSetupEntryModel>();
        }

        public override void Reset()
        {
            ClassSetupID = 0;
            IsActive = true;
            StartDate = DateTime.Now;
            EndDate = null;
            Entries = new ObservableCollection<ClassesSetupEntryModel>();
        }

        public int ClassSetupID
        { get; set; }

        public bool IsActive
        { get; set; }

        public DateTime StartDate
        { get; set; }

        public DateTime? EndDate
        { get; set; }

        public ObservableCollection<ClassesSetupEntryModel> Entries
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

    public class ClassesSetupEntryModel : ClassModel
    {
        public ClassesSetupEntryModel()
        {
            ClassSetupID = 0;
        }

        public ClassesSetupEntryModel(ClassModel classModel)
        {
            ClassSetupID = 0;
            this.ClassID = classModel.ClassID;
            this.NameOfClass = classModel.NameOfClass;            
        }
        public int ClassSetupID
        {
            get;
            set;
        }

        public override void Reset()
        {
            ClassSetupID = 0;
        }
    }
}
