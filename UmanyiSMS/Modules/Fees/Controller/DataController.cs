using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UmanyiSMS.Lib;
using UmanyiSMS.Lib.Controllers;
using UmanyiSMS.Lib.Presentation;
using UmanyiSMS.Modules.Fees.Models;
using UmanyiSMS.Modules.Institution.Models;
using UmanyiSMS.Modules.Purchases.Models;
using UmanyiSMS.Modules.Students.Models;

namespace UmanyiSMS.Modules.Fees.Controller
{
    public class DataController
    {
        public static Task<ClassBalancesListModel> GetBalancesList(ClassModel selectedClass)
        {
            return Task.Factory.StartNew<ClassBalancesListModel>(delegate
            {
                ClassBalancesListModel classBalancesListModel = new ClassBalancesListModel();
                classBalancesListModel.ClassID = selectedClass.ClassID;
                classBalancesListModel.NameOfClass = selectedClass.NameOfClass;
                classBalancesListModel.Date = DateTime.Now;
                classBalancesListModel.Total = 0m;
                classBalancesListModel.Entries = GetClassBalancesListAsync(selectedClass.ClassID).Result;
                foreach (StudentFeesDefaultModel current in classBalancesListModel.Entries)
                {
                    classBalancesListModel.Total += current.Balance;
                }
                foreach (StudentFeesDefaultModel current in classBalancesListModel.Entries)
                {
                    classBalancesListModel.TotalUnpaid += ((current.Balance > 0m) ? current.Balance : 0m);
                }
                return classBalancesListModel;
            });
        }

        private static Task<ObservableCollection<StudentFeesDefaultModel>> GetClassBalancesListAsync(int classID)
        {
            return Task.Factory.StartNew<ObservableCollection<StudentFeesDefaultModel>>(delegate
            {
                ObservableCollection<StudentFeesDefaultModel> observableCollection = new ObservableCollection<StudentFeesDefaultModel>();
                string commandText = "SELECT s.StudentID, NameOfStudent, GuardianPhoneNo,dbo.GetCurrentBalance(s.StudentID) FROM [Student]s LEFT OUTER JOIN [StudentClass]sc ON (sc.StudentID=s.StudentID AND sc.[Year]=DATEPART(year,sysdatetime())) WHERE sc.ClassID=" + classID + " AND sc.[Year]=DATEPART(year,sysdatetime()) AND s.IsActive = 1";
                DataTable dataTable = DataAccessHelper.Helper.ExecuteNonQueryWithResult(commandText);
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    observableCollection.Add(new StudentFeesDefaultModel
                    {
                        StudentID = int.Parse(dataRow[0].ToString()),
                        NameOfStudent = dataRow[1].ToString(),
                        GuardianPhoneNo = dataRow[2].ToString(),
                        Balance = decimal.Parse(dataRow[3].ToString())
                    });
                }
                return observableCollection;
            });
        }


        public static Task<SaleModel> GetTermInvoice(int studentID, DateTime date)
        {
            var term = Institution.Controller.DataController.GetTerm(Institution.Controller.DataController.GetTerm(date));
            return GetTermInvoice(studentID, term);
        }

        public static Task<SaleModel> GetTermInvoice(int studentID, TermModel term)
        {
            return Task.Factory.StartNew<SaleModel>(delegate
            {
                SaleModel saleModel = new SaleModel();
                DateTime startd = term.StartDate.Date;
                DateTime endd = new DateTime(term.EndDate.Year, term.EndDate.Month, term.EndDate.Day, 23, 59, 59, 998);

                string text = "SELECT SaleID,EmployeeID,OrderDate,TotalAmt FROM [SaleHeader] WHERE CustomerID=@cid AND OrderDate BETWEEN @startd AND @endd";
                var paramColl = new List<SqlParameter>();
                paramColl.Add(new SqlParameter("@cid", studentID));
                paramColl.Add(new SqlParameter("@startd", startd));
                paramColl.Add(new SqlParameter("@endd", endd));
                DataTable dataTable = DataAccessHelper.Helper.ExecuteNonQueryWithResultTable(text,paramColl);
                if (dataTable.Rows.Count > 0)
                {
                    DataRow dataRow = dataTable.Rows[0];
                    saleModel.SaleID = int.Parse(dataRow[0].ToString());
                    saleModel.CustomerID = studentID;
                    saleModel.EmployeeID = int.Parse(dataRow[1].ToString());
                    saleModel.DateAdded = DateTime.Parse(dataRow[2].ToString());
                    saleModel.OrderTotal = decimal.Parse(dataRow[3].ToString());
                    saleModel.SaleItems = GetSaleItems(saleModel.SaleID);
                }
                return saleModel;
            });
        }

