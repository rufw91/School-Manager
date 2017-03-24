using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Windows;

using System.Security;
using System.Net;
using System.Data.SqlClient;
using System.IO;

namespace UmanyiSMS.Lib.Controllers
{
    public static class ConnectionStringHelper
    {
        static string localDB = "Server=(LocalDB)\\v11.0;MultipleActiveResultSets=true;TrustServerCertificate=True;" +
            "AttachDBFilename="+ Path.Combine(new FileInfo(Application.ResourceAssembly.Location).DirectoryName,"UmanyiSMS.mdf")+";Initial Catalog=UmanyiSMS;";
        static string masterLocalDB = "Server=(LocalDB)\\v11.0;Integrated Security=SSPI;MultipleActiveResultSets=true;TrustServerCertificate=True;" 
           + ";Initial Catalog=master;";
        static readonly string connStr = "Data Source=" +
                Lib.Properties.Settings.Default.Info.ServerName +
                ";Database=UmanyiSMS;Connection Timeout=300;Encrypt=True;TrustServerCertificate=True;";
        static readonly string masterConnString = "Data Source=" +
                Lib.Properties.Settings.Default.Info.ServerName +
                ";Database=Master;Connection Timeout=300;Encrypt=True;TrustServerCertificate=True;Intergrated Security=SSPI;";
        
        public static string ConnectionString
        {
            get { return localDB; }
        }

        public static string SSPIConnectionString
        {
            get { return masterLocalDB; }
        }

        public static string MasterConnectionString
        {
            get { return masterLocalDB; }
        }
            
    }
}
