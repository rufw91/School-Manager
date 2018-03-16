using System;
using System.Collections.Generic;
using UmanyiSMS.Modules.Students.Controller;

namespace UmanyiSMS.Modules.Students.Models
{
    public class LeavingCertificateModel : StudentBaseModel
    {
        private string remarks;
        private string classLeft;
        private string classEntered;
        private string nationality;
        private DateTime dateOfLeaving;
        private DateTime dateOfAdmission;
        private DateTime dateOfBirth;
        private DateTime dateOfIssue;
        public LeavingCertificateModel()
        {
            DateOfIssue = DateTime.Now;
            DateOfBirth = DateTime.Now;
            DateOfAdmission = DateTime.Now;
            DateOfLeaving = DateTime.Now;
            Nationality = "KENYAN";
            ClassEntered = "";
            ClassLeft = "";
            Remarks = "";
        }

        public DateTime DateOfIssue
        {
            get { return dateOfIssue; }

            set
            {
                if (value != dateOfIssue)
                {
                    dateOfIssue = value;
                    NotifyPropertyChanged("DateOfIssue");
                }
            }
        }

        public DateTime DateOfBirth
        {
            get { return dateOfBirth; }

            set
            {
                if (value != dateOfBirth)
                {
                    dateOfBirth = value;
                    NotifyPropertyChanged("DateOfBirth");
                }
            }
        }

        public DateTime DateOfAdmission
        {
            get { return dateOfAdmission; }

            set
            {
                if (value != dateOfAdmission)
                {
                    dateOfAdmission = value;
                    NotifyPropertyChanged("DateOfAdmission");
                }
            }
        }

        public DateTime DateOfLeaving
        {
            get { return dateOfLeaving; }

            set
            {
                if (value != dateOfLeaving)
                {
                    dateOfLeaving = value;
                    NotifyPropertyChanged("DateOfLeaving");
                }
            }
        }
        
        public string Nationality
        {
            get { return nationality; }

            set
            {
                if (value != nationality)
                {
                    nationality = value;
                    NotifyPropertyChanged("Nationality");
                }
            }
        }

        public string ClassEntered
        {
            get { return classEntered; }

            set
            {
                if (value != classEntered)
                {
                    classEntered = value;
                    NotifyPropertyChanged("ClassEntered");
                }
            }
        }

        public string ClassLeft
        {
            get { return classLeft; }

            set
            {
                if (value != classLeft)
                {
                    classLeft = value;
                    NotifyPropertyChanged("ClassLeft");
                }
            }
        }

        public string Remarks
        {
            get { return remarks; }

            set
            {
                if (value != remarks)
                {
                    remarks = value;
                    NotifyPropertyChanged("Remarks");
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
                    StudentModel student = DataController.GetStudent(StudentID);
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
            DateOfIssue = DateTime.Now;
            DateOfBirth = DateTime.Now;
            DateOfAdmission = DateTime.Now;
            DateOfLeaving = DateTime.Now;
            Nationality = "";
            ClassEntered = "";
            ClassLeft = "";
            Remarks = "";
        }
    }
}
