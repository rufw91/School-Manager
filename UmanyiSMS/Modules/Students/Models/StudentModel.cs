using System;
using System.Collections.Generic;
using System.ComponentModel;
using UmanyiSMS.Lib;
using UmanyiSMS.Modules.Students.Controller;

namespace UmanyiSMS.Modules.Students.Models
{
    public class StudentModel : StudentBaseModel
    {
        private string firstName = "";

        private string middleName = "";

        private string lastName = "";

        private DateTime dateOfBirth = new DateTime(1999, 1, 1);

        private DateTime dateOfAdmission = DateTime.Now;

        private string nameOfGuardian = "";

        private string guardianPhoneNo = "";

        private string email = "";

        private string address = "";

        private string city = "";

        private string postalCode = "";

        private byte[] sPhoto = null;

        private decimal prevBalance;

        private Gender gender;

        private int classID = 0;
        
        private string prevInstitution = "";
        
        private int kcpescore;

        private bool isActive;

        public string FirstName
        {
            get
            {
                return this.firstName;
            }
            set
            {
                if (value != this.firstName)
                {
                    this.firstName = value;
                    base.NotifyPropertyChanged("FirstName");
                }
            }
        }

        public string MiddleName
        {
            get
            {
                return this.middleName;
            }
            set
            {
                if (value != this.middleName)
                {
                    this.middleName = value;
                    base.NotifyPropertyChanged("MiddleName");
                }
            }
        }

        public string LastName
        {
            get
            {
                return this.lastName;
            }
            set
            {
                if (value != this.lastName)
                {
                    this.lastName = value;
                    base.NotifyPropertyChanged("LastName");
                }
            }
        }

        public Gender Gender
        {
            get
            {
                return this.gender;
            }
            set
            {
                if (value != this.gender)
                {
                    this.gender = value;
                    base.NotifyPropertyChanged("Gender");
                }
            }
        }

        public DateTime DateOfBirth
        {
            get
            {
                return this.dateOfBirth;
            }
            set
            {
                if (value != this.dateOfBirth)
                {
                    this.dateOfBirth = value;
                    base.NotifyPropertyChanged("DateOfBirth");
                }
            }
        }

        public DateTime DateOfAdmission
        {
            get
            {
                return this.dateOfAdmission;
            }
            set
            {
                if (value != this.dateOfAdmission)
                {
                    this.dateOfAdmission = value;
                    base.NotifyPropertyChanged("DateOfAdmission");
                }
            }
        }

        public string NameOfGuardian
        {
            get
            {
                return this.nameOfGuardian;
            }
            set
            {
                if (value != this.nameOfGuardian)
                {
                    this.nameOfGuardian = value;
                    base.NotifyPropertyChanged("NameOfGuardian");
                }
            }
        }

        public string GuardianPhoneNo
        {
            get
            {
                return this.guardianPhoneNo;
            }
            set
            {
                if (value != this.guardianPhoneNo)
                {
                    this.guardianPhoneNo = value;
                    base.NotifyPropertyChanged("GuardianPhoneNo");
                }
            }
        }

        public string Email
        {
            get
            {
                return this.email;
            }
            set
            {
                if (value != this.email)
                {
                    this.email = value;
                    base.NotifyPropertyChanged("Email");
                }
            }
        }

        public string Address
        {
            get
            {
                return this.address;
            }
            set
            {
                if (value != this.address)
                {
                    this.address = value;
                    base.NotifyPropertyChanged("Address");
                }
            }
        }

        public string City
        {
            get
            {
                return this.city;
            }
            set
            {
                if (value != this.city)
                {
                    this.city = value;
                    base.NotifyPropertyChanged("City");
                }
            }
        }

        public string PostalCode
        {
            get
            {
                return this.postalCode;
            }
            set
            {
                if (value != this.postalCode)
                {
                    this.postalCode = value;
                    base.NotifyPropertyChanged("PostalCode");
                }
            }
        }

        public byte[] SPhoto
        {
            get
            {
                return this.sPhoto;
            }
            set
            {
                if (value != this.sPhoto)
                {
                    this.sPhoto = value;
                    base.NotifyPropertyChanged("SPhoto");
                }
            }
        }