        private static ObservableCollection<FeesStructureEntryModel> GetSaleItems(int saleID)
        {
            ObservableCollection<FeesStructureEntryModel> observableCollection = new ObservableCollection<FeesStructureEntryModel>();
            if (saleID==0)
                return observableCollection;
            string commandText = "SELECT Name,Amount FROM [SaleDetail] WHERE SaleID=" + saleID;
            DataTable dataTable = DataAccessHelper.Helper.ExecuteNonQueryWithResult(commandText);
            foreach (DataRow dataRow in dataTable.Rows)
            {
                observableCollection.Add(new FeesStructureEntryModel
                {
                    Name = dataRow[0].ToString(),
                    Amount = decimal.Parse(dataRow[1].ToString())
                });
            }
            return observableCollection;
        }

        public static Task<FeesStructureModel> GetFeesStructureAsync(int currentClassID,int term)
        {
            return Task.Factory.StartNew<FeesStructureModel>(delegate
            {
                FeesStructureModel feesStructureModel = new FeesStructureModel();
                // string text = currentDate.Date.ToString("g");
                string commandText = "DECLARE @id int\r\nSET @id=(SELECT TOP 1 FeesStructureID FROM [FeesStructureHeader] WHERE ClassID=" 
                + currentClassID + "AND Term="+term+" AND [Year]=DATEPART(YEAR,sysdatetime()))\r\nSELECT ISNULL(@id,0)";
                int num = int.Parse(DataAccessHelper.Helper.ExecuteScalar(commandText));
                FeesStructureModel result;
                if (num <= 0)
                {
                    result = feesStructureModel;
                }
                else
                {
                    commandText = "SELECT Name, Amount FROM [FeesStructureDetail] WHERE FeesStructureID =" + num;
                    DataTable dataTable = DataAccessHelper.Helper.ExecuteNonQueryWithResult(commandText);
                    foreach (DataRow dataRow in dataTable.Rows)
                    {
                        FeesStructureEntryModel feesStructureEntryModel = new FeesStructureEntryModel();
                        feesStructureEntryModel.Amount = decimal.Parse(dataRow[1].ToString());
                        feesStructureEntryModel.Name = dataRow[0].ToString();
                        feesStructureModel.Entries.Add(feesStructureEntryModel);
                    }
                    result = feesStructureModel;
                }
                return result;
            });
        }

        public static Task<bool> HasInvoicedOnTerm(int studentID, TermModel term)
        {
            return Task.Factory.StartNew<bool>(delegate
            {
                DateTime startd = term.StartDate.Date;
                DateTime endd = new DateTime(term.EndDate.Year, term.EndDate.Month, term.EndDate.Day, 23, 59, 59, 998);
                var paramColl = new List<SqlParameter>();
                paramColl.Add(new SqlParameter("@cid", studentID));
                paramColl.Add(new SqlParameter("@startd", startd));
                paramColl.Add(new SqlParameter("@endd", endd));
                string text = "IF EXISTS(SELECT * FROM [SaleHeader] WHERE CustomerID=@cid AND OrderDate BETWEEN @startd AND @endd) SELECT 'True' ELSE SELECT 'False'";
                return bool.Parse(DataAccessHelper.Helper.ExecuteScalar(text,paramColl));
            });
        }

