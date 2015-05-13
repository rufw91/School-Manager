using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helper.Models
{
    public class StudentSubjectSelectionEntryModel:SubjectBaseModel
    {
        private bool isSelected;
        public StudentSubjectSelectionEntryModel()
        {
            IsSelected = true;
        }

        public StudentSubjectSelectionEntryModel(SubjectBaseModel subject)
        {
            IsSelected = true;
            this.SubjectID = subject.SubjectID;
            this.NameOfSubject = subject.NameOfSubject;
        }

        public bool IsSelected
        {
            get { return this.isSelected; }

            set
            {
                if (value != this.isSelected)
                {
                    this.isSelected = value;
                    NotifyPropertyChanged("IsSelected");
                }
            }
        }

        public override void Reset()
        {
            base.Reset();
            IsSelected = true;
        }
    }
}
