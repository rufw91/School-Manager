using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Globalization;
using System.Windows;
namespace Helper.Models
{
    public class ApplicationModel: NotifiesPropertyChanged
    {
        int culture;
        public ApplicationModel()
        {
            Name = "St Mary's Girls";
            Address = "80 - 90108";
            City = "Kola";
            PhoneNo = "+254 721 437 475";
            Email = "stmarysgirlskola@yahoo.com";

            Culture = new CultureInfo("en-GB").LCID;
        }
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string PhoneNo { get; set; }
        public string Email { get; set; }
        public string Motto { get; set; }

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
    }
}
