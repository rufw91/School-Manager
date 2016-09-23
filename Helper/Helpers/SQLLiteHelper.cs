using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data.SQLite;

namespace Helper
{
    public sealed class SQLLiteHelper : DBHelper
    {
        private SqlCredential Credentials;
        public SQLLiteHelper(SqlCredential credentials)
        {
            Credentials = credentials;
        }
        internal override bool TestCredential(SqlCredential newCredentials)
        {
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(ConnectionStringHelper.GetConnectionString(newCredentials)))
                {
                    conn.Open();
                    conn.Close();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public override dynamic CreateConnection()
        {
            SQLiteConnection conn;
            try
            {
                conn = CreateConnection(ConnectionStringHelper.GetConnectionString(this.Credentials));
            }
            catch
            {
                throw;
            }
            return conn;
        }

        internal override dynamic CreateConnection(string connectionString)
        {
            SQLiteConnection conn;
            try
            {

                conn = new SQLiteConnection(connectionString);

                conn.Open();
                if (conn.State == ConnectionState.Connecting)
                    while (conn.State != ConnectionState.Open)
                    { }
            }
            catch (Exception e)
            {
                throw;
            }
            return conn;
        }

        public override string ExecuteScalar(string commandText)
        {
            object res = ExecuteObjectScalar(commandText);
            if (res != null)
                return res.ToString();
            else return "";
        }

        public override string ExecuteScalar(string commandText, IEnumerable<DbParameter> paramColl)
        {
            object res = ExecuteObjectScalar(commandText, paramColl);
            if (res != null)
                return res.ToString();
            else return "";
            
        }

        public override object ExecuteObjectScalar(string commandText)
        {
            try
            {
                object tx;
                commandText = "SET DATEFORMAT DMY\r\n" + commandText;
                using (SQLiteConnection DBConnection = CreateConnection())
                {
                    SQLiteCommand sqlcmd = new SQLiteCommand(commandText, DBConnection);
                    tx = sqlcmd.ExecuteScalar();
                    sqlcmd.Dispose();
                }
                return tx;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public override object ExecuteObjectScalar(string commandText, IEnumerable<DbParameter> paramColl)
        {
            try
            {
                object tx;
                commandText = "SET DATEFORMAT DMY\r\n" + commandText;
                using (SQLiteConnection DBConnection = CreateConnection())
                {
                    SQLiteCommand sqlcmd = new SQLiteCommand(commandText, DBConnection);
                    foreach (SQLiteParameter param in paramColl)
                    { sqlcmd.Parameters.Add(param); }
                    tx = sqlcmd.ExecuteScalar();
                    sqlcmd.Dispose();
                }
                return tx;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public override DataTable ExecuteNonQueryWithResultTable(string commandText, IEnumerable<DbParameter> paramColl)
        {
            DataTable result = new DataTable();

            try
            {
                using (SQLiteConnection DBConnection = CreateConnection())
                {
                    SQLiteCommand dtab = new SQLiteCommand();
                    dtab.CommandText = commandText;
                    dtab.Connection = DBConnection;
                    foreach (SQLiteParameter param in paramColl)
                    { dtab.Parameters.Add(param); }

                    SQLiteDataReader reader = dtab.ExecuteReader();
                    result = GetResultTable(reader);
                    reader.Close();
                    dtab.Dispose();
                }
            }
            catch (Exception e)
            {
                throw;
            }
            return result;
        }

        public override DataTable ExecuteNonQueryWithResultTable(string commandText)
        {
            DataTable result = new DataTable();
            try
            {
                using (SQLiteConnection DBConnection = CreateConnection())
                {
                    SQLiteCommand cmd = new SQLiteCommand();
                    cmd.CommandText = commandText;
                    cmd.Connection = DBConnection;

                    cmd.ExecuteNonQuery();
                    SQLiteDataReader reader = cmd.ExecuteReader();

                    result = GetResultTable(reader);
                    reader.Close();
                    DBConnection.Close();
                    cmd.Dispose();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return result;
        }

        

        public override bool ExecuteNonQuery(string commandText, IEnumerable<DbParameter> paramColl)
        {
            bool result = false;

            try
            {
                using (SQLiteConnection DBConnection = CreateConnection())
                {
                    using (SQLiteCommand dta = new SQLiteCommand())
                    {
                        dta.CommandText = commandText;
                        dta.Connection = DBConnection;
                        foreach (SQLiteParameter param in paramColl)
                        { dta.Parameters.Add(param); }
                        dta.ExecuteNonQuery();
                    }
                }
                result = true;
            }
            catch (Exception e)
            {
                throw e;
            }
            return result;
        }

        public override bool ExecuteNonQuery(string commandText)
        {
            bool succ = false;
            commandText = "SET DATEFORMAT DMY\r\n" + commandText;

            try
            {
                using (SQLiteConnection DBConnection = CreateConnection())
                {
                    SQLiteCommand dta = new SQLiteCommand(commandText, DBConnection);
                    dta.ExecuteNonQuery();
                    DBConnection.Close();
                    dta.Dispose();
                }
                succ = true;
            }
            catch (Exception e)
            {
                throw e;
            }
            return succ;

        }

        internal override void SetCredential(SqlCredential newCredentials)
        {
            Credentials = newCredentials;
        }

        public override List<string> CopyFirstColumnToList(string commandText)
        {
            List<string> result = new List<string>();
            try
            {
                using (SQLiteConnection DBConnection = CreateConnection())
                {
                    SQLiteCommand cmd = new SQLiteCommand();
                    cmd.CommandText = commandText;
                    cmd.Connection = DBConnection;

                    cmd.ExecuteNonQuery();
                    SQLiteDataReader reader = cmd.ExecuteReader();

                    result = GetResultFirstCol(reader);
                    reader.Close();
                    DBConnection.Close();
                    cmd.Dispose();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return result;
        }

        public override List<string> CopyFirstColumnToList(string commandText, IEnumerable<DbParameter> paramColl)
        {
            List<string> result = new List<string>();

            try
            {
                using (SQLiteConnection DBConnection = CreateConnection())
                {
                    SQLiteCommand dtab = new SQLiteCommand();
                    dtab.CommandText = commandText;
                    dtab.Connection = DBConnection;
                    foreach (SQLiteParameter param in paramColl)
                    { dtab.Parameters.Add(param); }

                    SQLiteDataReader reader = dtab.ExecuteReader();
                    result = GetResultFirstCol(reader);
                    reader.Close();
                    dtab.Dispose();
                }
            }
            catch (Exception e)
            {
                throw;
            }
            return result;
        }
    }
}