        public static Task<bool> SaveNewStudentBill(SaleModel newSale)
        {
            return Task.Factory.StartNew<bool>(delegate
            {
                var paramColl = new List<SqlParameter>();
                string text = string.Concat(new object[]
                {
                    "BEGIN TRANSACTION\r\nDECLARE @id int; SET @id = dbo.GetNewID('dbo.SaleHeader')\r\n",
                    "INSERT INTO [SaleHeader] (SaleID,CustomerID,EmployeeID,OrderDate)" ,
                    " VALUES(@id,@cid,@eid,@odate)"
                });
                paramColl.Add(new SqlParameter("@cid", newSale.CustomerID));
                paramColl.Add(new SqlParameter("@eid", newSale.EmployeeID));
                paramColl.Add(new SqlParameter("@odate", newSale.DateAdded));
                int index = 0;
                foreach (FeesStructureEntryModel current in newSale.SaleItems)
                {
                    paramColl.Add(new SqlParameter("@nam"+index, current.Name));
                    paramColl.Add(new SqlParameter("@amt"+index, current.Amount));
                    object obj = text;
                    text = string.Concat(new object[]
                    {
                        obj,
                        "\r\nINSERT INTO [SaleDetail] (SaleID,Name,Amount) VALUES(@id,@nam",index,",@amt",index,")"
                    });
                    index++;
                }
                text += "\r\nCOMMIT";
                return DataAccessHelper.Helper.ExecuteNonQuery(text,paramColl);
            });
        }

        public static Task<bool> SaveNewClassBill(SaleModel newSale, TermModel term)
        {
            return Task.Factory.StartNew<bool>(delegate
            {
                DateTime startd = term.StartDate.Date;
                DateTime endd = new DateTime(term.EndDate.Year, term.EndDate.Month, term.EndDate.Day, 23, 59, 59, 998);

                var paramColl = new List<SqlParameter>();
                paramColl.Add(new SqlParameter("@startd", startd));
                paramColl.Add(new SqlParameter("@endd", endd));
                paramColl.Add(new SqlParameter("@ordd", startd.AddDays(1)));
                paramColl.Add(new SqlParameter("@eid", newSale.EmployeeID));

                int index2 = 0;
                foreach (var current2 in newSale.SaleItems)
                {
                    paramColl.Add(new SqlParameter("@nam" + index2, current2.Name));
                    paramColl.Add(new SqlParameter("@amt" + index2, current2.Amount));
                    index2++;
                }

                string selectString = "SELECT s.StudentID FROM [Student]s LEFT OUTER JOIN [StudentClass]cs ON (s.StudentID = cs.StudentID AND cs.[Year]=DATEPART(YEAR,sysdatetime())) WHERE s.IsActive=1 AND cs.[Year]=DATEPART(YEAR,sysdatetime()) AND cs.ClassID=" + newSale.CustomerID;
                List<string> observableCollection = DataAccessHelper.Helper.CopyFirstColumnToList(selectString);

                int index = 0;
                
                string text = "BEGIN TRANSACTION\r\nDECLARE @id int;";
                foreach (string current in observableCollection)
                {
                    paramColl.Add(new SqlParameter("@cid" + index, current));
                    text +=
                        "\r\nIF NOT EXISTS(SELECT * FROM [SaleHeader] WHERE CustomerID=@cid" + index + " AND OrderDate BETWEEN @startd AND @endd)\r\n" +
                        "BEGIN\r\nSET @id = dbo.GetNewID('dbo.SaleHeader');\r\nINSERT INTO [SaleHeader] (SaleID,CustomerID,EmployeeID,OrderDate) " +
                        "VALUES(@id,@cid" + index + ",@eid,@ordd)\r\n";
                    index2 = 0;
                    foreach (var current2 in newSale.SaleItems)
                    {
                        text += "INSERT INTO [SaleDetail] (SaleID,Name,Amount) VALUES(@id,@nam" + index2 + ",@amt" + index2 + ");\r\n";
                        index2++;
                    }
                    text += "END\r\nELSE\r\nBEGIN\r\n";
                    text += "SET @id =(SELECT SaleID FROM [SaleHeader] WHERE CustomerID=@cid" + index + " AND OrderDate BETWEEN @startd AND @endd) " +
                    "DELETE FROM [SaleDetail] WHERE SaleID=@id";
                    index2 = 0;
                    foreach (var current2 in newSale.SaleItems)
                    {
                        text += "\r\nINSERT INTO [SaleDetail] (SaleID,Name,Amount) VALUES(@id,@nam" + index2 + ",@amt" + index2 + ")";
                        index2++;
                    }
                    text+="\r\nEND";
                    index++;
                }
                text += "\r\nCOMMIT";
                return DataAccessHelper.Helper.ExecuteNonQuery(text, paramColl);
            });
        }


