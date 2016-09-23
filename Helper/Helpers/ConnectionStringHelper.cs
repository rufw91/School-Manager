using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Windows;
using Helper.Security;
using System.Security;
using System.Net;
using System.Data.SqlClient;


namespace Helper
{
    public static class ConnectionStringHelper
    {
        static readonly string connStr = "Data Source=" +
                Helper.Properties.Settings.Default.Info.ServerName +
                ";Database=UmanyiSMS;Connection Timeout=300;Encrypt=True;TrustServerCertificate=True;";
        static readonly string masterConnString = "Data Source=" +
                Helper.Properties.Settings.Default.Info.ServerName +
                ";Database=Master;Connection Timeout=300;Encrypt=True;TrustServerCertificate=True;";
        static readonly string win32ConnString = "Data Source=" +
                Helper.Properties.Settings.Default.Info.ServerName +
                ";Database=Master;Connection Timeout=300;Encrypt=True;TrustServerCertificate=True;Integrated Security='SSPI'";
        static readonly string testConnString = "Data Source=" +
               Helper.Properties.Settings.Default.Info.ServerName +
               ";Connection Timeout=300;Encrypt=True;TrustServerCertificate=True;";

        public static string ConnectionString
        {
            get { return connStr; }
        }

        public static string MasterConnectionString
        {
            get { return masterConnString; }
        }
        public static string TestConnectionString
        {
            get { return testConnString; }
        }

        public static string Win32ConnectionString
        {
            get { return win32ConnString; }
        }

        public static string CreateTestConnSTr(string serverName)
        {
            return "Data Source=" + serverName +
                ";Connection Timeout=30;Encrypt=True;TrustServerCertificate=True;";
        }

        public static string GetConnectionString(SqlCredential newCredentials)
        {
            return connStr + "User Id=" + newCredentials.UserId + ";Password=" + new NetworkCredential(newCredentials.UserId, newCredentials.Password).Password;
        }
        internal static string GetConnectionString(SqlCredential newCredentials,string connStr)
        {
            return connStr + " User Id=" + newCredentials.UserId + ";Password=" + new NetworkCredential(newCredentials.UserId, newCredentials.Password).Password;
        }
    
    }
}
