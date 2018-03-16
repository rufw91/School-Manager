using System;
using System.IO;
using System.Text.RegularExpressions;

namespace UmanyiSMS.Lib.Controllers
{
    public static class ConnectionStringHelper
    {

        public static string GetConnectionString(string serverName, bool useIS)
        {
            bool isLocalDb = Regex.Match(serverName, "LocalDB", RegexOptions.IgnoreCase).Success;
            string dbFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), @"Raphael Muindi\UmanyiSMS\UmanyiSMS.mdf");
            if (isLocalDb)
            {
                if (!File.Exists(dbFilePath))
                    throw new InvalidOperationException("If the server is LocalDb server, dbFilePath cannot be null. The file " + dbFilePath + " does not exist.");
                if (useIS)
                    return "Server=" + serverName + ";MultipleActiveResultSets=true;Connection Timeout=300;AttachDBFilename=" + dbFilePath +
                ";Initial Catalog=UmanyiSMS;Integrated Security=SSPI;";
                else return "Server=" + serverName + ";MultipleActiveResultSets=true;Connection Timeout=300;AttachDBFilename=" + dbFilePath +
                	";Initial Catalog=UmanyiSMS;";
            }
            else
            {
                if (useIS)
                    return "Data Source=" + serverName +
                    ";Connection Timeout=300;Encrypt=True;TrustServerCertificate=True;Integrated Security=SSPI;";
                else return "Data Source=" + serverName +
                ";Connection Timeout=300;Encrypt=True;TrustServerCertificate=True;";
            }
        }
        
    }
}