        public static Task<bool> UpdateStudentBill(SaleModel newSale)
        {
            return Task.Factory.StartNew<bool>(delegate
            {
                string text = "BEGIN TRANSACTION\r\nDELETE FROM [SaleDetail] WHERE SaleID=@sid";
                var paramColl = new List<SqlParameter>();
                paramColl.Add(new SqlParameter("@sid", newSale.SaleID));
                int index = 0;
                foreach (FeesStructureEntryModel current in newSale.SaleItems)
                {
                    paramColl.Add(new SqlParameter("@nam" + index, current.Name));
                    paramColl.Add(new SqlParameter("@amt" + index, current.Amount));
                    object obj = text;
                    text = string.Concat(new object[]
                    {
                        obj,
                         "\r\nINSERT INTO [SaleDetail] (SaleID,Name,Amount) VALUES(@sid,@nam",index,",@amt",index,")"
                    });
                }
                text += "\r\nCOMMIT";
                return DataAccessHelper.Helper.ExecuteNonQuery(text,paramColl);
            });
        }

        public static Task<ObservableCollection<FeesPaymentHistoryModel>> GetFeesPaymentsHistoryAsync(DateTime? from, DateTime? to)
        {
            return Task.Factory.StartNew<ObservableCollection<FeesPaymentHistoryModel>>(() => GetFeesPaymentsHistory(from, to));
        }

