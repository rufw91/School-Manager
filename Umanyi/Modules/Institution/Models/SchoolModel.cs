using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.ComponentModel;
using UmanyiSMS.Lib;

namespace UmanyiSMS.Modules.Institution.Models
{

    public class InstitutionModel : ModelBase
    {
         public InstitutionModel()
         {
             
         }

         public string Name
         { get; set; }

         public string Address
         { get; set; }

         public string City
         { get; set; }

         public string PostalCode
         { get; set; }

         public string PhoneNo
         { get; set; }

         public string AltPhoneNo
         { get; set; }

         public string Email
         { get; set; }

         public string Fax
         { get; set; }

         public string Motto
         { get; set; }

         public override void Reset()
         {
             
         }
    }
}
