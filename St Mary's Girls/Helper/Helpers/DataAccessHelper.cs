using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
namespace Helper
{
    public static class DataAccessHelper
    {
        public static Task<DataTable> ExecuteQueryWithResultAsync(string query)
        {
            return Task.Run<DataTable>(() =>
            {
                SqlConnection conn = DataAccessHelper.CreateConnection();
                try
                {
                    DataTable succ;
                    using (conn)
                    {
                        var sc = new ServerConnection(conn);
                        Server server = new Server(sc);
                        succ = server.ConnectionContext.ExecuteWithResults(query).Tables == null ? new DataTable() : server.ConnectionContext.ExecuteWithResults(query).Tables[0];
                    }
                    return succ;
                }
                catch (Exception e)
                {
                    DataTable dt= new DataTable();
                    dt.Columns.Add(new DataColumn("Error"));
                    dt.Columns.Add(new DataColumn("Exception Details"));
                    DataRow dtr = dt.NewRow();
                    dtr[0] = e.Message;
                    dtr[1] = e.ToString();
                    dt.Rows.Add(dtr);
                    return dt;
                }
            });
        }
        
        public static Task<DataTable> ExecuteQueryAsync(string query)
        {
            return Task.Run<DataTable>(() =>
            {
                SqlConnection conn = DataAccessHelper.CreateConnection();
                try
                {
                    DataTable succ =new DataTable();
                    succ.Columns.Add(new DataColumn("",typeof(string)));
                    using (conn)
                    {
                        var sc = new ServerConnection(conn);
                        Server server = new Server(sc);
                        int no = server.ConnectionContext.ExecuteNonQuery(query);
                        var dtr = succ.NewRow();
                        dtr[0] = "Affected Rows: " + no;
                        succ.Rows.Add(dtr);
                    }
                    return succ;
                }
                catch (Exception e)
                {
                    DataTable dt = new DataTable();
                    dt.Columns.Add(new DataColumn("Error"));
                    dt.Columns.Add(new DataColumn("Exception Details"));
                    DataRow dtr = dt.NewRow();
                    dtr[0] = e.Message;
                    dtr[1] = e.ToString();
                    dt.Rows.Add(dtr);
                    return dt;
                }
            });
        }

