using System;
using System.Linq;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Windows.Input;
using UmanyiSMS.Lib;
using UmanyiSMS.Lib.Controllers;
using UmanyiSMS.Lib.Presentation;
using System.Diagnostics;

namespace DataTransfer
{
    public class MainWindowVM : ViewModelBase
    {
        private object source;
        private string server2;
        private string server1;
        private int progress;
        SqlServerHelper sourceServer = null;
        SqlServerHelper targetServer = null;
        private string progressText;
        List<BasicPair<string, string>> errorTables = new List<BasicPair<string, string>>();
        public MainWindowVM()
        {
            InitVars();
            CreateCommands();
        }

        public override void Reset()
        {

        }

        protected override void CreateCommands()
        {
            StartCommand = new RelayCommand(async o =>
            {
                IsBusy = true;
                var succ=await TransferData();
                if (succ)
                    Source = new SuccessVM();
                else
                    Source = new ErrorVM();
                IsBusy = false;
            }, o => !IsBusy);

            SaveCommand = new RelayCommand(o =>
            {
                Source = new Page1VM();
            }, o => true);

            LogCommand = new RelayCommand(o =>
            {
                try
                {
                    Process.Start(App.LogFilePath);
                }
                catch { }
            }, o => true);

            Page2Command = new RelayCommand(o =>
                {
                    Source = new Page2VM();
                }, o => true);

            Page1Command = new RelayCommand(o =>
            {
                Source = new Page1VM();
            }, o => true);
        }

