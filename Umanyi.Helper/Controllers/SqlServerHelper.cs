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
        private SqlServerHelper(SqlCredential credentials,bool useSSPI)
        {
            _credentials = credentials;
            _useSSPI = useSSPI;
        }

        public static SqlServerHelper CreateInstance(SqlCredential credentials, bool useSSPI)
        {
            if (instance == null)
                instance = new SqlServerHelper(credentials, useSSPI);
            else
            {
                instance.SetCredential(credentials);
                instance.SetUseSSPI(useSSPI);
            }
            return instance;
        }
        internal override bool TestCredential(SqlCredential newCredentials)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionStringHelper.GetConnectionString()))
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
        
        public override dynamic CreateConnection()
        {
            SqlConnection conn;
            try
            {
                conn=CreateConnection(ConnectionStringHelper.GetConnectionString(_useSSPI),_credentials);
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
                    commandText = "USE "+UmanyiSMS.Lib.Properties.Settings.Default.Info.DBName+"\r\nSET DATEFORMAT DMY\r\n" + commandText;
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
                    commandText = "USE " + UmanyiSMS.Lib.Properties.Settings.Default.Info.DBName + "\r\nSET DATEFORMAT DMY\r\n" + commandText;
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

        public override DataTable ExecuteNonQueryWithResultTable(string commandText)
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
                    dtab.CommandText = "USE " + UmanyiSMS.Lib.Properties.Settings.Default.Info.DBName + "\r\nSET DATEFORMAT DMY\r\n" + commandText;
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
                commandText = "USE " + UmanyiSMS.Lib.Properties.Settings.Default.Info.DBName + "\r\nSET DATEFORMAT DMY\r\n" + commandText;
            try
            {
                using (SqlConnection DBConnection = CreateConnection())
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = "USE " + UmanyiSMS.Lib.Properties.Settings.Default.Info.DBName + "\r\nSET DATEFORMAT DMY\r\n" + commandText;
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
                        dta.CommandText = "USE " + UmanyiSMS.Lib.Properties.Settings.Default.Info.DBName + "\r\nSET DATEFORMAT DMY\r\n" + commandText;
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
                commandText = "USE " + UmanyiSMS.Lib.Properties.Settings.Default.Info.DBName + "\r\nSET DATEFORMAT DMY\r\n" + commandText;

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

        public Task<bool> DeleteDb()
        {
            return Task.Factory.StartNew<bool>(() =>
            {
                try
                {
                    string dbName = UmanyiSMS.Lib.Properties.Settings.Default.Info.DBName;
                    string deleteStr = "DECLARE @dbId int\r\n" +
    "DECLARE @isStatAsyncOn bit\r\n" +
    "DECLARE @jobId int\r\n" +
    "DECLARE @sqlString nvarchar(500)\r\n" +

    "SELECT @dbId = database_id,\r\n" +
           "@isStatAsyncOn = is_auto_update_stats_async_on\r\n" +
    "FROM sys.databases\r\n" +
    "WHERE name = '" + dbName + "'\r\n" +

    "IF @isStatAsyncOn = 1\r\n" +
    "BEGIN\r\n" +
        "ALTER DATABASE " + dbName + " SET  AUTO_UPDATE_STATISTICS_ASYNC OFF\r\n" +

        "DECLARE jobsCursor CURSOR FOR\r\n" +
        "SELECT job_id\r\n" +
        "FROM sys.dm_exec_background_job_queue\r\n" +
        "WHERE database_id = @dbId\r\n" +

        "OPEN jobsCursor\r\n" +

        "FETCH NEXT FROM jobsCursor INTO @jobId\r\n" +
        "WHILE @@FETCH_STATUS = 0\r\n" +
        "BEGIN\r\n" +
            "set @sqlString = 'KILL STATS JOB ' + STR(@jobId)\r\n" +
            "EXECUTE sp_executesql @sqlString\r\n" +
            "FETCH NEXT FROM jobsCursor INTO @jobId\r\n" +
        "END\r\n" +

        "CLOSE jobsCursor\r\n" +
        "DEALLOCATE jobsCursor\r\n" +
    "END\r\n" +

    "ALTER DATABASE " + dbName + " SET  SINGLE_USER WITH ROLLBACK IMMEDIATE\r\n" +

    "DROP DATABASE " + dbName;
                    SqlConnection.ClearAllPools();
                    using (SqlConnection DBConnection = CreateConnection(ConnectionStringHelper.GetConnectionString(),null))
                    {
                        SqlCommand dta = new SqlCommand(deleteStr, DBConnection);
                        dta.ExecuteNonQuery();
                        DBConnection.Close();
                        dta.Dispose();
                    }
                    return true;
                }
                catch(Exception e) { Log.E(e.ToString(), null); return false; }
            });
        }

        public Task<bool> CreateBackupAsync(string pathToFile)
        {
            return Task.Factory.StartNew<bool>(() =>
            {
                SqlConnection conn = CreateConnection(ConnectionStringHelper.GetConnectionString(true), null);
                try
                {
                    using (conn)
                    {
                        string dbName = Properties.Settings.Default.Info.DBName;
                        string bkPath = Regex.Match(Properties.Settings.Default.Info.ServerName, "LocalDB", RegexOptions.IgnoreCase).Success ? FileHelper.GetTempFilePath("Bak") : FileHelper.GetNewNetworkServiceTempFilePath("Bak");                    
                     
                        var sc = new ServerConnection(conn);
                        Server server = new Server(sc);
                        Database db = server.Databases[dbName];
                        RecoveryModel recoveryMode = db.DatabaseOptions.RecoveryModel;
                        Backup b = new Backup();
                        b.Action = BackupActionType.Database;
                        b.BackupSetDescription = "Full backup of " + dbName;
                        b.BackupSetName = dbName + " Backup";
                        b.Database = dbName;
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
                        string dbName = UmanyiSMS.Lib.Properties.Settings.Default.Info.Name;
                        string bkFile=FileHelper.GetNewNetworkServiceTempFilePath("Rest");
                        FileSystem.CopyFile(fileName, bkFile, UIOption.AllDialogs, UICancelOption.ThrowException);

                        string alterStr = "USE master\r\n"+
                            "DECLARE @dbId int\r\n" +
    "DECLARE @isStatAsyncOn bit\r\n" +
    "DECLARE @jobId int\r\n" +
    "DECLARE @sqlString nvarchar(500)\r\n" +

    "SELECT @dbId = database_id,\r\n" +
           "@isStatAsyncOn = is_auto_update_stats_async_on\r\n" +
    "FROM sys.databases\r\n" +
    "WHERE name = '" + dbName + "'\r\n" +

    "IF @isStatAsyncOn = 1\r\n" +
    "BEGIN\r\n" +
        "ALTER DATABASE " + dbName + " SET  AUTO_UPDATE_STATISTICS_ASYNC OFF\r\n" +

        "DECLARE jobsCursor CURSOR FOR\r\n" +
        "SELECT job_id\r\n" +
        "FROM sys.dm_exec_background_job_queue\r\n" +
        "WHERE database_id = @dbId\r\n" +

        "OPEN jobsCursor\r\n" +

        "FETCH NEXT FROM jobsCursor INTO @jobId\r\n" +
        "WHILE @@FETCH_STATUS = 0\r\n" +
        "BEGIN\r\n" +
            "set @sqlString = 'KILL STATS JOB ' + STR(@jobId)\r\n" +
            "EXECUTE sp_executesql @sqlString\r\n" +
            "FETCH NEXT FROM jobsCursor INTO @jobId\r\n" +
        "END\r\n" +

        "CLOSE jobsCursor\r\n" +
        "DEALLOCATE jobsCursor\r\n" +
    "END\r\n" +
    "USE master\r\n" +
    "ALTER DATABASE UmanyiSMS SET  SINGLE_USER\r\n"+
    "RESTORE DATABASE [UmanyiSMS] FROM  DISK = N'" + bkFile + "' WITH  FILE = 1,  NOUNLOAD,  STATS = 10";
                        
                        SqlConnection.ClearAllPools();
                        ExecuteNonQuery(alterStr, false);
                    
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
                    string commandText = "USE " + UmanyiSMS.Lib.Properties.Settings.Default.Info.DBName + "\r\nSET DATEFORMAT DMY\r\n";
                   
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

        internal async Task SetOffline()
        {
            await  
             Task.Factory.StartNew(() =>
            {
                try
                {
                    string commandText = "ALTER DATABASE UmanyiSMS SET OFFLINE";
                    using (SqlConnection DBConnection = CreateConnection(ConnectionStringHelper.GetConnectionString(true),null))
                    {
                        SqlCommand dta = new SqlCommand(commandText, DBConnection);
                        dta.ExecuteNonQuery();
                        dta.Dispose();
                    }
                }
                catch (Exception e)
                {
                    Log.E(e.ToString(), null);
                }
            });
        }

        public static bool IsServerMachine
        { get { return GetIsServerMachine(); } }

        private static bool GetIsServerMachine()
        {
            if (Properties.Settings.Default.Info.ServerName.ToLowerInvariant().Contains("localdb"))
                return true;
            return Properties.Settings.Default.Info.ServerName.ToLowerInvariant() == (Environment.MachineName + @"\Umanyi").ToLowerInvariant();
        }

        public override List<string> CopyFirstColumnToList(string commandText)
        {
            List<string> result = new List<string>();
                commandText = "USE " + UmanyiSMS.Lib.Properties.Settings.Default.Info.DBName + "\r\nSET DATEFORMAT DMY\r\n" + commandText;
            try
            {
                using (SqlConnection DBConnection = CreateConnection())
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = "USE " + UmanyiSMS.Lib.Properties.Settings.Default.Info.DBName + "\r\nSET DATEFORMAT DMY\r\n" + commandText;
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
                    dtab.CommandText = "USE " + UmanyiSMS.Lib.Properties.Settings.Default.Info.DBName + "\r\nSET DATEFORMAT DMY\r\n" + commandText;
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
