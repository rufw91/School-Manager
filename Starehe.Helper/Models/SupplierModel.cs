using System;
namespace Helper.Models
{
    public class SupplierModel : SupplierBaseModel
    
    {
        string phoneNo;
        string altPhoneNo;
        string email;
        string address;
        string postalCode;
        string city;
        string pinNo;
        public SupplierModel()
        {
            PhoneNo = "";
            AltPhoneNo = "";
            Email = "";
            Address = "";
            PostalCode = "";
            City = "";
            PINNo = "";
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
        public string AltPhoneNo
        {
            get { return this.altPhoneNo; }

            set
            {
                if (value != this.altPhoneNo)
                {
                    this.altPhoneNo = value;
                    NotifyPropertyChanged("AltPhoneNo");
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
        public string PINNo
        {
            get { return this.pinNo; }

            set
            {
                if (value != this.pinNo)
                {
                    this.pinNo = value;
                    NotifyPropertyChanged("PINNo");
                }
            }
        }

        
        public override void Reset()
        {
            base.Reset();
            PhoneNo = "";
            AltPhoneNo = "";
            Email = "";
            Address = "";
            PostalCode = "";
            City = "";
            PINNo = "";
        }
    }
}
