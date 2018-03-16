using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UmanyiSMS.Lib.Models
{
    public class ApplicationPersistModel
    {
        public ApplicationPersistModel()
        {
            AccentColor = new byte[4] { 0, 0, 0, 0 };
        }
        public ApplicationPersistModel(ApplicationModel value)
        {
            SyncAddress = value.SyncAddress;
            ID = value.ID;
            Name = value.Name;
            FullName = value.FullName;
            FullNameAlt = value.FullNameAlt;
            AltInfo = value.AltInfo;
            Address = value.Address;
            City = value.City;
            PhoneNo = value.PhoneNo;
            Email = value.Email;
            Motto = value.Motto;
            SPhoto = value.SPhoto;
            ServerName = value.ServerName;
            DBName = value.DBName;
            Culture = value.Culture;
            Theme = value.Theme;
            AccentColor = new byte[4] { value.AccentColor.A, value.AccentColor.R, value.AccentColor.G, value.AccentColor.B };
        }
        public string SyncAddress { get; set; }
        public string ID { get; set; }
        public string Name { get; set; }
        public string FullName { get; set; }
        public string FullNameAlt { get; set; }
        public string AltInfo { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string PhoneNo { get; set; }
        public string Email { get; set; }
        public string Motto { get; set; }
        public string Theme { get; set; }
        public byte[] AccentColor { get; set; }
        public byte[] SPhoto { get; set; }
        public string ServerName { get; set; }
        public string DBName { get; set; }
        public int Culture { get; set; }

    }
}