        public async Task<bool> TransferData()
        {
            Progress = 12;
            ProgressText = "Initializing...";
            Log.I(progressText, null);
            sourceServer = SqlServerHelper.CreateInstance(server1, "UmanyiSMS", null, true);
            targetServer = SqlServerHelper.CreateInstance(server2, "UmanyiSMS", null, true);
            Progress = 15;
            ProgressText = "Testing servers...";
            Log.I(progressText, null);
            var s1 = TestServer1();
            var s2 = TestServer2();
            Task.WaitAll(s1, s2);
            Progress = 18;
            if (!(s1.Result && s2.Result))
            {
                Progress = 100;
                return false;
            }
            ProgressText = "Servers OK.";
            Log.I(progressText, null);
            DataTable dt = null;
            bool succ = true;

            List<BasicPair<string, string>> allTables = new List<BasicPair<string, string>>();
            allTables.Add(new BasicPair<string, string>("[Institution].[Book]", "[dbo].[Book]"));
            allTables.Add(new BasicPair<string, string>("[Institution].[BookIssueDetail]", "[dbo].[BookIssueDetail]"));
            allTables.Add(new BasicPair<string, string>("[Institution].[BookIssueHeader]", "[dbo].[BookIssueHeader]"));
            allTables.Add(new BasicPair<string, string>("[Institution].[BookReturnDetail]", "[dbo].[BookReturnDetail]"));
            allTables.Add(new BasicPair<string, string>("[Institution].[BookReturnHeader]", "[dbo].[BookReturnHeader]"));
            allTables.Add(new BasicPair<string, string>("[Institution].[Class]", "[dbo].[Class]"));
            allTables.Add(new BasicPair<string, string>("[Institution].[ExamClassDetail]", "[dbo].[ExamClassDetail]"));
            
            
            allTables.Add(new BasicPair<string, string>("[Institution].[ExamDetail]", "[dbo].[ExamDetail]"));
            allTables.Add(new BasicPair<string, string>("[Institution].[ExamHeader]", "[dbo].[ExamHeader]"));
            allTables.Add(new BasicPair<string, string>("[Institution].[ExamResultDetail]", "[dbo].[ExamResultDetail]"));
            allTables.Add(new BasicPair<string, string>("[Institution].[ExamResultHeader]", "[dbo].[ExamResultHeader]"));
            allTables.Add(new BasicPair<string, string>("[Institution].[ExamStudentDetail]", "[dbo].[ExamStudentDetail]"));
            allTables.Add(new BasicPair<string, string>("[Institution].[FeesPayment]", "[dbo].[FeesPayment]"));
            
            
            allTables.Add(new BasicPair<string, string>("[Institution].[FeesStructureDetail]", "[dbo].[FeesStructureDetail]"));
            allTables.Add(new BasicPair<string, string>("[Institution].[FeesStructureHeader]", "[dbo].[FeesStructureHeader]"));
            allTables.Add(new BasicPair<string, string>("[Sales].[Item]", "[dbo].[Item]"));
            allTables.Add(new BasicPair<string, string>("[Sales].[ItemCategory]", "[dbo].[ItemCategory]"));
            allTables.Add(new BasicPair<string, string>("[Sales].[ItemReceiptDetail]", "[dbo].[ItemReceiptDetail]"));
            allTables.Add(new BasicPair<string, string>("[Sales].[ItemReceiptHeader]", "[dbo].[ItemReceiptHeader]"));
            
            allTables.Add(new BasicPair<string, string>("[Institution].[LeavingCertificate]", "[dbo].[LeavingCertificate]"));
            allTables.Add(new BasicPair<string, string>("[Sales].[SaleDetail]", "[dbo].[SaleDetail]"));
            allTables.Add(new BasicPair<string, string>("[Sales].[SaleHeader]", "[dbo].[SaleHeader]"));
            allTables.Add(new BasicPair<string, string>("[Institution].[Settings]", "[dbo].[Settings]"));
            allTables.Add(new BasicPair<string, string>("[Institution].[Staff]", "[dbo].[Staff]"));
            allTables.Add(new BasicPair<string, string>("[Institution].[Student]", "[dbo].[Student]"));
            allTables.Add(new BasicPair<string, string>("[Institution].[StudentClass]", "[dbo].[StudentClass]"));
            allTables.Add(new BasicPair<string, string>("[Institution].[StudentClearance]", "[dbo].[StudentClearance]"));
            allTables.Add(new BasicPair<string, string>("[Institution].[StudentSubjectSelectionDetail]", "[dbo].[StudentSubjectSelectionDetail]"));
            allTables.Add(new BasicPair<string, string>("[Institution].[StudentSubjectSelectionHeader]", "[dbo].[StudentSubjectSelectionHeader]"));
            allTables.Add(new BasicPair<string, string>("[Institution].[StudentTranscriptDetail]", "[dbo].[StudentTranscriptDetail]"));
            allTables.Add(new BasicPair<string, string>("[Institution].[StudentTranscriptExamDetail]", "[dbo].[StudentTranscriptExamDetail]"));
            allTables.Add(new BasicPair<string, string>("[Institution].[StudentTranscriptHeader]", "[dbo].[StudentTranscriptHeader]"));
            allTables.Add(new BasicPair<string, string>("[Institution].[Subject]", "[dbo].[Subject]"));
            allTables.Add(new BasicPair<string, string>("[Sales].[Supplier]", "[dbo].[Supplier]"));
            allTables.Add(new BasicPair<string, string>("[Sales].[SupplierPayment]", "[dbo].[SupplierPayment]"));

            int i = 0;
            foreach (var current in allTables)
            {
                bool y = false;
                Log.I(progressText, null);
                Progress = (int)decimal.Ceiling(((decimal)i / (decimal)allTables.Count) * 100m);
                i++;
                if (CompareTableColumns(current.Key, current.Value))
                {
                    dt = await FetchTableData(current.Key);
                    y = await InsertData(current.Value, dt);
                    succ = succ && y;
                }
                else
                {
                    y = await AdjustAndInsertData(current.Key, current.Value);
                    succ = succ && y;
                }
                ProgressText = "Table: " + current.Value + " Succeeded:" + y;
            }
            foreach (var t in errorTables)
                Log.E("Table :" + t.Key + ", Error:" + t.Value,null);
            return succ;
        }

