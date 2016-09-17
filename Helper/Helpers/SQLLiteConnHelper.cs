using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Data.SQLite;
using System.Data.SqlClient;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.VisualBasic.FileIO;

namespace Helper.Helpers
{
    public static class SQLLiteConnHelper
    {
        /*
        internal static bool TestCredential(SqlCredential newCredentials)
        {
            try
            {

                using (SQLiteConnection conn = new SQLiteConnection(ConnectionStringHelper.GetConnectionString(newCredentials)))
                {
                    conn.Open();
                }


                return true;
            }
            catch (Exception e)
            {
                Log.E(e.ToString(), typeof(SQLLiteConnHelper));
                return false;
            }
        }

        static SqlCredential Credentials { get; set; }

        internal static void SetCredential(SqlCredential newCredentials)
        {
            Credentials = newCredentials;
        }

        public static SQLiteConnection CreateConnection()
        {
            SQLiteConnection conn;
            try
            {
                conn = CreateConnection(Credentials, ConnectionStringHelper.ConnectionString);
            }
            catch (Exception e)
            {
                Log.E(e.ToString(), typeof(SQLLiteConnHelper));
                throw;
            }
            return conn;
        }

        public static SQLiteConnection CreateConnection(bool isMaster)
        {
            SQLiteConnection conn;
            try
            {
                conn = CreateConnection(Credentials, isMaster ? ConnectionStringHelper.MasterConnectionString : ConnectionStringHelper.ConnectionString);
            }
            catch (Exception e)
            {
                Log.E(e.ToString(), typeof(SQLLiteConnHelper));
                throw;
            }
            return conn;
        }

        internal static SQLiteConnection CreateConnection(string connectionString)
        {
            SQLiteConnection conn;
            try
            {
                conn = CreateConnection(Credentials, connectionString);
            }
            catch (Exception e)
            {
                Log.E(e.ToString(), typeof(SQLLiteConnHelper));
                throw;
            }
            return conn;
        }

        internal static SQLiteConnection CreateConnection(SqlCredential cred, string connectionString)
        {
            SQLiteConnection conn;
            try
            {
                conn = new SQLiteConnection(ConnectionStringHelper.GetConnectionString(cred, connectionString));


                conn.Open();
                if (conn.State == ConnectionState.Connecting)
                    while (conn.State != ConnectionState.Open)
                    { }
            }
            catch (Exception e)
            {
                Log.E(e.ToString(), typeof(SQLLiteConnHelper));
                throw;
            }
            return conn;
        }

        public static string ExecuteScalar(string commandText)
        {
            return ExecuteScalar(commandText, true);
        }

        public static string ExecuteScalar(string commandText, ObservableCollection<SQLiteParameter> paramColl)
        {
            return ExecuteScalar(commandText, paramColl, true);
        }

        public static string ExecuteScalar(string commandText, bool hasHeader)
        {
            object res = ExecuteObjectScalar(commandText, hasHeader);
            if (res != null)
                return res.ToString();
            else return "";
        }
        public static string ExecuteScalar(string commandText, ObservableCollection<SQLiteParameter> paramColl, bool hasHeader)
        {
            object res = ExecuteObjectScalar(commandText, paramColl, hasHeader);
            if (res != null)
                return res.ToString();
            else return "";
        }

        public static object ExecuteObjectScalar(string commandText, bool hasHeader)
        {
            try
            {
                object tx;
                if (hasHeader)
                    commandText = "USE " + Helper.Properties.Settings.Default.DBName + "\r\nSET DATEFORMAT DMY\r\n" + commandText;
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
                Log.E(e.ToString(), typeof(SQLLiteConnHelper));
                throw;
            }
        }

        public static object ExecuteObjectScalar(string commandText, ObservableCollection<SQLiteParameter> paramColl, bool hasHeader)
        {
            try
            {
                object tx;
                if (hasHeader)
                    commandText = "USE " + Helper.Properties.Settings.Default.DBName + "\r\nSET DATEFORMAT DMY\r\n" + commandText;
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
                Log.E(e.ToString(), typeof(SQLLiteConnHelper));
                throw;
            }
        }

        public static ObservableCollection<string> CopyFromDBtoObservableCollection(string selectString)
        {
            try
            {
                ObservableCollection<string> res = new ObservableCollection<string>();
                DataTable tb = SQLLiteConnHelper.ExecuteNonQueryWithResultTable(selectString);
                foreach (DataRow dtr in tb.Rows)
                {
                    res.Add(dtr[0].ToString());
                }
                return res;
            }
            catch (Exception e)
            {
                Log.E(e.ToString(), typeof(SQLLiteConnHelper));
            }
            return new ObservableCollection<string>();
        }

        public static DataTable ExecuteNonQueryWithParametersWithResultTable(string commandText,
            ObservableCollection<SQLiteParameter> paramColl)
        {
            DataTable result = new DataTable();

            try
            {
                using (SQLiteConnection DBConnection = CreateConnection())
                {
                    SQLiteCommand dtab = new SQLiteCommand();
                    dtab.CommandText = "USE " + Helper.Properties.Settings.Default.DBName + "\r\nSET DATEFORMAT DMY\r\n" + commandText;
                    dtab.Connection = DBConnection;
                    foreach (SQLiteParameter param in paramColl)
                    { dtab.Parameters.Add(param); }

                    SQLiteDataReader reader = dtab.ExecuteReader();
                    result = GetResultTable(ref reader);
                    reader.Close();
                    dtab.Dispose();
                }
            }
            catch (Exception e)
            {
                Log.E(e.ToString(), typeof(SQLLiteConnHelper));
                throw;
            }
            return result;
        }

        private static DataTable GetResultTable(ref SQLiteDataReader reader)
        {
            DataTable dt = new DataTable();
            if (!reader.IsClosed)
                if (reader.HasRows)
                {
                    int colIndex = reader.FieldCount;
                    for (int i = 0; i < colIndex; i++)
                    {
                        dt.Columns.Add(new DataColumn("", typeof(object)));
                    }
                    DataRow dtr;
                    while (reader.Read())
                    {
                        dtr = dt.NewRow();
                        for (int i = 0; i < colIndex; i++)
                            dtr[i] = reader.GetValue(i);
                        dt.Rows.Add(dtr);
                    }
                }
            return dt;
        }

        public static bool ExecuteNonQueryWithParameters(string commandText, ObservableCollection<SQLiteParameter> paramColl)
        {
            bool result = false;

            try
            {
                using (SQLiteConnection DBConnection = CreateConnection())
                {
                    using (SQLiteCommand dta = new SQLiteCommand())
                    {
                        dta.CommandText = "USE " + Helper.Properties.Settings.Default.DBName + "\r\nSET DATEFORMAT DMY\r\n" + commandText;
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
                Log.E(e.ToString(), typeof(SQLLiteConnHelper));
                throw;
            }
            return result;
        }

        public static DataTable ExecuteNonQueryWithResultTable(string commandText)
        {
            return ExecuteNonQueryWithResultTable(commandText, true);
        }

        public static DataTable ExecuteNonQueryWithResultTable(string commandText, bool hasHeader)
        {
            DataTable result = new DataTable();
            if (hasHeader)
                commandText = "USE " + Helper.Properties.Settings.Default.DBName + "\r\nSET DATEFORMAT DMY\r\n" + commandText;
            try
            {
                using (SQLiteConnection DBConnection = CreateConnection())
                {
                    SQLiteCommand cmd = new SQLiteCommand();
                    cmd.CommandText = "USE " + Helper.Properties.Settings.Default.DBName + "\r\nSET DATEFORMAT DMY\r\n" + commandText;
                    cmd.Connection = DBConnection;

                    cmd.ExecuteNonQuery();
                    SQLiteDataReader reader = cmd.ExecuteReader();

                    result = GetResultTable(ref reader);
                    reader.Close();
                    DBConnection.Close();
                    cmd.Dispose();
                }
            }
            catch (Exception e)
            {
                Log.E(e.ToString(), typeof(SQLLiteConnHelper));
                throw;
            }
            return result;
        }

        public static bool ExecuteNonQuery(string commandText)
        {
            return ExecuteNonQuery(commandText, true);
        }

        public static bool ExecuteNonQuery(string commandText, bool hasHeader)
        {
            bool succ = false;
            if (hasHeader)
                commandText = "USE " + Helper.Properties.Settings.Default.DBName + "\r\nSET DATEFORMAT DMY\r\n" + commandText;

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
                Log.E(e.ToString(), typeof(SQLLiteConnHelper));
                throw;
            }
            return succ;

        }

        public static Task<bool> TestDb()
        {
            return Task.Factory.StartNew<bool>(() =>
            {
                return SQLLiteConnHelper.ExecuteNonQuery("");
            });
        }

        public static Task<bool> TestDb(string connectionStr)
        {
            return Task.Factory.StartNew<bool>(() =>
            {
                bool succ = false;
                try
                {
                    using (SQLiteConnection DBConnection = new SQLiteConnection(ConnectionStringHelper.GetConnectionString(Credentials, connectionStr)))
                    {
                        DBConnection.Open();
                        SQLiteCommand dta = new SQLiteCommand("USE " + Helper.Properties.Settings.Default.DBName + "", DBConnection);
                        dta.ExecuteNonQuery();
                        DBConnection.Close();
                        dta.Dispose();
                    }
                    succ = true;
                }
                catch (Exception e)
                {
                    Log.E(e.ToString(), typeof(SQLLiteConnHelper));
                }
                return succ;
            });
        }
        
        public static Task<bool> ClearDb()
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

        public static Task<bool> DeleteDb()
        {
            return Task.Factory.StartNew<bool>(() =>
            {
                try
                {
                    string dbName = Helper.Properties.Settings.Default.DBName;
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
                    SQLiteConnection.ClearAllPools();
                    using (SQLiteConnection DBConnection = CreateConnection(ConnectionStringHelper.MasterConnectionString))
                    {
                        SQLiteCommand dta = new SQLiteCommand(deleteStr, DBConnection);
                        dta.ExecuteNonQuery();
                        DBConnection.Close();
                        dta.Dispose();
                    }
                    return true;
                }
                catch (Exception e) { Log.E(e.ToString(), typeof(SQLLiteConnHelper)); return false; }
            });
        }

        public static Task<bool> CreateBackupAsync(string pathToFile)
        {
            return Task.Factory.StartNew<bool>(() =>
            {
                SQLiteConnection conn = CreateConnection(true);
                try
                {
                    using (conn)
                    {
                        string dbName = Helper.Properties.Settings.Default.DBName;
                        string bkPath = FileHelper.GetNewNetworkServiceTempFilePath("Bak");
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
                catch (Exception e)
                {
                    MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    Log.E(e.ToString(), typeof(SQLLiteConnHelper));
                    return false;
                }
            });
        }

        public static Task<bool> RestoreDb(string fileName)
        {
            return Task.Factory.StartNew<bool>(() =>
            {
                if (string.IsNullOrWhiteSpace(fileName))
                    return false;
                try
                {
                    string dbName = Helper.Properties.Settings.Default.DBName;
                    string bkFile = FileHelper.GetNewNetworkServiceTempFilePath("Rest");
                    FileSystem.CopyFile(fileName, bkFile, UIOption.AllDialogs, UICancelOption.ThrowException);

                    string alterStr = "USE master\r\n" +
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
"ALTER DATABASE UmanyiSMS SET  SINGLE_USER\r\n" +
"RESTORE DATABASE [UmanyiSMS] FROM  DISK = N'" + bkFile + "' WITH  FILE = 1,  NOUNLOAD,  STATS = 10";

                    SQLiteConnection.ClearAllPools();
                    SQLLiteConnHelper.ExecuteNonQuery(alterStr, false);

                    return true;
                }
                catch (Exception e)
                {
                    Log.E(e.ToString(), typeof(SQLLiteConnHelper));
                    return false;
                }
            });
        }

        internal static Task<bool> TestBackupFile(string fileName)
        {
            string error;
            return Task.Factory.StartNew<bool>(() =>
            {
                try
                {
                    bool verifySuccessful = false;
                    SQLiteConnection conn = SQLLiteConnHelper.CreateConnection();

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

        public static Task<bool> CleanDb()
        {
            return Task.Factory.StartNew<bool>(() =>
            {
                try
                {
                    string commandText = "USE " + Helper.Properties.Settings.Default.DBName + "\r\nSET DATEFORMAT DMY\r\n";
                    commandText += "DELETE FROM [Institution].[ClassSetupDetail] WHERE ClassSetupID IN (SELECT ClassSetupID FROM [Institution].[ClassSetupHeader] WHERE IsActive=0)\r\n" +
                        "DELETE FROM [Institution].[ClassSetupHeader] WHERE IsActive=0\r\n" +
                        "DELETE FROM [Institution].[ExamResultDetail] WHERE ExamResultID IN (SELECT ExamResultID FROM [Institution].[ExamResultHeader] WHERE IsActive=0)\r\n" +
                        "DELETE FROM [Institution].[ExamResultHeader] WHERE IsActive=0\r\n" +
                        "DELETE FROM [Institution].[FeesStructureDetail] WHERE FeesStructureID IN (SELECT FeesStructureID FROM [Institution].[FeesStructureHeader] WHERE IsActive=0)\r\n" +
                        "DELETE FROM [Institution].[FeesStructureHeader] WHERE IsActive=0\r\n" +
                        "DELETE FROM [Institution].[StudentSubjectSelectionDetail] WHERE StudentSubjectSelectionID IN (SELECT StudentSubjectSelectionID FROM [Institution].[StudentSubjectSelectionHeader] WHERE IsActive=0)\r\n" +
                        "DELETE FROM [Institution].[StudentSubjectSelectionHeader] WHERE IsActive=0\r\n" +
                        "DELETE FROM [Institution].[SubjectSetupDetail] WHERE SubjectSetupID IN (SELECT SubjectSetupID FROM [Institution].[SubjectSetupHeader] WHERE IsActive=0)\r\n" +
                        "DELETE FROM [Institution].[SubjectSetupHeader] WHERE IsActive=0\r\n" +
                        "DELETE FROM [Institution].[TimeTableDetail] WHERE TimeTableID IN (SELECT TimeTableID FROM [Institution].[TimeTableHeader] WHERE IsActive=0)\r\n" +
                        "DELETE FROM [Institution].[TimeTableHeader] WHERE IsActive=0\r\n" +
                        "DELETE FROM [Institution].[TimeTableSettings] WHERE IsActive=0\r\n";
                    bool succ = false;

                    int y = 0;
                    try
                    {
                        using (SQLiteConnection DBConnection = CreateConnection())
                        {
                            SQLiteCommand dta = new SQLiteCommand(commandText, DBConnection);
                            y = dta.ExecuteNonQuery();
                            dta.Dispose();
                        }
                        succ = true;
                        MessageBox.Show("Successfully removed " + y + " records.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    catch (Exception e)
                    {
                        Log.E(e.ToString(), typeof(SQLLiteConnHelper));
                        throw;
                    }
                    return succ;
                }
                catch (Exception e) { Log.E(e.ToString(), typeof(SQLLiteConnHelper)); return false; }
            });
        }

        internal async static Task SetOffline()
        {
            await
             Task.Factory.StartNew(() =>
             {
                 try
                 {
                     string commandText = "ALTER DATABASE UmanyiSMS SET OFFLINE";
                     using (SQLiteConnection DBConnection = CreateConnection(null, ConnectionStringHelper.Win32ConnectionString))
                     {
                         SQLiteCommand dta = new SQLiteCommand(commandText, DBConnection);
                         dta.ExecuteNonQuery();
                         dta.Dispose();
                     }
                 }
                 catch (Exception e)
                 {
                     Log.E(e.ToString(), typeof(SQLLiteConnHelper));
                 }
             });
        }
        */
    }
}
