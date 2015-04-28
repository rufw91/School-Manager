using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace Helper.Models
{
    public class StaffModel: ModelBase
    {
        private int staffId=0;
        private string nameOfStaff = "";
        private DateTime dateOfAdmission = DateTime.Now;
        private string nationalId = "";
        private string phoneNo = "";
        private string email = "";
        private string address = "";
        private string city = "";
        private string postalCode = "";
        private byte[] sPhoto=new byte[0];

        public StaffModel() { }

        public int StaffID
        {
            get { return this.staffId; }

            set
            {
                if (value != this.staffId)
                {
                    this.staffId = value;
                    NotifyPropertyChanged("StaffID");
                }
            }
        }

        public string Name
        {
            get { return this.nameOfStaff; }

            set
            {
                if (value != this.nameOfStaff)
                {
                    this.nameOfStaff = value;
                    NotifyPropertyChanged("Name");
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

        public string NationalID
        {
            get { return this.nationalId; }

            set
            {
                if (value != this.nationalId)
                {
                    this.nationalId = value;
                    NotifyPropertyChanged("NationalID");
                }
            }
        }

        public string PhoneNo
        {
            get { return this.phoneNo; }

            set
            {
                if (value != this.phoneNo)
                {
                    this.phoneNo = value;
                    NotifyPropertyChanged("PhoneNo");
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

        public override void Reset()
        {
            StaffID = 0;
            Name = "";
            DateOfAdmission = new DateTime(1900,1,1);
            NationalID = "";
            PhoneNo = "";
            Email = "";
            Address = "";
            City = "";
            PostalCode = "";
            SPhoto = new byte[0];
        }

        
    }

    
}
