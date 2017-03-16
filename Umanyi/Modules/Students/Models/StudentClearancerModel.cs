using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UmanyiSMS.Modules.Students.Models
{
    public class StudentClearanceModel: StudentBaseModel
    {
        private DateTime dateCleared;
        public StudentClearanceModel()
        {
            DateCleared = DateTime.Now;
        }

        public DateTime DateCleared
        {
            get { return this.dateCleared; }

            set
            {
                if (value != this.dateCleared)
                {
                    this.dateCleared = value;
                    NotifyPropertyChanged("DateCleared");
                }
            }
        }

        public override bool CheckErrors()
        {            
            try
            {
                ClearAllErrors();
                if (StudentID == 0)
                {
                    List<string> errors = new List<string>();
                    errors.Add("Student does not exist.");
                    SetErrors("StudentID", errors);
                }
                else
                {
                    StudentModel student = DataAccess.GetStudent(StudentID);
                    if (student.StudentID == 0)
                    {
                        List<string> errors = new List<string>();
                        errors.Add("Student does not exist.");
                        SetErrors("StudentID", errors);
                    }
                    else
                    {
                        ClearErrors("StudentID");
                        this.NameOfStudent = student.NameOfStudent;
                    }
                }
            }
            catch (Exception e)
            {
                List<string> errors = new List<string>();
                errors.Add(e.Message);
                SetErrors("", errors);
            }
            NotifyPropertyChanged("HasErrors");
            return HasErrors;    
        }
        
        public override void Reset()
        {
            base.Reset();
            DateCleared = DateTime.Now;
        }
    }
}