        public int ClassID
        {
            get
            {
                return this.classID;
            }
            set
            {
                if (value != this.classID)
                {
                    this.classID = value;
                    base.NotifyPropertyChanged("ClassID");
                }
            }
        }
                
        public decimal PrevBalance
        {
            get
            {
                return this.prevBalance;
            }
            set
            {
                if (value != this.prevBalance)
                {
                    this.prevBalance = value;
                    base.NotifyPropertyChanged("PrevBalance");
                }
            }
        }

        public string PrevInstitution
        {
            get
            {
                return this.prevInstitution;
            }
            set
            {
                if (value != this.prevInstitution)
                {
                    this.prevInstitution = value;
                    base.NotifyPropertyChanged("PrevInstitution");
                }
            }
        }

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
        
        public int KCPEScore
        {
            get
            {
                return this.kcpescore;
            }
            set
            {
                if (value != this.kcpescore)
                {
                    this.kcpescore = value;
                }
                base.NotifyPropertyChanged("KCPEScore");
            }
        }

        public StudentModel()
        {
            base.PropertyChanged += delegate (object o, PropertyChangedEventArgs pcea)
            {
                if (pcea.PropertyName == "FirstName" || pcea.PropertyName == "MiddleName" || pcea.PropertyName == "LastName")
                {
                    base.NameOfStudent = string.Concat(new string[]
                    {
                        this.FirstName,
                        " ",
                        this.MiddleName,
                        " ",
                        this.lastName
                    });
                }
            };
            this.Email = "test@example.com";
            this.PrevBalance = 0m;
            this.IsActive = true;
            this.SPhoto = new byte[1];
            this.sPhoto[0] = 1;
        }

        public override string ToString()
        {
            return base.NameOfStudent;
        }

        public override bool CheckErrors()
        {
            base.ErrorCheckingStatus = Lib.ErrorCheckingStatus.Incomplete;
            try
            {
                base.ClearAllErrors();
                StudentModel student = DataController.GetStudent(base.StudentID);
                if (student.StudentID > 0)
                {
                    base.SetErrors("StudentID", new List<string>
                    {
                        "Student already exists. (" + student.NameOfStudent.ToUpper() + ")"
                    });
                }                
            }
            catch (Exception ex)
            {
                base.SetErrors("", new List<string>
                {
                    ex.Message
                });
            }
            base.NotifyPropertyChanged("HasErrors");
            base.ErrorCheckingStatus = Lib.ErrorCheckingStatus.Complete;
            return base.HasErrors;
        }

        public override void Reset()
        {
            base.Reset();
            this.FirstName = "";
            this.MiddleName = "";
            this.LastName = "";
            this.DateOfBirth = new DateTime(1999, 1, 1);
            this.DateOfAdmission = DateTime.Now;
            this.NameOfGuardian = "";
            this.GuardianPhoneNo = "";
            this.Address = "";
            this.City = "";
            this.PostalCode = "";
            this.IsActive = true;
            this.SPhoto = new byte[1];
            this.sPhoto[0] = 1;
            this.ClassID = 0;
            this.PrevInstitution = "";
            this.Gender = Gender.Male;
            this.KCPEScore = 0;
            this.PrevBalance = 0m;
        }

        public void CopyFrom(StudentModel student)
        {
            base.CopyFrom(student);
            this.Address = student.Address;
            this.NameOfGuardian = student.NameOfGuardian;
            this.GuardianPhoneNo = student.GuardianPhoneNo;
            this.City = student.City;
            base.StudentID = student.StudentID;
            this.FirstName = student.FirstName;
            this.MiddleName = student.MiddleName;
            this.LastName = student.LastName;
            this.SPhoto = student.SPhoto;
            this.Email = student.Email;
            this.PostalCode = student.PostalCode;
            this.PrevInstitution = student.PrevInstitution;
            this.ClassID = student.ClassID;
            this.DateOfAdmission = student.DateOfAdmission;
            this.DateOfBirth = student.DateOfBirth;
            this.PrevBalance = student.PrevBalance;
            this.IsActive = student.IsActive;
            this.Gender = student.Gender;
            this.KCPEScore = student.KCPEScore;
        }
    }
}
