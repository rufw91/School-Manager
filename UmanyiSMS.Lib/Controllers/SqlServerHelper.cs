using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
namespace UmanyiSMS.Lib.Controllers
{
    public sealed class SqlServerHelper: DBHelper
    {
        static SqlServerHelper instance;
        private SqlServerHelper()
        {
            _credentials = null;
            _useSSPI = false; 
        }
        private SqlServerHelper(string serverName,string dbName, SqlCredential credentials,bool useSSPI)
        {
            _serverName = serverName;
            _dbName = dbName;
            _credentials = credentials;
            _useSSPI = useSSPI;
        }

        public static SqlServerHelper CreateInstance(string serverName, string dbName, SqlCredential credentials, bool useSSPI)
        {
            instance = new SqlServerHelper(serverName, dbName, credentials, useSSPI);
            return instance;
        }

        internal void SetServer(string serverName)
        {
            _serverName = serverName;
        }

        internal void SetDb(string dbName)
        {
            _dbName = dbName;
        }

        internal override bool TestCredential(string serverName,SqlCredential newCredentials)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionStringHelper.GetConnectionString(serverName,false)))
                {
                    conn.Credential = newCredentials;
                    conn.Open();
                }
                return true;
            }
            catch (Exception e)
            {
                Log.E(e.ToString(), null);
                return false;
            }
        }

        internal override void SetCredential(SqlCredential newCredentials)
        {
            _credentials = newCredentials;
            if (_credentials!=null)
            _useSSPI = false;
        }

        internal void SetUseSSPI(bool useSSPI)
        {
            _useSSPI = useSSPI;
        }

        SqlCredential _credentials;

        bool _useSSPI;

        string _dbName;
        private string _serverName;

        public override dynamic CreateConnection()
        {
            SqlConnection conn;
            try
            {
                conn=CreateConnection(ConnectionStringHelper.GetConnectionString(_serverName,_useSSPI),_credentials);
            }
            catch (Exception e)
            {
                Log.E(e.ToString(), null);
                throw e;
            }
            return conn;
        }

        internal override dynamic CreateConnection(string connectionString,SqlCredential credential)
        {
            SqlConnection conn;
            try
            {
                conn = new SqlConnection(connectionString);               
                conn.Credential = credential;
                conn.Open();
                if (conn.State == ConnectionState.Connecting)
                    while (conn.State != ConnectionState.Open)
                    { }
            }
            catch (Exception e)
            {
                Log.E(e.ToString(), null);
                throw e;
            }
            return conn;
        }

        public override string ExecuteScalar(string commandText)
        {
            return ExecuteScalar(commandText, true);
        }
        public override string ExecuteScalar(string commandText, IEnumerable<DbParameter> paramColl)
        {
            return ExecuteScalar(commandText, paramColl, true);
        }

        public string ExecuteScalar(string commandText, bool hasHeader)
        {
            object res = ExecuteObjectScalar(commandText, hasHeader);
            if (res != null)
                return res.ToString();
            else return "";
        }
        public string ExecuteScalar(string commandText, IEnumerable<DbParameter> paramColl, bool hasHeader)
        {
            object res = ExecuteObjectScalar(commandText,paramColl, hasHeader);
            if (res != null)
                return res.ToString();
            else return "";
        }

        public override object ExecuteObjectScalar(string commandText)
        {
            return ExecuteObjectScalar(commandText, true);
        }
        public override object ExecuteObjectScalar(string commandText, IEnumerable<DbParameter> paramColl)
        {
            return ExecuteObjectScalar(commandText, paramColl, true);
        }

        public object ExecuteObjectScalar(string commandText, bool hasHeader)
        {
            try
            {
                object tx;
                if (hasHeader)
                    commandText = "USE "+_dbName+"\r\nSET DATEFORMAT DMY\r\n" + commandText;
                using (SqlConnection DBConnection = CreateConnection())
                {
                    SqlCommand sqlcmd = new SqlCommand(commandText, DBConnection);
                    tx = sqlcmd.ExecuteScalar();
                    sqlcmd.Dispose();
                }
                return tx;
            }
            catch (Exception e)
            {
                Log.E(e.ToString(), null);
                throw;
            }
        }
        public object ExecuteObjectScalar(string commandText, IEnumerable<DbParameter> paramColl, bool hasHeader)
        {
            try
            {
                object tx;
                if (hasHeader)
                    commandText = "USE " + _dbName + "\r\nSET DATEFORMAT DMY\r\n" + commandText;
                using (SqlConnection DBConnection = CreateConnection())
                {
                    SqlCommand sqlcmd = new SqlCommand(commandText, DBConnection);
                    foreach (SqlParameter param in paramColl)
                    { sqlcmd.Parameters.Add(param); }
                    tx = sqlcmd.ExecuteScalar();
                    sqlcmd.Dispose();
                }
                return tx;
            }
            catch (Exception e)
            {
                Log.E(e.ToString(),null);
                throw;
            }
        }

        public override DataTable ExecuteNonQueryWithResult(string commandText)
        {
            return ExecuteNonQueryWithResultTable(commandText, true);
        }
        public override DataTable ExecuteNonQueryWithResultTable(string commandText, IEnumerable<DbParameter> paramColl)
        {
            DataTable result = new DataTable();

            try
            {
                using (SqlConnection DBConnection = CreateConnection())
                {
                    SqlCommand dtab = new SqlCommand();
                    dtab.CommandText = "USE " + _dbName + "\r\nSET DATEFORMAT DMY\r\n" + commandText;
                    dtab.Connection = DBConnection;
                    foreach (SqlParameter param in paramColl)
                    { dtab.Parameters.Add(param); }

                    SqlDataReader reader = dtab.ExecuteReader();
                    result = GetResultTable(reader);
                    reader.Close();
                    dtab.Dispose();
                }
            }
            catch (Exception e)
            {
                Log.E(e.ToString(), null);
                throw;
            }
            return result;
        }

        public DataTable ExecuteNonQueryWithResultTable(string commandText, bool hasHeader)
        {
            DataTable result = new DataTable();
            if (hasHeader)
                commandText = "USE " + _dbName + "\r\nSET DATEFORMAT DMY\r\n" + commandText;
            try
            {
                using (SqlConnection DBConnection = CreateConnection())
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = "USE " + _dbName + "\r\nSET DATEFORMAT DMY\r\n" + commandText;
                    cmd.Connection = DBConnection;

                    cmd.ExecuteNonQuery();
                    SqlDataReader reader = cmd.ExecuteReader();

                    result = GetResultTable(reader);
                    reader.Close();
                    DBConnection.Close();
                    cmd.Dispose();
                }
            }
            catch (Exception e)
            {
                Log.E(e.ToString(), null);
                throw;
            }
            return result;
        }

        public override bool ExecuteNonQuery(string commandText)
        {
            return ExecuteNonQuery(commandText, true);
        }
        public override bool ExecuteNonQuery(string commandText, IEnumerable<DbParameter> paramColl)
        {
            bool result = false;

            try
            {
                using (SqlConnection DBConnection = CreateConnection())
                {
                    using (SqlCommand dta = new SqlCommand())
                    {
                        dta.CommandText = "USE " + _dbName + "\r\nSET DATEFORMAT DMY\r\n" + commandText;
                        dta.Connection = DBConnection;
                        foreach (SqlParameter param in paramColl)
                        { dta.Parameters.Add(param); }
                        dta.ExecuteNonQuery();
                    }
                }
                result = true;
            }
            catch (Exception e)
            {
                Log.E(e.ToString(), null);
                throw;
            }
            return result;
        }

        public bool ExecuteNonQuery(string commandText, bool hasHeader)
        {
            bool succ = false;
            if (hasHeader)
                commandText = "USE " + _dbName + "\r\nSET DATEFORMAT DMY\r\n" + commandText;

            try
            {
                using (SqlConnection DBConnection = CreateConnection())
                {
                    SqlCommand dta = new SqlCommand(commandText, DBConnection);
                    dta.ExecuteNonQuery();
                    DBConnection.Close();
                    dta.Dispose();
                }
                succ = true;
            }
            catch (Exception e)
            {
                Log.E(e.ToString(), null);
                throw;
            }
            return succ;

        }

        public Task<bool> TestDb(string connectionStr)
        {
            return TestDb(connectionStr,_credentials);
        }

        public Task<bool> TestDb(string connectionStr, SqlCredential credential)
        {
            return Task.Factory.StartNew<bool>(() =>
            {
                bool succ = false;
                try
                {
                    using (SqlConnection DBConnection = CreateConnection(connectionStr, credential))
                    {
                        SqlCommand dta = new SqlCommand("USE " + Properties.Settings.Default.Info.DBName + "", DBConnection);
                        dta.ExecuteNonQuery();
                        dta.Dispose();
                    }

                    succ = true;
                }
                catch (Exception e)
                {
                    Log.E(e.ToString(), null);
                }
                return succ;
            });
        }

        public Task<bool> ClearDb()
        {
            return Task.Factory.StartNew<bool>(() =>
            {
                string deleteStr =
                     "EXEC sp_MSForEachTable 'DISABLE TRIGGER ALL ON ?'";
                bool succ = ExecuteNonQuery(deleteStr);
                deleteStr = "EXEC sp_MSForEachTable 'ALTER TABLE ? NOCHECK CONSTRAINT ALL'";
                succ = succ && ExecuteNonQuery(deleteStr);
                deleteStr = "EXEC sp_MSForEachTable 'DELETE FROM ?'";
                succ = succ && ExecuteNonQuery(deleteStr);
                deleteStr = "EXEC sp_MSForEachTable 'ALTER TABLE ? CHECK CONSTRAINT ALL'";
                succ = succ && ExecuteNonQuery(deleteStr);
                deleteStr = "EXEC sp_MSForEachTable 'ENABLE TRIGGER ALL ON ?'";
                succ = succ && ExecuteNonQuery(deleteStr);
                deleteStr = "EXEC dbo.PrepareDb";
                succ = succ && ExecuteNonQuery(deleteStr);
                return succ;
            });
        }
        
        public Task<bool> CreateBackupAsync(string pathToFile)
        {
            return Task.Factory.StartNew<bool>(() =>
            {                
                try
                {
                    SqlConnection conn = CreateConnection(ConnectionStringHelper.GetConnectionString(_serverName, true), null);
                    using (conn)
                    {
                        string bkPath = Regex.Match(_serverName, "LocalDB", RegexOptions.IgnoreCase).Success ? FileHelper.GetTempFilePath("Bak") : FileHelper.GetNewNetworkServiceTempFilePath("Bak");                    
                     
                        var sc = new ServerConnection(conn);
                        Server server = new Server(sc);
                        Database db = server.Databases[_dbName];
                        RecoveryModel recoveryMode = db.DatabaseOptions.RecoveryModel;
                        Backup b = new Backup();
                        b.Action = BackupActionType.Database;
                        b.BackupSetDescription = "Full backup of " + _dbName;
                        b.BackupSetName = _dbName + " Backup";
                        b.Database = _dbName;
                        b.Devices.AddDevice(bkPath, DeviceType.File);
                        b.Incremental = false;
                        b.SqlBackup(server);
                        FileSystem.CopyFile(bkPath, pathToFile, UIOption.AllDialogs, UICancelOption.ThrowException);

                    }
                    return true;
                }
                catch(Exception e) {
                    MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    Log.E(e.ToString(), null);
                    return false; }
            });
        }

        public Task<bool> RestoreDb(string fileName)
        {
            return Task.Factory.StartNew<bool>(() =>
            {
                if (string.IsNullOrWhiteSpace(fileName))
                    return false;
                try
                {
                    SqlConnection conn = CreateConnection(ConnectionStringHelper.GetConnectionString(_serverName, true), null);
                    using (conn)
                    {                       
                        var sc = new ServerConnection(conn);
                        Server server = new Server(sc);
                        Database db = server.Databases[_dbName];
                        RecoveryModel recoveryMode = db.DatabaseOptions.RecoveryModel;
                        Restore b = new Restore();
                        b.Action = RestoreActionType.Database;
                        b.Database = db.Name;
                        b.Restart = true;
                        b.Database = _dbName;
                        b.Devices.AddDevice(fileName, DeviceType.File);
                       
                        b.SqlRestore(server);

                    }
                    return true;
                }
                catch(Exception e)
                {
                    Log.E(e.ToString(), typeof(DataAccessHelper));
                    return false;
                }
            });
        }

        internal Task<bool> TestBackupFile(string fileName)
        {
            string error;
            return Task.Factory.StartNew<bool>(() =>
            {
                try
                {
                    bool verifySuccessful = false;
                    SqlConnection conn = CreateConnection();

                    using (conn)
                    {
                        var sc = new ServerConnection(conn);
                        Server server = new Server(sc);
                        Restore rest = new Restore();

                        string pathToFile = FileHelper.GetNewNetworkServiceTempFilePath("Test");
                        FileSystem.CopyFile(fileName, pathToFile, UIOption.AllDialogs, UICancelOption.ThrowException);

                        rest.Devices.AddDevice(pathToFile, DeviceType.File);
                        verifySuccessful = rest.SqlVerify(server, out error);
                    }
                    return verifySuccessful;

                }
                catch
                {
                    return false;
                }
            });
        }

        public Task<bool> CleanDb()
        {
            return Task.Factory.StartNew<bool>(() =>
            {
                try
                {
                    string commandText = "USE " + _dbName + "\r\nSET DATEFORMAT DMY\r\n";
                   
                    bool succ = false;

                    int y = 0;
                    try
                    {
                        using (SqlConnection DBConnection = CreateConnection())
                        {
                            SqlCommand dta = new SqlCommand(commandText, DBConnection);
                            y = dta.ExecuteNonQuery();
                            dta.Dispose();
                        }
                        succ = true;
                        MessageBox.Show("Successfully removed " + y + " records.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    catch (Exception e)
                    {
                        Log.E(e.ToString(), typeof(DataAccessHelper));
                        throw;
                    }
                    return succ;
                }
                catch (Exception e) { Log.E(e.ToString(), typeof(DataAccessHelper)); return false; }
            });
        }

        internal Task<bool> SetOffline()
        {
            return  
             Task.Factory.StartNew<bool>(() =>
            {
                try
                {                	 
                    string commandText = "USE MASTER\r\nALTER DATABASE "+_dbName+" SET OFFLINE";
                    using (SqlConnection DBConnection = CreateConnection(ConnectionStringHelper.GetConnectionString(_serverName,true),null))
                    {
                        SqlCommand dta = new SqlCommand(commandText, DBConnection);
                        dta.ExecuteNonQuery();
                       dta.Dispose();
                    }
                   return true;
                }
                catch (Exception e)
                {
                    Log.E(e.ToString(), null);
                }
                return false;
            });
        }

        internal Task<bool> SetOnline()
        {
            return  
             Task.Factory.StartNew<bool>(() =>
            {
                try
                {                	 
                    string commandText = "USE MASTER\r\nALTER DATABASE "+_dbName+" SET ONLINE";
                    using (SqlConnection DBConnection = CreateConnection(ConnectionStringHelper.GetConnectionString(_serverName,true),null))
                    {
                        SqlCommand dta = new SqlCommand(commandText, DBConnection);
                        dta.ExecuteNonQuery();
                       dta.Dispose();
                    }
                   return true;
                }
                catch (Exception e)
                {
                    Log.E(e.ToString(), null);
                }
                return false;
            });
        }
        
        public static bool IsServerMachine
        { get { return GetIsServerMachine(); } }

        private static bool GetIsServerMachine()
        {            
            if (instance._serverName.ToLowerInvariant().Contains("localdb"))
                return true;
            return instance._serverName.ToLowerInvariant() == (Environment.MachineName + @"\Umanyi").ToLowerInvariant();
        }

        public override List<string> CopyFirstColumnToList(string commandText)
        {
            List<string> result = new List<string>();
                commandText = "USE " + _dbName + "\r\nSET DATEFORMAT DMY\r\n" + commandText;
            try
            {
                using (SqlConnection DBConnection = CreateConnection())
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = "USE " + _dbName + "\r\nSET DATEFORMAT DMY\r\n" + commandText;
                    cmd.Connection = DBConnection;

                    cmd.ExecuteNonQuery();
                    SqlDataReader reader = cmd.ExecuteReader();

                    result = GetResultFirstCol(reader);
                    reader.Close();
                    DBConnection.Close();
                    cmd.Dispose();
                }
            }
            catch (Exception e)
            {
                Log.E(e.ToString(), null);
                throw;
            }
            return result;
        }

        public override List<string> CopyFirstColumnToList(string commandText, IEnumerable<DbParameter> paramColl)
        {
            List<string> result = new List<string>();

            try
            {
                using (SqlConnection DBConnection = CreateConnection())
                {
                    SqlCommand dtab = new SqlCommand();
                    dtab.CommandText = "USE " + _dbName + "\r\nSET DATEFORMAT DMY\r\n" + commandText;
                    dtab.Connection = DBConnection;
                    foreach (SqlParameter param in paramColl)
                    { dtab.Parameters.Add(param); }

                    SqlDataReader reader = dtab.ExecuteReader();
                    result = GetResultFirstCol(reader);
                    reader.Close();
                    dtab.Dispose();
                }
            }
            catch (Exception e)
            {
                Log.E(e.ToString(), null);
                throw;
            }
            return result;
        }
    }
}
