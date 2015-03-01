﻿using System;
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
                string connstring = "Data Source=" + server +
               ";Connection Timeout=30;Encrypt=True;TrustServerCertificate=True;Password=" + pwd + ";User ID=" + userName + ";";
                session["CONNSTRING"] = connstring;
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