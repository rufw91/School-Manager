using System;
using System.Globalization;
using System.IO;
using System.Windows.Media.Imaging;
using UmanyiSMS.Lib.Presentation;

namespace UmanyiSMS.Lib.Models
{
    public sealed class ApplicationModel : NotifiesPropertyChanged
    {
        int culture;
        string syncAddress;
        byte[] sPhoto;
        private string id;
        private string name;
        private UserModel currentUser;
        private string serverName;
        private string dbName;
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
            SyncAddress = "";
            ID = "";
            Name = "";
            FullName = "";
            FullNameAlt = "";
            AltInfo = "";
            Address = "";
            City = "";
            PhoneNo = "";
            CurrentUser = new UserModel();
            Email = "";
            ServerName = Environment.MachineName + "\\Umanyi";
            DBName = "UmanyiSMS";
            Culture = new CultureInfo("en-GB").LCID;
            SPhoto = null;
            Logo = GetImage();
            Motto = "";
            
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
            SyncAddress = info.SyncAddress;
            ID = info.ID;
            Name = info.Name;
            FullName = info.FullName;
            FullNameAlt = info.FullNameAlt;
            AltInfo = info.AltInfo;
            Address = info.Address;
            City = info.City;
            PhoneNo = info.PhoneNo;
            CurrentUser = new UserModel();
            Email = info.Email;
            DBName = info.DBName;
            ServerName = info.ServerName;
            Culture = info.Culture;
            SPhoto = info.SPhoto;
            Logo = GetImage();
            Motto = info.Motto;
        }

        public string SyncAddress
        {
            get { return syncAddress; }
            set
            {
                if (this.syncAddress != value)
                {
                    this.syncAddress = value;
                    NotifyPropertyChanged("SyncAddress");
                }
            }
        }

        public string ID
        {
            get { return id; }
            set
            {
                if (this.id != value)
                {
                    this.id = value;
                    NotifyPropertyChanged("ID");
                }
            }
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
        public string DBName
        {
            get { return dbName; }
            set
            {
                if (this.dbName != value)
                {
                    this.dbName = value;
                    NotifyPropertyChanged("DBName");
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
            if (sPhoto == null)
                return null;
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
            SyncAddress = info.SyncAddress;
            ID = info.ID;
            Name = info.Name;
            FullName = info.FullName;
            FullNameAlt = info.FullNameAlt;
            AltInfo = info.AltInfo;
            Address = info.Address;
            City = info.City;
            PhoneNo = info.PhoneNo;
            DBName = info.DBName;
            Email = info.Email;
            ServerName = info.ServerName;
            Culture = info.Culture;
            SPhoto = info.SPhoto;
            Logo = GetImage();
        }
    }
}
