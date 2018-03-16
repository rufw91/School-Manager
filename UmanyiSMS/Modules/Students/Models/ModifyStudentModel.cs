using System;
using System.Collections.Generic;
using UmanyiSMS.Modules.Students.Controller;

namespace UmanyiSMS.Modules.Students.Models
{
    public class ModifyStudentModel:StudentModel
    {
        public ModifyStudentModel()
        {
            CheckErrors();
        }
        public override bool CheckErrors()
        {
                ClearAllErrors();
                if (StudentID == 0)
                {
                    List<string> errors = new List<string>();
                    errors.Add("Student does not exist.");
                    SetErrors("StudentID", errors);
                    Clean();
                }
                else
                {
                    StudentModel student = DataController.GetStudent(StudentID);
                    if (student.StudentID == 0)
                    {
                        List<string> errors = new List<string>();
                        errors.Add("Student does not exist.");
                        SetErrors("StudentID", errors);
                        Clean();
                    }
                    else
                    {
                        ClearErrors("StudentID");
                        this.CopyFrom(student);
                    }
                }
            
            
            NotifyPropertyChanged("HasErrors");
            return HasErrors;
        }

        protected virtual void Clean()
        {
            FirstName = "";
            MiddleName = "";
            LastName = "";
            DateOfBirth = new DateTime(2000, 1, 1);
            DateOfAdmission = DateTime.Now;
            NameOfGuardian = "";
            GuardianPhoneNo = "";
            Email = "";
            Address = "";
            City = "";
            PostalCode = "";
            SPhoto = null;
            ClassID = 0;
            PrevInstitution = "";
            PrevBalance = 0;
            IsActive = true;
        }
    }
}