        private List<string> GetTableColumns(DBHelper server, string tableName)
        {
            return server.CopyFirstColumnToList(" SELECT name FROM sys.columns WHERE Object_id = OBJECT_ID('" + tableName + "')");
        }

        private bool GetTableColumnCanInsertNull(DBHelper server, string tableName, string columnName)
        {
            var t = server.ExecuteScalar("SELECT is_nullable FROM sys.columns WHERE Object_id = OBJECT_ID('" + tableName + "') AND LOWER(name)=LOWER('" + columnName + "')");
            return t == "1";
        }

        private async Task<bool> AdjustAndInsertData(string sourceTableName, string targetTableName)
        {
            var t = GetTableColumns(targetServer, targetTableName);
            var s = GetTableColumns(sourceServer, sourceTableName);
            bool targetIsSubset = true;
            bool targetIsSuperset = true;
            foreach (var n in s)
                targetIsSuperset = targetIsSuperset && (t.Any(o => o.Trim().Equals(n.Trim(), StringComparison.OrdinalIgnoreCase)));
            foreach (var n in t)
                targetIsSubset = targetIsSubset && (s.Any(o => o.Trim().Equals(n.Trim(), StringComparison.OrdinalIgnoreCase)));
            if (targetIsSubset)
            {
                var dt = await FetchTableData(sourceTableName, t);
                return await InsertData(targetTableName, dt);
            }
            else if (targetIsSuperset)
            {
                var newColumns = new List<string>(t.Where(o => !s.Any(y => y.Trim().Equals(o.Trim(), StringComparison.OrdinalIgnoreCase))));
                bool allNewAreNullable = true;
                foreach (var n in newColumns)
                    allNewAreNullable = allNewAreNullable && GetTableColumnCanInsertNull(targetServer, targetTableName, n);
                if (!allNewAreNullable)
                {
                    errorTables.Add(new BasicPair<string, string>(sourceTableName, targetTableName));
                    return false;
                }
                var dt = await FetchTableData(sourceTableName);
                return await InsertData(targetTableName, dt);

            }
            else
            {
                errorTables.Add(new BasicPair<string, string>(sourceTableName, targetTableName));
                return false;
            }

        }

        private Task<DataTable> FetchTableData(string tableName)
        {
            return Task.Factory.StartNew<DataTable>(() =>
            {
                DataTable dt = new DataTable();
                try
                {
                    var cols = GetTableColumns(sourceServer, tableName);
                   
                    dt = FetchTableData(tableName, cols).Result;
                }
                catch (Exception e)
                {
                    Log.E(e.ToString(), null);

                }

                return dt;
            });
        }