        internal static bool TestCredential(SqlCredential newCredentials)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionStringHelper.ConnectionString))
                {
                    conn.Credential = newCredentials;
                    conn.Open();
                }
                return true;
            }
            catch
            { return false; }
        }

        static SqlCredential Credentials { get; set; }

        internal static void SetCredential(SqlCredential newCredentials)
        {
            Credentials = newCredentials;
        }

        public static SqlConnection CreateConnection()
        {
            SqlConnection conn;
            try
            {
                conn = CreateConnection(Credentials, ConnectionStringHelper.ConnectionString);
            }
            catch (Exception e)
            { throw e; }
            return conn;
        }

        internal static SqlConnection CreateConnection(string connectionString)
        {
            SqlConnection conn;
            try
            {
                conn = CreateConnection(Credentials, connectionString);
            }
            catch (Exception e)
            { throw e; }
            return conn;
        }

        internal static SqlConnection CreateConnection(SqlCredential cred, string connectionString)
        {
            SqlConnection conn;
            try
            {
                conn = new SqlConnection(connectionString);
                conn.Credential = cred;
                conn.Open();
                if (conn.State == ConnectionState.Connecting)
                    while (conn.State != ConnectionState.Open)
                    { }
            }
            catch (Exception e)
            { throw e; }
            return conn;
        }

        public static string ExecuteScalar(string commandText)
        {
            return ExecuteScalar(commandText, true);
        }

        public static string ExecuteScalar(string commandText, bool hasHeader)
        {
            object res = ExecuteObjectScalar(commandText, hasHeader);
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
                    commandText = "USE Starehe\r\nSET DATEFORMAT DMY\r\n" + commandText;
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
                throw e;
            }
        }

        public static ObservableCollection<string> CopyFromDBtoObservableCollection(string selectString)
        {
            try
            {
                ObservableCollection<string> res = new ObservableCollection<string>();
                DataTable tb = DataAccessHelper.ExecuteNonQueryWithResultTable(selectString);
                foreach (DataRow dtr in tb.Rows)
                {
                    res.Add(dtr[0].ToString());
                }
                return res;
            }
            catch
            {
            }
            return new ObservableCollection<string>();
        }

        public static DataTable ExecuteNonQueryWithParametersWithResultTable(string commandText,
            ObservableCollection<SqlParameter> paramColl)
        {
            DataTable result = new DataTable();

            try
            {
                using (SqlConnection DBConnection = CreateConnection())
                {
                    SqlCommand dtab = new SqlCommand();
                    dtab.CommandText = "USE Starehe\r\nSET DATEFORMAT DMY\r\n" + commandText;
                    dtab.Connection = DBConnection;
                    foreach (SqlParameter param in paramColl)
                    { dtab.Parameters.Add(param); }

                    SqlDataReader reader = dtab.ExecuteReader();
                    result = GetResultTable(ref reader);
                    reader.Close();
                    dtab.Dispose();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return result;
        }

        private static DataTable GetResultTable(ref SqlDataReader reader)
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

        public static bool ExecuteNonQueryWithParameters(string commandText, ObservableCollection<SqlParameter> paramColl)
        {
            bool result = false;

            try
            {
                using (SqlConnection DBConnection = CreateConnection())
                {
                    SqlCommand dta = new SqlCommand();
                    dta.CommandText = "USE Starehe\r\nSET DATEFORMAT DMY\r\n" + commandText;
                    dta.Connection = DBConnection;
                    foreach (SqlParameter param in paramColl)
                    { dta.Parameters.Add(param); }
                    dta.ExecuteNonQuery();
                    dta.Dispose();
                }
                result = true;
            }
            catch (Exception e)
            {
                throw e;
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
                commandText = "USE Starehe\r\nSET DATEFORMAT DMY\r\n" + commandText;
            try
            {
                using (SqlConnection DBConnection = CreateConnection())
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = "USE Starehe\r\nSET DATEFORMAT DMY\r\n" + commandText;
                    cmd.Connection = DBConnection;

                    cmd.ExecuteNonQuery();
                    SqlDataReader reader = cmd.ExecuteReader();

                    result = GetResultTable(ref reader);
                    reader.Close();
                    cmd.Dispose();
                }
            }
            catch (Exception e)
            {
                throw e;
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
                commandText = "USE Starehe\r\nSET DATEFORMAT DMY\r\n" + commandText;

            try
            {
                using (SqlConnection DBConnection = CreateConnection())
                {
                    SqlCommand dta = new SqlCommand(commandText, DBConnection);
                    dta.ExecuteNonQuery();
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

        public static Task<bool> TestDb()
        {
            return Task.Run<bool>(() =>
            {
                return DataAccessHelper.ExecuteNonQuery("");
            });
        }

        public static Task<bool> ClearDb()
        {
            return Task.Run<bool>(() =>
            {
                string deleteStr =
                    "DELETE FROM [Institution].[Class]\r\n" +
                    "DELETE FROM [Institution].[ClassSetupDetail]\r\n" +
                    "DELETE FROM [Institution].[ClassSetupHeader]\r\n" +
                    "DELETE FROM [Institution].[Dormitory]\r\n" +
                    "DELETE FROM [Institution].[Event]\r\n" +
                    "DELETE FROM [Institution].[ExamDetail]\r\n" +
                    "DELETE FROM [Institution].[ExamHeader]\r\n" +
                    "DELETE FROM [Institution].[ExamResultDetail]\r\n" +
                    "DELETE FROM [Institution].[ExamResultHeader]\r\n" +
                    "DELETE FROM [Institution].[FeesPayment]\r\n" +
                    "DELETE FROM [Institution].[FeesStructureDetail]\r\n" +
                    "DELETE FROM [Institution].[FeesStructureHeader]\r\n" +
                    "DELETE FROM [Institution].[Gallery]\r\n" +
                    "DELETE FROM [Institution].[LeavingCertificate]\r\n" +
                    "DELETE FROM [Institution].[Staff]\r\n" +
                    "DELETE FROM [Institution].[Student]\r\n" +
                    "DELETE FROM [Institution].[StudentTransfer]\r\n" +
                    "DELETE FROM [Institution].[Subject]\r\n" +
                    "DELETE FROM [Institution].[SubjectSetupDetail]\r\n" +
                    "DELETE FROM [Institution].[SubjectSetupHeader]\r\n" +
                    "DELETE FROM [Institution].[TimeTableDetail]\r\n" +
                    "DELETE FROM [Institution].[TimeTableHeader]\r\n" +
                    "DELETE FROM [Sales].[Item]\r\n" +
                    "DELETE FROM [Sales].[ItemCategory]\r\n" +
                    "DELETE FROM [Sales].[ItemReceiptDetail]\r\n" +
                    "DELETE FROM [Sales].[ItemReceiptHeader]\r\n" +
                    "DELETE FROM [Sales].[SaleDetail]\r\n" +
                    "DELETE FROM [Sales].[SaleHeader]\r\n" +
                    "DELETE FROM [Sales].[Supplier]\r\n" +
                    "DELETE FROM [Sales].[SupplierDetail]\r\n" +
                    "DELETE FROM [Sales].[StockTakingDetail]\r\n" +
                    "DELETE FROM [Sales].[StockTakingHeader]\r\n" +
                    "DELETE FROM [Sales].[SupplierDetail]\r\n" +
                    "DELETE FROM [Sales].[SupplierPayment]\r\n" +
                    "DELETE FROM [Sales].[Vat]\r\n" +
                    "DELETE FROM [Users].[User]\r\n" +
                    "DELETE FROM [Users].[UserDetail]\r\n" +
                    "DELETE FROM [Users].[UserRole]\r\n" +
                    "exec dbo.ResetUniqueIDs";
                return DataAccessHelper.ExecuteNonQuery(deleteStr);
            });
        }

        public static Task<bool> DeleteDb()
        {
            return Task.Run<bool>(() =>
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
                    SqlConnection.ClearAllPools();
                    using (SqlConnection DBConnection = CreateConnection(ConnectionStringHelper.MasterConnectionString))
                    {
                        SqlCommand dta = new SqlCommand(deleteStr, DBConnection);
                        dta.ExecuteNonQuery();
                        dta.Dispose();
                    }
                    return true;
                }
                catch { return false; }
            });
        }

        public static Task<bool> CreateBackupAsync(string pathToFile)
        {
            return Task.Run<bool>(() =>
            {
                SqlConnection conn = DataAccessHelper.CreateConnection();
                try
                {
                    using (conn)
                    {
                        string dbName = Helper.Properties.Settings.Default.DBName;
                        var sc = new ServerConnection(conn);
                        Server server = new Server(sc);
                        Database db = server.Databases[dbName];
                        RecoveryModel recoveryMode = db.DatabaseOptions.RecoveryModel;
                        Backup b = new Backup();
                        b.Action = BackupActionType.Database;
                        b.BackupSetDescription = "Full backup of " + dbName;
                        b.BackupSetName = dbName + " Backup";
                        b.Database = dbName;
                        b.Devices.AddDevice(pathToFile, DeviceType.File);
                        b.Incremental = false;
                        b.SqlBackup(server);
                    }
                    return true;
                }
                catch { return false; }
            });
        }

        public static Task<bool> RestoreDb(string fileName)
        {
            return Task.Run<bool>(() =>
            {
                SqlConnection conn = DataAccessHelper.CreateConnection(ConnectionStringHelper.MasterConnectionString);
                try
                {
                    using (conn)
                    {
                        var sc = new ServerConnection(conn);
                        Server server = new Server(sc);
                        Restore res = new Restore();

                        string databaseName = Helper.Properties.Settings.Default.DBName;

                        res.Database = databaseName;
                        res.Action = RestoreActionType.Database;
                        res.Devices.AddDevice(fileName, DeviceType.File);

                        res.ReplaceDatabase = true;

                        res.SqlRestore(server);
                    }
                    return true;
                }
                catch
                {
                    return false;
                }
            });
        }
    }
}
