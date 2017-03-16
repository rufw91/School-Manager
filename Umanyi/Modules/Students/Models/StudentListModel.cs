using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UmanyiSMS.Modules.Students.Models
{
    public class StudentListModel: StudentModel
    {
        private string nameOfClass;
        private bool isActive;
        private bool isSelected;
        public StudentListModel()
        {
            NameOfClass = "";
            IsActive = true;
            
        }
        
        public string NameOfClass
        {
            get { return this.nameOfClass; }

            set
            {
                if (value != this.nameOfClass)
                {
                    this.nameOfClass = value;
                    NotifyPropertyChanged("NameOfClass");
                }
            }
        }
        
        public override void Reset()
        {
            base.Reset();
            NameOfClass="";
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
        
        public void CopyFrom(StudentListModel newStudent)
        {
            base.CopyFrom(newStudent);
            this.NameOfClass = newStudent.NameOfClass;
            this.IsSelected = newStudent.IsSelected;
            this.IsActive = newStudent.IsActive;
        }
    }
}
