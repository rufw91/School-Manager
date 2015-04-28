using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Helper.Models
{
    public class UserModel
    {
        public UserModel()
        {
            UserName = "";
            UserID = "";
            Role = "None";
            Photo = new byte[0];
        }
        public string UserName { get; set; }
        public string UserID { get; set; }
        public byte[] Photo { get; set; }
        public string Role { get; set; }
    }
}
