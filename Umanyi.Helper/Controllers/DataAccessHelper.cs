using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UmanyiSMS.Lib.Controllers
{
    public static class DataAccessHelper
    {
        private static DBHelper helper;
        public static DBHelper Helper
        {
            get { return helper; }
            set { helper = value; } 
        }
    }
}
