using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helper
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
