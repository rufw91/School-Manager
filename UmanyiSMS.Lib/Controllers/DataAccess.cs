
using UmanyiSMS.Lib.Presentation;
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
using System.Windows;
using UmanyiSMS;

namespace Helper
{
 /*
    public static class DataAccess
    {
        public static Task<ObservableCollection<FeePaymentModel>> GetFeesPaymentsAsync(int? studentID, DateTime? startTime, DateTime? endTime, string paymentMethod)
        {
            return Task.Factory.StartNew<ObservableCollection<FeePaymentModel>>(() => DataAccess.GetFeesPayments(studentID, startTime, endTime, paymentMethod));
        }

        private static ObservableCollection<SaleModel> GetSales(bool includeAllDetails, int? studentID, DateTime? startTime, DateTime? endTime)
        {
            string text;
            if (studentID.HasValue)
            {
                text = "SELECT SaleID,CustomerID,EmployeeID,OrderDate,TotalAmt FROM [SaleHeader] WHERE CustomerID=" + studentID;
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
                text = "SELECT SaleID,CustomerID,EmployeeID,OrderDate,TotalAmt FROM [SaleHeader]";
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
            DataTable dataTable = DataAccessHelper.Helper.ExecuteNonQueryWithResultTable(text);
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
        
        private static ObservableCollection<FeePaymentModel> GetFeesPayments(int? studentID, DateTime? startTime, DateTime? endTime, string paymentMethod)
        {
            string text;
            if (studentID.HasValue)
            {
                text = "SELECT s.NameOfStudent,fp.StudentID,fp.AmountPaid,fp.DatePaid,fp.PaymentMethod FROM [FeesPayment] fp LEFT OUTER JOIN [Student]s ON (fp.StudentID = s.StudentID) WHERE fp.StudentID =" + studentID;
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
                text = "SELECT s.NameOfStudent,fp.StudentID,fp.AmountPaid,fp.DatePaid,fp.PaymentMethod FROM [FeesPayment] fp LEFT OUTER JOIN [Student]s ON (fp.StudentID = s.StudentID)";
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
            DataTable dataTable = DataAccessHelper.Helper.ExecuteNonQueryWithResultTable(text);
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
        
        public static Task<ObservableImmutableList<ExamModel>> GetRegisteredExams(int studentID)
        {
            return Task.Factory.StartNew<ObservableImmutableList<ExamModel>>(delegate
            {
                ObservableImmutableList<ExamModel> observableCollection = new ObservableImmutableList<ExamModel>();
                string commandText =

                    "SELECT e.NameOfExam, es.ExamID, e.OutOf,e.ExamDateTime FROM [ExamHeader] e RIGHT OUTER JOIN " +
                    "[ExamStudentDetail] es ON (es.ExamID=e.ExamID) WHERE es.StudentID=" + studentID;
                DataTable dataTable = DataAccessHelper.Helper.ExecuteNonQueryWithResultTable(commandText);
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

        
        public static async Task<ObservableCollection<TimetableClassModel>> GetCurrentTimeTableAsync(int day)
        {
            ObservableCollection<TimetableClassModel> observableCollection = new ObservableCollection<TimetableClassModel>();
            ObservableCollection<ClassModel> observableCollection2 = await DataAccess.GetAllClassesAsync();
            Task[] array = new Task[observableCollection2.Count];
            for (int i = 0; i < observableCollection2.Count; i++)
            {
                array[i] = DataAccess.GetClassTimetableAsync(observableCollection2[i].ClassID, day);
            }
            Task.WaitAll(array);
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
        
        public static bool RemoveSubject(string SubjectID)
        {
            bool result = false;
            try
            {
                string commandText = "DELETE FROM [Subject] WHERE SubjectID = '" + SubjectID + "' ";
                result = DataAccessHelper.Helper.ExecuteNonQuery(commandText);
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
                string commandText = "DELETE FROM [Student] WHERE StudentID = '" + StudentID + "' ";
                result = DataAccessHelper.Helper.ExecuteNonQuery(commandText);
            }
            catch
            {
            }
            return result;
        }
        
        public static Task<bool> SaveNewDonation(DonationModel donation, string type)
        {
            return Task.Factory.StartNew<bool>(delegate
            {
                string commandText = "INSERT INTO [Donation] (DonorID,AmountDonated,DateDonated,DonateTo,[Type]) VALUES(@donorID,@amount,@dod,@dnt,@typ)";
                return DataAccessHelper.Helper.ExecuteNonQuery(commandText, new ObservableCollection<SqlParameter>
                {
                    new SqlParameter("@donorID", donation.DonorID),
                    new SqlParameter("@amount", donation.Amount),
                    new SqlParameter("@dod", donation.DateDonated),
                    new SqlParameter("@dnt", donation.DonateTo.ToString()),
                    new SqlParameter("@typ", type.Equals("D") ? 1 : 2)
                });
            });
        }

        public static Task<bool> SaveNewFeesPaymentAsync(FeePaymentModel newPayment, SaleModel newSale)
        {
            return Task.Factory.StartNew<bool>(delegate
            {
                if (newPayment.DatePaid.Date.Equals(DateTime.Now.Date))
                    newPayment.DatePaid = DateTime.Now;
                ObservableCollection<SqlParameter> observableCollection = new ObservableCollection<SqlParameter>();
                string text = "BEGIN TRANSACTION\r\nDECLARE @id int; SET @id = dbo.GetNewID('dbo.FeesPayment')\r\nDECLARE @id2 int; SET @id2 = dbo.GetNewID('dbo.SaleHeader')\r\nINSERT INTO [FeesPayment] (FeesPaymentID,StudentID,AmountPaid,DatePaid,PaymentMethod) VALUES(@id,@studentID,@amount,@dop,@paym)\r\nINSERT INTO [SaleHeader] (SaleID,CustomerID,EmployeeID,IsCancelled,OrderDate,IsDiscount,PaymentID) VALUES(@id2,@studentID,@employeeID,@isCancelled,@dateAdded,@isDiscount,@id)";
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
                        "\r\nINSERT INTO [SaleDetail] (SaleID,Name,Amount) VALUES(@id2,@entryName",
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
                observableCollection.Add(new SqlParameter("@paym", newPayment.PaymentMethod));
                observableCollection.Add(new SqlParameter("@dop", newPayment.DatePaid));
                observableCollection.Add(new SqlParameter("@employeeID", newSale.EmployeeID));
                observableCollection.Add(new SqlParameter("@isCancelled", newSale.IsCancelled ? 0 : 1));
                observableCollection.Add(new SqlParameter("@dateAdded", newSale.DateAdded));
                observableCollection.Add(new SqlParameter("@isDiscount", newSale.IsDiscount ? 0 : 1));
                return DataAccessHelper.Helper.ExecuteNonQuery(text, observableCollection);
            });
        }
        

        public static Task<decimal> GetAllSalesAsync(int studentID)
        {
            return Task.Factory.StartNew<decimal>(delegate
            {
                string commandText = "SELECT SUM(CONVERT(DECIMAL,TotalAmt)) FROM Sales.SaleHeader WHERE CustomerID = " + studentID;
                return decimal.Parse(DataAccessHelper.Helper.ExecuteScalar(commandText));
            });
        }

        public static Task<decimal> GetAllFeesPaidAsync(int studentID)
        {
            return Task.Factory.StartNew<decimal>(delegate
            {
                string commandText = "SELECT SUM(CONVERT(DECIMAL,AmountPaid)) FROM [FeesPayment] WHERE StudentID =" + studentID;
                return decimal.Parse(DataAccessHelper.Helper.ExecuteScalar(commandText));
            });
        }

        public static Task<ObservableCollection<FeePaymentReceiptModel>> GetRecentPaymentsReceiptAsync(int studentID)
        {
            return Task.Factory.StartNew<ObservableCollection<FeePaymentReceiptModel>>(delegate
            {
                ObservableCollection<FeePaymentReceiptModel> observableCollection = new ObservableCollection<FeePaymentReceiptModel>();
                string commandText = "SELECT TOP 20 FeesPaymentID, AmountPaid, DatePaid FROM [FeesPayment] WHERE StudentID =" + studentID + " ORDER BY [DatePaid] desc";
                DataTable dataTable = DataAccessHelper.Helper.ExecuteNonQueryWithResultTable(commandText);
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
        
        
        public static Task<ObservableCollection<StudentBaseModel>> GetClassStudents(int classID)
        {
            return Task.Factory.StartNew<ObservableCollection<StudentBaseModel>>(delegate
            {
                ObservableCollection<StudentBaseModel> observableCollection = new ObservableCollection<StudentBaseModel>();
                string commandText = "SELECT s.StudentID, FirstName+' '+MiddleName+' '+LastName FROM [Student]s WHERE s.ClassID =" 
                + classID + " AND s.StudentID NOT IN (SELECT StudentID FROM [StudentClearance]) AND s.StudentID NOT IN (SELECT StudentID FROM [StudentTransfer])";
                DataTable dataTable = DataAccessHelper.Helper.ExecuteNonQueryWithResultTable(commandText);
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
            return Task.Factory.StartNew<SaleModel>(delegate
            {
                SaleModel saleModel = new SaleModel();
                string commandText = "SELECT SaleID,CustomerID,EmployeeID,OrderDate,TotalAmt FROM [SaleHeader] WHERE PaymentID=" + feesPaymentID;
                DataTable dataTable = DataAccessHelper.Helper.ExecuteNonQueryWithResultTable(commandText);
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
        
        
        private static ObservableCollection<StudentExamResultEntryModel> GetTranscriptEntries(int studentID, IEnumerable<ExamWeightModel> exams, bool getBest7,TermModel term)
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
                "SELECT sub.NameOfSubject, dbo.GetWeightedExamSubjectScore(@sid,@e1Id,sssd.SubjectID,@e1W),",
                "dbo.GetWeightedExamSubjectScore(@sid,@e2Id,sssd.SubjectID,@e2W),",
                "dbo.GetWeightedExamSubjectScore(@sid,@e3Id,sssd.SubjectID,@e3W),ssd.Tutor,sub.Code,sub.SubjectID,std.Remarks ",
                "FROM [StudentSubjectSelectionDetail] sssd LEFT OUTER JOIN [StudentSubjectSelectionHeader] ",
                "sssh ON (sssd.StudentSubjectSelectionID=sssh.StudentSubjectSelectionID) LEFT OUTER JOIN [Subject] sub ON (sssd.SubjectID=sub.SubjectID) ",
                "LEFT OUTER JOIN [Student] s ON(sssh.StudentID=s.StudentID) LEFT OUTER JOIN [SubjectSetupHeader] ssh ON(ssh.ClassID=s.ClassID) ",
                "LEFT OUTER JOIN [SubjectSetupDetail] ssd ON(ssh.SubjectSetupID=ssd.SubjectSetupID AND ssd.SubjectID=sub.SubjectID) ",
                "LEFT OUTER JOIN [StudentTranscriptHeader] sth ON(sssh.StudentID=sth.StudentID AND sth.DateSaved >=@startd AND sth.DateSaved<@endd) ",
                "LEFT OUTER JOIN [StudentTranscriptDetail] std ON(std.SubjectID=sssd.SubjectID AND sth.StudentTranscriptID=std.StudentTranscriptID) ",
                "WHERE sssh.IsActive=1 AND ssh.IsActive=1 AND sssh.StudentID=@sid"
            });
            var paramColl = new List<SqlParameter>();
            paramColl.Add(new SqlParameter("@e1Id",num));
            paramColl.Add(new SqlParameter("@e1W",num4));
            paramColl.Add(new SqlParameter("@e2Id",num2));
            paramColl.Add(new SqlParameter("e2W",num5));
            paramColl.Add(new SqlParameter("@e3Id",num3));
            paramColl.Add(new SqlParameter("@e3W",num6));
            paramColl.Add(new SqlParameter("@sid",studentID));
            paramColl.Add(new SqlParameter("@startd", term.StartDate));
            paramColl.Add(new SqlParameter("@endd", term.EndDate.AddDays(1)));

            DataTable dataTable = DataAccessHelper.Helper.ExecuteNonQueryWithResultTable(commandText,paramColl);
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
            var opts = (from a in observableCollection
                        where optionals.Contains(a.Code)
                        select a);

            if (opts.Count() > 0 && getBest7)
            {
                decimal min = opts.Min((StudentExamResultEntryModel o) => o.MeanScore);
                StudentExamResultEntryModel item = observableCollection.First((StudentExamResultEntryModel a) => optionals.Contains(a.Code) && a.MeanScore == min);
                if (observableCollection.Count < 11 && observableCollection.Count > 7)
                {
                    observableCollection.Remove(item);
                }
            }
            return observableCollection;
        }

        public static Task<bool> SaveNewStudentTranscript(StudentTranscriptModel transcript, IEnumerable<ExamWeightModel> exams)
        {
            return Task.Factory.StartNew<bool>(delegate
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
                        "BEGIN TRANSACTION DECLARE @id int;\r\nIF EXISTS (SELECT * FROM [StudentTranscriptHeader] WHERE DateSaved BETWEEN @startD AND @endD AND StudentID=@sid)",
                        "\r\nBEGIN\r\nSET @id=(SELECT StudentTranscriptID FROM [StudentTranscriptHeader] WHERE DateSaved WHERE DateSaved BETWEEN @startD AND @endD AND StudentID=@sid)",
                        "\r\nUPDATE [StudentTranscriptHeader] SET Responsibilities=@resp,ClubsAndSport=@clubsp,Boarding=@board,ClassTeacher=@clstr,ClassTeacherComments=@clstrC,",
                        "Principal=@princ,PrincipalComments=@princC,OpeningDay=@openD,ClosingDay=@closD,DateSaved=@dtSav WHERE StudentTranscriptID= ",
                        transcript.StudentTranscriptID,
                        "\r\nEND\r\nELSE\r\nBEGIN\r\nSET @id = [dbo].GetNewID('dbo.StudentTranscriptHeader') INSERT INTO [StudentTranscriptHeader] (StudentTranscriptID,StudentID,Responsibilities,ClubsAndSport,Boarding,ClassTeacher,ClassTeacherComments,Principal,PrincipalComments,OpeningDay,ClosingDay,DateSaved) VALUES (@id,",
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
                            "IF NOT EXISTS (SELECT * FROM [StudentTranscriptDetail] WHERE StudentTranscriptID=@id AND SubjectID=",
                            current.SubjectID,
                            ")INSERT INTO [StudentTranscriptDetail] (StudentTranscriptID,SubjectID,Remarks) VALUES (@id,",
                            current.SubjectID,
                            ",'",
                            current.Remarks,
                            "')\r\nELSE\r\nUPDATE [StudentTranscriptDetail] SET Remarks='",
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
                        "IF NOT EXISTS (SELECT * FROM [StudentTranscriptExamDetail] WHERE StudentTranscriptID=@id)INSERT INTO [StudentTranscriptExamDetail] (StudentTranscriptID,Exam1ID,Exam2ID,Exam3ID,Exam1Weight,Exam2Weight,Exam3Weight) VALUES (@id,",
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
                        ")\r\nELSE\r\nUPDATE [StudentTranscriptExamDetail] SET Exam1ID=",
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
                    result = DataAccessHelper.Helper.ExecuteNonQuery(text7);
                    return result;
                }
                catch
                {
                }
                result = flag;
                return result;
            });
        }

        public static async Task<StudentTranscriptModel> GetStudentTranscript(int studentID, IEnumerable<ExamWeightModel> exams, IEnumerable<ClassModel> classes,TermModel term)
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
                "),0) DESC) row_no, StudentID, (SELECT COUNT(*) FROM [Student] WHERE ClassID =",
                num,
                " AND IsActive=1) no_of_students FROM [Student] WHERE CLassID=",
                num,
                " AND IsActive=1 group by StudentID ) x WHERE x.StudentID=s.StudentID) ClassPosition,(SELECT CONVERT(varchar(50),row_no)+'/'+CONVERT(varchar(50),no_of_students) FROM (SELECT ROW_NUMBER() OVER(ORDER BY ISNULL(dbo.GetExamTotalScore(StudentID,",
                num2,
                "),0)+ISNULL(dbo.GetExamTotalScore(StudentID,",
                num3,
                "),0)+ISNULL(dbo.GetExamTotalScore(StudentID,",
                num4,
                "),0) DESC) row_no, StudentID,(SELECT COUNT(*) FROM [Student] WHERE ClassID IN(",
                text,
                ") AND IsActive=1) no_of_students FROM [Student] WHERE CLassID IN (",
                text,
                ") AND IsActive=1 GROUP by StudentID ) x WHERE x.StudentID=s.StudentID) OverAllPosition,dbo.GetExamTotalScore(s.StudentID,",
                num2,
                ") Exam1Score,dbo.GetExamTotalScore(s.StudentID,",
                num3,
                ")Exam2Score,dbo.GetExamTotalScore(s.StudentID,",
                num4,
                ")Exam3Score,ISNULL(sth.StudentTranscriptID,0),Responsibilities,ClubsAndSport, Boarding, ClassTeacher,ClassTeacherComments,Principal,PrincipalComments,OpeningDay,ClosingDay,Term1Pos,Term2Pos,Term3Pos,DateSaved FROM [Student] s LEFT OUTER JOIN [Class] c ON(s.ClassID=c.ClassID) LEFT OUTER JOIN [StudentTranscriptHeader] sth ON(sth.StudentID=s.StudentID AND (sth.Exam1ID IN (",
                text2,
                ") OR sth.Exam2ID IN (",
                text2,
                ") OR sth.Exam3ID IN (",
                text2,
                "))) WHERE s.StudentID=",
                studentID
            });
            DataTable dataTable = DataAccessHelper.Helper.ExecuteNonQueryWithResultTable(commandText);
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

            bool getBest7 = false;
            switch (((IApp)Application.Current).ExamSettings.Best7Subjects)
            {
                case 0: getBest7 = studentTranscriptModel.NameOfClass.ToLowerInvariant().Contains("form 4"); break;
                case 1: getBest7 = studentTranscriptModel.NameOfClass.ToLowerInvariant().Contains("form 4") || studentTranscriptModel.NameOfClass.ToLowerInvariant().Contains("form 3"); break;
                case 2: getBest7 = true; break;
                case 3: getBest7 = false; break;
            }

            studentTranscriptModel.Entries = DataAccess.GetTranscriptEntries(studentID, exams, getBest7,term);

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
                "SELECT eh.ExamID,eh.NameOfExam,eh.ExamDatetime,ISNULL(eh.OutOf,100) OutOf, [Index] = CASE (eh.ExamID) \r\nWHEN sted.Exam1ID THEN 1\r\nWHEN sted.Exam2ID THEN 2\r\nWHEN sted.Exam3ID THEN 3\r\nELSE 0\r\nEND, [Weight] = CASE (eh.ExamID) \r\nWHEN sted.Exam1ID THEN sted.Exam1Weight\r\nWHEN sted.Exam2ID THEN sted.Exam2Weight\r\nWHEN sted.Exam3ID THEN sted.Exam3Weight\r\nELSE 0\r\nEND\r\nFROM [ExamHeader] eh LEFT OUTER JOIN [StudentTranscriptExamDetail] sted  ON (sted.Exam1ID = eh.ExamID OR sted.Exam2ID = eh.ExamID OR sted.Exam3ID = eh.ExamID) LEFT OUTER JOIN [StudentTranscriptHeader] sth  ON (sth.StudentTranscriptID=sted.StudentTranscriptID) WHERE sth.StudentID=",
                studentID,
                " AND ((eh.ExamDateTime>= CONVERT(datetime,'",
                GetTerm(otherTerms[0]).StartDate.ToString("g"),
                "') AND eh.ExamDateTime<= CONVERT(datetime,'",
                GetTerm(otherTerms[0]).EndDate.ToString("g"),
                "')) OR((eh.ExamDateTime>= CONVERT(datetime,'",
                GetTerm(otherTerms[1]).StartDate.ToString("g"),
                "') AND eh.ExamDateTime<= CONVERT(datetime,'",
                GetTerm(otherTerms[1]).EndDate.ToString("g"),
                "')) OR((eh.ExamDateTime>= CONVERT(datetime,'",
                GetTerm(otherTerms[2]).StartDate.ToString("g"),
                "') AND eh.ExamDateTime<= CONVERT(datetime,'",
                GetTerm(otherTerms[2]).EndDate.ToString("g"),
                "')))))"
            });
            DataTable dataTable = DataAccessHelper.Helper.ExecuteNonQueryWithResultTable(commandText);
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
                "SELECT DISTINCT eh.ExamID,eh.NameOfExam,eh.ExamDatetime,ISNULL(eh.OutOf,100) OutOf, [Index] = CASE (eh.ExamID) \r\nWHEN sted.Exam1ID THEN 1\r\nWHEN sted.Exam2ID THEN 2\r\nWHEN sted.Exam3ID THEN 3\r\nELSE 0\r\nEND, [Weight] = CASE (eh.ExamID) \r\nWHEN sted.Exam1ID THEN sted.Exam1Weight\r\nWHEN sted.Exam2ID THEN sted.Exam2Weight\r\nWHEN sted.Exam3ID THEN sted.Exam3Weight\r\nELSE 0\r\nEND\r\nFROM [ExamHeader] eh LEFT OUTER JOIN [StudentTranscriptExamDetail] sted  ON (sted.Exam1ID = eh.ExamID OR sted.Exam2ID = eh.ExamID OR sted.Exam3ID = eh.ExamID) LEFT OUTER JOIN [StudentTranscriptHeader] sth  ON (sth.StudentTranscriptID=sted.StudentTranscriptID) LEFT OUTER JOIN [Student] s ON (s.StudentID=sth.StudentID) WHERE s.ClassID=",
                classID,
                " AND ((eh.ExamDateTime>= CONVERT(datetime,'",
                GetTerm(otherTerms[0]).StartDate.ToString("g"),
                "') AND eh.ExamDateTime<= CONVERT(datetime,'",
               GetTerm(otherTerms[0]).EndDate.ToString("g"),
                "')) OR((eh.ExamDateTime>= CONVERT(datetime,'",
                GetTerm(otherTerms[1]).StartDate.ToString("g"),
                "') AND eh.ExamDateTime<= CONVERT(datetime,'",
                GetTerm(otherTerms[1]).EndDate.ToString("g"),
                "')) OR((eh.ExamDateTime>= CONVERT(datetime,'",
                GetTerm(otherTerms[2]).StartDate.ToString("g"),
                "') AND eh.ExamDateTime<= CONVERT(datetime,'",
                GetTerm(otherTerms[2]).EndDate.ToString("g"),
                "')))))"
            });
            DataTable dataTable = DataAccessHelper.Helper.ExecuteNonQueryWithResultTable(commandText);
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

        public static Task<StudentTranscriptModel2> GetStudentTranscript2(int studentID, IEnumerable<ExamWeightModel> exams, IEnumerable<ClassModel> classes,TermModel term)
        {
            return DataAccess.GetStudentTranscript2(studentID, exams, classes, 0,term);
        }

        
        public static Task<StudentTranscriptModel2> GetStudentTranscript2(int studentID, IEnumerable<ExamWeightModel> exams, IEnumerable<ClassModel> classes, int transcriptID, TermModel term)
        {
            return Task.Factory.StartNew(() =>
            {
                var c = GetClassIDFromStudentID(studentID).Result;
                int pyT3E1 = 0, pyT3E2 = 0, pyT3E3 = 0, t1E1 = 0, t1E2 = 0, t1E3 = 0, t2E1 = 0, t2E2 = 0, t2E3 = 0, t3E1 = 0, t3E2 = 0, t3E3 = 0;
                decimal pyT3E1W = 0, pyT3E2W = 0, pyT3E3W = 0, t1E1W = 0, t1E2W = 0, t1E3W = 0, t2E1W = 0, t2E2W = 0, t2E3W = 0, t3E1W = 0, t3E2W = 0, t3E3W = 0;
                
                switch (term.TermID)
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



                List<int> otherTerms = new List<int>(new List<int>(3) { -1, 1, 2, 3 }.Where(o => o != term.TermID));
                var s = GetOtherTermExams(studentID, otherTerms);
                foreach (int tx in otherTerms)
                {
                    if (tx == -1)
                    {
                        pyT3E1 = s.Any(o => o.Index == 1 && GetTerm(o.ExamDateTime) == -1) ? s.First(o => o.Index == 1 && GetTerm(o.ExamDateTime) == -1).ExamID : 0;
                        pyT3E1W = s.Any(o => o.Index == 1 && GetTerm(o.ExamDateTime) == -1) ? s.First(o => o.Index == 1 && GetTerm(o.ExamDateTime) == -1).Weight : 0;
                        pyT3E2 = s.Any(o => o.Index == 2 && GetTerm(o.ExamDateTime) == -1) ? s.First(o => o.Index == 2 && GetTerm(o.ExamDateTime) == -1).ExamID : 0;
                        pyT3E2W = s.Any(o => o.Index == 2 && GetTerm(o.ExamDateTime) == -1) ? s.First(o => o.Index == 2 && GetTerm(o.ExamDateTime) == -1).Weight : 0;
                        pyT3E3 = s.Any(o => o.Index == 3 && GetTerm(o.ExamDateTime) == -1) ? s.First(o => o.Index == 3 && GetTerm(o.ExamDateTime) == -1).ExamID : 0;
                        pyT3E3W = s.Any(o => o.Index == 3 && GetTerm(o.ExamDateTime) == -1) ? s.First(o => o.Index == 3 && GetTerm(o.ExamDateTime) == -1).Weight : 0;
                    }
                    else if (tx == 1)
                    {
                        t1E1 = s.Any(o => o.Index == 1 && GetTerm(o.ExamDateTime) == 1) ? s.First(o => o.Index == 1 && GetTerm(o.ExamDateTime) == 1).ExamID : 0;
                        t1E1W = s.Any(o => o.Index == 1 && GetTerm(o.ExamDateTime) == 1) ? s.First(o => o.Index == 1 && GetTerm(o.ExamDateTime) == 1).Weight : 0;
                        t1E2 = s.Any(o => o.Index == 2 && GetTerm(o.ExamDateTime) == 1) ? s.First(o => o.Index == 2 && GetTerm(o.ExamDateTime) == 1).ExamID : 0;
                        t1E2W = s.Any(o => o.Index == 2 && GetTerm(o.ExamDateTime) == 1) ? s.First(o => o.Index == 2 && GetTerm(o.ExamDateTime) == 1).Weight : 0;
                        t1E3 = s.Any(o => o.Index == 3 && GetTerm(o.ExamDateTime) == 1) ? s.First(o => o.Index == 3 && GetTerm(o.ExamDateTime) == 1).ExamID : 0;
                        t1E3W = s.Any(o => o.Index == 3 && GetTerm(o.ExamDateTime) == 1) ? s.First(o => o.Index == 3 && GetTerm(o.ExamDateTime) == 1).Weight : 0;
                    }
                    else if (tx == 2)
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
                    "," + t1E2W + "),0)+ISNULL(dbo.GetWeightedExamTotalScore(StudentID," + t1E3 + "," + t1E3W + "),0) DESC) row_no, StudentID, (SELECT COUNT(*) FROM [Student] WHERE ClassID =" + c + " AND IsActive=1)" +
                    " no_of_students FROM [Student] WHERE CLassID=" + c + " AND IsActive=1 group by StudentID ) x WHERE x.StudentID=s.StudentID) ClassPosition," +
                    "(SELECT CONVERT(varchar(50),row_no)+'/'+CONVERT(varchar(50),no_of_students) FROM (SELECT ROW_NUMBER() OVER(ORDER BY ISNULL(dbo.GetWeightedExamTotalScore(StudentID," + t1E1 +
                    "," + t1E1W + "),0)+ISNULL(dbo.GetWeightedExamTotalScore(StudentID," + t2E2 + "," + t1E2W + "),0)+ISNULL(dbo.GetWeightedExamTotalScore(StudentID," + t1E3 + "," + t1E3W + "),0) DESC) row_no, StudentID," +
                    "(SELECT COUNT(*) FROM [Student] WHERE ClassID IN(" + cStr + ") AND IsActive=1) no_of_students FROM [Student] WHERE CLassID IN (" + cStr +
                    ") AND IsActive=1 GROUP by StudentID ) x WHERE x.StudentID=s.StudentID) OverAllPosition,dbo.GetWeightedExamTotalScore(s.StudentID," + t1E1 + "," + t1E1W + ") Exam1Score,dbo.GetWeightedExamTotalScore(s.StudentID," + t1E2 +
                    "," + t1E2W + ")Exam2Score,dbo.GetWeightedExamTotalScore(s.StudentID," + t1E3 + "," + t1E3W + ")Exam3Score,SPhoto FROM [Student] s LEFT OUTER JOIN [Class] c ON(s.ClassID=c.ClassID)" +
                    " LEFT OUTER JOIN [StudentTranscriptHeader] sth ON(sth.StudentID=s.StudentID " +
                    ") LEFT OUTER JOIN [StudentTranscriptExamDetail] sted ON (sted.StudentTranscriptID=sth.StudentTranscriptID) WHERE s.IsActive=1 AND s.StudentID=" + studentID +
                    ") t1 " +


                    "LEFT OUTER JOIN (SELECT s.StudentID,(SELECT CONVERT(varchar(50),row_no)+'/'+CONVERT(varchar(50),no_of_students) FROM (SELECT ROW_NUMBER() " +
                    "OVER(ORDER BY ISNULL(dbo.GetWeightedExamTotalScore(StudentID," + t2E1 + "," + t2E1W + "),0)+ISNULL(dbo.GetWeightedExamTotalScore(StudentID," + t2E2 + "," + t2E2W + "),0)+ISNULL(dbo.GetWeightedExamTotalScore(StudentID," + t2E3 +
                    "," + t2E3W + "),0) DESC) row_no, StudentID, (SELECT COUNT(*) FROM [Student] WHERE ClassID =" + c + " AND IsActive=1) no_of_students FROM [Student]" +
                    " WHERE CLassID=" + c + " AND IsActive=1 group by StudentID ) x WHERE x.StudentID=s.StudentID) T2ClassPosition,(SELECT CONVERT(varchar(50),row_no)+'/'+CONVERT(varchar(50),no_of_students) " +
                    "FROM (SELECT ROW_NUMBER() OVER(ORDER BY ISNULL(dbo.GetWeightedExamTotalScore(StudentID," + t2E1 + "," + t2E1W + "),0)+ISNULL(dbo.GetWeightedExamTotalScore(StudentID," + t2E2 +
                    "," + t2E2W + "),0)+ISNULL(dbo.GetWeightedExamTotalScore(StudentID," + t2E3 + "," + t2E3W + "),0) DESC) row_no, StudentID,(SELECT COUNT(*) FROM [Student] WHERE ClassID IN(" + cStr +
                    ") AND IsActive=1) no_of_students FROM [Student] WHERE CLassID IN (" + cStr + ") AND IsActive=1 GROUP by StudentID ) x WHERE x.StudentID=s.StudentID)" +
                    " T2OverAllPosition,dbo.GetWeightedExamTotalScore(s.StudentID," + t2E1 + "," + t2E1W + ") T2Exam1Score,dbo.GetWeightedExamTotalScore(s.StudentID," + t2E2 + "," + t2E2W + ")T2Exam2Score,dbo.GetWeightedExamTotalScore(s.StudentID," + t2E3 + "," + t2E3W + ")T2Exam3Score " +
                    "FROM [Student] s LEFT OUTER JOIN [Class] c ON(s.ClassID=c.ClassID) LEFT OUTER JOIN [StudentTranscriptHeader] " +
                    "sth ON(sth.StudentID=s.StudentID AND sth.DateSaved BETWEEN CONVERT(datetime,'" + term.StartDate.ToString("g") + "') AND CONVERT(datetime,'" + term.EndDate.ToString("g") +
                    "')) " +
                    "LEFT OUTER JOIN [StudentTranscriptExamDetail] sted ON (sted.StudentTranscriptID=sth.StudentTranscriptID) WHERE s.IsActive=1 AND s.StudentID=" + studentID +
                    ") t2 ON (t1.StudentID=t2.StudentID) " +


                    "LEFT OUTER JOIN (SELECT s.StudentID,(SELECT CONVERT(varchar(50),row_no)+'/'+CONVERT(varchar(50),no_of_students) FROM (SELECT ROW_NUMBER() " +
                    "OVER(ORDER BY ISNULL(dbo.GetWeightedExamTotalScore(StudentID," + t3E1 + "," + t3E1W + "),0)+ISNULL(dbo.GetWeightedExamTotalScore(StudentID," + t3E2 + "," + t3E2W + "),0)+ISNULL(dbo.GetWeightedExamTotalScore(StudentID," + t3E3 +
                    "," + t3E3W + "),0) DESC) row_no, StudentID, (SELECT COUNT(*) FROM [Student] WHERE ClassID =" + c + " AND IsActive=1) no_of_students FROM [Student] " +
                    "WHERE CLassID=" + c + " AND IsActive=1 group by StudentID ) x WHERE x.StudentID=s.StudentID) T3ClassPosition,(SELECT CONVERT(varchar(50),row_no)+'/'+CONVERT(varchar(50),no_of_students) " +
                    "FROM (SELECT ROW_NUMBER() OVER(ORDER BY ISNULL(dbo.GetWeightedExamTotalScore(StudentID," + t3E1 + "," + t3E1W + "),0)+ISNULL(dbo.GetWeightedExamTotalScore(StudentID," + t3E2 +
                    "," + t3E2W + "),0)+ISNULL(dbo.GetWeightedExamTotalScore(StudentID," + t3E3 + "," + t3E3W + "),0) DESC) row_no, StudentID,(SELECT COUNT(*) FROM [Student] WHERE ClassID IN(" + cStr +
                    ") AND IsActive=1) no_of_students FROM [Student] WHERE CLassID IN (" + cStr + ") AND IsActive=1 GROUP by StudentID ) x WHERE x.StudentID=s.StudentID) " +
                    "T3OverAllPosition,dbo.GetWeightedExamTotalScore(s.StudentID," + t3E1 + "," + t3E1W + ") T3Exam1Score,dbo.GetWeightedExamTotalScore(s.StudentID," + t3E2 + "," + t3E2W + ")T3Exam2Score,dbo.GetWeightedExamTotalScore(s.StudentID," + t3E3 + "," + t3E3W + ")T3Exam3Score " +
                    "FROM [Student] s LEFT OUTER JOIN [Class] c ON(s.ClassID=c.ClassID) LEFT OUTER JOIN [StudentTranscriptHeader] " +
                    "sth ON(sth.StudentID=s.StudentID AND sth.DateSaved BETWEEN CONVERT(datetime,'" + term.StartDate.ToString("g") + "') AND CONVERT(datetime,'" + term.EndDate.ToString("g") +
                    "')) LEFT OUTER JOIN [StudentTranscriptExamDetail] sted ON (sted.StudentTranscriptID=sth.StudentTranscriptID)" +
                    " WHERE s.IsActive=1 AND s.StudentID=" + studentID + " )t3  ON (t3.StudentID=t1.StudentID) " +


                    "LEFT OUTER JOIN (SELECT s.StudentID, dbo.GetWeightedExamTotalScore(s.StudentID," + pyT3E1 + "," + pyT3E1W + ") PyT3Exam1Score,dbo.GetWeightedExamTotalScore(s.StudentID," + pyT3E2 + "," + pyT3E2W + ") PyT3Exam2Score," +
                    "dbo.GetWeightedExamTotalScore(s.StudentID," + pyT3E3 + "," + pyT3E3W + ")PyT3Exam3Score FROM [Student] s LEFT OUTER JOIN [Class] c ON(s.ClassID=c.ClassID) LEFT OUTER JOIN [StudentTranscriptHeader] " +
                    "sth ON(sth.StudentID=s.StudentID AND sth.DateSaved BETWEEN CONVERT(datetime,'" + term.StartDate.ToString("g") + "') AND CONVERT(datetime,'" + term.EndDate.ToString("g") +
                    "')) LEFT OUTER JOIN [StudentTranscriptExamDetail] sted ON (sted.StudentTranscriptID=sth.StudentTranscriptID)" +
                    " WHERE s.IsActive=1 AND s.StudentID=" + studentID + ")pyT3  ON (pyT3.StudentID=t1.StudentID)"

                    :

                    "SELECT t1.StudentTranscriptID,t1.StudentID, t1.NameOfStudent,t1.KCPEScore, t1.NameOfClass,t1.Responsibilities,t1.ClubsAndSport,t1.Boarding,t1.ClassTeacherComments, " +
                    "t1.PrincipalComments,t1.OpeningDay,t1.ClosingDay,t1.DateSaved,t1.ClassPosition,t1.OverAllPosition,t1.Exam1Score,t1.Exam2Score,t1.Exam3Score,t2.T2ClassPosition," +
                    "t2.T2OverAllPosition,t2.T2Exam1Score,t2.T2Exam2Score,t2.T2Exam3Score,t3.T3ClassPosition,t3.T3OverAllPosition,t3.T3Exam1Score,t3.T3Exam2Score,t3.T3Exam3Score," +
                    "pyT3.PyT3Exam1Score,pyT3.PyT3Exam2Score,pyt3.PyT3Exam3Score,t1.SPhoto FROM (SELECT s.StudentID, s.NameOfStudent,s.KCPEScore, c.NameOfClass,ISNULL(sth.StudentTranscriptID,0) StudentTranscriptID,Responsibilities,ClubsAndSport," +
                    " Boarding,ClassTeacherComments,PrincipalComments,OpeningDay,ClosingDay,DateSaved,(SELECT CONVERT(varchar(50),row_no)+'/'+CONVERT(varchar(50),no_of_students) " +
                    "FROM (SELECT ROW_NUMBER() OVER(ORDER BY ISNULL(dbo.GetWeightedExamTotalScore(StudentID," + t1E1 + "," + t1E1W + "),0)+ISNULL(dbo.GetWeightedExamTotalScore(StudentID," + t1E2 +
                    "," + t1E2W + "),0)+ISNULL(dbo.GetWeightedExamTotalScore(StudentID," + t1E3 + "," + t1E3W + "),0) DESC) row_no, StudentID, (SELECT COUNT(*) FROM [Student] WHERE ClassID =" + c + " AND IsActive=1)" +
                    " no_of_students FROM [Student] WHERE CLassID=" + c + " AND IsActive=1 group by StudentID ) x WHERE x.StudentID=s.StudentID) ClassPosition," +
                    "(SELECT CONVERT(varchar(50),row_no)+'/'+CONVERT(varchar(50),no_of_students) FROM (SELECT ROW_NUMBER() OVER(ORDER BY ISNULL(dbo.GetWeightedExamTotalScore(StudentID," + t1E1 +
                    "," + t1E1W + "),0)+ISNULL(dbo.GetWeightedExamTotalScore(StudentID," + t2E2 + "," + t1E2W + "),0)+ISNULL(dbo.GetWeightedExamTotalScore(StudentID," + t1E3 + "," + t1E3W + "),0) DESC) row_no, StudentID," +
                    "(SELECT COUNT(*) FROM [Student] WHERE ClassID IN(" + cStr + ") AND IsActive=1) no_of_students FROM [Student] WHERE CLassID IN (" + cStr +
                    ") AND IsActive=1 GROUP by StudentID ) x WHERE x.StudentID=s.StudentID) OverAllPosition,dbo.GetWeightedExamTotalScore(s.StudentID," + t1E1 + "," + t1E1W + ") Exam1Score,dbo.GetWeightedExamTotalScore(s.StudentID," + t1E2 +
                    "," + t1E2W + ")Exam2Score,dbo.GetWeightedExamTotalScore(s.StudentID," + t1E3 + "," + t1E3W + ")Exam3Score,SPhoto FROM [Student] s LEFT OUTER JOIN [Class] c ON(s.ClassID=c.ClassID)" +
                    " LEFT OUTER JOIN [StudentTranscriptHeader] sth ON(sth.StudentID=s.StudentID " +
                    ") LEFT OUTER JOIN [StudentTranscriptExamDetail] sted ON (sted.StudentTranscriptID=sth.StudentTranscriptID) WHERE s.IsActive=1 AND s.StudentID=" + studentID +
                    ") t1 " +


                    "LEFT OUTER JOIN (SELECT s.StudentID,(SELECT CONVERT(varchar(50),row_no)+'/'+CONVERT(varchar(50),no_of_students) FROM (SELECT ROW_NUMBER() " +
                    "OVER(ORDER BY ISNULL(dbo.GetWeightedExamTotalScore(StudentID," + t2E1 + "," + t2E1W + "),0)+ISNULL(dbo.GetWeightedExamTotalScore(StudentID," + t2E2 + "," + t2E2W + "),0)+ISNULL(dbo.GetWeightedExamTotalScore(StudentID," + t2E3 +
                    "," + t2E3W + "),0) DESC) row_no, StudentID, (SELECT COUNT(*) FROM [Student] WHERE ClassID =" + c + " AND IsActive=1) no_of_students FROM [Student]" +
                    " WHERE CLassID=" + c + " AND IsActive=1 group by StudentID ) x WHERE x.StudentID=s.StudentID) T2ClassPosition,(SELECT CONVERT(varchar(50),row_no)+'/'+CONVERT(varchar(50),no_of_students) " +
                    "FROM (SELECT ROW_NUMBER() OVER(ORDER BY ISNULL(dbo.GetWeightedExamTotalScore(StudentID," + t2E1 + "," + t2E1W + "),0)+ISNULL(dbo.GetWeightedExamTotalScore(StudentID," + t2E2 +
                    "," + t2E2W + "),0)+ISNULL(dbo.GetWeightedExamTotalScore(StudentID," + t2E3 + "," + t2E3W + "),0) DESC) row_no, StudentID,(SELECT COUNT(*) FROM [Student] WHERE ClassID IN(" + cStr +
                    ") AND IsActive=1) no_of_students FROM [Student] WHERE CLassID IN (" + cStr + ") AND IsActive=1 GROUP by StudentID ) x WHERE x.StudentID=s.StudentID)" +
                    " T2OverAllPosition,dbo.GetWeightedExamTotalScore(s.StudentID," + t2E1 + "," + t2E1W + ") T2Exam1Score,dbo.GetWeightedExamTotalScore(s.StudentID," + t2E2 + "," + t2E2W + ")T2Exam2Score,dbo.GetWeightedExamTotalScore(s.StudentID," + t2E3 + "," + t2E3W + ")T2Exam3Score " +
                    "FROM [Student] s LEFT OUTER JOIN [Class] c ON(s.ClassID=c.ClassID) LEFT OUTER JOIN [StudentTranscriptHeader] " +
                    "sth ON(sth.StudentID=s.StudentID AND sth.DateSaved BETWEEN CONVERT(datetime,'" + term.StartDate.ToString("g") + "') AND CONVERT(datetime,'" + term.EndDate.ToString("g") +
                    "')) " +
                    "LEFT OUTER JOIN [StudentTranscriptExamDetail] sted ON (sted.StudentTranscriptID=sth.StudentTranscriptID) WHERE s.IsActive=1 AND s.StudentID=" + studentID +
                    ") t2 ON (t1.StudentID=t2.StudentID) " +


                    "LEFT OUTER JOIN (SELECT s.StudentID,(SELECT CONVERT(varchar(50),row_no)+'/'+CONVERT(varchar(50),no_of_students) FROM (SELECT ROW_NUMBER() " +
                    "OVER(ORDER BY ISNULL(dbo.GetWeightedExamTotalScore(StudentID," + t3E1 + "," + t3E1W + "),0)+ISNULL(dbo.GetWeightedExamTotalScore(StudentID," + t3E2 + "," + t3E2W + "),0)+ISNULL(dbo.GetWeightedExamTotalScore(StudentID," + t3E3 +
                    "," + t3E3W + "),0) DESC) row_no, StudentID, (SELECT COUNT(*) FROM [Student] WHERE ClassID =" + c + " AND IsActive=1) no_of_students FROM [Student] " +
                    "WHERE CLassID=" + c + " AND IsActive=1 group by StudentID ) x WHERE x.StudentID=s.StudentID) T3ClassPosition,(SELECT CONVERT(varchar(50),row_no)+'/'+CONVERT(varchar(50),no_of_students) " +
                    "FROM (SELECT ROW_NUMBER() OVER(ORDER BY ISNULL(dbo.GetWeightedExamTotalScore(StudentID," + t3E1 + "," + t3E1W + "),0)+ISNULL(dbo.GetWeightedExamTotalScore(StudentID," + t3E2 +
                    "," + t3E2W + "),0)+ISNULL(dbo.GetWeightedExamTotalScore(StudentID," + t3E3 + "," + t3E3W + "),0) DESC) row_no, StudentID,(SELECT COUNT(*) FROM [Student] WHERE ClassID IN(" + cStr +
                    ") AND IsActive=1) no_of_students FROM [Student] WHERE CLassID IN (" + cStr + ") AND IsActive=1 GROUP by StudentID ) x WHERE x.StudentID=s.StudentID) " +
                    "T3OverAllPosition,dbo.GetWeightedExamTotalScore(s.StudentID," + t3E1 + "," + t3E1W + ") T3Exam1Score,dbo.GetWeightedExamTotalScore(s.StudentID," + t3E2 + "," + t3E2W + ")T3Exam2Score,dbo.GetWeightedExamTotalScore(s.StudentID," + t3E3 + "," + t3E3W + ")T3Exam3Score " +
                    "FROM [Student] s LEFT OUTER JOIN [Class] c ON(s.ClassID=c.ClassID) LEFT OUTER JOIN [StudentTranscriptHeader] " +
                    "sth ON(sth.StudentID=s.StudentID AND sth.DateSaved BETWEEN CONVERT(datetime,'" + term.StartDate.ToString("g") + "') AND CONVERT(datetime,'" + term.EndDate.ToString("g") +
                    "')) LEFT OUTER JOIN [StudentTranscriptExamDetail] sted ON (sted.StudentTranscriptID=sth.StudentTranscriptID)" +
                    " WHERE s.IsActive=1 AND s.StudentID=" + studentID + " )t3  ON (t3.StudentID=t1.StudentID) " +


                    "LEFT OUTER JOIN (SELECT s.StudentID, dbo.GetWeightedExamTotalScore(s.StudentID," + pyT3E1 + "," + pyT3E1W + ") PyT3Exam1Score,dbo.GetWeightedExamTotalScore(s.StudentID," + pyT3E2 + "," + pyT3E2W + ") PyT3Exam2Score," +
                    "dbo.GetWeightedExamTotalScore(s.StudentID," + pyT3E3 + "," + pyT3E3W + ")PyT3Exam3Score FROM [Student] s LEFT OUTER JOIN [Class] c ON(s.ClassID=c.ClassID) LEFT OUTER JOIN [StudentTranscriptHeader] " +
                    "sth ON(sth.StudentID=s.StudentID AND sth.DateSaved BETWEEN CONVERT(datetime,'" + term.StartDate.ToString("g") + "') AND CONVERT(datetime,'" + term.EndDate.ToString("g") +
                    "')) LEFT OUTER JOIN [StudentTranscriptExamDetail] sted ON (sted.StudentTranscriptID=sth.StudentTranscriptID)" +
                    " WHERE s.IsActive=1 AND s.StudentID=" + studentID + ")pyT3  ON (pyT3.StudentID=t1.StudentID)";


                DataTable dt = DataAccessHelper.Helper.ExecuteNonQueryWithResultTable(selectStr);

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

                bool getBest7 = false;
                switch (((IApp)Application.Current).ExamSettings.Best7Subjects)
                {
                    case 0: getBest7 = temp.NameOfClass.ToLowerInvariant().Contains("form 4"); break;
                    case 1: getBest7 = temp.NameOfClass.ToLowerInvariant().Contains("form 4") || temp.NameOfClass.ToLowerInvariant().Contains("form 3"); break;
                    case 2: getBest7 = true; break;
                    case 3: getBest7 = false; break;
                }

                switch (term.TermID)
                {
                    case 1:
                        temp.Term1Entries = GetTranscriptEntries(studentID, exams, getBest7,term);
                        temp.Term2Entries = GetTranscriptEntries(studentID, s.Where(o => GetTerm(o.ExamDateTime) == 2), getBest7, term);
                        temp.Term3Entries = GetTranscriptEntries(studentID, s.Where(o => GetTerm(o.ExamDateTime) == 3), getBest7, term);
                        temp.PrevYearEntries = GetTranscriptEntries(studentID, s.Where(o => GetTerm(o.ExamDateTime) == -1), getBest7, term);
                        break;
                    case 2:
                        temp.Term2Entries = GetTranscriptEntries(studentID, exams, getBest7, term);
                        temp.Term1Entries = GetTranscriptEntries(studentID, s.Where(o => GetTerm(o.ExamDateTime) == 1), getBest7, term);
                        temp.Term3Entries = GetTranscriptEntries(studentID, s.Where(o => GetTerm(o.ExamDateTime) == 3), getBest7, term);
                        temp.PrevYearEntries = GetTranscriptEntries(studentID, s.Where(o => GetTerm(o.ExamDateTime) == -1), getBest7, term);
                        break;
                    case 3:
                        temp.Term3Entries = GetTranscriptEntries(studentID, exams, getBest7, term);
                        temp.Term1Entries = GetTranscriptEntries(studentID, s.Where(o => GetTerm(o.ExamDateTime) == 1), getBest7, term);
                        temp.Term2Entries = GetTranscriptEntries(studentID, s.Where(o => GetTerm(o.ExamDateTime) == 2), getBest7, term);
                        temp.PrevYearEntries = GetTranscriptEntries(studentID, s.Where(o => GetTerm(o.ExamDateTime) == -1), getBest7, term);
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

                switch (term.TermID)
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

       
        public static Task<TimeTableSettingsModel> GetCurrentTimeTableSettings()
        {
            return Task.Factory.StartNew<TimeTableSettingsModel>(delegate
            {
                TimeTableSettingsModel timeTableSettingsModel = new TimeTableSettingsModel();
                string commandText = "SELECT TimeTableSettingsID,NoOfLessons,LessonDuration,LessonsStartTime,BreakIndices,BreakDuration FROM [TimeTableSettings] WHERE IsActive=1";
                DataTable dataTable = DataAccessHelper.Helper.ExecuteNonQueryWithResultTable(commandText);
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
            return Task.Factory.StartNew<TimeTableModel>(delegate
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
                string commandText = "SELECT tth.ClassID,ttd.NameOfSubject,ttd.Tutor,ttd.[Day],ttd.StartTime,ttd.EndTime,ttd.SubjectIndex FROM [TimeTableDetail] ttd INNER JOIN [TimeTableHeader] tth ON (ttd.TimeTableID= tth.TimeTableID) WHERE tth.IsActive = 1";
                DataTable dataTable = DataAccessHelper.Helper.ExecuteNonQueryWithResultTable(commandText);
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
            return Task.Factory.StartNew<bool>(delegate
            {
                string text = "BEGIN TRANSACTION\r\ndeclare @id int; declare @id2 int;\r\n";
                object obj = text;
                text = string.Concat(new object[]
                {
                    obj,
                    "IF NOT EXISTS (SELECT * FROM [SubjectSetupHeader] WHERE IsActive=1 AND ClassID=",
                    classID,
                    ")BEGIN SET @id = [dbo].GetNewID('dbo.SubjectSetupHeader') INSERT INTO [SubjectSetupHeader] (SubjectSetupID,ClassID,StartDate) VALUES (@id,",
                    classID,
                    ",'",
                    DateTime.Now.ToString("g"),
                    "')\r\nEND\r\nELSE SET @id=(SELECT SubjectSetupID FROM [SubjectSetupHeader] WHERE IsActive=1 AND ClassID=",
                    classID,
                    ")\r\n"
                });
                foreach (SubjectModel current in subjects)
                {
                    string text2 = text;
                    text = string.Concat(new string[]
                    {
                        text2,
                        "SET @id2 = (SELECT SubjectID FROM [Subject] WHERE NameOfSubject='",
                        current.NameOfSubject,
                        "')\r\nIF NOT EXISTS (SELECT * FROM [SubjectSetupDetail] ssd INNER JOIN [Subject] s ON(ssd.SubjectID=s.SubjectID)  WHERE s.NameOfSubject='",
                        current.NameOfSubject,
                        "' AND ssd.SubjectSetupID=@id)\r\nINSERT INTO [SubjectSetupDetail] (SubjectSetupID,SubjectID,Tutor) VALUES (@id,@id2,'",
                        current.Tutor,
                        "')\r\nELSE\r\nUPDATE [SubjectSetupDetail] SET Tutor='",
                        current.Tutor,
                        "' WHERE SubjectSetupID=@id AND SubjectID=@id2\r\n"
                    });
                }
                text += " COMMIT";
                return DataAccessHelper.Helper.ExecuteNonQuery(text);
            });
        }
        private static ObservableCollection<FeesStructureEntryModel> GetPayslipEntries(int paySlipID)
        {
            ObservableCollection<FeesStructureEntryModel> observableCollection = new ObservableCollection<FeesStructureEntryModel>();
            string commandText = "SELECT [Description], [Amount] FROM [PayslipDetail] WHERE PaySlipID =" + paySlipID;
            DataTable dataTable = DataAccessHelper.Helper.ExecuteNonQueryWithResultTable(commandText);
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

        
        
    }
}
*/
  }