        private Task<DataTable> FetchTableData(string tableName, List<string> columns)
        {
            return Task.Factory.StartNew<DataTable>(() =>
            {
                var gy = "";
                DataTable dt = new DataTable();
                try
                {
                    if (columns.Count == 0)
                        throw new ArgumentException("Columns count cannot be zero.");
                    using (SqlConnection DBConnection = sourceServer.CreateConnection())
                    {
                        SqlCommand cmd = new SqlCommand();
                        cmd.CommandText = "USE UmanyiSMS\r\nSET DATEFORMAT DMY\r\nSELECT ";
                        foreach (var c in columns)
                            cmd.CommandText +="["+ c + "],";
                        cmd.CommandText = cmd.CommandText.Remove(cmd.CommandText.Length - 1);
                        cmd.CommandText += " FROM " + tableName;
                        gy = cmd.CommandText;
                        cmd.Connection = DBConnection;
                        
                        SqlDataReader reader = cmd.ExecuteReader();

                        if (!reader.IsClosed && reader.HasRows)

                        {
                            int colIndex = reader.FieldCount;
                            for (int i = 0; i < colIndex; i++)
                            {
                                dt.Columns.Add(new DataColumn(columns[i], typeof(object)));
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
                        reader.Close();
                        DBConnection.Close();
                        cmd.Dispose();
                    }
                }
                catch (Exception e)
                {
                    Log.E(e.ToString(), null);

                }

                return dt;
            });
        }

        private bool CompareTableColumns(string sourceTableName, string targetTableName)
        {
            var t = GetTableColumns(targetServer, targetTableName);
            var s = GetTableColumns(sourceServer, sourceTableName);
            if (t.Count != s.Count)
                return false;
            for (int i = 0; i < t.Count; i++)
                if (!t[i].Equals(s[i]))
                    return false;
            return true;
        }

        private Task<bool> InsertData(string tableName, DataTable dt)
        {
            return Task.Factory.StartNew<bool>(() =>
            {
                try
                {
                    var cols = GetTableColumns(targetServer, tableName);
                    using (var DBConnection = targetServer.CreateConnection())
                    {
                        var dta = new SqlCommand();

                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            var b = GetInsertRowCommandText(tableName, cols, dt.Rows[i], i);
                            dta.Connection = DBConnection;
                            dta.CommandText = b.Key;
                            foreach (var t in b.Value)
                                dta.Parameters.Add(t);
                            //dta.ExecuteNonQuery();
                        }
                        DBConnection.Close();
                        dta.Dispose();
                    }
                    return true;
                }
                catch (Exception e)
                {
                    Log.E(e.ToString(), null);
                    return false;
                }
            });
        }

        private BasicPair<string, List<SqlParameter>> GetInsertRowCommandText(string tableName, List<string> columns, DataRow row, int rowNo)
        {
            string tex = "INSERT INTO " + tableName + " (";
            List<SqlParameter> para = new List<SqlParameter>();
            foreach (var c in columns)
                tex += c + ",";
            tex = tex.Remove(tex.Length - 1) + ") VALUES (";
            for (int c = 0; c < columns.Count; c++)
            {
                tex += "@paraC" + c + "R" + rowNo;
                para.Add(new SqlParameter("@paraC" + c + "R" + rowNo, row[c]));
            }
            return new BasicPair<string, List<SqlParameter>>(tex, para);
        }



        public Task<bool> TestServer1()
        {
            return Task.Factory.StartNew<bool>(() =>
            {
                return sourceServer.ExecuteNonQuery("USE UMANYISMS"); ;
            });

        }

        public Task<bool> TestServer2()
        {
            return Task.Factory.StartNew<bool>(() =>
            {
                return targetServer.ExecuteNonQuery("USE UMANYISMS"); ;
            });
        }

        protected override void InitVars()
        {
            Progress = 10;
            Source = new Page1VM();
            Server1 = Environment.MachineName + @"\Umanyi";
            Server2 = @"(LocalDb)\v11.0";
        }
        
        public ICommand SaveCommand
        { get; private set; }

        public ICommand StartCommand
        { get; private set; }

        public ICommand Page1Command
        { get; private set; }

        public ICommand Page2Command
        { get; private set; }

        public ICommand LogCommand
        { get; private set; }

        public int Progress
        {
            get { return progress; }
            set
            {
                if (value != this.progress)
                {
                    this.progress = value;
                    NotifyPropertyChanged("Progress");
                }
            }
        }

        public string ProgressText
        {
            get { return progressText; }
            set
            {
                if (value != this.progressText)
                {
                    this.progressText = value;
                    NotifyPropertyChanged("ProgressText");
                }
            }
        }

        public List<BasicPair<string, string>> ErrorTables
        {
            get { return errorTables; }
        }


        public object Source
        {
            get { return source; }
            set
            {
                if (value != this.source)
                {
                    this.source = value;
                    NotifyPropertyChanged("Source");
                }
            }
        }

        public string Server1
        {
            get { return server1; }
            set
            {
                if (value != this.server1)
                {
                    this.server1 = value;
                    NotifyPropertyChanged("Server1");
                }
            }
        }

        public string Server2
        {
            get { return server2; }
            set
            {
                if (value != this.server2)
                {
                    this.server2 = value;
                    NotifyPropertyChanged("Server2");
                }
            }
        }
    }
}