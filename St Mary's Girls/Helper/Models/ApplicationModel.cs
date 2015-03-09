using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Globalization;
using System.Windows;
using System.Windows.Media.Imaging;
using System.IO;
using System.ComponentModel;
namespace Helper.Models
{
    public sealed class ApplicationModel : NotifiesPropertyChanged
    {
        int culture;
        byte[] sPhoto;
        private string name;
        private UserModel currentUser;
        private string serverName;
        private BitmapImage logo;
        private string motto;
        private string email;
        private string phoneNo;
        private string city;
        private string address;
        private string altInfo;
        private string fullNameAlt;
        private string fullName;
        public ApplicationModel()
        {
            PropertyChanged += (o, e) =>
            {
                if (e.PropertyName == "SPhoto")
                {
                    Logo = GetImage();
                }
            };
            Name = "Mbee High School";
            FullName = "MBEE HIGH SCHOOL - KATHIANI";
            FullNameAlt = "Mbee High School";
            AltInfo = "Mixed Secondary School";
            Address = "40 - 90105";
            City = "Kathiani";
            PhoneNo = "+254 729 897 871";
            CurrentUser = new UserModel();
            Email = "info@monsoondigital.co.ke";
            ServerName = Environment.MachineName + "\\Starehe";
            Culture = new CultureInfo("en-GB").LCID;
            SPhoto = null;
            Logo = GetImage();
            Motto = "Strive for Excellence";
            
        }
        public ApplicationModel(ApplicationPersistModel info)
        {
            PropertyChanged += (o, e) =>
            {
                if (e.PropertyName == "SPhoto")
                {
                    Logo = GetImage();
                }
            };
            Name = info.Name;
            FullName = info.FullName;
            FullNameAlt = info.FullNameAlt;
            AltInfo = info.AltInfo;
            Address = info.Address;
            City = info.City;
            PhoneNo = info.PhoneNo;
            CurrentUser = new UserModel();
            Email = info.Email;
            ServerName = info.ServerName;
            Culture = info.Culture;
            SPhoto = info.SPhoto;
            Logo = GetImage();
            Motto = info.Motto;
        }
        public string Name
        {
            get { return name; }
            set
            {
                if (this.name != value)
                {
                    this.name = value;
                    NotifyPropertyChanged("Name");
                }
            }
        }
        public string FullName
        {
            get { return fullName; }
            set
            {
                if (this.fullName != value)
                {
                    this.fullName = value;
                    NotifyPropertyChanged("FullName");
                }
            }
        }
        public string FullNameAlt
        {
            get { return fullNameAlt; }
            set
            {
                if (this.fullNameAlt != value)
                {
                    this.fullNameAlt = value;
                    NotifyPropertyChanged("FullNameAlt");
                }
            }
        }
        public string AltInfo
        {
            get { return altInfo; }
            set
            {
                if (this.altInfo != value)
                {
                    this.altInfo = value;
                    NotifyPropertyChanged("AltInfo");
                }
            }
        }
        public string Address
        {
            get { return address; }
            set
            {
                if (this.address != value)
                {
                    this.address = value;
                    NotifyPropertyChanged("Address");
                }
            }
        }
        public string City
        {
            get { return city; }
            set
            {
                if (this.city != value)
                {
                    this.city = value;
                    NotifyPropertyChanged("City");
                }
            }
        }
        public string PhoneNo
        {
            get { return phoneNo; }
            set
            {
                if (this.phoneNo != value)
                {
                    this.phoneNo = value;
                    NotifyPropertyChanged("PhoneNo");
                }
            }
        }
        public string Email
        {
            get { return email; }
            set
            {
                if (this.email != value)
                {
                    this.email = value;
                    NotifyPropertyChanged("Email");
                }
            }
        }
        public string Motto
        {
            get { return motto; }
            set
            {
                if (this.motto != value)
                {
                    this.motto = value;
                    NotifyPropertyChanged("Motto");
                }
            }
        }
        public BitmapImage Logo
        {
            get { return logo; }
            set
            {
                if (this.logo != value)
                {
                    this.logo = value;
                    NotifyPropertyChanged("Logo");
                }
            }
        }
        public string ServerName
        {
            get { return serverName; }
            set
            {
                if (this.serverName != value)
                {
                    this.serverName = value;
                    NotifyPropertyChanged("ServerName");
                }
            }
        }
        public int Culture
        {
            get { return this.culture; }
            set
            {
                if (this.culture != value)
                {
                    this.culture = value;
                    NotifyPropertyChanged("Culture");
                    CultureInfo.DefaultThreadCurrentCulture = new CultureInfo(culture);
                    CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo(culture);
                }
            }
        }

        private BitmapImage GetImage()
        {
            BitmapImage gy = new BitmapImage();
            if (sPhoto.Length > 0)
            {

                using (MemoryStream mem = new MemoryStream(sPhoto))
                {
                    mem.Seek(0, System.IO.SeekOrigin.Begin);
                    gy.BeginInit();
                    gy.CacheOption = BitmapCacheOption.OnLoad;
                    gy.StreamSource = mem;
                    gy.EndInit();
                }
            }
            return gy;

        }

        public UserModel CurrentUser
        {
            get { return currentUser; }
            set
            {
                if (this.currentUser != value)
                {
                    this.currentUser = value;
                    NotifyPropertyChanged("CurrentUser");
                }
            }
        }

        public byte[] SPhoto
        {
            get { return sPhoto; }
            set
            {
                if (this.sPhoto != value)
                {
                    this.sPhoto = value;
                    NotifyPropertyChanged("SPhoto");
                }
            }
        }

        public void CopyFrom(ApplicationModel info)
        {
            Name = info.Name;
            FullName = info.FullName;
            FullNameAlt = info.FullNameAlt;
            AltInfo = info.AltInfo;
            Address = info.Address;
            City = info.City;
            PhoneNo = info.PhoneNo;
            Email = info.Email;
            ServerName = info.ServerName;
            Culture = info.Culture;
            SPhoto = info.SPhoto;
            Logo = GetImage();
        }
    }
}
