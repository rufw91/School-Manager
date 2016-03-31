using System;
using System.Collections.Generic;

namespace Helper.Models
{
    public class StudentSelectModel : StudentBaseModel
    {
        private bool isActive;

        public bool IsActive
        {
            get
            {
                return this.isActive;
            }
            set
            {
                if (value != this.isActive)
                {
                    this.isActive = value;
                    base.NotifyPropertyChanged("IsActive");
                }
            }
        }

        public StudentSelectModel()
        {
            this.IsActive = true;
            this.CheckErrors();
        }

        public override bool CheckErrors()
        {
            base.ClearAllErrors();
            if (base.StudentID == 0)
            {
                base.SetErrors("StudentID", new List<string>
                {
                    "Student does not exist."
                });
                base.NameOfStudent = "";
                this.IsActive = true;
            }
            else
            {
                StudentModel student = DataAccess.GetStudent(base.StudentID);
                if (student.StudentID == 0)
                {
                    base.SetErrors("StudentID", new List<string>
                    {
                        "Student does not exist."
                    });
                    base.NameOfStudent = "";
                }
                else
                {
                    base.ClearErrors("StudentID");
                    base.StudentID = student.StudentID;
                    base.NameOfStudent = student.NameOfStudent;
                    this.IsActive = student.IsActive;
                    if (!this.isActive)
                    {
                        base.SetErrors("StudentID", new List<string>
                        {
                            "Student is not active."
                        });
                    }
                }
            }
            base.NotifyPropertyChanged("HasErrors");
            return base.HasErrors;
        }
    }
}
