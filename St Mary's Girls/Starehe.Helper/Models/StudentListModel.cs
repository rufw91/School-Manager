using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Helper.Models
{
    public class StudentListModel: StudentModel
    {
        private string nameOfDormitory;
        private string nameOfClass;
        private bool isCleared;
        private bool isTransferred;
        private bool isActive;
        private bool isSelected;
        public StudentListModel()
        {
            NameOfClass = "";
            NameOfDormitory = "";
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
        public string NameOfDormitory
        {
            get { return this.nameOfDormitory; }

            set
            {
                if (value != this.nameOfDormitory)
                {
                    this.nameOfDormitory = value;
                    NotifyPropertyChanged("NameOfDormitory");
                }
            }
        }

        public override void Reset()
        {
            base.Reset();
            NameOfClass="";
            NameOfDormitory = "";
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

        public bool IsActive
        {
            get { return this.isActive; }

            set
            {
                if (value != this.isActive)
                {
                    this.isActive = value;
                    NotifyPropertyChanged("IsActive");
                }
            }
        }

        public bool IsTransferred
        {
            get { return this.isTransferred; }

            set
            {
                if (value != this.isTransferred)
                {
                    this.isTransferred = value;
                    NotifyPropertyChanged("IsTransferred");
                }
            }
        }

        public bool IsCleared
        {
            get { return this.isCleared; }

            set
            {
                if (value != this.isCleared)
                {
                    this.isCleared = value;
                    NotifyPropertyChanged("IsCleared");
                }
            }
        }

        public new void CopyFrom(StudentListModel newStudent)
        {
            base.CopyFrom(newStudent);
            this.NameOfDormitory = newStudent.NameOfDormitory;
            this.NameOfClass = newStudent.NameOfClass;
            this.IsSelected = newStudent.IsSelected;
            this.IsActive = newStudent.IsActive;
            this.IsTransferred = newStudent.IsTransferred;
            this.IsCleared = newStudent.IsCleared;
        }
    }
}
