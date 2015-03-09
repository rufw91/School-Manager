using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helper.Models
{
    public class ApplicationPersistModel
    {
        public ApplicationPersistModel()
        {

        }
        public ApplicationPersistModel(ApplicationModel value)
        {
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
            Culture = value.Culture;
        }
        public string Name { get; set; }
        public string FullName { get; set; }
        public string FullNameAlt { get; set; }
        public string AltInfo { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string PhoneNo { get; set; }
        public string Email { get; set; }
        public string Motto { get; set; }
        public byte[] SPhoto { get; set; }
        public string ServerName { get; set; }
        public int Culture { get; set; }

    }
}
