using Helper.Models;
using Helper.Presentation;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UmanyiSMS;

namespace Helper
{
    public static class DataAccess
    {
        public static Task<ObservableCollection<FeePaymentModel>> GetFeesPaymentsAsync(int? studentID, DateTime? startTime, DateTime? endTime, string paymentMethod)
        {
            return Task.Run<ObservableCollection<FeePaymentModel>>(() => DataAccess.GetFeesPayments(studentID, startTime, endTime, paymentMethod));
        }

        private static ObservableCollection<SaleModel> GetSales(bool includeAllDetails, int? studentID, DateTime? startTime, DateTime? endTime)
        {
            string text;
            if (studentID.HasValue)
            {
                text = "SELECT SaleID,CustomerID,EmployeeID,OrderDate,TotalAmt FROM [Sales].[SaleHeader] WHERE CustomerID=" + studentID;
                if (startTime.HasValue && endTime.HasValue)
                {
                    string text2 = text;
                    text = string.Concat(new string[]
                    {
                        text2,
                        " AND OrderDate BETWEEN CONVERT(datetime,'",
                        startTime.Value.Day.ToString(),
                        "-",
                        startTime.Value.Month.ToString(),
                        "-",
                        startTime.Value.Year.ToString(),
                        " 00:00:00.000') AND convert(datetime,'",
                        endTime.Value.Day.ToString(),
                        "-",
                        endTime.Value.Month.ToString(),
                        "-",
                        endTime.Value.Year.ToString(),
                        " 23:59:59.998')\r\n"
                    });
                }
            }
            else
            {
                text = "SELECT SaleID,CustomerID,EmployeeID,OrderDate,TotalAmt FROM [Sales].[SaleHeader]";
                if (startTime.HasValue && endTime.HasValue)
                {
                    string text2 = text;
                    text = string.Concat(new string[]
                    {
                        text2,
                        " WHERE OrderDate BETWEEN CONVERT(datetime,'",
                        startTime.Value.Day.ToString(),
                        "-",
                        startTime.Value.Month.ToString(),
                        "-",
                        startTime.Value.Year.ToString(),
                        " 00:00:00.000') AND convert(datetime,'",
                        endTime.Value.Day.ToString(),
                        "-",
                        endTime.Value.Month.ToString(),
                        "-",
                        endTime.Value.Year.ToString(),
                        " 23:59:59.998')\r\n"
                    });
                }
            }
            DataTable dataTable = DataAccessHelper.ExecuteNonQueryWithResultTable(text);
            ObservableCollection<SaleModel> result;
            if (dataTable.Rows.Count == 0)
            {
                result = new ObservableCollection<SaleModel>();
            }
            else
            {
                ObservableCollection<SaleModel> observableCollection = new ObservableCollection<SaleModel>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    SaleModel saleModel = new SaleModel();
                    saleModel.SaleID = int.Parse(dataRow[0].ToString());
                    if (studentID.HasValue)
                    {
                        saleModel.CustomerID = studentID.Value;
                    }
                    else
                    {
                        saleModel.CustomerID = int.Parse(dataRow[1].ToString());
                    }
                    saleModel.EmployeeID = int.Parse(dataRow[2].ToString());
                    saleModel.DateAdded = DateTime.Parse(dataRow[3].ToString());
                    saleModel.OrderTotal = decimal.Parse(dataRow[4].ToString());

                    if (includeAllDetails)
                    {
                        saleModel.SaleItems = DataAccess.GetSaleItems(saleModel.SaleID);
                    }
                    observableCollection.Add(saleModel);
                }
                result = observableCollection;
            }
            return result;
        }

        public static Task<bool> CheckIfRegisteredForExam(int studentID, int examID)
        {
            return Task.Run<bool>(()=>
            {
                string commandText = "IF EXISTS (SELECT * FROM [Institution].[ExamStudentDetail] WHERE StudentID="
                + studentID + " AND ExamID=" + examID+") SELECT '1' ELSE SELECT '0'";
                int result;
                int.TryParse(DataAccessHelper.ExecuteScalar(commandText), out result);
                return (result==1);
            });
        

    }

        public static Task<GeneralLedgerModel> GetGeneralLedgerAsync(GeneralLedgerAccounts accType, DateTime from, DateTime to)
        {
            return Task.Run<GeneralLedgerModel>(() =>
            {
                GeneralLedgerModel temp = new GeneralLedgerModel();
                string selectStr;
                switch(accType)
                {
                    case GeneralLedgerAccounts.AccountsPayable:
                        {
                            temp.AccountName = "Accounts Payable";
                            var t = GetItemReceipts(false, null, from, to);
                            
                            List<TransactionModel> r = new List<TransactionModel>();

                            foreach (var y in t)
                                r.Add(new TransactionModel() { TransactionID = "PUR-" + y.PurchaseID, TransactionAmt = y.OrderTotal, TransactionDateTime = y.OrderDate, TransactionType = TransactionTypes.Credit });
                            
                            var f = r.OrderBy(o => o.TransactionDateTime);
                            foreach (var h in f)
                                temp.Entries.Add(new TransactionModel() { TransactionID = h.TransactionID, TransactionAmt = h.TransactionAmt, TransactionDateTime = h.TransactionDateTime, TransactionType = h.TransactionType });
                            break;
                        }
                    case GeneralLedgerAccounts.AccountsReceivable:
                        {
                            temp.AccountName = "Accounts Receivable";
                            var t = GetSales(false, null, from, to);

                            List<TransactionModel> r = new List<TransactionModel>();

                            foreach (var y in t)
                                r.Add(new TransactionModel() { TransactionID = "SALE-" + y.SaleID, TransactionAmt = y.OrderTotal, TransactionDateTime = y.DateAdded, TransactionType = TransactionTypes.Debit });

                            var f = r.OrderBy(o => o.TransactionDateTime);
                            foreach (var h in f)
                                temp.Entries.Add(new TransactionModel() { TransactionID = h.TransactionID, TransactionAmt = h.TransactionAmt, TransactionDateTime = h.TransactionDateTime, TransactionType = h.TransactionType });
                            break;
                        }
                    case GeneralLedgerAccounts.Cash:
                        {
                            temp.AccountName = "Cash Account";
                            var t = GetFeesPayments(null, from, to, "CASH");
                            List<TransactionModel> r = new List<TransactionModel>();

                            foreach (var y in t)
                                r.Add(new TransactionModel() { TransactionID = "PMT-" + y.FeePaymentID, TransactionAmt = y.AmountPaid, TransactionDateTime = y.DatePaid, TransactionType = TransactionTypes.Debit });

                            var f = r.OrderBy(o => o.TransactionDateTime);
                            foreach (var h in f)
                                temp.Entries.Add(new TransactionModel() { TransactionID = h.TransactionID, TransactionAmt = h.TransactionAmt, TransactionDateTime = h.TransactionDateTime, TransactionType = h.TransactionType });
                            break;
                        }
                    case GeneralLedgerAccounts.OtherExpenses:
                        {
                            temp.AccountName = "Other Expenses";
                            var t =  GetPaymentVouchersAsync(false,from,to).Result;

                            List<TransactionModel> r = new List<TransactionModel>();

                            foreach (var y in t)
                                r.Add(new TransactionModel() { TransactionID = "EXP-" + y.PaymentVoucherID, TransactionAmt = y.Total, TransactionDateTime = y.DatePaid, TransactionType = TransactionTypes.Debit });

                            var f = r.OrderBy(o => o.TransactionDateTime);
                            foreach (var h in f)
                                temp.Entries.Add(new TransactionModel() { TransactionID = h.TransactionID, TransactionAmt = h.TransactionAmt, TransactionDateTime = h.TransactionDateTime, TransactionType = h.TransactionType });
                            break;
                        }
                    case GeneralLedgerAccounts.Salaries: temp.AccountName = "Payroll Expenses"; break;
                    case GeneralLedgerAccounts.Sales:
                        {
                            temp.AccountName = "Sales";
                            var t = GetSales(false, null, from, to);

                            List<TransactionModel> r = new List<TransactionModel>();

                            foreach (var y in t)
                                r.Add(new TransactionModel() { TransactionID = "SALE-" + y.SaleID, TransactionAmt = y.OrderTotal, TransactionDateTime = y.DateAdded, TransactionType = TransactionTypes.Credit });

                            var f = r.OrderBy(o => o.TransactionDateTime);
                            foreach (var h in f)
                                temp.Entries.Add(new TransactionModel() { TransactionID = h.TransactionID, TransactionAmt = h.TransactionAmt, TransactionDateTime = h.TransactionDateTime, TransactionType = h.TransactionType });
                            break;
                        }
                    case GeneralLedgerAccounts.OtherRevenue: temp.AccountName = "Other Revenue"; break;
                }

                temp.Date = DateTime.Now;
                

                return temp;
            });
        }

        public static Task<bool> SaveNewExamRegistrationAsync(ExamRegistrationStudentModel student)
        {
            return Task.Run<bool>(()=>
            {
                string text = 
                    "BEGIN TRANSACTION\r\n"+
                    "INSERT INTO [Institution].[ExamStudentDetail] (ExamID,StudentID) VALUES ("+ student.ExamID+
                    ","+student.StudentID+
                    ")\r\n"+" COMMIT";
                return DataAccessHelper.ExecuteNonQuery(text);
            });
        }

        private static ObservableCollection<FeePaymentModel> GetFeesPayments(int? studentID, DateTime? startTime, DateTime? endTime, string paymentMethod)
        {
            string text;
            if (studentID.HasValue)
            {
                text = "SELECT s.NameOfStudent,fp.StudentID,fp.AmountPaid,fp.DatePaid,fp.PaymentMethod FROM [Institution].[FeesPayment] fp LEFT OUTER JOIN [Institution].[Student]s ON (fp.StudentID = s.StudentID) WHERE fp.StudentID =" + studentID;
                if (startTime.HasValue && endTime.HasValue)
                {
                    string text2 = text;
                    text = string.Concat(new string[]
                    {
                        text2,
                        " AND fp.DatePaid BETWEEN CONVERT(datetime,'",
                        startTime.Value.Day.ToString(),
                        "/",
                        startTime.Value.Month.ToString(),
                        "/",
                        startTime.Value.Year.ToString(),
                        " 00:00:00.000') AND CONVERT(datetime,'",
                        endTime.Value.Day.ToString(),
                        "/",
                        endTime.Value.Month.ToString(),
                        "/",
                        endTime.Value.Year.ToString(),
                        " 23:59:59.998')"
                    });
                }
                if (paymentMethod != "ALL")
                {
                    text = text + " AND fp.PaymentMethod='" + paymentMethod + "'\r\n ORDER BY fp.FeesPaymentID";
                }
            }
            else
            {
                text = "SELECT s.NameOfStudent,fp.StudentID,fp.AmountPaid,fp.DatePaid,fp.PaymentMethod FROM [Institution].[FeesPayment] fp LEFT OUTER JOIN [Institution].[Student]s ON (fp.StudentID = s.StudentID)";
                if (startTime.HasValue && endTime.HasValue)
                {
                    string text2 = text;
                    text = string.Concat(new string[]
                    {
                        text2,
                        " WHERE fp.DatePaid BETWEEN CONVERT(datetime,'",
                        startTime.Value.Day.ToString(),
                        "/",
                        startTime.Value.Month.ToString(),
                        "/",
                        startTime.Value.Year.ToString(),
                        " 00:00:00.000') AND CONVERT(datetime,'",
                        endTime.Value.Day.ToString(),
                        "/",
                        endTime.Value.Month.ToString(),
                        "/",
                        endTime.Value.Year.ToString(),
                        " 23:59:59.998')"
                    });
                    if (paymentMethod != "ALL")
                    {
                        text = text + " AND fp.PaymentMethod='" + paymentMethod + "'";
                    }
                }
                else if (paymentMethod != "ALL")
                {
                    text = text + " WHERE fp.PaymentMethod='" + paymentMethod + "'";
                }
                text += "\r\n ORDER BY fp.FeesPaymentID";
            }
            DataTable dataTable = DataAccessHelper.ExecuteNonQueryWithResultTable(text);
            ObservableCollection<FeePaymentModel> result;
            if (dataTable.Rows.Count == 0)
            {
                result = new ObservableCollection<FeePaymentModel>();
            }
            else
            {
                ObservableCollection<FeePaymentModel> observableCollection = new ObservableCollection<FeePaymentModel>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    observableCollection.Add(new FeePaymentModel
                    {
                        NameOfStudent = dataRow[0].ToString(),
                        StudentID = int.Parse(dataRow[1].ToString()),
                        AmountPaid = decimal.Parse(dataRow[2].ToString()),
                        DatePaid = DateTime.Parse(dataRow[3].ToString()),
                        PaymentMethod = dataRow[4].ToString()
                    });
                }
                result = observableCollection;
            }
            return result;
        }

        public static Task<bool> SaveNewCombinedClassExamRegistrationAsync(CombinedClassModel selectedCombinedClass, int selectedExamID)
        {
            return Task.Run<bool>(() =>
            {
                string classes = "0,";
                foreach (var v in selectedCombinedClass.Entries)
                    classes += v.ClassID + ",";
                classes = classes.Remove(classes.Length - 1);
                string selecteStr = "SELECT StudentID FROM [Institution].[Student] WHERE ClassID IN(" + classes + ")";

                ObservableCollection<string> list = DataAccessHelper.CopyFromDBtoObservableCollection(selecteStr);

                string text =
                    "BEGIN TRANSACTION\r\n";
                foreach (var t in list)
                    text +="IF NOT EXISTS (SELECT * FROM [Institution].[ExamStudentDetail] WHERE StudentID="+t+" AND ExamID="+selectedExamID+")\r\n"+
                    "INSERT INTO [Institution].[ExamStudentDetail] (ExamID,StudentID) VALUES (" + selectedExamID +
                    "," + t +
                    ")\r\n";
                text+= " COMMIT";
                return DataAccessHelper.ExecuteNonQuery(text);
            });
        }

        public static Task<bool> SaveNewClassExamRegistrationAsync(int selectedClassID, int selectedExamID)
        {
            return Task.Run<bool>(() =>
            {
                string selecteStr = "SELECT StudentID FROM [Institution].[Student] WHERE ClassID ="+selectedClassID;

                ObservableCollection<string> list = DataAccessHelper.CopyFromDBtoObservableCollection(selecteStr);

                string text =
                    "BEGIN TRANSACTION\r\n";
                foreach (var t in list)
                    text += "IF NOT EXISTS (SELECT * FROM [Institution].[ExamStudentDetail] WHERE StudentID=" + t + " AND ExamID=" + selectedExamID + ")\r\n" +
                    "INSERT INTO [Institution].[ExamStudentDetail] (ExamID,StudentID) VALUES (" + selectedExamID +
                    "," + t +
                    ")\r\n";
                text += " COMMIT";
                return DataAccessHelper.ExecuteNonQuery(text);
            });
        }

        public static Task<ObservableImmutableList<ExamModel>> GetRegisteredExams(int studentID)
        {
            return Task.Run<ObservableImmutableList<ExamModel>>(delegate
            {
                ObservableImmutableList<ExamModel> observableCollection = new ObservableImmutableList<ExamModel>();
                string commandText =

                    "SELECT e.NameOfExam, es.ExamID, e.OutOf,e.ExamDateTime FROM [Institution].[ExamHeader] e RIGHT OUTER JOIN " +
                    "[Institution].[ExamStudentDetail] es ON (es.ExamID=e.ExamID) WHERE es.StudentID=" + studentID;
                DataTable dataTable = DataAccessHelper.ExecuteNonQueryWithResultTable(commandText);
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    observableCollection.Add(new ExamModel
                    {
                        NameOfExam = dataRow[0].ToString(),
                        ExamID = int.Parse(dataRow[1].ToString()),
                        OutOf = decimal.Parse(dataRow[2].ToString()),
                        ExamDateTime = DateTime.Parse(dataRow[3].ToString())
                    });
                }
                return observableCollection;
            });
        }

        public static Task<ObservableCollection<ExamResultStudentSubjectEntryModel>> GetStudentSubjectsResults(int classID, int examID, int subjectID, decimal outOf)
        {
            return Task.Run<ObservableCollection<ExamResultStudentSubjectEntryModel>>(delegate
            {
                ObservableCollection<ExamResultStudentSubjectEntryModel> observableCollection = new ObservableCollection<ExamResultStudentSubjectEntryModel>();
                string commandText = string.Concat(new object[]
                {
                    "SELECT s.StudentID, s.NameOfStudent, ISNULL(erd.Score,0),ISNULL(erd.Remarks,''),ISNULL(erh.ExamResultID,0), sub.NameOfSubject FROM [Institution].[StudentSubjectSelectionDetail] sssd LEFT OUTER JOIN [Institution].[StudentSubjectSelectionHeader] sssh ON (sssd.StudentSubjectSelectionID=sssh.StudentSubjectSelectionID) LEFT OUTER JOIN [Institution].[Student] s ON (sssh.StudentID=s.StudentID) LEFT OUTER JOIN (SELECT * FROM [Institution].[ExamResultHeader] WHERE ExamID=",
                    examID,
                    " AND IsActive=1) erh ON (sssh.StudentID=erh.StudentID) LEFT OUTER JOIN [Institution].[ExamresultDetail] erd ON (erh.ExamresultID=erd.ExamResultID AND sssd.SubjectID=erd.SubjectID) LEFT OUTER JOIN [Institution].[Subject] sub ON (sssd.SubjectID=sub.SubjectID) WHERE sssd.SubjectID=",
                    subjectID,
                    " AND s.ClassID=",
                    classID,
                    " AND s.IsActive=1"
                });
                DataTable dataTable = DataAccessHelper.ExecuteNonQueryWithResultTable(commandText);
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    observableCollection.Add(new ExamResultStudentSubjectEntryModel
                    {
                        SubjectID = subjectID,
                        ExamResultID = int.Parse(dataRow[4].ToString()),
                        StudentID = int.Parse(dataRow[0].ToString()),
                        NameOfStudent = dataRow[1].ToString(),
                        Score = decimal.Parse(dataRow[2].ToString()),
                        OutOf = outOf,
                        Remarks = dataRow[3].ToString(),
                        NameOfSubject = dataRow[5].ToString()
                    });
                }
                return observableCollection;
            });
        }

        public static Task<ObservableCollection<FeesStructureModel>> GetFullFeesStructure(DateTime currentDate)
        {
            return Task.Run<ObservableCollection<FeesStructureModel>>(delegate
            {
                ObservableCollection<FeesStructureModel> observableCollection = new ObservableCollection<FeesStructureModel>();
                ObservableCollection<CombinedClassModel> result = DataAccess.GetAllCombinedClassesAsync().Result;
                ObservableCollection<FeesStructureModel> result2;
                using (IEnumerator<CombinedClassModel> enumerator = result.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        CombinedClassModel c = enumerator.Current;
                        FeesStructureModel feesStructureModel = new FeesStructureModel();
                        string text = currentDate.Date.ToString("g");
                        string commandText = "DECLARE @id int\r\nSET @id=(SELECT TOP 1 FeesStructureID FROM [Institution].[FeesStructureHeader] WHERE ClassID=" + c.Entries[0].ClassID + "\r\nAND IsActive=1)\r\nSELECT ISNULL(@id,0)";
                        feesStructureModel.NameOfCombinedClass = result.First((CombinedClassModel o) => o.Entries.Any((ClassModel a) => a.ClassID == c.Entries[0].ClassID)).Description;
                        int num = int.Parse(DataAccessHelper.ExecuteScalar(commandText));
                        if (num <= 0)
                        {
                            result2 = observableCollection;
                            return result2;
                        }
                        commandText = "SELECT Name, Amount FROM [Institution].[FeesStructureDetail] WHERE FeesStructureID =" + num;
                        DataTable dataTable = DataAccessHelper.ExecuteNonQueryWithResultTable(commandText);
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

        public static Task<ObservableCollection<FeePaymentModel>> GetRecentPaymentsAsync(StudentBaseModel student)
        {
            return Task.Run<ObservableCollection<FeePaymentModel>>(delegate
            {
                ObservableCollection<FeePaymentModel> observableCollection = new ObservableCollection<FeePaymentModel>();
                string commandText = "SELECT TOP 20 FeesPaymentID,AmountPaid, DatePaid, PaymentMethod FROM [Institution].[FeesPayment] WHERE StudentID =" + student.StudentID + " ORDER BY [DatePaid] desc";
                DataTable dataTable = DataAccessHelper.ExecuteNonQueryWithResultTable(commandText);
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    observableCollection.Add(new FeePaymentModel
                    {
                        FeePaymentID = int.Parse(dataRow[0].ToString()),
                        AmountPaid = decimal.Parse(dataRow[1].ToString()),
                        StudentID = student.StudentID,
                        NameOfStudent = student.NameOfStudent,
                        DatePaid = DateTime.Parse(dataRow[2].ToString()),
                        PaymentMethod=dataRow[3].ToString()
                    });
                }
                return observableCollection;
            });
        }

        public static Task<FeePaymentReceiptModel> GetReceiptAsync(FeePaymentModel currentPayment, ObservableImmutableList<FeesStructureEntryModel> currentFeesStructure)
        {
            return Task.Run<FeePaymentReceiptModel>(delegate
            {
                FeePaymentReceiptModel feePaymentReceiptModel = new FeePaymentReceiptModel();
                feePaymentReceiptModel.FeePaymentID = currentPayment.FeePaymentID;
                feePaymentReceiptModel.AmountPaid = currentPayment.AmountPaid;
                feePaymentReceiptModel.Entries = currentFeesStructure;
                feePaymentReceiptModel.DatePaid = currentPayment.DatePaid;
                feePaymentReceiptModel.NameOfClass = DataAccess.GetClassAsync(DataAccess.GetClassIDFromStudentID(currentPayment.StudentID).Result).Result.NameOfClass;
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
                feesStructureEntryModel3.Amount = DataAccess.GetBalanceBroughtForwardAsync(feePaymentReceiptModel.StudentID, currentPayment.FeePaymentID, currentPayment.DatePaid).Result;
                feesStructureEntryModel3.Name = "BALANCE B/F";
                FeesStructureEntryModel feesStructureEntryModel4 = new FeesStructureEntryModel();
                feesStructureEntryModel4.Amount = DataAccess.GetCurrentBalanceAsync(feePaymentReceiptModel.StudentID).Result;
                feesStructureEntryModel4.Name = "TOTAL BALANCE";
                feePaymentReceiptModel.Entries.Add(feesStructureEntryModel);
                feePaymentReceiptModel.Entries.Add(feesStructureEntryModel2);
                feePaymentReceiptModel.Entries.Add(feesStructureEntryModel3);
                feePaymentReceiptModel.Entries.Add(feesStructureEntryModel4);
                return feePaymentReceiptModel;
            });
        }

        public static Task<bool> SaveNewEmployeePaymentAsync(EmployeePaymentModel payment)
        {
            return Task.Run<bool>(delegate
            {
                string text = string.Concat(new object[]
                {
                    "BEGIN TRANSACTION\r\ndeclare @id int; SET @id = [dbo].GetNewID('Institution.EmployeePayment') INSERT INTO [Institution].[EmployeePayment] (EmployeePaymentID,EmployeeID,AmountPaid,DatePaid,Notes) VALUES (@id,",
                    payment.StaffID,
                    ",",
                    payment.Amount,
                    ",'",
                    payment.DatePaid.ToString("g"),
                    "','",
                    payment.Notes,
                    "')\r\n"
                });
                text += " COMMIT";
                return DataAccessHelper.ExecuteNonQuery(text);
            });
        }

        public static Task<bool> SaveNewTimeTable(TimeTableModel timeTable)
        {
            return Task.Run<bool>(delegate
            {
                string text = "BEGIN TRANSACTION\r\ndeclare @id int; ";
                ObservableCollection<ClassModel> result = DataAccess.GetAllClassesAsync().Result;
                using (IEnumerator<ClassModel> enumerator = result.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        ClassModel c = enumerator.Current;
                        object obj = text;
                        text = string.Concat(new object[]
                        {
                            obj,
                            "IF EXISTS(SELECT * FROM [Institution].[TimeTableHeader] WHERE ClassID=",
                            c.ClassID,
                            " AND IsActive=1)\r\nSET @id=(SELECT TimeTableID FROM [Institution].[TimeTableHeader] WHERE ClassID=",
                            c.ClassID,
                            " AND IsActive=1)\r\nELSE\r\nBEGIN\r\nSET @id = [dbo].GetNewID('Institution.TimeTableHeader')INSERT INTO [Institution].[TimeTableHeader] (TimeTableID,ClassID) VALUES (@id,",
                            c.ClassID,
                            ")END\r\n"
                        });
                        text += "DELETE FROM [Institution].[TimeTableDetail] WHERE TimeTableID=@id;\r\n";
                        IEnumerable<ClassLessons> enumerable = from o in timeTable
                                                               where o.ClassID == c.ClassID
                                                               select o;
                        foreach (ClassLessons current in enumerable)
                        {
                            foreach (Lesson current2 in current)
                            {
                                obj = text;
                                text = string.Concat(new object[]
                                {
                                    obj,
                                    "INSERT INTO [Institution].[TimeTableDetail] (TimeTableID,SubjectIndex,NameOfSubject,Tutor,[Day],StartTime,EndTime) VALUES (@id,",
                                    current2.SubjectIndex,
                                    ",'",
                                    current2.Subject,
                                    "','",
                                    current2.Tutor,
                                    "','",
                                    current.Day,
                                    "','",
                                    current2.StartTime.ToString(),
                                    "','",
                                    current2.EndTime.ToString(),
                                    "')\r\n"
                                });
                            }
                        }
                    }
                }
                text = (text ?? "");
                text += " COMMIT";
                return DataAccessHelper.ExecuteNonQuery(text);
            });
        }

        public static Task<bool> SaveNewGalleryItemsAsync(ObservableCollection<GalleryItemModel> galleryItems)
        {
            return Task.Run<bool>(delegate
            {
                bool result = true;
                try
                {
                    string text = "BEGIN TRANSACTION\r\n";
                    int num = 0;
                    ObservableCollection<SqlParameter> observableCollection = new ObservableCollection<SqlParameter>();
                    foreach (GalleryItemModel current in galleryItems)
                    {
                        object obj = text;
                        text = string.Concat(new object[]
                        {
                            obj,
                            "INSERT INTO [Institution].[Gallery] (Name,DateAdded,Data) VALUES('",
                            current.Name,
                            "','",
                            DateTime.Now.ToString("g"),
                            "',@item",
                            num,
                            ")\r\n"
                        });
                        object result2 = DataAccess.GetGallerItemDataFromPathAsync(current.Path).Result;
                        observableCollection.Add(new SqlParameter("@item" + num, SqlDbType.Binary)
                        {
                            Value = (result2 == null) ? DBNull.Value : result2
                        });
                        num++;
                    }
                    text += "COMMIT";
                    result = DataAccessHelper.ExecuteNonQueryWithParameters(text, observableCollection);
                }
                catch
                {
                }
                return result;
            });
        }

        public static Task<byte[]> GetGallerItemDataFromPathAsync(string path)
        {
            return Task.Run<byte[]>(delegate
            {
                byte[] result = null;
                try
                {
                    result = File.ReadAllBytes(path);
                }
                catch
                {
                }
                return result;
            });
        }

        public static async Task<ObservableCollection<TimetableClassModel>> GetCurrentTimeTableAsync(int day)
        {
            ObservableCollection<TimetableClassModel> observableCollection = new ObservableCollection<TimetableClassModel>();
            ObservableCollection<ClassModel> observableCollection2 = await DataAccess.GetAllClassesAsync();
            Task[] array = new Task[observableCollection2.Count];
            for (int i = 0; i < observableCollection2.Count; i++)
            {
                array[i] = DataAccess.GetClassTimetableAsync(observableCollection2[i].ClassID, day);
            }
            await Task.WhenAll(array);
            foreach (ClassModel current in observableCollection2)
            {
                observableCollection.Add(new TimetableClassModel
                {
                    ClassID = current.ClassID,
                    NameOfClass = current.NameOfClass
                });
            }
            using (IEnumerator<TimetableClassModel> var_15 = observableCollection.GetEnumerator())
            {
                while (var_15.MoveNext())
                {
                    TimetableClassModel ttcm = var_15.Current;
                    ttcm.Entries = ((from o in array
                                     where (o as Task<TimetableClassModel>).Result.ClassID == ttcm.ClassID
                                     select o).First<Task>() as Task<TimetableClassModel>).Result.Entries;
                }
            }
            return observableCollection;
        }

        public static Task<ClassLessons> GetClassTimetableAsync(int classID, int day)
        {
            return Task.Run<ClassLessons>(delegate
            {
                ClassLessons classLessons = new ClassLessons();
                string commandText = string.Concat(new object[]
                {
                    "SELECT s.NameOfSubject, t.Tutor, t.[Day], t.StartTime, t.EndTime FROM (SELECT td.SubjectID, td.Tutor, td.[Day], td.StartTime, td.EndTime FROM [Institution].[TimeTableHeader] th LEFT OUTER JOIN [Institution].[TimeTableDetail] td ON (th.TimeTableID = td.TimeTableID) WHERE th.ClassID=",
                    classID,
                    " AND th.IsActive=1) t LEFT OUTER JOIN [Institution].[Subject] s ON (t.SubjectID=s.SubjectID) WHERE t.[Day]='",
                    ((DayOfWeek)day).ToString(),
                    "'"
                });
                classLessons.ClassID = classID;
                DataTable dataTable = DataAccessHelper.ExecuteNonQueryWithResultTable(commandText);
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    Lesson item = new Lesson();
                    classLessons.Add(item);
                }
                return classLessons;
            });
        }

        public static Task<bool> UpdateStudentAsync(StudentModel student)
        {
            return Task.Run<bool>(delegate
            {
                string commandText = string.Concat(new object[]
                {
                    "UPDATE [Institution].[Student] SET FirstName='",
                    student.FirstName,
                    "', LastName='",
                    student.LastName,
                    "', MiddleName='",
                    student.MiddleName,
                    "', Gender='",
                    student.Gender,
                    "', DateOfAdmission='",
                    student.DateOfAdmission,
                    "', DateOfBirth='",
                    student.DateOfBirth,
                    "', NameOfGuardian='",
                    student.NameOfGuardian,
                    "', GuardianPhoneNo='",
                    student.GuardianPhoneNo,
                    "', Email='",
                    student.Email,
                    "', Address='",
                    student.Address,
                    "', PostalCode='",
                    student.PostalCode,
                    "', City='",
                    student.City,
                    "', PreviousInstitution='",
                    student.PrevInstitution,
                    "', KCPEScore=",
                    student.KCPEScore,
                    (student.DormitoryID > 0) ? string.Concat(new object[]
                    {
                        ", DormitoryID=",
                        student.DormitoryID,
                        ", BedNo='",
                        student.BedNo,
                        "'"
                    }) : "",
                    ", PreviousBalance='",
                    student.PrevBalance,
                    "', SPhoto=@photo WHERE StudentID=",
                    student.StudentID
                });
                return DataAccessHelper.ExecuteNonQueryWithParameters(commandText, new ObservableCollection<SqlParameter>
                {
                    new SqlParameter("@photo", student.SPhoto)
                });
            });
        }

        public static Task<bool> UpdateStaffAsync(StaffModel staff)
        {
            return Task.Run<bool>(delegate
            {
                string commandText = string.Concat(new object[]
                {
                    "UPDATE [Institution].[Staff] SET Name='",
                    staff.Name,
                    "', NationalID='",
                    staff.NationalID,
                    "', DateOfAdmission='",
                    staff.DateOfAdmission,
                    "', PhoneNo='",
                    staff.PhoneNo,
                    "', Email='",
                    staff.Email,
                    "', Address='",
                    staff.Address,
                    "', PostalCode='",
                    staff.PostalCode,
                    "', City='",
                    staff.City,
                    "', SPhoto=@photo WHERE StaffID=",
                    staff.StaffID
                });
                return DataAccessHelper.ExecuteNonQueryWithParameters(commandText, new ObservableCollection<SqlParameter>
                {
                    new SqlParameter("@photo", staff.SPhoto)
                });
            });
        }

        private static DataTable GetItemIssues(bool includeAllDetails, DateTime? startTime, DateTime? endTime)
        {
            string text = "SELECT iih.Description,i.Description as NameOfItem, iid.Quantity, iih.DateIssued,iih.IsCancelled FROM [Sales].[ItemIssueDetail] iid LEFT OUTER JOIN [Sales].[ItemIssueHeader] iih ON (iih.ItemIssueID=iid.ItemIssueID) LEFT OUTER JOIN [Sales].[Item] i ON (iid.ItemID=i.ItemID)";
            if (startTime.HasValue && endTime.HasValue)
            {
                string text2 = text;
                text = string.Concat(new string[]
                {
                    text2,
                    " WHERE DateIssued BETWEEN '",
                    startTime.Value.Day.ToString(),
                    "/",
                    startTime.Value.Month.ToString(),
                    "/",
                    startTime.Value.Year.ToString(),
                    " 00:00:00.000' AND '",
                    endTime.Value.Day.ToString(),
                    "/",
                    endTime.Value.Month.ToString(),
                    "/",
                    endTime.Value.Year.ToString(),
                    " 23:59:59.998'"
                });
            }
            return DataAccessHelper.ExecuteNonQueryWithResultTable(text);
        }

        private static ObservableCollection<PurchaseModel> GetItemReceipts(bool includeAllDetails, int? supplierID, DateTime? startTime, DateTime? endTime)
        {
            string text;
            if (supplierID.HasValue)
            {
                text = "SELECT sh.ItemReceiptID,sh.OrderDate,TotalAmt,SupplierID,IsCancelled,ISNULL(SUM(ISNULL(sd.Quantity,0)),0),RefNo FROM [Sales].[ItemReceiptHeader] sh LEFT OUTER JOIN [Sales].[ItemReceiptDetail] sd ON(sh.ItemReceiptID=sd.ItemReceiptID) WHERE sh.SupplierID =" + supplierID;
                if (startTime.HasValue && endTime.HasValue)
                {
                    string text2 = text;
                    text = string.Concat(new string[]
                    {
                        text2,
                        " AND sh.OrderDate BETWEEN '",
                        startTime.Value.Day.ToString(),
                        "/",
                        startTime.Value.Month.ToString(),
                        "/",
                        startTime.Value.Year.ToString(),
                        " 00:00:00.000' AND '",
                        endTime.Value.Day.ToString(),
                        "/",
                        endTime.Value.Month.ToString(),
                        "/",
                        endTime.Value.Year.ToString(),
                        " 23:59:59.998'\r\n GROUP BY sh.ItemReceiptID,sh.OrderDate, TotalAmt,SupplierID,IsCancelled,RefNo"
                    });
                }
            }
            else
            {
                text = "SELECT sh.ItemReceiptID,sh.OrderDate,TotalAmt,SupplierID,IsCancelled, ISNULL(SUM(ISNULL(sd.Quantity,0)),0),RefNo FROM [Sales].[ItemReceiptHeader] sh LEFT OUTER JOIN [Sales].[ItemReceiptDetail] sd ON(sh.ItemReceiptID=sd.ItemReceiptID)";
                if (startTime.HasValue && endTime.HasValue)
                {
                    string text2 = text;
                    text = string.Concat(new string[]
                    {
                        text2,
                        " WHERE sh.OrderDate BETWEEN '",
                        startTime.Value.Day.ToString(),
                        "/",
                        startTime.Value.Month.ToString(),
                        "/",
                        startTime.Value.Year.ToString(),
                        " 00:00:00.000' AND '",
                        endTime.Value.Day.ToString(),
                        "/",
                        endTime.Value.Month.ToString(),
                        "/",
                        endTime.Value.Year.ToString(),
                        " 23:59:59.998'\r\n GROUP BY sh.ItemReceiptID,sh.OrderDate, TotalAmt,SupplierID,IsCancelled,RefNo"
                    });
                }
            }
            DataTable dataTable = DataAccessHelper.ExecuteNonQueryWithResultTable(text);
            ObservableCollection<PurchaseModel> result;
            if (dataTable.Rows.Count == 0)
            {
                result = new ObservableCollection<PurchaseModel>();
            }
            else
            {
                ObservableCollection<PurchaseModel> observableCollection = new ObservableCollection<PurchaseModel>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    PurchaseModel purchaseModel = new PurchaseModel();
                    purchaseModel.PurchaseID = int.Parse(dataRow[0].ToString());
                    purchaseModel.OrderDate = DateTime.Parse(dataRow[1].ToString());
                    purchaseModel.OrderTotal = decimal.Parse(dataRow[2].ToString());
                    if (supplierID.HasValue)
                    {
                        purchaseModel.SupplierID = supplierID.Value;
                    }
                    else
                    {
                        purchaseModel.SupplierID = int.Parse(dataRow[3].ToString());
                    }
                    purchaseModel.IsCancelled = bool.Parse(dataRow[4].ToString());
                    purchaseModel.NoOfItems = decimal.Parse(dataRow[5].ToString());
                    purchaseModel.RefNo = dataRow[6].ToString();
                    if (includeAllDetails)
                    {
                        purchaseModel.Items = DataAccess.GetItemsReceiptItems(purchaseModel.PurchaseID);
                        purchaseModel.NoOfItems = purchaseModel.Items.Count;
                    }
                    observableCollection.Add(purchaseModel);
                }
                result = observableCollection;
            }
            return result;
        }

        private static ObservableCollection<BooksPurchaseModel> GetBookReceipts(bool includeAllDetails, int? supplierID, DateTime? startTime, DateTime? endTime)
        {
            string text;
            if (supplierID.HasValue)
            {
                text = "SELECT sh.BookReceiptID,sh.DateReceived,TotalAmt,SupplierID,IsCancelled,ISNULL(SUM(ISNULL(sd.Quantity,0)),0),RefNo FROM [Sales].[BookReceiptHeader] sh LEFT OUTER JOIN [Sales].[BookReceiptDetail] sd ON(sh.BookReceiptID=sd.BookReceiptID) WHERE sh.SupplierID =" + supplierID;
                if (startTime.HasValue && endTime.HasValue)
                {
                    string text2 = text;
                    text = string.Concat(new string[]
                    {
                        text2,
                        " AND sh.DateReceived BETWEEN CONVERT(datetime,'",
                        startTime.Value.Day.ToString(),
                        "/",
                        startTime.Value.Month.ToString(),
                        "/",
                        startTime.Value.Year.ToString(),
                        " 00:00:00.000') AND CONVERT(datetime,'",
                        endTime.Value.Day.ToString(),
                        "/",
                        endTime.Value.Month.ToString(),
                        "/",
                        endTime.Value.Year.ToString(),
                        " 23:59:59.998')\r\n GROUP BY sh.BookReceiptID,sh.DateReceived, TotalAmt,SupplierID,IsCancelled,RefNo"
                    });
                }
            }
            else
            {
                text = "SELECT sh.BookReceiptID,sh.DateReceived,TotalAmt,SupplierID,IsCancelled, ISNULL(SUM(ISNULL(sd.Quantity,0)),0),RefNo FROM [Sales].[BookReceiptHeader] sh LEFT OUTER JOIN [Sales].[BookReceiptDetail] sd ON(sh.BookReceiptID=sd.BookReceiptID)";
                if (startTime.HasValue && endTime.HasValue)
                {
                    string text2 = text;
                    text = string.Concat(new string[]
                    {
                        text2,
                        " WHERE sh.DateReceived BETWEEN CONVERT(datetime,'",
                        startTime.Value.Day.ToString(),
                        "/",
                        startTime.Value.Month.ToString(),
                        "/",
                        startTime.Value.Year.ToString(),
                        " 00:00:00.000') AND CONVERT(datetime,'",
                        endTime.Value.Day.ToString(),
                        "/",
                        endTime.Value.Month.ToString(),
                        "/",
                        endTime.Value.Year.ToString(),
                        " 23:59:59.998')\r\n GROUP BY sh.BookReceiptID,sh.DateReceived, TotalAmt,SupplierID,IsCancelled,RefNo"
                    });
                }
            }
            DataTable dataTable = DataAccessHelper.ExecuteNonQueryWithResultTable(text);
            ObservableCollection<BooksPurchaseModel> result;
            if (dataTable.Rows.Count == 0)
            {
                result = new ObservableCollection<BooksPurchaseModel>();
            }
            else
            {
                ObservableCollection<BooksPurchaseModel> observableCollection = new ObservableCollection<BooksPurchaseModel>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    BooksPurchaseModel booksPurchaseModel = new BooksPurchaseModel();
                    booksPurchaseModel.PurchaseID = int.Parse(dataRow[0].ToString());
                    booksPurchaseModel.OrderDate = DateTime.Parse(dataRow[1].ToString());
                    booksPurchaseModel.OrderTotal = decimal.Parse(dataRow[2].ToString());
                    if (supplierID.HasValue)
                    {
                        booksPurchaseModel.SupplierID = supplierID.Value;
                    }
                    else
                    {
                        booksPurchaseModel.SupplierID = int.Parse(dataRow[3].ToString());
                    }
                    booksPurchaseModel.IsCancelled = bool.Parse(dataRow[4].ToString());
                    booksPurchaseModel.NoOfItems = decimal.Parse(dataRow[5].ToString());
                    booksPurchaseModel.RefNo = dataRow[6].ToString();
                    if (includeAllDetails)
                    {
                        booksPurchaseModel.Items = DataAccess.GetBooksReceiptItems(booksPurchaseModel.PurchaseID);
                        booksPurchaseModel.NoOfItems = booksPurchaseModel.Items.Count;
                    }
                    observableCollection.Add(booksPurchaseModel);
                }
                result = observableCollection;
            }
            return result;
        }

        public static ObservableCollection<ItemPurchaseModel> GetItemsReceiptItems(int saleId)
        {
            ObservableCollection<ItemPurchaseModel> observableCollection = new ObservableCollection<ItemPurchaseModel>();
            string commandText = "SELECT sod.ItemID,p.Description,sod.UnitPrice,sod.Quantity FROM Sales.SaleDetail sod LEFT OUTER JOIN Sales.Item p ON( sod.ItemID = p.ItemID) WHERE sod.SaleID = " + saleId;
            DataTable dataTable = DataAccessHelper.ExecuteNonQueryWithResultTable(commandText);
            foreach (DataRow dataRow in dataTable.Rows)
            {
                long itemID = long.Parse(dataRow[0].ToString());
                decimal buyingPrice;
                decimal.TryParse(dataRow[2].ToString(), out buyingPrice);
                decimal quantity;
                decimal.TryParse(dataRow[3].ToString(), out quantity);
                observableCollection.Add(new ItemPurchaseModel(itemID, dataRow[1].ToString(), quantity, buyingPrice));
            }
            return observableCollection;
        }

        public static ObservableCollection<BookReceiptModel> GetBooksReceiptItems(int bookReceiptID)
        {
            ObservableCollection<BookReceiptModel> observableCollection = new ObservableCollection<BookReceiptModel>();
            string commandText = "SELECT sod.BookID,p.Author,p.Title,p.ISBN,,sod.UnitPrice,sod.Quantity FROM [Sales].[BookReceiptDetail] sod LEFT OUTER JOIN [Institution].[Book] p ON( sod.BookID = p.BookID) WHERE sod.BookReceiptID = " + bookReceiptID;
            DataTable dataTable = DataAccessHelper.ExecuteNonQueryWithResultTable(commandText);
            foreach (DataRow dataRow in dataTable.Rows)
            {
                observableCollection.Add(new BookReceiptModel
                {
                    BookID = int.Parse(dataRow[0].ToString()),
                    Author = dataRow[1].ToString(),
                    Title = dataRow[2].ToString(),
                    ISBN = dataRow[3].ToString(),
                    Price = decimal.Parse(dataRow[4].ToString()),
                    Quantity = decimal.Parse(dataRow[5].ToString())
                });
            }
            return observableCollection;
        }

        public static ObservableCollection<ItemIssueModel> GetItemsIssueItems(int issueID)
        {
            ObservableCollection<ItemIssueModel> observableCollection = new ObservableCollection<ItemIssueModel>();
            string commandText = "SELECT sod.ItemID,p.Description,sod.Quantity FROM [Sales].[ItemIssueDetail] sod LEFT OUTER JOIN [Sales].[Item] p ON( sod.ItemID = p.ItemID) WHERE sod.ItemIssueID = " + issueID;
            DataTable dataTable = DataAccessHelper.ExecuteNonQueryWithResultTable(commandText);
            foreach (DataRow dataRow in dataTable.Rows)
            {
                long itemID = long.Parse(dataRow[0].ToString());
                decimal num;
                decimal.TryParse(dataRow[2].ToString(), out num);
                decimal quantity;
                decimal.TryParse(dataRow[3].ToString(), out quantity);
                observableCollection.Add(new ItemIssueModel
                {
                    ItemID = itemID,
                    Description = dataRow[1].ToString(),
                    Quantity = quantity
                });
            }
            return observableCollection;
        }

        public static Task<DataTable> GetItemIssuesAsync(bool includeAllDetails, DateTime? startTime, DateTime? endTime)
        {
            return Task.Run<DataTable>(() => DataAccess.GetItemIssues(includeAllDetails, startTime, endTime));
        }

        public static Task<ObservableCollection<PurchaseModel>> GetItemReceiptsAsync(bool includeAllDetails, int? supplierID, DateTime? startTime, DateTime? endTime)
        {
            return Task.Run<ObservableCollection<PurchaseModel>>(() => DataAccess.GetItemReceipts(includeAllDetails, supplierID, startTime, endTime));
        }

        public static Task<ObservableCollection<BooksPurchaseModel>> GetBookReceiptsAsync(bool includeAllDetails, int? supplierID, DateTime? startTime, DateTime? endTime)
        {
            return Task.Run<ObservableCollection<BooksPurchaseModel>>(() => DataAccess.GetBookReceipts(includeAllDetails, supplierID, startTime, endTime));
        }

        public static Task<ObservableCollection<EmployeePaymentModel>> GetEmployeePaymentsAsync(int? employeeId, DateTime? from, DateTime? to)
        {
            return Task.Run<ObservableCollection<EmployeePaymentModel>>(() => DataAccess.GetEmployeePayments(employeeId, from, to));
        }

        private static ObservableCollection<EmployeePaymentModel> GetEmployeePayments(int? employeeId, DateTime? from, DateTime? to)
        {
            ObservableCollection<EmployeePaymentModel> observableCollection = new ObservableCollection<EmployeePaymentModel>();
            ObservableCollection<EmployeePaymentModel> result;
            try
            {
                string text = "SELECT ep.EmployeePaymentID,ep.EmployeeID,s.Name, ep.AmountPaid,ep.DatePaid,ep.Notes FROM [Institution].[EmployeePayment] ep LEFT OUTER JOIN [Institution].[Staff] s ON (ep.EmployeeID=s.StaffID)";
                if (employeeId.HasValue)
                {
                    text = text + " WHERE ep.EmployeeID =" + employeeId;
                    if (from.HasValue && to.HasValue)
                    {
                        string text2 = text;
                        text = string.Concat(new string[]
                        {
                            text2,
                            " AND ep.DatePaid BETWEEN '",
                            from.Value.Day.ToString(),
                            "/",
                            from.Value.Month.ToString(),
                            "/",
                            from.Value.Year.ToString(),
                            " 00:00:00.000' AND '",
                            to.Value.Day.ToString(),
                            "/",
                            to.Value.Month.ToString(),
                            "/",
                            to.Value.Year.ToString(),
                            " 23:59:59.998'"
                        });
                    }
                }
                else if (from.HasValue && to.HasValue)
                {
                    string text2 = text;
                    text = string.Concat(new string[]
                    {
                        text2,
                        " WHERE ep.DatePaid BETWEEN '",
                        from.Value.Day.ToString(),
                        "/",
                        from.Value.Month.ToString(),
                        "/",
                        from.Value.Year.ToString(),
                        " 00:00:00.000' AND '",
                        to.Value.Day.ToString(),
                        "/",
                        to.Value.Month.ToString(),
                        "/",
                        to.Value.Year.ToString(),
                        " 23:59:59.998'"
                    });
                }
                DataTable dataTable = DataAccessHelper.ExecuteNonQueryWithResultTable(text);
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    observableCollection.Add(new EmployeePaymentModel
                    {
                        EmployeePaymentID = int.Parse(dataRow[0].ToString()),
                        StaffID = int.Parse(dataRow[1].ToString()),
                        Name = dataRow[2].ToString(),
                        Amount = decimal.Parse(dataRow[3].ToString()),
                        DatePaid = DateTime.Parse(dataRow[4].ToString()),
                        Notes = dataRow[5].ToString()
                    });
                }
                result = observableCollection;
            }
            catch
            {
                result = new ObservableCollection<EmployeePaymentModel>();
            }
            return result;
        }


        public static Task<ObservableCollection<TransactionModel>> GetAccountsTransactionHistoryAsync(TransactionTypes type, DateTime? from, DateTime? to)
        {
            return Task.Run<ObservableCollection<TransactionModel>>(() => DataAccess.GetAccountsTransactionHistory(type,from, to));
        }

        private static ObservableCollection<TransactionModel> GetAccountsTransactionHistory(TransactionTypes type, DateTime? from, DateTime? to)
        {
            ObservableCollection<TransactionModel> observableCollection = new ObservableCollection<TransactionModel>();
            
            try
            {
                string text = "SELECT SaleID,OrderDate, TotalAmt FROM [Sales].[SaleHeader] ";
                if (from.HasValue && to.HasValue)
                {
                    string text2 = text;
                    text = string.Concat(new string[]
                    {
                            text2,
                            "WHERE OrderDate BETWEEN CONVERT(DATE,'",
                            from.Value.ToString("dd-MM-yyyy"),
                            " 00:00:00.000') AND CONVERT(DATE,'",
                            to.Value.ToString("dd-MM-yyyy"),
                            " 23:59:59.998')"
                    });
                }
                string text3 = "SELECT FeesPaymentID, DatePaid, AmountPaid FROM [Institution].[FeesPayment] ";
                if (from.HasValue && to.HasValue)
                {
                    string text2 = text3;
                    text3 = string.Concat(new string[]
                    {
                            text2,
                            "WHERE DatePaid BETWEEN CONVERT(DATETIME, '",
                            from.Value.ToString("dd-MM-yyyy"),
                            " 00:00:00.000') AND CONVERT(DATETIME, '",
                            to.Value.ToString("dd-MM-yyyy"),
                            " 23:59:59.998')"
                    });
                }
                DataTable dataTable = null;
                DataTable dataTable2 = null;
                if ((type ==  TransactionTypes.Debit)|| (type==TransactionTypes.All))
                    dataTable = DataAccessHelper.ExecuteNonQueryWithResultTable(text);
                if ((type==  TransactionTypes.Credit)|| (type==TransactionTypes.All))
                    dataTable2 = DataAccessHelper.ExecuteNonQueryWithResultTable(text3);

                if (dataTable!=null)
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    DateTime transactionDateTime;
                    DateTime.TryParse(dataRow[1].ToString(), out transactionDateTime);
                    decimal num;
                    decimal.TryParse(dataRow[2].ToString(), out num);
                    observableCollection.Add(new TransactionModel(TransactionTypes.Credit, "SALE-"+dataRow[0].ToString(), transactionDateTime, num));

                }

                if (dataTable2 != null)
                    foreach (DataRow dataRow in dataTable2.Rows)
                {
                    DateTime transactionDateTime;
                    DateTime.TryParse(dataRow[1].ToString(), out transactionDateTime);
                    decimal num;
                    decimal.TryParse(dataRow[2].ToString(), out num);
                    observableCollection.Add(new TransactionModel(TransactionTypes.Credit, "PMT-"+dataRow[0].ToString(), transactionDateTime, num));
                    
                }
                IEnumerable<TransactionModel> enumerable = from fruit in observableCollection
                                                           orderby fruit.TransactionDateTime
                                                           select fruit;
                observableCollection = new ObservableCollection<TransactionModel>(enumerable);


            }
            catch
            {
            }
            return observableCollection;
        }

        public static Task<ObservableCollection<FeesPaymentHistoryModel>> GetFeesPaymentsHistoryAsync(DateTime? from, DateTime? to)
        {
            return Task.Run<ObservableCollection<FeesPaymentHistoryModel>>(() => DataAccess.GetFeesPaymentsHistory(from, to));
        }

        private static ObservableCollection<FeesPaymentHistoryModel> GetFeesPaymentsHistory( DateTime? from, DateTime? to)
        {
            ObservableCollection<FeesPaymentHistoryModel> observableCollection = new ObservableCollection<FeesPaymentHistoryModel>();
            ObservableCollection<FeesPaymentHistoryModel> result;
            try
            {
                string text = "SELECT ISNULL(PaymentMethod,''), SUM(CONVERT(decimal(18,0),AmountPaid)) FROM [Institution].[FeesPayment] ";
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
                DataTable dataTable = DataAccessHelper.ExecuteNonQueryWithResultTable(text);
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    observableCollection.Add(new FeesPaymentHistoryModel
                    {
                        PaymentMode = string.IsNullOrWhiteSpace(dataRow[0].ToString())?"UNSET":
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


        public static Task<ObservableCollection<PayslipModel>> GetPayslipsAsync(int? employeeId, DateTime? from, DateTime? to)
        {
            return Task.Run<ObservableCollection<PayslipModel>>(() => DataAccess.GetPayslips(employeeId, from, to));
        }

        private static ObservableCollection<PayslipModel> GetPayslips(int? employeeId, DateTime? from, DateTime? to)
        {
            ObservableCollection<PayslipModel> observableCollection = new ObservableCollection<PayslipModel>();
            ObservableCollection<PayslipModel> result;
            try
            {
                string text = "SELECT ep.PayslipModel,ep.StaffID,s.Name, ep.AmountPaid,ep.DatePaid,ep.Designation FROM [Institution].[PayslipHeader] ep LEFT OUTER JOIN [Institution].[Staff] s ON (ep.StaffID=s.StaffID)";
                if (employeeId.HasValue)
                {
                    text = text + " WHERE ep.StaffID =" + employeeId;
                    if (from.HasValue && to.HasValue)
                    {
                        string text2 = text;
                        text = string.Concat(new string[]
                        {
                            text2,
                            " AND ep.DatePaid BETWEEN CONVERT(datetime,'",
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
                }
                else if (from.HasValue && to.HasValue)
                {
                    string text2 = text;
                    text = string.Concat(new string[]
                    {
                        text2,
                        " WHERE ep.DatePaid BETWEEN CONVERT(datetime,'",
                        from.Value.Day.ToString(),
                        "/",
                        from.Value.Month.ToString(),
                        "/",
                        from.Value.Year.ToString(),
                        " 00:00:00.000' AND CONVERT(datetime,'",
                        to.Value.Day.ToString(),
                        "/",
                        to.Value.Month.ToString(),
                        "/",
                        to.Value.Year.ToString(),
                        " 23:59:59.998')"
                    });
                }
                DataTable dataTable = DataAccessHelper.ExecuteNonQueryWithResultTable(text);
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    observableCollection.Add(new PayslipModel
                    {
                        PayslipID = int.Parse(dataRow[0].ToString()),
                        StaffID = int.Parse(dataRow[1].ToString()),
                        Name = dataRow[2].ToString(),
                        AmountPaid = decimal.Parse(dataRow[3].ToString()),
                        DatePaid = DateTime.Parse(dataRow[4].ToString()),
                        Designation = dataRow[5].ToString()
                    });
                }
                result = observableCollection;
            }
            catch
            {
                result = new ObservableCollection<PayslipModel>();
            }
            return result;
        }

        public static Task<ObservableCollection<SupplierPaymentModel>> GetSupplierPaymentsAsync(int? supplierId, DateTime? from, DateTime? to)
        {
            return Task.Run<ObservableCollection<SupplierPaymentModel>>(() => DataAccess.GetSupplierPayments(supplierId, from, to));
        }

        private static ObservableCollection<SupplierPaymentModel> GetSupplierPayments(int? supplierId, DateTime? from, DateTime? to)
        {
            ObservableCollection<SupplierPaymentModel> observableCollection = new ObservableCollection<SupplierPaymentModel>();
            ObservableCollection<SupplierPaymentModel> result;
            try
            {
                string text = "SELECT sp.SupplierPaymentID,sp.SupplierID,s.NameOfSupplier, sp.AmountPaid,sp.DatePaid,sp.Notes FROM [Sales].[SupplierPayment] sp LEFT OUTER JOIN [Sales].[Supplier] s ON (sp.SupplierID=s.SupplierID)";
                if (supplierId.HasValue)
                {
                    text = text + " WHERE sp.SupplierID =" + supplierId;
                    if (from.HasValue && to.HasValue)
                    {
                        string text2 = text;
                        text = string.Concat(new string[]
                        {
                            text2,
                            " AND sp.DatePaid BETWEEN '",
                            from.Value.Day.ToString(),
                            "/",
                            from.Value.Month.ToString(),
                            "/",
                            from.Value.Year.ToString(),
                            " 00:00:00.000' AND '",
                            to.Value.Day.ToString(),
                            "/",
                            to.Value.Month.ToString(),
                            "/",
                            to.Value.Year.ToString(),
                            " 23:59:59.998'"
                        });
                    }
                }
                else if (from.HasValue && to.HasValue)
                {
                    string text2 = text;
                    text = string.Concat(new string[]
                    {
                        text2,
                        " WHERE sp.DatePaid BETWEEN '",
                        from.Value.Day.ToString(),
                        "/",
                        from.Value.Month.ToString(),
                        "/",
                        from.Value.Year.ToString(),
                        " 00:00:00.000' AND '",
                        to.Value.Day.ToString(),
                        "/",
                        to.Value.Month.ToString(),
                        "/",
                        to.Value.Year.ToString(),
                        " 23:59:59.998'"
                    });
                }
                DataTable dataTable = DataAccessHelper.ExecuteNonQueryWithResultTable(text);
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    observableCollection.Add(new SupplierPaymentModel(int.Parse(dataRow[0].ToString()), int.Parse(dataRow[1].ToString()), dataRow[2].ToString(), decimal.Parse(dataRow[3].ToString()), DateTime.Parse(dataRow[4].ToString()), dataRow[5].ToString()));
                }
                result = observableCollection;
            }
            catch
            {
                result = new ObservableCollection<SupplierPaymentModel>();
            }
            return result;
        }

        public static Task<StockTakingResultsModel> GetStockTakingResults(int stockTakingID)
        {
            return Task.Run<StockTakingResultsModel>(delegate
            {
                StockTakingResultsModel stockTakingResultsModel = new StockTakingResultsModel();
                string commandText = "SELECT std.ItemID,i.Description,std.AvailableQuantity,std.Expected,std.VarianceQty,CASE (dbo.GetCurrentQuantity([std].[ItemID])) \r\nWHEN 0 THEN 0\r\nELSE \r\nstd.VariancePc/dbo.GetCurrentQuantity([std].[ItemID]) END FROM [Sales].[StockTakingDetail] std LEFT OUTER JOIN [Sales].[Item] i ON( std.ItemID = i.ItemID) WHERE std.StockTakingID = " + stockTakingID;
                DataTable dataTable = DataAccessHelper.ExecuteNonQueryWithResultTable(commandText);
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    ItemStockTakingResultsModel itemStockTakingResultsModel = new ItemStockTakingResultsModel();
                    itemStockTakingResultsModel.ItemID = long.Parse(dataRow[0].ToString());
                    itemStockTakingResultsModel.Description = dataRow[1].ToString();
                    itemStockTakingResultsModel.Counted = decimal.Parse(dataRow[2].ToString());
                    itemStockTakingResultsModel.Expected = decimal.Parse(dataRow[3].ToString());
                    itemStockTakingResultsModel.VarianceQty = decimal.Parse(dataRow[4].ToString());
                    itemStockTakingResultsModel.VariancePc = decimal.Parse(dataRow[5].ToString());
                    stockTakingResultsModel.Items.Add(itemStockTakingResultsModel);
                }
                return stockTakingResultsModel;
            });
        }

        public static Task<bool> SaveNewSupplierPaymentAsync(SupplierPaymentModel newPayment)
        {
            return Task.Run<bool>(delegate
            {
                string commandText = string.Concat(new object[]
                {
                    "INSERT INTO [Sales].[SupplierPayment] (SupplierID,DatePaid,AmountPaid,Notes) VALUES(",
                    newPayment.SupplierID,
                    ",'",
                    newPayment.DatePaid.ToString("g"),
                    "',",
                    newPayment.Amount,
                    ",'",
                    newPayment.Notes,
                    "')"
                });
                return DataAccessHelper.ExecuteNonQuery(commandText);
            });
        }

        public static Task<bool> RemoveSupplierAsync(int supplierID)
        {
            return Task.Run<bool>(delegate
            {
                string commandText = "DELETE FROM [Sales].[Supplier] WHERE SupplierID=" + supplierID + ";";
                return DataAccessHelper.ExecuteNonQuery(commandText);
            });
        }

        public static Task<bool> UpdateSupplierAsync(SupplierModel newSupplier)
        {
            return Task.Run<bool>(delegate
            {
                string commandText = string.Concat(new object[]
                {
                    "UPDATE [Sales].[Supplier] SET NameOfSupplier='",
                    newSupplier.NameOfSupplier,
                    "', PhoneNo='",
                    newSupplier.PhoneNo,
                    "', AltPhoneNo='",
                    newSupplier.AltPhoneNo,
                    "', Email='",
                    newSupplier.Email,
                    "', Address='",
                    newSupplier.Address,
                    "', PostalCode='",
                    newSupplier.PostalCode,
                    "', City='",
                    newSupplier.City,
                    "', PINNo='",
                    newSupplier.PINNo,
                    "' WHERE SupplierID=",
                    newSupplier.SupplierID
                });
                return DataAccessHelper.ExecuteNonQuery(commandText);
            });
        }

        public static Task<SupplierModel> GetSupplierAsync(int supplierID)
        {
            return Task.Run<SupplierModel>(() => DataAccess.GetSupplier(supplierID));
        }

        public static SupplierModel GetSupplier(int supplierID)
        {
            SupplierModel supplierModel = new SupplierModel();
            string commandText = "SELECT SupplierID, NameOfSupplier, PhoneNo, AltPhoneNo, Email, Address, PostalCode, City, PINNo FROM [Sales].[Supplier] WHERE SupplierID=" + supplierID;
            DataTable dataTable = DataAccessHelper.ExecuteNonQueryWithResultTable(commandText);
            if (dataTable.Rows.Count >= 1)
            {
                DataRow dataRow = dataTable.Rows[0];
                supplierModel.SupplierID = int.Parse(dataRow[0].ToString());
                supplierModel.NameOfSupplier = dataRow[1].ToString();
                supplierModel.PhoneNo = dataRow[2].ToString();
                supplierModel.AltPhoneNo = dataRow[3].ToString();
                supplierModel.Email = dataRow[4].ToString();
                supplierModel.Address = dataRow[5].ToString();
                supplierModel.PostalCode = dataRow[6].ToString();
                supplierModel.City = dataRow[7].ToString();
                supplierModel.PINNo = dataRow[8].ToString();
            }
            return supplierModel;
        }

        public static Task<bool> SaveNewItemAsync(ItemModel item)
        {
            return Task.Run<bool>(delegate
            {
                string commandText = string.Concat(new object[]
                {
                    "INSERT INTO [Sales].[Item] (ItemID,Description,DateAdded,ItemCategoryID,Price,Cost,VatID,StartQuantity) VALUES(",
                    item.ItemID,
                    ",'",
                    item.Description,
                    "','",
                    item.DateAdded.ToString("g"),
                    "',",
                    item.ItemCategoryID,
                    ",",
                    item.Price,
                    ",",
                    item.Cost,
                    ",",
                    item.VatID,
                    ",",
                    item.StartQuantity,
                    ")"
                });
                return DataAccessHelper.ExecuteNonQuery(commandText);
            });
        }

        public static Task<bool> SaveNewItemCategoryAsync(ItemCategoryModel itemCategory)
        {
            return Task.Run<bool>(delegate
            {
                string commandText = "INSERT INTO [Sales].[ItemCategory] (Description) VALUES('" + itemCategory.Description + "')";
                return DataAccessHelper.ExecuteNonQuery(commandText);
            });
        }

        public static Task<bool> SaveNewVATRateAsync(VATRateModel rate)
        {
            return Task.Run<bool>(delegate
            {
                string commandText = string.Concat(new object[]
                {
                    "INSERT INTO [Sales].[VAT] (Description,Rate) VALUES('",
                    rate.Description,
                    "',",
                    rate.Rate,
                    ")"
                });
                return DataAccessHelper.ExecuteNonQuery(commandText);
            });
        }

        public static Task<bool> SaveNewSupplierAsync(SupplierModel newSupplier)
        {
            return Task.Run<bool>(delegate
            {
                string commandText;
                if (newSupplier.SupplierID <= 0)
                {
                    commandText = string.Concat(new string[]
                    {
                        "INSERT INTO [Sales].[Supplier] (NameOfSupplier,PhoneNo,AltPhoneNo,Email, Address, PostalCode, City,PINNo) VALUES('",
                        newSupplier.NameOfSupplier,
                        "','",
                        newSupplier.PhoneNo,
                        "','",
                        newSupplier.AltPhoneNo,
                        "','",
                        newSupplier.Email,
                        "','",
                        newSupplier.Address,
                        "','",
                        newSupplier.PostalCode,
                        "','",
                        newSupplier.City,
                        "','",
                        newSupplier.PINNo,
                        "')"
                    });
                }
                else
                {
                    commandText = string.Concat(new object[]
                    {
                        "INSERT INTO [Sales].[Supplier] (SupplierID, NameOfSupplier,PhoneNo,AltPhoneNo,Email, Address, PostalCode, City,PINNo) VALUES(",
                        newSupplier.SupplierID,
                        ",'",
                        newSupplier.NameOfSupplier,
                        "','",
                        newSupplier.PhoneNo,
                        "','",
                        newSupplier.AltPhoneNo,
                        "','",
                        newSupplier.Email,
                        "','",
                        newSupplier.Address,
                        "','",
                        newSupplier.PostalCode,
                        "','",
                        newSupplier.City,
                        "','",
                        newSupplier.PINNo,
                        "')"
                    });
                }
                return DataAccessHelper.ExecuteNonQuery(commandText);
            });
        }

        public static Task<bool> SaveNewDonorAsync(DonorModel newDonor)
        {
            return Task.Run<bool>(delegate
            {
                string commandText = string.Concat(new string[]
                {
                    "INSERT INTO [Institution].[Donor] (NameOfDonor,PhoneNo) VALUES('",
                    newDonor.NameOfDonor,
                    "','",
                    newDonor.PhoneNo,
                    "')"
                });
                return DataAccessHelper.ExecuteNonQuery(commandText);
            });
        }

        public static Task<bool> SaveNewStockTakingAsync(StockTakingModel newStockTaking)
        {
            return Task.Run<bool>(delegate
            {
                string text = "BEGIN TRANSACTION\r\nDECLARE @id int; SET @id = dbo.GetNewID('Sales.StockTakingHeader')\r\nINSERT INTO [Sales].[StockTakingHeader] (StockTakingID,DateTaken) VALUES(@id,'" + newStockTaking.DateTaken.Value.ToString("dd-MM-yyyy") + "')";
                foreach (ItemStockTakingModel current in newStockTaking.Items)
                {
                    object obj = text;
                    text = string.Concat(new object[]
                    {
                        obj,
                        "\r\nINSERT INTO [Sales].[StockTakingDetail] (StockTakingID,ItemID,AvailableQuantity) VALUES(@id,",
                        current.ItemID,
                        ",",
                        current.AvailableQuantity,
                        ")"
                    });
                }
                text += "\r\nCOMMIT";
                DataAccessHelper.ExecuteNonQuery(text);
                return true;
            });
        }

        public static Task<ObservableCollection<StockTakingBaseModel>> GetAllStockTakings()
        {
            return Task.Run<ObservableCollection<StockTakingBaseModel>>(delegate
            {
                ObservableCollection<StockTakingBaseModel> observableCollection = new ObservableCollection<StockTakingBaseModel>();
                string commandText = "SELECT StockTakingID,DateTaken FROM [Sales].[StockTakingHeader]";
                DataTable dataTable = DataAccessHelper.ExecuteNonQueryWithResultTable(commandText);
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    observableCollection.Add(new StockTakingBaseModel
                    {
                        StockTakingID = int.Parse(dataRow[0].ToString()),
                        DateTaken = new DateTime?(DateTime.Parse(dataRow[1].ToString()))
                    });
                }
                return observableCollection;
            });
        }

        public static Task<ObservableCollection<SupplierModel>> GetAllSuppliersFullAsync()
        {
            return Task.Run<ObservableCollection<SupplierModel>>(delegate
            {
                ObservableCollection<SupplierModel> observableCollection = new ObservableCollection<SupplierModel>();
                string commandText = "SELECT SupplierID,NameOfSupplier,PhoneNo,AltPhoneNo,Email,Address,PostalCode,City,PINNo FROM [Sales].[Supplier]";
                DataTable dataTable = DataAccessHelper.ExecuteNonQueryWithResultTable(commandText);
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    observableCollection.Add(new SupplierModel
                    {
                        SupplierID = int.Parse(dataRow[0].ToString()),
                        NameOfSupplier = dataRow[1].ToString(),
                        PhoneNo = dataRow[2].ToString(),
                        AltPhoneNo = dataRow[3].ToString(),
                        Email = dataRow[4].ToString(),
                        Address = dataRow[5].ToString(),
                        PostalCode = dataRow[6].ToString(),
                        City = dataRow[7].ToString(),
                        PINNo = dataRow[8].ToString()
                    });
                }
                return observableCollection;
            });
        }

        public static Task<ItemModel> GetItemAsync(long itemID)
        {
            return Task.Run<ItemModel>(() => DataAccess.GetItem(itemID));
        }

        public static Task<ObservableCollection<ItemListModel>> GetAllItemsWithCurrentQuantityAsync()
        {
            return Task.Run<ObservableCollection<ItemListModel>>(delegate
            {
                ObservableCollection<ItemListModel> observableCollection = new ObservableCollection<ItemListModel>();
                string commandText = "SELECT ItemID,Description,DateAdded,ItemCategoryID,Price,Cost,StartQuantity,VatID,dbo.GetCurrentQuantity(ItemID) FROM [Sales].[Item]";
                DataTable dataTable = DataAccessHelper.ExecuteNonQueryWithResultTable(commandText);
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    observableCollection.Add(new ItemListModel
                    {
                        ItemID = long.Parse(dataRow[0].ToString()),
                        Description = dataRow[1].ToString(),
                        DateAdded = DateTime.Parse(dataRow[2].ToString()),
                        ItemCategoryID = int.Parse(dataRow[3].ToString()),
                        Price = decimal.Parse(dataRow[4].ToString()),
                        Cost = decimal.Parse(dataRow[5].ToString()),
                        StartQuantity = decimal.Parse(dataRow[6].ToString()),
                        VatID = int.Parse(dataRow[7].ToString()),
                        CurrentQuantity = decimal.Parse(dataRow[8].ToString())
                    });
                }
                return observableCollection;
            });
        }

        public static Task<ObservableCollection<VATAnalysisModel>> GetVatAnalysisAsync(DateTime from, DateTime to)
        {
            return Task.Run<ObservableCollection<VATAnalysisModel>>(delegate
            {
                ObservableCollection<VATAnalysisModel> observableCollection = new ObservableCollection<VATAnalysisModel>();
                string commandText = string.Concat(new string[]
                {
                    "select v.VatID, v.Description, v.Rate, ISNULL(SUM(ISNULL(sd.LineTotal,0)),0), ISNULL(SUM(ISNULL(sd.LineTotal,0)),0)*0.16 FROM [Sales].[Vat] v LEFT OUTER JOIN [Sales].[Item] i ON (i.VatID=v.VatID) LEFT OUTER JOIN [Sales].[SaleDetail] sd ON (sd.ItemID=i.ItemID) LEFT OUTER JOIN [Sales].[SaleHeader] sh ON(sd.SaleID=sh.SaleID) WHERE sh.OrderDate BETWEEN '",
                    from.Day.ToString(),
                    "/",
                    from.Month.ToString(),
                    "/",
                    from.Year.ToString(),
                    " 00:00:00.000' AND '",
                    to.Day.ToString(),
                    "/",
                    to.Month.ToString(),
                    "/",
                    to.Year.ToString(),
                    " 23:59:59.998' "
                });
                DataTable dataTable = DataAccessHelper.ExecuteNonQueryWithResultTable(commandText);
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    observableCollection.Add(new VATAnalysisModel
                    {
                        VatID = int.Parse(dataRow[0].ToString()),
                        Description = dataRow[1].ToString(),
                        Rate = decimal.Parse(dataRow[2].ToString()),
                        SalesTaxable = decimal.Parse(dataRow[3].ToString()),
                        TotalVATCollected = decimal.Parse(dataRow[4].ToString())
                    });
                }
                return observableCollection;
            });
        }

        public static Task<bool> UpdateItemAsync(ItemModel item)
        {
            return Task.Run<bool>(delegate
            {
                string commandText = string.Concat(new object[]
                {
                    "UPDATE [Sales].[Item] SET Description='",
                    item.Description,
                    "', DateAdded='",
                    item.DateAdded.ToString("g"),
                    "', ItemCategoryID=",
                    item.ItemCategoryID,
                    ", Price=",
                    item.Price,
                    ", Cost=",
                    item.Cost,
                    ", StartQuantity=",
                    item.StartQuantity,
                    ", VatID=",
                    item.VatID,
                    " WHERE ItemID=",
                    item.ItemID
                });
                return DataAccessHelper.ExecuteNonQuery(commandText);
            });
        }

        public static Task<bool> SaveNewPurchaseAsync(PurchaseModel currentPurchase)
        {
            return Task.Run<bool>(delegate
            {
                string text = string.Concat(new object[]
                {
                    "BEGIN TRANSACTION\r\nDECLARE @id int; SET @id = dbo.GetNewID('Sales.ItemReceiptHeader')\r\nINSERT INTO [Sales].[ItemReceiptHeader] (ItemReceiptID,SupplierID,OrderDate,RefNo,IsCancelled) VALUES(@id,",
                    currentPurchase.SupplierID,
                    ",'",
                    currentPurchase.OrderDate.ToString("g"),
                    "','",
                    currentPurchase.RefNo,
                    "',",
                    currentPurchase.IsCancelled ? "1" : "0",
                    ")"
                });
                foreach (ItemPurchaseModel current in currentPurchase.Items)
                {
                    object obj = text;
                    text = string.Concat(new object[]
                    {
                        obj,
                        "\r\nINSERT INTO [Sales].[ItemReceiptDetail] (ItemReceiptID,ItemID,UnitCost,Quantity,LineTotal) VALUES(@id,",
                        current.ItemID,
                        ",",
                        current.Cost,
                        ",",
                        current.Quantity,
                        ",",
                        current.TotalAmt,
                        ")"
                    });
                }
                text += "\r\nCOMMIT";
                DataAccessHelper.ExecuteNonQuery(text);
                return true;
            });
        }

        public static Task<bool> SaveNewBooksPurchaseAsync(BooksPurchaseModel currentPurchase)
        {
            return Task.Run<bool>(delegate
            {
                string text = string.Concat(new object[]
                {
                    "BEGIN TRANSACTION\r\nDECLARE @id int; SET @id = dbo.GetNewID('Sales.BookReceiptHeader')\r\nINSERT INTO [Sales].[BookReceiptHeader] (BookReceiptID,SupplierID,DateReceived,RefNo,IsCancelled) VALUES(@id,",
                    currentPurchase.SupplierID,
                    ",'",
                    currentPurchase.OrderDate.ToString("g"),
                    "','",
                    currentPurchase.RefNo,
                    "',",
                    currentPurchase.IsCancelled ? "1" : "0",
                    ")"
                });
                foreach (BookReceiptModel current in currentPurchase.Items)
                {
                    object obj = text;
                    text = string.Concat(new object[]
                    {
                        obj,
                        "\r\nINSERT INTO [Sales].[BookReceiptDetail] (BookReceiptID,BookID,UnitCost,Quantity,LineTotal) VALUES(@id,",
                        current.BookID,
                        ",",
                        current.Cost,
                        ",",
                        current.Quantity,
                        ",",
                        current.TotalAmt,
                        ")"
                    });
                }
                text += "\r\nCOMMIT";
                DataAccessHelper.ExecuteNonQuery(text);
                return true;
            });
        }

        public static Task<ObservableCollection<ItemCategoryModel>> GetAllItemCategoriesAsync()
        {
            return Task.Run<ObservableCollection<ItemCategoryModel>>(delegate
            {
                ObservableCollection<ItemCategoryModel> observableCollection = new ObservableCollection<ItemCategoryModel>();
                string commandText = "SELECT ItemCategoryID,Description FROM [Sales].[ItemCategory]";
                DataTable dataTable = DataAccessHelper.ExecuteNonQueryWithResultTable(commandText);
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    observableCollection.Add(new ItemCategoryModel
                    {
                        ItemCategoryID = int.Parse(dataRow[0].ToString()),
                        Description = dataRow[1].ToString()
                    });
                }
                return observableCollection;
            });
        }

        public static Task<ObservableCollection<SupplierBaseModel>> GetAllSuppliersAsync()
        {
            return Task.Run<ObservableCollection<SupplierBaseModel>>(delegate
            {
                ObservableCollection<SupplierBaseModel> observableCollection = new ObservableCollection<SupplierBaseModel>();
                string commandText = "SELECT SupplierID,NameOfSupplier FROM [Sales].[Supplier]";
                DataTable dataTable = DataAccessHelper.ExecuteNonQueryWithResultTable(commandText);
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    observableCollection.Add(new SupplierBaseModel
                    {
                        SupplierID = int.Parse(dataRow[0].ToString()),
                        NameOfSupplier = dataRow[1].ToString()
                    });
                }
                return observableCollection;
            });
        }

        public static Task<ObservableCollection<VATRateModel>> GetAllVatsAsync()
        {
            return Task.Run<ObservableCollection<VATRateModel>>(delegate
            {
                ObservableCollection<VATRateModel> observableCollection = new ObservableCollection<VATRateModel>();
                string commandText = "SELECT VatID,Description,Rate FROM [Sales].[Vat]";
                DataTable dataTable = DataAccessHelper.ExecuteNonQueryWithResultTable(commandText);
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    observableCollection.Add(new VATRateModel
                    {
                        VatID = int.Parse(dataRow[0].ToString()),
                        Description = dataRow[1].ToString(),
                        Rate = decimal.Parse(dataRow[2].ToString())
                    });
                }
                return observableCollection;
            });
        }

        internal static ItemModel GetItem(long itemID)
        {
            ItemModel itemModel = new ItemModel();
            string commandText = "SELECT ItemID,Description,DateAdded,ItemCategoryID,Price,Cost,StartQuantity,VatID FROM [Sales].[Item] WHERE ItemID =" + itemID;
            DataTable dataTable = DataAccessHelper.ExecuteNonQueryWithResultTable(commandText);
            if (dataTable.Rows.Count > 0)
            {
                DataRow dataRow = dataTable.Rows[0];
                itemModel.ItemID = long.Parse(dataRow[0].ToString());
                itemModel.Description = dataRow[1].ToString();
                itemModel.DateAdded = DateTime.Parse(dataRow[2].ToString());
                itemModel.ItemCategoryID = int.Parse(dataRow[3].ToString());
                itemModel.Price = decimal.Parse(dataRow[4].ToString());
                itemModel.Cost = 1m;
                itemModel.StartQuantity = decimal.Parse(dataRow[6].ToString());
                itemModel.VatID = int.Parse(dataRow[7].ToString());
            }
            return itemModel;
        }

        private static Task<decimal> GetBalanceBroughtForwardAsync(int studentID, int paymentID, DateTime endTime)
        {
            return Task.Run<decimal>(delegate
            {
                string commandText = string.Concat(new object[]
                {
                    "DECLARE  @sal decimal=(SELECT SUM(ISNULL(CONVERT(DECIMAL,TotalAmt),0)) FROM  [Sales].[SaleHeader] WHERE CustomerID ='",
                    studentID,
                    "');\r\nDECLARE  @pur decimal=(SELECT SUM(ISNULL(CONVERT(DECIMAL,AmountPaid),0)) FROM  [Institution].[FeesPayment] WHERE StudentID =",
                    studentID,
                    " AND FeesPaymentID <> ",
                    paymentID,
                    " AND DatePaid<'",
                    endTime,
                    "');\r\nDECLARE  @prev decimal=(SELECT SUM(ISNULL(CONVERT(DECIMAL,PreviousBalance),0)) FROM  [Institution].[Student] WHERE StudentID=",
                    studentID,
                    ")\r\nselect (ISNULL(@sal,0)+ISNULL(@prev,0))-ISNULL(@pur,0)"
                });
                decimal result;
                decimal.TryParse(DataAccessHelper.ExecuteScalar(commandText), out result);
                return result;
            });
        }

        private static Task<decimal> GetBalanceBroughtForwardAsync(int studentID, DateTime endTime)
        {
            return Task.Run<decimal>(delegate
            {
                string commandText = string.Concat(new object[]
                {
                    "DECLARE  @sal decimal=(SELECT SUM(ISNULL(CONVERT(DECIMAL,TotalAmt),0)) FROM  [Sales].[SaleHeader] WHERE CustomerID ='",
                    studentID,
                    "');\r\nDECLARE  @pur decimal=(SELECT SUM(ISNULL(CONVERT(DECIMAL,AmountPaid),0)) FROM  [Institution].[FeesPayment] WHERE StudentID =",
                    studentID,
                    " AND DatePaid <'",
                    endTime.ToString("g"),
                    "');\r\nDECLARE  @prev decimal=(SELECT SUM(ISNULL(CONVERT(DECIMAL,PreviousBalance),0)) FROM  [Institution].[Student] WHERE StudentID=",
                    studentID,
                    ")\r\nselect (ISNULL(@sal,0)+ISNULL(@prev,0))-ISNULL(@pur,0)"
                });
                decimal result;
                decimal.TryParse(DataAccessHelper.ExecuteScalar(commandText), out result);
                return result;
            });
        }

        private static Task<decimal> GetCurrentBalanceAsync(int studentID)
        {
            return Task.Run<decimal>(delegate
            {
                string commandText = "SELECT dbo.GetCurrentBalance(" + studentID + ")";
                decimal result;
                decimal.TryParse(DataAccessHelper.ExecuteScalar(commandText), out result);
                return result;
            });
        }

        public static Task<FeesStatementModel> GetFeesStatementAsync(int studentID, DateTime? startTime, DateTime? endTime)
        {
            return Task.Run<FeesStatementModel>(delegate
            {
                FeesStatementModel result;
                if (studentID <= 0)
                {
                    result = new FeesStatementModel();
                }
                else
                {
                    FeesStatementModel feesStatementModel = new FeesStatementModel();
                    string text = "SELECT SaleID,OrderDate, TotalAmt FROM [Sales].[SaleHeader] WHERE [CustomerID] ='" + studentID + "'";
                    if (startTime.HasValue && endTime.HasValue)
                    {
                        string text2 = text;
                        text = string.Concat(new string[]
                        {
                            text2,
                            " AND CONVERT(DATE, OrderDate) BETWEEN '",
                            startTime.Value.ToString("dd-MM-yyyy"),
                            " 00:00:00.000' AND '",
                            endTime.Value.ToString("dd-MM-yyyy"),
                            " 23:59:59.998'"
                        });
                    }
                    string text3 = "SELECT FeesPaymentID, DatePaid, AmountPaid FROM [Institution].[FeesPayment]  WHERE [StudentID] ='" + studentID + "'";
                    if (startTime.HasValue && endTime.HasValue)
                    {
                        string text2 = text3;
                        text3 = string.Concat(new string[]
                        {
                            text2,
                            " AND CONVERT(DATE, DatePaid) BETWEEN '",
                            startTime.Value.ToString("dd-MM-yyyy"),
                            " 00:00:00.000' AND '",
                            endTime.Value.ToString("dd-MM-yyyy"),
                            " 23:59:59.998'"
                        });
                    }
                    DataTable dataTable = DataAccessHelper.ExecuteNonQueryWithResultTable(text);
                    DataTable dataTable2 = DataAccessHelper.ExecuteNonQueryWithResultTable(text3);
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
                    feesStatementModel.BalanceBroughtForward = DataAccess.GetBalanceBroughtForwardAsync(studentID, startTime.Value).Result;
                    feesStatementModel.Transactions.Add(new TransactionModel(TransactionTypes.Credit, "0", DateTime.Now, feesStatementModel.BalanceBroughtForward));
                    foreach (TransactionModel current in enumerable)
                    {
                        feesStatementModel.Transactions.Add(current);
                    }
                    feesStatementModel.StudentID = studentID;
                    feesStatementModel.From = startTime.Value;
                    feesStatementModel.To = endTime.Value;
                    feesStatementModel.TotalDue = DataAccess.GetCurrentBalanceAsync(studentID).Result;
                    result = feesStatementModel;
                }
                return result;
            });
        }

        public static Task<StudentModel> GetStudentAsync(int studentID)
        {
            return Task.Run<StudentModel>(() => DataAccess.GetStudent(studentID));
        }

        public static Task<StaffModel> GetStaffAsync(int staffID)
        {
            return Task.Run<StaffModel>(() => DataAccess.GetStaff(staffID));
        }

        public static StudentModel GetStudent(int studentID)
        {
            StudentModel studentModel = new StudentModel();
            string commandText = "SELECT FirstName,LastName,MiddleName,ClassID,DateOfBirth,DateOfAdmission,NameOfGuardian,GuardianPhoneNo,Email,Address,City,PostalCode,PreviousInstitution,KCPEScore,DormitoryID,BedNo,SPhoto,PreviousBalance,Gender,IsActive FROM [Institution].[Student] WHERE StudentID=" + studentID;
            DataTable dataTable = DataAccessHelper.ExecuteNonQueryWithResultTable(commandText);
            if (dataTable.Rows.Count != 0)
            {
                studentModel.StudentID = studentID;
                studentModel.FirstName = dataTable.Rows[0][0].ToString();
                studentModel.LastName = dataTable.Rows[0][1].ToString();
                studentModel.MiddleName = dataTable.Rows[0][2].ToString();
                studentModel.NameOfStudent = string.Concat(new string[]
                {
                    studentModel.FirstName,
                    " ",
                    studentModel.MiddleName,
                    " ",
                    studentModel.LastName
                });
                studentModel.ClassID = int.Parse(dataTable.Rows[0][3].ToString());
                studentModel.DateOfBirth = DateTime.Parse(dataTable.Rows[0][4].ToString());
                studentModel.DateOfAdmission = DateTime.Parse(dataTable.Rows[0][5].ToString());
                studentModel.NameOfGuardian = dataTable.Rows[0][6].ToString();
                studentModel.GuardianPhoneNo = dataTable.Rows[0][7].ToString();
                studentModel.Email = dataTable.Rows[0][8].ToString();
                studentModel.Address = dataTable.Rows[0][9].ToString();
                studentModel.City = dataTable.Rows[0][10].ToString();
                studentModel.PostalCode = dataTable.Rows[0][11].ToString();
                studentModel.PrevInstitution = dataTable.Rows[0][12].ToString();
                studentModel.KCPEScore = (string.IsNullOrWhiteSpace(dataTable.Rows[0][13].ToString()) ? 0 : int.Parse(dataTable.Rows[0][13].ToString()));
                studentModel.DormitoryID = (string.IsNullOrWhiteSpace(dataTable.Rows[0][14].ToString()) ? 0 : int.Parse(dataTable.Rows[0][14].ToString()));
                studentModel.BedNo = dataTable.Rows[0][15].ToString();
                studentModel.SPhoto = (byte[])dataTable.Rows[0][16];
                studentModel.PrevBalance = decimal.Parse(dataTable.Rows[0][17].ToString());
                studentModel.Gender = (Gender)Enum.Parse(typeof(Gender), dataTable.Rows[0][18].ToString());
                studentModel.IsActive = bool.Parse(dataTable.Rows[0][19].ToString());
            }
            return studentModel;
        }

        public static DonorModel GetDonor(int donorID)
        {
            DonorModel donorModel = new DonorModel();
            string commandText = "SELECT DonorID,NameOfDonor FROM [Institution].[Donor] WHERE DonorID=" + donorID;
            DataTable dataTable = DataAccessHelper.ExecuteNonQueryWithResultTable(commandText);
            if (dataTable.Rows.Count != 0)
            {
                donorModel.DonorID = donorID;
                donorModel.NameOfDonor = dataTable.Rows[0][1].ToString();
            }
            return donorModel;
        }

        public static StaffModel GetStaff(int staffID)
        {
            StaffModel staffModel = new StaffModel();
            try
            {
                string commandText = "SELECT Name,NationalID,DateOfAdmission,PhoneNo,Email,Address,City,PostalCode,SPhoto FROM [Institution].[Staff] WHERE StaffID='" + staffID + "'";
                DataTable dataTable = DataAccessHelper.ExecuteNonQueryWithResultTable(commandText);
                if (dataTable.Rows.Count != 0)
                {
                    staffModel.StaffID = staffID;
                    staffModel.Name = dataTable.Rows[0][0].ToString();
                    staffModel.NationalID = dataTable.Rows[0][1].ToString();
                    staffModel.DateOfAdmission = DateTime.Parse(dataTable.Rows[0][2].ToString());
                    staffModel.PhoneNo = dataTable.Rows[0][3].ToString();
                    staffModel.Email = dataTable.Rows[0][4].ToString();
                    staffModel.Address = dataTable.Rows[0][5].ToString();
                    staffModel.City = dataTable.Rows[0][6].ToString();
                    staffModel.PostalCode = dataTable.Rows[0][7].ToString();
                    staffModel.SPhoto = (byte[])dataTable.Rows[0][8];
                }
            }
            catch
            {
            }
            return staffModel;
        }

        public static Task<ObservableCollection<DormModel>> GetAllDormsAsync()
        {
            return Task.Run<ObservableCollection<DormModel>>(delegate
            {
                string commandText = "SELECT DormitoryID,NameOfDormitory FROM [Institution].[Dormitory]";
                DataTable dataTable = DataAccessHelper.ExecuteNonQueryWithResultTable(commandText);
                ObservableCollection<DormModel> observableCollection = new ObservableCollection<DormModel>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    observableCollection.Add(new DormModel(int.Parse(dataRow[0].ToString()), dataRow[1].ToString()));
                }
                return observableCollection;
            });
        }

        public static Task<ObservableCollection<DormitoryMemberModel>> GetDormitoryMembers(int dormitoryID)
        {
            return Task.Run<ObservableCollection<DormitoryMemberModel>>(delegate
            {
                ObservableCollection<DormitoryMemberModel> observableCollection = new ObservableCollection<DormitoryMemberModel>();
                string commandText = "SELECT StudentID,FirstName +' '+LastName+' '+MiddleName,BedNo FROM [Institution].[Student] WHERE DormitoryID=" + dormitoryID;
                DataTable dataTable = DataAccessHelper.ExecuteNonQueryWithResultTable(commandText);
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    observableCollection.Add(new DormitoryMemberModel
                    {
                        StudentID = int.Parse(dataRow[0].ToString()),
                        NameOfStudent = dataRow[1].ToString(),
                        BedNo = dataRow[2].ToString()
                    });
                }
                return observableCollection;
            });
        }

        public static bool StudentExists(int studentID)
        {
            string commandText = "SELECT StudentID FROM [Institution].[Student] WHERE StudentID ='" + studentID + "'";
            string value = DataAccessHelper.ExecuteScalar(commandText);
            return !string.IsNullOrWhiteSpace(value);
        }

        public static bool StudentIsCleared(int studentID)
        {
            string commandText = "IF EXISTS (SELECT StudentID FROM [Institution].[StudentClearance] WHERE StudentID =" + studentID + ")\r\nSELECT 'true' ELSE SELECT 'false'";
            string value = DataAccessHelper.ExecuteScalar(commandText);
            return bool.Parse(value);
        }

        public static bool RemoveSubject(string SubjectID)
        {
            bool result = false;
            try
            {
                string commandText = "DELETE FROM [Institution].[Subject] WHERE SubjectID = '" + SubjectID + "' ";
                result = DataAccessHelper.ExecuteNonQuery(commandText);
            }
            catch
            {
            }
            return result;
        }

        public static bool RemoveStudent(string StudentID)
        {
            bool result = false;
            try
            {
                string commandText = "DELETE FROM [Institution].[Student] WHERE StudentID = '" + StudentID + "' ";
                result = DataAccessHelper.ExecuteNonQuery(commandText);
            }
            catch
            {
            }
            return result;
        }

        public static Task<bool> RemovePaymentAsync(int paymentID)
        {
            return Task.Run<bool>(delegate
            {
                bool result = false;
                try
                {
                    string commandText = "DELETE FROM [Institution].[FeesPayment] WHERE FeesPaymentID = " + paymentID;
                    result = DataAccessHelper.ExecuteNonQuery(commandText);
                }
                catch
                {
                }
                return result;
            });
        }

        public static Task<bool> RemoveSaleAsync(int saleID)
        {
            return Task.Run<bool>(delegate
            {
                bool result = false;
                try
                {
                    string text = "DELETE FROM [Sales].[SaleHeader] WHERE SaleID = " + saleID;
                    text = text + "\r\nDELETE FROM [Sales].[SaleDetail] WHERE SaleID = " + saleID;
                    result = DataAccessHelper.ExecuteNonQuery(text);
                }
                catch
                {
                }
                return result;
            });
        }

        internal static int GetClassIDFromStudent(int studentID)
        {
            string commandText = "SELECT ClassID FROM [Institution].[Student] WHERE StudentID=" + studentID;
            int result;
            int.TryParse(DataAccessHelper.ExecuteScalar(commandText), out result);
            return result;
        }

        public static async Task<ObservableCollection<ClassModel>> GetAllClassesAsync()
        {
            return await DataAccess.GetCurrentRegistredClassesAsync();
        }

        public static Task<ObservableCollection<CombinedClassModel>> GetAllCombinedClassesAsync()
        {
            return DataAccess.GetCurrentRegistredCombinedClassesAsync();
        }

        public static Task<bool> SaveNewStudentAsync(StudentModel student)
        {
            return Task.Run<bool>(delegate
            {
                bool flag = student.StudentID == 0;
                string text = "BEGIN TRANSACTION\r\nDECLARE @id INT; SET @id=dbo.GetNewID('Institution.Student')INSERT INTO [Institution].[Student] (StudentID,FirstName,LastName,MiddleName,Gender,DateOfBirth,DateOfAdmission,NameOfGuardian,GuardianPhoneNo,Email,Address,City,PostalCode,IsBoarder";
                if (student.IsBoarder)
                {
                    text += ",DormitoryID ,BedNo";
                }
                string text2 = text;
                text = string.Concat(new string[]
                {
                    text2,
                    ",PreviousInstitution,KCPEScore,PreviousBalance,SPhoto) VALUES(",
                    flag ? "@id" : "@studID",
                    ",@firstName,@lastName,@middleName,@gender,@dob,@doa,@nameOfGuardian,@guardianPhoneNo,@email,@address,@city,@postalCode,",
                    student.IsBoarder ? "1" : "0",
                    ","
                });
                if (student.IsBoarder)
                {
                    text += "@dormID,@bedNo,";
                }
                text = text + "@prevInstitution,@kcpeScore,@prevBalance,@photo)\r\nINSERT INTO [Institution].[CurrentClass] (StudentID,ClassID,IsActive,StartDateTime,EndDateTime) "+
                "VALUES(" + (flag ? "@id" : "@studID") + ",@classID,1,'01-01-"+DateTime.Now.Year+"','31-12-"+DateTime.Now.Year+"')\r\nCOMMIT";
                return DataAccessHelper.ExecuteNonQueryWithParameters(text, new ObservableCollection<SqlParameter>
                {
                    new SqlParameter("@studID", student.StudentID),
                    new SqlParameter("@firstName", student.FirstName),
                    new SqlParameter("@middleName", student.MiddleName),
                    new SqlParameter("@lastName", student.LastName),
                    new SqlParameter("@gender", student.Gender),
                    new SqlParameter("@dob", student.DateOfBirth.ToString("g")),
                    new SqlParameter("@doa", student.DateOfAdmission.ToString("g")),
                    new SqlParameter("@nameOfGuardian", student.NameOfGuardian),
                    new SqlParameter("@guardianPhoneNo", student.GuardianPhoneNo),
                    new SqlParameter("@email", student.Email),
                    new SqlParameter("@address", student.Address),
                    new SqlParameter("@city", student.City),
                    new SqlParameter("@postalCode", student.PostalCode),
                    new SqlParameter("@dormID", student.DormitoryID),
                    new SqlParameter("@bedNo", student.BedNo),
                    new SqlParameter("@prevInstitution", student.PrevInstitution),
                    new SqlParameter("@kcpeScore", student.KCPEScore),
                    new SqlParameter("@prevBalance", student.PrevBalance),
                    new SqlParameter("@photo", student.SPhoto),
                    new SqlParameter("@classID", student.ClassID)
                }); 
            });
        }

        public static Task<ObservableCollection<StaffModel>> GetAllStaffAsync()
        {
            return Task.Run<ObservableCollection<StaffModel>>(delegate
            {
                string commandText = "SELECT TOP 1000000 StaffID,Name,NationalID,DateOfAdmission,PhoneNo,Email,Address,City,PostalCode,SPhoto FROM [Institution].[Staff]";
                ObservableCollection<StaffModel> observableCollection = new ObservableCollection<StaffModel>();
                DataTable dataTable = DataAccessHelper.ExecuteNonQueryWithResultTable(commandText);
                if (dataTable.Rows.Count != 0)
                {
                    foreach (DataRow dataRow in dataTable.Rows)
                    {
                        observableCollection.Add(new StaffModel
                        {
                            StaffID = (int)dataRow[0],
                            Name = dataRow[1].ToString(),
                            NationalID = dataRow[2].ToString(),
                            DateOfAdmission = DateTime.Parse(dataRow[3].ToString()),
                            PhoneNo = dataRow[4].ToString(),
                            Email = dataRow[5].ToString(),
                            Address = dataRow[6].ToString(),
                            City = dataRow[7].ToString(),
                            PostalCode = dataRow[8].ToString(),
                            SPhoto = dataRow[9] as byte[]
                        });
                    }
                }
                return observableCollection;
            });
        }

        public static Task<ObservableCollection<StudentModel>> GetAllStudentsAsync()
        {
            return Task.Run<ObservableCollection<StudentModel>>(delegate
            {
                string commandText = "SELECT TOP 1000000 StudentID,FirstName,LastName,MiddleName,ClassID,DateOfBirth,DateOfAdmission,NameOfGuardian,GuardianPhoneNo,Email,Address,City,PostalCode,DormitoryID,BedNo,PreviousInstitution,KCPEScore,SPhoto FROM [Institution].[Student]";
                ObservableCollection<StudentModel> observableCollection = new ObservableCollection<StudentModel>();
                DataTable dataTable = DataAccessHelper.ExecuteNonQueryWithResultTable(commandText);
                if (dataTable.Rows.Count != 0)
                {
                    foreach (DataRow dataRow in dataTable.Rows)
                    {
                        observableCollection.Add(new StudentModel
                        {
                            StudentID = (int)dataRow[0],
                            FirstName = dataRow[1].ToString(),
                            MiddleName = dataRow[3].ToString(),
                            LastName = dataRow[2].ToString(),
                            ClassID = (int)dataRow[4],
                            DateOfBirth = DateTime.Parse(dataRow[5].ToString()),
                            DateOfAdmission = DateTime.Parse(dataRow[6].ToString()),
                            NameOfGuardian = dataRow[7].ToString(),
                            GuardianPhoneNo = dataRow[8].ToString(),
                            Email = dataRow[9].ToString(),
                            Address = dataRow[10].ToString(),
                            City = dataRow[11].ToString(),
                            PostalCode = dataRow[12].ToString(),
                            DormitoryID = (!Convert.IsDBNull(dataRow[13])) ? int.Parse(dataRow[13].ToString()) : 0,
                            BedNo = (!Convert.IsDBNull(dataRow[13])) ? dataRow[14].ToString() : "",
                            PrevInstitution = (!Convert.IsDBNull(dataRow[13])) ? dataRow[15].ToString() : "",
                            KCPEScore = Convert.IsDBNull(dataRow[16]) ? 0 : int.Parse(dataRow[16].ToString()),
                            SPhoto = dataRow[17] as byte[]
                        });
                    }
                }
                return observableCollection;
            });
        }

        public static Task<ObservableCollection<StudentListModel>> GetAllStudentsListAsync()
        {
            return Task.Run<ObservableCollection<StudentListModel>>(delegate
            {
                string commandText = "SELECT TOP 1000000 s.StudentID,s.FirstName,s.LastName,s.MiddleName,s.ClassID, c.NameOfClass,s.DateOfBirth,"+
                "s.DateOfAdmission,s.NameOfGuardian,s.GuardianPhoneNo,s.Address,s.City,s.PostalCode,s.BedNo,s.PreviousInstitution,s.KCPEScore, s.DormitoryID, "+
                "s.PreviousBalance,d.NameOfDormitory, s.IsActive,s.IsBoarder,s.Gender, s.SPhoto FROM [Institution].[Student] s LEFT OUTER JOIN [Institution].[Class] c ON "+
                "(s.ClassID=c.ClassID) LEFT OUTER JOIN [Institution].[Dormitory] d ON (s.DormitoryID=d.DormitoryID)";
                ObservableCollection<StudentListModel> observableCollection = new ObservableCollection<StudentListModel>();
                DataTable dataTable = DataAccessHelper.ExecuteNonQueryWithResultTable(commandText);
                if (dataTable.Rows.Count != 0)
                {
                    foreach (DataRow dataRow in dataTable.Rows)
                    {
                        observableCollection.Add(new StudentListModel
                        {
                            StudentID = int.Parse(dataRow[0].ToString()),
                            FirstName = dataRow[1].ToString(),
                            LastName = dataRow[2].ToString(),
                            MiddleName = dataRow[3].ToString(),
                            ClassID = int.Parse(dataRow[4].ToString()),
                            NameOfClass = dataRow[5].ToString(),
                            DateOfBirth = DateTime.Parse(dataRow[6].ToString()),
                            DateOfAdmission = DateTime.Parse(dataRow[7].ToString()),
                            NameOfGuardian = dataRow[8].ToString(),
                            GuardianPhoneNo = dataRow[9].ToString(),
                            Address = dataRow[10].ToString(),
                            City = dataRow[11].ToString(),
                            PostalCode = dataRow[12].ToString(),
                            BedNo = dataRow[13].ToString(),
                            PrevInstitution = dataRow[14].ToString(),
                            KCPEScore = string.IsNullOrWhiteSpace(dataRow[15].ToString()) ? 0 : int.Parse(dataRow[15].ToString()),
                            DormitoryID = string.IsNullOrWhiteSpace(dataRow[16].ToString()) ? 0 : int.Parse(dataRow[16].ToString()),
                            PrevBalance = decimal.Parse(dataRow[17].ToString()),
                            NameOfDormitory = dataRow[18].ToString(),
                            IsActive = bool.Parse(dataRow[19].ToString()),
                            IsBoarder = bool.Parse(dataRow[20].ToString()),
                            Gender = (Gender)Enum.Parse(typeof(Gender), dataRow[21].ToString()),
                            SPhoto = (byte[])dataRow[22]
                        });
                    }
                }
                return observableCollection;
            });
        }

        public static Task<bool> SaveNewStaffAsync(StaffModel newStaff)
        {
            return Task.Run<bool>(delegate
            {
                bool flag = newStaff.StaffID == 0;
                string commandText = "BEGIN TRANSACTION\r\nDECLARE @id INT; SET @id=dbo.GetNewID('Institution.Staff'); INSERT INTO [Institution].[Staff] (StaffID,Name,NationalID,DateOfAdmission,PhoneNo,Email,Address,City,PostalCode,SPhoto) VALUES(" + (flag ? "@id" : "@staffID") + ",@name,@nationalID,@doa,@phoneNo,@email,@address,@city,@postalCode,@photo)\r\nCOMMIT";
                return DataAccessHelper.ExecuteNonQueryWithParameters(commandText, new ObservableCollection<SqlParameter>
                {
                    new SqlParameter("@staffID", newStaff.StaffID),
                    new SqlParameter("@name", newStaff.Name),
                    new SqlParameter("@nationalID", newStaff.NationalID),
                    new SqlParameter("@doa", newStaff.DateOfAdmission.ToString("g")),
                    new SqlParameter("@phoneNo", newStaff.PhoneNo),
                    new SqlParameter("@email", newStaff.Email),
                    new SqlParameter("@address", newStaff.Address),
                    new SqlParameter("@city", newStaff.City),
                    new SqlParameter("@postalCode", newStaff.PostalCode),
                    new SqlParameter("@photo", newStaff.SPhoto)
                });
            });
        }

        public static Task<bool> SaveNewFeesPaymentAsync(FeePaymentModel newPayment)
        {
            return Task.Run<bool>(delegate
            {
                string commandText = "INSERT INTO [Institution].[FeesPayment] (FeesPaymentID,StudentID,AmountPaid,DatePaid,PaymentMethod) VALUES(dbo.GetNewID('Institution.FeesPayment'),@studentID,@amount,@dop,@paym)";
                return DataAccessHelper.ExecuteNonQueryWithParameters(commandText, new ObservableCollection<SqlParameter>
                {
                    new SqlParameter("@studentID", newPayment.StudentID),
                    new SqlParameter("@amount", newPayment.AmountPaid),
                    new SqlParameter("@dop", newPayment.DatePaid.ToString("g")),
                    new SqlParameter("@paym", newPayment.PaymentMethod)
                });
            });
        }

        public static Task<bool> SaveNewDonation(DonationModel donation, string type)
        {
            return Task.Run<bool>(delegate
            {
                string commandText = "INSERT INTO [Institution].[Donation] (DonorID,AmountDonated,DateDonated,DonateTo,[Type]) VALUES(@donorID,@amount,@dod,@dnt,@typ)";
                return DataAccessHelper.ExecuteNonQueryWithParameters(commandText, new ObservableCollection<SqlParameter>
                {
                    new SqlParameter("@donorID", donation.DonorID),
                    new SqlParameter("@amount", donation.Amount),
                    new SqlParameter("@dod", donation.DateDonated.ToString("g")),
                    new SqlParameter("@dnt", donation.DonateTo.ToString()),
                    new SqlParameter("@typ", type.Equals("D") ? 1 : 2)
                });
            });
        }

        public static Task<bool> SaveNewFeesPaymentAsync(FeePaymentModel newPayment, SaleModel newSale)
        {
            return Task.Run<bool>(delegate
            {
                ObservableCollection<SqlParameter> observableCollection = new ObservableCollection<SqlParameter>();
                string text = "BEGIN TRANSACTION\r\nDECLARE @id int; SET @id = dbo.GetNewID('Institution.FeesPayment')\r\nDECLARE @id2 int; SET @id2 = dbo.GetNewID('Sales.SaleHeader')\r\nINSERT INTO [Institution].[FeesPayment] (FeesPaymentID,StudentID,AmountPaid,DatePaid) VALUES(@id,@studentID,@amount,@dop)\r\nINSERT INTO [Sales].[SaleHeader] (SaleID,CustomerID,EmployeeID,IsCancelled,OrderDate,IsDiscount,PaymentID) VALUES(@id2,@studentID,@employeeID,@isCancelled,@dateAdded,@isDiscount,@id)";
                int num = 1;
                foreach (FeesStructureEntryModel current in newSale.SaleItems)
                {
                    string parameterName = "@entryName" + num;
                    string parameterName2 = "@entryAmount" + num;
                    observableCollection.Add(new SqlParameter(parameterName, current.Name));
                    observableCollection.Add(new SqlParameter(parameterName2, current.Amount));
                    object obj = text;
                    text = string.Concat(new object[]
                    {
                        obj,
                        "\r\nINSERT INTO [Sales].[SaleDetail] (SaleID,Name,Amount) VALUES(@id2,@entryName",
                        num,
                        ",@entryAmount",
                        num,
                        ")"
                    });
                    num++;
                }
                text += "\r\nCOMMIT";
                observableCollection.Add(new SqlParameter("@studentID", newPayment.StudentID));
                observableCollection.Add(new SqlParameter("@amount", newPayment.AmountPaid));
                observableCollection.Add(new SqlParameter("@dop", newPayment.DatePaid.ToString("g")));
                observableCollection.Add(new SqlParameter("@employeeID", newSale.EmployeeID));
                observableCollection.Add(new SqlParameter("@isCancelled", newSale.IsCancelled ? 0 : 1));
                observableCollection.Add(new SqlParameter("@dateAdded", newSale.DateAdded.ToString("g")));
                observableCollection.Add(new SqlParameter("@isDiscount", newSale.IsDiscount ? 0 : 1));
                return DataAccessHelper.ExecuteNonQueryWithParameters(text, observableCollection);
            });
        }

        public static Task<bool> SaveNewStudentBill(SaleModel newSale)
        {
            return Task.Run<bool>(delegate
            {
                string text = string.Concat(new object[]
                {
                    "BEGIN TRANSACTION\r\nDECLARE @id int; SET @id = dbo.GetNewID('Sales.SaleHeader')\r\nINSERT INTO [Sales].[SaleHeader] (SaleID,CustomerID,EmployeeID,IsCancelled,OrderDate,IsDiscount,PaymentID) VALUES(@id,'",
                    newSale.CustomerID,
                    "',",
                    newSale.EmployeeID,
                    ",'",
                    newSale.IsCancelled,
                    "','",
                    newSale.DateAdded.ToString("g"),
                    "','",
                    newSale.IsDiscount,
                    "',@id)"
                });
                foreach (FeesStructureEntryModel current in newSale.SaleItems)
                {
                    object obj = text;
                    text = string.Concat(new object[]
                    {
                        obj,
                        "\r\nINSERT INTO [Sales].[SaleDetail] (SaleID,Name,Amount) VALUES(@id,'",
                        current.Name,
                        "',",
                        current.Amount,
                        ")"
                    });
                }
                text += "\r\nCOMMIT";
                return DataAccessHelper.ExecuteNonQuery(text);
            });
        }

        public static Task<bool> SaveNewClassBill(SaleModel newSale)
        {
            return Task.Run<bool>(delegate
            {
                int term = DataAccess.GetTerm();
                DateTime? dateTime = null;
                DateTime? dateTime2 = null;
                switch (term)
                {
                    case 1:
                        dateTime = new DateTime?(new DateTime(DateTime.Now.Year, 1, 1));
                        dateTime2 = new DateTime?(new DateTime(DateTime.Now.Year, 4, 30));
                        break;
                    case 2:
                        dateTime = new DateTime?(new DateTime(DateTime.Now.Year, 5, 1));
                        dateTime2 = new DateTime?(new DateTime(DateTime.Now.Year, 8, 31));
                        break;
                    case 3:
                        dateTime = new DateTime?(new DateTime(DateTime.Now.Year, 9, 1));
                        dateTime2 = new DateTime?(new DateTime(DateTime.Now.Year, 12, 31));
                        break;
                }
                string selectString = "SELECT StudentID FROM [Institution].[Student] WHERE IsActive=1 AND ClassID=" + newSale.CustomerID;
                ObservableCollection<string> observableCollection = DataAccessHelper.CopyFromDBtoObservableCollection(selectString);
                string text = "BEGIN TRANSACTION\r\n DECLARE @id int;\r\n";
                foreach (string current in observableCollection)
                {
                    object obj = text;
                    text = string.Concat(new object[]
                    {
                        obj,
                        "IF NOT EXISTS(SELECT * FROM [Sales].[SaleHeader] WHERE CustomerID=",
                        current,
                        " AND OrderDate BETWEEN '",
                        dateTime.Value.Day.ToString(),
                        "/",
                        dateTime.Value.Month.ToString(),
                        "/",
                        dateTime.Value.Year.ToString(),
                        " 00:00:00.000' AND '",
                        dateTime2.Value.Day.ToString(),
                        "/",
                        dateTime2.Value.Month.ToString(),
                        "/",
                        dateTime2.Value.Year.ToString(),
                        " 23:59:59.998')\r\nBEGIN\r\nSET @id = dbo.GetNewID('Sales.SaleHeader');\r\nINSERT INTO [Sales].[SaleHeader] (SaleID,CustomerID,EmployeeID,IsCancelled,OrderDate,IsDiscount,PaymentID) VALUES(@id,'",
                        current,
                        "',",
                        newSale.EmployeeID,
                        ",'",
                        newSale.IsCancelled,
                        "','",
                        newSale.DateAdded.ToString("g"),
                        "','",
                        newSale.IsDiscount,
                        "',0)\r\n;"
                    });
                    foreach (FeesStructureEntryModel current2 in newSale.SaleItems)
                    {
                        obj = text;
                        text = string.Concat(new object[]
                        {
                            obj,
                            "INSERT INTO [Sales].[SaleDetail] (SaleID,Name,Amount) VALUES(@id,'",
                            current2.Name,
                            "',",
                            current2.Amount,
                            ");\r\n"
                        });
                    }
                    text += "\r\nEND\r\n";
                }
                text += "COMMIT";
                return DataAccessHelper.ExecuteNonQuery(text);
            });
        }

        public static Task<bool> UpdateStudentBill(SaleModel newSale)
        {
            return Task.Run<bool>(delegate
            {
                string text = "BEGIN TRANSACTION\r\nDELETE FROM [Sales].[SaleDetail] WHERE SaleID=" + newSale.SaleID;
                foreach (FeesStructureEntryModel current in newSale.SaleItems)
                {
                    object obj = text;
                    text = string.Concat(new object[]
                    {
                        obj,
                        "\r\nINSERT INTO [Sales].[SaleDetail] (SaleID,Name,Amount) VALUES(",
                        newSale.SaleID,
                        ",'",
                        current.Name,
                        "',",
                        current.Amount,
                        ")"
                    });
                }
                text += "\r\nCOMMIT";
                return DataAccessHelper.ExecuteNonQuery(text);
            });
        }

        public static bool SearchAllStudentProperties(StudentModel student, string searchText)
        {
            Regex.CacheSize = 14;
            return Regex.Match(student.StudentID.ToString(), searchText, RegexOptions.IgnoreCase).Success || Regex.Match(student.FirstName, searchText, RegexOptions.IgnoreCase).Success || Regex.Match(student.LastName, searchText, RegexOptions.IgnoreCase).Success || Regex.Match(student.MiddleName, searchText, RegexOptions.IgnoreCase).Success || Regex.Match(student.NameOfGuardian, searchText, RegexOptions.IgnoreCase).Success || Regex.Match(student.Address, searchText, RegexOptions.IgnoreCase).Success || Regex.Match(student.BedNo, searchText, RegexOptions.IgnoreCase).Success || Regex.Match(student.City, searchText, RegexOptions.IgnoreCase).Success || Regex.Match(student.ClassID.ToString(), searchText, RegexOptions.IgnoreCase).Success || Regex.Match(student.DormitoryID.ToString(), searchText, RegexOptions.IgnoreCase).Success || Regex.Match(student.Email, searchText, RegexOptions.IgnoreCase).Success || Regex.Match(student.NameOfStudent, searchText, RegexOptions.IgnoreCase).Success || Regex.Match(student.PostalCode, searchText, RegexOptions.IgnoreCase).Success || Regex.Match(student.PrevInstitution, searchText, RegexOptions.IgnoreCase).Success;
        }

        public static bool SearchAllBookProperties(BookModel student, string searchText)
        {
            Regex.CacheSize = 14;
            return Regex.Match(student.ISBN, searchText, RegexOptions.IgnoreCase).Success || Regex.Match(student.Title, searchText, RegexOptions.IgnoreCase).Success || Regex.Match(student.Author, searchText, RegexOptions.IgnoreCase).Success || Regex.Match(student.Publisher, searchText, RegexOptions.IgnoreCase).Success || Regex.Match(student.Price.ToString(), searchText, RegexOptions.IgnoreCase).Success;
        }

        public static bool SearchAllStaffProperties(StaffModel staff, string searchText)
        {
            Regex.CacheSize = 14;
            return Regex.Match(staff.StaffID.ToString(), searchText, RegexOptions.IgnoreCase).Success || Regex.Match(staff.Name, searchText, RegexOptions.IgnoreCase).Success || Regex.Match(staff.NationalID, searchText, RegexOptions.IgnoreCase).Success || Regex.Match(staff.Address, searchText, RegexOptions.IgnoreCase).Success || Regex.Match(staff.City, searchText, RegexOptions.IgnoreCase).Success || Regex.Match(staff.Email, searchText, RegexOptions.IgnoreCase).Success || Regex.Match(staff.PhoneNo, searchText, RegexOptions.IgnoreCase).Success || Regex.Match(staff.PostalCode, searchText, RegexOptions.IgnoreCase).Success;
        }

        public static Task<bool> SaveNewFeesStructureAsync(FeesStructureModel currrentStruct)
        {
            return Task.Run<bool>(delegate
            {
                string text = string.Concat(new object[]
                {
                    "BEGIN TRANSACTION\r\nDECLARE @id int; SET @id = dbo.GetNewID('Institution.FeesStructureHeader')\r\nINSERT INTO [Institution].[FeesStructureHeader] (FeesStructureID,ClassID, StartDate) VALUES (@id,",
                    currrentStruct.ClassID,
                    ",'",
                    currrentStruct.StartDate.ToString("g"),
                    "')\r\n"
                });
                foreach (FeesStructureEntryModel current in currrentStruct.Entries)
                {
                    object obj = text;
                    text = string.Concat(new object[]
                    {
                        obj,
                        "INSERT INTO [Institution].[FeesStructureDetail] (FeesStructureID,Name,Amount) VALUES (@id,'",
                        current.Name,
                        "','",
                        current.Amount,
                        "')\r\n"
                    });
                }
                text += " COMMIT";
                return DataAccessHelper.ExecuteNonQuery(text);
            });
        }

        public static Task<FeesStructureModel> GetFeesStructureAsync(int currentClassID, DateTime currentDate)
        {
            return Task.Run<FeesStructureModel>(delegate
            {
                FeesStructureModel feesStructureModel = new FeesStructureModel();
                string text = currentDate.Date.ToString("g");
                string commandText = "DECLARE @id int\r\nSET @id=(SELECT TOP 1 FeesStructureID FROM [Institution].[FeesStructureHeader] WHERE ClassID=" + currentClassID + "\r\nAND IsActive=1)\r\nSELECT ISNULL(@id,0)";
                int num = int.Parse(DataAccessHelper.ExecuteScalar(commandText));
                FeesStructureModel result;
                if (num <= 0)
                {
                    result = feesStructureModel;
                }
                else
                {
                    commandText = "SELECT Name, Amount FROM [Institution].[FeesStructureDetail] WHERE FeesStructureID =" + num;
                    DataTable dataTable = DataAccessHelper.ExecuteNonQueryWithResultTable(commandText);
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

        public static Task<ObservableCollection<SubjectModel>> GetSubjectsRegistredToClassAsync(int selectedClassID)
        {
            return Task.Run<ObservableCollection<SubjectModel>>(delegate
            {
                ObservableCollection<SubjectModel> observableCollection = new ObservableCollection<SubjectModel>();
                string commandText = "SELECT ssd.SubjectID,s.NameOfSubject,ssd.Tutor, s.MaximumScore,s.Code,s.IsOptional FROM [Institution].[SubjectSetupDetail] ssd LEFT OUTER JOIN [Institution].[Subject] s ON (ssd.SubjectID = s.SubjectID) LEFT OUTER JOIN [Institution].[SubjectSetupHeader] ssh ON (ssd.SubjectSetupID = ssh.SubjectSetupID) WHERE IsACtive=1 AND ssh.ClassID=" + selectedClassID + " ORDER BY s.Code";
                DataTable dataTable = DataAccessHelper.ExecuteNonQueryWithResultTable(commandText);
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    SubjectModel subjectModel = new SubjectModel();
                    subjectModel.SubjectID = (int)dataRow[0];
                    subjectModel.NameOfSubject = dataRow[1].ToString();
                    subjectModel.Tutor = dataRow[2].ToString();
                    subjectModel.MaximumScore = decimal.Parse(dataRow[3].ToString());
                    subjectModel.Code = int.Parse(dataRow[4].ToString());
                    subjectModel.IsOptional = bool.Parse(dataRow[5].ToString());
                    if (!subjectModel.NameOfSubject.ToUpper().Trim().Contains("SKILLS"))
                    {
                        observableCollection.Add(subjectModel);
                    }
                }
                return observableCollection;
            });
        }

        public static Task<ObservableCollection<SubjectModel>> GetSubjectsRegistredToCombinedClassAsync(CombinedClassModel combinedClass)
        {
            return Task.Run<ObservableCollection<SubjectModel>>(delegate
            {
                ObservableCollection<SubjectModel> observableCollection = new ObservableCollection<SubjectModel>();
                string text = "0,";
                foreach (ClassModel current in combinedClass.Entries)
                {
                    text = text + current.ClassID + ",";
                }
                text = text.Remove(text.Length - 1);
                string commandText = "SELECT DISTINCT ssd.SubjectID,s.NameOfSubject,ssd.Tutor, s.MaximumScore,s.Code,s.IsOptional FROM [Institution].[SubjectSetupDetail] ssd LEFT OUTER JOIN [Institution].[Subject] s ON (ssd.SubjectID = s.SubjectID) LEFT OUTER JOIN [Institution].[SubjectSetupHeader] ssh ON (ssd.SubjectSetupID = ssh.SubjectSetupID) WHERE IsACtive=1 AND ssh.ClassID IN(" + text + ") ORDER BY s.Code";
                DataTable dataTable = DataAccessHelper.ExecuteNonQueryWithResultTable(commandText);
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    SubjectModel subjectModel = new SubjectModel();
                    subjectModel.SubjectID = (int)dataRow[0];
                    subjectModel.NameOfSubject = dataRow[1].ToString();
                    subjectModel.Tutor = dataRow[2].ToString();
                    subjectModel.MaximumScore = decimal.Parse(dataRow[3].ToString());
                    subjectModel.Code = int.Parse(dataRow[4].ToString());
                    subjectModel.IsOptional = bool.Parse(dataRow[5].ToString());
                    if (!subjectModel.NameOfSubject.ToUpper().Trim().Contains("SKILLS"))
                    {
                        observableCollection.Add(subjectModel);
                    }
                }
                return observableCollection;
            });
        }

        public static Task<ObservableCollection<ClassModel>> GetCurrentRegistredClassesAsync()
        {
            return Task.Run<ObservableCollection<ClassModel>>(delegate
            {
                ObservableCollection<ClassModel> observableCollection = new ObservableCollection<ClassModel>();
                string commandText = "DECLARE @id int\r\n SET @id=(SELECT ClassSetupID FROM [Institution].[ClassSetupHeader] WHERE IsActive=1)\r\nIF @id>0\r\nBEGIN\r\nSELECT csd.ClassID,c.NameOfClass FROM [Institution].[ClassSetupDetail] csd LEFT OUTER JOIN [Institution].[Class] c on (csd.ClassID = c.ClassID) WHERE csd.ClassSetupID =@id\r\nEND";
                DataTable dataTable = DataAccessHelper.ExecuteNonQueryWithResultTable(commandText);
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    observableCollection.Add(new ClassModel
                    {
                        ClassID = (int)dataRow[0],
                        NameOfClass = dataRow[1].ToString()
                    });
                }
                return observableCollection;
            });
        }

        public static Task<ObservableCollection<CombinedClassModel>> GetCurrentRegistredCombinedClassesAsync()
        {
            return Task.Run<ObservableCollection<CombinedClassModel>>(delegate
            {
                ObservableCollection<CombinedClassModel> observableCollection = new ObservableCollection<CombinedClassModel>();
                ObservableCollection<ClassModel> observableCollection2 = new ObservableCollection<ClassModel>();
                string commandText = "DECLARE @id int\r\n SET @id=(SELECT ClassSetupID FROM [Institution].[ClassSetupHeader] WHERE IsActive=1)\r\nIF @id>0\r\nBEGIN\r\nSELECT csd.ClassID,c.NameOfClass FROM [Institution].[ClassSetupDetail] csd LEFT OUTER JOIN [Institution].[Class] c on (csd.ClassID = c.ClassID) WHERE csd.ClassSetupID =@id\r\nEND";
                DataTable dataTable = DataAccessHelper.ExecuteNonQueryWithResultTable(commandText);
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    observableCollection2.Add(new ClassModel
                    {
                        ClassID = (int)dataRow[0],
                        NameOfClass = dataRow[1].ToString()
                    });
                }

                List<string> f = new List<string>();
                foreach (ClassModel current in observableCollection2)
                {
                    if (!f.Contains(current.NameOfClass.Substring(0, 6).ToUpper()))
                    {

                        f.Add(current.NameOfClass.Substring(0, 6).ToUpper());
                    }
                }
                int i;
                for (i = 0; i < f.Count; i++)
                {
                    CombinedClassModel combinedClassModel = new CombinedClassModel();
                    combinedClassModel.Description = f[i];
                    combinedClassModel.Entries = new ObservableCollection<ClassModel>(from o in observableCollection2
                                                                                      where o.NameOfClass.Trim().ToUpper().Contains(f[i])
                                                                                      select o);
                    observableCollection.Add(combinedClassModel);
                }
                return observableCollection;
            });
        }

        public static Task<decimal> GetAllSalesAsync(int studentID)
        {
            return Task.Run<decimal>(delegate
            {
                string commandText = "SELECT SUM(CONVERT(DECIMAL,TotalAmt)) FROM Sales.SaleHeader WHERE CustomerID = " + studentID;
                return decimal.Parse(DataAccessHelper.ExecuteScalar(commandText));
            });
        }

        public static Task<decimal> GetAllFeesPaidAsync(int studentID)
        {
            return Task.Run<decimal>(delegate
            {
                string commandText = "SELECT SUM(CONVERT(DECIMAL,AmountPaid)) FROM [Institution].[FeesPayment] WHERE StudentID =" + studentID;
                return decimal.Parse(DataAccessHelper.ExecuteScalar(commandText));
            });
        }

        public static Task<ObservableCollection<FeePaymentReceiptModel>> GetRecentPaymentsReceiptAsync(int studentID)
        {
            return Task.Run<ObservableCollection<FeePaymentReceiptModel>>(delegate
            {
                ObservableCollection<FeePaymentReceiptModel> observableCollection = new ObservableCollection<FeePaymentReceiptModel>();
                string commandText = "SELECT TOP 20 FeesPaymentID, AmountPaid, DatePaid FROM [Institution].[FeesPayment] WHERE StudentID =" + studentID + " ORDER BY [DatePaid] desc";
                DataTable dataTable = DataAccessHelper.ExecuteNonQueryWithResultTable(commandText);
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    observableCollection.Add(new FeePaymentReceiptModel
                    {
                        AmountPaid = decimal.Parse(dataRow[1].ToString()),
                        StudentID = studentID,
                        FeePaymentID = int.Parse(dataRow[0].ToString()),
                        DatePaid = DateTime.Parse(dataRow[2].ToString())
                    });
                }
                return observableCollection;
            });
        }

        public static Task<bool> SaveNewExamAsync(ExamModel newExam)
        {
            return Task.Run<bool>(delegate
            {
                string text = string.Concat(new object[]
                {
                    "BEGIN TRANSACTION\r\nDECLARE @id int;\r\n SET @id = dbo.GetNewID('Institution.ExamHeader') INSERT INTO [Institution].[ExamHeader] (ExamID,NameOfExam,OutOf,ExamDateTime) VALUES (@id,'",
                    newExam.NameOfExam,
                    "',",
                    newExam.OutOf,
                    ",'",
                    DateTime.Now.ToString("g"),
                    "')\r\n"
                });
                foreach (ClassModel current in newExam.Classes)
                {
                    object obj = text;
                    text = string.Concat(new object[]
                    {
                        obj,
                        "INSERT INTO [Institution].[ExamClassDetail] (ExamID,ClassID) VALUES (@id,",
                        current.ClassID,
                        ")\r\n"
                    });
                    string selecteStr = "SELECT StudentID FROM [Institution].[Student] WHERE ClassID =" + current.ClassID;

                    ObservableCollection<string> list = DataAccessHelper.CopyFromDBtoObservableCollection(selecteStr);
                    foreach (var t in list)
                        text += "IF NOT EXISTS (SELECT * FROM [Institution].[ExamStudentDetail] WHERE StudentID=" + t + " AND ExamID=@id)\r\n" +
                    "INSERT INTO [Institution].[ExamStudentDetail] (ExamID,StudentID) VALUES (@id" +
                    "," + t +
                    ")\r\n";

                }
                foreach (ExamSubjectEntryModel current2 in newExam.Entries)
                {
                    object obj = text;
                    text = string.Concat(new object[]
                    {
                        obj,
                        "INSERT INTO [Institution].[ExamDetail] (ExamID,SubjectID,ExamDateTime) VALUES (@id,",
                        current2.SubjectID,
                        ",'",
                        current2.ExamDateTime.ToString("g"),
                        "')\r\n"
                    });
                }      

                text += " COMMIT";


                return DataAccessHelper.ExecuteNonQuery(text);
            });
        }

        public static Task<bool> SaveNewClassSetupAsync(ClassesSetupModel classSetup)
        {
            return Task.Run<bool>(delegate
            {
                string text = "BEGIN TRANSACTION\r\ndeclare @id int; declare @id2 int; SET @id = [dbo].GetNewID('Institution.ClassSetupHeader') INSERT INTO [Institution].[ClassSetupHeader] (ClassSetupID,StartDate) VALUES (@id,'" + classSetup.StartDate.ToString("g") + "')\r\n";
                foreach (ClassesSetupEntryModel current in classSetup.Entries)
                {
                    object obj = text;
                    text = string.Concat(new object[]
                    {
                        obj,
                        "IF NOT EXISTS (SELECT * FROM [Institution].[Class] WHERE ClassID=",
                        current.ClassID,
                        " AND NameOfClass='",
                        current.NameOfClass,
                        "')\r\nBEGIN\r\nSET @id2 = [dbo].GetNewID('Institution.Class')\r\n INSERT INTO [Institution].[Class] (ClassID,NameOfClass) VALUES (@id2,'",
                        current.NameOfClass,
                        "')\r\nINSERT INTO [Institution].[ClassSetupDetail] (ClassSetupID,ClassID) VALUES (@id,@id2)\r\n END\r\nELSE\r\nBEGIN\r\nUPDATE [Institution].[ClassSetupDetail] SET ClassSetupID=@id WHERE ClassID=",
                        current.ClassID,
                        "\r\nEND\r\n"
                    });
                }
                text += " COMMIT";
                return DataAccessHelper.ExecuteNonQuery(text);
            });
        }

        public static Task<int> GetClassIDFromStudentID(int selectedStudentID)
        {
            return Task.Run<int>(delegate
            {
                string commandText = string.Concat(new object[]
                {
                    "IF EXISTS(SELECT ClassID FROM [Institution].[Student] WHERE StudentID = ",
                    selectedStudentID,
                    ")\r\nSELECT ClassID FROM [Institution].[Student] WHERE StudentID = ",
                    selectedStudentID,
                    "\r\nELSE SELECT 0"
                });
                string s = DataAccessHelper.ExecuteScalar(commandText);
                return int.Parse(s);
            });
        }

        public static Task<ObservableCollection<ExamModel>> GetExamsByClass(int classID)
        {
            return Task.Run<ObservableCollection<ExamModel>>(async delegate
            {
                ObservableCollection<ExamModel> observableCollection = new ObservableCollection<ExamModel>();
                string commandText = string.Concat(new object[]
                {
                    "SELECT ecd.ExamID,eh.NameOfExam,eh.ExamDatetime,ISNULL(eh.OutOf,100) FROM [Institution].[ExamHeader] eh LEFT OUTER JOIN[Institution].[ExamClassDetail] ecd ON (ecd.ExamID=eh.ExamID) WHERE ecd.ClassID=",
                    classID,
                    " AND eh.ExamDateTime>= CONVERT(datetime,'",
                    DataAccess.GetTermStart().ToString("g"),
                    "') AND eh.ExamDateTime<= CONVERT(datetime,'",
                    DataAccess.GetTermEnd().ToString("g"),
                    "')"
                });
                DataTable dataTable = DataAccessHelper.ExecuteNonQueryWithResultTable(commandText);
                List<Task<KeyValuePair<int, ObservableCollection<ExamSubjectEntryModel>>>> list = new List<Task<KeyValuePair<int, ObservableCollection<ExamSubjectEntryModel>>>>(dataTable.Rows.Count);
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    ExamModel examModel = new ExamModel();
                    examModel.ExamID = int.Parse(dataRow[0].ToString());
                    examModel.NameOfExam = dataRow[1].ToString();
                    examModel.OutOf = decimal.Parse(dataRow[3].ToString());
                    list.Add(DataAccess.GetExamEntries(examModel.ExamID, examModel.OutOf));
                    observableCollection.Add(examModel);
                }
                KeyValuePair<int, ObservableCollection<ExamSubjectEntryModel>>[] source = await Task.WhenAll<KeyValuePair<int, ObservableCollection<ExamSubjectEntryModel>>>(list);
                using (IEnumerator<ExamModel> var_16 = observableCollection.GetEnumerator())
                {
                    while (var_16.MoveNext())
                    {
                        ExamModel ems = var_16.Current;
                        ParallelQuery<KeyValuePair<int, ObservableCollection<ExamSubjectEntryModel>>> source2 = from entry in source.AsParallel<KeyValuePair<int, ObservableCollection<ExamSubjectEntryModel>>>()
                                                                                                                where entry.Key == ems.ExamID
                                                                                                                select entry;
                        ems.Entries = source2.First<KeyValuePair<int, ObservableCollection<ExamSubjectEntryModel>>>().Value;
                    }
                }
                return observableCollection;
            });
        }

        public static async Task<ExamModel> GetExamAsync(int examID)
        {
            ExamModel examModel = new ExamModel();
            string commandText = "SELECT NameOfExam,OutOf FROM [Institution].[ExamHeader] WHERE ExamID=" + examID;
            DataTable dataTable = DataAccessHelper.ExecuteNonQueryWithResultTable(commandText);
            ExamModel result;
            if (dataTable.Rows.Count <= 0)
            {
                result = examModel;
            }
            else
            {
                examModel.ExamID = examID;
                examModel.NameOfExam = dataTable.Rows[0][0].ToString();
                examModel.Classes = await DataAccess.GetExamClasses(examID);
                examModel.OutOf = decimal.Parse(dataTable.Rows[0][1].ToString());
                examModel.Entries = (await DataAccess.GetExamEntries(examID, examModel.OutOf)).Value;
                result = examModel;
            }
            return result;
        }

        private static Task<ObservableCollection<ClassModel>> GetExamClasses(int examID)
        {
            return Task.Run<ObservableCollection<ClassModel>>(delegate
            {
                string commandText = "SELECT ecd.ClassID, c.NameOfClass FROM [Institution].[ExamClassDetail] ecd LEFT OUTER JOIN [Institution].[Class] c ON (ecd.ClassID = c.ClassID) WHERE ecd.ExamID =" + examID;
                DataTable dataTable = DataAccessHelper.ExecuteNonQueryWithResultTable(commandText);
                ObservableCollection<ClassModel> observableCollection = new ObservableCollection<ClassModel>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    observableCollection.Add(new ClassModel
                    {
                        ClassID = int.Parse(dataRow[0].ToString()),
                        NameOfClass = dataRow[1].ToString()
                    });
                }
                return observableCollection;
            });
        }

        private static Task<KeyValuePair<int, ObservableCollection<ExamSubjectEntryModel>>> GetExamEntries(int examID, decimal outOf)
        {
            return Task.Run<KeyValuePair<int, ObservableCollection<ExamSubjectEntryModel>>>(delegate
            {
                string commandText = "SELECT ed.SubjectID, s.NameOfSubject, ed.ExamDateTime FROM [Institution].[ExamDetail] ed LEFT OUTER JOIN [Institution].[Subject] s ON (ed.SubjectID = s.SubjectID) WHERE ed.ExamID =" + examID + " ORDER BY s.[Code]";
                DataTable dataTable = DataAccessHelper.ExecuteNonQueryWithResultTable(commandText);
                ObservableCollection<ExamSubjectEntryModel> observableCollection = new ObservableCollection<ExamSubjectEntryModel>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    observableCollection.Add(new ExamSubjectEntryModel
                    {
                        SubjectID = int.Parse(dataRow[0].ToString()),
                        NameOfSubject = dataRow[1].ToString(),
                        ExamDateTime = DateTime.Parse(dataRow[2].ToString()),
                        MaximumScore = outOf
                    });
                }
                KeyValuePair<int, ObservableCollection<ExamSubjectEntryModel>> result = new KeyValuePair<int, ObservableCollection<ExamSubjectEntryModel>>(examID, observableCollection);
                return result;
            });
        }

        public static Task<bool> SaveNewExamResultAsync(ExamResultStudentModel newResult)
        {
            return Task.Run<bool>(delegate
            {
                string text = "BEGIN TRANSACTION\r\nDECLARE @id int; \r\n ";
                text += "SET @id = dbo.GetNewID('Institution.ExamResultHeader')\r\n";
                object obj = text;
                text = string.Concat(new object[]
                {
                    obj,
                    "IF NOT EXISTS (SELECT * FROM [Institution].[ExamResultHeader] WHERE ExamID=",
                    newResult.ExamID,
                    " AND StudentID=",
                    newResult.StudentID,
                    " AND IsActive=1)\r\n"
                });
                obj = text;
                text = string.Concat(new object[]
                {
                    obj,
                    "INSERT INTO [Institution].[ExamResultHeader] (ExamResultID,ExamID,StudentID) VALUES (@id,",
                    newResult.ExamID,
                    ",",
                    newResult.StudentID,
                    ")\r\n"
                });
                obj = text;
                text = string.Concat(new object[]
                {
                    obj,
                    "ELSE SET @id=(SELECT ExamResultID FROM [Institution].[ExamResultHeader] WHERE ExamID=",
                    newResult.ExamID,
                    " AND StudentID=",
                    newResult.StudentID,
                    " AND IsActive=1)\r\n"
                });
                foreach (ExamResultSubjectEntryModel current in newResult.Entries)
                {
                    obj = text;
                    text = string.Concat(new object[]
                    {
                        obj,
                        "IF NOT EXISTS (SELECT * FROM [Institution].[ExamResultDetail] WHERE ExamResultID=@id AND SubjectID=",
                        current.SubjectID,
                        ")\r\n"
                    });
                    obj = text;
                    text = string.Concat(new object[]
                    {
                        obj,
                        "INSERT INTO [Institution].[ExamResultDetail] (ExamResultID,SubjectID,Score,Remarks,Tutor) VALUES (@id,",
                        current.SubjectID,
                        ",'",
                        current.Score,
                        "','",
                        current.Remarks,
                        "','",
                        current.Tutor,
                        "')\r\n"
                    });
                    obj = text;
                    text = string.Concat(new object[]
                    {
                        obj,
                        "ELSE UPDATE [Institution].[ExamResultDetail] SET Score='",
                        current.Score,
                        "', Remarks='",
                        current.Remarks,
                        "', Tutor='",
                        current.Tutor,
                        "' WHERE ExamResultID=@id AND SubjectID=",
                        current.SubjectID
                    });
                }
                text += " COMMIT";
                return DataAccessHelper.ExecuteNonQuery(text);
            });
        }

        public static Task<bool> SaveNewExamResultAsync(ObservableCollection<ExamResultStudentModel> newResult)
        {
            return Task.Run<bool>(delegate
            {
                string text = "BEGIN TRANSACTION\r\nDECLARE @id int; \r\n ";
                foreach (ExamResultStudentModel current in newResult)
                {
                    text += "SET @id = dbo.GetNewID('Institution.ExamResultHeader')\r\n";
                    object obj = text;
                    text = string.Concat(new object[]
                    {
                        obj,
                        "IF NOT EXISTS (SELECT * FROM [Institution].[ExamResultHeader] WHERE ExamID=",
                        current.ExamID,
                        " AND StudentID=",
                        current.StudentID,
                        " AND IsActive=1)\r\n"
                    });
                    obj = text;
                    text = string.Concat(new object[]
                    {
                        obj,
                        "INSERT INTO [Institution].[ExamResultHeader] (ExamResultID,ExamID,StudentID) VALUES (@id,",
                        current.ExamID,
                        ",",
                        current.StudentID,
                        ")\r\n"
                    });
                    obj = text;
                    text = string.Concat(new object[]
                    {
                        obj,
                        "ELSE SET @id=(SELECT ExamResultID FROM [Institution].[ExamResultHeader] WHERE ExamID=",
                        current.ExamID,
                        " AND StudentID=",
                        current.StudentID,
                        " AND IsActive=1)\r\n"
                    });
                    foreach (ExamResultSubjectEntryModel current2 in current.Entries)
                    {
                        obj = text;
                        text = string.Concat(new object[]
                        {
                            obj,
                            "IF NOT EXISTS (SELECT * FROM [Institution].[ExamResultDetail] WHERE ExamResultID=@id AND SubjectID=",
                            current2.SubjectID,
                            ")\r\n"
                        });
                        obj = text;
                        text = string.Concat(new object[]
                        {
                            obj,
                            "INSERT INTO [Institution].[ExamResultDetail] (ExamResultID,SubjectID,Score,Remarks,Tutor) VALUES (@id,",
                            current2.SubjectID,
                            ",'",
                            current2.Score,
                            "','",
                            current2.Remarks,
                            "','",
                            current2.Tutor,
                            "')\r\n"
                        });
                        obj = text;
                        text = string.Concat(new object[]
                        {
                            obj,
                            "ELSE UPDATE [Institution].[ExamResultDetail] SET Score='",
                            current2.Score,
                            "', Remarks='",
                            current2.Remarks,
                            "', Tutor='",
                            current2.Tutor,
                            "' WHERE ExamResultID=@id AND SubjectID=",
                            current2.SubjectID
                        });
                    }
                }
                text += " COMMIT";
                return DataAccessHelper.ExecuteNonQuery(text);
            });
        }

        public static Task<ExamResultStudentModel> GetStudentExamResultAync(int studentID, int examID, decimal outOf)
        {
            string selectStr = string.Concat(new object[]
            {
                "SELECT sssd.SubjectID, s.NameOfSubject, ISNULL(erd.Score,0), erd.Remarks,ssd.Tutor,s.Code,erh.ExamResultID FROM [Institution].[StudentSubjectSelectionDetail] sssd LEFT OUTER JOIN [Institution].[StudentSubjectSelectionHeader] sssh ON(sssd.StudentSubjectSelectionID=sssh.StudentSubjectSelectionID) LEFT OUTER JOIN [Institution].[ExamResultHeader] erh ON (sssh.StudentID=erh.StudentID) LEFT OUTER JOIN [Institution].[ExamResultDetail] erd ON (erh.ExamResultID = erd.ExamResultID AND erd.SubjectID=sssd.SubjectID) LEFT OUTER JOIN [Institution].[Subject] s ON(sssd.SubjectID=s.SubjectID) LEFT OUTER JOIN [Institution].[Student] st ON (sssh.StudentID = st.StudentID) LEFT OUTER JOIN [Institution].[SubjectSetupHeader] ssh ON (ssh.ClassID=st.ClassID) LEFT OUTER JOIN [Institution].[SubjectSetupDetail] ssd ON (ssd.SubjectID=sssd.SubjectID AND ssd.SubjectSetupID=ssh.SubjectSetupID)  WHERE ssh.IsActive=1 AND sssh.IsActive=1 AND erh.IsActive=1 AND sssh.StudentID=",
                studentID,
                " AND erh.ExamID=",
                examID,
                " ORDER BY s.[Code]"
            });
            return Task.Run<ExamResultStudentModel>(delegate
            {
                ExamResultStudentModel examResultStudentModel = new ExamResultStudentModel();
                examResultStudentModel.StudentID = studentID;
                examResultStudentModel.ExamID = examID;
                DataTable dataTable = DataAccessHelper.ExecuteNonQueryWithResultTable(selectStr);
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    ExamResultSubjectEntryModel examResultSubjectEntryModel = new ExamResultSubjectEntryModel();
                    examResultSubjectEntryModel.SubjectID = int.Parse(dataRow[0].ToString());
                    examResultSubjectEntryModel.NameOfSubject = dataRow[1].ToString();
                    examResultSubjectEntryModel.Remarks = dataRow[3].ToString();
                    examResultSubjectEntryModel.OutOf = outOf;
                    examResultSubjectEntryModel.Score = (string.IsNullOrWhiteSpace(dataRow[2].ToString()) ? 0m : decimal.Parse(dataRow[2].ToString()));
                    examResultSubjectEntryModel.Tutor = dataRow[4].ToString();
                    examResultSubjectEntryModel.Code = int.Parse(dataRow[5].ToString());
                    examResultSubjectEntryModel.ExamResultID = int.Parse(dataRow[6].ToString());
                    examResultStudentModel.Entries.Add(examResultSubjectEntryModel);
                }
                return examResultStudentModel;
            });
        }

        public static Task<ExamResultClassModel> GetClassExamResultAsync(int classID, int examID, decimal outOf)
        {
            return Task.Run<ExamResultClassModel>(delegate
            {
                ExamResultClassModel examResultClassModel = new ExamResultClassModel();
                ObservableCollection<SubjectModel> result = DataAccess.GetSubjectsRegistredToClassAsync(classID).Result;
                string text = "SELECT StudentID, NameOfStudent,";
                object obj;
                foreach (SubjectModel current in result)
                {
                    obj = text;
                    text = string.Concat(new object[]
                    {
                        obj,
                        "dbo.GetWeightedExamSubjectScore(StudentID,",
                        examID,
                        ",",
                        current.SubjectID,
                        ",",
                        outOf,
                        "),"
                    });
                }
                text = text.Remove(text.Length - 1);
                obj = text;
                text = string.Concat(new object[]
                {
                    obj,
                    " FROM [Institution].[Student] WHERE ClassID=",
                    classID,
                    " AND IsACtive=1"
                });
                DataTable dataTable = DataAccessHelper.ExecuteNonQueryWithResultTable(text);
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    ExamResultStudentModel examResultStudentModel = new ExamResultStudentModel();
                    examResultStudentModel.StudentID = int.Parse(dataRow[0].ToString());
                    examResultStudentModel.NameOfStudent = dataRow[1].ToString();
                    for (int i = 2; i < dataRow.ItemArray.Length; i++)
                    {
                        if (!string.IsNullOrWhiteSpace(dataRow[i].ToString()))
                        {
                            ExamResultSubjectEntryModel examResultSubjectEntryModel = new ExamResultSubjectEntryModel();
                            examResultSubjectEntryModel.SubjectID = result[i - 2].SubjectID;
                            examResultSubjectEntryModel.NameOfSubject = result[i - 2].NameOfSubject;
                            examResultSubjectEntryModel.Code = result[i - 2].Code;
                            examResultSubjectEntryModel.Score = decimal.Parse(dataRow[i].ToString());
                            examResultSubjectEntryModel.OutOf = outOf;
                            examResultStudentModel.Entries.Add(examResultSubjectEntryModel);
                        }
                    }
                    examResultClassModel.Entries.Add(examResultStudentModel);
                }
                return examResultClassModel;
            });
        }

        public static Task<ClassStudentsExamResultModel> GetClassExamResultForTranscriptAsync(int classID, int examID, decimal outOf)
        {
            return Task.Run<ClassStudentsExamResultModel>(delegate
            {
                ClassStudentsExamResultModel classStudentsExamResultModel = new ClassStudentsExamResultModel();
                ObservableCollection<SubjectModel> result = DataAccess.GetSubjectsRegistredToClassAsync(classID).Result;
                string text = "SELECT StudentID, NameOfStudent,";
                object obj;
                foreach (SubjectModel current in result)
                {
                    obj = text;
                    text = string.Concat(new object[]
                    {
                        obj,
                        "dbo.GetWeightedExamSubjectScore(StudentID,",
                        examID,
                        ",",
                        current.SubjectID,
                        ",",
                        outOf,
                        "),"
                    });
                }
                text = text.Remove(text.Length - 1);
                obj = text;
                text = string.Concat(new object[]
                {
                    obj,
                    " FROM [Institution].[Student] WHERE ClassID=",
                    classID,
                    " AND IsACtive=1"
                });
                DataTable dataTable = DataAccessHelper.ExecuteNonQueryWithResultTable(text);
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    StudentExamResultModel studentExamResultModel = new StudentExamResultModel();
                    studentExamResultModel.StudentID = int.Parse(dataRow[0].ToString());
                    studentExamResultModel.NameOfStudent = dataRow[1].ToString();
                    for (int i = 2; i < dataRow.ItemArray.Length; i++)
                    {
                        if (!string.IsNullOrWhiteSpace(dataRow[i].ToString()))
                        {
                            ExamResultSubjectEntryModel examResultSubjectEntryModel = new ExamResultSubjectEntryModel();
                            examResultSubjectEntryModel.SubjectID = result[i - 2].SubjectID;
                            examResultSubjectEntryModel.NameOfSubject = result[i - 2].NameOfSubject;
                            examResultSubjectEntryModel.Code = result[i - 2].Code;
                            examResultSubjectEntryModel.Score = decimal.Parse(dataRow[i].ToString());
                            examResultSubjectEntryModel.OutOf = outOf;
                            studentExamResultModel.Entries.Add(new StudentTranscriptSubjectModel(examResultSubjectEntryModel));
                        }
                    }
                    classStudentsExamResultModel.Entries.Add(studentExamResultModel);
                }
                return classStudentsExamResultModel;
            });
        }

        private static bool StudentResultAdded(int studentID, ObservableCollection<ExamResultStudentModel> entries)
        {
            bool result = false;
            foreach (ExamResultStudentModel current in entries)
            {
                if (current.StudentID == studentID)
                {
                    result = true;
                    break;
                }
            }
            return result;
        }

        public static Task<ClassModel> GetClassAsync(int classID)
        {
            return Task.Run<ClassModel>(() => DataAccess.GetClass(classID));
        }

        public static ClassModel GetClass(int classID)
        {
            ClassModel classModel = new ClassModel();
            string commandText = "SELECT ClassID,NameOfClass FROM [Institution].[Class] WHERE ClassID=" + classID;
            DataTable dataTable = DataAccessHelper.ExecuteNonQueryWithResultTable(commandText);
            ClassModel result;
            if (dataTable.Rows.Count <= 0)
            {
                result = classModel;
            }
            else
            {
                classModel.ClassID = int.Parse(dataTable.Rows[0][0].ToString());
                classModel.NameOfClass = dataTable.Rows[0][1].ToString();
                result = classModel;
            }
            return result;
        }

        public static Task<bool> SaveNewEventAsync(EventModel em)
        {
            return Task.Run<bool>(delegate
            {
                string commandText = string.Concat(new string[]
                {
                    "BEGIN TRANSACTION\r\nINSERT INTO [Institution].[Event] (Name,StartDateTime,EndDateTime,Location,Subject,Message) VALUES ('",
                    em.Name,
                    "','",
                    em.StartDateTime.ToString(new CultureInfo("en-GB")),
                    "','",
                    em.EndDateTime.ToString("g"),
                    "','",
                    em.Location,
                    "','",
                    em.Subject,
                    "','",
                    em.Message,
                    "')\r\n COMMIT"
                });
                return DataAccessHelper.ExecuteNonQuery(commandText);
            });
        }

        public static Task<ObservableCollection<EventModel>> GetUpcomingEvents()
        {
            return Task.Run<ObservableCollection<EventModel>>(delegate
            {
                string commandText = "SELECT Name, StartDateTime, EndDateTime, Location, Subject, Message FROM [Institution].[Event] WHERE StartDateTime >=CONVERT(datetime, '" + DateTime.Now.ToString("g") + "')";
                DataTable dataTable = DataAccessHelper.ExecuteNonQueryWithResultTable(commandText);
                ObservableCollection<EventModel> observableCollection = new ObservableCollection<EventModel>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    observableCollection.Add(new EventModel
                    {
                        Name = dataRow[0].ToString(),
                        StartDateTime = DateTime.Parse(dataRow[1].ToString()),
                        EndDateTime = DateTime.Parse(dataRow[2].ToString()),
                        Location = dataRow[3].ToString(),
                        Subject = dataRow[4].ToString(),
                        Message = dataRow[5].ToString()
                    });
                }
                return observableCollection;
            });
        }

        public static Task<ObservableCollection<EventModel>> GetAllEvents()
        {
            return Task.Run<ObservableCollection<EventModel>>(delegate
            {
                string commandText = "SELECT Name, StartDateTime, EndDateTime, Location, Subject, Message FROM [Institution].[Event] ORDER BY StartDateTime desc";
                DataTable dataTable = DataAccessHelper.ExecuteNonQueryWithResultTable(commandText);
                ObservableCollection<EventModel> observableCollection = new ObservableCollection<EventModel>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    observableCollection.Add(new EventModel
                    {
                        Name = dataRow[0].ToString(),
                        StartDateTime = DateTime.Parse(dataRow[1].ToString()),
                        EndDateTime = DateTime.Parse(dataRow[2].ToString()),
                        Location = dataRow[3].ToString(),
                        Subject = dataRow[4].ToString(),
                        Message = dataRow[5].ToString()
                    });
                }
                return observableCollection;
            });
        }

        public static Task<bool> SaveNewStudentTransfersAsync(ObservableCollection<StudentTransferModel> students)
        {
            return Task.Run<bool>(delegate
            {
                string text = "BEGIN TRANSACTION\r\n";
                foreach (StudentTransferModel current in students)
                {
                    object obj = text;
                    text = string.Concat(new object[]
                    {
                        obj,
                        "INSERT INTO [Institution].[StudentTransfer] (StudentID,DateTransferred) VALUES (",
                        current.StudentID,
                        ",'",
                        current.DateTransferred.ToString("g"),
                        "')\r\n"
                    });
                }
                text += " COMMIT";
                return DataAccessHelper.ExecuteNonQuery(text);
            });
        }

        public static Task<bool> SaveNewStudentClearancesAsync(ObservableCollection<StudentClearancerModel> students)
        {
            return Task.Run<bool>(delegate
            {
                string text = "BEGIN TRANSACTION\r\n";
                foreach (StudentClearancerModel current in students)
                {
                    object obj = text;
                    text = string.Concat(new object[]
                    {
                        obj,
                        "INSERT INTO [Institution].[StudentClearance] (StudentID,DateCleared) VALUES (",
                        current.StudentID,
                        ",'",
                        current.DateCleared.ToString("g"),
                        "')\r\n"
                    });
                }
                text += " COMMIT";
                return DataAccessHelper.ExecuteNonQuery(text);
            });
        }

        public static Task<bool> SaveNewClassClearance(int classID)
        {
            return Task.Run<bool>(delegate
            {
                //string selectStr="SELECT StudentID FROM [Institution].[Student] WHERE ClassID="
                string text = "BEGIN TRANSACTION\r\n";
            return DataAccessHelper.ExecuteNonQuery(text);
        });
        }

    public static Task<bool> SetStudentActiveAsync(StudentBaseModel student)
        {
            return Task.Run<bool>(delegate
            {
                string text = "BEGIN TRANSACTION\r\n";
                object obj = text;
                text = string.Concat(new object[]
                {
                    obj,
                    "DELETE FROM [Institution].[StudentClearance] WHERE StudentID=",
                    student.StudentID,
                    "\r\n"
                });
                obj = text;
                text = string.Concat(new object[]
                {
                    obj,
                    "DELETE FROM [Institution].[StudentTransfer] WHERE StudentID=",
                    student.StudentID,
                    "\r\n"
                });
                text += " COMMIT";
                return DataAccessHelper.ExecuteNonQuery(text);
            });
        }

        public static Task<bool> SetNewStudentsClassAsync(ObservableCollection<StudentBaseModel> students, int classID)
        {
            return Task.Run<bool>(delegate
            {
                string text = "BEGIN TRANSACTION\r\n";
                foreach (StudentBaseModel current in students)
                {
                    object obj = text;
                    text = string.Concat(new object[]
                    {
                        obj,
                        "UPDATE [Institution].[Student] SET ClassID=",
                        classID,
                        " WHERE StudentID=",
                        current.StudentID,
                        "\r\n"
                    });
                }
                text += " COMMIT";
                return DataAccessHelper.ExecuteNonQuery(text);
            });
        }

        public static Task<bool> SaveNewLeavingCertificateAsync(LeavingCertificateModel leavingCertificate)
        {
            return Task.Run<bool>(delegate
            {
                string commandText = string.Concat(new object[]
                {
                    "BEGIN TRANSACTION\r\nDELETE FROM [Institution].[LeavingCertificate] WHERE StudentID=",
                    leavingCertificate.StudentID,
                    "\r\nINSERT INTO [Institution].[LeavingCertificate] (LeavingCertificateID,StudentID,DateOfIssue,DateOfBirth,DateOfAdmission,DateOfLeaving,Nationality,ClassEntered,ClassLeft,Remarks) VALUES (dbo.GetNewID('Institution.LeavingCertificate'),",
                    leavingCertificate.StudentID,
                    ",'",
                    leavingCertificate.DateOfIssue.ToString("g"),
                    "','",
                    leavingCertificate.DateOfBirth.ToString("d"),
                    "','",
                    leavingCertificate.DateOfAdmission.ToString("g"),
                    "','",
                    leavingCertificate.DateOfLeaving.ToString("g"),
                    "','",
                    leavingCertificate.Nationality,
                    "','",
                    leavingCertificate.ClassEntered,
                    "','",
                    leavingCertificate.ClassLeft,
                    "','",
                    leavingCertificate.Remarks,
                    "')\r\nCOMMIT"
                });
                return DataAccessHelper.ExecuteNonQuery(commandText);
            });
        }

        public static Task<bool> SaveNewDormitory(DormModel newDormitory)
        {
            return Task.Run<bool>(delegate
            {
                string commandText = "BEGIN TRANSACTION\r\nINSERT INTO [Institution].[Dormitory] (NameOfDormitory) VALUES ('" + newDormitory.NameOfDormitory + "')\r\nCOMMIT";
                return DataAccessHelper.ExecuteNonQuery(commandText);
            });
        }

        public static StudentExamResultModel GetStudentExamResult(ExamResultStudentDisplayModel studentResult)
        {
            decimal num = 0m;
            int num2 = 0;
            StudentExamResultModel studentExamResultModel = new StudentExamResultModel();
            studentExamResultModel.ClassPosition = DataAccess.GetClassPosition(studentResult.StudentID, studentResult.ExamID);
            studentExamResultModel.Entries = new ObservableCollection<StudentTranscriptSubjectModel>();
            foreach (ExamResultSubjectEntryModel current in studentResult.Entries)
            {
                studentExamResultModel.Entries.Add(new StudentTranscriptSubjectModel(current));
            }
            studentExamResultModel.NameOfClass = studentResult.NameOfClass;
            studentExamResultModel.NameOfStudent = studentResult.NameOfStudent;
            studentExamResultModel.StudentID = studentResult.StudentID;
            studentExamResultModel.NameOfExam = studentResult.NameOfExam;
            studentExamResultModel.OverAllPosition = DataAccess.GetOverallPosition(studentResult.StudentID, studentResult.ExamID);
            foreach (ExamResultSubjectEntryModel current in studentResult.Entries)
            {
                num += current.Score;
            }
            foreach (StudentTranscriptSubjectModel current2 in studentExamResultModel.Entries)
            {
                num2 += current2.Points;
            }
            studentExamResultModel.MeanGrade = ((studentExamResultModel.Entries.Count > 0) ? DataAccess.CalculateGradeFromPoints((num2 + (studentExamResultModel.Entries.Count - 1)) / studentExamResultModel.Entries.Count) : "E");
            studentExamResultModel.TotalMarks = num;
            studentExamResultModel.Points = DataAccess.CalculatePoints(studentExamResultModel.MeanGrade);
            return studentExamResultModel;
        }

        public static ClassExamResultModel GetClassExamResult(ExamResultClassDisplayModel classResult)
        {
            return new ClassExamResultModel
            {
                ClassID = classResult.ClassID,
                NameOfClass = classResult.NameOfClass,
                Entries = classResult.ResultTable
            };
        }

        internal static string CalculateGrade(decimal scoreNew)
        {
            decimal num = decimal.Ceiling(scoreNew);
            string result;
            if (num >= 85m && num <= 100m)
            {
                result = "A";
            }
            else if (num >= 80m && num <= 84m)
            {
                result = "A-";
            }
            else if (num >= 75m && num <= 79m)
            {
                result = "B+";
            }
            else if (num >= 70m && num <= 74m)
            {
                result = "B";
            }
            else if (num >= 65m && num <= 69m)
            {
                result = "B-";
            }
            else if (num >= 60m && num <= 64m)
            {
                result = "C+";
            }
            else if (num >= 55m && num <= 59m)
            {
                result = "C";
            }
            else if (num >= 50m && num <= 54m)
            {
                result = "C-";
            }
            else if (num >= 45m && num <= 49m)
            {
                result = "D+";
            }
            else if (num >= 40m && num <= 44m)
            {
                result = "D";
            }
            else if (num >= 35m && num <= 39m)
            {
                result = "D-";
            }
            else
            {
                if (!(num >= 0m) || !(num <= 34m))
                {
                    throw new ArgumentOutOfRangeException("Score", "Value [" + num + "] should be a non-negative number less than or equal to 100.");
                }
                result = "E";
            }
            return result;
        }

        internal static int CalculatePoints(string grade)
        {
            switch (grade)
            {
                case "A": return 12;
                case "A-": return 11;
                case "B+": return 10;
                case "B": return 9;
                case "B-": return 8;
                case "C+": return 7;
                case "C": return 6;
                case "C-": return 5;
                case "D+": return 4;
                case "D": return 3;
                case "D-": return 2;
                case "E": return 1;
            }
            throw new ArgumentOutOfRangeException("Grade", "Value [" + grade + "] should be one of: {A,A-,B+,B,B-,C+,C,C-,D+,D,D-,E}.");
        }
        internal static string CalculateGradeFromPoints(decimal points)
        {
            int newPoints = (int)decimal.Ceiling(points);
            switch (newPoints)
            {
                case 12: return "A";
                case 11: return "A-";
                case 10: return "B+";
                case 9: return "B";
                case 8: return "B-";
                case 7: return "C+";
                case 6: return "C";
                case 5: return "C-";
                case 4: return "D+";
                case 3: return "D";
                case 2: return "D-";
                case 1: return "E";
            }
            throw new ArgumentOutOfRangeException("Points", "Value should be a non-zero and non-negative number less than or equal to 12.");
        }
        
        internal static string GetClassPosition(int studentID, int examID)
        {
            int classIDFromStudent = DataAccess.GetClassIDFromStudent(studentID);
            string commandText = string.Concat(new object[]
            {
                "SELECT row_no,no_of_students FROM(SELECT ROW_NUMBER() OVER(ORDER BY ISNULL(SUM(ISNULL(CONVERT(decimal,erd.Score),0)),0) DESC) row_no, res.StudentID,(SELECT COUNT(*) FROM [Institution].[Student] WHERE ClassID =",
                classIDFromStudent,
                ")no_of_students FROM [Institution].[ExamResultDetail] erd RIGHT OUTER JOIN (SELECT StudentID,ExamResultID FROM [Institution].[ExamResultHeader] WHERE IsActive=1 AND ExamID =",
                examID,
                " AND StudentID IN (SELECT StudentID FROM [Institution].[Student] WHERE ClassID=",
                classIDFromStudent,
                ")) res ON (erd.ExamResultID=res.ExamResultID) GROUP BY res.StudentID )x WHERE x.StudentID=",
                studentID
            });
            DataTable dataTable = DataAccessHelper.ExecuteNonQueryWithResultTable(commandText);
            string result;
            if (dataTable.Rows.Count == 0)
            {
                result = "0/0";
            }
            else
            {
                result = dataTable.Rows[0][0].ToString() + "/" + dataTable.Rows[0][1].ToString();
            }
            return result;
        }

        internal static string GetOverallPosition(int studentID, int examID)
        {
            char c = DataAccess.GetClass(DataAccess.GetClassIDFromStudent(studentID)).NameOfClass[5];
            string commandText = string.Concat(new object[]
            {
                "SELECT row_no,studs FROM(SELECT ROW_NUMBER() OVER(ORDER BY ISNULL(SUM(ISNULL(CONVERT(decimal,erd.Score),0)),0) DESC) row_no, res.StudentID, (SELECT COUNT(*) FROM [Institution].[Student] WHERE ClassID IN(SELECT ClassID FROM [Institution].[Class] WHERE NameOfClass LIKE '%",
                c,
                "%'))studs FROM [Institution].[ExamResultDetail] erd RIGHT OUTER JOIN (SELECT StudentID,ExamResultID FROM [Institution].[ExamResultHeader] WHERE IsActive=1 AND ExamID =",
                examID,
                ") res ON (erd.ExamResultID=res.ExamResultID) GROUP BY res.StudentID)x WHERE x.StudentID=",
                studentID
            });
            DataTable dataTable = DataAccessHelper.ExecuteNonQueryWithResultTable(commandText);
            string result;
            if (dataTable.Rows.Count == 0)
            {
                result = "0/0";
            }
            else
            {
                result = dataTable.Rows[0][0].ToString() + "/" + dataTable.Rows[0][1].ToString();
            }
            return result;
        }

        public static Task<bool> HasInvoicedThisTerm(int studentID)
        {
            return Task.Run<bool>(delegate
            {
                int term = DataAccess.GetTerm();
                DateTime? dateTime = null;
                DateTime? dateTime2 = null;
                string text = "IF EXISTS(SELECT * FROM [Sales].[SaleHeader] WHERE CustomerID=" + studentID + " AND OrderDate BETWEEN '";
                switch (term)
                {
                    case 1:
                        dateTime = new DateTime?(new DateTime(DateTime.Now.Year, 1, 1));
                        dateTime2 = new DateTime?(new DateTime(DateTime.Now.Year, 4, 30));
                        break;
                    case 2:
                        dateTime = new DateTime?(new DateTime(DateTime.Now.Year, 5, 1));
                        dateTime2 = new DateTime?(new DateTime(DateTime.Now.Year, 8, 31));
                        break;
                    case 3:
                        dateTime = new DateTime?(new DateTime(DateTime.Now.Year, 9, 1));
                        dateTime2 = new DateTime?(new DateTime(DateTime.Now.Year, 12, 31));
                        break;
                }
                string text2 = text;
                text = string.Concat(new string[]
                {
                    text2,
                    dateTime.Value.Day.ToString(),
                    "/",
                    dateTime.Value.Month.ToString(),
                    "/",
                    dateTime.Value.Year.ToString(),
                    " 00:00:00.000' AND '",
                    dateTime2.Value.Day.ToString(),
                    "/",
                    dateTime2.Value.Month.ToString(),
                    "/",
                    dateTime2.Value.Year.ToString(),
                    " 23:59:59.998') SELECT 'True' ELSE SELECT 'False'"
                });
                return bool.Parse(DataAccessHelper.ExecuteScalar(text));
            });
        }

        private static int GetTerm()
        {
            return DataAccess.GetTerm(DateTime.Now);
        }

        internal static int GetTerm(DateTime date)
        {
            int result;
            if (date.Year == DateTime.Now.Year)
            {
                if (date.Month >= 1 && date.Month <= 4)
                {
                    result = 1;
                }
                else if (date.Month >= 5 && date.Month <= 8)
                {
                    result = 2;
                }
                else
                {
                    result = 3;
                }
            }
            else if (date.Month >= 1 && date.Month <= 4)
            {
                result = date.Year - DateTime.Now.Year - 2;
            }
            else if (date.Month >= 5 && date.Month <= 8)
            {
                result = date.Year - DateTime.Now.Year - 1;
            }
            else
            {
                result = date.Year - DateTime.Now.Year;
            }
            return result;
        }

        private static DateTime GetTermStart(DateTime date)
        {
            DateTime result;
            if (date.Month >= 1 && date.Month <= 4)
            {
                result = new DateTime(date.Year, 1, 1);
            }
            else if (date.Month >= 5 && date.Month <= 8)
            {
                result = new DateTime(date.Year, 5, 1);
            }
            else
            {
                result = new DateTime(date.Year, 9, 1);
            }
            return result;
        }

        private static DateTime GetTermStart(int term)
        {
            DateTime result;
            if (term == -1)
            {
                result = new DateTime(DateTime.Now.Year - 1, 9, 1);
            }
            else if (term == 1)
            {
                result = new DateTime(DateTime.Now.Year, 1, 1);
            }
            else if (term == 2)
            {
                result = new DateTime(DateTime.Now.Year, 5, 1);
            }
            else
            {
                result = new DateTime(DateTime.Now.Year, 9, 1);
            }
            return result;
        }

        private static DateTime GetTermEnd(DateTime date)
        {
            DateTime result;
            if (date.Month >= 1 && date.Month <= 4)
            {
                result = new DateTime(date.Year, 4, 30, 23, 59, 59);
            }
            else if (date.Month >= 5 && date.Month <= 8)
            {
                result = new DateTime(date.Year, 8, 31, 23, 59, 59);
            }
            else
            {
                result = new DateTime(date.Year, 12, 31, 23, 59, 59);
            }
            return result;
        }

        private static DateTime GetTermEnd(int term)
        {
            DateTime result;
            if (term == -1)
            {
                result = new DateTime(DateTime.Now.Year - 1, 12, 31, 23, 59, 59);
            }
            else if (term == 1)
            {
                result = new DateTime(DateTime.Now.Year, 4, 30, 23, 59, 59);
            }
            else if (term == 2)
            {
                result = new DateTime(DateTime.Now.Year, 8, 31, 23, 59, 59);
            }
            else
            {
                result = new DateTime(DateTime.Now.Year, 12, 31, 23, 59, 59);
            }
            return result;
        }

        private static DateTime GetTermStart()
        {
            return DataAccess.GetTermStart(DateTime.Now);
        }

        private static DateTime GetTermEnd()
        {
            return DataAccess.GetTermEnd(DateTime.Now);
        }

        public static Task<ClassBalancesListModel> GetBalancesList(ClassModel selectedClass)
        {
            return Task.Run<ClassBalancesListModel>(async delegate
            {
                ClassBalancesListModel classBalancesListModel = new ClassBalancesListModel();
                classBalancesListModel.ClassID = selectedClass.ClassID;
                classBalancesListModel.NameOfClass = selectedClass.NameOfClass;
                classBalancesListModel.Date = DateTime.Now;
                classBalancesListModel.Total = 0m;
                classBalancesListModel.Entries = await DataAccess.GetClassBalancesListAsync(selectedClass.ClassID);
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
            return Task.Run<ObservableCollection<StudentFeesDefaultModel>>(delegate
            {
                ObservableCollection<StudentFeesDefaultModel> observableCollection = new ObservableCollection<StudentFeesDefaultModel>();
                string commandText = "SELECT StudentID, FirstName+' '+LastName+' '+MiddleName, GuardianPhoneNo,dbo.GetCurrentBalance(StudentID) FROM [Institution].[Student]  WHERE ClassID=" + classID + " AND IsActive = 1";
                DataTable dataTable = DataAccessHelper.ExecuteNonQueryWithResultTable(commandText);
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

        public static Task<ObservableCollection<StudentBaseModel>> GetClassStudents(int classID)
        {
            return Task.Run<ObservableCollection<StudentBaseModel>>(delegate
            {
                ObservableCollection<StudentBaseModel> observableCollection = new ObservableCollection<StudentBaseModel>();
                string commandText = "SELECT StudentID, FirstName+' '+MiddleName+' '+LastName FROM [Institution].[Student] WHERE ClassID =" + classID + " AND StudentID NOT IN (SELECT StudentID FROM [Institution].[StudentClearance]) AND StudentID NOT IN (SELECT StudentID FROM [Institution].[StudentTransfer])";
                DataTable dataTable = DataAccessHelper.ExecuteNonQueryWithResultTable(commandText);
                ObservableCollection<StudentBaseModel> result;
                if (dataTable.Rows.Count == 0)
                {
                    result = observableCollection;
                }
                else
                {
                    foreach (DataRow dataRow in dataTable.Rows)
                    {
                        StudentBaseModel item = new StudentBaseModel(int.Parse(dataRow[0].ToString()), dataRow[1].ToString());
                        observableCollection.Add(item);
                    }
                    result = observableCollection;
                }
                return result;
            });
        }

        public static Task<SaleModel> GetSaleAsync(int feesPaymentID)
        {
            return Task.Run<SaleModel>(delegate
            {
                SaleModel saleModel = new SaleModel();
                string commandText = "SELECT SaleID,CustomerID,EmployeeID,OrderDate,TotalAmt FROM [Sales].[SaleHeader] WHERE PaymentID=" + feesPaymentID;
                DataTable dataTable = DataAccessHelper.ExecuteNonQueryWithResultTable(commandText);
                if (dataTable.Rows.Count > 0)
                {
                    DataRow dataRow = dataTable.Rows[0];
                    saleModel.SaleID = int.Parse(dataRow[0].ToString());
                    saleModel.CustomerID = int.Parse(dataRow[1].ToString());
                    saleModel.EmployeeID = int.Parse(dataRow[2].ToString());
                    saleModel.DateAdded = DateTime.Parse(dataRow[3].ToString());
                    saleModel.OrderTotal = decimal.Parse(dataRow[4].ToString());
                    saleModel.SaleItems = DataAccess.GetSaleItems(saleModel.SaleID);
                }
                return saleModel;
            });
        }

        private static ObservableCollection<FeesStructureEntryModel> GetSaleItems(int saleID)
        {
            ObservableCollection<FeesStructureEntryModel> observableCollection = new ObservableCollection<FeesStructureEntryModel>();
            string commandText = "SELECT Name,Amount FROM [Sales].[SaleDetail] WHERE SaleID=" + saleID;
            DataTable dataTable = DataAccessHelper.ExecuteNonQueryWithResultTable(commandText);
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

        public static Task<SaleModel> GetThisTermInvoice(int studentID)
        {
            return Task.Run<SaleModel>(delegate
            {
                SaleModel saleModel = new SaleModel();
                int term = DataAccess.GetTerm();
                DateTime? dateTime = null;
                DateTime? dateTime2 = null;
                string text = "SELECT SaleID,EmployeeID,PaymentID,OrderDate,TotalAmt FROM [Sales].[SaleHeader] WHERE CustomerID=" + studentID + " AND OrderDate BETWEEN '";
                switch (term)
                {
                    case 1:
                        dateTime = new DateTime?(new DateTime(DateTime.Now.Year, 1, 1));
                        dateTime2 = new DateTime?(new DateTime(DateTime.Now.Year, 4, 30));
                        break;
                    case 2:
                        dateTime = new DateTime?(new DateTime(DateTime.Now.Year, 5, 1));
                        dateTime2 = new DateTime?(new DateTime(DateTime.Now.Year, 8, 31));
                        break;
                    case 3:
                        dateTime = new DateTime?(new DateTime(DateTime.Now.Year, 9, 1));
                        dateTime2 = new DateTime?(new DateTime(DateTime.Now.Year, 12, 31));
                        break;
                }
                string text2 = text;
                text = string.Concat(new string[]
                {
                    text2,
                    dateTime.Value.Day.ToString(),
                    "/",
                    dateTime.Value.Month.ToString(),
                    "/",
                    dateTime.Value.Year.ToString(),
                    " 00:00:00.000' AND '",
                    dateTime2.Value.Day.ToString(),
                    "/",
                    dateTime2.Value.Month.ToString(),
                    "/",
                    dateTime2.Value.Year.ToString(),
                    " 23:59:59.998'"
                });
                DataTable dataTable = DataAccessHelper.ExecuteNonQueryWithResultTable(text);
                if (dataTable.Rows.Count > 0)
                {
                    DataRow dataRow = dataTable.Rows[0];
                    saleModel.SaleID = int.Parse(dataRow[0].ToString());
                    saleModel.CustomerID = studentID;
                    saleModel.EmployeeID = int.Parse(dataRow[1].ToString());
                    saleModel.PaymentID = int.Parse(dataRow[2].ToString());
                    saleModel.DateAdded = DateTime.Parse(dataRow[3].ToString());
                    saleModel.OrderTotal = decimal.Parse(dataRow[4].ToString());
                    saleModel.SaleItems = DataAccess.GetSaleItems(saleModel.SaleID);
                }
                return saleModel;
            });
        }

        public static Task<SaleModel> GetTermInvoice(int studentID, DateTime date)
        {
            return Task.Run<SaleModel>(delegate
            {
                SaleModel saleModel = new SaleModel();
                DateTime termStart = DataAccess.GetTermStart(date);
                DateTime termEnd = DataAccess.GetTermEnd(date);
                string text = "SELECT SaleID,EmployeeID,PaymentID,OrderDate,TotalAmt FROM [Sales].[SaleHeader] WHERE CustomerID=" + studentID + " AND OrderDate BETWEEN '";
                string text2 = text;
                text = string.Concat(new string[]
                {
                    text2,
                    termStart.Day.ToString(),
                    "/",
                    termStart.Month.ToString(),
                    "/",
                    termStart.Year.ToString(),
                    " 00:00:00.000' AND '",
                    termEnd.Day.ToString(),
                    "/",
                    termEnd.Month.ToString(),
                    "/",
                    termEnd.Year.ToString(),
                    " 23:59:59.998'"
                });
                DataTable dataTable = DataAccessHelper.ExecuteNonQueryWithResultTable(text);
                if (dataTable.Rows.Count > 0)
                {
                    DataRow dataRow = dataTable.Rows[0];
                    saleModel.SaleID = int.Parse(dataRow[0].ToString());
                    saleModel.CustomerID = studentID;
                    saleModel.EmployeeID = int.Parse(dataRow[1].ToString());
                    saleModel.PaymentID = int.Parse(dataRow[2].ToString());
                    saleModel.DateAdded = DateTime.Parse(dataRow[3].ToString());
                    saleModel.OrderTotal = decimal.Parse(dataRow[4].ToString());
                    saleModel.SaleItems = DataAccess.GetSaleItems(saleModel.SaleID);
                }
                return saleModel;
            });
        }

        public static Task<ClassStudentListModel> GetClassStudentListAsync(ClassModel selectedClass)
        {
            return Task.Run<ClassStudentListModel>(delegate
            {
                ClassStudentListModel classStudentListModel = new ClassStudentListModel();
                classStudentListModel.ClassID = selectedClass.ClassID;
                classStudentListModel.NameOfClass = selectedClass.NameOfClass;
                string commandText = "SELECT StudentID,FirstName,LastName,MiddleName FROM [Institution].[Student] WHERE ClassID=" + selectedClass.ClassID + " AND IsActive=1";
                DataTable dataTable = DataAccessHelper.ExecuteNonQueryWithResultTable(commandText);
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    StudentBaseModel studentBaseModel = new StudentBaseModel();
                    studentBaseModel.StudentID = int.Parse(dataRow[0].ToString());
                    studentBaseModel.NameOfStudent = string.Concat(new string[]
                    {
                        dataRow[2].ToString(),
                        " ",
                        dataRow[3].ToString(),
                        " ",
                        dataRow[1].ToString()
                    });
                    classStudentListModel.Entries.Add(studentBaseModel);
                }
                return classStudentListModel;
            });
        }

        public static Task<ClassStudentListModel> GetCombinedClassStudentListAsync(CombinedClassModel currentClass)
        {
            return Task.Run<ClassStudentListModel>(delegate
            {
                ClassStudentListModel classStudentListModel = new ClassStudentListModel();
                string text = "0,";
                foreach (ClassModel current in currentClass.Entries)
                {
                    text = text + current.ClassID + ",";
                }
                text = text.Remove(text.Length - 1);
                classStudentListModel.ClassID = 0;
                classStudentListModel.NameOfClass = currentClass.Description;
                string commandText = "SELECT StudentID,FirstName+' '+LastName+' '+MiddleName FROM [Institution].[Student] WHERE ClassID IN (" + text + ") AND IsActive=1";
                DataTable dataTable = DataAccessHelper.ExecuteNonQueryWithResultTable(commandText);
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    StudentBaseModel studentBaseModel = new StudentBaseModel();
                    studentBaseModel.StudentID = int.Parse(dataRow[0].ToString());
                    studentBaseModel.NameOfStudent = dataRow[1].ToString();
                    classStudentListModel.Entries.Add(studentBaseModel);
                }
                return classStudentListModel;
            });
        }

        private static ObservableCollection<StudentExamResultEntryModel> GetTranscriptEntries(int studentID, IEnumerable<ExamWeightModel> exams)
        {
            ObservableCollection<StudentExamResultEntryModel> observableCollection = new ObservableCollection<StudentExamResultEntryModel>();
            int num = 0;
            int num2 = 0;
            int num3 = 0;
            decimal num4 = 100m;
            decimal num5 = 100m;
            decimal num6 = 100m;
            if (exams.Any((ExamWeightModel o) => o.Index == 1))
            {
                num = (from o in exams
                       where o.Index == 1
                       select o).ElementAt(0).ExamID;
                num4 = (from o in exams
                        where o.Index == 1
                        select o).ElementAt(0).Weight;
            }
            if (exams.Any((ExamWeightModel o) => o.Index == 2))
            {
                num2 = (from o in exams
                        where o.Index == 2
                        select o).ElementAt(0).ExamID;
                num5 = (from o in exams
                        where o.Index == 2
                        select o).ElementAt(0).Weight;
            }
            if (exams.Any((ExamWeightModel o) => o.Index == 3))
            {
                num3 = (from o in exams
                        where o.Index == 3
                        select o).ElementAt(0).ExamID;
                num6 = (from o in exams
                        where o.Index == 3
                        select o).ElementAt(0).Weight;
            }
            string commandText = string.Concat(new object[]
            {
                "SELECT sub.NameOfSubject, dbo.GetWeightedExamSubjectScore(",
                studentID,
                ",",
                num,
                ",sssd.SubjectID,",
                num4,
                "),dbo.GetWeightedExamSubjectScore(",
                studentID,
                ",",
                num2,
                ",sssd.SubjectID,",
                num5,
                "),dbo.GetWeightedExamSubjectScore(",
                studentID,
                ",",
                num3,
                ",sssd.SubjectID,",
                num6,
                "),ssd.Tutor,sub.Code,sub.SubjectID,std.Remarks FROM [Institution].[StudentSubjectSelectionDetail] sssd LEFT OUTER JOIN [Institution].[StudentSubjectSelectionHeader] sssh ON (sssd.StudentSubjectSelectionID=sssh.StudentSubjectSelectionID) LEFT OUTER JOIN [Institution].[Subject] sub ON (sssd.SubjectID=sub.SubjectID) LEFT OUTER JOIN [Institution].[Student] s ON(sssh.StudentID=s.StudentID) LEFT OUTER JOIN [Institution].[SubjectSetupHeader] ssh ON(ssh.ClassID=s.ClassID) LEFT OUTER JOIN [Institution].[SubjectSetupDetail] ssd ON(ssh.SubjectSetupID=ssd.SubjectSetupID AND ssd.SubjectID=sub.SubjectID) LEFT OUTER JOIN [Institution].[StudentTranscriptHeader] sth ON(sssh.StudentID=sth.StudentID AND sth.DateSaved BETWEEN CONVERT(datetime,'",
                DataAccess.GetTermStart().ToString("g"),
                "') AND CONVERT(datetime,'",
                DataAccess.GetTermEnd().ToString("g"),
                "')) LEFT OUTER JOIN [Institution].[StudentTranscriptDetail] std ON(std.SubjectID=sssd.SubjectID AND sth.StudentTranscriptID=std.StudentTranscriptID) WHERE sssh.IsActive=1 AND ssh.IsActive=1 AND sssh.StudentID=",
                studentID
            });
            DataTable dataTable = DataAccessHelper.ExecuteNonQueryWithResultTable(commandText);
            foreach (DataRow dataRow in dataTable.Rows)
            {
                StudentExamResultEntryModel studentExamResultEntryModel = new StudentExamResultEntryModel();
                studentExamResultEntryModel.NameOfSubject = dataRow[0].ToString();
                studentExamResultEntryModel.Cat1Score = (string.IsNullOrWhiteSpace(dataRow[1].ToString()) ? null : new decimal?(decimal.Parse(dataRow[1].ToString())));
                studentExamResultEntryModel.Cat2Score = (string.IsNullOrWhiteSpace(dataRow[2].ToString()) ? null : new decimal?(decimal.Parse(dataRow[2].ToString())));
                studentExamResultEntryModel.ExamScore = (string.IsNullOrWhiteSpace(dataRow[3].ToString()) ? null : new decimal?(decimal.Parse(dataRow[3].ToString())));
                studentExamResultEntryModel.MeanScore = (studentExamResultEntryModel.Cat1Score.HasValue ? studentExamResultEntryModel.Cat1Score.Value : 0m) + (studentExamResultEntryModel.Cat2Score.HasValue ? studentExamResultEntryModel.Cat2Score.Value : 0m) + (studentExamResultEntryModel.ExamScore.HasValue ? studentExamResultEntryModel.ExamScore.Value : 0m);
                studentExamResultEntryModel.Code = (string.IsNullOrWhiteSpace(dataRow[5].ToString()) ? 0 : int.Parse(dataRow[5].ToString()));
                studentExamResultEntryModel.Tutor = dataRow[4].ToString();
                studentExamResultEntryModel.SubjectID = int.Parse(dataRow[6].ToString());
                studentExamResultEntryModel.Remarks = studentExamResultEntryModel.GetRemark(studentExamResultEntryModel.MeanScore);
                studentExamResultEntryModel.Grade = DataAccess.CalculateGrade(studentExamResultEntryModel.MeanScore);
                studentExamResultEntryModel.Points = DataAccess.CalculatePoints(studentExamResultEntryModel.Grade);
                observableCollection.Add(studentExamResultEntryModel);
            }
            List<int> optionals = new List<int>
            {
                311,
                312,
                443,
                565
            };
            decimal min = (from a in observableCollection
                           where optionals.Contains(a.Code)
                           select a).Min((StudentExamResultEntryModel o) => o.MeanScore);
            StudentExamResultEntryModel item = observableCollection.First((StudentExamResultEntryModel a) => optionals.Contains(a.Code) && a.MeanScore == min);
            if (observableCollection.Count < 11 && observableCollection.Count > 7)
            {
                observableCollection.Remove(item);
            }
            return observableCollection;
        }

        public static Task<bool> SaveNewStudentTranscript(StudentTranscriptModel transcript, IEnumerable<ExamWeightModel> exams)
        {
            return Task.Run<bool>(delegate
            {
                bool flag = false;
                bool result;
                try
                {
                    string text = "0";
                    string text2 = "0";
                    string text3 = "0";
                    string text4 = "0";
                    string text5 = "0";
                    string text6 = "0";
                    if (exams.Any((ExamWeightModel o) => o.Index == 1))
                    {
                        text = exams.First((ExamWeightModel o1) => o1.Index == 1).ExamID.ToString();
                        text4 = exams.First((ExamWeightModel o1) => o1.Index == 1).Weight.ToString();
                    }
                    if (exams.Any((ExamWeightModel o) => o.Index == 2))
                    {
                        text2 = exams.First((ExamWeightModel o1) => o1.Index == 2).ExamID.ToString();
                        text5 = exams.First((ExamWeightModel o1) => o1.Index == 2).Weight.ToString();
                    }
                    if (exams.Any((ExamWeightModel o) => o.Index == 2))
                    {
                        text3 = exams.First((ExamWeightModel o1) => o1.Index == 3).ExamID.ToString();
                        text6 = exams.First((ExamWeightModel o1) => o1.Index == 3).Weight.ToString();
                    }
                    string text7 = "";
                    text7 = string.Concat(new object[]
                    {
                        "BEGIN TRANSACTION DECLARE @id int;\r\nIF EXISTS (SELECT * FROM [Institution].[StudentTranscriptHeader] WHERE DateSaved BETWEEN CONVERT(datetime,'",
                        DataAccess.GetTermStart().ToString("g"),
                        "') AND CONVERT(datetime,'",
                        DataAccess.GetTermEnd().ToString("g"),
                        "') AND StudentTranscriptID=",
                        transcript.StudentTranscriptID,
                        ")\r\nBEGIN\r\nSET @id=(SELECT StudentTranscriptID FROM [Institution].[StudentTranscriptHeader] WHERE DateSaved BETWEEN CONVERT(datetime,'",
                        DataAccess.GetTermStart().ToString("g"),
                        "') AND CONVERT(datetime,'",
                        DataAccess.GetTermEnd().ToString("g"),
                        "') AND StudentTranscriptID=",
                        transcript.StudentTranscriptID,
                        ")\r\nUPDATE [Institution].[StudentTranscriptHeader] SET Responsibilities='",
                        transcript.Responsibilities,
                        "',ClubsAndSport='",
                        transcript.ClubsAndSport,
                        "',Boarding='",
                        transcript.Boarding,
                        "',ClassTeacher='",
                        transcript.ClassTeacher,
                        "',ClassTeacherComments='",
                        transcript.ClassTeacherComments,
                        "',Principal='",
                        transcript.Principal,
                        "',PrincipalComments='",
                        transcript.PrincipalComments,
                        "',OpeningDay='",
                        transcript.OpeningDay.ToString("g"),
                        "',ClosingDay='",
                        transcript.ClosingDay.ToString("g"),
                        "',DateSaved='",
                        DateTime.Now.ToString("g"),
                        "' WHERE StudentTranscriptID= ",
                        transcript.StudentTranscriptID,
                        "\r\nEND\r\nELSE\r\nBEGIN\r\nSET @id = [dbo].GetNewID('Institution.StudentTranscriptHeader') INSERT INTO [Institution].[StudentTranscriptHeader] (StudentTranscriptID,StudentID,Responsibilities,ClubsAndSport,Boarding,ClassTeacher,ClassTeacherComments,Principal,PrincipalComments,OpeningDay,ClosingDay,DateSaved) VALUES (@id,",
                        transcript.StudentID,
                        ",'",
                        transcript.Responsibilities,
                        "','",
                        transcript.ClubsAndSport,
                        "','",
                        transcript.Boarding,
                        "','",
                        transcript.ClassTeacher,
                        "','",
                        transcript.ClassTeacherComments,
                        "','",
                        transcript.Principal,
                        "','",
                        transcript.PrincipalComments,
                        "','",
                        transcript.OpeningDay.ToString("g"),
                        "','",
                        transcript.ClosingDay.ToString("g"),
                        "','",
                        transcript.DateSaved.ToString("g"),
                        "')\r\nEND\r\n"
                    });
                    foreach (StudentExamResultEntryModel current in transcript.Entries)
                    {
                        object obj = text7;
                        text7 = string.Concat(new object[]
                        {
                            obj,
                            "IF NOT EXISTS (SELECT * FROM [Institution].[StudentTranscriptDetail] WHERE StudentTranscriptID=@id AND SubjectID=",
                            current.SubjectID,
                            ")INSERT INTO [Institution].[StudentTranscriptDetail] (StudentTranscriptID,SubjectID,Remarks) VALUES (@id,",
                            current.SubjectID,
                            ",'",
                            current.Remarks,
                            "')\r\nELSE\r\nUPDATE [Institution].[StudentTranscriptDetail] SET Remarks='",
                            current.Remarks,
                            "' WHERE SubjectID=",
                            current.SubjectID,
                            " AND StudentTranscriptID=@id\r\n"
                        });
                    }
                    string text8 = text7;
                    text7 = string.Concat(new string[]
                    {
                        text8,
                        "IF NOT EXISTS (SELECT * FROM [Institution].[StudentTranscriptExamDetail] WHERE StudentTranscriptID=@id)INSERT INTO [Institution].[StudentTranscriptExamDetail] (StudentTranscriptID,Exam1ID,Exam2ID,Exam3ID,Exam1Weight,Exam2Weight,Exam3Weight) VALUES (@id,",
                        text,
                        ",",
                        text2,
                        ",",
                        text3,
                        ",",
                        text4,
                        ",",
                        text5,
                        ",",
                        text6,
                        ")\r\nELSE\r\nUPDATE [Institution].[StudentTranscriptExamDetail] SET Exam1ID=",
                        text,
                        ", Exam2ID=",
                        text2,
                        ", Exam3ID=",
                        text3,
                        ", Exam1Weight=",
                        text4,
                        ", Exam2Weight=",
                        text5,
                        ", Exam3Weight=",
                        text6,
                        "  WHERE StudentTranscriptID=@id\r\n"
                    });
                    text7 += "COMMIT";
                    result = DataAccessHelper.ExecuteNonQuery(text7);
                    return result;
                }
                catch
                {
                }
                result = flag;
                return result;
            });
        }

        public static async Task<StudentTranscriptModel> GetStudentTranscript(int studentID, IEnumerable<ExamWeightModel> exams, IEnumerable<ClassModel> classes)
        {
            int num = await DataAccess.GetClassIDFromStudentID(studentID);
            int num2 = 0;
            int num3 = 0;
            int num4 = 0;
            if (exams.Any((ExamWeightModel o) => o.Index == 1))
            {
                num2 = (from o in exams
                        where o.Index == 1
                        select o).ElementAt(0).ExamID;
            }
            if (exams.Any((ExamWeightModel o) => o.Index == 2))
            {
                num3 = (from o in exams
                        where o.Index == 2
                        select o).ElementAt(0).ExamID;
            }
            if (exams.Any((ExamWeightModel o) => o.Index == 3))
            {
                num4 = (from o in exams
                        where o.Index == 3
                        select o).ElementAt(0).ExamID;
            }
            string text = "0,";
            foreach (ClassModel current in classes)
            {
                text = text + current.ClassID + ",";
            }
            text = text.Remove(text.Length - 1);
            string text2 = "0,";
            foreach (ExamWeightModel current2 in exams)
            {
                text2 = text2 + current2.ExamID + ",";
            }
            text2 = text2.Remove(text2.Length - 1);
            string commandText = string.Concat(new object[]
            {
                "SELECT s.StudentID, s.NameOfStudent,s.KCPEScore, c.NameOfClass,(SELECT CONVERT(varchar(50),row_no)+'/'+CONVERT(varchar(50),no_of_students) FROM (SELECT ROW_NUMBER() OVER(ORDER BY ISNULL(dbo.GetExamTotalScore(StudentID,",
                num2,
                "),0)+ISNULL(dbo.GetExamTotalScore(StudentID,",
                num3,
                "),0)+ISNULL(dbo.GetExamTotalScore(StudentID,",
                num4,
                "),0) DESC) row_no, StudentID, (SELECT COUNT(*) FROM [Institution].[Student] WHERE ClassID =",
                num,
                " AND IsActive=1) no_of_students FROM [Institution].[Student] WHERE CLassID=",
                num,
                " AND IsActive=1 group by StudentID ) x WHERE x.StudentID=s.StudentID) ClassPosition,(SELECT CONVERT(varchar(50),row_no)+'/'+CONVERT(varchar(50),no_of_students) FROM (SELECT ROW_NUMBER() OVER(ORDER BY ISNULL(dbo.GetExamTotalScore(StudentID,",
                num2,
                "),0)+ISNULL(dbo.GetExamTotalScore(StudentID,",
                num3,
                "),0)+ISNULL(dbo.GetExamTotalScore(StudentID,",
                num4,
                "),0) DESC) row_no, StudentID,(SELECT COUNT(*) FROM [Institution].[Student] WHERE ClassID IN(",
                text,
                ") AND IsActive=1) no_of_students FROM [Institution].[Student] WHERE CLassID IN (",
                text,
                ") AND IsActive=1 GROUP by StudentID ) x WHERE x.StudentID=s.StudentID) OverAllPosition,dbo.GetExamTotalScore(s.StudentID,",
                num2,
                ") Exam1Score,dbo.GetExamTotalScore(s.StudentID,",
                num3,
                ")Exam2Score,dbo.GetExamTotalScore(s.StudentID,",
                num4,
                ")Exam3Score,ISNULL(sth.StudentTranscriptID,0),Responsibilities,ClubsAndSport, Boarding, ClassTeacher,ClassTeacherComments,Principal,PrincipalComments,OpeningDay,ClosingDay,Term1Pos,Term2Pos,Term3Pos,DateSaved FROM [Institution].[Student] s LEFT OUTER JOIN [Institution].[Class] c ON(s.ClassID=c.ClassID) LEFT OUTER JOIN [Institution].[StudentTranscriptHeader] sth ON(sth.StudentID=s.StudentID AND (sth.Exam1ID IN (",
                text2,
                ") OR sth.Exam2ID IN (",
                text2,
                ") OR sth.Exam3ID IN (",
                text2,
                "))) WHERE s.StudentID=",
                studentID
            });
            DataTable dataTable = DataAccessHelper.ExecuteNonQueryWithResultTable(commandText);
            StudentTranscriptModel studentTranscriptModel = new StudentTranscriptModel();
            studentTranscriptModel.StudentID = int.Parse(dataTable.Rows[0][0].ToString());
            studentTranscriptModel.NameOfStudent = dataTable.Rows[0][1].ToString();
            studentTranscriptModel.KCPEScore = (string.IsNullOrWhiteSpace(dataTable.Rows[0][2].ToString()) ? 0 : int.Parse(dataTable.Rows[0][2].ToString()));
            studentTranscriptModel.NameOfClass = dataTable.Rows[0][3].ToString();
            studentTranscriptModel.ClassPosition = dataTable.Rows[0][4].ToString();
            studentTranscriptModel.OverAllPosition = dataTable.Rows[0][5].ToString();
            studentTranscriptModel.CAT1Score = (string.IsNullOrWhiteSpace(dataTable.Rows[0][6].ToString()) ? null : new decimal?(decimal.Parse(dataTable.Rows[0][6].ToString())));
            studentTranscriptModel.CAT2Score = (string.IsNullOrWhiteSpace(dataTable.Rows[0][7].ToString()) ? null : new decimal?(decimal.Parse(dataTable.Rows[0][7].ToString())));
            studentTranscriptModel.ExamScore = (string.IsNullOrWhiteSpace(dataTable.Rows[0][8].ToString()) ? null : new decimal?(decimal.Parse(dataTable.Rows[0][8].ToString())));
            studentTranscriptModel.MeanScore = (studentTranscriptModel.CAT1Score.HasValue ? studentTranscriptModel.CAT1Score.Value : 0m) + (studentTranscriptModel.CAT2Score.HasValue ? studentTranscriptModel.CAT2Score.Value : 0m) + (studentTranscriptModel.ExamScore.HasValue ? studentTranscriptModel.ExamScore.Value : 0m);
            studentTranscriptModel.StudentTranscriptID = int.Parse(dataTable.Rows[0][9].ToString());
            studentTranscriptModel.Responsibilities = dataTable.Rows[0][10].ToString();
            studentTranscriptModel.ClubsAndSport = dataTable.Rows[0][11].ToString();
            studentTranscriptModel.Boarding = dataTable.Rows[0][12].ToString();
            studentTranscriptModel.ClassTeacher = dataTable.Rows[0][13].ToString();
            studentTranscriptModel.ClassTeacherComments = dataTable.Rows[0][14].ToString();
            studentTranscriptModel.Principal = dataTable.Rows[0][15].ToString();
            studentTranscriptModel.PrincipalComments = dataTable.Rows[0][16].ToString();
            studentTranscriptModel.OpeningDay = (string.IsNullOrWhiteSpace(dataTable.Rows[0][17].ToString()) ? DateTime.Now : DateTime.Parse(dataTable.Rows[0][17].ToString()));
            studentTranscriptModel.ClosingDay = (string.IsNullOrWhiteSpace(dataTable.Rows[0][18].ToString()) ? DateTime.Now : DateTime.Parse(dataTable.Rows[0][18].ToString()));
            studentTranscriptModel.Term1Pos = dataTable.Rows[0][19].ToString();
            studentTranscriptModel.Term2Pos = dataTable.Rows[0][20].ToString();
            studentTranscriptModel.Term3Pos = dataTable.Rows[0][21].ToString();
            studentTranscriptModel.DateSaved = (string.IsNullOrWhiteSpace(dataTable.Rows[0][22].ToString()) ? DateTime.Now : DateTime.Parse(dataTable.Rows[0][22].ToString()));
            studentTranscriptModel.Entries = DataAccess.GetTranscriptEntries(studentID, exams);
            studentTranscriptModel.Points = decimal.Ceiling(DataAccess.GetTranscriptAvgPoints(studentTranscriptModel.Entries));
            studentTranscriptModel.MeanGrade = DataAccess.CalculateGradeFromPoints(studentTranscriptModel.Points);
            decimal d = 0m;
            decimal d2 = 0m;
            decimal d3 = 0m;
            ExamWeightModel arg_B8E_1;
            if (!exams.Any((ExamWeightModel o) => o.Index == 1))
            {
                arg_B8E_1 = null;
            }
            else
            {
                arg_B8E_1 = (from o in exams
                             where o.Index == 1
                             select o).ElementAt(0);
            }
            ExamWeightModel examWeightModel = arg_B8E_1;
            ExamWeightModel arg_BF4_1;
            if (!exams.Any((ExamWeightModel o) => o.Index == 2))
            {
                arg_BF4_1 = null;
            }
            else
            {
                arg_BF4_1 = (from o in exams
                             where o.Index == 2
                             select o).ElementAt(0);
            }
            ExamWeightModel examWeightModel2 = arg_BF4_1;
            ExamWeightModel arg_C5A_1;
            if (!exams.Any((ExamWeightModel o) => o.Index == 3))
            {
                arg_C5A_1 = null;
            }
            else
            {
                arg_C5A_1 = (from o in exams
                             where o.Index == 3
                             select o).ElementAt(0);
            }
            ExamWeightModel examWeightModel3 = arg_C5A_1;
            foreach (StudentExamResultEntryModel current3 in studentTranscriptModel.Entries)
            {
                if (current3.Cat1Score.HasValue && examWeightModel != null)
                {
                    DataAccess.CalculatePoints(DataAccess.CalculateGrade(DataAccess.ConvertScoreToOutOf(current3.Cat1Score.Value, examWeightModel.OutOf, 100m)));
                }
                d += ((current3.Cat1Score.HasValue && examWeightModel != null) ? DataAccess.CalculatePoints(DataAccess.CalculateGrade(decimal.Ceiling(DataAccess.ConvertScoreToOutOf(current3.Cat1Score.Value, examWeightModel.Weight, 100m)))) : 1);
                d2 += ((current3.Cat2Score.HasValue && examWeightModel2 != null) ? DataAccess.CalculatePoints(DataAccess.CalculateGrade(decimal.Ceiling(DataAccess.ConvertScoreToOutOf(current3.Cat2Score.Value, examWeightModel2.Weight, 100m)))) : 1);
                d3 += ((current3.ExamScore.HasValue && examWeightModel3 != null) ? DataAccess.CalculatePoints(DataAccess.CalculateGrade(decimal.Ceiling(DataAccess.ConvertScoreToOutOf(current3.ExamScore.Value, examWeightModel3.Weight, 100m)))) : 1);
            }
            studentTranscriptModel.CAT1Grade = (((num2 == 0 && studentTranscriptModel.Entries.Count > 0) || d == 0m || studentTranscriptModel.Entries.Count == 0) ? "E" : DataAccess.CalculateGradeFromPoints((int)decimal.Ceiling(d / studentTranscriptModel.Entries.Count)));
            studentTranscriptModel.CAT2Grade = (((num3 == 0 && studentTranscriptModel.Entries.Count > 0) || d == 0m || studentTranscriptModel.Entries.Count == 0) ? "E" : DataAccess.CalculateGradeFromPoints((int)decimal.Ceiling(d2 / studentTranscriptModel.Entries.Count)));
            studentTranscriptModel.ExamGrade = (((num4 == 0 && studentTranscriptModel.Entries.Count > 0) || d == 0m || studentTranscriptModel.Entries.Count == 0) ? "E" : DataAccess.CalculateGradeFromPoints((int)decimal.Ceiling(d3 / studentTranscriptModel.Entries.Count)));
            return studentTranscriptModel;
        }

        private static int GetTermExamID(IEnumerable<ExamWeightModel> exams, int index)
        {
            int[] array = new int[3];
            int num = 0;
            int num2 = 0;
            int num3 = 0;
            if (exams.Any((ExamWeightModel o) => o.Index == 1))
            {
                num = (from o in exams
                       where o.Index == 1
                       select o).ElementAt(0).ExamID;
            }
            if (exams.Any((ExamWeightModel o) => o.Index == 2))
            {
                num2 = (from o in exams
                        where o.Index == 2
                        select o).ElementAt(0).ExamID;
            }
            if (exams.Any((ExamWeightModel o) => o.Index == 3))
            {
                num3 = (from o in exams
                        where o.Index == 3
                        select o).ElementAt(0).ExamID;
            }
            array[0] = num;
            array[1] = num2;
            array[2] = num3;
            return array[index - 1];
        }

        private static decimal GetTermExamWeight(IEnumerable<ExamWeightModel> exams, int index)
        {
            decimal[] array = new decimal[3];
            decimal num = 0m;
            decimal num2 = 0m;
            decimal num3 = 0m;
            if (exams.Any((ExamWeightModel o) => o.Index == 1))
            {
                num = (from o in exams
                       where o.Index == 1
                       select o).ElementAt(0).Weight;
            }
            if (exams.Any((ExamWeightModel o) => o.Index == 2))
            {
                num2 = (from o in exams
                        where o.Index == 2
                        select o).ElementAt(0).Weight;
            }
            if (exams.Any((ExamWeightModel o) => o.Index == 3))
            {
                num3 = (from o in exams
                        where o.Index == 3
                        select o).ElementAt(0).Weight;
            }
            array[0] = num;
            array[1] = num2;
            array[2] = num3;
            return array[index - 1];
        }

        private static List<ExamWeightModel> GetOtherTermExams(int studentID, List<int> otherTerms)
        {
            string commandText = string.Concat(new object[]
            {
                "SELECT eh.ExamID,eh.NameOfExam,eh.ExamDatetime,ISNULL(eh.OutOf,100) OutOf, [Index] = CASE (eh.ExamID) \r\nWHEN sted.Exam1ID THEN 1\r\nWHEN sted.Exam2ID THEN 2\r\nWHEN sted.Exam3ID THEN 3\r\nELSE 0\r\nEND, [Weight] = CASE (eh.ExamID) \r\nWHEN sted.Exam1ID THEN sted.Exam1Weight\r\nWHEN sted.Exam2ID THEN sted.Exam2Weight\r\nWHEN sted.Exam3ID THEN sted.Exam3Weight\r\nELSE 0\r\nEND\r\nFROM [Institution].[ExamHeader] eh LEFT OUTER JOIN [Institution].[StudentTranscriptExamDetail] sted  ON (sted.Exam1ID = eh.ExamID OR sted.Exam2ID = eh.ExamID OR sted.Exam3ID = eh.ExamID) LEFT OUTER JOIN [Institution].[StudentTranscriptHeader] sth  ON (sth.StudentTranscriptID=sted.StudentTranscriptID) WHERE sth.StudentID=",
                studentID,
                " AND ((eh.ExamDateTime>= CONVERT(datetime,'",
                DataAccess.GetTermStart(otherTerms[0]).ToString("g"),
                "') AND eh.ExamDateTime<= CONVERT(datetime,'",
                DataAccess.GetTermEnd(otherTerms[0]).ToString("g"),
                "')) OR((eh.ExamDateTime>= CONVERT(datetime,'",
                DataAccess.GetTermStart(otherTerms[1]).ToString("g"),
                "') AND eh.ExamDateTime<= CONVERT(datetime,'",
                DataAccess.GetTermEnd(otherTerms[1]).ToString("g"),
                "')) OR((eh.ExamDateTime>= CONVERT(datetime,'",
                DataAccess.GetTermStart(otherTerms[2]).ToString("g"),
                "') AND eh.ExamDateTime<= CONVERT(datetime,'",
                DataAccess.GetTermEnd(otherTerms[2]).ToString("g"),
                "')))))"
            });
            DataTable dataTable = DataAccessHelper.ExecuteNonQueryWithResultTable(commandText);
            List<ExamWeightModel> list = new List<ExamWeightModel>();
            foreach (DataRow dataRow in dataTable.Rows)
            {
                list.Add(new ExamWeightModel
                {
                    ExamID = int.Parse(dataRow[0].ToString()),
                    NameOfExam = dataRow[1].ToString(),
                    ExamDateTime = DateTime.Parse(dataRow[2].ToString()),
                    OutOf = decimal.Parse(dataRow[3].ToString()),
                    Index = int.Parse(dataRow[4].ToString()),
                    Weight = decimal.Parse(dataRow[5].ToString())
                });
            }
            return list;
        }

        private static List<ExamWeightModel> GetOtherTermClassExams(int classID, List<int> otherTerms)
        {
            string commandText = string.Concat(new object[]
            {
                "SELECT DISTINCT eh.ExamID,eh.NameOfExam,eh.ExamDatetime,ISNULL(eh.OutOf,100) OutOf, [Index] = CASE (eh.ExamID) \r\nWHEN sted.Exam1ID THEN 1\r\nWHEN sted.Exam2ID THEN 2\r\nWHEN sted.Exam3ID THEN 3\r\nELSE 0\r\nEND, [Weight] = CASE (eh.ExamID) \r\nWHEN sted.Exam1ID THEN sted.Exam1Weight\r\nWHEN sted.Exam2ID THEN sted.Exam2Weight\r\nWHEN sted.Exam3ID THEN sted.Exam3Weight\r\nELSE 0\r\nEND\r\nFROM [Institution].[ExamHeader] eh LEFT OUTER JOIN [Institution].[StudentTranscriptExamDetail] sted  ON (sted.Exam1ID = eh.ExamID OR sted.Exam2ID = eh.ExamID OR sted.Exam3ID = eh.ExamID) LEFT OUTER JOIN [Institution].[StudentTranscriptHeader] sth  ON (sth.StudentTranscriptID=sted.StudentTranscriptID) LEFT OUTER JOIN [Institution].[Student] s ON (s.StudentID=sth.StudentID) WHERE s.ClassID=",
                classID,
                " AND ((eh.ExamDateTime>= CONVERT(datetime,'",
                DataAccess.GetTermStart(otherTerms[0]).ToString("g"),
                "') AND eh.ExamDateTime<= CONVERT(datetime,'",
                DataAccess.GetTermEnd(otherTerms[0]).ToString("g"),
                "')) OR((eh.ExamDateTime>= CONVERT(datetime,'",
                DataAccess.GetTermStart(otherTerms[1]).ToString("g"),
                "') AND eh.ExamDateTime<= CONVERT(datetime,'",
                DataAccess.GetTermEnd(otherTerms[1]).ToString("g"),
                "')) OR((eh.ExamDateTime>= CONVERT(datetime,'",
                DataAccess.GetTermStart(otherTerms[2]).ToString("g"),
                "') AND eh.ExamDateTime<= CONVERT(datetime,'",
                DataAccess.GetTermEnd(otherTerms[2]).ToString("g"),
                "')))))"
            });
            DataTable dataTable = DataAccessHelper.ExecuteNonQueryWithResultTable(commandText);
            List<ExamWeightModel> list = new List<ExamWeightModel>();
            foreach (DataRow dataRow in dataTable.Rows)
            {
                list.Add(new ExamWeightModel
                {
                    ExamID = int.Parse(dataRow[0].ToString()),
                    NameOfExam = dataRow[1].ToString(),
                    ExamDateTime = DateTime.Parse(dataRow[2].ToString()),
                    OutOf = decimal.Parse(dataRow[3].ToString()),
                    Index = int.Parse(dataRow[4].ToString()),
                    Weight = decimal.Parse(dataRow[5].ToString())
                });
            }
            return list;
        }

        public static Task<StudentTranscriptModel2> GetStudentTranscript2(int studentID, IEnumerable<ExamWeightModel> exams, IEnumerable<ClassModel> classes)
        {
            return DataAccess.GetStudentTranscript2(studentID, exams, classes, 0);
        }

        public static Task<StudentTranscriptModel2> GetStudentTranscript2(int studentID, IEnumerable<ExamWeightModel> exams, IEnumerable<ClassModel> classes, int transcriptID)
        {
            return Task.Run(async () =>
            {
                var c = await GetClassIDFromStudentID(studentID);
                int pyT3E1 = 0, pyT3E2 = 0, pyT3E3 = 0, t1E1 = 0, t1E2 = 0, t1E3 = 0, t2E1 = 0, t2E2 = 0, t2E3 = 0, t3E1 = 0, t3E2 = 0, t3E3 = 0;
                decimal pyT3E1W = 0, pyT3E2W = 0, pyT3E3W = 0, t1E1W = 0, t1E2W = 0, t1E3W = 0, t2E1W = 0, t2E2W = 0, t2E3W = 0, t3E1W = 0, t3E2W = 0, t3E3W = 0;
                int currentTerm = GetTerm();
                switch (currentTerm)
                {
                    case 1:
                        t1E1 = GetTermExamID(exams, 1);
                        t1E1W = GetTermExamWeight(exams, 1);
                        t1E2 = GetTermExamID(exams, 2);
                        t1E2W = GetTermExamWeight(exams, 2);
                        t1E3 = GetTermExamID(exams, 3);
                        t1E3W = GetTermExamWeight(exams, 3);
                        break;
                    case 2:
                        t2E1 = GetTermExamID(exams, 1);
                        t2E1W = GetTermExamWeight(exams, 1);
                        t2E2 = GetTermExamID(exams, 2);
                        t2E2W = GetTermExamWeight(exams, 2);
                        t2E3 = GetTermExamID(exams, 3);
                        t2E3W = GetTermExamWeight(exams, 3); break;
                    case 3:
                        t3E1 = GetTermExamID(exams, 1);
                        t3E1W = GetTermExamWeight(exams, 1);
                        t3E2 = GetTermExamID(exams, 2);
                        t3E2W = GetTermExamWeight(exams, 2);
                        t3E3 = GetTermExamID(exams, 3);
                        t3E3W = GetTermExamWeight(exams, 3); break;
                }



                List<int> otherTerms = new List<int>(new List<int>(3) { -1, 1, 2, 3 }.Where(o => o != currentTerm));
                var s = GetOtherTermExams(studentID, otherTerms);
                foreach (int term in otherTerms)
                {
                    if (term == -1)
                    {
                        pyT3E1 = s.Any(o => o.Index == 1 && GetTerm(o.ExamDateTime) == -1) ? s.First(o => o.Index == 1 && GetTerm(o.ExamDateTime) == -1).ExamID : 0;
                        pyT3E1W = s.Any(o => o.Index == 1 && GetTerm(o.ExamDateTime) == -1) ? s.First(o => o.Index == 1 && GetTerm(o.ExamDateTime) == -1).Weight : 0;
                        pyT3E2 = s.Any(o => o.Index == 2 && GetTerm(o.ExamDateTime) == -1) ? s.First(o => o.Index == 2 && GetTerm(o.ExamDateTime) == -1).ExamID : 0;
                        pyT3E2W = s.Any(o => o.Index == 2 && GetTerm(o.ExamDateTime) == -1) ? s.First(o => o.Index == 2 && GetTerm(o.ExamDateTime) == -1).Weight : 0;
                        pyT3E3 = s.Any(o => o.Index == 3 && GetTerm(o.ExamDateTime) == -1) ? s.First(o => o.Index == 3 && GetTerm(o.ExamDateTime) == -1).ExamID : 0;
                        pyT3E3W = s.Any(o => o.Index == 3 && GetTerm(o.ExamDateTime) == -1) ? s.First(o => o.Index == 3 && GetTerm(o.ExamDateTime) == -1).Weight : 0;
                    }
                    else if (term == 1)
                    {
                        t1E1 = s.Any(o => o.Index == 1 && GetTerm(o.ExamDateTime) == 1) ? s.First(o => o.Index == 1 && GetTerm(o.ExamDateTime) == 1).ExamID : 0;
                        t1E1W = s.Any(o => o.Index == 1 && GetTerm(o.ExamDateTime) == 1) ? s.First(o => o.Index == 1 && GetTerm(o.ExamDateTime) == 1).Weight : 0;
                        t1E2 = s.Any(o => o.Index == 2 && GetTerm(o.ExamDateTime) == 1) ? s.First(o => o.Index == 2 && GetTerm(o.ExamDateTime) == 1).ExamID : 0;
                        t1E2W = s.Any(o => o.Index == 2 && GetTerm(o.ExamDateTime) == 1) ? s.First(o => o.Index == 2 && GetTerm(o.ExamDateTime) == 1).Weight : 0;
                        t1E3 = s.Any(o => o.Index == 3 && GetTerm(o.ExamDateTime) == 1) ? s.First(o => o.Index == 3 && GetTerm(o.ExamDateTime) == 1).ExamID : 0;
                        t1E3W = s.Any(o => o.Index == 3 && GetTerm(o.ExamDateTime) == 1) ? s.First(o => o.Index == 3 && GetTerm(o.ExamDateTime) == 1).Weight : 0;
                    }
                    else if (term == 2)
                    {
                        t2E1 = s.Any(o => o.Index == 1 && GetTerm(o.ExamDateTime) == 2) ? s.First(o => o.Index == 1 && GetTerm(o.ExamDateTime) == 2).ExamID : 0;
                        t2E1W = s.Any(o => o.Index == 1 && GetTerm(o.ExamDateTime) == 2) ? s.First(o => o.Index == 1 && GetTerm(o.ExamDateTime) == 2).Weight : 0;
                        t2E2 = s.Any(o => o.Index == 2 && GetTerm(o.ExamDateTime) == 2) ? s.First(o => o.Index == 2 && GetTerm(o.ExamDateTime) == 2).ExamID : 0;
                        t2E2W = s.Any(o => o.Index == 2 && GetTerm(o.ExamDateTime) == 2) ? s.First(o => o.Index == 2 && GetTerm(o.ExamDateTime) == 2).Weight : 0;
                        t2E3 = s.Any(o => o.Index == 3 && GetTerm(o.ExamDateTime) == 2) ? s.First(o => o.Index == 3 && GetTerm(o.ExamDateTime) == 2).ExamID : 0;
                        t2E3W = s.Any(o => o.Index == 3 && GetTerm(o.ExamDateTime) == 2) ? s.First(o => o.Index == 3 && GetTerm(o.ExamDateTime) == 2).Weight : 0;
                    }
                    else
                    {
                        t3E1 = s.Any(o => o.Index == 1 && GetTerm(o.ExamDateTime) == 3) ? s.First(o => o.Index == 1 && GetTerm(o.ExamDateTime) == 3).ExamID : 0;
                        t3E1W = s.Any(o => o.Index == 1 && GetTerm(o.ExamDateTime) == 3) ? s.First(o => o.Index == 1 && GetTerm(o.ExamDateTime) == 3).Weight : 0;
                        t3E2 = s.Any(o => o.Index == 2 && GetTerm(o.ExamDateTime) == 3) ? s.First(o => o.Index == 2 && GetTerm(o.ExamDateTime) == 3).ExamID : 0;
                        t3E2W = s.Any(o => o.Index == 2 && GetTerm(o.ExamDateTime) == 3) ? s.First(o => o.Index == 2 && GetTerm(o.ExamDateTime) == 3).Weight : 0;
                        t3E3 = s.Any(o => o.Index == 3 && GetTerm(o.ExamDateTime) == 3) ? s.First(o => o.Index == 3 && GetTerm(o.ExamDateTime) == 3).ExamID : 0;
                        t3E3W = s.Any(o => o.Index == 3 && GetTerm(o.ExamDateTime) == 3) ? s.First(o => o.Index == 3 && GetTerm(o.ExamDateTime) == 3).Weight : 0;
                    }
                }

                string cStr = "0,";
                foreach (var t in classes)
                    cStr += t.ClassID + ",";
                cStr = cStr.Remove(cStr.Length - 1);

                string pyT3ExStr = pyT3E1 + "," + pyT3E2 + "," + pyT3E3;
                string t1ExStr = t1E1 + "," + t1E2 + "," + t1E3;
                string t2ExStr = t2E1 + "," + t2E2 + "," + t2E3;
                string t3ExStr = t3E1 + "," + t3E2 + "," + t3E3;



                string selectStr = (transcriptID == 0) ? "SELECT t1.StudentTranscriptID,t1.StudentID, t1.NameOfStudent,t1.KCPEScore, t1.NameOfClass,t1.Responsibilities,t1.ClubsAndSport,t1.Boarding,t1.ClassTeacherComments, " +
                    "t1.PrincipalComments,t1.OpeningDay,t1.ClosingDay,t1.DateSaved,t1.ClassPosition,t1.OverAllPosition,t1.Exam1Score,t1.Exam2Score,t1.Exam3Score,t2.T2ClassPosition," +
                    "t2.T2OverAllPosition,t2.T2Exam1Score,t2.T2Exam2Score,t2.T2Exam3Score,t3.T3ClassPosition,t3.T3OverAllPosition,t3.T3Exam1Score,t3.T3Exam2Score,t3.T3Exam3Score," +
                    "pyT3.PyT3Exam1Score,pyT3.PyT3Exam2Score,pyt3.PyT3Exam3Score,t1.SPhoto FROM (SELECT s.StudentID, s.NameOfStudent,s.KCPEScore, c.NameOfClass,ISNULL(sth.StudentTranscriptID,0) StudentTranscriptID,Responsibilities,ClubsAndSport," +
                    " Boarding,ClassTeacherComments,PrincipalComments,OpeningDay,ClosingDay,DateSaved,(SELECT CONVERT(varchar(50),row_no)+'/'+CONVERT(varchar(50),no_of_students) " +
                    " FROM (SELECT ROW_NUMBER() OVER(ORDER BY ISNULL(dbo.GetWeightedExamTotalScore(StudentID," + t1E1 + "," + t1E1W + "),0)+ISNULL(dbo.GetWeightedExamTotalScore(StudentID," + t1E2 +
                    "," + t1E2W + "),0)+ISNULL(dbo.GetWeightedExamTotalScore(StudentID," + t1E3 + "," + t1E3W + "),0) DESC) row_no, StudentID, (SELECT COUNT(*) FROM [Institution].[Student] WHERE ClassID =" + c + " AND IsActive=1)" +
                    " no_of_students FROM [Institution].[Student] WHERE CLassID=" + c + " AND IsActive=1 group by StudentID ) x WHERE x.StudentID=s.StudentID) ClassPosition," +
                    "(SELECT CONVERT(varchar(50),row_no)+'/'+CONVERT(varchar(50),no_of_students) FROM (SELECT ROW_NUMBER() OVER(ORDER BY ISNULL(dbo.GetWeightedExamTotalScore(StudentID," + t1E1 +
                    "," + t1E1W + "),0)+ISNULL(dbo.GetWeightedExamTotalScore(StudentID," + t2E2 + "," + t1E2W + "),0)+ISNULL(dbo.GetWeightedExamTotalScore(StudentID," + t1E3 + "," + t1E3W + "),0) DESC) row_no, StudentID," +
                    "(SELECT COUNT(*) FROM [Institution].[Student] WHERE ClassID IN(" + cStr + ") AND IsActive=1) no_of_students FROM [Institution].[Student] WHERE CLassID IN (" + cStr +
                    ") AND IsActive=1 GROUP by StudentID ) x WHERE x.StudentID=s.StudentID) OverAllPosition,dbo.GetWeightedExamTotalScore(s.StudentID," + t1E1 + "," + t1E1W + ") Exam1Score,dbo.GetWeightedExamTotalScore(s.StudentID," + t1E2 +
                    "," + t1E2W + ")Exam2Score,dbo.GetWeightedExamTotalScore(s.StudentID," + t1E3 + "," + t1E3W + ")Exam3Score,SPhoto FROM [Institution].[Student] s LEFT OUTER JOIN [Institution].[Class] c ON(s.ClassID=c.ClassID)" +
                    " LEFT OUTER JOIN [Institution].[StudentTranscriptHeader] sth ON(sth.StudentID=s.StudentID " +
                    ") LEFT OUTER JOIN [Institution].[StudentTranscriptExamDetail] sted ON (sted.StudentTranscriptID=sth.StudentTranscriptID) WHERE s.IsActive=1 AND s.StudentID=" + studentID +
                    ") t1 " +


                    "LEFT OUTER JOIN (SELECT s.StudentID,(SELECT CONVERT(varchar(50),row_no)+'/'+CONVERT(varchar(50),no_of_students) FROM (SELECT ROW_NUMBER() " +
                    "OVER(ORDER BY ISNULL(dbo.GetWeightedExamTotalScore(StudentID," + t2E1 + "," + t2E1W + "),0)+ISNULL(dbo.GetWeightedExamTotalScore(StudentID," + t2E2 + "," + t2E2W + "),0)+ISNULL(dbo.GetWeightedExamTotalScore(StudentID," + t2E3 +
                    "," + t2E3W + "),0) DESC) row_no, StudentID, (SELECT COUNT(*) FROM [Institution].[Student] WHERE ClassID =" + c + " AND IsActive=1) no_of_students FROM [Institution].[Student]" +
                    " WHERE CLassID=" + c + " AND IsActive=1 group by StudentID ) x WHERE x.StudentID=s.StudentID) T2ClassPosition,(SELECT CONVERT(varchar(50),row_no)+'/'+CONVERT(varchar(50),no_of_students) " +
                    "FROM (SELECT ROW_NUMBER() OVER(ORDER BY ISNULL(dbo.GetWeightedExamTotalScore(StudentID," + t2E1 + "," + t2E1W + "),0)+ISNULL(dbo.GetWeightedExamTotalScore(StudentID," + t2E2 +
                    "," + t2E2W + "),0)+ISNULL(dbo.GetWeightedExamTotalScore(StudentID," + t2E3 + "," + t2E3W + "),0) DESC) row_no, StudentID,(SELECT COUNT(*) FROM [Institution].[Student] WHERE ClassID IN(" + cStr +
                    ") AND IsActive=1) no_of_students FROM [Institution].[Student] WHERE CLassID IN (" + cStr + ") AND IsActive=1 GROUP by StudentID ) x WHERE x.StudentID=s.StudentID)" +
                    " T2OverAllPosition,dbo.GetWeightedExamTotalScore(s.StudentID," + t2E1 + "," + t2E1W + ") T2Exam1Score,dbo.GetWeightedExamTotalScore(s.StudentID," + t2E2 + "," + t2E2W + ")T2Exam2Score,dbo.GetWeightedExamTotalScore(s.StudentID," + t2E3 + "," + t2E3W + ")T2Exam3Score " +
                    "FROM [Institution].[Student] s LEFT OUTER JOIN [Institution].[Class] c ON(s.ClassID=c.ClassID) LEFT OUTER JOIN [Institution].[StudentTranscriptHeader] " +
                    "sth ON(sth.StudentID=s.StudentID AND sth.DateSaved BETWEEN CONVERT(datetime,'" + GetTermStart().ToString("g") + "') AND CONVERT(datetime,'" + GetTermEnd().ToString("g") +
                    "')) " +
                    "LEFT OUTER JOIN [Institution].[StudentTranscriptExamDetail] sted ON (sted.StudentTranscriptID=sth.StudentTranscriptID) WHERE s.IsActive=1 AND s.StudentID=" + studentID +
                    ") t2 ON (t1.StudentID=t2.StudentID) " +


                    "LEFT OUTER JOIN (SELECT s.StudentID,(SELECT CONVERT(varchar(50),row_no)+'/'+CONVERT(varchar(50),no_of_students) FROM (SELECT ROW_NUMBER() " +
                    "OVER(ORDER BY ISNULL(dbo.GetWeightedExamTotalScore(StudentID," + t3E1 + "," + t3E1W + "),0)+ISNULL(dbo.GetWeightedExamTotalScore(StudentID," + t3E2 + "," + t3E2W + "),0)+ISNULL(dbo.GetWeightedExamTotalScore(StudentID," + t3E3 +
                    "," + t3E3W + "),0) DESC) row_no, StudentID, (SELECT COUNT(*) FROM [Institution].[Student] WHERE ClassID =" + c + " AND IsActive=1) no_of_students FROM [Institution].[Student] " +
                    "WHERE CLassID=" + c + " AND IsActive=1 group by StudentID ) x WHERE x.StudentID=s.StudentID) T3ClassPosition,(SELECT CONVERT(varchar(50),row_no)+'/'+CONVERT(varchar(50),no_of_students) " +
                    "FROM (SELECT ROW_NUMBER() OVER(ORDER BY ISNULL(dbo.GetWeightedExamTotalScore(StudentID," + t3E1 + "," + t3E1W + "),0)+ISNULL(dbo.GetWeightedExamTotalScore(StudentID," + t3E2 +
                    "," + t3E2W + "),0)+ISNULL(dbo.GetWeightedExamTotalScore(StudentID," + t3E3 + "," + t3E3W + "),0) DESC) row_no, StudentID,(SELECT COUNT(*) FROM [Institution].[Student] WHERE ClassID IN(" + cStr +
                    ") AND IsActive=1) no_of_students FROM [Institution].[Student] WHERE CLassID IN (" + cStr + ") AND IsActive=1 GROUP by StudentID ) x WHERE x.StudentID=s.StudentID) " +
                    "T3OverAllPosition,dbo.GetWeightedExamTotalScore(s.StudentID," + t3E1 + "," + t3E1W + ") T3Exam1Score,dbo.GetWeightedExamTotalScore(s.StudentID," + t3E2 + "," + t3E2W + ")T3Exam2Score,dbo.GetWeightedExamTotalScore(s.StudentID," + t3E3 + "," + t3E3W + ")T3Exam3Score " +
                    "FROM [Institution].[Student] s LEFT OUTER JOIN [Institution].[Class] c ON(s.ClassID=c.ClassID) LEFT OUTER JOIN [Institution].[StudentTranscriptHeader] " +
                    "sth ON(sth.StudentID=s.StudentID AND sth.DateSaved BETWEEN CONVERT(datetime,'" + GetTermStart().ToString("g") + "') AND CONVERT(datetime,'" + GetTermEnd().ToString("g") +
                    "')) LEFT OUTER JOIN [Institution].[StudentTranscriptExamDetail] sted ON (sted.StudentTranscriptID=sth.StudentTranscriptID)" +
                    " WHERE s.IsActive=1 AND s.StudentID=" + studentID + " )t3  ON (t3.StudentID=t1.StudentID) " +


                    "LEFT OUTER JOIN (SELECT s.StudentID, dbo.GetWeightedExamTotalScore(s.StudentID," + pyT3E1 + "," + pyT3E1W + ") PyT3Exam1Score,dbo.GetWeightedExamTotalScore(s.StudentID," + pyT3E2 + "," + pyT3E2W + ") PyT3Exam2Score," +
                    "dbo.GetWeightedExamTotalScore(s.StudentID," + pyT3E3 + "," + pyT3E3W + ")PyT3Exam3Score FROM [Institution].[Student] s LEFT OUTER JOIN [Institution].[Class] c ON(s.ClassID=c.ClassID) LEFT OUTER JOIN [Institution].[StudentTranscriptHeader] " +
                    "sth ON(sth.StudentID=s.StudentID AND sth.DateSaved BETWEEN CONVERT(datetime,'" + GetTermStart().ToString("g") + "') AND CONVERT(datetime,'" + GetTermEnd().ToString("g") +
                    "')) LEFT OUTER JOIN [Institution].[StudentTranscriptExamDetail] sted ON (sted.StudentTranscriptID=sth.StudentTranscriptID)" +
                    " WHERE s.IsActive=1 AND s.StudentID=" + studentID + ")pyT3  ON (pyT3.StudentID=t1.StudentID)"

                    :

                    "SELECT t1.StudentTranscriptID,t1.StudentID, t1.NameOfStudent,t1.KCPEScore, t1.NameOfClass,t1.Responsibilities,t1.ClubsAndSport,t1.Boarding,t1.ClassTeacherComments, " +
                    "t1.PrincipalComments,t1.OpeningDay,t1.ClosingDay,t1.DateSaved,t1.ClassPosition,t1.OverAllPosition,t1.Exam1Score,t1.Exam2Score,t1.Exam3Score,t2.T2ClassPosition," +
                    "t2.T2OverAllPosition,t2.T2Exam1Score,t2.T2Exam2Score,t2.T2Exam3Score,t3.T3ClassPosition,t3.T3OverAllPosition,t3.T3Exam1Score,t3.T3Exam2Score,t3.T3Exam3Score," +
                    "pyT3.PyT3Exam1Score,pyT3.PyT3Exam2Score,pyt3.PyT3Exam3Score,t1.SPhoto FROM (SELECT s.StudentID, s.NameOfStudent,s.KCPEScore, c.NameOfClass,ISNULL(sth.StudentTranscriptID,0) StudentTranscriptID,Responsibilities,ClubsAndSport," +
                    " Boarding,ClassTeacherComments,PrincipalComments,OpeningDay,ClosingDay,DateSaved,(SELECT CONVERT(varchar(50),row_no)+'/'+CONVERT(varchar(50),no_of_students) " +
                    "FROM (SELECT ROW_NUMBER() OVER(ORDER BY ISNULL(dbo.GetWeightedExamTotalScore(StudentID," + t1E1 + "," + t1E1W + "),0)+ISNULL(dbo.GetWeightedExamTotalScore(StudentID," + t1E2 +
                    "," + t1E2W + "),0)+ISNULL(dbo.GetWeightedExamTotalScore(StudentID," + t1E3 + "," + t1E3W + "),0) DESC) row_no, StudentID, (SELECT COUNT(*) FROM [Institution].[Student] WHERE ClassID =" + c + " AND IsActive=1)" +
                    " no_of_students FROM [Institution].[Student] WHERE CLassID=" + c + " AND IsActive=1 group by StudentID ) x WHERE x.StudentID=s.StudentID) ClassPosition," +
                    "(SELECT CONVERT(varchar(50),row_no)+'/'+CONVERT(varchar(50),no_of_students) FROM (SELECT ROW_NUMBER() OVER(ORDER BY ISNULL(dbo.GetWeightedExamTotalScore(StudentID," + t1E1 +
                    "," + t1E1W + "),0)+ISNULL(dbo.GetWeightedExamTotalScore(StudentID," + t2E2 + "," + t1E2W + "),0)+ISNULL(dbo.GetWeightedExamTotalScore(StudentID," + t1E3 + "," + t1E3W + "),0) DESC) row_no, StudentID," +
                    "(SELECT COUNT(*) FROM [Institution].[Student] WHERE ClassID IN(" + cStr + ") AND IsActive=1) no_of_students FROM [Institution].[Student] WHERE CLassID IN (" + cStr +
                    ") AND IsActive=1 GROUP by StudentID ) x WHERE x.StudentID=s.StudentID) OverAllPosition,dbo.GetWeightedExamTotalScore(s.StudentID," + t1E1 + "," + t1E1W + ") Exam1Score,dbo.GetWeightedExamTotalScore(s.StudentID," + t1E2 +
                    "," + t1E2W + ")Exam2Score,dbo.GetWeightedExamTotalScore(s.StudentID," + t1E3 + "," + t1E3W + ")Exam3Score,SPhoto FROM [Institution].[Student] s LEFT OUTER JOIN [Institution].[Class] c ON(s.ClassID=c.ClassID)" +
                    " LEFT OUTER JOIN [Institution].[StudentTranscriptHeader] sth ON(sth.StudentID=s.StudentID " +
                    ") LEFT OUTER JOIN [Institution].[StudentTranscriptExamDetail] sted ON (sted.StudentTranscriptID=sth.StudentTranscriptID) WHERE s.IsActive=1 AND s.StudentID=" + studentID +
                    ") t1 " +


                    "LEFT OUTER JOIN (SELECT s.StudentID,(SELECT CONVERT(varchar(50),row_no)+'/'+CONVERT(varchar(50),no_of_students) FROM (SELECT ROW_NUMBER() " +
                    "OVER(ORDER BY ISNULL(dbo.GetWeightedExamTotalScore(StudentID," + t2E1 + "," + t2E1W + "),0)+ISNULL(dbo.GetWeightedExamTotalScore(StudentID," + t2E2 + "," + t2E2W + "),0)+ISNULL(dbo.GetWeightedExamTotalScore(StudentID," + t2E3 +
                    "," + t2E3W + "),0) DESC) row_no, StudentID, (SELECT COUNT(*) FROM [Institution].[Student] WHERE ClassID =" + c + " AND IsActive=1) no_of_students FROM [Institution].[Student]" +
                    " WHERE CLassID=" + c + " AND IsActive=1 group by StudentID ) x WHERE x.StudentID=s.StudentID) T2ClassPosition,(SELECT CONVERT(varchar(50),row_no)+'/'+CONVERT(varchar(50),no_of_students) " +
                    "FROM (SELECT ROW_NUMBER() OVER(ORDER BY ISNULL(dbo.GetWeightedExamTotalScore(StudentID," + t2E1 + "," + t2E1W + "),0)+ISNULL(dbo.GetWeightedExamTotalScore(StudentID," + t2E2 +
                    "," + t2E2W + "),0)+ISNULL(dbo.GetWeightedExamTotalScore(StudentID," + t2E3 + "," + t2E3W + "),0) DESC) row_no, StudentID,(SELECT COUNT(*) FROM [Institution].[Student] WHERE ClassID IN(" + cStr +
                    ") AND IsActive=1) no_of_students FROM [Institution].[Student] WHERE CLassID IN (" + cStr + ") AND IsActive=1 GROUP by StudentID ) x WHERE x.StudentID=s.StudentID)" +
                    " T2OverAllPosition,dbo.GetWeightedExamTotalScore(s.StudentID," + t2E1 + "," + t2E1W + ") T2Exam1Score,dbo.GetWeightedExamTotalScore(s.StudentID," + t2E2 + "," + t2E2W + ")T2Exam2Score,dbo.GetWeightedExamTotalScore(s.StudentID," + t2E3 + "," + t2E3W + ")T2Exam3Score " +
                    "FROM [Institution].[Student] s LEFT OUTER JOIN [Institution].[Class] c ON(s.ClassID=c.ClassID) LEFT OUTER JOIN [Institution].[StudentTranscriptHeader] " +
                    "sth ON(sth.StudentID=s.StudentID AND sth.DateSaved BETWEEN CONVERT(datetime,'" + GetTermStart().ToString("g") + "') AND CONVERT(datetime,'" + GetTermEnd().ToString("g") +
                    "')) " +
                    "LEFT OUTER JOIN [Institution].[StudentTranscriptExamDetail] sted ON (sted.StudentTranscriptID=sth.StudentTranscriptID) WHERE s.IsActive=1 AND s.StudentID=" + studentID +
                    ") t2 ON (t1.StudentID=t2.StudentID) " +


                    "LEFT OUTER JOIN (SELECT s.StudentID,(SELECT CONVERT(varchar(50),row_no)+'/'+CONVERT(varchar(50),no_of_students) FROM (SELECT ROW_NUMBER() " +
                    "OVER(ORDER BY ISNULL(dbo.GetWeightedExamTotalScore(StudentID," + t3E1 + "," + t3E1W + "),0)+ISNULL(dbo.GetWeightedExamTotalScore(StudentID," + t3E2 + "," + t3E2W + "),0)+ISNULL(dbo.GetWeightedExamTotalScore(StudentID," + t3E3 +
                    "," + t3E3W + "),0) DESC) row_no, StudentID, (SELECT COUNT(*) FROM [Institution].[Student] WHERE ClassID =" + c + " AND IsActive=1) no_of_students FROM [Institution].[Student] " +
                    "WHERE CLassID=" + c + " AND IsActive=1 group by StudentID ) x WHERE x.StudentID=s.StudentID) T3ClassPosition,(SELECT CONVERT(varchar(50),row_no)+'/'+CONVERT(varchar(50),no_of_students) " +
                    "FROM (SELECT ROW_NUMBER() OVER(ORDER BY ISNULL(dbo.GetWeightedExamTotalScore(StudentID," + t3E1 + "," + t3E1W + "),0)+ISNULL(dbo.GetWeightedExamTotalScore(StudentID," + t3E2 +
                    "," + t3E2W + "),0)+ISNULL(dbo.GetWeightedExamTotalScore(StudentID," + t3E3 + "," + t3E3W + "),0) DESC) row_no, StudentID,(SELECT COUNT(*) FROM [Institution].[Student] WHERE ClassID IN(" + cStr +
                    ") AND IsActive=1) no_of_students FROM [Institution].[Student] WHERE CLassID IN (" + cStr + ") AND IsActive=1 GROUP by StudentID ) x WHERE x.StudentID=s.StudentID) " +
                    "T3OverAllPosition,dbo.GetWeightedExamTotalScore(s.StudentID," + t3E1 + "," + t3E1W + ") T3Exam1Score,dbo.GetWeightedExamTotalScore(s.StudentID," + t3E2 + "," + t3E2W + ")T3Exam2Score,dbo.GetWeightedExamTotalScore(s.StudentID," + t3E3 + "," + t3E3W + ")T3Exam3Score " +
                    "FROM [Institution].[Student] s LEFT OUTER JOIN [Institution].[Class] c ON(s.ClassID=c.ClassID) LEFT OUTER JOIN [Institution].[StudentTranscriptHeader] " +
                    "sth ON(sth.StudentID=s.StudentID AND sth.DateSaved BETWEEN CONVERT(datetime,'" + GetTermStart().ToString("g") + "') AND CONVERT(datetime,'" + GetTermEnd().ToString("g") +
                    "')) LEFT OUTER JOIN [Institution].[StudentTranscriptExamDetail] sted ON (sted.StudentTranscriptID=sth.StudentTranscriptID)" +
                    " WHERE s.IsActive=1 AND s.StudentID=" + studentID + " )t3  ON (t3.StudentID=t1.StudentID) " +


                    "LEFT OUTER JOIN (SELECT s.StudentID, dbo.GetWeightedExamTotalScore(s.StudentID," + pyT3E1 + "," + pyT3E1W + ") PyT3Exam1Score,dbo.GetWeightedExamTotalScore(s.StudentID," + pyT3E2 + "," + pyT3E2W + ") PyT3Exam2Score," +
                    "dbo.GetWeightedExamTotalScore(s.StudentID," + pyT3E3 + "," + pyT3E3W + ")PyT3Exam3Score FROM [Institution].[Student] s LEFT OUTER JOIN [Institution].[Class] c ON(s.ClassID=c.ClassID) LEFT OUTER JOIN [Institution].[StudentTranscriptHeader] " +
                    "sth ON(sth.StudentID=s.StudentID AND sth.DateSaved BETWEEN CONVERT(datetime,'" + GetTermStart().ToString("g") + "') AND CONVERT(datetime,'" + GetTermEnd().ToString("g") +
                    "')) LEFT OUTER JOIN [Institution].[StudentTranscriptExamDetail] sted ON (sted.StudentTranscriptID=sth.StudentTranscriptID)" +
                    " WHERE s.IsActive=1 AND s.StudentID=" + studentID + ")pyT3  ON (pyT3.StudentID=t1.StudentID)";


                DataTable dt = DataAccessHelper.ExecuteNonQueryWithResultTable(selectStr);

                StudentTranscriptModel2 temp = new StudentTranscriptModel2();
                temp.StudentTranscriptID = int.Parse(dt.Rows[0][0].ToString());
                temp.StudentID = int.Parse(dt.Rows[0][1].ToString());
                temp.NameOfStudent = dt.Rows[0][2].ToString();
                temp.KCPEScore = string.IsNullOrWhiteSpace(dt.Rows[0][3].ToString()) ? 0 : int.Parse(dt.Rows[0][3].ToString());
                temp.NameOfClass = dt.Rows[0][4].ToString();

                temp.Responsibilities = dt.Rows[0][5].ToString();
                temp.ClubsAndSport = dt.Rows[0][6].ToString();
                temp.Boarding = dt.Rows[0][7].ToString();
                temp.ClassTeacherComments = dt.Rows[0][8].ToString();
                temp.PrincipalComments = dt.Rows[0][9].ToString();
                temp.OpeningDay = string.IsNullOrWhiteSpace(dt.Rows[0][10].ToString()) ? DateTime.Now : DateTime.Parse(dt.Rows[0][10].ToString());
                temp.ClosingDay = string.IsNullOrWhiteSpace(dt.Rows[0][11].ToString()) ? DateTime.Now : DateTime.Parse(dt.Rows[0][11].ToString());

                temp.DateSaved = string.IsNullOrWhiteSpace(dt.Rows[0][12].ToString()) ? DateTime.Now : DateTime.Parse(dt.Rows[0][12].ToString());

                temp.Term1Pos = dt.Rows[0][13].ToString();
                temp.Term1OverallPos = dt.Rows[0][14].ToString();

                switch (currentTerm)
                {
                    case 1:
                        temp.Term1Entries = GetTranscriptEntries(studentID, exams);
                        temp.Term2Entries = GetTranscriptEntries(studentID, s.Where(o => GetTerm(o.ExamDateTime) == 2));
                        temp.Term3Entries = GetTranscriptEntries(studentID, s.Where(o => GetTerm(o.ExamDateTime) == 3));
                        temp.PrevYearEntries = GetTranscriptEntries(studentID, s.Where(o => GetTerm(o.ExamDateTime) == -1));
                        break;
                    case 2:
                        temp.Term2Entries = GetTranscriptEntries(studentID, exams);
                        temp.Term1Entries = GetTranscriptEntries(studentID, s.Where(o => GetTerm(o.ExamDateTime) == 1));
                        temp.Term3Entries = GetTranscriptEntries(studentID, s.Where(o => GetTerm(o.ExamDateTime) == 3));
                        temp.PrevYearEntries = GetTranscriptEntries(studentID, s.Where(o => GetTerm(o.ExamDateTime) == -1));
                        break;
                    case 3:
                        temp.Term3Entries = GetTranscriptEntries(studentID, exams);
                        temp.Term1Entries = GetTranscriptEntries(studentID, s.Where(o => GetTerm(o.ExamDateTime) == 1));
                        temp.Term2Entries = GetTranscriptEntries(studentID, s.Where(o => GetTerm(o.ExamDateTime) == 2));
                        temp.PrevYearEntries = GetTranscriptEntries(studentID, s.Where(o => GetTerm(o.ExamDateTime) == -1));
                        break;

                }



                decimal? term1TotScore = (string.IsNullOrWhiteSpace(dt.Rows[0][15].ToString()) &&
                    string.IsNullOrWhiteSpace(dt.Rows[0][16].ToString()) &&
                    string.IsNullOrWhiteSpace(dt.Rows[0][17].ToString())) ? null : (decimal?)
                    ((string.IsNullOrWhiteSpace(dt.Rows[0][15].ToString()) ? 0 : decimal.Parse(dt.Rows[0][15].ToString())) +
                    (string.IsNullOrWhiteSpace(dt.Rows[0][16].ToString()) ? 0 : decimal.Parse(dt.Rows[0][16].ToString())) +
                    (string.IsNullOrWhiteSpace(dt.Rows[0][17].ToString()) ? 0 : decimal.Parse(dt.Rows[0][17].ToString())));

                temp.Term1TotalScore = term1TotScore + " of " + (100 * temp.Term1Entries.Count);

                temp.Term2Pos = dt.Rows[0][18].ToString();
                temp.Term2OverallPos = dt.Rows[0][19].ToString();

                decimal? term2TotScore = (string.IsNullOrWhiteSpace(dt.Rows[0][20].ToString()) &&
                    string.IsNullOrWhiteSpace(dt.Rows[0][21].ToString()) &&
                    string.IsNullOrWhiteSpace(dt.Rows[0][22].ToString())) ? null : (decimal?)
                    ((string.IsNullOrWhiteSpace(dt.Rows[0][20].ToString()) ? 0 : decimal.Parse(dt.Rows[0][20].ToString())) +
                    (string.IsNullOrWhiteSpace(dt.Rows[0][21].ToString()) ? 0 : decimal.Parse(dt.Rows[0][21].ToString())) +
                    (string.IsNullOrWhiteSpace(dt.Rows[0][22].ToString()) ? 0 : decimal.Parse(dt.Rows[0][22].ToString())));

                temp.Term2TotalScore = term2TotScore + " of " + (100 * temp.Term2Entries.Count);

                temp.Term3Pos = dt.Rows[0][23].ToString();
                temp.Term3OverallPos = dt.Rows[0][24].ToString();
                decimal? term3TotScore = (string.IsNullOrWhiteSpace(dt.Rows[0][25].ToString()) &&
                    string.IsNullOrWhiteSpace(dt.Rows[0][26].ToString()) &&
                    string.IsNullOrWhiteSpace(dt.Rows[0][27].ToString())) ? null : (decimal?)
                    ((string.IsNullOrWhiteSpace(dt.Rows[0][25].ToString()) ? 0 : decimal.Parse(dt.Rows[0][25].ToString())) +
                    (string.IsNullOrWhiteSpace(dt.Rows[0][26].ToString()) ? 0 : decimal.Parse(dt.Rows[0][26].ToString())) +
                    (string.IsNullOrWhiteSpace(dt.Rows[0][27].ToString()) ? 0 : decimal.Parse(dt.Rows[0][27].ToString())));

                temp.Term3TotalScore = term3TotScore + " of " + (100 * temp.Term3Entries.Count);

                decimal? prevYearTotScore = (string.IsNullOrWhiteSpace(dt.Rows[0][28].ToString()) &&
                    string.IsNullOrWhiteSpace(dt.Rows[0][29].ToString()) &&
                    string.IsNullOrWhiteSpace(dt.Rows[0][30].ToString())) ? null : (decimal?)
                    ((string.IsNullOrWhiteSpace(dt.Rows[0][28].ToString()) ? 0 : decimal.Parse(dt.Rows[0][28].ToString())) +
                    (string.IsNullOrWhiteSpace(dt.Rows[0][29].ToString()) ? 0 : decimal.Parse(dt.Rows[0][29].ToString())) +
                    (string.IsNullOrWhiteSpace(dt.Rows[0][30].ToString()) ? 0 : decimal.Parse(dt.Rows[0][30].ToString())));

                temp.SPhoto = (byte[])dt.Rows[0][31];

                decimal term1TotPoints = GetTranscriptTotPoints(temp.Term1Entries);
                decimal term2TotPoints = GetTranscriptTotPoints(temp.Term2Entries);
                decimal term3TotPoints = GetTranscriptTotPoints(temp.Term3Entries);

                temp.Term1TotalPoints = term1TotPoints + " of " + temp.Term1Entries.Count * 12;
                temp.Term2TotalPoints = term2TotPoints + " of " + temp.Term2Entries.Count * 12;
                temp.Term3TotalPoints = term3TotPoints + " of " + temp.Term3Entries.Count * 12;

                temp.PrevYearAvgPoints = prevYearTotScore.HasValue ? GetTranscriptAvgPoints(temp.PrevYearEntries) : 0;
                temp.Term1AvgPts = term1TotScore.HasValue ? GetTranscriptAvgPoints(temp.Term1Entries) : 0;
                temp.Term2AvgPts = term2TotScore.HasValue ? GetTranscriptAvgPoints(temp.Term2Entries) : 0;
                temp.Term3AvgPts = term3TotScore.HasValue ? GetTranscriptAvgPoints(temp.Term3Entries) : 0;

                temp.Term1PtsChange = temp.Term1AvgPts - temp.PrevYearAvgPoints;
                temp.Term2PtsChange = temp.Term2AvgPts - temp.Term1AvgPts;
                temp.Term3PtsChange = temp.Term3AvgPts - temp.Term2AvgPts;

                temp.Term1Grade = temp.Term1AvgPts > 0 ? CalculateGradeFromPoints(temp.Term1AvgPts) : "E";
                temp.Term2Grade = temp.Term2AvgPts > 0 ? CalculateGradeFromPoints(temp.Term2AvgPts) : "E";
                temp.Term3Grade = temp.Term3AvgPts > 0 ? CalculateGradeFromPoints(temp.Term3AvgPts) : "E";

                temp.Term1MeanScore = temp.Term1Entries.Count > 0 && term1TotScore.HasValue ? (term1TotScore.Value / (temp.Term1Entries.Count)) : 0;
                temp.Term2MeanScore = temp.Term2Entries.Count > 0 && term2TotScore.HasValue ? (term2TotScore.Value / (temp.Term2Entries.Count)) : 0;
                temp.Term3MeanScore = temp.Term3Entries.Count > 0 && term3TotScore.HasValue ? (term3TotScore.Value / (temp.Term3Entries.Count)) : 0;

                switch (currentTerm)
                {
                    case 1:
                        temp.Entries = temp.Term1Entries;
                        temp.TotalMarks = term1TotScore.HasValue ? term1TotScore.Value : 0;
                        temp.Points = temp.Term1AvgPts;
                        temp.MeanGrade = temp.Term1Grade;
                        temp.MeanScore = temp.Term1MeanScore;
                        temp.ClassPosition = temp.Term1Pos;
                        temp.OverAllPosition = temp.Term1OverallPos;
                        break;
                    case 2:
                        temp.Entries = temp.Term2Entries;
                        temp.TotalMarks = term2TotScore.HasValue ? term2TotScore.Value : 0;
                        temp.Points = temp.Term2AvgPts;
                        temp.MeanGrade = temp.Term2Grade;
                        temp.MeanScore = temp.Term2MeanScore;
                        temp.ClassPosition = temp.Term2Pos;
                        temp.OverAllPosition = temp.Term2OverallPos;
                        break;
                    case 3:
                        temp.Entries = temp.Term3Entries;
                        temp.TotalMarks = term3TotScore.HasValue ? term3TotScore.Value : 0;
                        temp.Points = temp.Term3AvgPts;
                        temp.MeanGrade = temp.Term3Grade;
                        temp.MeanScore = temp.Term3MeanScore;
                        temp.ClassPosition = temp.Term3Pos;
                        temp.OverAllPosition = temp.Term3OverallPos;
                        break;
                }

                return temp;
            });
        }

        internal static decimal GetTranscriptAvgPoints(ObservableCollection<StudentExamResultEntryModel> entries)
        {
            decimal d = 0m;
            foreach (StudentExamResultEntryModel current in entries)
            {
                d += current.Points;
            }
            int count = entries.Count;
            decimal result;
            if (d == 0m || entries.Count == 0)
            {
                result = 1m;
            }
            else
            {
                result = d / entries.Count;
            }
            return result;
        }

        internal static decimal GetTranscriptTotPoints(ObservableCollection<StudentExamResultEntryModel> entries)
        {
            decimal num = 0m;
            foreach (StudentExamResultEntryModel current in entries)
            {
                num += current.Points;
            }
            decimal result;
            if (num == 0m || entries.Count == 0)
            {
                result = 1m;
            }
            else
            {
                result = num;
            }
            return result;
        }

        public static Task<bool> SaveNewBookAsync(BookModel book)
        {
            return Task.Run<bool>(delegate
            {
                string commandText = string.Concat(new object[]
                {
                    "INSERT INTO [Institution].[Book] (Name,ISBN,Author,Price,Publisher,SPhoto) VALUES('",
                    book.Title,
                    "','",
                    book.ISBN,
                    "','",
                    book.Author,
                    "',",
                    book.Price,
                    ",'",
                    book.Publisher,
                    "',@Photo)"
                });
                return DataAccessHelper.ExecuteNonQueryWithParameters(commandText, new ObservableCollection<SqlParameter>
                {
                    new SqlParameter("@Photo", book.SPhoto)
                });
            });
        }

        public static Task<int> GetLastPaymentIDAsync(int studentID, DateTime datePaid)
        {
            return Task.Run<int>(delegate
            {
                string commandText = string.Concat(new object[]
                {
                    "SELECT FeesPaymentID FROM [Institution].[FeesPayment] WHERE StudentID=",
                    studentID,
                    " AND DatePaid='",
                    datePaid.ToString("g"),
                    "'"
                });
                int result;
                int.TryParse(DataAccessHelper.ExecuteScalar(commandText), out result);
                return result;
            });
        }

        public static Task<bool> SaveNewPaymentVoucher(PaymentVoucherModel newVoucher)
        {
            return Task.Run<bool>(delegate
            {
                ObservableCollection<SqlParameter> observableCollection = new ObservableCollection<SqlParameter>();
                string text = "BEGIN TRANSACTION\r\ndeclare @id int; SET @id = [dbo].GetNewID('Institution.PayoutHeader')\r\nINSERT INTO [Institution].[PayoutHeader] (PayoutID,Payee,Description,Address,TotalPaid) VALUES (@id,@p1,@p2,@p3,@p4)\r\n";
                int num = 4;
                foreach (PaymentVoucherEntryModel current in newVoucher.Entries)
                {
                    num++;
                    string text2 = "@pd" + num;
                    string text3 = "@ps" + num;
                    string text4 = "@pa" + num;
                    observableCollection.Add(new SqlParameter(text2, current.Description));
                    observableCollection.Add(new SqlParameter(text3, current.DatePaid.ToString("g")));
                    observableCollection.Add(new SqlParameter(text4, current.Amount));
                    string text5 = text;
                    text = string.Concat(new string[]
                    {
                        text5,
                        "INSERT INTO [Institution].[PayoutDetail] (PayoutID,Description,DatePaid,Amount) VALUES(@id,",
                        text2,
                        ",",
                        text3,
                        ",",
                        text4,
                        ")\r\n"
                    });
                }
                text += " COMMIT";
                observableCollection.Add(new SqlParameter("@p1", newVoucher.NameOfPayee));
                observableCollection.Add(new SqlParameter("@p2", newVoucher.Description));
                observableCollection.Add(new SqlParameter("@p3", newVoucher.Address));
                observableCollection.Add(new SqlParameter("@p4", newVoucher.Total));
                return DataAccessHelper.ExecuteNonQueryWithParameters(text, observableCollection);
            });
        }

        public static Task<ObservableCollection<PaymentVoucherModel>> GetAllPaymentVouchersAsync()
        {
            return Task.Run<ObservableCollection<PaymentVoucherModel>>(delegate
            {
                ObservableCollection<PaymentVoucherModel> observableCollection = new ObservableCollection<PaymentVoucherModel>();
                string commandText = "SELECT PayoutID,Payee,Address,TotalPaid FROM [Institution].[PayoutHeader]";
                DataTable dataTable = DataAccessHelper.ExecuteNonQueryWithResultTable(commandText);
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    PaymentVoucherModel paymentVoucherModel = new PaymentVoucherModel();
                    paymentVoucherModel.PaymentVoucherID = int.Parse(dataRow[0].ToString());
                    paymentVoucherModel.NameOfPayee = dataRow[1].ToString();
                    paymentVoucherModel.Address = dataRow[2].ToString();
                    paymentVoucherModel.Total = decimal.Parse(dataRow[3].ToString());
                    paymentVoucherModel.Entries = DataAccess.GetPaymentVoucherEntries(paymentVoucherModel.PaymentVoucherID);
                    observableCollection.Add(paymentVoucherModel);
                }
                return observableCollection;
            });
        }

        public static Task<ObservableCollection<PaymentVoucherModel>> GetPaymentVouchersAsync(bool includeDetails,DateTime? from, DateTime? to)
        {
            return Task.Run<ObservableCollection<PaymentVoucherModel>>(delegate
            {
                ObservableCollection<PaymentVoucherModel> observableCollection = new ObservableCollection<PaymentVoucherModel>();
                string commandText = "SELECT PayoutID,Payee,Address,TotalPaid,ISNULL(DatePaid,[ModifiedDate]) FROM [Institution].[PayoutHeader]";
                if (from.HasValue && to.HasValue)
                    commandText += " WHERE ISNULL(DatePaid,[ModifiedDate]) BETWEEN CONVERT(datetime,'" +
                        from.Value.Day.ToString() + "-" + from.Value.Month.ToString() + "-" + from.Value.Year.ToString() +
                        " 00:00:00.000') AND convert(datetime,'" + to.Value.Day.ToString() + "-" + to.Value.Month.ToString() +
                        "-" + to.Value.Year.ToString() + " 23:59:59.998')\r\n";
                DataTable dataTable = DataAccessHelper.ExecuteNonQueryWithResultTable(commandText);
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    PaymentVoucherModel paymentVoucherModel = new PaymentVoucherModel();
                    paymentVoucherModel.PaymentVoucherID = int.Parse(dataRow[0].ToString());
                    paymentVoucherModel.NameOfPayee = dataRow[1].ToString();
                    paymentVoucherModel.Address = dataRow[2].ToString();
                    paymentVoucherModel.Total = decimal.Parse(dataRow[3].ToString());
                    paymentVoucherModel.DatePaid = DateTime.Parse(dataRow[4].ToString());
                    if (includeDetails)
                    paymentVoucherModel.Entries = DataAccess.GetPaymentVoucherEntries(paymentVoucherModel.PaymentVoucherID);
                    observableCollection.Add(paymentVoucherModel);
                }
                return observableCollection;
            });
        }


        private static ObservableCollection<PaymentVoucherEntryModel> GetPaymentVoucherEntries(int paymentVoucherID)
        {
            ObservableCollection<PaymentVoucherEntryModel> observableCollection = new ObservableCollection<PaymentVoucherEntryModel>();
            string commandText = "SELECT Description,DatePaid,Amount FROM [Institution].[PayoutDetail]";
            DataTable dataTable = DataAccessHelper.ExecuteNonQueryWithResultTable(commandText);
            foreach (DataRow dataRow in dataTable.Rows)
            {
                observableCollection.Add(new PaymentVoucherEntryModel
                {
                    Description = dataRow[0].ToString(),
                    DatePaid = DateTime.Parse(dataRow[1].ToString()),
                    Amount = decimal.Parse(dataRow[2].ToString())
                });
            }
            return observableCollection;
        }

        private static ObservableCollection<AggregateResultEntryModel> GetAggregateResultEntries(ObservableCollection<ClassModel> classes, ExamModel selectedExam)
        {
            ObservableCollection<AggregateResultEntryModel> observableCollection = new ObservableCollection<AggregateResultEntryModel>();
            string commandText = "SELECT sub.SubjectID,sub.NameOfSubject,ROUND(AVG(erd.Score),4) FROM [Institution].[ExamDetail] ed INNER JOIN [Institution].[StudentSubjectSelectionDetail] sssd on (sssd.SubjectID = ed.SubjectID) LEFT OUTER JOIN [Institution].[ExamHeader] eh ON (eh.ExamID=ed.ExamID) LEFT OUTER JOIN [Institution].[ExamResultHeader] erh ON (erh.ExamID=eh.ExamID) INNER JOIN [Institution].[StudentSubjectSelectionHeader] sssh on (sssh.StudentID = erh.StudentID AND sssd.StudentSubjectSelectionID= sssh.StudentSubjectSelectionID) INNER JOIN [Institution].[ExamResultDetail] erd ON (sssd.SubjectID=erd.SubjectID AND erd.ExamResultID=erh.ExamResultID) LEFT OUTER JOIN [Institution].[Subject] sub ON(sssd.SubjectID=sub.SubjectID) LEFT OUTER JOIN [Institution].[Student] s ON(erh.StudentID=s.StudentID) WHERE sssh.IsActive=1 AND erh.IsActive=1  AND erh.ExamID=" + selectedExam.ExamID + " GROUP BY sub.SubjectID,sub.NameOfSubject ORDER BY ROUND(AVG(erd.Score),4) DESC";
            DataTable dataTable = DataAccessHelper.ExecuteNonQueryWithResultTable(commandText);
            foreach (DataRow dataRow in dataTable.Rows)
            {
                AggregateResultEntryModel aggregateResultEntryModel = new AggregateResultEntryModel();
                aggregateResultEntryModel.NameOfSubject = dataRow[1].ToString();
                aggregateResultEntryModel.MeanScore = decimal.Parse(dataRow[2].ToString());
                aggregateResultEntryModel.MeanGrade = DataAccess.CalculateGrade(aggregateResultEntryModel.MeanScore * 100m / selectedExam.OutOf);
                aggregateResultEntryModel.Points = DataAccess.CalculatePoints(aggregateResultEntryModel.MeanGrade);
                observableCollection.Add(aggregateResultEntryModel);
            }
            return observableCollection;
        }

        private static ObservableCollection<AggregateResultEntryModel> GetAggregateResultEntries(ClassModel selectedClass, ExamModel selectedExam)
        {
            ObservableCollection<AggregateResultEntryModel> observableCollection = new ObservableCollection<AggregateResultEntryModel>();
            string commandText = string.Concat(new object[]
            {
                "SELECT sub.SubjectID,sub.NameOfSubject,ROUND(AVG(erd.Score),4) FROM [Institution].[ExamDetail] ed INNER JOIN [Institution].[StudentSubjectSelectionDetail] sssd on (sssd.SubjectID = ed.SubjectID) LEFT OUTER JOIN [Institution].[ExamHeader] eh ON (eh.ExamID=ed.ExamID) LEFT OUTER JOIN [Institution].[ExamResultHeader] erh ON (erh.ExamID=eh.ExamID) INNER JOIN [Institution].[StudentSubjectSelectionHeader] sssh on (sssh.StudentID = erh.StudentID AND sssd.StudentSubjectSelectionID= sssh.StudentSubjectSelectionID) INNER JOIN [Institution].[ExamResultDetail] erd ON (sssd.SubjectID=erd.SubjectID AND erd.ExamResultID=erh.ExamResultID) LEFT OUTER JOIN [Institution].[Subject] sub ON(sssd.SubjectID=sub.SubjectID) LEFT OUTER JOIN [Institution].[Student] s ON(erh.StudentID=s.StudentID) WHERE s.ClassID=",
                selectedClass.ClassID,
                " AND sssh.IsActive=1 AND erh.IsActive=1  AND erh.ExamID=",
                selectedExam.ExamID,
                " GROUP BY sub.SubjectID,sub.NameOfSubject ORDER BY ROUND(AVG(erd.Score),4) DESC"
            });
            DataTable dataTable = DataAccessHelper.ExecuteNonQueryWithResultTable(commandText);
            foreach (DataRow dataRow in dataTable.Rows)
            {
                AggregateResultEntryModel aggregateResultEntryModel = new AggregateResultEntryModel();
                aggregateResultEntryModel.NameOfSubject = dataRow[1].ToString();
                aggregateResultEntryModel.MeanScore = decimal.Parse(dataRow[2].ToString());
                aggregateResultEntryModel.MeanGrade = DataAccess.CalculateGrade(aggregateResultEntryModel.MeanScore * 100m / selectedExam.OutOf);
                aggregateResultEntryModel.Points = DataAccess.CalculatePoints(aggregateResultEntryModel.MeanGrade);
                observableCollection.Add(aggregateResultEntryModel);
            }
            return observableCollection;
        }

        private static ObservableCollection<AggregateResultEntryModel> GetCombinedAggregateResultEntries(ClassModel selectedClass, ObservableCollection<ExamWeightModel> exams)
        {
            ObservableCollection<AggregateResultEntryModel> temp = new ObservableCollection<AggregateResultEntryModel>();

            foreach (var e in exams)
            {
                string selectStr = "SELECT sub.SubjectID,sub.NameOfSubject,ROUND(AVG((erd.Score*" + e.Weight + "/eh.OutOf)),4) FROM [Institution].[ExamDetail] ed INNER JOIN " +
                    "[Institution].[StudentSubjectSelectionDetail] sssd on (sssd.SubjectID = ed.SubjectID) LEFT OUTER JOIN [Institution].[ExamHeader] eh " +
                    "ON (eh.ExamID=ed.ExamID) LEFT OUTER JOIN [Institution].[ExamResultHeader] erh ON (erh.ExamID=eh.ExamID) INNER JOIN " +
                    "[Institution].[StudentSubjectSelectionHeader] sssh on (sssh.StudentID = erh.StudentID AND sssd.StudentSubjectSelectionID= sssh.StudentSubjectSelectionID)" +
                    " INNER JOIN [Institution].[ExamResultDetail] erd ON (sssd.SubjectID=erd.SubjectID AND erd.ExamResultID=erh.ExamResultID) LEFT OUTER JOIN " +
                    "[Institution].[Subject] sub ON(sssd.SubjectID=sub.SubjectID) LEFT OUTER JOIN [Institution].[Student] s ON(erh.StudentID=s.StudentID) " +
                    "WHERE s.ClassID=" + selectedClass.ClassID + " AND sssh.IsActive=1 AND erh.IsActive=1  AND erh.ExamID=" + e.ExamID +
                    " GROUP BY sub.SubjectID,sub.NameOfSubject ORDER BY ROUND(AVG((erd.Score*" + e.Weight + "/eh.OutOf)),4) DESC";

                DataTable dt = DataAccessHelper.ExecuteNonQueryWithResultTable(selectStr);
                AggregateResultEntryModel cls;
                foreach (DataRow dtr in dt.Rows)
                {
                    cls = new AggregateResultEntryModel();

                    cls.NameOfSubject = dtr[1].ToString();
                    cls.MeanScore = decimal.Parse(dtr[2].ToString());
                    cls.MeanGrade = CalculateGrade(cls.MeanScore * 100 / e.Weight);
                    cls.Points = CalculatePoints(cls.MeanGrade);
                    temp.Add(cls);
                }
            }

            ObservableCollection<AggregateResultEntryModel> tempCls = new ObservableCollection<AggregateResultEntryModel>();
            for (int i = 0; i < temp.Count; i++)
            {
                if (tempCls.Any(o => o.NameOfSubject == temp[i].NameOfSubject))
                    tempCls.Where(x => x.NameOfSubject == temp[i].NameOfSubject).First().MeanScore += temp[i].MeanScore;
                else
                    tempCls.Add(temp[i]);
            }

            return tempCls;
        }

        private static ObservableCollection<AggregateResultEntryModel> GetCombinedAggregateResultEntries(ObservableCollection<ClassModel> classes, ObservableCollection<ExamWeightModel> exams)
        {
            ObservableCollection<AggregateResultEntryModel>  temp = new ObservableCollection<AggregateResultEntryModel>();
            foreach (ExamWeightModel current in exams)
            {
                string commandText = string.Concat(new object[]
                {
                    "SELECT sub.SubjectID,sub.NameOfSubject,ROUND(AVG((erd.Score*",
                    current.Weight,
                    "/eh.OutOf)),4) FROM [Institution].[ExamDetail] ed INNER JOIN [Institution].[StudentSubjectSelectionDetail] sssd on (sssd.SubjectID = ed.SubjectID) LEFT OUTER JOIN [Institution].[ExamHeader] eh ON (eh.ExamID=ed.ExamID) LEFT OUTER JOIN [Institution].[ExamResultHeader] erh ON (erh.ExamID=eh.ExamID) INNER JOIN [Institution].[StudentSubjectSelectionHeader] sssh on (sssh.StudentID = erh.StudentID AND sssd.StudentSubjectSelectionID= sssh.StudentSubjectSelectionID) INNER JOIN [Institution].[ExamResultDetail] erd ON (sssd.SubjectID=erd.SubjectID AND erd.ExamResultID=erh.ExamResultID) LEFT OUTER JOIN [Institution].[Subject] sub ON(sssd.SubjectID=sub.SubjectID) LEFT OUTER JOIN [Institution].[Student] s ON(erh.StudentID=s.StudentID) WHERE sssh.IsActive=1 AND erh.IsActive=1  AND erh.ExamID=",
                    current.ExamID,
                    " GROUP BY sub.SubjectID,sub.NameOfSubject ORDER BY ROUND(AVG((erd.Score*",
                    current.Weight,
                    "/eh.OutOf)),4) DESC"
                });
                DataTable dataTable = DataAccessHelper.ExecuteNonQueryWithResultTable(commandText);
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    AggregateResultEntryModel aggregateResultEntryModel = new AggregateResultEntryModel();
                    aggregateResultEntryModel.NameOfSubject = dataRow[1].ToString();
                    aggregateResultEntryModel.MeanScore = decimal.Parse(dataRow[2].ToString());
                    aggregateResultEntryModel.MeanGrade = DataAccess.CalculateGrade(aggregateResultEntryModel.MeanScore * 100m / current.Weight);
                    aggregateResultEntryModel.Points = DataAccess.CalculatePoints(aggregateResultEntryModel.MeanGrade);

                    temp.Add(aggregateResultEntryModel);
                }
            }
            ObservableCollection<AggregateResultEntryModel> observableCollection = new ObservableCollection<AggregateResultEntryModel>();
            int i;
            for (i = 0; i <temp.Count; i++)
            {
                if (observableCollection.Any((AggregateResultEntryModel o) => o.NameOfSubject == temp[i].NameOfSubject))
                {
                    (from x in observableCollection
                     where x.NameOfSubject == temp[i].NameOfSubject
                     select x).First<AggregateResultEntryModel>().MeanScore += temp[i].MeanScore;
                }
                else
                {
                    observableCollection.Add(temp[i]);
                }
            }
            return observableCollection;
        }

        public static Task<AggregateResultModel> GetAggregateResultAsync(ClassModel selectedClass, ExamModel selectedExam)
        {
            return Task.Run<AggregateResultModel>(delegate
            {
                AggregateResultModel aggregateResultModel = new AggregateResultModel();
                aggregateResultModel.NameOfClass = selectedClass.NameOfClass;
                aggregateResultModel.NameOfExam = selectedExam.NameOfExam;
                string commandText = string.Concat(new object[]
                {
                    "SELECT AVG(x.[Average]) FROM (SELECT sub.SubjectID,sub.NameOfSubject,ROUND(AVG(erd.Score),4) [Average] FROM [Institution].[ExamDetail] ed INNER JOIN [Institution].[StudentSubjectSelectionDetail] sssd on (sssd.SubjectID = ed.SubjectID) LEFT OUTER JOIN [Institution].[ExamHeader] eh ON (eh.ExamID=ed.ExamID) LEFT OUTER JOIN [Institution].[ExamResultHeader] erh ON (erh.ExamID=eh.ExamID) INNER JOIN [Institution].[StudentSubjectSelectionHeader] sssh on (sssh.StudentID = erh.StudentID AND sssd.StudentSubjectSelectionID= sssh.StudentSubjectSelectionID) INNER JOIN [Institution].[ExamResultDetail] erd ON (sssd.SubjectID=erd.SubjectID AND erd.ExamResultID=erh.ExamResultID) LEFT OUTER JOIN [Institution].[Subject] sub ON(sssd.SubjectID=sub.SubjectID) LEFT OUTER JOIN [Institution].[Student] s ON(erh.StudentID=s.StudentID) WHERE s.ClassID=",
                    selectedClass.ClassID,
                    " AND sssh.IsActive=1 AND erh.IsActive=1  AND erh.ExamID=",
                    selectedExam.ExamID,
                    " GROUP BY sub.SubjectID,sub.NameOfSubject) x"
                });
                aggregateResultModel.MeanScore = decimal.Parse(DataAccessHelper.ExecuteScalar(commandText));
                aggregateResultModel.MeanGrade = DataAccess.CalculateGrade(aggregateResultModel.MeanScore * 100m / selectedExam.OutOf);
                aggregateResultModel.Points = DataAccess.CalculatePoints(aggregateResultModel.MeanGrade);
                aggregateResultModel.Entries = GetAggregateResultEntries(selectedClass, selectedExam);
                return aggregateResultModel;
            });
        }

        public static Task<AggregateResultModel> GetAggregateResultAsync(CombinedClassModel selectedCombinedClass, ExamModel selectedExam)
        {
            return Task.Run(delegate
            {
                AggregateResultModel aggregateResultModel = new AggregateResultModel();
                aggregateResultModel.NameOfClass = selectedCombinedClass.Description;
                aggregateResultModel.NameOfExam = selectedExam.NameOfExam;
                string commandText = "SELECT AVG(x.[Average]) FROM (SELECT sub.SubjectID,sub.NameOfSubject,ROUND(AVG(erd.Score),4) [Average] FROM [Institution].[ExamDetail] ed INNER JOIN [Institution].[StudentSubjectSelectionDetail] sssd on (sssd.SubjectID = ed.SubjectID) LEFT OUTER JOIN [Institution].[ExamHeader] eh ON (eh.ExamID=ed.ExamID) LEFT OUTER JOIN [Institution].[ExamResultHeader] erh ON (erh.ExamID=eh.ExamID) INNER JOIN [Institution].[StudentSubjectSelectionHeader] sssh on (sssh.StudentID = erh.StudentID AND sssd.StudentSubjectSelectionID= sssh.StudentSubjectSelectionID) INNER JOIN [Institution].[ExamResultDetail] erd ON (sssd.SubjectID=erd.SubjectID AND erd.ExamResultID=erh.ExamResultID) LEFT OUTER JOIN [Institution].[Subject] sub ON(sssd.SubjectID=sub.SubjectID) LEFT OUTER JOIN [Institution].[Student] s ON(erh.StudentID=s.StudentID) WHERE sssh.IsActive=1 AND erh.IsActive=1  AND erh.ExamID=" + selectedExam.ExamID + " GROUP BY sub.SubjectID,sub.NameOfSubject) x";
                aggregateResultModel.MeanScore = decimal.Parse(DataAccessHelper.ExecuteScalar(commandText));
                aggregateResultModel.MeanGrade = DataAccess.CalculateGrade(aggregateResultModel.MeanScore * 100m / selectedExam.OutOf);
                aggregateResultModel.Points = DataAccess.CalculatePoints(aggregateResultModel.MeanGrade);
                aggregateResultModel.Entries = DataAccess.GetAggregateResultEntries(selectedCombinedClass.Entries, selectedExam);
                return aggregateResultModel;
            });
        }

        public static Task<AggregateResultModel> GetCombinedAggregateResultAsync(ClassModel selectedClass, ObservableCollection<ExamWeightModel> exams)
        {
            return Task.Run<AggregateResultModel>(delegate
            {
                AggregateResultModel aggregateResultModel = new AggregateResultModel();
                aggregateResultModel.NameOfClass = selectedClass.NameOfClass;
                foreach (ExamWeightModel current in exams)
                {
                    AggregateResultModel expr_36 = aggregateResultModel;
                    expr_36.NameOfExam = expr_36.NameOfExam + current.NameOfExam + ", ";
                    string commandText = string.Concat(new object[]
                    {
                        "SELECT AVG(x.[Average]) FROM (SELECT sub.SubjectID,sub.NameOfSubject,ROUND(AVG((erd.Score*",
                        current.Weight,
                        "/eh.OutOf)),4) [Average] FROM [Institution].[ExamDetail] ed INNER JOIN [Institution].[StudentSubjectSelectionDetail] sssd on (sssd.SubjectID = ed.SubjectID) LEFT OUTER JOIN [Institution].[ExamHeader] eh ON (eh.ExamID=ed.ExamID) LEFT OUTER JOIN [Institution].[ExamResultHeader] erh ON (erh.ExamID=eh.ExamID) INNER JOIN [Institution].[StudentSubjectSelectionHeader] sssh on (sssh.StudentID = erh.StudentID AND sssd.StudentSubjectSelectionID= sssh.StudentSubjectSelectionID) INNER JOIN [Institution].[ExamResultDetail] erd ON (sssd.SubjectID=erd.SubjectID AND erd.ExamResultID=erh.ExamResultID) LEFT OUTER JOIN [Institution].[Subject] sub ON(sssd.SubjectID=sub.SubjectID) LEFT OUTER JOIN [Institution].[Student] s ON(erh.StudentID=s.StudentID) WHERE s.ClassID=",
                        selectedClass.ClassID,
                        " AND sssh.IsActive=1 AND erh.IsActive=1  AND erh.ExamID=",
                        current.ExamID,
                        " GROUP BY sub.SubjectID,sub.NameOfSubject) x"
                    });
                    aggregateResultModel.MeanScore += decimal.Parse(DataAccessHelper.ExecuteScalar(commandText));
                }
                aggregateResultModel.MeanGrade = DataAccess.CalculateGrade(aggregateResultModel.MeanScore);
                aggregateResultModel.Points = DataAccess.CalculatePoints(aggregateResultModel.MeanGrade);
                aggregateResultModel.Entries = DataAccess.GetCombinedAggregateResultEntries(selectedClass, exams);
                return aggregateResultModel;
            });
        }

        public static Task<AggregateResultModel> GetCombinedAggregateResultAsync(CombinedClassModel selectedCombinedClass, ObservableCollection<ExamWeightModel> exams)
        {
            return Task.Run<AggregateResultModel>(delegate
            {
                AggregateResultModel aggregateResultModel = new AggregateResultModel();
                aggregateResultModel.NameOfClass = selectedCombinedClass.Description;
                foreach (ExamWeightModel current in exams)
                {
                    AggregateResultModel expr_36 = aggregateResultModel;
                    expr_36.NameOfExam = expr_36.NameOfExam + current.NameOfExam + ", ";
                    string commandText = string.Concat(new object[]
                    {
                        "SELECT AVG(x.[Average]) FROM (SELECT sub.SubjectID,sub.NameOfSubject,ROUND(AVG((erd.Score*",
                        current.Weight,
                        "/eh.OutOf)),4) [Average] FROM [Institution].[ExamDetail] ed INNER JOIN [Institution].[StudentSubjectSelectionDetail] sssd on (sssd.SubjectID = ed.SubjectID) LEFT OUTER JOIN [Institution].[ExamHeader] eh ON (eh.ExamID=ed.ExamID) LEFT OUTER JOIN [Institution].[ExamResultHeader] erh ON (erh.ExamID=eh.ExamID) INNER JOIN [Institution].[StudentSubjectSelectionHeader] sssh on (sssh.StudentID = erh.StudentID AND sssd.StudentSubjectSelectionID= sssh.StudentSubjectSelectionID) INNER JOIN [Institution].[ExamResultDetail] erd ON (sssd.SubjectID=erd.SubjectID AND erd.ExamResultID=erh.ExamResultID) LEFT OUTER JOIN [Institution].[Subject] sub ON(sssd.SubjectID=sub.SubjectID) LEFT OUTER JOIN [Institution].[Student] s ON(erh.StudentID=s.StudentID) WHERE sssh.IsActive=1 AND erh.IsActive=1  AND erh.ExamID=",
                        current.ExamID,
                        " GROUP BY sub.SubjectID,sub.NameOfSubject) x"
                    });
                    aggregateResultModel.MeanScore += decimal.Parse(DataAccessHelper.ExecuteScalar(commandText));
                }
                aggregateResultModel.MeanGrade = DataAccess.CalculateGrade(aggregateResultModel.MeanScore);
                aggregateResultModel.Points = DataAccess.CalculatePoints(aggregateResultModel.MeanGrade);
                aggregateResultModel.Entries = DataAccess.GetCombinedAggregateResultEntries(selectedCombinedClass.Entries, exams);
                return aggregateResultModel;
            });
        }

        public static Task<ObservableCollection<BookModel>> GetAllBooksAsync()
        {
            return Task.Run<ObservableCollection<BookModel>>(delegate
            {
                ObservableCollection<BookModel> observableCollection = new ObservableCollection<BookModel>();
                string commandText = "SELECT ISBN, Name,Author,Publisher,SPhoto,BookID,Price FROM [Institution].[Book]";
                DataTable dataTable = DataAccessHelper.ExecuteNonQueryWithResultTable(commandText);
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    observableCollection.Add(new BookModel
                    {
                        ISBN = dataRow[0].ToString(),
                        Title = dataRow[1].ToString(),
                        Author = dataRow[2].ToString(),
                        Publisher = dataRow[3].ToString(),
                        SPhoto = (dataRow[4] != null && !(dataRow[4] is DBNull)) ? ((byte[])dataRow[4]) : new byte[0],
                        BookID = int.Parse(dataRow[5].ToString()),
                        Price = decimal.Parse(dataRow[6].ToString())
                    });
                }
                return observableCollection;
            });
        }

        public static Task<ObservableCollection<DonorListModel>> GetAllDonorsAsync()
        {
            return Task.Run<ObservableCollection<DonorListModel>>(delegate
            {
                ObservableCollection<DonorListModel> observableCollection = new ObservableCollection<DonorListModel>();
                string commandText = "SELECT d.DonorID, d.NameOfDonor,d.PhoneNo, ISNULL(SUM(CONVERT(decimal(18,0),dn.AmountDonated)),0) FROM [Institution].[Donor] d LEFT OUTER JOIN [Institution].[Donation] dn ON(d.DonorID=dn.DonorID) GROUP BY d.DonorID,d.NameOfDonor,d.PhoneNo";
                DataTable dataTable = DataAccessHelper.ExecuteNonQueryWithResultTable(commandText);
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    observableCollection.Add(new DonorListModel
                    {
                        DonorID = int.Parse(dataRow[0].ToString()),
                        NameOfDonor = dataRow[1].ToString(),
                        PhoneNo = dataRow[2].ToString(),
                        TotalDonations = decimal.Parse(dataRow[3].ToString())
                    });
                }
                return observableCollection;
            });
        }

        public static Task<bool> SaveNewBookIssueAsync(BookIssueModel bim)
        {
            return Task.Run<bool>(delegate
            {
                string text = string.Concat(new object[]
                {
                    "BEGIN TRANSACTION\r\ndeclare @id int; SET @id = [dbo].GetNewID('Institution.BookIssueHeader') INSERT INTO [Institution].[BookIssueHeader] (BookIssueID,StudentID,DateIssued) VALUES (@id,",
                    bim.StudentID,
                    ",'",
                    bim.DateIssued.ToString("g"),
                    "')\r\n"
                });
                foreach (BookModel current in bim.Entries)
                {
                    object obj = text;
                    text = string.Concat(new object[]
                    {
                        obj,
                        "INSERT INTO [Institution].[BookIssueDetail] (BookIssueID,BookID) VALUES (@id,",
                        current.BookID,
                        ")\r\n"
                    });
                }
                text += " COMMIT";
                return DataAccessHelper.ExecuteNonQuery(text);
            });
        }

        public static Task<ObservableCollection<BookModel>> GetUnreturnedBooksAsync(int studenID)
        {
            return Task.Run<ObservableCollection<BookModel>>(delegate
            {
                ObservableCollection<BookModel> observableCollection = new ObservableCollection<BookModel>();
                string commandText = string.Concat(new object[]
                {
                    "SELECT x.BookID,b.ISBN,b.Name,b.Author,b.Publisher,b.SPhoto,b.Price FROM ((SELECT bid.BookID FROM [Institution].[BookIssueDetail] bid INNER JOIN [Institution].[BookIssueHeader] bih ON(bid.BookIssueID=bih.BookIssueID) WHERE bih.StudentID=",
                    studenID,
                    " AND NOT EXISTS(SELECT brd.BookID FROM [Institution].[BookReturnDetail] brd INNER JOIN [Institution].[BookReturnHeader] brh ON(brd.BookReturnID=brh.BookReturnID) WHERE brh.DateReturned>bih.DateIssued AND brd.BookID=bid.BookID AND brh.StudentID=",
                    studenID,
                    ")) x LEFT OUTER JOIN [Institution].[Book] b ON (x.BookID=b.BookID))"
                });
                DataTable dataTable = DataAccessHelper.ExecuteNonQueryWithResultTable(commandText);
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    observableCollection.Add(new BookModel
                    {
                        BookID = int.Parse(dataRow[0].ToString()),
                        ISBN = dataRow[1].ToString(),
                        Title = dataRow[2].ToString(),
                        Author = dataRow[3].ToString(),
                        Publisher = dataRow[4].ToString(),
                        SPhoto = (dataRow[5] != null && !(dataRow[5] is DBNull)) ? ((byte[])dataRow[5]) : new byte[0],
                        Price = decimal.Parse(dataRow[6].ToString())
                    });
                }
                return observableCollection;
            });
        }

        public static Task<ObservableCollection<UnreturnedBookModel>> GetUnreturnedBooksAsync()
        {
            return Task.Run<ObservableCollection<UnreturnedBookModel>>(delegate
            {
                ObservableCollection<UnreturnedBookModel> observableCollection = new ObservableCollection<UnreturnedBookModel>();
                string commandText = "SELECT x.BookID,b.ISBN,b.Name,b.Author,b.Publisher,b.SPhoto,b.Price,dbo.GetUnreturnedCopies(x.BookID) FROM ((SELECT bid.BookID FROM [Institution].[BookIssueDetail] bid INNER JOIN [Institution].[BookIssueHeader] bih ON(bid.BookIssueID=bih.BookIssueID) WHERE NOT EXISTS(SELECT brd.BookID FROM [Institution].[BookReturnDetail] brd INNER JOIN [Institution].[BookReturnHeader] brh ON(brd.BookReturnID=brh.BookReturnID) WHERE brh.DateReturned>bih.DateIssued AND brd.BookID=bid.BookID)) x LEFT OUTER JOIN [Institution].[Book] b ON (x.BookID=b.BookID))";
                DataTable dataTable = DataAccessHelper.ExecuteNonQueryWithResultTable(commandText);
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    observableCollection.Add(new UnreturnedBookModel
                    {
                        BookID = int.Parse(dataRow[0].ToString()),
                        ISBN = dataRow[1].ToString(),
                        Title = dataRow[2].ToString(),
                        Author = dataRow[3].ToString(),
                        Publisher = dataRow[4].ToString(),
                        SPhoto = (dataRow[5] != null && !(dataRow[5] is DBNull)) ? ((byte[])dataRow[5]) : new byte[0],
                        Price = decimal.Parse(dataRow[6].ToString()),
                        UnreturnedCopies = decimal.Parse(dataRow[7].ToString())
                    });
                }
                return observableCollection;
            });
        }

        public static Task<bool> SaveNewBookReturnAsync(BookReturnModel bim)
        {
            return Task.Run<bool>(delegate
            {
                string text = string.Concat(new object[]
                {
                    "BEGIN TRANSACTION\r\ndeclare @id int; SET @id = [dbo].GetNewID('Institution.BookReturnHeader') INSERT INTO [Institution].[BookReturnHeader] (BookReturnID,StudentID,DateReturned) VALUES (@id,",
                    bim.StudentID,
                    ",'",
                    bim.DateReturned.ToString("g"),
                    "')\r\n"
                });
                foreach (BookModel current in bim.Entries)
                {
                    object obj = text;
                    text = string.Concat(new object[]
                    {
                        obj,
                        "INSERT INTO [Institution].[BookReturnDetail] (BookReturnID,BookID) VALUES (@id,",
                        current.BookID,
                        ")\r\n"
                    });
                }
                text += " COMMIT";
                return DataAccessHelper.ExecuteNonQuery(text);
            });
        }

        internal static BookModel GetBook(int bookID)
        {
            string commandText = "SELECT BookID,ISBN,Name,Author,Publisher,SPhoto,b.Price FROM [Institution].[Book] WHERE BookID=" + bookID;
            DataTable dataTable = DataAccessHelper.ExecuteNonQueryWithResultTable(commandText);
            BookModel result;
            if (dataTable.Rows.Count == 0)
            {
                result = new BookModel();
            }
            else
            {
                BookModel bookModel = new BookModel();
                DataRow dataRow = dataTable.Rows[0];
                result = new BookModel
                {
                    BookID = int.Parse(dataRow[0].ToString()),
                    ISBN = dataRow[1].ToString(),
                    Title = dataRow[2].ToString(),
                    Author = dataRow[3].ToString(),
                    Publisher = dataRow[4].ToString(),
                    SPhoto = (dataRow[5] != null && !(dataRow[5] is DBNull)) ? ((byte[])dataRow[5]) : new byte[0],
                    Price = decimal.Parse(dataRow[6].ToString())
                };
            }
            return result;
        }

        public static Task<bool> UpdateBookAsync(BookSelectModel book)
        {
            return Task.Run<bool>(delegate
            {
                string commandText = string.Concat(new object[]
                {
                    "UPDATE [Institution].[Book] SET ISBN='",
                    book.ISBN,
                    "', Name='",
                    book.Title,
                    "', Author='",
                    book.Author,
                    "', Publisher='",
                    book.Publisher,
                    "', Price='",
                    book.Price,
                    "', SPhoto=@photo WHERE BookID=",
                    book.BookID
                });
                return DataAccessHelper.ExecuteNonQueryWithParameters(commandText, new ObservableCollection<SqlParameter>
                {
                    new SqlParameter("@photo", book.SPhoto)
                });
            });
        }

        public static Task<bool> AssignNewStudentNewClass(int studentID, int newClassID)
        {
            return Task.Run<bool>(delegate
            {
                string commandText = string.Concat(new object[]
                {
                    "IF NOT EXISTS (SELECT * FROM [Institution].[CurrentClass] WHERE DATEPART(year,StartDateTime)=DATEPART(year,"+(DateTime.Now.Year)+"))\r\n ",
                    "INSERT INTO [Institution].[CurrentClass] (StudentID,ClassID,IsActive,StartDateTime,EndDateTime) VALUES(",
                    studentID,
                    ",",
                    newClassID,
                    ",1,'01-01-",(DateTime.Now.Year)," 00:00:00','31-12-",(DateTime.Now.Year)," 00:00:00')\r\n"
                });
                return DataAccessHelper.ExecuteNonQuery(commandText);
            });
        }

        public static Task<bool> AssignStudentNewClass(int studentID, int newClassID)
        {
            return Task.Run<bool>(delegate
            {
                string commandText = string.Concat(new object[]
                {
                    "IF NOT EXISTS (SELECT * FROM [Institution].[CurrentClass] WHERE DATEPART(year,StartDateTime)=DATEPART(year,"+(DateTime.Now.Year+1)+"))\r\n ",
                    "INSERT INTO [Institution].[CurrentClass] (StudentID,ClassID,IsActive,StartDateTime,EndDateTime) VALUES(",
                    studentID,
                    ",",
                    newClassID,
                    ",1,'01-01-",(DateTime.Now.Year+1)," 00:00:00','31-12-",(DateTime.Now.Year+1)," 00:00:00')\r\n"
                });
                return DataAccessHelper.ExecuteNonQuery(commandText);
            });
        }

        public static Task<bool> AssignClassNewClass(int classID, int newClassID)
        {
            return Task.Run<bool>(delegate
            {
                string selectString = "SELECT DISTINCT s.StudentID FROM [Institution].[Student]s LEFT OUTER JOIN [Institution].[CurrentClass] cc ON (s.StudentID=cc.StudentID)" +
                " WHERE s.ClassID =" + classID;
                ObservableCollection<string> observableCollection = DataAccessHelper.CopyFromDBtoObservableCollection(selectString);
                string text = "";
                foreach (string current in observableCollection)
                {
                    text +=
                    "IF NOT EXISTS (SELECT * FROM [Institution].[CurrentClass] WHERE DATEPART(year,StartDateTime)=DATEPART(year,"+(DateTime.Now.Year+1)+") AND StudentID=" + current + ")\r\n " +
                    "INSERT INTO [Institution].[CurrentClass] (StudentID,ClassID,IsActive,StartDateTime,EndDateTime) VALUES(" +
                    current + "," + newClassID + ",1,'01-01-" + (DateTime.Now.Year + 1) + " 00:00:00','31-12-" + (DateTime.Now.Year + 1) + " 00:00:00')\r\n";
                }
                return DataAccessHelper.ExecuteNonQuery(text);
            });
        }

        internal static StudentBaseModel GetBedNoUser(string bedNo)
        {
            StudentBaseModel studentBaseModel = new StudentBaseModel();
            try
            {
                string commandText = "SELECT StudentID, FirstName +' '+LastName+' '+MiddleName FROM [Institution].[Student] WHERE BedNo='" + bedNo + "'";
                DataTable dataTable = DataAccessHelper.ExecuteNonQueryWithResultTable(commandText);
                if (dataTable.Rows.Count != 0)
                {
                    studentBaseModel.StudentID = int.Parse(dataTable.Rows[0][0].ToString());
                    studentBaseModel.NameOfStudent = dataTable.Rows[0][1].ToString();
                }
            }
            catch
            {
            }
            return studentBaseModel;
        }

        public static Task<LeavingCertificateModel> GetStudentLeavingCert(StudentBaseModel student)
        {
            return Task.Run<LeavingCertificateModel>(delegate
            {
                LeavingCertificateModel leavingCertificateModel = new LeavingCertificateModel();
                try
                {
                    string commandText = "SELECT DateOfIssue,DateOfBirth,DateOfAdmission,DateOfLeaving,Nationality,ClassEntered,ClassLeft,Remarks FROM [Institution].[LeavingCertificate] WHERE StudentID=" + student.StudentID;
                    DataTable dataTable = DataAccessHelper.ExecuteNonQueryWithResultTable(commandText);
                    if (dataTable.Rows.Count != 0)
                    {
                        leavingCertificateModel.StudentID = student.StudentID;
                        leavingCertificateModel.NameOfStudent = student.NameOfStudent;
                        leavingCertificateModel.DateOfIssue = DateTime.Parse(dataTable.Rows[0][0].ToString());
                        leavingCertificateModel.DateOfBirth = DateTime.Parse(dataTable.Rows[0][1].ToString());
                        leavingCertificateModel.DateOfAdmission = DateTime.Parse(dataTable.Rows[0][2].ToString());
                        leavingCertificateModel.DateOfLeaving = DateTime.Parse(dataTable.Rows[0][3].ToString());
                        leavingCertificateModel.Nationality = dataTable.Rows[0][4].ToString();
                        leavingCertificateModel.ClassEntered = dataTable.Rows[0][5].ToString();
                        leavingCertificateModel.ClassLeft = dataTable.Rows[0][6].ToString();
                        leavingCertificateModel.Remarks = dataTable.Rows[0][7].ToString();
                    }
                }
                catch
                {
                }
                return leavingCertificateModel;
            });
        }

        public static Task<ObservableCollection<StudentTranscriptModel>> GetClassTranscriptsAsync(int classID, IEnumerable<ExamWeightModel> exams, IEnumerable<ClassModel> classes)
        {
            return Task.Run<ObservableCollection<StudentTranscriptModel>>(delegate
            {
                ObservableCollection<StudentTranscriptModel> observableCollection = new ObservableCollection<StudentTranscriptModel>();
                int num = 0;
                int num2 = 0;
                int num3 = 0;
                if (exams.Any((ExamWeightModel o) => o.Index == 1))
                {
                    num = (from o in exams
                           where o.Index == 1
                           select o).ElementAt(0).ExamID;
                }
                if (exams.Any((ExamWeightModel o) => o.Index == 2))
                {
                    num2 = (from o in exams
                            where o.Index == 2
                            select o).ElementAt(0).ExamID;
                }
                if (exams.Any((ExamWeightModel o) => o.Index == 3))
                {
                    num3 = (from o in exams
                            where o.Index == 3
                            select o).ElementAt(0).ExamID;
                }
                string text = "0,";
                foreach (ClassModel current in classes)
                {
                    text = text + current.ClassID + ",";
                }
                text = text.Remove(text.Length - 1);
                string text2 = "0,";
                foreach (ExamWeightModel current2 in exams)
                {
                    text2 = text2 + current2.ExamID + ",";
                }
                text2 = text2.Remove(text2.Length - 1);
                string commandText = string.Concat(new object[]
                {
                    "SELECT s.StudentID, s.NameOfStudent, s.KCPESCore,c.NameOfClass,(SELECT CONVERT(varchar(50),row_no)+'/'+CONVERT(varchar(50),no_of_students) FROM (SELECT ROW_NUMBER() OVER(ORDER BY ISNULL(dbo.GetExamTotalScore(StudentID,",
                    num,
                    "),0)+ISNULL(dbo.GetExamTotalScore(StudentID,",
                    num2,
                    "),0)+ISNULL(dbo.GetExamTotalScore(StudentID,",
                    num3,
                    "),0) DESC) row_no, StudentID, (SELECT COUNT(*) FROM [Institution].[Student] WHERE ClassID =",
                    classID,
                    " AND IsActive=1) no_of_students FROM [Institution].[Student] WHERE CLassID=",
                    classID,
                    " AND IsActive=1 group by StudentID ) x WHERE x.StudentID=s.StudentID) ClassPosition,(SELECT CONVERT(varchar(50),row_no)+'/'+CONVERT(varchar(50),no_of_students) FROM (SELECT ROW_NUMBER() OVER(ORDER BY ISNULL(dbo.GetExamTotalScore(StudentID,",
                    num,
                    "),0)+ISNULL(dbo.GetExamTotalScore(StudentID,",
                    num2,
                    "),0)+ISNULL(dbo.GetExamTotalScore(StudentID,",
                    num3,
                    "),0) DESC) row_no, StudentID,(SELECT COUNT(*) FROM [Institution].[Student] WHERE ClassID IN(",
                    text,
                    ") AND IsActive=1) no_of_students FROM [Institution].[Student] WHERE CLassID IN (",
                    text,
                    ") AND IsActive=1 GROUP by StudentID ) x WHERE x.StudentID=s.StudentID) OverAllPosition,dbo.GetExamTotalScore(s.StudentID,",
                    num,
                    ") Exam1Score,dbo.GetExamTotalScore(s.StudentID,",
                    num2,
                    ")Exam2Score,dbo.GetExamTotalScore(s.StudentID,",
                    num3,
                    ")Exam3Score,ISNULL(sth.StudentTranscriptID,0),Responsibilities,ClubsAndSport, Boarding, ClassTeacher,ClassTeacherComments,Principal,PrincipalComments,OpeningDay,ClosingDay,Term1Pos,Term2Pos,Term3Pos,DateSaved FROM [Institution].[Student] s LEFT OUTER JOIN [Institution].[Class] c ON(s.ClassID=c.ClassID) LEFT OUTER JOIN [Institution].[StudentTranscriptHeader] sth ON(sth.StudentID=s.StudentID AND (sth.Exam1ID IN (",
                    text2,
                    ") OR sth.Exam2ID IN (",
                    text2,
                    ") OR sth.Exam3ID IN (",
                    text2,
                    "))) WHERE s.ClassID=",
                    classID,
                    " AND s.IsActive=1"
                });
                DataTable dataTable = DataAccessHelper.ExecuteNonQueryWithResultTable(commandText);
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    StudentTranscriptModel studentTranscriptModel = new StudentTranscriptModel();
                    studentTranscriptModel.StudentID = int.Parse(dataRow[0].ToString());
                    studentTranscriptModel.NameOfStudent = dataRow[1].ToString();
                    studentTranscriptModel.KCPEScore = (string.IsNullOrWhiteSpace(dataRow[2].ToString()) ? 0 : int.Parse(dataRow[2].ToString()));
                    studentTranscriptModel.NameOfClass = dataRow[3].ToString();
                    studentTranscriptModel.ClassPosition = dataRow[4].ToString();
                    studentTranscriptModel.OverAllPosition = dataRow[5].ToString();
                    studentTranscriptModel.CAT1Score = (string.IsNullOrWhiteSpace(dataRow[6].ToString()) ? null : new decimal?(decimal.Parse(dataRow[6].ToString())));
                    studentTranscriptModel.CAT2Score = (string.IsNullOrWhiteSpace(dataRow[7].ToString()) ? null : new decimal?(decimal.Parse(dataRow[7].ToString())));
                    studentTranscriptModel.ExamScore = (string.IsNullOrWhiteSpace(dataRow[8].ToString()) ? null : new decimal?(decimal.Parse(dataRow[8].ToString())));
                    studentTranscriptModel.MeanScore = (studentTranscriptModel.CAT1Score.HasValue ? studentTranscriptModel.CAT1Score.Value : 0m) + (studentTranscriptModel.CAT2Score.HasValue ? studentTranscriptModel.CAT2Score.Value : 0m) + (studentTranscriptModel.ExamScore.HasValue ? studentTranscriptModel.ExamScore.Value : 0m);
                    studentTranscriptModel.StudentTranscriptID = int.Parse(dataRow[9].ToString());
                    studentTranscriptModel.Responsibilities = dataRow[10].ToString();
                    studentTranscriptModel.ClubsAndSport = dataRow[11].ToString();
                    studentTranscriptModel.Boarding = dataRow[12].ToString();
                    studentTranscriptModel.ClassTeacher = dataRow[13].ToString();
                    studentTranscriptModel.ClassTeacherComments = dataRow[14].ToString();
                    studentTranscriptModel.Principal = dataRow[15].ToString();
                    studentTranscriptModel.PrincipalComments = dataRow[16].ToString();
                    studentTranscriptModel.OpeningDay = (string.IsNullOrWhiteSpace(dataRow[17].ToString()) ? DateTime.Now : DateTime.Parse(dataRow[17].ToString()));
                    studentTranscriptModel.ClosingDay = (string.IsNullOrWhiteSpace(dataRow[18].ToString()) ? DateTime.Now : DateTime.Parse(dataRow[18].ToString()));
                    studentTranscriptModel.Term1Pos = dataRow[19].ToString();
                    studentTranscriptModel.Term2Pos = dataRow[20].ToString();
                    studentTranscriptModel.Term3Pos = dataRow[21].ToString();
                    studentTranscriptModel.DateSaved = (string.IsNullOrWhiteSpace(dataRow[22].ToString()) ? DateTime.Now : DateTime.Parse(dataRow[22].ToString()));
                    studentTranscriptModel.Entries = DataAccess.GetTranscriptEntries(studentTranscriptModel.StudentID, exams);
                    studentTranscriptModel.Points = DataAccess.GetTranscriptAvgPoints(studentTranscriptModel.Entries);
                    studentTranscriptModel.MeanGrade = DataAccess.CalculateGradeFromPoints(studentTranscriptModel.Points);
                    decimal d = 0m;
                    decimal d2 = 0m;
                    decimal d3 = 0m;
                    ExamWeightModel arg_81A_0;
                    if (!exams.Any((ExamWeightModel o) => o.Index == 1))
                    {
                        arg_81A_0 = null;
                    }
                    else
                    {
                        arg_81A_0 = (from o in exams
                                     where o.Index == 1
                                     select o).ElementAt(0);
                    }
                    ExamWeightModel examWeightModel = arg_81A_0;
                    ExamWeightModel arg_87C_0;
                    if (!exams.Any((ExamWeightModel o) => o.Index == 2))
                    {
                        arg_87C_0 = null;
                    }
                    else
                    {
                        arg_87C_0 = (from o in exams
                                     where o.Index == 2
                                     select o).ElementAt(0);
                    }
                    ExamWeightModel examWeightModel2 = arg_87C_0;
                    ExamWeightModel arg_8DE_0;
                    if (!exams.Any((ExamWeightModel o) => o.Index == 3))
                    {
                        arg_8DE_0 = null;
                    }
                    else
                    {
                        arg_8DE_0 = (from o in exams
                                     where o.Index == 3
                                     select o).ElementAt(0);
                    }
                    ExamWeightModel examWeightModel3 = arg_8DE_0;
                    foreach (StudentExamResultEntryModel current3 in studentTranscriptModel.Entries)
                    {
                        decimal arg_940_0 = (current3.Cat1Score.HasValue && examWeightModel != null) ? DataAccess.ConvertScoreToOutOf(current3.Cat1Score.Value, examWeightModel.OutOf, 100m) : 0m;
                        d += ((current3.Cat1Score.HasValue && examWeightModel != null) ? DataAccess.CalculatePoints(DataAccess.CalculateGrade(DataAccess.ConvertScoreToOutOf(current3.Cat1Score.Value, examWeightModel.Weight, 100m))) : 0);
                        d2 += ((current3.Cat2Score.HasValue && examWeightModel2 != null) ? DataAccess.CalculatePoints(DataAccess.CalculateGrade(DataAccess.ConvertScoreToOutOf(current3.Cat2Score.Value, examWeightModel2.Weight, 100m))) : 0);
                        d3 += ((current3.ExamScore.HasValue && examWeightModel3 != null) ? DataAccess.CalculatePoints(DataAccess.CalculateGrade(DataAccess.ConvertScoreToOutOf(current3.ExamScore.Value, examWeightModel3.Weight, 100m))) : 0);
                    }
                    studentTranscriptModel.CAT1Grade = (((num == 0 && studentTranscriptModel.Entries.Count > 0) || d == 0m || studentTranscriptModel.Entries.Count == 0) ? "E" : DataAccess.CalculateGradeFromPoints((int)decimal.Ceiling(d / studentTranscriptModel.Entries.Count)));
                    studentTranscriptModel.CAT2Grade = (((num2 == 0 && studentTranscriptModel.Entries.Count > 0) || d == 0m || studentTranscriptModel.Entries.Count == 0) ? "E" : DataAccess.CalculateGradeFromPoints((int)decimal.Ceiling(d2 / studentTranscriptModel.Entries.Count)));
                    studentTranscriptModel.ExamGrade = (((num3 == 0 && studentTranscriptModel.Entries.Count > 0) || d == 0m || studentTranscriptModel.Entries.Count == 0) ? "E" : DataAccess.CalculateGradeFromPoints((int)decimal.Ceiling(d3 / studentTranscriptModel.Entries.Count)));
                    observableCollection.Add(studentTranscriptModel);
                }
                return observableCollection;
            });
        }

        public static Task<ObservableCollection<StudentTranscriptModel2>> GetClassTranscripts2Async(int classID, IEnumerable<ExamWeightModel> exams, IEnumerable<ClassModel> classes, IProgress<OperationProgress> progressReporter)
        {
            return Task.Run<ObservableCollection<StudentTranscriptModel2>>(delegate
            {
                progressReporter.Report(new OperationProgress(1, "Initializing"));
                progressReporter.Report(new OperationProgress(3, "Obtaning Exam Data"));
                ObservableCollection<StudentTranscriptModel2> observableCollection = new ObservableCollection<StudentTranscriptModel2>();
                int num = 0;
                int num2 = 0;
                int num3 = 0;
                int num4 = 0;
                int num5 = 0;
                int num6 = 0;
                int num7 = 0;
                int num8 = 0;
                int num9 = 0;
                int num10 = 0;
                int num11 = 0;
                int num12 = 0;
                decimal num13 = 0m;
                decimal num14 = 0m;
                decimal num15 = 0m;
                decimal num16 = 0m;
                decimal num17 = 0m;
                decimal num18 = 0m;
                decimal num19 = 0m;
                decimal num20 = 0m;
                decimal num21 = 0m;
                decimal num22 = 0m;
                decimal num23 = 0m;
                decimal num24 = 0m;
                int currentTerm = DataAccess.GetTerm();
                switch (currentTerm)
                {
                    case 1:
                        num4 = DataAccess.GetTermExamID(exams, 1);
                        num16 = DataAccess.GetTermExamWeight(exams, 1);
                        num5 = DataAccess.GetTermExamID(exams, 2);
                        num17 = DataAccess.GetTermExamWeight(exams, 2);
                        num6 = DataAccess.GetTermExamID(exams, 3);
                        num18 = DataAccess.GetTermExamWeight(exams, 3);
                        break;
                    case 2:
                        num7 = DataAccess.GetTermExamID(exams, 1);
                        num19 = DataAccess.GetTermExamWeight(exams, 1);
                        num8 = DataAccess.GetTermExamID(exams, 2);
                        num20 = DataAccess.GetTermExamWeight(exams, 2);
                        num9 = DataAccess.GetTermExamID(exams, 3);
                        num21 = DataAccess.GetTermExamWeight(exams, 3);
                        break;
                    case 3:
                        num10 = DataAccess.GetTermExamID(exams, 1);
                        num22 = DataAccess.GetTermExamWeight(exams, 1);
                        num11 = DataAccess.GetTermExamID(exams, 2);
                        num23 = DataAccess.GetTermExamWeight(exams, 2);
                        num12 = DataAccess.GetTermExamID(exams, 3);
                        num24 = DataAccess.GetTermExamWeight(exams, 3);
                        break;
                }
                List<int> list = new List<int>(from o in new List<int>(3)
                {
                    -1,
                    1,
                    2,
                    3
                }
                                               where o != currentTerm
                                               select o);
                List<ExamWeightModel> otherTermClassExams = DataAccess.GetOtherTermClassExams(classID, list);
                foreach (int current in list)
                {
                    if (current == -1)
                    {
                        int arg_2D1_0;
                        if (!otherTermClassExams.Any((ExamWeightModel o) => o.Index == 1 && DataAccess.GetTerm(o.ExamDateTime) == -1))
                        {
                            arg_2D1_0 = 0;
                        }
                        else
                        {
                            arg_2D1_0 = otherTermClassExams.First((ExamWeightModel o) => o.Index == 1 && DataAccess.GetTerm(o.ExamDateTime) == -1).ExamID;
                        }
                        num = arg_2D1_0;
                        decimal arg_32E_0;
                        if (!otherTermClassExams.Any((ExamWeightModel o) => o.Index == 1 && DataAccess.GetTerm(o.ExamDateTime) == -1))
                        {
                            arg_32E_0 = 0m;
                        }
                        else
                        {
                            arg_32E_0 = otherTermClassExams.First((ExamWeightModel o) => o.Index == 1 && DataAccess.GetTerm(o.ExamDateTime) == -1).Weight;
                        }
                        num13 = arg_32E_0;
                        int arg_387_0;
                        if (!otherTermClassExams.Any((ExamWeightModel o) => o.Index == 2 && DataAccess.GetTerm(o.ExamDateTime) == -1))
                        {
                            arg_387_0 = 0;
                        }
                        else
                        {
                            arg_387_0 = otherTermClassExams.First((ExamWeightModel o) => o.Index == 2 && DataAccess.GetTerm(o.ExamDateTime) == -1).ExamID;
                        }
                        num2 = arg_387_0;
                        decimal arg_3E4_0;
                        if (!otherTermClassExams.Any((ExamWeightModel o) => o.Index == 2 && DataAccess.GetTerm(o.ExamDateTime) == -1))
                        {
                            arg_3E4_0 = 0m;
                        }
                        else
                        {
                            arg_3E4_0 = otherTermClassExams.First((ExamWeightModel o) => o.Index == 2 && DataAccess.GetTerm(o.ExamDateTime) == -1).Weight;
                        }
                        num14 = arg_3E4_0;
                        int arg_43D_0;
                        if (!otherTermClassExams.Any((ExamWeightModel o) => o.Index == 3 && DataAccess.GetTerm(o.ExamDateTime) == -1))
                        {
                            arg_43D_0 = 0;
                        }
                        else
                        {
                            arg_43D_0 = otherTermClassExams.First((ExamWeightModel o) => o.Index == 3 && DataAccess.GetTerm(o.ExamDateTime) == -1).ExamID;
                        }
                        num3 = arg_43D_0;
                        decimal arg_49A_0;
                        if (!otherTermClassExams.Any((ExamWeightModel o) => o.Index == 3 && DataAccess.GetTerm(o.ExamDateTime) == -1))
                        {
                            arg_49A_0 = 0m;
                        }
                        else
                        {
                            arg_49A_0 = otherTermClassExams.First((ExamWeightModel o) => o.Index == 3 && DataAccess.GetTerm(o.ExamDateTime) == -1).Weight;
                        }
                        num15 = arg_49A_0;
                    }
                    else if (current == 1)
                    {
                        int arg_50B_0;
                        if (!otherTermClassExams.Any((ExamWeightModel o) => o.Index == 1 && DataAccess.GetTerm(o.ExamDateTime) == 1))
                        {
                            arg_50B_0 = 0;
                        }
                        else
                        {
                            arg_50B_0 = otherTermClassExams.First((ExamWeightModel o) => o.Index == 1 && DataAccess.GetTerm(o.ExamDateTime) == 1).ExamID;
                        }
                        num4 = arg_50B_0;
                        decimal arg_569_0;
                        if (!otherTermClassExams.Any((ExamWeightModel o) => o.Index == 1 && DataAccess.GetTerm(o.ExamDateTime) == 1))
                        {
                            arg_569_0 = 0m;
                        }
                        else
                        {
                            arg_569_0 = otherTermClassExams.First((ExamWeightModel o) => o.Index == 1 && DataAccess.GetTerm(o.ExamDateTime) == 1).Weight;
                        }
                        num16 = arg_569_0;
                        int arg_5C2_0;
                        if (!otherTermClassExams.Any((ExamWeightModel o) => o.Index == 2 && DataAccess.GetTerm(o.ExamDateTime) == 1))
                        {
                            arg_5C2_0 = 0;
                        }
                        else
                        {
                            arg_5C2_0 = otherTermClassExams.First((ExamWeightModel o) => o.Index == 2 && DataAccess.GetTerm(o.ExamDateTime) == 1).ExamID;
                        }
                        num5 = arg_5C2_0;
                        decimal arg_620_0;
                        if (!otherTermClassExams.Any((ExamWeightModel o) => o.Index == 2 && DataAccess.GetTerm(o.ExamDateTime) == 1))
                        {
                            arg_620_0 = 0m;
                        }
                        else
                        {
                            arg_620_0 = otherTermClassExams.First((ExamWeightModel o) => o.Index == 2 && DataAccess.GetTerm(o.ExamDateTime) == 1).Weight;
                        }
                        num17 = arg_620_0;
                        int arg_679_0;
                        if (!otherTermClassExams.Any((ExamWeightModel o) => o.Index == 3 && DataAccess.GetTerm(o.ExamDateTime) == 1))
                        {
                            arg_679_0 = 0;
                        }
                        else
                        {
                            arg_679_0 = otherTermClassExams.First((ExamWeightModel o) => o.Index == 3 && DataAccess.GetTerm(o.ExamDateTime) == 1).ExamID;
                        }
                        num6 = arg_679_0;
                        decimal arg_6D7_0;
                        if (!otherTermClassExams.Any((ExamWeightModel o) => o.Index == 3 && DataAccess.GetTerm(o.ExamDateTime) == 1))
                        {
                            arg_6D7_0 = 0m;
                        }
                        else
                        {
                            arg_6D7_0 = otherTermClassExams.First((ExamWeightModel o) => o.Index == 3 && DataAccess.GetTerm(o.ExamDateTime) == 1).Weight;
                        }
                        num18 = arg_6D7_0;
                    }
                    else if (current == 2)
                    {
                        int arg_748_0;
                        if (!otherTermClassExams.Any((ExamWeightModel o) => o.Index == 1 && DataAccess.GetTerm(o.ExamDateTime) == 2))
                        {
                            arg_748_0 = 0;
                        }
                        else
                        {
                            arg_748_0 = otherTermClassExams.First((ExamWeightModel o) => o.Index == 1 && DataAccess.GetTerm(o.ExamDateTime) == 2).ExamID;
                        }
                        num7 = arg_748_0;
                        decimal arg_7A6_0;
                        if (!otherTermClassExams.Any((ExamWeightModel o) => o.Index == 1 && DataAccess.GetTerm(o.ExamDateTime) == 2))
                        {
                            arg_7A6_0 = 0m;
                        }
                        else
                        {
                            arg_7A6_0 = otherTermClassExams.First((ExamWeightModel o) => o.Index == 1 && DataAccess.GetTerm(o.ExamDateTime) == 2).Weight;
                        }
                        num19 = arg_7A6_0;
                        int arg_7FF_0;
                        if (!otherTermClassExams.Any((ExamWeightModel o) => o.Index == 2 && DataAccess.GetTerm(o.ExamDateTime) == 2))
                        {
                            arg_7FF_0 = 0;
                        }
                        else
                        {
                            arg_7FF_0 = otherTermClassExams.First((ExamWeightModel o) => o.Index == 2 && DataAccess.GetTerm(o.ExamDateTime) == 2).ExamID;
                        }
                        num8 = arg_7FF_0;
                        decimal arg_85D_0;
                        if (!otherTermClassExams.Any((ExamWeightModel o) => o.Index == 2 && DataAccess.GetTerm(o.ExamDateTime) == 2))
                        {
                            arg_85D_0 = 0m;
                        }
                        else
                        {
                            arg_85D_0 = otherTermClassExams.First((ExamWeightModel o) => o.Index == 2 && DataAccess.GetTerm(o.ExamDateTime) == 2).Weight;
                        }
                        num20 = arg_85D_0;
                        int arg_8B6_0;
                        if (!otherTermClassExams.Any((ExamWeightModel o) => o.Index == 3 && DataAccess.GetTerm(o.ExamDateTime) == 2))
                        {
                            arg_8B6_0 = 0;
                        }
                        else
                        {
                            arg_8B6_0 = otherTermClassExams.First((ExamWeightModel o) => o.Index == 3 && DataAccess.GetTerm(o.ExamDateTime) == 2).ExamID;
                        }
                        num9 = arg_8B6_0;
                        decimal arg_914_0;
                        if (!otherTermClassExams.Any((ExamWeightModel o) => o.Index == 3 && DataAccess.GetTerm(o.ExamDateTime) == 2))
                        {
                            arg_914_0 = 0m;
                        }
                        else
                        {
                            arg_914_0 = otherTermClassExams.First((ExamWeightModel o) => o.Index == 3 && DataAccess.GetTerm(o.ExamDateTime) == 2).Weight;
                        }
                        num21 = arg_914_0;
                    }
                    else
                    {
                        int arg_974_0;
                        if (!otherTermClassExams.Any((ExamWeightModel o) => o.Index == 1 && DataAccess.GetTerm(o.ExamDateTime) == 3))
                        {
                            arg_974_0 = 0;
                        }
                        else
                        {
                            arg_974_0 = otherTermClassExams.First((ExamWeightModel o) => o.Index == 1 && DataAccess.GetTerm(o.ExamDateTime) == 3).ExamID;
                        }
                        num10 = arg_974_0;
                        decimal arg_9D2_0;
                        if (!otherTermClassExams.Any((ExamWeightModel o) => o.Index == 1 && DataAccess.GetTerm(o.ExamDateTime) == 3))
                        {
                            arg_9D2_0 = 0m;
                        }
                        else
                        {
                            arg_9D2_0 = otherTermClassExams.First((ExamWeightModel o) => o.Index == 1 && DataAccess.GetTerm(o.ExamDateTime) == 3).Weight;
                        }
                        num22 = arg_9D2_0;
                        int arg_A2B_0;
                        if (!otherTermClassExams.Any((ExamWeightModel o) => o.Index == 2 && DataAccess.GetTerm(o.ExamDateTime) == 3))
                        {
                            arg_A2B_0 = 0;
                        }
                        else
                        {
                            arg_A2B_0 = otherTermClassExams.First((ExamWeightModel o) => o.Index == 2 && DataAccess.GetTerm(o.ExamDateTime) == 3).ExamID;
                        }
                        num11 = arg_A2B_0;
                        decimal arg_A89_0;
                        if (!otherTermClassExams.Any((ExamWeightModel o) => o.Index == 2 && DataAccess.GetTerm(o.ExamDateTime) == 3))
                        {
                            arg_A89_0 = 0m;
                        }
                        else
                        {
                            arg_A89_0 = otherTermClassExams.First((ExamWeightModel o) => o.Index == 2 && DataAccess.GetTerm(o.ExamDateTime) == 3).Weight;
                        }
                        num23 = arg_A89_0;
                        int arg_AE2_0;
                        if (!otherTermClassExams.Any((ExamWeightModel o) => o.Index == 3 && DataAccess.GetTerm(o.ExamDateTime) == 3))
                        {
                            arg_AE2_0 = 0;
                        }
                        else
                        {
                            arg_AE2_0 = otherTermClassExams.First((ExamWeightModel o) => o.Index == 3 && DataAccess.GetTerm(o.ExamDateTime) == 3).ExamID;
                        }
                        num12 = arg_AE2_0;
                        decimal arg_B40_0;
                        if (!otherTermClassExams.Any((ExamWeightModel o) => o.Index == 3 && DataAccess.GetTerm(o.ExamDateTime) == 3))
                        {
                            arg_B40_0 = 0m;
                        }
                        else
                        {
                            arg_B40_0 = otherTermClassExams.First((ExamWeightModel o) => o.Index == 3 && DataAccess.GetTerm(o.ExamDateTime) == 3).Weight;
                        }
                        num24 = arg_B40_0;
                    }
                }
                progressReporter.Report(new OperationProgress(5, "Filtering Data"));
                string text = "0,";
                foreach (ClassModel current2 in classes)
                {
                    text = text + current2.ClassID + ",";
                }
                text = text.Remove(text.Length - 1);
                string text2 = string.Concat(new object[]
                {
                    num,
                    ",",
                    num2,
                    ",",
                    num3
                });
                string text3 = string.Concat(new object[]
                {
                    num4,
                    ",",
                    num5,
                    ",",
                    num6
                });
                string text4 = string.Concat(new object[]
                {
                    num7,
                    ",",
                    num8,
                    ",",
                    num9
                });
                string text5 = string.Concat(new object[]
                {
                    num10,
                    ",",
                    num11,
                    ",",
                    num12
                });
                progressReporter.Report(new OperationProgress(4, "Executing Query"));
                string commandText = string.Concat(new object[]
                {
                    "SELECT t1.StudentTranscriptID,t1.StudentID, t1.NameOfStudent,t1.KCPEScore, t1.NameOfClass,t1.Responsibilities,t1.ClubsAndSport,t1.Boarding,t1.ClassTeacherComments, t1.PrincipalComments,t1.OpeningDay,t1.ClosingDay,t1.DateSaved,t1.ClassPosition,t1.OverAllPosition,t1.Exam1Score,t1.Exam2Score,t1.Exam3Score,t2.T2ClassPosition,t2.T2OverAllPosition,t2.T2Exam1Score,t2.T2Exam2Score,t2.T2Exam3Score,t3.T3ClassPosition,t3.T3OverAllPosition,t3.T3Exam1Score,t3.T3Exam2Score,t3.T3Exam3Score,pyT3.PyT3Exam1Score,pyT3.PyT3Exam2Score,pyt3.PyT3Exam3Score FROM (SELECT s.StudentID, s.NameOfStudent,s.KCPEScore, c.NameOfClass,ISNULL(sth.StudentTranscriptID,0) StudentTranscriptID,Responsibilities,ClubsAndSport, Boarding,ClassTeacherComments,PrincipalComments,OpeningDay,ClosingDay,DateSaved,(SELECT CONVERT(varchar(50),row_no)+'/'+CONVERT(varchar(50),no_of_students) FROM (SELECT ROW_NUMBER() OVER(ORDER BY ISNULL(dbo.GetWeightedExamTotalScore(StudentID,",
                    num4,
                    ",",
                    num16,
                    "),0)+ISNULL(dbo.GetWeightedExamTotalScore(StudentID,",
                    num5,
                    ",",
                    num17,
                    "),0)+ISNULL(dbo.GetWeightedExamTotalScore(StudentID,",
                    num6,
                    ",",
                    num18,
                    "),0) DESC) row_no, StudentID, (SELECT COUNT(*) FROM [Institution].[Student] WHERE ClassID =",
                    classID,
                    " AND IsActive=1) no_of_students FROM [Institution].[Student] WHERE CLassID=",
                    classID,
                    " AND IsActive=1 group by StudentID ) x WHERE x.StudentID=s.StudentID) ClassPosition,(SELECT CONVERT(varchar(50),row_no)+'/'+CONVERT(varchar(50),no_of_students) FROM (SELECT ROW_NUMBER() OVER(ORDER BY ISNULL(dbo.GetWeightedExamTotalScore(StudentID,",
                    num4,
                    ",",
                    num16,
                    "),0)+ISNULL(dbo.GetWeightedExamTotalScore(StudentID,",
                    num5,
                    ",",
                    num17,
                    "),0)+ISNULL(dbo.GetWeightedExamTotalScore(StudentID,",
                    num6,
                    ",",
                    num18,
                    "),0) DESC) row_no, StudentID,(SELECT COUNT(*) FROM [Institution].[Student] WHERE ClassID IN(",
                    text,
                    ") AND IsActive=1) no_of_students FROM [Institution].[Student] WHERE CLassID IN (",
                    text,
                    ") AND IsActive=1 GROUP by StudentID ) x WHERE x.StudentID=s.StudentID) OverAllPosition,dbo.GetWeightedExamTotalScore(s.StudentID,",
                    num4,
                    ",",
                    num16,
                    ") Exam1Score,dbo.GetWeightedExamTotalScore(s.StudentID,",
                    num5,
                    ",",
                    num17,
                    ")Exam2Score,dbo.GetWeightedExamTotalScore(s.StudentID,",
                    num6,
                    ",",
                    num18,
                    ")Exam3Score FROM [Institution].[Student] s LEFT OUTER JOIN [Institution].[Class] c ON(s.ClassID=c.ClassID) LEFT OUTER JOIN [Institution].[StudentTranscriptHeader] sth ON(sth.StudentID=s.StudentID  AND sth.DateSaved BETWEEN CONVERT(datetime,'",
                    DataAccess.GetTermStart().ToString("g"),
                    "') AND CONVERT(datetime,'",
                    DataAccess.GetTermEnd().ToString("g"),
                    "')) LEFT OUTER JOIN [Institution].[StudentTranscriptExamDetail] sted ON (sted.StudentTranscriptID=sth.StudentTranscriptID) WHERE s.IsActive=1 AND s.ClassID=",
                    classID,
                    ") t1 LEFT OUTER JOIN (SELECT s.StudentID,(SELECT CONVERT(varchar(50),row_no)+'/'+CONVERT(varchar(50),no_of_students) FROM (SELECT ROW_NUMBER() OVER(ORDER BY ISNULL(dbo.GetWeightedExamTotalScore(StudentID,",
                    num7,
                    ",",
                    num19,
                    "),0)+ISNULL(dbo.GetWeightedExamTotalScore(StudentID,",
                    num8,
                    ",",
                    num20,
                    "),0)+ISNULL(dbo.GetWeightedExamTotalScore(StudentID,",
                    num9,
                    ",",
                    num21,
                    "),0) DESC) row_no, StudentID, (SELECT COUNT(*) FROM [Institution].[Student] WHERE ClassID =",
                    classID,
                    " AND IsActive=1) no_of_students FROM [Institution].[Student] WHERE CLassID=",
                    classID,
                    " AND IsActive=1 group by StudentID ) x WHERE x.StudentID=s.StudentID) T2ClassPosition,(SELECT CONVERT(varchar(50),row_no)+'/'+CONVERT(varchar(50),no_of_students) FROM (SELECT ROW_NUMBER() OVER(ORDER BY ISNULL(dbo.GetWeightedExamTotalScore(StudentID,",
                    num7,
                    ",",
                    num19,
                    "),0)+ISNULL(dbo.GetWeightedExamTotalScore(StudentID,",
                    num8,
                    ",",
                    num20,
                    "),0)+ISNULL(dbo.GetWeightedExamTotalScore(StudentID,",
                    num9,
                    ",",
                    num24,
                    "),0) DESC) row_no, StudentID,(SELECT COUNT(*) FROM [Institution].[Student] WHERE ClassID IN(",
                    text,
                    ") AND IsActive=1) no_of_students FROM [Institution].[Student] WHERE CLassID IN (",
                    text,
                    ") AND IsActive=1 GROUP by StudentID ) x WHERE x.StudentID=s.StudentID) T2OverAllPosition,dbo.GetWeightedExamTotalScore(s.StudentID,",
                    num7,
                    ",",
                    num19,
                    ") T2Exam1Score,dbo.GetWeightedExamTotalScore(s.StudentID,",
                    num8,
                    ",",
                    num20,
                    ")T2Exam2Score,dbo.GetWeightedExamTotalScore(s.StudentID,",
                    num9,
                    ",",
                    num21,
                    ")T2Exam3Score FROM [Institution].[Student] s LEFT OUTER JOIN [Institution].[Class] c ON(s.ClassID=c.ClassID) LEFT OUTER JOIN [Institution].[StudentTranscriptHeader] sth ON(sth.StudentID=s.StudentID AND sth.DateSaved BETWEEN CONVERT(datetime,'",
                    DataAccess.GetTermStart().ToString("g"),
                    "') AND CONVERT(datetime,'",
                    DataAccess.GetTermEnd().ToString("g"),
                    "')) LEFT OUTER JOIN [Institution].[StudentTranscriptExamDetail] sted ON (sted.StudentTranscriptID=sth.StudentTranscriptID) WHERE s.IsActive=1 AND s.ClassID=",
                    classID,
                    ") t2 ON (t1.StudentID=t2.StudentID) LEFT OUTER JOIN (SELECT s.StudentID,(SELECT CONVERT(varchar(50),row_no)+'/'+CONVERT(varchar(50),no_of_students) FROM (SELECT ROW_NUMBER() OVER(ORDER BY ISNULL(dbo.GetWeightedExamTotalScore(StudentID,",
                    num10,
                    ",",
                    num22,
                    "),0)+ISNULL(dbo.GetWeightedExamTotalScore(StudentID,",
                    num11,
                    ",",
                    num23,
                    "),0)+ISNULL(dbo.GetWeightedExamTotalScore(StudentID,",
                    num12,
                    ",",
                    num24,
                    "),0) DESC) row_no, StudentID, (SELECT COUNT(*) FROM [Institution].[Student] WHERE ClassID =",
                    classID,
                    " AND IsActive=1) no_of_students FROM [Institution].[Student] WHERE CLassID=",
                    classID,
                    " AND IsActive=1 group by StudentID ) x WHERE x.StudentID=s.StudentID) T3ClassPosition,(SELECT CONVERT(varchar(50),row_no)+'/'+CONVERT(varchar(50),no_of_students) FROM (SELECT ROW_NUMBER() OVER(ORDER BY ISNULL(dbo.GetWeightedExamTotalScore(StudentID,",
                    num10,
                    ",",
                    num22,
                    "),0)+ISNULL(dbo.GetWeightedExamTotalScore(StudentID,",
                    num11,
                    ",",
                    num23,
                    "),0)+ISNULL(dbo.GetWeightedExamTotalScore(StudentID,",
                    num12,
                    ",",
                    num24,
                    "),0) DESC) row_no, StudentID,(SELECT COUNT(*) FROM [Institution].[Student] WHERE ClassID IN(",
                    text,
                    ") AND IsActive=1) no_of_students FROM [Institution].[Student] WHERE CLassID IN (",
                    text,
                    ") AND IsActive=1 GROUP by StudentID ) x WHERE x.StudentID=s.StudentID) T3OverAllPosition,dbo.GetWeightedExamTotalScore(s.StudentID,",
                    num10,
                    ",",
                    num22,
                    ") T3Exam1Score,dbo.GetWeightedExamTotalScore(s.StudentID,",
                    num11,
                    ",",
                    num23,
                    ")T3Exam2Score,dbo.GetWeightedExamTotalScore(s.StudentID,",
                    num12,
                    ",",
                    num24,
                    ")T3Exam3Score FROM [Institution].[Student] s LEFT OUTER JOIN [Institution].[Class] c ON(s.ClassID=c.ClassID) LEFT OUTER JOIN [Institution].[StudentTranscriptHeader] sth ON(sth.StudentID=s.StudentID AND sth.DateSaved BETWEEN CONVERT(datetime,'",
                    DataAccess.GetTermStart().ToString("g"),
                    "') AND CONVERT(datetime,'",
                    DataAccess.GetTermEnd().ToString("g"),
                    "')) LEFT OUTER JOIN [Institution].[StudentTranscriptExamDetail] sted ON (sted.StudentTranscriptID=sth.StudentTranscriptID) WHERE s.IsActive=1 AND s.ClassID=",
                    classID,
                    " )t3  ON (t3.StudentID=t1.StudentID) LEFT OUTER JOIN (SELECT s.StudentID, dbo.GetWeightedExamTotalScore(s.StudentID,",
                    num,
                    ",",
                    num13,
                    ") PyT3Exam1Score,dbo.GetWeightedExamTotalScore(s.StudentID,",
                    num2,
                    ",",
                    num14,
                    ") PyT3Exam2Score,dbo.GetWeightedExamTotalScore(s.StudentID,",
                    num3,
                    ",",
                    num15,
                    ")PyT3Exam3Score FROM [Institution].[Student] s LEFT OUTER JOIN [Institution].[Class] c ON(s.ClassID=c.ClassID) LEFT OUTER JOIN [Institution].[StudentTranscriptHeader] sth ON(sth.StudentID=s.StudentID AND sth.DateSaved BETWEEN CONVERT(datetime,'",
                    DataAccess.GetTermStart().ToString("g"),
                    "') AND CONVERT(datetime,'",
                    DataAccess.GetTermEnd().ToString("g"),
                    "')) LEFT OUTER JOIN [Institution].[StudentTranscriptExamDetail] sted ON (sted.StudentTranscriptID=sth.StudentTranscriptID) WHERE s.IsActive=1 AND s.ClassID=",
                    classID,
                    " )pyT3  ON (pyT3.StudentID=t1.StudentID)"
                });
                DataTable dataTable = DataAccessHelper.ExecuteNonQueryWithResultTable(commandText);
                progressReporter.Report(new OperationProgress(15, "Filling Report Forms"));
                double num25 = 0.0;
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    StudentTranscriptModel2 studentTranscriptModel = new StudentTranscriptModel2();
                    studentTranscriptModel.StudentTranscriptID = int.Parse(dataRow[0].ToString());
                    studentTranscriptModel.StudentID = int.Parse(dataRow[1].ToString());
                    studentTranscriptModel.NameOfStudent = dataRow[2].ToString();
                    studentTranscriptModel.KCPEScore = (string.IsNullOrWhiteSpace(dataRow[3].ToString()) ? 0 : int.Parse(dataRow[3].ToString()));
                    studentTranscriptModel.NameOfClass = dataRow[4].ToString();
                    studentTranscriptModel.Responsibilities = dataRow[5].ToString();
                    studentTranscriptModel.ClubsAndSport = dataRow[6].ToString();
                    studentTranscriptModel.Boarding = dataRow[7].ToString();
                    studentTranscriptModel.ClassTeacherComments = dataRow[8].ToString();
                    studentTranscriptModel.PrincipalComments = dataRow[9].ToString();
                    studentTranscriptModel.OpeningDay = (string.IsNullOrWhiteSpace(dataRow[10].ToString()) ? DateTime.Now : DateTime.Parse(dataRow[10].ToString()));
                    studentTranscriptModel.ClosingDay = (string.IsNullOrWhiteSpace(dataRow[11].ToString()) ? DateTime.Now : DateTime.Parse(dataRow[11].ToString()));
                    studentTranscriptModel.DateSaved = (string.IsNullOrWhiteSpace(dataRow[12].ToString()) ? DateTime.Now : DateTime.Parse(dataRow[12].ToString()));
                    studentTranscriptModel.Term1Pos = dataRow[13].ToString();
                    studentTranscriptModel.Term1OverallPos = dataRow[14].ToString();
                    switch (currentTerm)
                    {
                        case 1:
                            studentTranscriptModel.Term1Entries = DataAccess.GetTranscriptEntries(studentTranscriptModel.StudentID, exams);
                            studentTranscriptModel.Term2Entries = DataAccess.GetTranscriptEntries(studentTranscriptModel.StudentID, from o in otherTermClassExams
                                                                                                                                    where DataAccess.GetTerm(o.ExamDateTime) == 2
                                                                                                                                    select o);
                            studentTranscriptModel.Term3Entries = DataAccess.GetTranscriptEntries(studentTranscriptModel.StudentID, from o in otherTermClassExams
                                                                                                                                    where DataAccess.GetTerm(o.ExamDateTime) == 3
                                                                                                                                    select o);
                            studentTranscriptModel.PrevYearEntries = DataAccess.GetTranscriptEntries(studentTranscriptModel.StudentID, from o in otherTermClassExams
                                                                                                                                       where DataAccess.GetTerm(o.ExamDateTime) == -1
                                                                                                                                       select o);
                            break;
                        case 2:
                            studentTranscriptModel.Term2Entries = DataAccess.GetTranscriptEntries(studentTranscriptModel.StudentID, exams);
                            studentTranscriptModel.Term1Entries = DataAccess.GetTranscriptEntries(studentTranscriptModel.StudentID, from o in otherTermClassExams
                                                                                                                                    where DataAccess.GetTerm(o.ExamDateTime) == 1
                                                                                                                                    select o);
                            studentTranscriptModel.Term3Entries = DataAccess.GetTranscriptEntries(studentTranscriptModel.StudentID, from o in otherTermClassExams
                                                                                                                                    where DataAccess.GetTerm(o.ExamDateTime) == 3
                                                                                                                                    select o);
                            studentTranscriptModel.PrevYearEntries = DataAccess.GetTranscriptEntries(studentTranscriptModel.StudentID, from o in otherTermClassExams
                                                                                                                                       where DataAccess.GetTerm(o.ExamDateTime) == -1
                                                                                                                                       select o);
                            break;
                        case 3:
                            studentTranscriptModel.Term3Entries = DataAccess.GetTranscriptEntries(studentTranscriptModel.StudentID, exams);
                            studentTranscriptModel.Term1Entries = DataAccess.GetTranscriptEntries(studentTranscriptModel.StudentID, from o in otherTermClassExams
                                                                                                                                    where DataAccess.GetTerm(o.ExamDateTime) == 1
                                                                                                                                    select o);
                            studentTranscriptModel.Term2Entries = DataAccess.GetTranscriptEntries(studentTranscriptModel.StudentID, from o in otherTermClassExams
                                                                                                                                    where DataAccess.GetTerm(o.ExamDateTime) == 2
                                                                                                                                    select o);
                            studentTranscriptModel.PrevYearEntries = DataAccess.GetTranscriptEntries(studentTranscriptModel.StudentID, from o in otherTermClassExams
                                                                                                                                       where DataAccess.GetTerm(o.ExamDateTime) == -1
                                                                                                                                       select o);
                            break;
                    }
                    decimal? num26 = (string.IsNullOrWhiteSpace(dataRow[15].ToString()) && string.IsNullOrWhiteSpace(dataRow[16].ToString()) && string.IsNullOrWhiteSpace(dataRow[17].ToString())) ? null : new decimal?((string.IsNullOrWhiteSpace(dataRow[15].ToString()) ? 0m : decimal.Parse(dataRow[15].ToString())) + (string.IsNullOrWhiteSpace(dataRow[16].ToString()) ? 0m : decimal.Parse(dataRow[16].ToString())) + (string.IsNullOrWhiteSpace(dataRow[17].ToString()) ? 0m : decimal.Parse(dataRow[17].ToString())));
                    studentTranscriptModel.Term1TotalScore = (num26.HasValue ? num26.Value.ToString("N0") : "") + " of " + 100 * studentTranscriptModel.Term1Entries.Count;
                    studentTranscriptModel.Term2Pos = dataRow[18].ToString();
                    studentTranscriptModel.Term2OverallPos = dataRow[19].ToString();
                    decimal? num27 = (string.IsNullOrWhiteSpace(dataRow[20].ToString()) && string.IsNullOrWhiteSpace(dataRow[21].ToString()) && string.IsNullOrWhiteSpace(dataRow[22].ToString())) ? null : new decimal?((string.IsNullOrWhiteSpace(dataRow[20].ToString()) ? 0m : decimal.Parse(dataRow[20].ToString())) + (string.IsNullOrWhiteSpace(dataRow[21].ToString()) ? 0m : decimal.Parse(dataRow[21].ToString())) + (string.IsNullOrWhiteSpace(dataRow[22].ToString()) ? 0m : decimal.Parse(dataRow[22].ToString())));
                    studentTranscriptModel.Term2TotalScore = (num27.HasValue ? num27.Value.ToString("N0") : "") + " of " + 100 * studentTranscriptModel.Term2Entries.Count;
                    studentTranscriptModel.Term3Pos = dataRow[23].ToString();
                    studentTranscriptModel.Term3OverallPos = dataRow[24].ToString();
                    decimal? num28 = (string.IsNullOrWhiteSpace(dataRow[25].ToString()) && string.IsNullOrWhiteSpace(dataRow[26].ToString()) && string.IsNullOrWhiteSpace(dataRow[27].ToString())) ? null : new decimal?((string.IsNullOrWhiteSpace(dataRow[25].ToString()) ? 0m : decimal.Parse(dataRow[25].ToString())) + (string.IsNullOrWhiteSpace(dataRow[26].ToString()) ? 0m : decimal.Parse(dataRow[26].ToString())) + (string.IsNullOrWhiteSpace(dataRow[27].ToString()) ? 0m : decimal.Parse(dataRow[27].ToString())));
                    studentTranscriptModel.Term3TotalScore = (num28.HasValue ? num28.Value.ToString("N0") : "") + " of " + 100 * studentTranscriptModel.Term3Entries.Count;
                    decimal? num29 = (string.IsNullOrWhiteSpace(dataRow[28].ToString()) && string.IsNullOrWhiteSpace(dataRow[29].ToString()) && string.IsNullOrWhiteSpace(dataRow[30].ToString())) ? null : new decimal?((string.IsNullOrWhiteSpace(dataRow[28].ToString()) ? 0m : decimal.Parse(dataRow[28].ToString())) + (string.IsNullOrWhiteSpace(dataRow[29].ToString()) ? 0m : decimal.Parse(dataRow[29].ToString())) + (string.IsNullOrWhiteSpace(dataRow[30].ToString()) ? 0m : decimal.Parse(dataRow[30].ToString())));
                    decimal transcriptTotPoints = DataAccess.GetTranscriptTotPoints(studentTranscriptModel.Term1Entries);
                    decimal transcriptTotPoints2 = DataAccess.GetTranscriptTotPoints(studentTranscriptModel.Term2Entries);
                    decimal transcriptTotPoints3 = DataAccess.GetTranscriptTotPoints(studentTranscriptModel.Term3Entries);
                    studentTranscriptModel.Term1TotalPoints = transcriptTotPoints + " of " + studentTranscriptModel.Term1Entries.Count * 12;
                    studentTranscriptModel.Term2TotalPoints = transcriptTotPoints2 + " of " + studentTranscriptModel.Term2Entries.Count * 12;
                    studentTranscriptModel.Term3TotalPoints = transcriptTotPoints3 + " of " + studentTranscriptModel.Term3Entries.Count * 12;
                    studentTranscriptModel.PrevYearAvgPoints = (num29.HasValue ? DataAccess.GetTranscriptAvgPoints(studentTranscriptModel.PrevYearEntries) : 0m);
                    studentTranscriptModel.Term1AvgPts = (num26.HasValue ? DataAccess.GetTranscriptAvgPoints(studentTranscriptModel.Term1Entries) : 0m);
                    studentTranscriptModel.Term2AvgPts = (num27.HasValue ? DataAccess.GetTranscriptAvgPoints(studentTranscriptModel.Term2Entries) : 0m);
                    studentTranscriptModel.Term3AvgPts = (num28.HasValue ? DataAccess.GetTranscriptAvgPoints(studentTranscriptModel.Term3Entries) : 0m);
                    studentTranscriptModel.Term1PtsChange = studentTranscriptModel.Term1AvgPts - studentTranscriptModel.PrevYearAvgPoints;
                    studentTranscriptModel.Term2PtsChange = studentTranscriptModel.Term2AvgPts - studentTranscriptModel.Term1AvgPts;
                    studentTranscriptModel.Term3PtsChange = studentTranscriptModel.Term3AvgPts - studentTranscriptModel.Term2AvgPts;
                    studentTranscriptModel.Term1Grade = ((studentTranscriptModel.Term1AvgPts > 0m) ? DataAccess.CalculateGradeFromPoints(studentTranscriptModel.Term1AvgPts) : "E");
                    studentTranscriptModel.Term2Grade = ((studentTranscriptModel.Term2AvgPts > 0m) ? DataAccess.CalculateGradeFromPoints(studentTranscriptModel.Term2AvgPts) : "E");
                    studentTranscriptModel.Term3Grade = ((studentTranscriptModel.Term3AvgPts > 0m) ? DataAccess.CalculateGradeFromPoints(studentTranscriptModel.Term3AvgPts) : "E");
                    studentTranscriptModel.Term1MeanScore = ((studentTranscriptModel.Term1Entries.Count > 0 && num26.HasValue) ? (num26.Value / studentTranscriptModel.Term1Entries.Count) : 0m);
                    studentTranscriptModel.Term2MeanScore = ((studentTranscriptModel.Term2Entries.Count > 0 && num27.HasValue) ? (num27.Value / studentTranscriptModel.Term2Entries.Count) : 0m);
                    studentTranscriptModel.Term3MeanScore = ((studentTranscriptModel.Term3Entries.Count > 0 && num28.HasValue) ? (num28.Value / studentTranscriptModel.Term3Entries.Count) : 0m);
                    switch (currentTerm)
                    {
                        case 1:
                            studentTranscriptModel.Entries = studentTranscriptModel.Term1Entries;
                            studentTranscriptModel.TotalMarks = (num26.HasValue ? num26.Value : 0m);
                            studentTranscriptModel.Points = studentTranscriptModel.Term1AvgPts;
                            studentTranscriptModel.MeanGrade = studentTranscriptModel.Term1Grade;
                            studentTranscriptModel.MeanScore = studentTranscriptModel.Term1MeanScore;
                            studentTranscriptModel.ClassPosition = studentTranscriptModel.Term1Pos;
                            studentTranscriptModel.OverAllPosition = studentTranscriptModel.Term1OverallPos;
                            break;
                        case 2:
                            studentTranscriptModel.Entries = studentTranscriptModel.Term2Entries;
                            studentTranscriptModel.TotalMarks = (num27.HasValue ? num27.Value : 0m);
                            studentTranscriptModel.Points = studentTranscriptModel.Term2AvgPts;
                            studentTranscriptModel.MeanGrade = studentTranscriptModel.Term2Grade;
                            studentTranscriptModel.MeanScore = studentTranscriptModel.Term2MeanScore;
                            studentTranscriptModel.ClassPosition = studentTranscriptModel.Term2Pos;
                            studentTranscriptModel.OverAllPosition = studentTranscriptModel.Term2OverallPos;
                            break;
                        case 3:
                            studentTranscriptModel.Entries = studentTranscriptModel.Term3Entries;
                            studentTranscriptModel.TotalMarks = (num28.HasValue ? num28.Value : 0m);
                            studentTranscriptModel.Points = studentTranscriptModel.Term3AvgPts;
                            studentTranscriptModel.MeanGrade = studentTranscriptModel.Term3Grade;
                            studentTranscriptModel.MeanScore = studentTranscriptModel.Term3MeanScore;
                            studentTranscriptModel.ClassPosition = studentTranscriptModel.Term3Pos;
                            studentTranscriptModel.OverAllPosition = studentTranscriptModel.Term3OverallPos;
                            break;
                    }
                    double num30 = 15.0 + num25 / (double)dataTable.Rows.Count * 85.0;
                    progressReporter.Report(new OperationProgress((int)num30, "Filling Report Forms"));
                    observableCollection.Add(studentTranscriptModel);
                    num25 += 1.0;
                }
                return observableCollection;
            });
        }

        public static Task<bool> SaveNewDiscipline(DisciplineModel discipline)
        {
            return Task.Run<bool>(delegate
            {
                string commandText = string.Concat(new object[]
                {
                    "INSERT INTO [Institution].[Discipline] (StudentID,Issue,DateAdded,SPhoto) VALUES (",
                    discipline.StudentID,
                    ",'",
                    discipline.Issue,
                    "','",
                    discipline.DateAdded.ToString("g"),
                    "',@sPhoto)"
                });
                ObservableCollection<SqlParameter> paramColl = new ObservableCollection<SqlParameter>
                {
                    new SqlParameter("@sPhoto", discipline.SPhoto)
                };
                return DataAccessHelper.ExecuteNonQueryWithParameters(commandText, paramColl);
            });
        }

        public static Task<ObservableCollection<DisciplineModel>> GetStudentDiscipline(StudentBaseModel student, DateTime? start, DateTime? end)
        {
            return Task.Run<ObservableCollection<DisciplineModel>>(delegate
            {
                ObservableCollection<DisciplineModel> observableCollection = new ObservableCollection<DisciplineModel>();
                try
                {
                    string text = "SELECT Issue,DateAdded,SPhoto FROM [Institution].[Discipline] ";
                    if (student.StudentID > 0)
                    {
                        text = text + "WHERE StudentID=" + student.StudentID;
                    }
                    if (start.HasValue && end.HasValue)
                    {
                        if (student.StudentID > 0)
                        {
                            string text2 = text;
                            text = string.Concat(new string[]
                            {
                                text2,
                                " AND DateAdded BETWEEN '",
                                start.Value.ToString("dd-MM-yyyy"),
                                " 00:00:00.000' AND '",
                                end.Value.ToString("dd-MM-yyyy"),
                                " 23:59:59.998'"
                            });
                        }
                        else
                        {
                            string text2 = text;
                            text = string.Concat(new string[]
                            {
                                text2,
                                " WHERE DateAdded BETWEEN '",
                                start.Value.ToString("dd-MM-yyyy"),
                                " 00:00:00.000' AND '",
                                end.Value.ToString("dd-MM-yyyy"),
                                " 23:59:59.998'"
                            });
                        }
                    }
                    DataTable dataTable = DataAccessHelper.ExecuteNonQueryWithResultTable(text);
                    foreach (DataRow dataRow in dataTable.Rows)
                    {
                        observableCollection.Add(new DisciplineModel
                        {
                            StudentID = student.StudentID,
                            NameOfStudent = student.NameOfStudent,
                            Issue = dataRow[0].ToString(),
                            DateAdded = DateTime.Parse(dataRow[1].ToString()),
                            SPhoto = (byte[])dataRow[2]
                        });
                    }
                }
                catch
                {
                }
                return observableCollection;
            });
        }

        public static Task<bool> SaveNewCombinedExamAsync(ExamModel newExam, CombinedClassModel selectedCombinedClass)
        {
            return Task.Run<bool>(delegate
            {
                string text = "BEGIN TRANSACTION\r\nDECLARE @id int; SET @id = dbo.GetNewID('Institution.ExamHeader')\r\n";
                foreach (ClassModel current in selectedCombinedClass.Entries)
                {
                    object obj = text;
                    text = string.Concat(new object[]
                    {
                        obj,
                        "INSERT INTO [Institution].[ExamHeader] (ExamID,ClassID,NameOfExam,ExamDateTime) VALUES (@id,",
                        current.ClassID,
                        ",'",
                        newExam.NameOfExam,
                        "','",
                        DateTime.Now.ToString("g"),
                        "')\r\nSET @id = dbo.GetNewID('Institution.ExamHeader')\r\n"
                    });
                    foreach (ExamSubjectEntryModel current2 in newExam.Entries)
                    {
                        obj = text;
                        text = string.Concat(new object[]
                        {
                            obj,
                            "INSERT INTO [Institution].[ExamDetail] (ExamID,SubjectID,ExamDateTime) VALUES (@id,",
                            current2.SubjectID,
                            ",'",
                            current2.ExamDateTime.ToString("g", new CultureInfo("en-GB")),
                            "')\r\n"
                        });
                    }
                    text += " COMMIT";
                }
                return DataAccessHelper.ExecuteNonQuery(text);
            });
        }

        public static Task<bool> SaveNewItemIssueAsync(IssueModel newIssue)
        {
            return Task.Run<bool>(delegate
            {
                string text = string.Concat(new string[]
                {
                    "BEGIN TRANSACTION\r\nDECLARE @id int; SET @id = dbo.GetNewID('Sales.ItemIssueHeader')\r\nINSERT INTO [Sales].[ItemIssueHeader] (ItemIssueID,Description,DateIssued,IsCancelled) VALUES(@id,'",
                    newIssue.Description,
                    "','",
                    newIssue.DateIssued.ToString("g"),
                    "',",
                    newIssue.IsCancelled ? "1" : "0",
                    ")"
                });
                foreach (ItemIssueModel current in newIssue.Items)
                {
                    object obj = text;
                    text = string.Concat(new object[]
                    {
                        obj,
                        "\r\nINSERT INTO [Sales].[ItemIssueDetail] (ItemIssueID,ItemID,Quantity) VALUES(@id,",
                        current.ItemID,
                        ",",
                        current.Quantity,
                        ")"
                    });
                }
                text += "\r\nCOMMIT";
                DataAccessHelper.ExecuteNonQuery(text);
                return true;
            });
        }

        public static Task<ObservableCollection<VoteHeadModel>> GetVoteHeadsSummaryByClass(int classID)
        {
            return Task.Run<ObservableCollection<VoteHeadModel>>(delegate
            {
                DateTime termStart = DataAccess.GetTermStart();
                DateTime termEnd = DataAccess.GetTermEnd();
                string commandText = string.Concat(new object[]
                {
                    "SELECT sd.Name,ISNULL(SUM(sd.Amount),0) FROM [Sales].[SaleDetail] sd LEFT OUTER JOIN [Sales].[SaleHeader] sh ON (sd.SaleID=sh.SaleID) INNER JOIN [Institution].[Student] s ON (s.StudentID=CONVERT(INT,CustomerID)) WHERE s.CLassID=",
                    classID,
                    " AND s.IsActive=1 AND sh.OrderDate BETWEEN CONVERT(datetime,'",
                    termStart.Day.ToString(),
                    "/",
                    termStart.Month.ToString(),
                    "/",
                    termStart.Year.ToString(),
                    " 00:00:00.000') AND CONVERT(datetime,'",
                    termEnd.Day.ToString(),
                    "/",
                    termEnd.Month.ToString(),
                    "/",
                    termEnd.Year.ToString(),
                    " 23:59:59.998') GROUP BY sd.Name"
                });
                DataTable dataTable = DataAccessHelper.ExecuteNonQueryWithResultTable(commandText);
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

        public static Task<bool> RemoveExam(int examID)
        {
            return Task.Run<bool>(delegate
            {
                string text = "BEGIN TRANSACTION\r\n";
                object obj = text;
                text = string.Concat(new object[]
                {
                    obj,
                    "DELETE FROM [Institution].[ExamClassDetail] WHERE ExamID=",
                    examID,
                    "\r\nDELETE FROM [Institution].[ExamResultDetail] WHERE ExamResultID IN (SELECT ExamResultID FROM [Institution].[ExamResultHeader] WHERE ExamID=",
                    examID,
                    ")\r\nDELETE FROM [Institution].[ExamResultHeader] WHERE ExamID=",
                    examID,
                    "\r\nDELETE FROM [Institution].[ExamDetail] WHERE ExamID=",
                    examID,
                    "\r\nDELETE FROM [Institution].[ExamHeader] WHERE ExamID=",
                    examID,
                    "\r\n"
                });
                text += " COMMIT";
                return DataAccessHelper.ExecuteNonQuery(text);
            });
        }

        internal static decimal ConvertScoreToOutOf(decimal score, decimal oldOutOf, decimal newOutOf)
        {
            return newOutOf / oldOutOf * score;
        }

        public static async Task<ExamResultClassModel> GetClassCombinedExamResultAsync(int classID, ObservableCollection<ExamWeightModel> exams)
        {
            ExamResultClassModel examResultClassModel = new ExamResultClassModel();
            ObservableCollection<SubjectModel> observableCollection = await DataAccess.GetSubjectsRegistredToClassAsync(classID);
            string text = "SELECT StudentID, NameOfStudent,";
            foreach (SubjectModel current in observableCollection)
            {
                text += "dbo.AddValuesIgnoringNull(";
                for (int i = 0; i < 5; i++)
                {
                    if (i < exams.Count)
                    {
                        text = string.Concat(new object[]
                        {
                            text,
                            "dbo.GetWeightedExamSubjectScore(StudentID,",
                            exams[i].ExamID,
                            ",",
                            current.SubjectID,
                            ",",
                            exams[i].Weight,
                            "),"
                        });
                    }
                    else
                    {
                        text += "NULL,";
                    }
                }
                text = text.Remove(text.Length - 1);
                text += "),";
            }
            text = text.Remove(text.Length - 1);
            text = string.Concat(new object[]
            {
                text,
                " FROM [Institution].[Student] WHERE ClassID=",
                classID,
                " AND IsACtive=1"
            });
            DataTable dataTable = DataAccessHelper.ExecuteNonQueryWithResultTable(text);
            foreach (DataRow dataRow in dataTable.Rows)
            {
                ExamResultStudentModel examResultStudentModel = new ExamResultStudentModel();
                examResultStudentModel.StudentID = int.Parse(dataRow[0].ToString());
                examResultStudentModel.NameOfStudent = dataRow[1].ToString();
                for (int i = 2; i < dataRow.ItemArray.Length; i++)
                {
                    if (!string.IsNullOrWhiteSpace(dataRow[i].ToString()))
                    {
                        ExamResultSubjectEntryModel examResultSubjectEntryModel = new ExamResultSubjectEntryModel();
                        examResultSubjectEntryModel.SubjectID = observableCollection[i - 2].SubjectID;
                        examResultSubjectEntryModel.NameOfSubject = observableCollection[i - 2].NameOfSubject;
                        examResultSubjectEntryModel.Code = observableCollection[i - 2].Code;
                        examResultSubjectEntryModel.Score = decimal.Parse(dataRow[i].ToString());
                        examResultSubjectEntryModel.MaximumScore = 100m;
                        examResultStudentModel.Entries.Add(examResultSubjectEntryModel);
                    }
                }
                examResultClassModel.Entries.Add(examResultStudentModel);
            }
            return examResultClassModel;
        }

        public static async Task<ExamResultClassModel> GetCombinedClassCombinedExamResultAsync(ObservableCollection<ClassModel> classes, ObservableCollection<ExamWeightModel> exams)
        {
            ExamResultClassModel examResultClassModel = new ExamResultClassModel();
            ObservableCollection<SubjectModel> observableCollection = await DataAccess.GetSubjectsRegistredToClassAsync(classes[0].ClassID);
            string text = "SELECT StudentID, NameOfStudent,";
            string text2 = "0,";
            foreach (ClassModel current in classes)
            {
                text2 = text2 + current.ClassID + ",";
            }
            text2 = text2.Remove(text2.Length - 1);
            foreach (SubjectModel current2 in observableCollection)
            {
                text += "dbo.AddValuesIgnoringNull(";
                for (int i = 0; i < 5; i++)
                {
                    if (i < exams.Count)
                    {
                        text = string.Concat(new object[]
                        {
                            text,
                            "dbo.GetWeightedExamSubjectScore(StudentID,",
                            exams[i].ExamID,
                            ",",
                            current2.SubjectID,
                            ",",
                            exams[i].Weight,
                            "),"
                        });
                    }
                    else
                    {
                        text += "NULL,";
                    }
                }
                text = text.Remove(text.Length - 1);
                text += "),";
            }
            text = text.Remove(text.Length - 1);
            text = text + " FROM [Institution].[Student] WHERE ClassID IN (" + text2 + ") AND IsACtive=1";
            DataTable dataTable = DataAccessHelper.ExecuteNonQueryWithResultTable(text);
            foreach (DataRow dataRow in dataTable.Rows)
            {
                ExamResultStudentModel examResultStudentModel = new ExamResultStudentModel();
                examResultStudentModel.StudentID = int.Parse(dataRow[0].ToString());
                examResultStudentModel.NameOfStudent = dataRow[1].ToString();
                for (int i = 2; i < dataRow.ItemArray.Length; i++)
                {
                    if (!string.IsNullOrWhiteSpace(dataRow[i].ToString()))
                    {
                        ExamResultSubjectEntryModel examResultSubjectEntryModel = new ExamResultSubjectEntryModel();
                        examResultSubjectEntryModel.SubjectID = observableCollection[i - 2].SubjectID;
                        examResultSubjectEntryModel.NameOfSubject = observableCollection[i - 2].NameOfSubject;
                        examResultSubjectEntryModel.Code = observableCollection[i - 2].Code;
                        examResultSubjectEntryModel.Score = decimal.Parse(dataRow[i].ToString());
                        examResultStudentModel.Entries.Add(examResultSubjectEntryModel);
                    }
                }
                examResultClassModel.Entries.Add(examResultStudentModel);
            }
            return examResultClassModel;
        }

        public static Task<ObservableCollection<StudentSubjectSelectionModel>> GetClassStudentSubjectSelection(int classID)
        {
            return Task.Run<ObservableCollection<StudentSubjectSelectionModel>>(delegate
            {
                ObservableCollection<StudentSubjectSelectionModel> observableCollection = new ObservableCollection<StudentSubjectSelectionModel>();
                ObservableCollection<SubjectModel> result = DataAccess.GetSubjectsRegistredToClassAsync(classID).Result;
                string text = "SELECT StudentID, NameOfStudent,";
                foreach (SubjectModel current in result)
                {
                    object obj = text;
                    text = string.Concat(new object[]
                    {
                        obj,
                        "dbo.GetSubjectSelection(",
                        current.SubjectID,
                        ",StudentID),"
                    });
                }
                text = text.Remove(text.Length - 1);
                text = text + " FROM [Institution].[Student] WHERE IsActive=1 AND ClassID=" + classID;
                DataTable dataTable = DataAccessHelper.ExecuteNonQueryWithResultTable(text);
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    StudentSubjectSelectionModel studentSubjectSelectionModel = new StudentSubjectSelectionModel();
                    studentSubjectSelectionModel.StudentID = int.Parse(dataRow[0].ToString());
                    studentSubjectSelectionModel.NameOfStudent = dataRow[1].ToString();
                    for (int i = 2; i < dataRow.ItemArray.Length; i++)
                    {
                        StudentSubjectSelectionEntryModel studentSubjectSelectionEntryModel = new StudentSubjectSelectionEntryModel();
                        studentSubjectSelectionEntryModel.NameOfSubject = dataTable.Columns[i].ColumnName;
                        studentSubjectSelectionEntryModel.IsSelected = bool.Parse(dataRow[i].ToString());
                        studentSubjectSelectionModel.Entries.Add(studentSubjectSelectionEntryModel);
                    }
                    observableCollection.Add(studentSubjectSelectionModel);
                }
                return observableCollection;
            });
        }

        public static Task<StudentSubjectSelectionModel> GetStudentSubjectSelection(int studentID)
        {
            return Task.Run<StudentSubjectSelectionModel>(delegate
            {
                StudentSubjectSelectionModel studentSubjectSelectionModel = new StudentSubjectSelectionModel();
                string commandText = "SELECT sub.NameOfSubject,sssd.SubjectID FROM [Institution].[StudentSubjectSelectionDetail] sssd LEFT OUTER JOIN [Institution].[StudentSubjectSelectionHeader] sssh ON(sssd.StudentSubjectSelectionID = sssh.StudentSubjectSelectionID) LEFT OUTER JOIN [Institution].[Subject] sub ON(sssd.SubjectID=sub.SubjectID) WHERE sssh.IsActive=1 AND sssh.StudentID=" + studentID + " ORDER BY sub.[Code]";
                DataTable dataTable = DataAccessHelper.ExecuteNonQueryWithResultTable(commandText);
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    StudentSubjectSelectionEntryModel studentSubjectSelectionEntryModel = new StudentSubjectSelectionEntryModel();
                    studentSubjectSelectionEntryModel.NameOfSubject = dataRow[0].ToString();
                    studentSubjectSelectionEntryModel.SubjectID = int.Parse(dataRow[1].ToString());
                    studentSubjectSelectionModel.Entries.Add(studentSubjectSelectionEntryModel);
                }
                return studentSubjectSelectionModel;
            });
        }

        public static Task<bool> SaveNewSubjectSelection(ObservableCollection<StudentSubjectSelectionModel> subjectSelections)
        {
            return Task.Run<bool>(delegate
            {
                bool flag = true;
                foreach (StudentSubjectSelectionModel current in subjectSelections)
                {
                    string text = "BEGIN TRANSACTION\r\nDECLARE @id int; \r\n ";
                    string text2 = "0,";
                    foreach (StudentSubjectSelectionEntryModel current2 in current.Entries)
                    {
                        text2 = text2 + current2.SubjectID + ",";
                    }
                    text2 = text2.Remove(text2.Length - 1);
                    text2 = (text2 ?? "");
                    text += "SET @id = dbo.GetNewID('Institution.StudentSubjectSelectionHeader')\r\n";
                    object obj = text;
                    text = string.Concat(new object[]
                    {
                        obj,
                        "IF NOT EXISTS (SELECT * FROM [Institution].[StudentSubjectSelectionHeader] WHERE IsActive=1 AND StudentID=",
                        current.StudentID,
                        ")\r\n"
                    });
                    obj = text;
                    text = string.Concat(new object[]
                    {
                        obj,
                        "INSERT INTO [Institution].[StudentSubjectSelectionHeader] (StudentSubjectSelectionID,StudentID,IsActive) VALUES (@id,",
                        current.StudentID,
                        ",1)\r\n"
                    });
                    obj = text;
                    text = string.Concat(new object[]
                    {
                        obj,
                        "ELSE SET @id=(SELECT StudentSubjectSelectionID FROM [Institution].[StudentSubjectSelectionHeader] WHERE StudentID=",
                        current.StudentID,
                        " AND IsActive=1)\r\n"
                    });
                    foreach (StudentSubjectSelectionEntryModel current3 in current.Entries)
                    {
                        obj = text;
                        text = string.Concat(new object[]
                        {
                            obj,
                            "IF NOT EXISTS (SELECT * FROM [Institution].[StudentSubjectSelectionDetail] WHERE StudentSubjectSelectionID=@id AND SubjectID=",
                            current3.SubjectID,
                            ")\r\n"
                        });
                        obj = text;
                        text = string.Concat(new object[]
                        {
                            obj,
                            "INSERT INTO [Institution].[StudentSubjectSelectionDetail] (StudentSubjectSelectionID,SubjectID) VALUES (@id,",
                            current3.SubjectID,
                            ")\r\n"
                        });
                    }
                    text = text + "DELETE FROM [Institution].[StudentSubjectSelectionDetail] WHERE StudentSubjectSelectionID=@id AND SubjectID NOT IN (" + text2 + ")\r\n";
                    text += " COMMIT";
                    flag = (flag && DataAccessHelper.ExecuteNonQuery(text));
                }
                return flag;
            });
        }

        public static Task<bool> SaveNewSubjectSelection(StudentSubjectSelectionModel subjectSelection)
        {
            return Task.Run<bool>(delegate
            {
                string text = "BEGIN TRANSACTION\r\nDECLARE @id int; \r\n ";
                string text2 = "0,";
                foreach (StudentSubjectSelectionEntryModel current in subjectSelection.Entries)
                {
                    text2 = text2 + current.SubjectID + ",";
                }
                text2 = text2.Remove(text2.Length - 1);
                text += "SET @id = dbo.GetNewID('Institution.StudentSubjectSelectionHeader')\r\n";
                object obj = text;
                text = string.Concat(new object[]
                {
                    obj,
                    "IF NOT EXISTS (SELECT * FROM [Institution].[StudentSubjectSelectionHeader] WHERE IsActive=1 AND StudentID=",
                    subjectSelection.StudentID,
                    ")\r\n"
                });
                obj = text;
                text = string.Concat(new object[]
                {
                    obj,
                    "INSERT INTO [Institution].[StudentSubjectSelectionHeader] (StudentSubjectSelectionID,StudentID,IsActive) VALUES (@id,",
                    subjectSelection.StudentID,
                    ",1)\r\n"
                });
                obj = text;
                text = string.Concat(new object[]
                {
                    obj,
                    "ELSE SET @id=(SELECT StudentSubjectSelectionID FROM [Institution].[StudentSubjectSelectionHeader] WHERE StudentID=",
                    subjectSelection.StudentID,
                    " AND IsActive=1)\r\n"
                });
                foreach (StudentSubjectSelectionEntryModel current2 in subjectSelection.Entries)
                {
                    obj = text;
                    text = string.Concat(new object[]
                    {
                        obj,
                        "IF NOT EXISTS (SELECT * FROM [Institution].[StudentSubjectSelectionDetail] WHERE StudentSubjectSelectionID=@id AND SubjectID=",
                        current2.SubjectID,
                        ")\r\n"
                    });
                    obj = text;
                    text = string.Concat(new object[]
                    {
                        obj,
                        "INSERT INTO [Institution].[StudentSubjectSelectionDetail] (StudentSubjectSelectionID,SubjectID) VALUES (@id,",
                        current2.SubjectID,
                        ")\r\n"
                    });
                }
                text = text + "DELETE FROM [Institution].[StudentSubjectSelectionDetail] WHERE StudentSubjectSelectionID=@id AND SubjectID NOT IN (" + text2 + ")\r\n";
                text += " COMMIT";
                return DataAccessHelper.ExecuteNonQuery(text);
            });
        }

        public static Task<TimeTableSettingsModel> GetCurrentTimeTableSettings()
        {
            return Task.Run<TimeTableSettingsModel>(delegate
            {
                TimeTableSettingsModel timeTableSettingsModel = new TimeTableSettingsModel();
                string commandText = "SELECT TimeTableSettingsID,NoOfLessons,LessonDuration,LessonsStartTime,BreakIndices,BreakDuration FROM [Institution].[TimeTableSettings] WHERE IsActive=1";
                DataTable dataTable = DataAccessHelper.ExecuteNonQueryWithResultTable(commandText);
                TimeTableSettingsModel result;
                if (dataTable.Rows.Count == 0)
                {
                    result = timeTableSettingsModel;
                }
                else
                {
                    timeTableSettingsModel.TimeTableSettingsID = int.Parse(dataTable.Rows[0][0].ToString());
                    timeTableSettingsModel.NoOfLessons = int.Parse(dataTable.Rows[0][1].ToString());
                    timeTableSettingsModel.LessonDuration = TimeSpan.Parse(dataTable.Rows[0][2].ToString());
                    timeTableSettingsModel.LessonsStartTime = TimeSpan.Parse(dataTable.Rows[0][3].ToString());
                    List<int> list = new List<int>();
                    List<TimeSpan> list2 = new List<TimeSpan>();
                    string text = dataTable.Rows[0][4].ToString();
                    string[] array = text.Split(new char[]
                    {
                        ','
                    });
                    string[] array2 = array;
                    for (int i = 0; i < array2.Length; i++)
                    {
                        string s = array2[i];
                        list.Add(int.Parse(s));
                    }
                    string text2 = dataTable.Rows[0][5].ToString();
                    array2 = array;
                    for (int i = 0; i < array2.Length; i++)
                    {
                        string s = array2[i];
                        list2.Add(TimeSpan.Parse(s));
                    }
                    string[] array3 = text2.Split(new char[]
                    {
                        ','
                    });
                    timeTableSettingsModel.BreakIndices = list;
                    timeTableSettingsModel.BreakDurations = list2;
                    result = timeTableSettingsModel;
                }
                return result;
            });
        }

        public static Task<TimeTableModel> GetCurrentTimeTableFull()
        {
            return Task.Run<TimeTableModel>(delegate
            {
                TimeTableModel timeTableModel = new TimeTableModel();
                ObservableCollection<ClassModel> result = DataAccess.GetAllClassesAsync().Result;
                TimeTableSettingsModel timeTableSettingsModel = new TimeTableSettingsModel();
                int num = 10;
                foreach (ClassModel current in result)
                {
                    ClassLessons classLessons = new ClassLessons
                    {
                        NameOfClass = current.NameOfClass,
                        ClassID = current.ClassID,
                        Day = DayOfWeek.Monday
                    };
                    for (int i = 0; i < num; i++)
                    {
                        classLessons.Add(new Lesson
                        {
                            Subject = "",
                            SubjectIndex = i
                        });
                    }
                    timeTableModel.Add(classLessons);
                }
                foreach (ClassModel current in result)
                {
                    ClassLessons classLessons = new ClassLessons
                    {
                        NameOfClass = current.NameOfClass,
                        ClassID = current.ClassID,
                        Day = DayOfWeek.Tuesday
                    };
                    for (int i = 0; i < num; i++)
                    {
                        classLessons.Add(new Lesson
                        {
                            Subject = "",
                            SubjectIndex = i
                        });
                    }
                    timeTableModel.Add(classLessons);
                }
                foreach (ClassModel current in result)
                {
                    ClassLessons classLessons = new ClassLessons
                    {
                        NameOfClass = current.NameOfClass,
                        ClassID = current.ClassID,
                        Day = DayOfWeek.Wednesday
                    };
                    for (int i = 0; i < num; i++)
                    {
                        classLessons.Add(new Lesson
                        {
                            Subject = "",
                            SubjectIndex = i
                        });
                    }
                    timeTableModel.Add(classLessons);
                }
                foreach (ClassModel current in result)
                {
                    ClassLessons classLessons = new ClassLessons
                    {
                        NameOfClass = current.NameOfClass,
                        ClassID = current.ClassID,
                        Day = DayOfWeek.Thursday
                    };
                    for (int i = 0; i < num; i++)
                    {
                        classLessons.Add(new Lesson
                        {
                            Subject = "",
                            SubjectIndex = i
                        });
                    }
                    timeTableModel.Add(classLessons);
                }
                foreach (ClassModel current in result)
                {
                    ClassLessons classLessons = new ClassLessons
                    {
                        NameOfClass = current.NameOfClass,
                        ClassID = current.ClassID,
                        Day = DayOfWeek.Friday
                    };
                    for (int i = 0; i < num; i++)
                    {
                        classLessons.Add(new Lesson
                        {
                            Subject = "",
                            SubjectIndex = i
                        });
                    }
                    timeTableModel.Add(classLessons);
                }
                foreach (ClassModel current in result)
                {
                    ClassLessons classLessons = new ClassLessons
                    {
                        NameOfClass = current.NameOfClass,
                        ClassID = current.ClassID,
                        Day = DayOfWeek.Saturday
                    };
                    for (int i = 0; i < num; i++)
                    {
                        classLessons.Add(new Lesson
                        {
                            Subject = "",
                            SubjectIndex = i
                        });
                    }
                    timeTableModel.Add(classLessons);
                }
                timeTableModel.Settings = DataAccess.GetCurrentTimeTableSettings().Result;
                string commandText = "SELECT tth.ClassID,ttd.NameOfSubject,ttd.Tutor,ttd.[Day],ttd.StartTime,ttd.EndTime,ttd.SubjectIndex FROM [Institution].[TimeTableDetail] ttd INNER JOIN [Institution].[TimeTableHeader] tth ON (ttd.TimeTableID= tth.TimeTableID) WHERE tth.IsActive = 1";
                DataTable dataTable = DataAccessHelper.ExecuteNonQueryWithResultTable(commandText);
                if (dataTable.Rows.Count > 0)
                {
                    foreach (ClassLessons current2 in timeTableModel)
                    {
                        current2.Clear();
                    }
                }
                IEnumerator enumerator3 = dataTable.Rows.GetEnumerator();
                try
                {
                    while (enumerator3.MoveNext())
                    {
                        DataRow dtr = (DataRow)enumerator3.Current;
                        Lesson lesson = new Lesson();
                        lesson.Subject = dtr[1].ToString();
                        lesson.Tutor = dtr[2].ToString();
                        lesson.StartTime = TimeSpan.Parse(dtr[4].ToString());
                        lesson.EndTime = TimeSpan.Parse(dtr[5].ToString());
                        lesson.SubjectIndex = int.Parse(dtr[6].ToString());
                        (from o in timeTableModel
                         where o.ClassID == int.Parse(dtr[0].ToString()) && o.Day == (DayOfWeek)Enum.Parse(typeof(DayOfWeek), dtr[3].ToString())
                         select o).ElementAt(0).Add(lesson);
                    }
                }
                finally
                {
                    IDisposable disposable = enumerator3 as IDisposable;
                    if (disposable != null)
                    {
                        disposable.Dispose();
                    }
                }
                return timeTableModel;
            });
        }

        public static Task<bool> SaveNewSubjectSetupAsync(int classID, ObservableCollection<SubjectModel> subjects)
        {
            return Task.Run<bool>(delegate
            {
                string text = "BEGIN TRANSACTION\r\ndeclare @id int; declare @id2 int;\r\n";
                object obj = text;
                text = string.Concat(new object[]
                {
                    obj,
                    "IF NOT EXISTS (SELECT * FROM [Institution].[SubjectSetupHeader] WHERE IsActive=1 AND ClassID=",
                    classID,
                    ")BEGIN SET @id = [dbo].GetNewID('Institution.SubjectSetupHeader') INSERT INTO [Institution].[SubjectSetupHeader] (SubjectSetupID,ClassID,StartDate) VALUES (@id,",
                    classID,
                    ",'",
                    DateTime.Now.ToString("g"),
                    "')\r\nEND\r\nELSE SET @id=(SELECT SubjectSetupID FROM [Institution].[SubjectSetupHeader] WHERE IsActive=1 AND ClassID=",
                    classID,
                    ")\r\n"
                });
                foreach (SubjectModel current in subjects)
                {
                    string text2 = text;
                    text = string.Concat(new string[]
                    {
                        text2,
                        "SET @id2 = (SELECT SubjectID FROM [Institution].[Subject] WHERE NameOfSubject='",
                        current.NameOfSubject,
                        "')\r\nIF NOT EXISTS (SELECT * FROM [Institution].[SubjectSetupDetail] ssd INNER JOIN [Institution].[Subject] s ON(ssd.SubjectID=s.SubjectID)  WHERE s.NameOfSubject='",
                        current.NameOfSubject,
                        "' AND ssd.SubjectSetupID=@id)\r\nINSERT INTO [Institution].[SubjectSetupDetail] (SubjectSetupID,SubjectID,Tutor) VALUES (@id,@id2,'",
                        current.Tutor,
                        "')\r\nELSE\r\nUPDATE [Institution].[SubjectSetupDetail] SET Tutor='",
                        current.Tutor,
                        "' WHERE SubjectSetupID=@id AND SubjectID=@id2\r\n"
                    });
                }
                text += " COMMIT";
                return DataAccessHelper.ExecuteNonQuery(text);
            });
        }

        public static Task<ObservableCollection<LeavingCertificateModel>> GetClassLeavingCerts(ObservableCollection<ClassModel> classes)
        {
            return Task.Run<ObservableCollection<LeavingCertificateModel>>(delegate
            {
                ObservableCollection<LeavingCertificateModel> observableCollection = new ObservableCollection<LeavingCertificateModel>();
                string text = "0,";
                foreach (ClassModel current in classes)
                {
                    text = text + current.ClassID + ",";
                }
                text = text.Remove(text.Length - 1);
                string commandText = "SELECT l.DateOfIssue,l.DateOfBirth,l.DateOfAdmission,l.DateOfLeaving,l.Nationality,l.ClassEntered,l.ClassLeft,l.Remarks,s.StudentID,s.NameOfStudent FROM [Institution].[LeavingCertificate] l LEFT OUTER JOIN [Institution].[Student] s  ON (l.StudentID=s.StudentID) WHERE s.ClassID IN (" + text + ")";
                DataTable dataTable = DataAccessHelper.ExecuteNonQueryWithResultTable(commandText);
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    observableCollection.Add(new LeavingCertificateModel
                    {
                        StudentID = int.Parse(dataTable.Rows[0][8].ToString()),
                        NameOfStudent = dataTable.Rows[0][9].ToString(),
                        DateOfIssue = DateTime.Parse(dataTable.Rows[0][0].ToString()),
                        DateOfBirth = DateTime.Parse(dataTable.Rows[0][1].ToString()),
                        DateOfAdmission = DateTime.Parse(dataTable.Rows[0][2].ToString()),
                        DateOfLeaving = DateTime.Parse(dataTable.Rows[0][3].ToString()),
                        Nationality = dataTable.Rows[0][4].ToString(),
                        ClassEntered = dataTable.Rows[0][5].ToString(),
                        ClassLeft = dataTable.Rows[0][6].ToString(),
                        Remarks = dataTable.Rows[0][7].ToString()
                    });
                }
                return observableCollection;
            });
        }

        public static Task<bool> SaveNewInstitutionSubjectSetup(ObservableCollection<SubjectModel> selectedSubjects)
        {
            return Task.Run<bool>(delegate
            {
                string text = "BEGIN TRANSACTION\r\n";
                string text2 = "'0',";
                foreach (SubjectModel current in selectedSubjects)
                {
                    text2 = text2 + "'" + current.NameOfSubject + "',";
                }
                text2 = text2.Remove(text2.Length - 1);
                foreach (SubjectModel current in from o in selectedSubjects
                                                 where o.SubjectID == 0
                                                 select o)
                {
                    object obj = text;
                    text = string.Concat(new object[]
                    {
                        obj,
                        "IF NOT EXISTS (SELECT * FROM[Institution].[Subject] WHERE NameOfSubject='",
                        current.NameOfSubject,
                        "')INSERT INTO [Institution].[Subject] (SubjectID, NameOfSubject, Code, MaximumScore, IsOptional) VALUES(dbo.GetNewID('Institution.Subject'), '",
                        current.NameOfSubject,
                        "',",
                        current.Code,
                        ",",
                        current.MaximumScore,
                        ",",
                        current.IsOptional ? "1" : "0",
                        ")\r\n"
                    });
                }
                text = text + "DELETE FROM [Institution].[Subject] WHERE NameOfSubject NOT IN (" + text2 + ")\r\n";
                text += "COMMIT";
                return DataAccessHelper.ExecuteNonQuery(text);
            });
        }

        public static Task<ObservableCollection<SubjectModel>> GetSubjectsRegistredToInstitutionAsync()
        {
            return Task.Run<ObservableCollection<SubjectModel>>(delegate
            {
                ObservableCollection<SubjectModel> observableCollection = new ObservableCollection<SubjectModel>();
                string commandText = "SELECT SubjectID,NameOfSubject,Code,IsOptional FROM [Institution].[Subject] ORDER BY Code";
                DataTable dataTable = DataAccessHelper.ExecuteNonQueryWithResultTable(commandText);
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    observableCollection.Add(new SubjectModel
                    {
                        SubjectID = (int)dataRow[0],
                        NameOfSubject = dataRow[1].ToString(),
                        Tutor = "",
                        MaximumScore = 100m,
                        Code = int.Parse(dataRow[2].ToString()),
                        IsOptional = bool.Parse(dataRow[3].ToString())
                    });
                }
                return observableCollection;
            });
        }

        public static Task<int> GetLastPaymentVoucherIDAsync(string payee, string description)
        {
            return Task.Run<int>(delegate
            {
                string commandText = "SELECT TOP 1 PayoutID FROM [Institution].[PayoutHeader] WHERE Payee=@payee AND Description=@description";
                DataTable dataTable = DataAccessHelper.ExecuteNonQueryWithParametersWithResultTable(commandText, new ObservableCollection<SqlParameter>
                {
                    new SqlParameter("@payee", payee),
                    new SqlParameter("@description", description)
                });
                int result;
                if (dataTable.Rows.Count > 0)
                {
                    result = int.Parse(dataTable.Rows[0][0].ToString());
                }
                else
                {
                    result = 0;
                }
                return result;
            });
        }

        public static Task<bool> SaveNewPayslip(PayslipModel newSlip)
        {
            return Task.Run<bool>(delegate
            {
                string text = "BEGIN TRANSACTION\r\nDECLARE @id INT; SET @id=dbo.GetNewID('Institution.PayslipHeader')INSERT INTO [Institution].[PayslipHeader] (PayslipID,StaffID,AmountPaid,DatePaid,Designation) VALUES(@id,@staffID,@amountPaid,@datePaid,@designation)\r\n";
                ObservableCollection<SqlParameter> observableCollection = new ObservableCollection<SqlParameter>();
                observableCollection.Add(new SqlParameter("@staffID", newSlip.StaffID));
                observableCollection.Add(new SqlParameter("@designation", newSlip.Designation));
                observableCollection.Add(new SqlParameter("@amountPaid", newSlip.AmountPaid));
                observableCollection.Add(new SqlParameter("@datePaid", newSlip.DatePaid.ToString("g")));
                int num = 0;
                foreach (FeesStructureEntryModel current in newSlip.Entries)
                {
                    string text2 = "@desc" + num;
                    string text3 = "@amt" + num;
                    string text4 = text;
                    text = string.Concat(new string[]
                    {
                        text4,
                        "\r\nINSERT INTO [Institution].[PayslipDetail] (PayslipID,Description,Amount) VALUES(@id,",
                        text2,
                        ",",
                        text3,
                        ")"
                    });
                    observableCollection.Add(new SqlParameter(text2, current.Name));
                    observableCollection.Add(new SqlParameter(text3, current.Amount));
                    num++;
                }
                text += "\r\nCOMMIT";
                return DataAccessHelper.ExecuteNonQueryWithParameters(text, observableCollection);
            });
        }

        public static Task<ObservableCollection<PayslipModel>> GetRecentPayslipsAsync(StaffSelectModel selectedStaff)
        {
            return Task.Run<ObservableCollection<PayslipModel>>(delegate
            {
                ObservableCollection<PayslipModel> observableCollection = new ObservableCollection<PayslipModel>();
                string commandText = "SELECT TOP 20 PayslipID,AmountPaid, DatePaid,Designation FROM [Institution].[PayslipHeader] WHERE StaffID =" + selectedStaff.StaffID + " ORDER BY [DatePaid] desc";
                DataTable dataTable = DataAccessHelper.ExecuteNonQueryWithResultTable(commandText);
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    PayslipModel payslipModel = new PayslipModel();
                    payslipModel.PayslipID = int.Parse(dataRow[0].ToString());
                    payslipModel.AmountPaid = decimal.Parse(dataRow[1].ToString());
                    payslipModel.StaffID = selectedStaff.StaffID;
                    payslipModel.Name = selectedStaff.Name;
                    payslipModel.DatePaid = DateTime.Parse(dataRow[2].ToString());
                    payslipModel.Designation = dataRow[3].ToString();
                    payslipModel.Entries = DataAccess.GetPayslipEntries(payslipModel.PayslipID);
                    observableCollection.Add(payslipModel);
                }
                return observableCollection;
            });
        }

        private static ObservableCollection<FeesStructureEntryModel> GetPayslipEntries(int paySlipID)
        {
            ObservableCollection<FeesStructureEntryModel> observableCollection = new ObservableCollection<FeesStructureEntryModel>();
            string commandText = "SELECT [Description], [Amount] FROM [Institution].[PayslipDetail] WHERE PaySlipID =" + paySlipID;
            DataTable dataTable = DataAccessHelper.ExecuteNonQueryWithResultTable(commandText);
            foreach (DataRow dataRow in dataTable.Rows)
            {
                observableCollection.Add(new FeesStructureEntryModel
                {
                    Amount = decimal.Parse(dataRow[1].ToString()),
                    Name = dataRow[0].ToString()
                });
            }
            return observableCollection;
        }

        internal static Task<ObservableCollection<SaleModel>> GetAllSalesAsync()
        {
            throw new NotImplementedException();
        }

        internal static Task<object> GetAllFinanceAsync()
        {
            throw new NotImplementedException();
        }

        public static Task<bool> SaveNewProject(ProjectModel newProject)
        {
            return Task.Run<bool>(delegate
            {
                string text = "BEGIN TRANSACTION\r\nDECLARE @id INT; SET @id=dbo.GetNewID('Institution.ProjectHeader')INSERT INTO [Institution].[ProjectHeader] ([ProjectID],[NameOfProject],[StartDateTime],[EndDateTime],[Budget],[Description]) VALUES(@id,@nameOfProject,@starts,@ends,@budget,@description1)\r\n";
                ObservableCollection<SqlParameter> observableCollection = new ObservableCollection<SqlParameter>();
                observableCollection.Add(new SqlParameter("@nameOfProject", newProject.Name));
                observableCollection.Add(new SqlParameter("@starts", newProject.StartDate.ToString("g")));
                observableCollection.Add(new SqlParameter("@ends", newProject.EndDate.ToString("g")));
                observableCollection.Add(new SqlParameter("@budget", newProject.Budget));
                observableCollection.Add(new SqlParameter("@description1", newProject.Description));
                int num = 0;
                foreach (ProjectDetailModel current in newProject.Tasks)
                {
                    string text2 = "@name" + num;
                    string text3 = "@allocation" + num;
                    string text4 = "@starts" + num;
                    string text5 = "@ends" + num;
                    string text6 = text;
                    text = string.Concat(new string[]
                    {
                        text6,
                        "\r\nINSERT INTO [Institution].[ProjectDetail] (ProjectID,Name,Allocation,StartDate,EndDate) VALUES(@id,",
                        text2,
                        ",",
                        text3,
                        ",",
                        text4,
                        ",",
                        text5,
                        ")"
                    });
                    observableCollection.Add(new SqlParameter(text2, current.Name));
                    observableCollection.Add(new SqlParameter(text3, current.Allocation));
                    observableCollection.Add(new SqlParameter(text4, current.StartDate.ToString("g")));
                    observableCollection.Add(new SqlParameter(text5, current.EndDate.ToString("g")));
                    num++;
                }
                text += "\r\nCOMMIT";
                return DataAccessHelper.ExecuteNonQueryWithParameters(text, observableCollection);
            });
        }

        public static Task<ObservableCollection<ProjectBaseModel>> GetAllProjectsDisplay()
        {
            return Task.Run<ObservableCollection<ProjectBaseModel>>(() => DataAccess.GetProjectsDisplay(new DateTime(2015, 1, 1), DateTime.Now.AddDays(1.0)));
        }

        public static Task<ObservableCollection<ProjectListModel>> GetAllProjects()
        {
            return Task.Run<ObservableCollection<ProjectListModel>>(() => DataAccess.GetProjects(new DateTime(2015, 1, 1), DateTime.Now.AddDays(1.0)));
        }

        private static ObservableCollection<ProjectListModel> GetProjects(DateTime startDate, DateTime endDate)
        {
            ObservableCollection<ProjectListModel> observableCollection = new ObservableCollection<ProjectListModel>();
            string commandText = string.Concat(new object[]
            {
                "SELECT p.ProjectID, p.NameOfProject,p.Budget, ISNULL(SUM(CONVERT(decimal(18,0),pd.Allocation)),0) FROM [Institution].[ProjectHeader] p LEFT OUTER JOIN [Institution].[ProjectDetail]pd ON (p.ProjectID=pd.ProjectID) WHERE p.StartDateTime >= CONVERT(datetime,'",
                startDate.Day,
                "/",
                startDate.Month,
                "/",
                startDate.Year,
                " 00:00:00.000') AND p.EndDateTime<= CONVERT(datetime,'",
                endDate.Day,
                "/",
                endDate.Month,
                "/",
                endDate.Year,
                " 23:59:59.998') GROUP BY p.ProjectID, p.NameOfProject,p.Budget"
            });
            DataTable dataTable = DataAccessHelper.ExecuteNonQueryWithResultTable(commandText);
            foreach (DataRow dataRow in dataTable.Rows)
            {
                observableCollection.Add(new ProjectListModel
                {
                    ProjectID = int.Parse(dataRow[0].ToString()),
                    Name = dataRow[1].ToString(),
                    Budget = decimal.Parse(dataRow[2].ToString()),
                    CurrentAllocation = decimal.Parse(dataRow[3].ToString())
                });
            }
            return observableCollection;
        }

        private static ObservableCollection<ProjectBaseModel> GetProjectsDisplay(DateTime startDate, DateTime endDate)
        {
            ObservableCollection<ProjectBaseModel> observableCollection = new ObservableCollection<ProjectBaseModel>();
            string commandText = string.Concat(new object[]
            {
                "SELECT [ProjectID], [NameOfProject] FROM [Institution].[ProjectHeader] WHERE [StartDateTime] >= CONVERT(datetime,'",
                startDate.Day,
                "/",
                startDate.Month,
                "/",
                startDate.Year,
                " 00:00:00.000') AND [EndDateTime]<= CONVERT(datetime,'",
                endDate.Day,
                "/",
                endDate.Month,
                "/",
                endDate.Year,
                " 23:59:59.998')"
            });
            DataTable dataTable = DataAccessHelper.ExecuteNonQueryWithResultTable(commandText);
            foreach (DataRow dataRow in dataTable.Rows)
            {
                observableCollection.Add(new ProjectBaseModel
                {
                    ProjectID = int.Parse(dataRow[0].ToString()),
                    Name = dataRow[1].ToString()
                });
            }
            return observableCollection;
        }

        public static Task<ObservableCollection<ProjectTaskModel>> GetProjectTasksAsync(int projectID)
        {
            return Task.Run<ObservableCollection<ProjectTaskModel>>(delegate
            {
                ObservableCollection<ProjectTaskModel> observableCollection = new ObservableCollection<ProjectTaskModel>();
                string commandText = "SELECT ProjectDetailID,[Name],[Allocation],StartDate,EndDate FROM [Institution].[ProjectDetail] WHERE ProjectID=" + projectID;
                DataTable dataTable = DataAccessHelper.ExecuteNonQueryWithResultTable(commandText);
                if (dataTable.Rows.Count > 0)
                {
                    foreach (DataRow dataRow in dataTable.Rows)
                    {
                        observableCollection.Add(new ProjectTaskModel
                        {
                            TaskID = int.Parse(dataRow[0].ToString()),
                            NameOfTask = dataRow[1].ToString(),
                            Allocation = decimal.Parse(dataRow[2].ToString()),
                            StartDate = DateTime.Parse(dataRow[3].ToString()),
                            EndDate = DateTime.Parse(dataRow[4].ToString())
                        });
                    }
                }
                return observableCollection;
            });
        }

        public static Task<bool> SaveNewProjectTimeLineAsync(int projectID, ObservableCollection<ProjectTaskModel> allTasks)
        {
            return Task.Run<bool>(delegate
            {
                string text = "DELETE FROM [Institution].[ProjectDetail] WHERE ProjectID=" + projectID;
                bool flag = DataAccessHelper.ExecuteNonQuery(text);
                ObservableCollection<SqlParameter> observableCollection = new ObservableCollection<SqlParameter>();
                text = "";
                for (int i = 0; i < allTasks.Count; i++)
                {
                    object obj = text;
                    text = string.Concat(new object[]
                    {
                        obj,
                        "INSERT INTO [Institution].[ProjectDetail] (ProjectID,[Name],Allocation,StartDate,EndDate) VALUES(@projID,@nam",
                        i,
                        ",@all",
                        i,
                        ",@startd",
                        i,
                        ",@endd",
                        i,
                        ")\r\n"
                    });
                    observableCollection.Add(new SqlParameter("@nam" + i, allTasks[i].NameOfTask));
                    observableCollection.Add(new SqlParameter("@all" + i, allTasks[i].Allocation));
                    observableCollection.Add(new SqlParameter("@startd" + i, allTasks[i].StartDate.ToString("g")));
                    observableCollection.Add(new SqlParameter("@endd" + i, allTasks[i].EndDate.ToString("g")));
                }
                observableCollection.Add(new SqlParameter("@projID", projectID));
                return flag && DataAccessHelper.ExecuteNonQueryWithParameters(text, observableCollection);
            });
        }

        public static Task<IncomeStatementModel> GetIncomeStatement(DateTime from, DateTime to)
        {
            return Task.Run<IncomeStatementModel>(() =>
            {
                IncomeStatementModel temp = new IncomeStatementModel();
                temp.StartTime = from;
                temp.EndTime = to;
                temp.ExpenseEntries = GetISExpenses(from, to);
                temp.RevenueEntries = GetISRevenues(from, to);
                temp.GainEntries = GetISGains(from, to);
                temp.LossEntries = GetISLosses(from, to);
                return temp;
            });
        }

        private static List<TransactionModel> GetISExpenses(DateTime startTime, DateTime endTime)
        {
            ///PARYROLL
            ///POSTAGE & PRINNTING
            ///MINTENANCE
            ///OTHER
            ///COGS
            List<TransactionModel> temp = new List<TransactionModel>();
            string selectstr = "SELECT ISNULL(SUM(AmountPaid),0) FROM [Institution].[PayslipHeader] WHERE DatePaid BETWEEN CONVERT(datetime,'"+
                        startTime.Day.ToString()+
                        "-"+
                        startTime.Month.ToString()+
                        "-"+
                        startTime.Year.ToString()+
                        " 00:00:00.000') AND convert(datetime,'"+
                        endTime.Day.ToString()+
                        "-"+
                        endTime.Month.ToString()+
                        "-"+
                        endTime.Year.ToString()+
                        " 23:59:59.998')";


            decimal payroll = decimal.Parse(DataAccessHelper.ExecuteScalar(selectstr));
            temp.Add(new TransactionModel(){TransactionID="PAYROLL", TransactionAmt=payroll});

            selectstr = "SELECT ISNULL(SUM(CONVERT(decimal,TotalPaid)),0) FROM [Institution].[PayoutHeader] WHERE Category='Postage & Printing' AND ISNULL(DatePaid,ModifiedDate) BETWEEN CONVERT(datetime,'" +
                        startTime.Day.ToString() +
                        "-" +
                        startTime.Month.ToString() +
                        "-" +
                        startTime.Year.ToString() +
                        " 00:00:00.000') AND convert(datetime,'" +
                        endTime.Day.ToString() +
                        "-" +
                        endTime.Month.ToString() +
                        "-" +
                        endTime.Year.ToString() +
                        " 23:59:59.998')";

            decimal printing = decimal.Parse(DataAccessHelper.ExecuteScalar(selectstr));
            temp.Add(new TransactionModel() { TransactionID = "PRINTING", TransactionAmt = printing });

            selectstr = "SELECT ISNULL(SUM(CONVERT(decimal,TotalPaid)),0) FROM [Institution].[PayoutHeader] WHERE Category='Equipment Maintenance' AND ISNULL(DatePaid,ModifiedDate) BETWEEN CONVERT(datetime,'" +
                        startTime.Day.ToString() +
                        "-" +
                        startTime.Month.ToString() +
                        "-" +
                        startTime.Year.ToString() +
                        " 00:00:00.000') AND convert(datetime,'" +
                        endTime.Day.ToString() +
                        "-" +
                        endTime.Month.ToString() +
                        "-" +
                        endTime.Year.ToString() +
                        " 23:59:59.998')";
            decimal maintenance = decimal.Parse(DataAccessHelper.ExecuteScalar(selectstr));
            temp.Add(new TransactionModel() { TransactionID = "MAINTENANCE", TransactionAmt = maintenance });

            selectstr = "SELECT ISNULL(SUM(CONVERT(decimal,TotalPaid)),0) FROM [Institution].[PayoutHeader] WHERE Category='Other' AND ISNULL(DatePaid,ModifiedDate) BETWEEN CONVERT(datetime,'" +
                       startTime.Day.ToString() +
                       "-" +
                       startTime.Month.ToString() +
                       "-" +
                       startTime.Year.ToString() +
                       " 00:00:00.000') AND convert(datetime,'" +
                       endTime.Day.ToString() +
                       "-" +
                       endTime.Month.ToString() +
                       "-" +
                       endTime.Year.ToString() +
                       " 23:59:59.998')";
            decimal other1 = decimal.Parse(DataAccessHelper.ExecuteScalar(selectstr));
            

            selectstr = "SELECT ISNULL(SUM(ird.LineTotal),0) FROM [Sales].[ItemReceiptHeader] irh LEFT OUTER JOIN [Sales].[ItemReceiptDetail] ird ON "+
                "(irh.ItemReceiptID = ird.ItemReceiptID) LEFT OUTER JOIN [Sales].[Item] i ON (i.ItemID = ird.ItemID) LEFT OUTER JOIN [Sales].[ItemCategory] "+
                "ic ON(i.ItemCategoryID=ic.ItemCategoryID) WHERE ic.[Description] IN ('FOOD') AND irh.OrderDate BETWEEN CONVERT(datetime,'" +
                       startTime.Day.ToString() +
                       "-" +
                       startTime.Month.ToString() +
                       "-" +
                       startTime.Year.ToString() +
                       " 00:00:00.000') AND convert(datetime,'" +
                       endTime.Day.ToString() +
                       "-" +
                       endTime.Month.ToString() +
                       "-" +
                       endTime.Year.ToString() +
                       " 23:59:59.998')";
            decimal food = decimal.Parse(DataAccessHelper.ExecuteScalar(selectstr));
            temp.Add(new TransactionModel() { TransactionID = "FOOD", TransactionAmt = food });

            selectstr = "SELECT ISNULL(SUM(ird.LineTotal),0) FROM [Sales].[ItemReceiptHeader] irh LEFT OUTER JOIN [Sales].[ItemReceiptDetail] ird ON " +
               "(irh.ItemReceiptID = ird.ItemReceiptID) LEFT OUTER JOIN [Sales].[Item] i ON (i.ItemID = ird.ItemID) LEFT OUTER JOIN [Sales].[ItemCategory] " +
               "ic ON(i.ItemCategoryID=ic.ItemCategoryID) WHERE ic.[Description] IN ('OFFICE SUPPLIES') AND irh.OrderDate BETWEEN CONVERT(datetime,'" +
                      startTime.Day.ToString() +
                      "-" +
                      startTime.Month.ToString() +
                      "-" +
                      startTime.Year.ToString() +
                      " 00:00:00.000') AND convert(datetime,'" +
                      endTime.Day.ToString() +
                      "-" +
                      endTime.Month.ToString() +
                      "-" +
                      endTime.Year.ToString() +
                      " 23:59:59.998')";
            decimal office = decimal.Parse(DataAccessHelper.ExecuteScalar(selectstr));
            temp.Add(new TransactionModel() { TransactionID = "OFFICE SUPPLIES", TransactionAmt = office });

            selectstr = "SELECT ISNULL(SUM(ird.LineTotal),0) FROM [Sales].[ItemReceiptHeader] irh LEFT OUTER JOIN [Sales].[ItemReceiptDetail] ird ON " +
              "(irh.ItemReceiptID = ird.ItemReceiptID) LEFT OUTER JOIN [Sales].[Item] i ON (i.ItemID = ird.ItemID) LEFT OUTER JOIN [Sales].[ItemCategory] " +
              "ic ON(i.ItemCategoryID=ic.ItemCategoryID) WHERE ic.[Description] IN ('OTHER') AND irh.OrderDate BETWEEN CONVERT(datetime,'" +
                     startTime.Day.ToString() +
                     "-" +
                     startTime.Month.ToString() +
                     "-" +
                     startTime.Year.ToString() +
                     " 00:00:00.000') AND convert(datetime,'" +
                     endTime.Day.ToString() +
                     "-" +
                     endTime.Month.ToString() +
                     "-" +
                     endTime.Year.ToString() +
                     " 23:59:59.998')";
            decimal other2 = decimal.Parse(DataAccessHelper.ExecuteScalar(selectstr));
            temp.Add(new TransactionModel() { TransactionID = "OTHER", TransactionAmt = (other1+other2) });

            return temp;
        }

        private static List<TransactionModel> GetISRevenues(DateTime startTime, DateTime endTime)
        {
            List<TransactionModel> temp = new List<TransactionModel>();
            string selectstr = "SELECT ISNULL(SUM(ird.LineTotal),0) FROM [Sales].[ItemReceiptHeader] irh LEFT OUTER JOIN [Sales].[ItemReceiptDetail] ird ON " +
              "(irh.ItemReceiptID = ird.ItemReceiptID) LEFT OUTER JOIN [Sales].[Item] i ON (i.ItemID = ird.ItemID) LEFT OUTER JOIN [Sales].[ItemCategory] " +
              "ic ON(i.ItemCategoryID=ic.ItemCategoryID) WHERE ic.[Description] IN ('OTHER') AND irh.OrderDate BETWEEN CONVERT(datetime,'" +
                     startTime.Day.ToString() +
                     "-" +
                     startTime.Month.ToString() +
                     "-" +
                     startTime.Year.ToString() +
                     " 00:00:00.000') AND convert(datetime,'" +
                     endTime.Day.ToString() +
                     "-" +
                     endTime.Month.ToString() +
                     "-" +
                     endTime.Year.ToString() +
                     " 23:59:59.998')";
            decimal fees = decimal.Parse(DataAccessHelper.ExecuteScalar(selectstr));
            temp.Add(new TransactionModel() { TransactionID = "FEES", TransactionAmt = fees });

            selectstr = "SELECT ISNULL(SUM(AmountDonated),0) FROM [Institution].[Donation] WHERE DateDonated BETWEEN CONVERT(datetime,'" +
                     startTime.Day.ToString() +
                     "-" +
                     startTime.Month.ToString() +
                     "-" +
                     startTime.Year.ToString() +
                     " 00:00:00.000') AND convert(datetime,'" +
                     endTime.Day.ToString() +
                     "-" +
                     endTime.Month.ToString() +
                     "-" +
                     endTime.Year.ToString() +
                     " 23:59:59.998')";
            decimal donations = decimal.Parse(DataAccessHelper.ExecuteScalar(selectstr));
            temp.Add(new TransactionModel() { TransactionID = "DONATIONS", TransactionAmt = donations });
            return temp;
        }

        private static List<TransactionModel> GetISGains(DateTime from, DateTime to)
        {
            List<TransactionModel> temp = new List<TransactionModel>();
            return temp;
        }

        private static List<TransactionModel> GetISLosses(DateTime from, DateTime to)
        {
            List<TransactionModel> temp = new List<TransactionModel>();
            return temp;
        }
    }
}
