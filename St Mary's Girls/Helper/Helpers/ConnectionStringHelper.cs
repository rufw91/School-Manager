using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Windows;
using Helper.Security;
using System.Security;


namespace Helper
{
    public static class ConnectionStringHelper
    {
        static readonly string connStr = "Data Source=" +
                Helper.Properties.Settings.Default.ServerName +
                ";Database=Starehe;Connection Timeout=30;Encrypt=True;TrustServerCertificate=True;";
        static readonly string masterConnString = "Data Source=" +
                Helper.Properties.Settings.Default.ServerName +
                ";Database=Master;Connection Timeout=30;Encrypt=True;TrustServerCertificate=True;";
        static readonly string testConnString = "Data Source=" +
               Helper.Properties.Settings.Default.ServerName +
               ";Connection Timeout=30;Encrypt=True;TrustServerCertificate=True;";

        public static string ConnectionString
        {
            get {  return connStr; }
        }

        public static string MasterConnectionString
        {
            get { return masterConnString; }
        }
        public static string TestConnectionString
        {
            get { return testConnString; }
        }
    }
}
