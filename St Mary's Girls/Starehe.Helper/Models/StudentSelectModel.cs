using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helper.Models
{
    public class StudentSelectModel : StudentBaseModel
    {
        private bool isActive;
        public StudentSelectModel()
        {
            this.IsActive = true;
            CheckErrors();
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
        public override bool CheckErrors()
        {
            ClearAllErrors();
            if (StudentID == 0)
            {
                List<string> errors = new List<string>();
                errors.Add("Student does not exist.");
                SetErrors("StudentID", errors);
                NameOfStudent = "";
                this.IsActive = true;
            }
            else
            {
                StudentModel student = DataAccess.GetStudent(StudentID);
                if (student.StudentID == 0)
                {
                    List<string> errors = new List<string>();
                    errors.Add("Student does not exist.");
                    SetErrors("StudentID", errors);
                    NameOfStudent = "";
                }
                else
                {
                    ClearErrors("StudentID");
                    this.StudentID = student.StudentID;
                    this.NameOfStudent = student.NameOfStudent;
                    this.IsActive = student.IsActive;
                    if (!this.isActive)
                    {
                        List<string> errors = new List<string>();
                        errors.Add("Student is not active.");
                        SetErrors("StudentID", errors);
                    }

                }
            }

            NotifyPropertyChanged("HasErrors");
            return HasErrors;
        }
    }
}
