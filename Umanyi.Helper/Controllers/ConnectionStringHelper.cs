﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Windows;

using System.Security;
using System.Net;
using System.Data.SqlClient;
using System.IO;
using System.Text.RegularExpressions;

namespace UmanyiSMS.Lib.Controllers
{
    public static class ConnectionStringHelper
    {
        static string localDB = "Server=" + Properties.Settings.Default.Info.ServerName + ";MultipleActiveResultSets=true;AttachDBFilename=" +
            Path.Combine(new FileInfo(Application.ResourceAssembly.Location).DirectoryName, "UmanyiSMS.mdf") + ";Initial Catalog=UmanyiSMS;";
        static string sqlServer = "Data Source=" +
                Lib.Properties.Settings.Default.Info.ServerName +
                ";Connection Timeout=300;Encrypt=True;TrustServerCertificate=True;Initial Catalog=UmanyiSMS;";
        static string masterLocalDB = "Server=" + Properties.Settings.Default.Info.ServerName + ";Integrated Security=SSPI;MultipleActiveResultSets=true;TrustServerCertificate=True;"
           + ";Initial Catalog=master;";
        static readonly string masterConnString = "Data Source=" +
                Lib.Properties.Settings.Default.Info.ServerName +
                ";Database=Master;Connection Timeout=300;Encrypt=True;TrustServerCertificate=True;Intergrated Security=SSPI;";

        public static string ConnectionString
        {
            get { return Regex.Match(Properties.Settings.Default.Info.ServerName, "LocalDB", RegexOptions.IgnoreCase).Success ? localDB : sqlServer; }
        }

        public static string SSPIConnectionString
        {
            get { return Regex.Match(Properties.Settings.Default.Info.ServerName, "LocalDB", RegexOptions.IgnoreCase).Success ? masterLocalDB : masterConnString; }
        }

        public static string MasterConnectionString
        {
            get { return Regex.Match(Properties.Settings.Default.Info.ServerName, "LocalDB", RegexOptions.IgnoreCase).Success ? masterLocalDB : masterConnString; }
        }

    }
}
