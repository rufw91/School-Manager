using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows.Media.Imaging;

namespace Helper.Models
{
    public class StudentModel : StudentBaseModel
    {
        private string firstName = "";
        private string middleName = "";
        private string lastName = "";
        private DateTime dateOfBirth = new DateTime(1980,1,1);
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
        private int dormitoryID = 0;
        private string bedNo = "";
        private string prevInstitution = "";
        private bool isBoarder;
        private int kcpescore;
        private bool isActive;

        public StudentModel()
        {
            PropertyChanged += delegate(object o, PropertyChangedEventArgs pcea)
                {
                    if ((pcea.PropertyName == "FirstName") ||
                    (pcea.PropertyName == "MiddleName") ||
                    (pcea.PropertyName == "LastName"))
                    {
                        NameOfStudent = FirstName + " " + MiddleName + " " + lastName;                     
                    }
                };
            Email = "test@example.com";
            PrevBalance = 0;
            IsBoarder = true;
            IsActive = true;
            SPhoto = new byte[1];
            sPhoto[0] = 1;
        }
       
      
        public string FirstName
        {
            get { return this.firstName; }

            set
            {
                if (value != this.firstName)
                {
                    this.firstName = value;
                    NotifyPropertyChanged("FirstName");
                }
            }
        }
        public string MiddleName
        {
            get { return this.middleName; }

            set
            {
                if (value != this.middleName)
                {
                    this.middleName = value;
                    NotifyPropertyChanged("MiddleName");
                }
            }
        }
        public string LastName
        {
            get { return this.lastName; }

            set
            {
                if (value != this.lastName)
                {
                    this.lastName = value;
                    NotifyPropertyChanged("LastName");
                }
            }
        }

        public Gender Gender
        {
            get { return this.gender; }

            set
            {
                if (value != this.gender)
                {
                    this.gender = value;
                    NotifyPropertyChanged("Gender");
                }
            }
        }

        public DateTime DateOfBirth
        {
            get { return this.dateOfBirth; }

            set
            {
                if (value != this.dateOfBirth)
                {
                    this.dateOfBirth = value;
                    NotifyPropertyChanged("DateOfBirth");
                }
            }
        }
        public DateTime DateOfAdmission
        {
            get { return this.dateOfAdmission; }

            set
            {
                if (value != this.dateOfAdmission)
                {
                    this.dateOfAdmission = value;
                    NotifyPropertyChanged("DateOfAdmission");
                }
            }
        }
        public string NameOfGuardian
        {
            get { return this.nameOfGuardian; }

            set
            {
                if (value != this.nameOfGuardian)
                {
                    this.nameOfGuardian = value;
                    NotifyPropertyChanged("NameOfGuardian");
                }
            }
        }
        public string GuardianPhoneNo
        {
            get { return this.guardianPhoneNo; }

            set
            {
                if (value != this.guardianPhoneNo)
                {
                    this.guardianPhoneNo = value;
                    NotifyPropertyChanged("GuardianPhoneNo");
                }
            }
        }
        public string Email
        {
            get { return this.email; }

            set
            {
                if (value != this.email)
                {
                    this.email = value;
                    NotifyPropertyChanged("Email");
                }
            }
        }
        public string Address
        {
            get { return this.address; }

            set
            {
                if (value != this.address)
                {
                    this.address = value;
                    NotifyPropertyChanged("Address");
                }
            }
        }
        public string City
        {
            get { return this.city; }

            set
            {
                if (value != this.city)
                {
                    this.city = value;
                    NotifyPropertyChanged("City");
                }
            }
        }
        public string PostalCode
        {
            get { return this.postalCode; }

            set
            {
                if (value != this.postalCode)
                {
                    this.postalCode = value;
                    NotifyPropertyChanged("PostalCode");
                }
            }
        }
        public byte[] SPhoto
        {
            get { return this.sPhoto; }

            set
            {
                if (value != this.sPhoto)
                {
                    this.sPhoto = value;
                    NotifyPropertyChanged("SPhoto");
                }
            }
        }

        public int ClassID
        {
            get { return this.classID; }

            set
            {
                if (value != this.classID)
                {
                    this.classID = value;
                    NotifyPropertyChanged("ClassID");
                }
            }
        }
        public int DormitoryID
        {
            get { return this.dormitoryID; }

            set
            {
                if (value != this.dormitoryID)
                {
                    this.dormitoryID = value;
                    NotifyPropertyChanged("DormitoryID");
                }
            }
        }
        public string BedNo
        {
            get { return this.bedNo; }

            set
            {
                if (value != this.bedNo)
                {
                    this.bedNo = value;
                    NotifyPropertyChanged("BedNo");
                }
            }
        }
        public decimal PrevBalance
        {
            get { return this.prevBalance; }

            set
            {
                if (value != this.prevBalance)
                {
                    this.prevBalance = value;
                    NotifyPropertyChanged("PrevBalance");
                }
            }
        }
        public string PrevInstitution
        {
            get { return this.prevInstitution; }

            set
            {
                if (value != this.prevInstitution)
                {
                    this.prevInstitution = value;
                    NotifyPropertyChanged("PrevInstitution");
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

        public override string ToString()
        {
            return NameOfStudent;
        }
        public override bool CheckErrors()
        {
            ErrorCheckingStatus = Helper.ErrorCheckingStatus.Incomplete;
            try
            {
                ClearAllErrors();
                var s = DataAccess.GetStudent(StudentID);
                if (s.StudentID>0)
                {
                    List<string> errors = new List<string>();
                    errors.Add("Student already exists. (" + s.NameOfStudent.ToUpper() + ")");
                    SetErrors("StudentID", errors);
                }
                var f = DataAccess.GetBedNoUser(BedNo);
                if ((f.StudentID > 0)&&(!isBoarder))
                {
                    List<string> errors = new List<string>();
                    errors.Add("Bed No already in use by: (" + s.NameOfStudent.ToUpper() + ")");
                    SetErrors("BedNo", errors);
                }
            }
            catch (Exception e)
            {
                List<string> errors = new List<string>();
                errors.Add(e.Message);
                SetErrors("", errors);
            }
            NotifyPropertyChanged("HasErrors");
            ErrorCheckingStatus = Helper.ErrorCheckingStatus.Complete;
            return HasErrors;
        }
        public override void Reset()
        {
            StudentID = 0;
            FirstName = "";
            MiddleName = "";
            LastName = "";
            DateOfBirth = new DateTime(1900, 1, 1);
            DateOfAdmission = DateTime.Now;
            NameOfGuardian = "";
            GuardianPhoneNo = "";
            Address = "";
            City = "";
            PostalCode = "";
            SPhoto = null;
            ClassID = 0;
            DormitoryID = 0;
            BedNo = "";
            PrevInstitution = "";
        }
        public new void CopyFrom(StudentModel student)
        {
            base.CopyFrom(student);
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
            this.IsActive = student.IsActive;
            this.IsBoarder = student.IsBoarder;
            this.Gender = student.Gender;
            this.KCPEScore = student.KCPEScore;
        }
        public bool IsBoarder
        {
            get { return isBoarder; }
            set
            {
                if (value != isBoarder)
                    isBoarder = value;
                NotifyPropertyChanged("IsBoarder");
            }
        }
        public int KCPEScore
        {
            get { return kcpescore; }
            set
            {
                if (value != kcpescore)
                    kcpescore = value;
                NotifyPropertyChanged("KCPEScore");
            }
        }
    }

}