        private static ObservableCollection<FeesPaymentHistoryModel> GetFeesPaymentsHistory(DateTime? from, DateTime? to)
        {
            ObservableCollection<FeesPaymentHistoryModel> observableCollection = new ObservableCollection<FeesPaymentHistoryModel>();
            ObservableCollection<FeesPaymentHistoryModel> result;
            try
            {
                string text = "SELECT ISNULL(PaymentMethod,''), SUM(CONVERT(decimal(18,0),AmountPaid)) FROM [FeesPayment] ";
                if (from.HasValue && to.HasValue)
                {
                    string text2 = text;
                    text = string.Concat(new string[]
                    {
                        text2,
                        " WHERE DatePaid BETWEEN CONVERT(datetime,'",
                        from.Value.Day.ToString(),
                        "/",
                        from.Value.Month.ToString(),
                        "/",
                        from.Value.Year.ToString(),
                        " 00:00:00.000') AND CONVERT(datetime,'",
                        to.Value.Day.ToString(),
                        "/",
                        to.Value.Month.ToString(),
                        "/",
                        to.Value.Year.ToString(),
                        " 23:59:59.998')"
                    });
                }

                text += "\r\nGROUP BY PaymentMethod";
                DataTable dataTable = DataAccessHelper.Helper.ExecuteNonQueryWithResult(text);
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    observableCollection.Add(new FeesPaymentHistoryModel
                    {
                        PaymentMode = string.IsNullOrWhiteSpace(dataRow[0].ToString()) ? "UNSET" :
                        dataRow[0].ToString(),
                        Amount = decimal.Parse(dataRow[1].ToString())
                    });
                }
                result = observableCollection;
            }
            catch
            {
                result = new ObservableCollection<FeesPaymentHistoryModel>();
            }
            return result;
        }

        public static Task<FeesStatementModel> GetFeesStatementAsync(int studentID, DateTime? startTime, DateTime? endTime)
        {
            return Task.Factory.StartNew<FeesStatementModel>(delegate
            {
                FeesStatementModel result;
                if (studentID <= 0)
                {
                    result = new FeesStatementModel();
                }
                else
                {
                    FeesStatementModel feesStatementModel = new FeesStatementModel();
                    string text = "SELECT SaleID,OrderDate, TotalAmt FROM [SaleHeader] WHERE [CustomerID] =" + studentID ;
                    if (startTime.HasValue && endTime.HasValue)
                    {
                        string text2 = text;
                        text = string.Concat(new string[]
                        {
                            text2,
                            " AND OrderDate BETWEEN CONVERT(datetime,'",
                            startTime.Value.ToString("dd-MM-yyyy"),
                            " 00:00:00.000') AND CONVERT(datetime,'",
                            endTime.Value.ToString("dd-MM-yyyy"),
                            " 23:59:59.998')"
                        });
                    }
                    string text3 = "SELECT FeesPaymentID, DatePaid, AmountPaid FROM [FeesPayment]  WHERE [StudentID] =" + studentID;
                    if (startTime.HasValue && endTime.HasValue)
                    {
                        string text2 = text3;
                        text3 = string.Concat(new string[]
                        {
                            text2,
                            " AND DatePaid BETWEEN CONVERT(datetime,'",
                            startTime.Value.ToString("dd-MM-yyyy"),
                            " 00:00:00.000') AND CONVERT(datetime,'",
                            endTime.Value.ToString("dd-MM-yyyy"),
                            " 23:59:59.998')"
                        });
                    }
                    DataTable dataTable = DataAccessHelper.Helper.ExecuteNonQueryWithResult(text);
                    DataTable dataTable2 = DataAccessHelper.Helper.ExecuteNonQueryWithResult(text3);
                    ObservableCollection<TransactionModel> observableCollection = new ObservableCollection<TransactionModel>();
                    foreach (DataRow dataRow in dataTable.Rows)
                    {
                        observableCollection.Add(new TransactionModel(TransactionTypes.Debit, dataRow[0].ToString(), DateTime.Parse(dataRow[1].ToString()), decimal.Parse(dataRow[2].ToString())));
                        feesStatementModel.TotalSales += decimal.Parse(dataRow[2].ToString());
                        feesStatementModel.TotalDue += decimal.Parse(dataRow[2].ToString());
                    }
                    foreach (DataRow dataRow in dataTable2.Rows)
                    {
                        DateTime transactionDateTime;
                        DateTime.TryParse(dataRow[1].ToString(), out transactionDateTime);
                        decimal num;
                        decimal.TryParse(dataRow[2].ToString(), out num);
                        observableCollection.Add(new TransactionModel(TransactionTypes.Credit, dataRow[0].ToString(), transactionDateTime, num));
                        feesStatementModel.TotalPayments += num;
                        feesStatementModel.TotalDue -= num;
                    }
                    IEnumerable<TransactionModel> enumerable = from fruit in observableCollection
                                                               orderby fruit.TransactionDateTime
                                                               select fruit;
                    feesStatementModel.From = new DateTime(startTime.Value.Year, startTime.Value.Month, startTime.Value.Day, 00, 00, 00, 000);
                    feesStatementModel.To = new DateTime(endTime.Value.Year, endTime.Value.Month, endTime.Value.Day, 23, 59, 59, 998);

                    feesStatementModel.BalanceBroughtForward = GetCurrentBalanceAsync(studentID, feesStatementModel.From).Result;
                    feesStatementModel.Transactions.Add(new TransactionModel(TransactionTypes.Credit, "0", DateTime.Now, feesStatementModel.BalanceBroughtForward));
                    foreach (TransactionModel current in enumerable)
                    {
                        feesStatementModel.Transactions.Add(current);
                    }
                    feesStatementModel.StudentID = studentID;

                    feesStatementModel.TotalDue = GetCurrentBalanceAsync(studentID, feesStatementModel.To).Result;
                    result = feesStatementModel;
                }
                return result;
            });
        }

        private static Task<decimal> GetCurrentBalanceAsync(int studentID, DateTime date)
        {
            return Task.Factory.StartNew<decimal>(delegate
            {
                string commandText = "DECLARE  @sal decimal=(SELECT SUM(ISNULL(CONVERT(DECIMAL,TotalAmt),0)) FROM  [SaleHeader] WHERE CustomerID =@studentID AND OrderDate <CONVERT(datetime,@dt));\r\n" +
                "DECLARE  @pur decimal=(SELECT SUM(ISNULL(CONVERT(DECIMAL,AmountPaid),0)) FROM  [FeesPayment] WHERE StudentID =@studentID  AND DatePaid <CONVERT(datetime,@dt));\r\n" +
                "DECLARE  @prev decimal=(SELECT CONVERT(DECIMAL,PreviousBalance) FROM  [Student] WHERE StudentID=@studentID)\r\n" +
                "SELECT (select (ISNULL(@sal,0)+ISNULL(@prev,0))-ISNULL(@pur,0));";
                decimal result;
                var paramColl = new List<SqlParameter>();
                paramColl.Add(new SqlParameter("@studentID", studentID));
                paramColl.Add(new SqlParameter("@dt", date));
                decimal.TryParse(DataAccessHelper.Helper.ExecuteScalar(commandText, paramColl), out result);
                return result;
            });
        }

        public static Task<int> GetLastPaymentIDAsync(int studentID)
        {
            return Task.Factory.StartNew<int>(delegate
            {
                string commandText = "SELECT TOP 1 FeesPaymentID FROM [FeesPayment] WHERE StudentID=@sid ORDER BY ModifiedDate DESC";
                int result;
                int.TryParse(DataAccessHelper.Helper.ExecuteScalar(commandText,new List<SqlParameter>() { new SqlParameter("@sid",studentID)}), out result);
                return result;
            });
        }

        public static Task<FeePaymentReceiptModel> GetReceiptAsync(FeePaymentModel currentPayment, ObservableImmutableList<FeesStructureEntryModel> currentFeesStructure)
        {
            return Task.Factory.StartNew<FeePaymentReceiptModel>(delegate
            {
                FeePaymentReceiptModel feePaymentReceiptModel = new FeePaymentReceiptModel();
                feePaymentReceiptModel.FeePaymentID = currentPayment.FeePaymentID;
                feePaymentReceiptModel.AmountPaid = currentPayment.AmountPaid;
                feePaymentReceiptModel.Entries = currentFeesStructure;
                feePaymentReceiptModel.DatePaid = currentPayment.DatePaid;
                feePaymentReceiptModel.NameOfClass = Institution.Controller.DataController.GetClassAsync(Students.Controller.DataController.GetClassIDFromStudentID(currentPayment.StudentID).Result).Result.NameOfClass;
                feePaymentReceiptModel.StudentID = currentPayment.StudentID;
                feePaymentReceiptModel.NameOfStudent = currentPayment.NameOfStudent;
                feePaymentReceiptModel.PaymentMethod = currentPayment.PaymentMethod;
                FeesStructureEntryModel feesStructureEntryModel = new FeesStructureEntryModel();
                feesStructureEntryModel.Name = "TOTAL";
                foreach (FeesStructureEntryModel current in feePaymentReceiptModel.Entries)
                {
                    feesStructureEntryModel.Amount += current.Amount;
                }
                FeesStructureEntryModel feesStructureEntryModel2 = new FeesStructureEntryModel();
                feesStructureEntryModel2.Amount = feePaymentReceiptModel.AmountPaid;
                feesStructureEntryModel2.Name = "AMOUNT PAID";
                FeesStructureEntryModel feesStructureEntryModel3 = new FeesStructureEntryModel();
                feesStructureEntryModel3.Amount = GetCurrentBalanceAsync(feePaymentReceiptModel.StudentID, currentPayment.DatePaid).Result;
                feesStructureEntryModel3.Name = "BALANCE B/F";
                FeesStructureEntryModel feesStructureEntryModel4 = new FeesStructureEntryModel();

                feesStructureEntryModel4.Amount = GetCurrentBalanceAsync(feePaymentReceiptModel.StudentID, feePaymentReceiptModel.DatePaid.AddSeconds(1)).Result;
                feesStructureEntryModel4.Name = "TOTAL BALANCE";
                feePaymentReceiptModel.Entries.Add(feesStructureEntryModel);
                feePaymentReceiptModel.Entries.Add(feesStructureEntryModel2);
                feePaymentReceiptModel.Entries.Add(feesStructureEntryModel3);
                feePaymentReceiptModel.Entries.Add(feesStructureEntryModel4);
                return feePaymentReceiptModel;
            });
        }

        public static Task<bool> SaveNewFeesPaymentAsync(FeePaymentModel newPayment)
        {
            return Task.Factory.StartNew<bool>(delegate
            {
                if (newPayment.DatePaid.Date.Equals(DateTime.Now.Date))
                    newPayment.DatePaid = DateTime.Now;
                string commandText = "INSERT INTO [FeesPayment] (FeesPaymentID,StudentID,AmountPaid,DatePaid,PaymentMethod) VALUES(dbo.GetNewID('dbo.FeesPayment'),@studentID,@amount,@dop,@paym)";
                return DataAccessHelper.Helper.ExecuteNonQuery(commandText, new ObservableCollection<SqlParameter>
                {
                    new SqlParameter("@studentID", newPayment.StudentID),
                    new SqlParameter("@amount", newPayment.AmountPaid),
                    new SqlParameter("@dop", newPayment.DatePaid),
                    new SqlParameter("@paym", newPayment.PaymentMethod)
                });
            });
        }

        public static Task<ObservableCollection<FeePaymentModel>> GetRecentPaymentsAsync(StudentBaseModel student)
        {
            return Task.Factory.StartNew<ObservableCollection<FeePaymentModel>>(delegate
            {
                ObservableCollection<FeePaymentModel> observableCollection = new ObservableCollection<FeePaymentModel>();
                string commandText = "SELECT TOP 20 FeesPaymentID,AmountPaid, DatePaid, PaymentMethod FROM [FeesPayment] WHERE StudentID =" + student.StudentID + " ORDER BY [DatePaid] desc";
                DataTable dataTable = DataAccessHelper.Helper.ExecuteNonQueryWithResult(commandText);
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    observableCollection.Add(new FeePaymentModel
                    {
                        FeePaymentID = int.Parse(dataRow[0].ToString()),
                        AmountPaid = decimal.Parse(dataRow[1].ToString()),
                        StudentID = student.StudentID,
                        NameOfStudent = student.NameOfStudent,
                        DatePaid = DateTime.Parse(dataRow[2].ToString()),
                        PaymentMethod = dataRow[3].ToString()
                    });
                }
                return observableCollection;
            });
        }

        public static Task<ObservableCollection<VoteHeadModel>> GetVoteHeadsSummaryByClass(int classID, TermModel term)
        {
            return Task.Factory.StartNew<ObservableCollection<VoteHeadModel>>(delegate
            {
                DateTime startd = term.StartDate.Date;
                DateTime endd = new DateTime(term.EndDate.Year, term.EndDate.Month, term.EndDate.Day, 23, 59, 59, 998);

                var paramColl = new List<SqlParameter>();
                paramColl.Add(new SqlParameter("@startd", startd));
                paramColl.Add(new SqlParameter("@endd", endd));
                paramColl.Add(new SqlParameter("@cid", classID));
                string commandText = string.Concat(new object[]
                {
                    "SELECT sd.Name,ISNULL(SUM(sd.Amount),0) FROM [SaleDetail] sd LEFT OUTER JOIN [SaleHeader] sh ON (sd.SaleID=sh.SaleID) ",
                    "LEFT OUTER JOIN [Student] s ON (s.StudentID=sh.CustomerID)",
                    "LEFT OUTER JOIN [StudentClass] sc ON (s.StudentID = sc.StudentID AND sc.[Year] = DATEPART(year,sysdatetime()))",
                    "WHERE sc.CLassID=@cid AND sc.[Year]=DATEPART(year,sysdatetime()) AND s.IsActive=1 AND sh.OrderDate BETWEEN @startd AND @endd GROUP BY sd.Name"
                });
                DataTable dataTable = DataAccessHelper.Helper.ExecuteNonQueryWithResultTable(commandText,paramColl);
                ObservableCollection<VoteHeadModel> observableCollection = new ObservableCollection<VoteHeadModel>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    observableCollection.Add(new VoteHeadModel
                    {
                        Name = dataRow[0].ToString(),
                        Amount = decimal.Parse(dataRow[1].ToString())
                    });
                }
                return observableCollection;
            });
        }

        public static Task<bool> RemoveSaleAsync(int saleID)
        {
            return Task.Factory.StartNew<bool>(delegate
            {
                bool result = false;
                try
                {
                    string text = "DELETE FROM [SaleDetail] WHERE SaleID = " + saleID;
                    text = text + "\r\nDELETE FROM [SaleHeader] WHERE SaleID = " + saleID;
                    result = DataAccessHelper.Helper.ExecuteNonQuery(text);
                }
                catch
                {
                }
                return result;
            });
        }

        public static Task<bool> SaveNewFeesStructureAsync(FeesStructureModel currrentStruct)
        {
            return Task.Factory.StartNew<bool>(delegate
            {
                string text = string.Concat(new object[]
                {
                    "BEGIN TRANSACTION\r\nDECLARE @id int; SET @id = dbo.GetNewID('dbo.FeesStructureHeader')\r\nINSERT INTO [FeesStructureHeader] (FeesStructureID,ClassID, Term,[Year]) VALUES (@id,",
                    currrentStruct.ClassID,
                    ",",
                    currrentStruct.Term,
                    ",",currrentStruct.Year,
                    ")\r\n"
                });
                foreach (FeesStructureEntryModel current in currrentStruct.Entries)
                {
                    object obj = text;
                    text = string.Concat(new object[]
                    {
                        obj,
                        "INSERT INTO [FeesStructureDetail] (FeesStructureID,Name,Amount) VALUES (@id,'",
                        current.Name,
                        "','",
                        current.Amount,
                        "')\r\n"
                    });
                }
                text += " COMMIT";
                return DataAccessHelper.Helper.ExecuteNonQuery(text);
            });
        }

        public static Task<bool> RemovePaymentAsync(int paymentID)
        {
            return Task.Factory.StartNew<bool>(delegate
            {
                bool result = false;
                try
                {
                    string commandText = "DELETE FROM [FeesPayment] WHERE FeesPaymentID = " + paymentID;
                    result = DataAccessHelper.Helper.ExecuteNonQuery(commandText);
                }
                catch
                {
                }
                return result;
            });
        }

        public static Task<ObservableCollection<FeesStructureModel>> GetFullFeesStructure(DateTime currentDate)
        {
            return Task.Factory.StartNew<ObservableCollection<FeesStructureModel>>(delegate
            {
                ObservableCollection<FeesStructureModel> observableCollection = new ObservableCollection<FeesStructureModel>();
                ObservableCollection<CombinedClassModel> result =Institution.Controller.DataController.GetAllCombinedClassesAsync().Result;
                ObservableCollection<FeesStructureModel> result2;
                using (IEnumerator<CombinedClassModel> enumerator = result.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        CombinedClassModel c = enumerator.Current;
                        FeesStructureModel feesStructureModel = new FeesStructureModel();
                        string text = currentDate.Date.ToString("g");
                        string commandText = "DECLARE @id int\r\nSET @id=(SELECT TOP 1 FeesStructureID FROM [FeesStructureHeader] WHERE ClassID=" + c.Entries[0].ClassID + 
                        " AND Term="+Institution.Controller.DataController.GetTerm(currentDate)+" AND [Year]="+currentDate.Year+")\r\nSELECT ISNULL(@id,0)";
                        feesStructureModel.NameOfCombinedClass = result.First((CombinedClassModel o) => o.Entries.Any((ClassModel a) => a.ClassID == c.Entries[0].ClassID)).Description;
                        feesStructureModel.Term = Institution.Controller.DataController.GetTerm(currentDate);
                        feesStructureModel.Year = currentDate.Year;
                        int num = int.Parse(DataAccessHelper.Helper.ExecuteScalar(commandText));
                        if (num <= 0)
                        {
                            result2 = observableCollection;
                            return result2;
                        }
                        commandText = "SELECT Name, Amount FROM [FeesStructureDetail] WHERE FeesStructureID =" + num;
                        DataTable dataTable = DataAccessHelper.Helper.ExecuteNonQueryWithResult(commandText);
                        foreach (DataRow dataRow in dataTable.Rows)
                        {
                            FeesStructureEntryModel feesStructureEntryModel = new FeesStructureEntryModel();
                            feesStructureEntryModel.Amount = decimal.Parse(dataRow[1].ToString());
                            feesStructureEntryModel.Name = dataRow[0].ToString();
                            feesStructureModel.Entries.Add(feesStructureEntryModel);
                        }
                        observableCollection.Add(feesStructureModel);
                    }
                }
                result2 = observableCollection;
                return result2;
            });
        }


    }
}
