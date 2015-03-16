using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helper.Models
{
    public class ModifyStudentModel:StudentModel
    {
        public ModifyStudentModel()
        {
            CheckErrors();
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
                    Clean();
                }
                else
                {
                    StudentModel student = DataAccess.GetStudent(StudentID);
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
                        this.Address = student.Address;
                        this.NameOfGuardian = student.NameOfGuardian;
                        this.GuardianPhoneNo = student.GuardianPhoneNo;
                        this.City = student.City;
                        this.StudentID = student.StudentID;
                        this.FirstName = student.FirstName;
                        this.MiddleName = student.MiddleName;
                        this.LastName = student.LastName;
                        this.SPhoto = student.SPhoto;
                        this.Email = student.Email;
                        this.PostalCode = student.PostalCode;
                        this.PrevInstitution = student.PrevInstitution;
                        this.BedNo = student.BedNo;
                        this.ClassID = student.ClassID;
                        this.DateOfAdmission = student.DateOfAdmission;
                        this.DateOfBirth = student.DateOfBirth;
                        this.DormitoryID = student.DormitoryID;
                        this.PrevBalance = student.PrevBalance;
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

        protected virtual void Clean()
        {
            FirstName = "";
            MiddleName = "";
            LastName = "";
            DateOfBirth = new DateTime(1900, 1, 1);
            DateOfAdmission = DateTime.Now;
            NameOfGuardian = "";
            GuardianPhoneNo = "";
            Email = "";
            Address = "";
            City = "";
            PostalCode = "";
            SPhoto = null;
            ClassID = 0;
            DormitoryID = 0;
            BedNo = "";
            PrevInstitution = "";
            PrevBalance = 0;
        }
    }
}
