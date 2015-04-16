using Helper.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Helper;
using Starehe;
using System.Windows;
using System.Diagnostics;

namespace Helper
{
    public static class DataAccess
    {
        public static Task<ObservableCollection<ExamResultStudentSubjectEntryModel>> GetStudentSubjectsResults(int classID, int examID, int subjectID, decimal outOf)
        {
            return Task.Run<ObservableCollection<ExamResultStudentSubjectEntryModel>>(() =>
            {
                ObservableCollection<ExamResultStudentSubjectEntryModel> temp = new ObservableCollection<ExamResultStudentSubjectEntryModel>();
                string selectStr = "SELECT s.StudentID, s.FirstName+' '+s.MiddleName+' '+LastName as Name, ISNULL(erd.Score,0),ISNULL(erd.Remarks,''),ISNULL(erh.ExamResultID,0) FROM [Institution].[Student] s LEFT OUTER JOIN" +
                    " (SELECT * FROM [Institution].[ExamResultHeader] WHERE ExamID=" + examID + " AND IsActive=1)erh " +
                    "ON(s.StudentID = erh.StudentID) LEFT OUTER JOIN (SELECT * FROM [Institution].[ExamresultDetail]" +
                    " WHERE SubjectID=" + subjectID + ")erd ON (erh.ExamresultID=erd.ExamResultID) WHERE s.ClassID=" + classID +
                    " AND s.StudentID NOT IN (SELECT StudentID FROM [Institution].[StudentClearance])" +
                    " AND s.StudentID NOT IN (SELECT StudentID FROM [Institution].[StudentTransfer])";
                DataTable dt = DataAccessHelper.ExecuteNonQueryWithResultTable(selectStr);
                ExamResultStudentSubjectEntryModel t;
                foreach (DataRow dtr in dt.Rows)
                {
                    t = new ExamResultStudentSubjectEntryModel();
                    t.SubjectID = subjectID;
                    t.ExamResultID = int.Parse(dtr[4].ToString());
                    t.StudentID = int.Parse(dtr[0].ToString());
                    t.NameOfStudent = dtr[1].ToString();
                    t.Score = decimal.Parse(dtr[2].ToString());
                    t.Remarks = dtr[3].ToString();
                    t.MaximumScore = outOf;
                    temp.Add(t);
                }
                return temp;
            });
        }

        public static Task<ObservableCollection<FeePaymentModel>> GetRecentPaymentsAsync(int studentID)
        {
            return Task.Run<ObservableCollection<FeePaymentModel>>(() =>
            {
                ObservableCollection<FeePaymentModel> temp = new ObservableCollection<FeePaymentModel>();
                string selectStr = "SELECT TOP 20 FeesPaymentID,AmountPaid, DatePaid FROM [Institution].[FeesPayment] WHERE StudentID =" +
                    studentID + " ORDER BY [DatePaid] desc";

                DataTable dt = DataAccessHelper.ExecuteNonQueryWithResultTable(selectStr);
                FeePaymentModel fpm;
                foreach (DataRow dtr in dt.Rows)
                {
                    fpm = new FeePaymentModel();
                    fpm.FeePaymentID = int.Parse(dtr[0].ToString());
                    fpm.AmountPaid = decimal.Parse(dtr[1].ToString());
                    fpm.StudentID = studentID;
                    fpm.DatePaid = DateTime.Parse(dtr[2].ToString());
                    temp.Add(fpm);
                }

                return temp;
            });
        }

        public static Task<FeePaymentReceiptModel> GetReceiptAsync(FeePaymentModel currentPayment, ObservableImmutableList<FeesStructureEntryModel> currentFeesStructure)
        {
            return Task.Run<FeePaymentReceiptModel>(async () =>
            {
                FeePaymentReceiptModel temp = new FeePaymentReceiptModel();
                temp.FeePaymentID = currentPayment.FeePaymentID;
                temp.AmountPaid = currentPayment.AmountPaid;
                temp.Entries = currentFeesStructure;
                temp.DatePaid = currentPayment.DatePaid;
                temp.NameOfClass = DataAccess.GetClassAsync(DataAccess.GetClassIDFromStudentID(currentPayment.StudentID).Result).Result.NameOfClass;
                temp.StudentID = currentPayment.StudentID;
                temp.NameOfStudent = currentPayment.NameOfStudent;


                var t = new FeesStructureEntryModel();
                t.Name = "TOTAL";
                foreach (var x in temp.Entries)
                    t.Amount += x.Amount;

                var a = new FeesStructureEntryModel();
                a.Amount = temp.AmountPaid;
                a.Name = "AMOUNT PAID";

                var b = new FeesStructureEntryModel();
                b.Amount = await GetBalanceBroughtForwardAsync(temp.StudentID, currentPayment.FeePaymentID, currentPayment.DatePaid);
                b.Name = "BALANCE B/F";

                var c = new FeesStructureEntryModel();
                c.Amount = await GetCurrentBalanceAsync(temp.StudentID);
                c.Name = "TOTAL BALANCE";

                temp.Entries.Add(t);
                temp.Entries.Add(a);
                temp.Entries.Add(b);
                temp.Entries.Add(c);
                return temp;
            });
        }

        public static Task<bool> SaveNewEmployeePaymentAsync(EmployeePaymentModel payment)
        {
            return Task.Run<bool>(() =>
            {
                string insertStr = "BEGIN TRANSACTION\r\n" +
                    "declare @id int; " +
                     "SET @id = [dbo].GetNewID('Institution.EmployeePayment') " +


                    "INSERT INTO [Institution].[EmployeePayment] (EmployeePaymentID,EmployeeID,AmountPaid,DatePaid,Notes)" +
                                " VALUES (@id," + payment.StaffID + "," + payment.Amount + ",'" + payment.DatePaid.ToString("g") +
                                "','" + payment.Notes + "')\r\n";

                insertStr += " COMMIT";

                return DataAccessHelper.ExecuteNonQuery(insertStr);
            });
        }

        public static Task<bool> SaveNewTimeTable(TimetableClassModel timeTable)
        {
            return Task.Run<bool>(() =>
            {
                string insertStr = "BEGIN TRANSACTION\r\n" +
                    "declare @id int; " +
                     "declare @id2 int; " +
                     "SET @id = [dbo].GetNewID('Institution.TimeTableHeader') " +


                    "INSERT INTO [Institution].[TimeTableHeader] (TimeTableID,ClassID)" +
                                " VALUES (@id," + timeTable.ClassID + ")\r\n";

                foreach (TimetableClassEntryModel entry in timeTable.Entries)
                {
                    insertStr += "SET @id2 = [dbo].GetNewID('Institution.TimeTableDetail')\r\n " +
                    "INSERT INTO [Institution].[TimeTableDetail] (TimeTableID,SubjectID,Tutor,Day,StartTime,EndTime)" +
                       " VALUES (@id," + entry.SubjectID + ",'" + entry.Tutor + "','" + entry.Day + "','" +
                    entry.StartTime.ToString("c") + "','" + entry.EndTime.ToString("c") + "')\r\n";
                }
                insertStr += " COMMIT";

                return DataAccessHelper.ExecuteNonQuery(insertStr);
            });
        }

        public async static Task<bool> SaveNewGalleryItemsAsync(ObservableCollection<GalleryItemModel> galleryItems)
        {
            bool succ = true;
            try
            {
                string insertStr = "BEGIN TRANSACTION\r\n";
                int count = 0;
                ObservableCollection<SqlParameter> paramColl = new ObservableCollection<SqlParameter>();
                foreach (GalleryItemModel item in galleryItems)
                {
                    insertStr +=
                      "INSERT INTO [Institution].[Gallery] (Name,DateAdded,Data) " +
                      "VALUES('" + item.Name + "','" + DateTime.Now.ToString("g") + "',@item" + count + ")\r\n";
                    object val = await GetGallerItemDataFromPathAsync(item.Path);
                    SqlParameter sp = new SqlParameter("@item" + count, SqlDbType.Binary);
                    sp.Value = (val == null) ? DBNull.Value : val;
                    paramColl.Add(sp);
                    count++;
                }
                insertStr += "COMMIT";
                succ = DataAccessHelper.ExecuteNonQueryWithParameters(insertStr, paramColl);
            }
            catch { }
            return succ;
        }

        public static Task<byte[]> GetGallerItemDataFromPathAsync(string path)
        {
            return Task.Run<byte[]>(() =>
            {
                byte[] temp = null;
                try
                {
                    temp = File.ReadAllBytes(path);
                }
                catch { }
                return temp;

            });
        }

        public async static Task<ObservableCollection<TimetableClassModel>> GetCurrentTimeTableAsync(int day)
        {
            ObservableCollection<TimetableClassModel> temp = new ObservableCollection<TimetableClassModel>();
            ObservableCollection<ClassModel> allClasses = await GetAllClassesAsync();
            Task[] tasks = new Task[allClasses.Count];
            for (int i = 0; i < allClasses.Count; i++)
            {
                tasks[i] = GetClassTimetableAsync(allClasses[i].ClassID, day);
            }
            await Task.WhenAll(tasks);

            foreach (ClassModel cm in allClasses)
                temp.Add(new TimetableClassModel() { ClassID = cm.ClassID, NameOfClass = cm.NameOfClass });
            foreach (TimetableClassModel ttcm in temp)
                ttcm.Entries = (tasks.Where(o => (o as Task<TimetableClassModel>).Result.ClassID == ttcm.ClassID).First() as Task<TimetableClassModel>).Result.Entries;
            return temp;
        }

        public static Task<TimetableClassModel> GetClassTimetableAsync(int classID, int day)
        {
            return Task.Run<TimetableClassModel>(() =>
            {
                TimetableClassModel temp = new TimetableClassModel();

                string selectStr = "SELECT s.NameOfSubject,t.SubjectID, t.Tutor, t.[Day], t.StartTime, t.EndTime FROM (SELECT td.SubjectID, td.Tutor, td.[Day], td.StartTime, td.EndTime FROM [Institution].[TimeTableHeader] th " +
                    "LEFT OUTER JOIN [Institution].[TimeTableDetail] td ON (th.TimeTableID = td.TimeTableID) " +
                    "WHERE th.ClassID=" + classID + " AND th.IsActive=1) t LEFT OUTER JOIN [Institution].[Subject] s ON " +
                    "(t.SubjectID=s.SubjectID) WHERE t.[Day]='" + ((DayOfWeek)day).ToString() + "'";

                temp.ClassID = classID;
                DataTable dt = DataAccessHelper.ExecuteNonQueryWithResultTable(selectStr);
                TimetableClassEntryModel ttcem;
                foreach (DataRow dtr in dt.Rows)
                {
                    ttcem = new TimetableClassEntryModel();
                    ttcem.NameOfSubject = dtr[0].ToString();
                    ttcem.SubjectID = int.Parse(dtr[1].ToString());
                    ttcem.Tutor = dtr[2].ToString();
                    ttcem.Day = dtr[3].ToString();
                    ttcem.StartTime = TimeSpan.Parse(dtr[4].ToString());
                    ttcem.EndTime = TimeSpan.Parse(dtr[5].ToString());
                    temp.Entries.Add(ttcem);
                }

                return temp;
            });
        }

        public static Task<bool> UpdateStudentAsync(StudentModel student)
        {
            return Task.Run<bool>(() =>
            {
                string upDateStr = "UPDATE [Institution].[Student] SET" +
                    " FirstName='" + student.FirstName +
                    "', LastName='" + student.LastName +
                    "', MiddleName='" + student.MiddleName +
                    "', DateOfAdmission='" + student.DateOfAdmission +
                    "', DateOfBirth='" + student.DateOfBirth +
                    "', NameOfGuardian='" + student.NameOfGuardian +
                    "', GuardianPhoneNo='" + student.GuardianPhoneNo +
                    "', Email='" + student.Email +
                    "', Address='" + student.Address +
                    "', PostalCode='" + student.PostalCode +
                    "', City='" + student.City +
                    "', PreviousInstitution='" + student.PrevInstitution + "'" +
                    ((student.DormitoryID > 0) ? (", DormitoryID=" + student.DormitoryID) : "") +
                    ", BedNo='" + student.BedNo +
                    "', PreviousBalance='" + student.PrevBalance +
                    "', SPhoto=@photo WHERE StudentID=" + student.StudentID;

                ObservableCollection<SqlParameter> paramColl = new ObservableCollection<SqlParameter>();

                paramColl.Add(new SqlParameter("@photo", student.SPhoto));

                bool succ = DataAccessHelper.ExecuteNonQueryWithParameters(upDateStr, paramColl);
                return succ;
            });
        }

        public static Task<bool> UpdateStaffAsync(StaffModel staff)
        {
            return Task.Run<bool>(() =>
            {
                string upDateStr = "UPDATE [Institution].[Staff] SET" +
                    " Name='" + staff.Name +
                    "', NationalID='" + staff.NationalID +
                    "', DateOfAdmission='" + staff.DateOfAdmission +
                    "', PhoneNo='" + staff.PhoneNo +
                    "', Email='" + staff.Email +
                    "', Address='" + staff.Address +
                    "', PostalCode='" + staff.PostalCode +
                    "', City='" + staff.City +
                    "', SPhoto=@photo WHERE StaffID=" + staff.StaffID;

                ObservableCollection<SqlParameter> paramColl = new ObservableCollection<SqlParameter>();

                paramColl.Add(new SqlParameter("@photo", staff.SPhoto));

                bool succ = DataAccessHelper.ExecuteNonQueryWithParameters(upDateStr, paramColl);
                return succ;
            });
        }

        private static ObservableCollection<IssueModel> GetItemIssues(bool includeAllDetails,
            DateTime? startTime, DateTime? endTime)
        {
            string selectStr;
            IssueModel temp;
            ObservableCollection<IssueModel> tempCls;

            selectStr = "SELECT ItemIssueID,Description,DateIssued,IsCancelled FROM " +
                                     "[Sales].[ItemIssueHeader] ";
            if ((startTime.HasValue && endTime.HasValue) == true)
                selectStr += " WHERE DateIssued BETWEEN '" +
   startTime.Value.Day.ToString() + "/" + startTime.Value.Month.ToString() + "/" + startTime.Value.Year.ToString() + " 00:00:00.000' AND '"
   + endTime.Value.Day.ToString() + "/" + endTime.Value.Month.ToString() + "/" + endTime.Value.Year.ToString() + " 23:59:59.998'";



            DataTable res = DataAccessHelper.ExecuteNonQueryWithResultTable(selectStr);

            if (res.Rows.Count == 0)
                return new ObservableCollection<IssueModel>();

            tempCls = new ObservableCollection<IssueModel>();
            foreach (DataRow d in res.Rows)
            {
                temp = new IssueModel();
                temp.IssueID = int.Parse(d[0].ToString());
                temp.Description = d[1].ToString();
                temp.DateIssued = DateTime.Parse(d[2].ToString());
                temp.IsCancelled = bool.Parse(d[3].ToString());

                if (includeAllDetails)
                    temp.Items = GetItemsIssueItems(temp.IssueID);

                tempCls.Add(temp);
            }
            return tempCls;
        }

        private static ObservableCollection<PurchaseModel> GetItemReceipts(bool includeAllDetails,
          int? supplierID, DateTime? startTime, DateTime? endTime)
        {
            string selectStr;
            PurchaseModel temp;
            ObservableCollection<PurchaseModel> tempCls;
            if (supplierID.HasValue)
            {
                selectStr = "SELECT sh.ItemReceiptID,sh.OrderDate,TotalAmt,SupplierID,IsCancelled,count(sd.ItemReceiptID),RefNo FROM " +
                                 "[Sales].[ItemReceiptHeader] sh LEFT OUTER JOIN [Sales].[ItemReceiptDetail] sd ON(sh.ItemReceiptID=sd.ItemReceiptID) WHERE sh.SupplierID =" + supplierID;
                if ((startTime.HasValue && endTime.HasValue) == true)
                    selectStr += " AND sh.OrderDate BETWEEN '" +
       startTime.Value.Day.ToString() + "/" + startTime.Value.Month.ToString() + "/" + startTime.Value.Year.ToString() + " 00:00:00.000' AND '"
       + endTime.Value.Day.ToString() + "/" + endTime.Value.Month.ToString() + "/" + endTime.Value.Year.ToString() + " 23:59:59.998'"
       + "\r\n GROUP BY sh.ItemReceiptID,sh.OrderDate, TotalAmt,SupplierID,IsCancelled,RefNo";
            }
            else
            {
                selectStr = "SELECT sh.ItemReceiptID,sh.OrderDate,TotalAmt,SupplierID,IsCancelled, count(sd.ItemReceiptID),RefNo FROM " +
                                     "[Sales].[ItemReceiptHeader] sh LEFT OUTER JOIN [Sales].[ItemReceiptDetail] sd ON(sh.ItemReceiptID=sd.ItemReceiptID)";
                if ((startTime.HasValue && endTime.HasValue) == true)
                    selectStr += " WHERE sh.OrderDate BETWEEN '" +
       startTime.Value.Day.ToString() + "/" + startTime.Value.Month.ToString() + "/" + startTime.Value.Year.ToString() + " 00:00:00.000' AND '"
       + endTime.Value.Day.ToString() + "/" + endTime.Value.Month.ToString() + "/" + endTime.Value.Year.ToString() + " 23:59:59.998'"
       + "\r\n GROUP BY sh.ItemReceiptID,sh.OrderDate, TotalAmt,SupplierID,IsCancelled,RefNo";
            }

            DataTable res = DataAccessHelper.ExecuteNonQueryWithResultTable(selectStr);

            if (res.Rows.Count == 0)
                return new ObservableCollection<PurchaseModel>();

            tempCls = new ObservableCollection<PurchaseModel>();
            foreach (DataRow d in res.Rows)
            {
                temp = new PurchaseModel();
                temp.PurchaseID = int.Parse(d[0].ToString());
                temp.OrderDate = DateTime.Parse(d[1].ToString());
                temp.OrderTotal = decimal.Parse(d[2].ToString());
                if (supplierID.HasValue)
                    temp.SupplierID = supplierID.Value;
                else temp.SupplierID = int.Parse(d[3].ToString());
                temp.IsCancelled = bool.Parse(d[4].ToString());
                temp.NoOfItems = decimal.Parse(d[5].ToString());
                temp.RefNo = d[6].ToString();
                if (includeAllDetails)
                {
                    temp.Items = GetItemsReceiptItems(temp.PurchaseID);
                    temp.NoOfItems = temp.Items.Count;
                }
                tempCls.Add(temp);
            }
            return tempCls;
        }

        public static ObservableCollection<ItemPurchaseModel> GetItemsReceiptItems(int saleId)
        {
            ObservableCollection<ItemPurchaseModel> tempcls = new ObservableCollection<ItemPurchaseModel>();

            string selectStr = "SELECT sod.ItemID,p.Description,sod.UnitPrice,sod.Quantity FROM " +
                "Sales.SaleDetail sod LEFT OUTER JOIN Sales.Item p ON( sod.ItemID = p.ItemID)" +
                " WHERE sod.SaleID = " + saleId;

            DataTable dt = DataAccessHelper.ExecuteNonQueryWithResultTable(selectStr);
            foreach (DataRow dtr in dt.Rows)
            {
                long id = long.Parse(dtr[0].ToString());
                decimal pr;
                decimal qt;
                decimal.TryParse(dtr[2].ToString(), out pr);
                decimal.TryParse(dtr[3].ToString(), out qt);
                tempcls.Add(new ItemPurchaseModel(id, dtr[1].ToString(), qt, pr));
            }
            return tempcls;
        }

        public static ObservableCollection<ItemIssueModel> GetItemsIssueItems(int issueID)
        {
            ObservableCollection<ItemIssueModel> tempcls = new ObservableCollection<ItemIssueModel>();

            string selectStr = "SELECT sod.ItemID,p.Description,sod.Quantity FROM " +
                "[Sales].[ItemIssueDetail] sod LEFT OUTER JOIN [Sales].[Item] p ON( sod.ItemID = p.ItemID)" +
                " WHERE sod.ItemIssueID = " + issueID;

            DataTable dt = DataAccessHelper.ExecuteNonQueryWithResultTable(selectStr);
            foreach (DataRow dtr in dt.Rows)
            {
                long id = long.Parse(dtr[0].ToString());
                decimal pr;
                decimal qt;
                decimal.TryParse(dtr[2].ToString(), out pr);
                decimal.TryParse(dtr[3].ToString(), out qt);
                tempcls.Add(new ItemIssueModel() { ItemID = id, Description = dtr[1].ToString(), Quantity = qt });
            }
            return tempcls;
        }

        public static Task<ObservableCollection<IssueModel>> GetItemIssuesAsync(bool includeAllDetails,
           DateTime? startTime, DateTime? endTime)
        {
            return Task.Run<ObservableCollection<IssueModel>>(() => GetItemIssues(includeAllDetails, startTime, endTime));

        }

        public static Task<ObservableCollection<PurchaseModel>> GetItemReceiptsAsync(bool includeAllDetails,
           int? supplierID, DateTime? startTime, DateTime? endTime)
        {
            return Task.Run<ObservableCollection<PurchaseModel>>(() => GetItemReceipts(includeAllDetails, supplierID, startTime, endTime));

        }

        public static Task<ObservableCollection<EmployeePaymentModel>> GetEmployeePaymentsAsync(int? employeeId, DateTime? from, DateTime? to)
        {
            return Task.Run<ObservableCollection<EmployeePaymentModel>>(() => GetEmployeePayments(employeeId, from, to));
        }

        private static ObservableCollection<EmployeePaymentModel> GetEmployeePayments(int? employeeId, DateTime? from, DateTime? to)
        {
            ObservableCollection<EmployeePaymentModel> tempCls = new ObservableCollection<EmployeePaymentModel>();
            try
            {
                string selectStr = "SELECT ep.EmployeePaymentID,ep.EmployeeID,s.Name, ep.AmountPaid,ep.DatePaid,ep.Notes " +
       "FROM [Institution].[EmployeePayment] ep LEFT OUTER JOIN [Institution].[Staff] s ON (ep.EmployeeID=s.StaffID)";
                if (employeeId.HasValue)
                {
                    selectStr += " WHERE ep.EmployeeID =" + employeeId;
                    if ((from.HasValue && to.HasValue) == true)
                        selectStr += " AND ep.DatePaid BETWEEN '" +
           from.Value.Day.ToString() + "/" + from.Value.Month.ToString() + "/" + from.Value.Year.ToString() + " 00:00:00.000' AND '"
           + to.Value.Day.ToString() + "/" + to.Value.Month.ToString() + "/" + to.Value.Year.ToString() + " 23:59:59.998'";
                }
                else
                    if ((from.HasValue && to.HasValue) == true)
                        selectStr += " WHERE ep.DatePaid BETWEEN '" +
           from.Value.Day.ToString() + "/" + from.Value.Month.ToString() + "/" + from.Value.Year.ToString() + " 00:00:00.000' AND '"
           + to.Value.Day.ToString() + "/" + to.Value.Month.ToString() + "/" + to.Value.Year.ToString() + " 23:59:59.998'";

                DataTable dtbl = DataAccessHelper.ExecuteNonQueryWithResultTable(selectStr);
                EmployeePaymentModel epm;
                foreach (DataRow d in dtbl.Rows)
                {
                    epm = new EmployeePaymentModel();
                    epm.EmployeePaymentID = int.Parse(d[0].ToString());
                    epm.StaffID = int.Parse(d[1].ToString());
                    epm.Name = d[2].ToString();
                    epm.Amount = decimal.Parse(d[3].ToString());
                    epm.DatePaid = DateTime.Parse(d[4].ToString());
                    epm.Notes = d[5].ToString();
                    tempCls.Add(epm);
                }
                return tempCls;
            }
            catch
            {
                return new ObservableCollection<EmployeePaymentModel>();
            }
        }

        public static Task<ObservableCollection<SupplierPaymentModel>> GetSupplierPaymentsAsync(int? supplierId, DateTime? from, DateTime? to)
        {
            return Task.Run<ObservableCollection<SupplierPaymentModel>>(() => GetSupplierPayments(supplierId, from, to));
        }

        private static ObservableCollection<SupplierPaymentModel> GetSupplierPayments(int? supplierId, DateTime? from, DateTime? to)
        {
            ObservableCollection<SupplierPaymentModel> tempCls = new ObservableCollection<SupplierPaymentModel>();
            try
            {
                string selectStr = "SELECT sp.SupplierPaymentID,sp.SupplierID,s.NameOfSupplier, sp.AmountPaid,sp.DatePaid,sp.Notes " +
       "FROM [Sales].[SupplierPayment] sp LEFT OUTER JOIN [Sales].[Supplier] s ON (sp.SupplierID=s.SupplierID)";
                if (supplierId.HasValue)
                {
                    selectStr += " WHERE sp.SupplierID =" + supplierId;
                    if ((from.HasValue && to.HasValue) == true)
                        selectStr += " AND sp.DatePaid BETWEEN '" +
           from.Value.Day.ToString() + "/" + from.Value.Month.ToString() + "/" + from.Value.Year.ToString() + " 00:00:00.000' AND '"
           + to.Value.Day.ToString() + "/" + to.Value.Month.ToString() + "/" + to.Value.Year.ToString() + " 23:59:59.998'";
                }
                else
                    if ((from.HasValue && to.HasValue) == true)
                        selectStr += " WHERE sp.DatePaid BETWEEN '" +
           from.Value.Day.ToString() + "/" + from.Value.Month.ToString() + "/" + from.Value.Year.ToString() + " 00:00:00.000' AND '"
           + to.Value.Day.ToString() + "/" + to.Value.Month.ToString() + "/" + to.Value.Year.ToString() + " 23:59:59.998'";

                DataTable dtbl = DataAccessHelper.ExecuteNonQueryWithResultTable(selectStr);
                foreach (DataRow d in dtbl.Rows)
                {
                    tempCls.Add(new SupplierPaymentModel(int.Parse(d[0].ToString()), int.Parse(d[1].ToString()), d[2].ToString(), decimal.Parse(d[3].ToString()), DateTime.Parse(d[4].ToString()), d[5].ToString()));
                }
                return tempCls;
            }
            catch
            {
                return new ObservableCollection<SupplierPaymentModel>();
            }
        }

        public static Task<StockTakingResultsModel> GetStockTakingResults(int stockTakingID)
        {
            return Task.Run<StockTakingResultsModel>(() =>
            {
                StockTakingResultsModel temp = new StockTakingResultsModel();

                string selectStr = "SELECT std.ItemID,i.Description," +
                    "std.AvailableQuantity,std.Expected,std.VarianceQty,std.VariancePc FROM " +
                    "[Sales].[StockTakingDetail] std LEFT OUTER JOIN [Sales].[Item] i ON( std.ItemID = i.ItemID)" +
                    " WHERE std.StockTakingID = " + stockTakingID;

                DataTable dt = DataAccessHelper.ExecuteNonQueryWithResultTable(selectStr);
                ItemStockTakingResultsModel istrm;
                foreach (DataRow dtr in dt.Rows)
                {
                    istrm = new ItemStockTakingResultsModel();
                    istrm.ItemID = long.Parse(dtr[0].ToString());
                    istrm.Description = dtr[1].ToString();
                    istrm.Counted = decimal.Parse(dtr[2].ToString());

                    istrm.Expected = decimal.Parse(dtr[3].ToString());
                    istrm.VarianceQty = decimal.Parse(dtr[4].ToString());
                    istrm.VariancePc = decimal.Parse(dtr[5].ToString());

                    temp.Items.Add(istrm);
                }
                return temp;
            });
        }

        public static Task<bool> SaveNewSupplierPaymentAsync(SupplierPaymentModel newPayment)
        {
            return Task.Run<bool>(() =>
            {
                string insertStr = "INSERT INTO [Sales].[SupplierPayment] (SupplierID,DatePaid,AmountPaid,Notes) " +
                      "VALUES(" + newPayment.SupplierID + ",'" + newPayment.DatePaid.ToString("g") + "',"
                      + newPayment.Amount + ",'" + newPayment.Notes + "')";
                return DataAccessHelper.ExecuteNonQuery(insertStr);
            });
        }

        public static Task<bool> RemoveSupplierAsync(int supplierID)
        {
            return Task.Run<bool>(() =>
            {
                string execStr = "DELETE FROM [Sales].[Supplier] WHERE SupplierID=" + supplierID + ";";
                bool succ = DataAccessHelper.ExecuteNonQuery(execStr);
                return succ;
            });
        }

        public static Task<bool> UpdateSupplierAsync(SupplierModel newSupplier)
        {
            return Task.Run<bool>(() =>
            {
                string upDateStr = "UPDATE [Sales].[Supplier] SET NameOfSupplier='" + newSupplier.NameOfSupplier + "', PhoneNo='" + newSupplier.PhoneNo +
                    "', AltPhoneNo='" + newSupplier.AltPhoneNo + "', Email='" + newSupplier.Email + "', Address='" + newSupplier.Address +
                    "', PostalCode='" + newSupplier.PostalCode + "', City='" + newSupplier.City + "', PINNo='" + newSupplier.PINNo + "' WHERE SupplierID=" + newSupplier.SupplierID;
                bool succ = DataAccessHelper.ExecuteNonQuery(upDateStr);
                return succ;
            });
        }

        public static Task<SupplierModel> GetSupplierAsync(int supplierID)
        {
            return Task.Run<SupplierModel>(() =>
            {
                SupplierModel temp = GetSupplier(supplierID);
                return temp;
            });
        }

        public static SupplierModel GetSupplier(int supplierID)
        {
            SupplierModel temp = new SupplierModel();
            string selectStr = "SELECT SupplierID, NameOfSupplier, PhoneNo, AltPhoneNo, " +
                "Email, Address, PostalCode, City, PINNo" +
                " FROM [Sales].[Supplier] WHERE SupplierID=" + supplierID;
            DataTable dt = DataAccessHelper.ExecuteNonQueryWithResultTable(selectStr);
            if (dt.Rows.Count >= 1)
            {
                DataRow dtr = dt.Rows[0];
                temp.SupplierID = int.Parse(dtr[0].ToString());
                temp.NameOfSupplier = dtr[1].ToString();
                temp.PhoneNo = dtr[2].ToString();
                temp.AltPhoneNo = dtr[3].ToString();
                temp.Email = dtr[4].ToString();
                temp.Address = dtr[5].ToString();
                temp.PostalCode = dtr[6].ToString();
                temp.City = dtr[7].ToString();
                temp.PINNo = dtr[8].ToString();
            }
            return temp;
        }

        public static Task<bool> SaveNewItemAsync(ItemModel item)
        {
            return Task.Run<bool>(() =>
            {
                string insertStr = "INSERT INTO [Sales].[Item] (ItemID,Description,DateAdded,ItemCategoryID,Price,Cost,VatID,StartQuantity) " +
                      "VALUES(" + item.ItemID + ",'" + item.Description + "','" + item.DateAdded.ToString("g") + "'," +
                      item.ItemCategoryID + "," + item.Price + "," + item.Cost + "," + item.VatID + "," + item.StartQuantity + ")";
                return DataAccessHelper.ExecuteNonQuery(insertStr);
            });
        }

        public static Task<bool> SaveNewItemCategoryAsync(ItemCategoryModel itemCategory)
        {
            return Task.Run<bool>(() =>
            {
                string insertStr = "INSERT INTO [Sales].[ItemCategory] (Description) " +
                      "VALUES('" + itemCategory.Description + "')";
                return DataAccessHelper.ExecuteNonQuery(insertStr);
            });
        }

        public static Task<bool> SaveNewVATRateAsync(VATRateModel rate)
        {
            return Task.Run<bool>(() =>
            {
                string insertStr = "INSERT INTO [Sales].[VAT] (Description,Rate) " +
                      "VALUES('" + rate.Description + "'," + rate.Rate + ")";
                return DataAccessHelper.ExecuteNonQuery(insertStr);
            });
        }

        public static Task<bool> SaveNewSupplierAsync(SupplierModel newSupplier)
        {
            return Task.Run<bool>(() =>
            {
                string insertStr = "";
                if (newSupplier.SupplierID <= 0)
                    insertStr = "INSERT INTO [Sales].[Supplier] (NameOfSupplier,PhoneNo,AltPhoneNo,Email, Address, PostalCode, City,PINNo) " +
                         "VALUES('" + newSupplier.NameOfSupplier + "','" + newSupplier.PhoneNo + "','"
                         + newSupplier.AltPhoneNo + "','" + newSupplier.Email + "','"
                         + newSupplier.Address + "','" + newSupplier.PostalCode + "','"
                         + newSupplier.City + "','" + newSupplier.PINNo + "')";
                else
                    insertStr = "INSERT INTO [Sales].[Supplier] (SupplierID, NameOfSupplier,PhoneNo,AltPhoneNo,Email, Address, PostalCode, City,PINNo) " +
                  "VALUES(" + newSupplier.SupplierID + ",'" + newSupplier.NameOfSupplier + "','" + newSupplier.PhoneNo + "','"
                  + newSupplier.AltPhoneNo + "','" + newSupplier.Email + "','"
                  + newSupplier.Address + "','" + newSupplier.PostalCode + "','"
                  + newSupplier.City + "','" + newSupplier.PINNo + "')";
                return DataAccessHelper.ExecuteNonQuery(insertStr);
            });
        }

        public static Task<bool> SaveNewStockTakingAsync(StockTakingModel newStockTaking)
        {
            return Task.Run<bool>(() =>
            {
                string insertStr = "BEGIN TRANSACTION\r\nDECLARE @id int; SET @id = dbo.GetNewID('Sales.StockTakingHeader')\r\n" +
                   "INSERT INTO [Sales].[StockTakingHeader] (StockTakingID,DateTaken) " +
                   "VALUES(@id,'" + newStockTaking.DateTaken.Value.ToString("dd-MM-yyyy") + "')";

                foreach (ItemStockTakingModel obs in newStockTaking.Items)
                {
                    insertStr += "\r\nINSERT INTO [Sales].[StockTakingDetail] (StockTakingID,ItemID,AvailableQuantity) " +
                        "VALUES(@id," + obs.ItemID + "," + obs.AvailableQuantity + ")";
                }
                insertStr += "\r\nCOMMIT";
                DataAccessHelper.ExecuteNonQuery(insertStr);
                return true;
            });
        }

        public static Task<ObservableCollection<StockTakingBaseModel>> GetAllStockTakings()
        {
            return Task.Run<ObservableCollection<StockTakingBaseModel>>(() =>
            {
                ObservableCollection<StockTakingBaseModel> temp = new ObservableCollection<StockTakingBaseModel>();
                string selecteStr = "SELECT StockTakingID," +
                                   "DateTaken FROM [Sales].[StockTakingHeader]";
                DataTable dt = DataAccessHelper.ExecuteNonQueryWithResultTable(selecteStr);
                StockTakingBaseModel stbm;
                foreach (DataRow dtr in dt.Rows)
                {
                    stbm = new StockTakingBaseModel();
                    stbm.StockTakingID = int.Parse(dtr[0].ToString());
                    stbm.DateTaken = DateTime.Parse(dtr[1].ToString());
                    temp.Add(stbm);
                }
                return temp;
            });
        }

        public static Task<ObservableCollection<SupplierModel>> GetAllSuppliersFullAsync()
        {
            return Task.Run<ObservableCollection<SupplierModel>>(() =>
            {
                ObservableCollection<SupplierModel> temp = new ObservableCollection<SupplierModel>();
                string selectStr = "SELECT SupplierID," +
                                        "NameOfSupplier," +
                                        "PhoneNo," +
                                        "AltPhoneNo," +
                                        "Email," +
                                        "Address," +
                                        "PostalCode," +
                                        "City," +
                                        "PINNo" +
                                        " FROM [Sales].[Supplier]";
                DataTable dt = DataAccessHelper.ExecuteNonQueryWithResultTable(selectStr);
                SupplierModel sm;
                foreach (DataRow dtr in dt.Rows)
                {
                    sm = new SupplierModel();
                    sm.SupplierID = int.Parse(dtr[0].ToString());
                    sm.NameOfSupplier = dtr[1].ToString();
                    sm.PhoneNo = dtr[2].ToString();
                    sm.AltPhoneNo = dtr[3].ToString();
                    sm.Email = dtr[4].ToString();
                    sm.Address = dtr[5].ToString();
                    sm.PostalCode = dtr[6].ToString();
                    sm.City = dtr[7].ToString();
                    sm.PINNo = dtr[8].ToString();

                    temp.Add(sm);
                }

                return temp;
            });
        }

        public static Task<ItemModel> GetItemAsync(long itemID)
        {
            return Task.Run<ItemModel>(() => GetItem(itemID));
        }

        public static Task<ObservableCollection<ItemListModel>> GetAllItemsWithCurrentQuantityAsync()
        {
            return Task.Run<ObservableCollection<ItemListModel>>(() =>
            {
                ObservableCollection<ItemListModel> temp = new ObservableCollection<ItemListModel>();
                string selecteStr = "SELECT ItemID," +
                                   "Description," +
                                   "DateAdded," +
                                   "ItemCategoryID," +
                                   "Price," +
                                   "Cost," +
                                   "StartQuantity," +
                                   "VatID," +
                                   "dbo.GetCurrentQuantity(ItemID)" +
                                   " FROM [Sales].[Item]";
                DataTable dt = DataAccessHelper.ExecuteNonQueryWithResultTable(selecteStr);
                ItemListModel im;
                foreach (DataRow dtr in dt.Rows)
                {
                    im = new ItemListModel();
                    im.ItemID = long.Parse(dtr[0].ToString());
                    im.Description = dtr[1].ToString();
                    im.DateAdded = DateTime.Parse(dtr[2].ToString());
                    im.ItemCategoryID = int.Parse(dtr[3].ToString());
                    im.Price = decimal.Parse(dtr[4].ToString());
                    im.Cost = decimal.Parse(dtr[5].ToString());
                    im.StartQuantity = decimal.Parse(dtr[6].ToString());
                    im.VatID = int.Parse(dtr[7].ToString());
                    im.CurrentQuantity = decimal.Parse(dtr[8].ToString());
                    temp.Add(im);
                }
                return temp;
            });
        }

        public static Task<ObservableCollection<VATAnalysisModel>> GetVatAnalysisAsync(DateTime from, DateTime to)
        {
            return Task.Run<ObservableCollection<VATAnalysisModel>>(() =>
            {
                ObservableCollection<VATAnalysisModel> temp = new ObservableCollection<VATAnalysisModel>();
                string selectStr = "select v.VatID, v.Description, v.Rate, isnULL(ca.TotalVat,0), isnULL(ca.TotalVat*v.Rate/100 ,0)FROM " +
                        "(SELECT i.VATID as VatID, isnULL(SUM(LineTotal),0) as TotalVAT  FROM " +
                        "(SELECT sd.ItemID as ItemID, isnULL(SUM(isnULL(sd.LineTotal,0)),0) LineTotal from " +
                        "Sales.SaleDetail sd LEFT OUTER JOIN Sales.SaleHeader sh ON(sd.SaleID=sh.SaleID) " +
                        "WHERE sh.OrderDate BETWEEN '" +
        from.Day.ToString() + "/" + from.Month.ToString() + "/" + from.Year.ToString() + " 00:00:00.000' AND '"
        + to.Day.ToString() + "/" + to.Month.ToString() + "/" + to.Year.ToString() + " 23:59:59.998' " +
                         "group by sd.ItemID) b LEFT OUTER JOIN Sales.Item i ON (b.ItemID= i.ItemID) group by i.VatID) ca " +
                         "RIGHT OUTER JOIN Sales.Vat v ON (ca.VatID = v.VatID)";
                DataTable dt = DataAccessHelper.ExecuteNonQueryWithResultTable(selectStr);
                VATAnalysisModel vam;
                foreach (DataRow dtr in dt.Rows)
                {
                    vam = new VATAnalysisModel();
                    vam.VatID = int.Parse(dtr[0].ToString());
                    vam.Description = dtr[1].ToString();
                    vam.Rate = decimal.Parse(dtr[2].ToString());
                    vam.SalesTaxable = decimal.Parse(dtr[3].ToString());
                    vam.TotalVATCollected = decimal.Parse(dtr[4].ToString());
                    temp.Add(vam);
                }
                return temp;
            });
        }

        public static Task<bool> UpdateItemAsync(ItemModel item)
        {
            return Task.Run<bool>(() =>
            {
                string upDateStr = "UPDATE [Sales].[Item] SET Description='" + item.Description + "', DateAdded='" + item.DateAdded.ToString("g") +
                    "', ItemCategoryID=" + item.ItemCategoryID + ", Price=" + item.Price + ", Cost=" + item.Cost +
                    ", StartQuantity=" + item.StartQuantity + ", VatID=" + item.VatID + " WHERE ItemID=" + item.ItemID;
                bool succ = DataAccessHelper.ExecuteNonQuery(upDateStr);
                return succ;
            });
        }

        public static Task<bool> SaveNewPurchaseAsync(PurchaseModel currentPurchase)
        {
            return Task.Run<bool>(() =>
            {
                string insertStr = "BEGIN TRANSACTION\r\nDECLARE @id int; SET @id = dbo.GetNewID('Sales.ItemReceiptHeader')\r\n" +
                   "INSERT INTO [Sales].[ItemReceiptHeader] (ItemReceiptID,SupplierID,OrderDate,RefNo,IsCancelled) " +
                   "VALUES(@id," + currentPurchase.SupplierID + ",'" + currentPurchase.OrderDate.ToString("g") + "','"
                   + currentPurchase.RefNo + "'," + (currentPurchase.IsCancelled ? "1" : "0") + ")";

                foreach (ItemPurchaseModel obs in currentPurchase.Items)
                {
                    insertStr += "\r\nINSERT INTO [Sales].[ItemReceiptDetail] (ItemReceiptID,ItemID,UnitCost,Quantity,LineTotal) " +
                        "VALUES(@id," + obs.ItemID + "," + obs.Cost + "," + obs.Quantity + "," + obs.TotalAmt + ")";
                }
                insertStr += "\r\nCOMMIT";
                DataAccessHelper.ExecuteNonQuery(insertStr);
                return true;
            });
        }

        public static Task<ObservableCollection<ItemCategoryModel>> GetAllItemCategoriesAsync()
        {
            return Task.Run<ObservableCollection<ItemCategoryModel>>(() =>
            {
                ObservableCollection<ItemCategoryModel> temp = new ObservableCollection<ItemCategoryModel>();
                string selecteStr = "SELECT ItemCategoryID," +
                                   "Description FROM [Sales].[ItemCategory]";
                DataTable dt = DataAccessHelper.ExecuteNonQueryWithResultTable(selecteStr);
                ItemCategoryModel icm;
                foreach (DataRow dtr in dt.Rows)
                {
                    icm = new ItemCategoryModel();
                    icm.ItemCategoryID = int.Parse(dtr[0].ToString());
                    icm.Description = dtr[1].ToString();
                    temp.Add(icm);
                }
                return temp;
            });
        }

        public static Task<ObservableCollection<SupplierBaseModel>> GetAllSuppliersAsync()
        {
            return Task.Run<ObservableCollection<SupplierBaseModel>>(() =>
            {
                ObservableCollection<SupplierBaseModel> temp = new ObservableCollection<SupplierBaseModel>();
                string selecteStr = "SELECT SupplierID," +
                                        "NameOfSupplier" +
                                        " FROM [Sales].[Supplier]";
                DataTable dt = DataAccessHelper.ExecuteNonQueryWithResultTable(selecteStr);
                SupplierBaseModel sm;
                foreach (DataRow dtr in dt.Rows)
                {
                    sm = new SupplierBaseModel();
                    sm.SupplierID = int.Parse(dtr[0].ToString());
                    sm.NameOfSupplier = dtr[1].ToString();
                    temp.Add(sm);
                }

                return temp;
            });
        }

        public static Task<ObservableCollection<VATRateModel>> GetAllVatsAsync()
        {
            return Task.Run<ObservableCollection<VATRateModel>>(() =>
            {
                ObservableCollection<VATRateModel> temp = new ObservableCollection<VATRateModel>();
                string selecteStr = "SELECT VatID," +
                                   "Description,Rate FROM [Sales].[Vat]";
                DataTable dt = DataAccessHelper.ExecuteNonQueryWithResultTable(selecteStr);
                VATRateModel vm;
                foreach (DataRow dtr in dt.Rows)
                {
                    vm = new VATRateModel();
                    vm.VatID = int.Parse(dtr[0].ToString());
                    vm.Description = dtr[1].ToString();
                    vm.Rate = decimal.Parse(dtr[2].ToString());
                    temp.Add(vm);
                }
                return temp;
            });
        }

        internal static ItemModel GetItem(long itemID)
        {
            ItemModel temp = new ItemModel();
            string selecteStr = "SELECT ItemID," +
                                "Description," +
                                "DateAdded," +
                                "ItemCategoryID," +
                                "Price," +
                                "Cost," +
                                "StartQuantity," +
                                "VatID" +
                                " FROM [Sales].[Item] WHERE ItemID =" + itemID;
            DataTable dt = DataAccessHelper.ExecuteNonQueryWithResultTable(selecteStr);
            if (dt.Rows.Count > 0)
            {
                DataRow dtr = dt.Rows[0];
                temp.ItemID = long.Parse(dtr[0].ToString());
                temp.Description = dtr[1].ToString();
                temp.DateAdded = DateTime.Parse(dtr[2].ToString());
                temp.ItemCategoryID = int.Parse(dtr[3].ToString());
                temp.Price = decimal.Parse(dtr[4].ToString());
                temp.Cost = 1;
                temp.StartQuantity = decimal.Parse(dtr[6].ToString());
                temp.VatID = int.Parse(dtr[7].ToString());
            }
            return temp;
        }

        private static Task<decimal> GetBalanceBroughtForwardAsync(int studentID, int paymentID, DateTime endTime)
        {
            return Task.Run<decimal>(() =>
            {
                string salesStr = "DECLARE  @sal decimal=(SELECT SUM(ISNULL(CONVERT(DECIMAL,TotalAmt),0)) FROM  " +
    "[Sales].[SaleHeader] WHERE CustomerID ='" + studentID + "');\r\n" +
    "DECLARE  @pur decimal=(SELECT SUM(ISNULL(CONVERT(DECIMAL,AmountPaid),0)) FROM  " +
    "[Institution].[FeesPayment] WHERE StudentID =" + studentID + " AND FeesPaymentID <> " + paymentID + " AND DatePaid<'" + endTime + "');\r\n" +
    "DECLARE  @prev decimal=(SELECT SUM(ISNULL(CONVERT(DECIMAL,PreviousBalance),0)) FROM  " +
    "[Institution].[Student] WHERE StudentID=" + studentID + ")\r\n" +
    "select (ISNULL(@sal,0)+ISNULL(@prev,0))-ISNULL(@pur,0)";

                decimal ft;
                decimal.TryParse(DataAccessHelper.ExecuteScalar(salesStr), out ft);
                return ft;
            });
        }

        private static Task<decimal> GetBalanceBroughtForwardAsync(int studentID, DateTime endTime)
        {
            return Task.Run<decimal>(() =>
            {
                string salesStr = "DECLARE  @sal decimal=(SELECT SUM(ISNULL(CONVERT(DECIMAL,TotalAmt),0)) FROM  " +
    "[Sales].[SaleHeader] WHERE CustomerID ='" + studentID + "');\r\n" +
    "DECLARE  @pur decimal=(SELECT SUM(ISNULL(CONVERT(DECIMAL,AmountPaid),0)) FROM  " +
    "[Institution].[FeesPayment] WHERE StudentID =" + studentID + " AND DatePaid <'" + endTime.ToString("g") + "');\r\n" +
    "DECLARE  @prev decimal=(SELECT SUM(ISNULL(CONVERT(DECIMAL,PreviousBalance),0)) FROM  " +
    "[Institution].[Student] WHERE StudentID=" + studentID + ")\r\n" +
    "select (ISNULL(@sal,0)+ISNULL(@prev,0))-ISNULL(@pur,0)";

                decimal ft;
                decimal.TryParse(DataAccessHelper.ExecuteScalar(salesStr), out ft);
                return ft;
            });
        }

        private static Task<decimal> GetCurrentBalanceAsync(int studentID)
        {
            return Task.Run<decimal>(() =>
            {

                string salesStr = "SELECT dbo.GetCurrentBalance(" + studentID + ")";

                decimal ft;
                decimal.TryParse(DataAccessHelper.ExecuteScalar(salesStr), out ft);
                return ft;
            });
        }

        public static Task<FeesStatementModel> GetFeesStatementAsync(int studentID, DateTime? startTime, DateTime? endTime)
        {
            return Task.Run<FeesStatementModel>(async () =>
            {
                if (studentID <= 0)

                    return new FeesStatementModel();


                FeesStatementModel temp = new FeesStatementModel();
                decimal amt;
                DateTime dt;

                string salesStr = "SELECT SaleID,OrderDate, TotalAmt FROM " +
                            "[Sales].[SaleHeader] WHERE [CustomerID] ='" + studentID + "'";
                if (startTime.HasValue && endTime.HasValue && true)
                    salesStr += " AND CONVERT(DATE, OrderDate) BETWEEN '" + startTime.Value.ToString("dd-MM-yyyy") +
                        " 00:00:00.000' AND '" + endTime.Value.ToString("dd-MM-yyyy") + " 23:59:59.998'";

                string paymentStr = "SELECT FeesPaymentID, DatePaid, AmountPaid FROM " +
                            "[Institution].[FeesPayment]  WHERE [StudentID] ='" + studentID + "'";
                if (startTime.HasValue && endTime.HasValue && true)
                    paymentStr += " AND CONVERT(DATE, DatePaid) BETWEEN '" + startTime.Value.ToString("dd-MM-yyyy") +
                        " 00:00:00.000' AND '" + endTime.Value.ToString("dd-MM-yyyy") + " 23:59:59.998'";

                DataTable dtSales = DataAccessHelper.ExecuteNonQueryWithResultTable(salesStr);
                DataTable dtPayments = DataAccessHelper.ExecuteNonQueryWithResultTable(paymentStr);
                ObservableCollection<TransactionModel> ish =
                    new ObservableCollection<TransactionModel>();
                foreach (DataRow dts in dtSales.Rows)
                {
                    ish.Add(new TransactionModel(TransactionTypes.Debit, dts[0].ToString(),
                        DateTime.Parse(dts[1].ToString()), decimal.Parse(dts[2].ToString())));
                    temp.TotalSales += decimal.Parse(dts[2].ToString());
                    temp.TotalDue += decimal.Parse(dts[2].ToString());
                }
                foreach (DataRow dts in dtPayments.Rows)
                {
                    DateTime.TryParse(dts[1].ToString(), out dt);
                    decimal.TryParse(dts[2].ToString(), out amt);
                    ish.Add(new TransactionModel(TransactionTypes.Credit, dts[0].ToString(),
                        dt, amt));
                    temp.TotalPayments += amt;
                    temp.TotalDue -= amt;
                }
                IEnumerable<TransactionModel> qa =
                  from fruit in ish
                  orderby fruit.TransactionDateTime
                  select fruit;
                temp.BalanceBroughtForward = await GetBalanceBroughtForwardAsync(studentID, startTime.Value);
                temp.Transactions.Add(new TransactionModel(TransactionTypes.Credit, "0", DateTime.Now, temp.BalanceBroughtForward));
                foreach (TransactionModel cc in qa)
                {
                    temp.Transactions.Add(cc);
                }

                temp.StudentID = studentID;
                temp.From = startTime.Value;
                temp.To = endTime.Value;
                temp.TotalDue = await GetCurrentBalanceAsync(studentID);
                return temp;
            });
        }

        public static Task<StudentModel> GetStudentAsync(int studentID)
        {
            return Task.Run<StudentModel>(() =>
            {
                StudentModel student = GetStudent(studentID);
                return student;
            });
        }

        public static Task<StaffModel> GetStaffAsync(int staffID)
        {
            return Task.Run<StaffModel>(() =>
            {
                StaffModel staff = GetStaff(staffID);
                return staff;
            });
        }

        public static StudentModel GetStudent(int studentID)
        {
            StudentModel CurrentStudent = new StudentModel();
            try
            {
                string SELECTSTR =
                       "SELECT FirstName,LastName,MiddleName,ClassID,DateOfBirth," +
                       "DateOfAdmission,NameOfGuardian,GuardianPhoneNo,Email," +
                       "Address,City,PostalCode,PreviousInstitution,DormitoryID,BedNo,SPhoto," +
                       "PreviousBalance FROM [Institution].[Student] WHERE StudentID='" + studentID + "'";
                DataTable r = DataAccessHelper.ExecuteNonQueryWithResultTable(SELECTSTR);
                if (r.Rows.Count != 0)
                {
                    CurrentStudent.StudentID = studentID;
                    CurrentStudent.FirstName = r.Rows[0][0].ToString();
                    CurrentStudent.LastName = r.Rows[0][1].ToString();
                    CurrentStudent.MiddleName = r.Rows[0][2].ToString();
                    CurrentStudent.NameOfStudent = CurrentStudent.FirstName + " " + CurrentStudent.MiddleName + " " + CurrentStudent.LastName;
                    CurrentStudent.ClassID = int.Parse(r.Rows[0][3].ToString());
                    CurrentStudent.DateOfBirth = DateTime.Parse(r.Rows[0][4].ToString());
                    CurrentStudent.DateOfAdmission = DateTime.Parse(r.Rows[0][5].ToString());
                    CurrentStudent.NameOfGuardian = r.Rows[0][6].ToString();
                    CurrentStudent.GuardianPhoneNo = r.Rows[0][7].ToString();
                    CurrentStudent.Email = r.Rows[0][8].ToString();
                    CurrentStudent.Address = r.Rows[0][9].ToString();
                    CurrentStudent.City = r.Rows[0][10].ToString();
                    CurrentStudent.PostalCode = r.Rows[0][11].ToString();
                    CurrentStudent.PrevInstitution = r.Rows[0][12].ToString();
                    CurrentStudent.DormitoryID = (!string.IsNullOrWhiteSpace(r.Rows[0][13].ToString())) ? int.Parse(r.Rows[0][13].ToString()) : 0;
                    CurrentStudent.BedNo = r.Rows[0][14].ToString();
                    CurrentStudent.SPhoto = (byte[])r.Rows[0][15];
                    CurrentStudent.PrevBalance = decimal.Parse(r.Rows[0][16].ToString());
                }
            }
            catch { }
            return CurrentStudent;
        }

        public static StaffModel GetStaff(int staffID)
        {
            StaffModel newStaff = new StaffModel();
            try
            {
                string SELECTSTR =
                       "SELECT Name,NationalID,DateOfAdmission,PhoneNo," +
                       "Email,Address,City,PostalCode,SPhoto" +
                       " FROM [Institution].[Staff] WHERE StaffID='" + staffID + "'";
                DataTable r = DataAccessHelper.ExecuteNonQueryWithResultTable(SELECTSTR);
                if (r.Rows.Count != 0)
                {
                    newStaff.StaffID = staffID;
                    newStaff.Name = r.Rows[0][0].ToString();
                    newStaff.NationalID = r.Rows[0][1].ToString();
                    newStaff.DateOfAdmission = DateTime.Parse(r.Rows[0][2].ToString());
                    newStaff.PhoneNo = r.Rows[0][3].ToString();
                    newStaff.Email = r.Rows[0][4].ToString();
                    newStaff.Address = r.Rows[0][5].ToString();
                    newStaff.City = r.Rows[0][6].ToString();
                    newStaff.PostalCode = r.Rows[0][7].ToString();
                    newStaff.SPhoto = (byte[])r.Rows[0][8];
                }
            }
            catch { }
            return newStaff;
        }

        public static Task<ObservableCollection<DormModel>> GetAllDormsAsync()
        {
            return Task.Run<ObservableCollection<DormModel>>(() =>
            {
                string selectStr =
                    "SELECT DormitoryID,NameOfDormitory FROM [Institution].[Dormitory]";
                DataTable tempColl = DataAccessHelper.ExecuteNonQueryWithResultTable(selectStr);
                ObservableCollection<DormModel> tempCls = new ObservableCollection<DormModel>();
                foreach (DataRow s in tempColl.Rows)
                {
                    tempCls.Add(new DormModel(int.Parse(s[0].ToString()), s[1].ToString()));
                }
                return tempCls;
            });
        }

        public static Task<ObservableCollection<DormitoryMemberModel>> GetDormitoryMembers(int dormitoryID)
        {
            return Task.Run<ObservableCollection<DormitoryMemberModel>>(() =>
            {
                ObservableCollection<DormitoryMemberModel> tempCls = new ObservableCollection<DormitoryMemberModel>();
                string selectStr =
                    "SELECT StudentID,FirstName +' '+LastName+' '+MiddleName,BedNo FROM [Institution].[Student] WHERE DormitoryID=" + dormitoryID;
                DataTable dt = DataAccessHelper.ExecuteNonQueryWithResultTable(selectStr);

                DormitoryMemberModel dmm;
                foreach (DataRow dtr in dt.Rows)
                {
                    dmm = new DormitoryMemberModel();
                    dmm.StudentID = int.Parse(dtr[0].ToString());
                    dmm.NameOfStudent = dtr[1].ToString();
                    dmm.BedNo = dtr[2].ToString();
                    tempCls.Add(dmm);
                }

                return tempCls;
            });
        }

        public static bool StudentExists(int studentID)
        {
            string selectStr =
                "SELECT StudentID FROM [Institution].[Student] WHERE StudentID ='" + studentID + "'";
            string result = DataAccessHelper.ExecuteScalar(selectStr);
            if (!string.IsNullOrWhiteSpace(result))
            {
                return true;
            }
            return false;
        }

        public static bool StudentIsCleared(int studentID)
        {
            string selectStr =
                "IF EXISTS (SELECT StudentID FROM [Institution].[StudentClearance] WHERE StudentID =" + studentID + ")\r\nSELECT 'true' ELSE SELECT 'false'";
            string result = DataAccessHelper.ExecuteScalar(selectStr);
            return bool.Parse(result);
        }

        public static bool RemoveSubject(string SubjectID)
        {
            bool resq = false;
            try
            {
                string insertCurrClass =
            "DELETE FROM [Institution].[Subject] WHERE SubjectID = '" + SubjectID +
            "' ";

                resq = DataAccessHelper.ExecuteNonQuery(insertCurrClass);
            }
            catch
            {
            }
            return resq;
        }

        public static bool RemoveStudent(string StudentID)
        {
            bool resq = false;
            try
            {
                string insertCurrClass =
            "DELETE FROM [Institution].[Student] WHERE StudentID = '" + StudentID +
            "' ";

                resq = DataAccessHelper.ExecuteNonQuery(insertCurrClass);
            }
            catch
            {
            }
            return resq;
        }

        public static Task<bool> RemovePaymentAsync(int paymentID)
        {
            return Task.Run<bool>(() =>
            {
                bool resq = false;
                try
                {
                    string insertCurrClass = "DELETE FROM [Institution].[FeesPayment] WHERE FeesPaymentID = " + paymentID;
                    resq = DataAccessHelper.ExecuteNonQuery(insertCurrClass);
                }
                catch
                {
                }
                return resq;
            });

        }

        public static Task<bool> RemoveSaleAsync(int saleID)
        {
            return Task.Run<bool>(() =>
            {
                bool resq = false;
                try
                {
                    string insertCurrClass = "DELETE FROM [Sales].[SaleHeader] WHERE SaleID = " + saleID;
                    insertCurrClass += "\r\nDELETE FROM [Sales].[SaleDetail] WHERE SaleID = " + saleID;
                    resq = DataAccessHelper.ExecuteNonQuery(insertCurrClass);
                }
                catch
                {
                }
                return resq;
            });

        }

        internal static int GetClassIDFromStudent(int studentID)
        {
            string SELECTSTR =
                       "SELECT ClassID FROM [Institution].[Student] WHERE StudentID=" + studentID;
            int ft;
            int.TryParse(DataAccessHelper.ExecuteScalar(SELECTSTR), out ft);
            return ft;
        }

        public async static Task<ObservableCollection<ClassModel>> GetAllClassesAsync()
        {
            return await GetCurrentRegistredClassesAsync();
        }

        public static Task<ObservableCollection<CombinedClassModel>> GetAllCombinedClassesAsync()
        {
            return GetCurrentRegistredCombinedClassesAsync();
        }
        /*
        public static Task<bool> SaveNewStudentAsync(StudentModel student)
        {
            return Task.Run<bool>(() =>
            {
                string INSERTSTR =
                    "INSERT INTO [Institution].[Student] (StudentID,FirstName,LastName,MiddleName,DateOfBirth,DateOfAdmission," +
                    "NameOfGuardian,GuardianPhoneNo,Email,Address,City,PostalCode,ClassID," +
                    "DormitoryID ,BedNo,PreviousInstitution,SPhoto) " +
                    "VALUES(dbo.GetNewID('Institution.Student'),'" +
                    student.FirstName + "','" +
                    student.LastName + "','" +
                    student.MiddleName + "','" +
                    student.DateOfBirth + "','" +
                    student.DateOfAdmission + "','" +
                    student.NameOfGuardian + "','" +
                    student.GuardianPhoneNo + "','" +
                    student.Email + "','" +
                    student.Address + "','" +
                    student.City + "','" +
                    student.PostalCode + "','" +
                    student.ClassID + "','" +
                    student.DormitoryID + "','" +
                    student.BedNo + "','" +
                    student.PrevInstitution + "',@photo)";

                ObservableCollection<SqlParameter> paramColl = new ObservableCollection<SqlParameter>();

                paramColl.Add(new SqlParameter("@photo",new byte[0]));

                return DataAccessHelper.ExecuteNonQueryWithParameters(INSERTSTR, paramColl);
            });
        }
        */

        public static Task<bool> SaveNewStudentAsync(StudentModel student)
        {
            return Task.Run<bool>(() =>
            {
                bool autoGenerateStudentID = student.StudentID == 0;
                string INSERTSTR =
                    "BEGIN TRANSACTION\r\n" +
                    "DECLARE @id INT; SET @id=dbo.GetNewID('Institution.Student')" +
                    "INSERT INTO [Institution].[Student] (StudentID,FirstName,LastName,MiddleName,DateOfBirth,DateOfAdmission," +
                    "NameOfGuardian,GuardianPhoneNo,Email,Address,City,PostalCode,IsBoarder";
                if (student.IsBoarder)
                    INSERTSTR += ",DormitoryID ,BedNo";
                INSERTSTR += ",PreviousInstitution,PreviousBalance,SPhoto) " +
                     "VALUES(" + (autoGenerateStudentID ? "@id" : student.StudentID.ToString()) + ",'" +
                     student.FirstName + "','" +
                     student.LastName + "','" +
                     student.MiddleName + "','" +
                     student.DateOfBirth + "','" +
                     student.DateOfAdmission + "','" +
                     student.NameOfGuardian + "','" +
                     student.GuardianPhoneNo + "','" +
                     student.Email + "','" +
                     student.Address + "','" +
                     student.City + "','" +
                     student.PostalCode + "','" +
                     student.IsBoarder + "','";
                if (student.IsBoarder)
                    INSERTSTR += student.DormitoryID + "','" +
                     student.BedNo + "','";
                INSERTSTR +=
                     student.PrevInstitution + "','" +
                     student.PrevBalance +
                     "',@photo)\r\n" +
                     "INSERT INTO [Institution].[CurrentClass] (StudentID,ClassID,IsActive) " +
                     "VALUES(" + (autoGenerateStudentID ? "@id" : student.StudentID.ToString()) + "," + student.ClassID + ",1)\r\n" +
                     "COMMIT";

                ObservableCollection<SqlParameter> paramColl = new ObservableCollection<SqlParameter>();

                paramColl.Add(new SqlParameter("@photo", new byte[0]));

                return DataAccessHelper.ExecuteNonQueryWithParameters(INSERTSTR, paramColl);
            });
        }

        public static Task<ObservableCollection<StaffModel>> GetAllStaffAsync()
        {
            return Task.Run<ObservableCollection<StaffModel>>(() =>
            {
                string selectStr =
                   "SELECT TOP 1000000 StaffID,Name,NationalID,DateOfAdmission,PhoneNo,Email," +
                   "Address,City,PostalCode,SPhoto" +
                   " FROM [Institution].[Staff]";
                ObservableCollection<StaffModel> allStaff = new ObservableCollection<StaffModel>();
                DataTable r = DataAccessHelper.ExecuteNonQueryWithResultTable(selectStr);
                if (r.Rows.Count != 0)
                {
                    StaffModel CurrentStaff;
                    foreach (DataRow dtr in r.Rows)
                    {
                        CurrentStaff = new StaffModel();
                        CurrentStaff.StaffID = (int)dtr[0];
                        CurrentStaff.Name = dtr[1].ToString();
                        CurrentStaff.NationalID = dtr[2].ToString();
                        CurrentStaff.DateOfAdmission = DateTime.Parse(dtr[3].ToString());
                        CurrentStaff.PhoneNo = dtr[4].ToString();
                        CurrentStaff.Email = dtr[5].ToString();
                        CurrentStaff.Address = dtr[6].ToString();
                        CurrentStaff.City = dtr[7].ToString();
                        CurrentStaff.PostalCode = dtr[8].ToString();
                        CurrentStaff.SPhoto = dtr[9] as byte[];
                        allStaff.Add(CurrentStaff);
                    }
                }
                return allStaff;
            }
            );
        }

        public static Task<ObservableCollection<StudentModel>> GetAllStudentsAsync()
        {

            return Task.Run<ObservableCollection<StudentModel>>(() =>
            {
                string selectStr =
                   "SELECT TOP 1000000 StudentID,FirstName,LastName,MiddleName,ClassID,DateOfBirth," +
                   "DateOfAdmission,NameOfGuardian,GuardianPhoneNo,Email," +
                   "Address,City,PostalCode,DormitoryID,BedNo,PreviousInstitution,SPhoto" +
                   " FROM [Institution].[Student]";
                ObservableCollection<StudentModel> allStudents = new ObservableCollection<StudentModel>();
                DataTable r = DataAccessHelper.ExecuteNonQueryWithResultTable(selectStr);
                if (r.Rows.Count != 0)
                {
                    StudentModel CurrentStudent;
                    foreach (DataRow dtr in r.Rows)
                    {
                        CurrentStudent = new StudentModel();
                        CurrentStudent.StudentID = (int)dtr[0];
                        CurrentStudent.FirstName = dtr[1].ToString();
                        CurrentStudent.MiddleName = dtr[3].ToString();
                        CurrentStudent.LastName = dtr[2].ToString();
                        CurrentStudent.ClassID = (int)dtr[4];
                        CurrentStudent.DateOfBirth = DateTime.Parse(dtr[5].ToString());
                        CurrentStudent.DateOfAdmission = DateTime.Parse(dtr[6].ToString());
                        CurrentStudent.NameOfGuardian = dtr[7].ToString();
                        CurrentStudent.GuardianPhoneNo = dtr[8].ToString();
                        CurrentStudent.Email = dtr[9].ToString();
                        CurrentStudent.Address = dtr[10].ToString();
                        CurrentStudent.City = dtr[11].ToString();
                        CurrentStudent.PostalCode = dtr[12].ToString();
                        CurrentStudent.DormitoryID = !Convert.IsDBNull(dtr[13]) ? int.Parse(dtr[13].ToString()) : 0;
                        CurrentStudent.BedNo = !Convert.IsDBNull(dtr[13]) ? dtr[14].ToString() : "";
                        CurrentStudent.PrevInstitution = !Convert.IsDBNull(dtr[13]) ? dtr[15].ToString() : "";
                        CurrentStudent.SPhoto = dtr[16] as byte[];
                        allStudents.Add(CurrentStudent);
                    }
                }
                return allStudents;
            }
            );
        }

        public static Task<ObservableCollection<StudentListModel>> GetAllStudentsListAsync()
        {

            return Task.Run<ObservableCollection<StudentListModel>>(() =>
            {
                string selectStr =
                   "SELECT TOP 1000000 s.StudentID,s.FirstName,s.LastName,s.MiddleName,s.ClassID, c.NameOfClass,s.DateOfBirth," +
                   "s.DateOfAdmission,s.NameOfGuardian,s.GuardianPhoneNo," +
                   "s.Address,s.City,s.PostalCode,s.BedNo,s.PreviousInstitution, s.DormitoryID, s.PreviousBalance,d.NameOfDormitory," +
                   " ISNULL(tr.StudentTransferID,'-1'), ISNULL(cl.StudentClearanceID,'-1'),s.IsBoarder FROM [Institution].[Student] s LEFT OUTER JOIN [Institution].[Class] c ON (s.ClassID=c.ClassID) LEFT OUTER JOIN [Institution].[Dormitory]" +
                   " d ON (s.DormitoryID=d.DormitoryID) LEFT OUTER JOIN [Institution].[StudentTransfer] tr ON(s.StudentID = tr.StudentID) LEFT OUTER JOIN " +
                   "[Institution].[StudentClearance] cl ON(s.StudentID=cl.StudentID)";
                ObservableCollection<StudentListModel> allStudents = new ObservableCollection<StudentListModel>();
                DataTable r = DataAccessHelper.ExecuteNonQueryWithResultTable(selectStr);
                if (r.Rows.Count != 0)
                {
                    StudentListModel CurrentStudent;
                    bool x;
                    foreach (DataRow dtr in r.Rows)
                    {
                        CurrentStudent = new StudentListModel();
                        CurrentStudent.StudentID = int.Parse(dtr[0].ToString());
                        CurrentStudent.FirstName = dtr[1].ToString();
                        CurrentStudent.LastName = dtr[2].ToString();
                        CurrentStudent.MiddleName = dtr[3].ToString();
                        CurrentStudent.ClassID = int.Parse(dtr[4].ToString());
                        CurrentStudent.NameOfClass = dtr[5].ToString();
                        CurrentStudent.DateOfBirth = DateTime.Parse(dtr[6].ToString());
                        CurrentStudent.DateOfAdmission = DateTime.Parse(dtr[7].ToString());
                        CurrentStudent.NameOfGuardian = dtr[8].ToString();
                        CurrentStudent.GuardianPhoneNo = dtr[9].ToString();
                        CurrentStudent.Address = dtr[10].ToString();
                        CurrentStudent.City = dtr[11].ToString();
                        CurrentStudent.PostalCode = dtr[12].ToString();
                        CurrentStudent.BedNo = dtr[13].ToString();
                        CurrentStudent.PrevInstitution = dtr[14].ToString();
                        CurrentStudent.DormitoryID = (!string.IsNullOrWhiteSpace(dtr[15].ToString())) ? int.Parse(dtr[15].ToString()) : 0;
                        CurrentStudent.PrevBalance = decimal.Parse(dtr[16].ToString());
                        CurrentStudent.NameOfDormitory = dtr[17].ToString();
                        CurrentStudent.IsTransferred = (dtr[18].ToString() != "-1") ? true : false;
                        CurrentStudent.IsCleared = (dtr[19].ToString() != "-1") ? true : false;
                        bool.TryParse(dtr[20].ToString(), out x);
                        CurrentStudent.IsBoarder = x;
                        allStudents.Add(CurrentStudent);
                    }
                }
                return allStudents;
            }
            );
        }

        public static Task<bool> SaveNewStaffAsync(StaffModel newStaff)
        {
            return Task.Run<bool>(() =>
            {
                string INSERTSTR =
                    "INSERT INTO [Institution].[Staff] (StaffID,Name,NationalID,DateOfAdmission,PhoneNo," +
                    "Email,Address,City,PostalCode,SPhoto) " +
                    "VALUES(dbo.GetNewID('Institution.Staff'),'" +
                    newStaff.Name + "','" +
                    newStaff.NationalID + "','" +
                    newStaff.DateOfAdmission.ToString("g") + "','" +
                    newStaff.PhoneNo + "','" +
                    newStaff.Email + "','" +
                    newStaff.Address + "','" +
                    newStaff.City + "','" +
                    newStaff.PostalCode + "',@photo)";

                ObservableCollection<SqlParameter> paramColl = new ObservableCollection<SqlParameter>();

                paramColl.Add(new SqlParameter("@photo", newStaff.SPhoto));

                return DataAccessHelper.ExecuteNonQueryWithParameters(INSERTSTR, paramColl);
            });
        }

        public static Task<bool> SaveNewFeesPaymentAsync(FeePaymentModel newPayment)
        {
            return Task.Run<bool>(() =>
            {
                string INSERTSTR =
                    "INSERT INTO [Institution].[FeesPayment] (FeesPaymentID,StudentID,AmountPaid,DatePaid) " +
                    "VALUES(dbo.GetNewID('Institution.FeesPayment')," +
                    newPayment.StudentID + ",'" +
                    newPayment.AmountPaid + "','" +
                    newPayment.DatePaid.ToString("g") + "')";
                return DataAccessHelper.ExecuteNonQuery(INSERTSTR);
            });
        }

        public static Task<bool> SaveNewFeesPaymentAsync(FeePaymentModel newPayment, SaleModel newSale)
        {
            return Task.Run<bool>(() =>
            {
                string insertStr = "BEGIN TRANSACTION\r\nDECLARE @id int; SET @id = dbo.GetNewID('Institution.FeesPayment')\r\n" +
                    "DECLARE @id2 int; SET @id2 = dbo.GetNewID('Sales.SaleHeader')\r\n" +
                    "INSERT INTO [Institution].[FeesPayment] (FeesPaymentID,StudentID,AmountPaid,DatePaid) " +
                    "VALUES(@id," +
                    newPayment.StudentID + ",'" +
                    newPayment.AmountPaid + "','" +
                    newPayment.DatePaid.ToString("g") + "')\r\n" +
                  "INSERT INTO [Sales].[SaleHeader] (SaleID,CustomerID,EmployeeID,IsCancelled,OrderDate,IsDiscount,PaymentID) " +
                  "VALUES(@id2,'" + newSale.CustomerID + "'," +
                  newSale.EmployeeID + ",'" + newSale.IsCancelled + "','" + newSale.DateAdded + "','"
                  + newSale.IsDiscount + "',@id)";

                foreach (FeesStructureEntryModel obs in newSale.SaleItems)
                {
                    insertStr += "\r\nINSERT INTO [Sales].[SaleDetail] (SaleID,Name,Amount) " +
                        "VALUES(@id2,'" + obs.Name + "'," + obs.Amount + ")";
                }
                insertStr += "\r\nCOMMIT";
                return DataAccessHelper.ExecuteNonQuery(insertStr);
            });
        }

        public static Task<bool> SaveNewStudentBill(SaleModel newSale)
        {
            return Task.Run<bool>(() =>
            {
                string insertStr = "BEGIN TRANSACTION\r\nDECLARE @id int; SET @id = dbo.GetNewID('Sales.SaleHeader')\r\n" +
                    "INSERT INTO [Sales].[SaleHeader] (SaleID,CustomerID,EmployeeID,IsCancelled,OrderDate,IsDiscount,PaymentID) " +
                  "VALUES(@id,'" + newSale.CustomerID + "'," +
                  newSale.EmployeeID + ",'" + newSale.IsCancelled + "','" + newSale.DateAdded.ToString("g") + "','"
                  + newSale.IsDiscount + "',@id)";

                foreach (FeesStructureEntryModel obs in newSale.SaleItems)
                {
                    insertStr += "\r\nINSERT INTO [Sales].[SaleDetail] (SaleID,Name,Amount) " +
                        "VALUES(@id,'" + obs.Name + "'," + obs.Amount + ")";
                }
                insertStr += "\r\nCOMMIT";
                return DataAccessHelper.ExecuteNonQuery(insertStr);
            });
        }

        public static Task<bool> SaveNewClassBill(SaleModel newSale)
        {
            return Task.Run<bool>(() =>
            {
                int term = GetTerm();
                DateTime? startTime = null;
                DateTime? endTime = null;
                switch (term)
                {
                    case 1: startTime = new DateTime(DateTime.Now.Year, 1, 1); endTime = new DateTime(DateTime.Now.Year, 4, 30); break;
                    case 2: startTime = new DateTime(DateTime.Now.Year, 5, 1); endTime = new DateTime(DateTime.Now.Year, 8, 31); break;
                    case 3: startTime = new DateTime(DateTime.Now.Year, 9, 1); endTime = new DateTime(DateTime.Now.Year, 12, 31); break;
                }
                string selectStuds = "SELECT StudentID FROM [Institution].[Student] WHERE ClassID=" + newSale.CustomerID;
                var studs = DataAccessHelper.CopyFromDBtoObservableCollection(selectStuds);
                string insertStr = "BEGIN TRANSACTION\r\n DECLARE @id int;\r\n";
                foreach (string s in studs)
                {

                    insertStr += "IF NOT EXISTS(SELECT * FROM [Sales].[SaleHeader] WHERE CustomerID=" + s +
                    " AND OrderDate BETWEEN '" + startTime.Value.Day.ToString() + "/" + startTime.Value.Month.ToString() + "/" + startTime.Value.Year.ToString() + " 00:00:00.000' AND '"
                + endTime.Value.Day.ToString() + "/" + endTime.Value.Month.ToString() + "/" + endTime.Value.Year.ToString() + " 23:59:59.998')\r\nBEGIN\r\n" +
                        "SET @id = dbo.GetNewID('Sales.SaleHeader');\r\n" +
                      "INSERT INTO [Sales].[SaleHeader] (SaleID,CustomerID,EmployeeID,IsCancelled,OrderDate,IsDiscount,PaymentID) " +
                    "VALUES(@id,'" + s + "'," +
                    newSale.EmployeeID + ",'" + newSale.IsCancelled + "','" + newSale.DateAdded.ToString("g") + "','" +
                    newSale.IsDiscount + "',0)\r\n;";


                    foreach (FeesStructureEntryModel obs in newSale.SaleItems)
                    {
                        insertStr += "INSERT INTO [Sales].[SaleDetail] (SaleID,Name,Amount) " +
                            "VALUES(@id,'" + obs.Name + "'," + obs.Amount + ");\r\n";
                    }
                    insertStr += "\r\nEND\r\n";
                }
                insertStr += "COMMIT";

                return DataAccessHelper.ExecuteNonQuery(insertStr);
            });
        }

        public static Task<bool> UpdateStudentBill(SaleModel newSale)
        {
            return Task.Run<bool>(() =>
            {
                string insertStr = "BEGIN TRANSACTION\r\nDELETE FROM [Sales].[SaleDetail] WHERE SaleID=" + newSale.SaleID;


                foreach (FeesStructureEntryModel obs in newSale.SaleItems)
                {
                    insertStr += "\r\nINSERT INTO [Sales].[SaleDetail] (SaleID,Name,Amount) " +
                        "VALUES(" + newSale.SaleID + ",'" + obs.Name + "'," + obs.Amount + ")";
                }
                insertStr += "\r\nCOMMIT";
                return DataAccessHelper.ExecuteNonQuery(insertStr);
            });
        }


        public static bool SearchAllStudentProperties(StudentModel student, string searchText)
        {
            Regex.CacheSize = 14;
            return
                Regex.Match(student.StudentID.ToString(), searchText, RegexOptions.IgnoreCase).Success ||
                Regex.Match(student.FirstName, searchText, RegexOptions.IgnoreCase).Success ||
                Regex.Match(student.LastName, searchText, RegexOptions.IgnoreCase).Success ||
                Regex.Match(student.MiddleName, searchText, RegexOptions.IgnoreCase).Success ||
                Regex.Match(student.NameOfGuardian, searchText, RegexOptions.IgnoreCase).Success ||
                Regex.Match(student.Address, searchText, RegexOptions.IgnoreCase).Success ||
                Regex.Match(student.BedNo, searchText, RegexOptions.IgnoreCase).Success ||
                Regex.Match(student.City, searchText, RegexOptions.IgnoreCase).Success ||
                Regex.Match(student.ClassID.ToString(), searchText, RegexOptions.IgnoreCase).Success ||
                Regex.Match(student.DormitoryID.ToString(), searchText, RegexOptions.IgnoreCase).Success ||
                Regex.Match(student.Email, searchText, RegexOptions.IgnoreCase).Success ||
                Regex.Match(student.NameOfStudent, searchText, RegexOptions.IgnoreCase).Success ||
                Regex.Match(student.PostalCode, searchText, RegexOptions.IgnoreCase).Success ||
                Regex.Match(student.PrevInstitution, searchText, RegexOptions.IgnoreCase).Success;
        }

        public static bool SearchAllBookProperties(BookModel student, string searchText)
        {
            Regex.CacheSize = 14;
            return
                Regex.Match(student.ISBN, searchText, RegexOptions.IgnoreCase).Success ||
                Regex.Match(student.Title, searchText, RegexOptions.IgnoreCase).Success ||
                Regex.Match(student.Author, searchText, RegexOptions.IgnoreCase).Success ||
                Regex.Match(student.Publisher, searchText, RegexOptions.IgnoreCase).Success ||
                Regex.Match(student.Price.ToString(), searchText, RegexOptions.IgnoreCase).Success;
        }

        public static bool SearchAllStaffProperties(StaffModel staff, string searchText)
        {
            Regex.CacheSize = 14;
            return
                Regex.Match(staff.StaffID.ToString(), searchText, RegexOptions.IgnoreCase).Success ||
                Regex.Match(staff.Name, searchText, RegexOptions.IgnoreCase).Success ||
                Regex.Match(staff.NationalID, searchText, RegexOptions.IgnoreCase).Success ||
                Regex.Match(staff.Address, searchText, RegexOptions.IgnoreCase).Success ||
                Regex.Match(staff.City, searchText, RegexOptions.IgnoreCase).Success ||
                Regex.Match(staff.Email, searchText, RegexOptions.IgnoreCase).Success ||
                Regex.Match(staff.PhoneNo, searchText, RegexOptions.IgnoreCase).Success ||
                Regex.Match(staff.PostalCode, searchText, RegexOptions.IgnoreCase).Success;
        }

        public static Task<bool> SaveNewFeesStructureAsync(FeesStructureModel currrentStruct)
        {
            return Task.Run<bool>(() =>
            {
                string insertStr = "BEGIN TRANSACTION\r\nDECLARE @id int; SET @id = dbo.GetNewID('Institution.FeesStructureHeader')\r\n" +
                    "INSERT INTO [Institution].[FeesStructureHeader] (FeesStructureID,ClassID, StartDate)" +
                                " VALUES (@id," + currrentStruct.ClassID +
                                ",'" + currrentStruct.StartDate.ToString("g") + "')\r\n";

                foreach (FeesStructureEntryModel entry in currrentStruct.Entries)
                    insertStr += "INSERT INTO [Institution].[FeesStructureDetail] (FeesStructureID,Name,Amount)" +
                        " VALUES (@id,'" + entry.Name +
                        "','" + entry.Amount +
                        "')\r\n";
                insertStr += " COMMIT";

                return DataAccessHelper.ExecuteNonQuery(insertStr);
            });
        }

        public static Task<FeesStructureModel> GetFeesStructureAsync(int currentClassID, DateTime currentDate)
        {
            return Task.Run<FeesStructureModel>(() =>
            {
                FeesStructureModel tempModel = new FeesStructureModel();
                string fdate = currentDate.Date.ToString("g");
                string selectStr = "DECLARE @id int\r\n" +
                "SET @id=(SELECT TOP 1 FeesStructureID FROM [Institution].[FeesStructureHeader] WHERE ClassID=" + currentClassID + "\r\n" +
                "AND IsActive=1)\r\n" +
                "SELECT ISNULL(@id,0)";

                int feesStructureID = int.Parse(DataAccessHelper.ExecuteScalar(selectStr));

                if (feesStructureID <= 0)
                    return tempModel;

                selectStr = "SELECT Name, Amount FROM [Institution].[FeesStructureDetail] WHERE FeesStructureID =" + feesStructureID;

                DataTable dt = DataAccessHelper.ExecuteNonQueryWithResultTable(selectStr);
                FeesStructureEntryModel fssem;
                foreach (DataRow dtr in dt.Rows)
                {
                    fssem = new FeesStructureEntryModel();
                    fssem.Amount = decimal.Parse(dtr[1].ToString());
                    fssem.Name = dtr[0].ToString();
                    tempModel.Entries.Add(fssem);
                }

                return tempModel;
            });
        }

        public static Task<ObservableCollection<SubjectModel>> GetSubjectsRegistredToClassAsync(int selectedClassID)
        {
            return Task.Run<ObservableCollection<SubjectModel>>(() =>
            {
                ObservableCollection<SubjectModel> allSubjects = new ObservableCollection<SubjectModel>();
                
                string selectStr = "SELECT ssd.SubjectID,s.NameOfSubject,s.MaximumScore FROM [Institution].[SubjectSetupDetail] ssd " +
                "LEFT OUTER JOIN [Institution].[Subject] s ON (ssd.SubjectID = s.SubjectID) LEFT OUTER JOIN [Institution].[SubjectSetupHeader] ssh "+
                "ON (ssd.SubjectSetupID = ssh.SubjectSetupID) WHERE IsACtive=1 AND ssh.ClassID="+selectedClassID+" ORDER BY s.Code";

                DataTable dt = DataAccessHelper.ExecuteNonQueryWithResultTable(selectStr);
                SubjectModel sub;
                foreach (DataRow dtr in dt.Rows)
                {
                    sub = new SubjectsSetupEntryModel();
                    sub.SubjectID = (int)dtr[0];
                    sub.NameOfSubject = dtr[1].ToString();
                    sub.MaximumScore = decimal.Parse(dtr[2].ToString());
                    if (sub.NameOfSubject.ToUpper().Trim().Contains("SKILLS"))
                        continue;
                    allSubjects.Add(sub);
                }

                return allSubjects;
            });
        }

        public static Task<ObservableCollection<ClassModel>> GetCurrentRegistredClassesAsync()
        {
            return Task.Run<ObservableCollection<ClassModel>>(() =>
            {
                ObservableCollection<ClassModel> allClasses = new ObservableCollection<ClassModel>();

                string selectStr = "DECLARE @id int\r\n SET @id=(" +
                    "SELECT ClassSetupID FROM " +
                "[Institution].[ClassSetupHeader] WHERE IsActive=1)\r\n" +
                "IF @id>0\r\nBEGIN\r\n" +
                "SELECT csd.ClassID,c.NameOfClass FROM [Institution].[ClassSetupDetail] csd " +
                "LEFT OUTER JOIN [Institution].[Class] c on (csd.ClassID = c.ClassID) WHERE csd.ClassSetupID =@id\r\nEND";

                DataTable dt = DataAccessHelper.ExecuteNonQueryWithResultTable(selectStr);
                ClassModel sub;
                foreach (DataRow dtr in dt.Rows)
                {
                    sub = new ClassModel();
                    sub.ClassID = (int)dtr[0];
                    sub.NameOfClass = dtr[1].ToString();
                    allClasses.Add(sub);
                }

                return allClasses;
            });
        }

        public static Task<ObservableCollection<CombinedClassModel>> GetCurrentRegistredCombinedClassesAsync()
        {
            return Task.Run<ObservableCollection<CombinedClassModel>>(() =>
            {
                ObservableCollection<CombinedClassModel> temp = new ObservableCollection<CombinedClassModel>();
                ObservableCollection<ClassModel> allClasses = new ObservableCollection<ClassModel>();

                string selectStr = "DECLARE @id int\r\n SET @id=(" +
                    "SELECT ClassSetupID FROM " +
                "[Institution].[ClassSetupHeader] WHERE IsActive=1)\r\n" +
                "IF @id>0\r\nBEGIN\r\n" +
                "SELECT csd.ClassID,c.NameOfClass FROM [Institution].[ClassSetupDetail] csd " +
                "LEFT OUTER JOIN [Institution].[Class] c on (csd.ClassID = c.ClassID) WHERE csd.ClassSetupID =@id\r\nEND";

                DataTable dt = DataAccessHelper.ExecuteNonQueryWithResultTable(selectStr);
                ClassModel sub;
                foreach (DataRow dtr in dt.Rows)
                {
                    sub = new ClassModel();
                    sub.ClassID = (int)dtr[0];
                    sub.NameOfClass = dtr[1].ToString();
                    allClasses.Add(sub);
                }
                var f = new List<string>();
                foreach (var s in allClasses)
                {
                    if (!f.Contains(s.NameOfClass.Substring(0, 6).ToUpper()))
                        f.Add(s.NameOfClass.Substring(0, 6).ToUpper());
                }
                CombinedClassModel ccm;
                for (int i = 0; i < f.Count; i++)
                {
                    ccm = new CombinedClassModel();
                    ccm.Description = f[i];
                    ccm.Entries = new ObservableCollection<ClassModel>(allClasses.Where(o => o.NameOfClass.Trim().ToUpper().Contains(f[i])));
                    temp.Add(ccm);
                }
                return temp;
            });
        }

        public static Task<decimal> GetAllSalesAsync(int studentID)
        {
            return Task.Run<decimal>(() =>
            {

                decimal tempcls = 0;

                string selectStr = "SELECT SUM(CONVERT(DECIMAL,TotalAmt)) FROM " +
                    "Sales.SaleHeader WHERE CustomerID = " + studentID;

                tempcls = decimal.Parse(DataAccessHelper.ExecuteScalar(selectStr));
                return tempcls;
            });
        }

        public static Task<decimal> GetAllFeesPaidAsync(int studentID)
        {
            return Task.Run<decimal>(() =>
            {
                decimal temp = 0;
                string selectStr = "SELECT SUM(CONVERT(DECIMAL,AmountPaid)) FROM [Institution].[FeesPayment] WHERE StudentID =" +
                    studentID;

                temp = decimal.Parse(DataAccessHelper.ExecuteScalar(selectStr));

                return temp;
            });
        }

        public static Task<ObservableCollection<FeePaymentReceiptModel>> GetRecentPaymentsReceiptAsync(int studentID)
        {
            return Task.Run<ObservableCollection<FeePaymentReceiptModel>>(() =>
            {
                ObservableCollection<FeePaymentReceiptModel> temp = new ObservableCollection<FeePaymentReceiptModel>();
                string selectStr = "SELECT TOP 20 FeesPaymentID, AmountPaid, DatePaid FROM [Institution].[FeesPayment] WHERE StudentID =" +
                    studentID + " ORDER BY [DatePaid] desc";

                DataTable dt = DataAccessHelper.ExecuteNonQueryWithResultTable(selectStr);
                FeePaymentReceiptModel fpm;
                foreach (DataRow dtr in dt.Rows)
                {
                    fpm = new FeePaymentReceiptModel();
                    fpm.AmountPaid = decimal.Parse(dtr[1].ToString());
                    fpm.StudentID = studentID;
                    fpm.FeePaymentID = int.Parse(dtr[0].ToString());
                    fpm.DatePaid = DateTime.Parse(dtr[2].ToString());
                    temp.Add(fpm);
                }

                return temp;
            });
        }

        public static Task<bool> SaveNewExamAsync(ExamModel newExam)
        {
            return Task.Run<bool>(() =>
            {
                string insertStr = "BEGIN TRANSACTION\r\nDECLARE @id int;\r\n SET @id = dbo.GetNewID('Institution.ExamHeader') " +
                     "INSERT INTO [Institution].[ExamHeader] (ExamID,NameOfExam,OutOf,ExamDateTime)" +
                                  " VALUES (@id,'" + newExam.NameOfExam + "'," + newExam.OutOf + ",'" + DateTime.Now.ToString("g") + "')\r\n";
                foreach (var c in newExam.Classes)

                    insertStr +=
                        "INSERT INTO [Institution].[ExamClassDetail] (ExamID,ClassID)" +
                                  " VALUES (@id," + c.ClassID + ")\r\n";

                foreach (ExamSubjectEntryModel entry in newExam.Entries)
                    insertStr += "INSERT INTO [Institution].[ExamDetail] (ExamID,SubjectID,ExamDateTime)" +
                        " VALUES (@id," + entry.SubjectID +
                        ",'" + entry.ExamDateTime.ToString("g") + "')\r\n";

                insertStr += " COMMIT";

                return DataAccessHelper.ExecuteNonQuery(insertStr);
            });
        }

        public static Task<bool> SaveNewSubjectSetupAsync(SubjectsSetupModel subjectsSetup)
        {
            return Task.Run<bool>(() =>
            {
                string insertStr = "BEGIN TRANSACTION\r\ndeclare @id int; declare @id2 int;\r\n";
                foreach (var f in subjectsSetup.Classes)
                {
                    insertStr += "SET @id = [dbo].GetNewID('Institution.SubjectSetupHeader') " +


                        "INSERT INTO [Institution].[SubjectSetupHeader] (SubjectSetupID,ClassID,StartDate)" +
                                    " VALUES (@id," + f.ClassID +
                                    ",'" + subjectsSetup.StartDate.ToString("g") + "')\r\n";

                    foreach (SubjectsSetupEntryModel entry in subjectsSetup.Entries)
                    {
                        insertStr += "SET @id2 = [dbo].GetNewID('Institution.Subject')\r\n " +
                        "INSERT INTO [Institution].[SubjectSetupDetail] (SubjectSetupID,SubjectID)" +
                           " VALUES (@id,@id2)\r\n" +
                        "INSERT INTO [Institution].[Subject] (SubjectID,NameOfSubject,MaximumScore)" +
                            " VALUES (@id2,'" + entry.NameOfSubject + "','" +
                            entry.MaximumScore + "')\r\n";
                    }
                }
                insertStr += " COMMIT";

                return DataAccessHelper.ExecuteNonQuery(insertStr);
            });
        }

        public static Task<bool> SaveNewClassSetupAsync(ClassesSetupModel classSetup)
        {
            return Task.Run<bool>(() =>
            {
                string insertStr = "BEGIN TRANSACTION\r\n" +
                    "declare @id int; " +
                     "declare @id2 int; " +
                     "SET @id = [dbo].GetNewID('Institution.ClassSetupHeader') " +

                    "INSERT INTO [Institution].[ClassSetupHeader] (ClassSetupID,StartDate)" +
                                " VALUES (@id,'" + classSetup.StartDate.ToString("g") + "')\r\n";

                foreach (ClassesSetupEntryModel entry in classSetup.Entries)
                {
                    insertStr += "IF NOT EXISTS (SELECT * FROM [Institution].[Class] WHERE ClassID=" + entry.ClassID + " AND NameOfClass='" + entry.NameOfClass +
                        "')\r\nBEGIN\r\n" +
                        "SET @id2 = [dbo].GetNewID('Institution.Class')\r\n " +
                        "INSERT INTO [Institution].[Class] (ClassID,NameOfClass)" +
                        " VALUES (@id2,'" + entry.NameOfClass + "')\r\n" +
                    "INSERT INTO [Institution].[ClassSetupDetail] (ClassSetupID,ClassID)" +
                       " VALUES (@id,@id2)\r\n END\r\n" +
                        "ELSE\r\nBEGIN\r\n" +
                        "UPDATE [Institution].[ClassSetupDetail] SET ClassSetupID=@id WHERE ClassID=" + entry.ClassID + "\r\nEND\r\n";

                }
                insertStr += " COMMIT";

                return DataAccessHelper.ExecuteNonQuery(insertStr);
            });
        }

        public static Task<int> GetClassIDFromStudentID(int selectedStudentID)
        {
            return Task.Run<int>(() =>
            {
                string selectStr = "IF EXISTS(SELECT ClassID FROM [Institution].[Student] WHERE StudentID = " + selectedStudentID + ")\r\n" +
                    "SELECT ClassID FROM [Institution].[Student] WHERE StudentID = " + selectedStudentID + "\r\nELSE SELECT 0";
                string res = DataAccessHelper.ExecuteScalar(selectStr);
                int temp = int.Parse(res);
                return temp;
            });
        }

        public static Task<ObservableCollection<ExamModel>> GetExamsByClass(int classID)
        {
            return Task.Run<ObservableCollection<ExamModel>>(async () =>
            {
                ObservableCollection<ExamModel> temp = new ObservableCollection<ExamModel>();
                string selecteStr = "SELECT ecd.ExamID,eh.NameOfExam,eh.ExamDatetime,ISNULL(eh.OutOf,100) FROM [Institution].[ExamHeader] eh LEFT OUTER JOIN" +
                    "[Institution].[ExamClassDetail] ecd ON (ecd.ExamID=eh.ExamID) WHERE ecd.ClassID=" + classID +
                    " AND eh.ExamDateTime>='" + GetTermStart().ToString("g") + "'";
                DataTable dt = DataAccessHelper.ExecuteNonQueryWithResultTable(selecteStr);
                ExamModel em;
                List<Task<KeyValuePair<int, ObservableCollection<ExamSubjectEntryModel>>>> tems =
                    new List<Task<KeyValuePair<int, ObservableCollection<ExamSubjectEntryModel>>>>(dt.Rows.Count);

                foreach (DataRow dtr in dt.Rows)
                {
                    em = new ExamModel();
                    em.ExamID = int.Parse(dtr[0].ToString());
                    em.NameOfExam = dtr[1].ToString();
                    em.OutOf = decimal.Parse(dtr[3].ToString());
                    tems.Add(GetExamEntries(em.ExamID));
                    temp.Add(em);
                }
                KeyValuePair<int, ObservableCollection<ExamSubjectEntryModel>>[] entriesCollection =
                    await Task.WhenAll(tems);
                foreach (ExamModel ems in temp)
                {
                    var entries = from entry in entriesCollection.AsParallel()
                                  where entry.Key == ems.ExamID
                                  select entry;
                    ems.Entries = entries.First().Value;
                }
                return temp;
            });
        }

        public async static Task<ExamModel> GetExamAsync(int examID)
        {
            ExamModel temp = new ExamModel();
            string selecteStr = "SELECT NameOfExam FROM [Institution].[ExamHeader] WHERE ExamID=" + examID;

            DataTable dt = DataAccessHelper.ExecuteNonQueryWithResultTable(selecteStr);
            if (dt.Rows.Count <= 0)
                return temp;
            temp.ExamID = examID;
            temp.NameOfExam = dt.Rows[0][0].ToString();
            temp.Classes = await GetExamClasses(examID);
            temp.Entries = (await GetExamEntries(examID)).Value;
            return temp;
        }

        private static Task<ObservableCollection<ClassModel>> GetExamClasses(int examID)
        {
            return Task.Run<ObservableCollection<ClassModel>>(() =>
            {
                string selectStr = "SELECT ecd.ClassID, c.NameOfClass FROM [Institution].[ExamClassDetail] ecd LEFT OUTER JOIN " +
                    "[Institution].[Class] c ON (ecd.ClassID = c.ClassID) WHERE ecd.ExamID =" + examID;

                DataTable dt = DataAccessHelper.ExecuteNonQueryWithResultTable(selectStr);
                ObservableCollection<ClassModel> entries = new ObservableCollection<ClassModel>();
                ClassModel esem;
                foreach (DataRow dtr in dt.Rows)
                {
                    esem = new ClassModel();
                    esem.ClassID = int.Parse(dtr[0].ToString());
                    esem.NameOfClass = dtr[1].ToString();
                    entries.Add(esem);
                }
                return entries;
            });
        }

        private static Task<KeyValuePair<int, ObservableCollection<ExamSubjectEntryModel>>> GetExamEntries(int examID)
        {
            return Task.Run<KeyValuePair<int, ObservableCollection<ExamSubjectEntryModel>>>(() =>
            {
                string selectStr = "SELECT ed.SubjectID, s.NameOfSubject, ed.ExamDateTime FROM [Institution].[ExamDetail] ed LEFT OUTER JOIN " +
                    "[Institution].[Subject] s ON (ed.SubjectID = s.SubjectID) WHERE ed.ExamID =" + examID;
                KeyValuePair<int, ObservableCollection<ExamSubjectEntryModel>> temp;

                DataTable dt = DataAccessHelper.ExecuteNonQueryWithResultTable(selectStr);
                ObservableCollection<ExamSubjectEntryModel> entries = new ObservableCollection<ExamSubjectEntryModel>();
                ExamSubjectEntryModel esem;
                foreach (DataRow dtr in dt.Rows)
                {
                    esem = new ExamSubjectEntryModel();
                    esem.SubjectID = int.Parse(dtr[0].ToString());
                    esem.NameOfSubject = dtr[1].ToString();
                    esem.ExamDateTime = DateTime.Parse(dtr[2].ToString());
                    entries.Add(esem);
                }
                temp = new KeyValuePair<int, ObservableCollection<ExamSubjectEntryModel>>(examID, entries);
                return temp;
            });
        }

        public static Task<bool> SaveNewExamResultAsync(ExamResultStudentModel newResult)
        {
            return Task.Run<bool>(() =>
            {
                string insertStr = "BEGIN TRANSACTION\r\nDECLARE @id int; SET @id = dbo.GetNewID('Institution.ExamResultHeader')\r\n" +
                    "INSERT INTO [Institution].[ExamResultHeader] (ExamResultID,ExamID,StudentID)" +
                                " VALUES (@id," + newResult.ExamID + "," + newResult.StudentID + ")\r\n";

                foreach (ExamResultSubjectEntryModel entry in newResult.Entries)
                    insertStr += "INSERT INTO [Institution].[ExamResultDetail] (ExamResultID,SubjectID,Score,OutOf,Remarks,Tutor)" +
                        " VALUES (@id," + entry.SubjectID + ",'" + entry.Score + "'," + entry.MaximumScore + ",'" + entry.Remarks + "','" + entry.Tutor + "')\r\n";
                insertStr += " COMMIT";

                return DataAccessHelper.ExecuteNonQuery(insertStr);
            });
        }

        public static Task<bool> SaveNewExamResultAsync(ObservableCollection<ExamResultStudentModel> newResult)
        {
            return Task.Run<bool>(() =>
            {
                string insertStr = "BEGIN TRANSACTION\r\nDECLARE @id int; \r\n ";
                foreach (var c in newResult)
                {
                    insertStr += "SET @id = dbo.GetNewID('Institution.ExamResultHeader')\r\n";
                    insertStr += "IF NOT EXISTS (SELECT * FROM [Institution].[ExamResultHeader] WHERE ExamID=" + c.ExamID + " AND StudentID=" + c.StudentID + " AND IsActive=1)\r\n";
                    insertStr += "INSERT INTO [Institution].[ExamResultHeader] (ExamResultID,ExamID,StudentID)" +
                                " VALUES (@id," + c.ExamID + "," + c.StudentID + ")\r\n";
                    insertStr += "ELSE SET @id=(SELECT ExamResultID FROM [Institution].[ExamResultHeader] WHERE ExamID=" + c.ExamID + " AND StudentID=" + c.StudentID + " AND IsActive=1)\r\n";
                    foreach (ExamResultSubjectEntryModel entry in c.Entries)
                    {
                        insertStr += "IF NOT EXISTS (SELECT * FROM [Institution].[ExamResultDetail] WHERE ExamResultID=@id AND SubjectID=" + entry.SubjectID + ")\r\n";
                        insertStr += "INSERT INTO [Institution].[ExamResultDetail] (ExamResultID,SubjectID,Score,Remarks,Tutor)" +
                            " VALUES (@id," + entry.SubjectID + ",'" + entry.Score + "','" + entry.Remarks + "','" + entry.Tutor + "')\r\n";
                        insertStr += "ELSE UPDATE [Institution].[ExamResultDetail] SET Score='" + entry.Score +
                            "', Remarks='" + entry.Remarks + "', Tutor='" + entry.Tutor + "' WHERE ExamResultID=@id AND SubjectID=" + entry.SubjectID;
                    }
                }
                insertStr += " COMMIT";
                return DataAccessHelper.ExecuteNonQuery(insertStr);
            });
        }

        public static Task<ExamResultStudentModel> GetStudentExamResultAync(int studentID, int examID)
        {
            return Task.Run<ExamResultStudentModel>(() =>
            {
                ExamResultStudentModel temp = new ExamResultStudentModel();
                temp.StudentID = studentID;
                temp.ExamID = examID;
                string selectStr =
                    "IF EXISTS(SELECT ExamResultID FROM [Institution].[ExamResultHeader] WHERE ExamID=" + examID +
                        " AND StudentID=" + studentID + " AND IsActive=1)\r\n SELECT ExamResultID FROM [Institution].[ExamResultHeader] WHERE ExamID=" + examID +
                        " AND StudentID=" + studentID + " AND IsActive=1\r\nELSE SELECT 0";
                string res = DataAccessHelper.ExecuteScalar(selectStr);
                int id = int.Parse(res);
                if (id == 0)
                    return temp;
                temp.ExamResultID = id;
                selectStr = "SELECT erd.SubjectID, s.NameOfSubject, erd.Score, erd.Remarks,erd.Tutor FROM [Institution].[ExamResultDetail] erd " +
                    "LEFT OUTER JOIN [Institution].[Subject] s ON(erd.SubjectID=s.SubjectID) WHERE erd.ExamResultID=" + id;
                ExamResultSubjectEntryModel ersm;
                DataTable dt = DataAccessHelper.ExecuteNonQueryWithResultTable(selectStr);
                foreach (DataRow dtr in dt.Rows)
                {
                    ersm = new ExamResultSubjectEntryModel();
                    ersm.ExamResultID = id;
                    ersm.SubjectID = int.Parse(dtr[0].ToString());
                    ersm.NameOfSubject = dtr[1].ToString();
                    ersm.Remarks = dtr[3].ToString();
                    ersm.Score = decimal.Parse(dtr[2].ToString());
                    ersm.Tutor = dtr[4].ToString();
                    temp.Entries.Add(ersm);
                }
                return temp;
            }
                );
        }

        public static Task<ExamResultClassModel> GetClassExamResultAsync(int classID, int examID,decimal outOf)
        {
            return Task.Run<ExamResultClassModel>(() =>
            {
                ExamResultClassModel temp = new ExamResultClassModel();
                temp.ClassID = classID;
                temp.ExamID = examID;
                string selectStr = "SELECT q.ExamResultID, q.NameOfStudent, q.StudentID, sub.NameOfSubject, q.SubjectID, q.Score  FROM " +
                    "(SELECT p.ExamResultID, s.FirstName +' '+s.MiddleName+ ' '+s.LastName as NameOfStudent, " +
                    "p.StudentID, p.SubjectID, p.Score FROM (SELECT erid.ExamResultID, erid.StudentID, erd.SubjectID, erd.Score FROM " +
                    "(SELECT ExamResultID, StudentID FROM [Institution].[ExamResultHeader] WHERE StudentID IN " +
                    "(SELECT StudentID FROM [Institution].[Student] WHERE ClassID=" + classID + ") AND IsActive=1 AND ExamID=" + examID + ") as erid " +
                    "LEFT OUTER JOIN [Institution].[ExamResultDetail] as erd ON (erid.ExamResultID = erd.ExamResultID)) as p " +
                    "LEFT OUTER JOIN [Institution].[Student] s ON (p.StudentID = s.StudentID) WHERE s.StudentID NOT IN " +
                    "(SELECT StudentID FROM [Institution].[StudentClearance])) as q LEFT OUTER JOIN [Institution].[Subject] sub on(q.SubjectID= sub.SubjectID)";

                DataTable dt = DataAccessHelper.ExecuteNonQueryWithResultTable(selectStr);
                ExamResultStudentModel ersm;
                ExamResultSubjectEntryModel e;
                int testId;
                List<ExamResultSubjectEntryModel> subList = new List<ExamResultSubjectEntryModel>();
                foreach (DataRow dtr in dt.Rows)
                {
                    e = new ExamResultSubjectEntryModel();
                    e.ExamResultID = int.Parse(dtr[0].ToString());
                    e.NameOfSubject = dtr[3].ToString();
                    e.SubjectID = int.Parse(dtr[4].ToString());
                    e.Score = decimal.Parse(dtr[5].ToString());
                    subList.Add(e);

                    testId = int.Parse(dtr[2].ToString());
                    if (!StudentResultAdded(testId, temp.Entries))
                    {
                        ersm = new ExamResultStudentModel();
                        ersm.ExamResultID = e.ExamResultID;
                        ersm.NameOfStudent = dtr[1].ToString();
                        ersm.StudentID = testId;
                        ersm.ExamID = examID;
                        temp.Entries.Add(ersm);
                    }
                }

                foreach (var v in subList)
                {
                    temp.Entries.First(o => o.ExamResultID == v.ExamResultID).Entries.Add(v);
                }

                return temp;
            }
                );
        }

        private static bool StudentResultAdded(int studentID, ObservableCollection<ExamResultStudentModel> entries)
        {
            bool added = false;
            foreach (var res in entries)
                if (res.StudentID == studentID)
                {
                    added = true;
                    break;
                }
            return added;
        }

        public static Task<ClassModel> GetClassAsync(int classID)
        {
            return Task.Run<ClassModel>(() =>
            {
                return GetClass(classID);
            });
        }

        public static ClassModel GetClass(int classID)
        {
            ClassModel temp = new ClassModel();
            string selecteStr = "SELECT ClassID,NameOfClass FROM [Institution].[Class] WHERE ClassID=" + classID;

            DataTable dt = DataAccessHelper.ExecuteNonQueryWithResultTable(selecteStr);
            if (dt.Rows.Count <= 0)
                return temp;
            temp.ClassID = int.Parse(dt.Rows[0][0].ToString());
            temp.NameOfClass = dt.Rows[0][1].ToString();
            return temp;
        }

        public static Task<bool> SaveNewEventAsync(EventModel em)
        {
            return Task.Run<bool>(() =>
            {
                string insertStr = "BEGIN TRANSACTION\r\n" +
                    "INSERT INTO [Institution].[Event] (Name,StartDateTime,EndDateTime,Location,Subject,Message)" +
                    " VALUES ('" + em.Name +
                    "','" + em.StartDateTime.ToString(new CultureInfo("en-GB")) + "','" +
                    em.EndDateTime.ToString("g") + "','" +
                    em.Location + "','" + em.Subject + "','" + em.Message + "')\r\n COMMIT";
                return DataAccessHelper.ExecuteNonQuery(insertStr);
            });
        }

        public static Task<ObservableCollection<EventModel>> GetUpcomingEvents()
        {
            return Task.Run<ObservableCollection<EventModel>>(() =>
            {
                string selectStr = "SELECT Name, StartDateTime, EndDateTime, Location, Subject, Message " +
                    "FROM [Institution].[Event] WHERE StartDateTime >= '" + DateTime.Now.ToString("g") + "'";
                DataTable dt = DataAccessHelper.ExecuteNonQueryWithResultTable(selectStr);
                ObservableCollection<EventModel> temp = new ObservableCollection<EventModel>();
                EventModel em;
                foreach (DataRow dtr in dt.Rows)
                {
                    em = new EventModel();
                    em.Name = dtr[0].ToString();
                    em.StartDateTime = DateTime.Parse(dtr[1].ToString());
                    em.EndDateTime = DateTime.Parse(dtr[2].ToString());
                    em.Location = dtr[3].ToString();
                    em.Subject = dtr[4].ToString();
                    em.Message = dtr[5].ToString();
                    temp.Add(em);
                }
                return temp;
            });
        }

        public static Task<ObservableCollection<EventModel>> GetAllEvents()
        {
            return Task.Run<ObservableCollection<EventModel>>(() =>
            {
                string selectStr = "SELECT Name, StartDateTime, EndDateTime, Location, Subject, Message " +
                    "FROM [Institution].[Event] ORDER BY StartDateTime desc";
                DataTable dt = DataAccessHelper.ExecuteNonQueryWithResultTable(selectStr);
                ObservableCollection<EventModel> temp = new ObservableCollection<EventModel>();
                EventModel em;
                foreach (DataRow dtr in dt.Rows)
                {
                    em = new EventModel();
                    em.Name = dtr[0].ToString();
                    em.StartDateTime = DateTime.Parse(dtr[1].ToString());
                    em.EndDateTime = DateTime.Parse(dtr[2].ToString());
                    em.Location = dtr[3].ToString();
                    em.Subject = dtr[4].ToString();
                    em.Message = dtr[5].ToString();
                    temp.Add(em);
                }
                return temp;
            });
        }

        public static Task<bool> SaveNewStudentTransfersAsync(ObservableCollection<StudentTransferModel> students)
        {
            return Task.Run<bool>(() =>
            {
                string insertStr = "BEGIN TRANSACTION\r\n";
                foreach (var v in students)
                {
                    insertStr += "INSERT INTO [Institution].[StudentTransfer] (StudentID,DateTransferred)" +
                               " VALUES (" + v.StudentID + ",'" + v.DateTransferred.ToString("g") + "')\r\n";
                }
                insertStr += " COMMIT";

                return DataAccessHelper.ExecuteNonQuery(insertStr);
            });
        }

        public static Task<bool> SaveNewStudentClearancesAsync(ObservableCollection<StudentClearancerModel> students)
        {
            return Task.Run<bool>(() =>
            {
                string insertStr = "BEGIN TRANSACTION\r\n";
                foreach (var v in students)
                {
                    insertStr += "INSERT INTO [Institution].[StudentClearance] (StudentID,DateCleared)" +
                               " VALUES (" + v.StudentID + ",'" + v.DateCleared.ToString("g") + "')\r\n";
                }
                insertStr += " COMMIT";

                return DataAccessHelper.ExecuteNonQuery(insertStr);
            });
        }

        public static Task<bool> SaveStudentsActiveAsync(ObservableCollection<StudentClearancerModel> students)
        {
            return Task.Run<bool>(() =>
            {
                string insertStr = "BEGIN TRANSACTION\r\n";
                foreach (var v in students)
                {
                    insertStr += "DELETE FROM [Institution].[StudentClearance] WHERE StudentID=" + v.StudentID + "\r\n";
                    insertStr += "DELETE FROM [Institution].[StudentTransfer] WHERE StudentID=" + v.StudentID + "\r\n";
                }
                insertStr += " COMMIT";

                return DataAccessHelper.ExecuteNonQuery(insertStr);
            });
        }

        public static Task<bool> SetNewStudentsClassAsync(ObservableCollection<StudentBaseModel> students, int classID)
        {
            return Task.Run<bool>(() =>
            {
                string insertStr = "BEGIN TRANSACTION\r\n";
                foreach (var v in students)
                {
                    insertStr += "UPDATE [Institution].[Student] SET ClassID=" + classID + " WHERE StudentID=" + v.StudentID + "\r\n";
                }
                insertStr += " COMMIT";
                return DataAccessHelper.ExecuteNonQuery(insertStr);
            });
        }

        public static Task<bool> SaveNewLeavingCertificateAsync(LeavingCertificateModel leavingCertificate)
        {
            return Task.Run<bool>(() =>
            {
                string insertStr = "BEGIN TRANSACTION\r\n" +
                    "DELETE FROM [Institution].[LeavingCertificate] WHERE StudentID=" + leavingCertificate.StudentID + "\r\n" +
                    "INSERT INTO [Institution].[LeavingCertificate] (LeavingCertificateID,StudentID,DateOfIssue,DateOfBirth,DateOfAdmission,DateOfLeaving" +
                    ",Nationality,ClassEntered,ClassLeft,Remarks)" +
                                " VALUES (dbo.GetNewID('Institution.LeavingCertificate')," + leavingCertificate.StudentID +
                                ",'" + leavingCertificate.DateOfIssue.ToString("g") +
                                "','" + leavingCertificate.DateOfBirth.ToString("d") +
                                "','" + leavingCertificate.DateOfAdmission.ToString("g") +
                                "','" + leavingCertificate.DateOfLeaving.ToString("g") +
                                "','" + leavingCertificate.Nationality +
                                "','" + leavingCertificate.ClassEntered +
                                "','" + leavingCertificate.ClassLeft +
                                "','" + leavingCertificate.Remarks +
                                "')\r\nCOMMIT";

                return DataAccessHelper.ExecuteNonQuery(insertStr);
            });
        }


        public static Task<bool> SaveNewDormitory(DormModel newDormitory)
        {
            return Task.Run<bool>(() =>
            {
                string insertStr = "BEGIN TRANSACTION\r\n" +
                    "INSERT INTO [Institution].[Dormitory] (NameOfDormitory)" +
                                " VALUES ('" + newDormitory.NameOfDormitory + "')" +
                                "\r\nCOMMIT";

                return DataAccessHelper.ExecuteNonQuery(insertStr);
            });
        }


        public static StudentExamResultModel GetStudentExamResult(ExamResultStudentDisplayModel studentResult)
        {
            decimal totalScore = 0;
            int totalPoints = 0;
            StudentExamResultModel st = new StudentExamResultModel();
            st.ClassPosition = GetClassPosition(studentResult.StudentID, studentResult.ExamID);
            st.Entries = new ObservableCollection<StudentTranscriptSubjectModel>();
            foreach (var v in studentResult.Entries)
                st.Entries.Add(new StudentTranscriptSubjectModel(v));
            st.NameOfClass = studentResult.NameOfClass;
            st.NameOfStudent = studentResult.NameOfStudent;
            st.StudentID = studentResult.StudentID;
            st.NameOfExam = studentResult.NameOfExam;
            st.OverAllPosition = GetOverallPosition(studentResult.StudentID, studentResult.ExamID);
            foreach (var v in studentResult.Entries)
                totalScore += v.Score;
            foreach (var f in st.Entries)
                totalPoints += f.Points;

            st.MeanGrade = (st.Entries.Count > 0) ? CalculateGradeFromPoints(((totalPoints + (st.Entries.Count - 1)) / st.Entries.Count)) : "E";
            st.TotalMarks = totalScore;
            st.Points = CalculatePoints(st.MeanGrade);

            return st;
        }

        public static ClassExamResultModel GetClassExamResult(ExamResultClassDisplayModel classResult)
        {
            ClassExamResultModel res = new ClassExamResultModel();
            res.ClassID = classResult.ClassID;
            res.NameOfClass = classResult.NameOfClass;
            res.Entries = classResult.ResultTable;
            return res;
        }

        internal static string CalculateGrade(decimal scoreNew)
        {
            var score = decimal.Ceiling(scoreNew);
            if (score >= 80M && score <= 100M)
                return "A";
            else if (score >= 75M && score <= 79M)
                return "A-";
            else if (score >= 70M && score <= 74M)
                return "B+";
            else if (score >= 65M && score <= 69M)
                return "B";
            else if (score >= 60M && score <= 64M)
                return "B-";
            else if (score >= 55M && score <= 59M)
                return "C+";
            else if (score >= 50M && score <= 54M)
                return "C";
            else if (score >= 45M && score <= 49M)
                return "C-";
            else if (score >= 40M && score <= 44M)
                return "D+";
            else if (score >= 35M && score <= 39M)
                return "D";
            else if (score >= 30M && score <= 34M)
                return "D-";
            else if (score >= 0M && score <= 29M)
                return "E";

            throw new ArgumentOutOfRangeException("Score", "Value [" + score + "] should be a non-negative number less than or equal to 100.");
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
        internal static string CalculateGradeFromPoints(int points)
        {

            switch (points)
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

        private static string GetClassPosition(int studentID, int examID)
        {
            int classID = GetClassIDFromStudent(studentID);

            string selectStr = "SELECT row_no,no_of_students FROM(SELECT ROW_NUMBER() OVER(ORDER BY " +
                "ISNULL(SUM(ISNULL(CONVERT(decimal,erd.Score),0)),0) DESC) row_no, res.StudentID," +
                "(SELECT COUNT(*) FROM [Institution].[Student] WHERE ClassID =" + classID + ")no_of_students" +
                " FROM [Institution].[ExamResultDetail] erd RIGHT OUTER JOIN (SELECT StudentID,ExamResultID " +
                "FROM [Institution].[ExamResultHeader] WHERE IsActive=1 AND ExamID =" + examID +
                " AND StudentID IN (SELECT StudentID FROM [Institution].[Student] WHERE ClassID=" + classID +
                ")) res ON (erd.ExamResultID=res.ExamResultID)" +
                " GROUP BY res.StudentID )x WHERE x.StudentID=" + studentID;
            DataTable dt = DataAccessHelper.ExecuteNonQueryWithResultTable(selectStr);
            if (dt.Rows.Count == 0)
                return "0/0";
            else
                return dt.Rows[0][0].ToString() + "/" + dt.Rows[0][1].ToString();
        }

        private static string GetOverallPosition(int studentID, int examID)
        {
            var form = GetClass(GetClassIDFromStudent(studentID)).NameOfClass[5];

            string selectStr = "SELECT row_no,studs FROM(SELECT ROW_NUMBER() OVER(ORDER BY ISNULL(SUM(ISNULL(CONVERT(decimal,erd.Score),0)),0) DESC) row_no" +
                ", res.StudentID, (SELECT COUNT(*) FROM [Institution].[Student] WHERE ClassID IN(SELECT ClassID FROM [Institution].[Class]" +
                " WHERE NameOfClass LIKE '%" + form + "%'))studs FROM [Institution].[ExamResultDetail] erd RIGHT OUTER JOIN " +
                "(SELECT StudentID,ExamResultID FROM [Institution].[ExamResultHeader] WHERE IsActive=1 AND ExamID =" + examID +
                ") res ON (erd.ExamResultID=res.ExamResultID) GROUP BY res.StudentID)x WHERE x.StudentID=" + studentID;
            DataTable dt = DataAccessHelper.ExecuteNonQueryWithResultTable(selectStr);
            if (dt.Rows.Count == 0)
                return "0/0";
            else
                return dt.Rows[0][0].ToString() + "/" + dt.Rows[0][1].ToString();
        }

        public static Task<bool> HasInvoicedThisTerm(int studentID)
        {
            return Task.Run<bool>(() =>
            {
                int term = GetTerm();

                DateTime? startTime = null;
                DateTime? endTime = null;


                string selectStr = "IF EXISTS(SELECT * FROM [Sales].[SaleHeader] WHERE CustomerID=" + studentID +
                    " AND OrderDate BETWEEN '";
                switch (term)
                {
                    case 1: startTime = new DateTime(DateTime.Now.Year, 1, 1); endTime = new DateTime(DateTime.Now.Year, 4, 30); break;
                    case 2: startTime = new DateTime(DateTime.Now.Year, 5, 1); endTime = new DateTime(DateTime.Now.Year, 8, 31); break;
                    case 3: startTime = new DateTime(DateTime.Now.Year, 9, 1); endTime = new DateTime(DateTime.Now.Year, 12, 31); break;
                }
                selectStr += startTime.Value.Day.ToString() + "/" + startTime.Value.Month.ToString() + "/" + startTime.Value.Year.ToString() + " 00:00:00.000' AND '"
                + endTime.Value.Day.ToString() + "/" + endTime.Value.Month.ToString() + "/" + endTime.Value.Year.ToString() + " 23:59:59.998') SELECT 'True' ELSE SELECT 'False'";

                return bool.Parse(DataAccessHelper.ExecuteScalar(selectStr));
            });
        }

        private static int GetTerm()
        {
            if (DateTime.Now.Month >= 1 && DateTime.Now.Month <= 4)
                return 1;
            else
                if (DateTime.Now.Month >= 5 && DateTime.Now.Month <= 8)
                    return 2;
                else
                    return 3;
        }

        private static DateTime GetTermStart()
        {
            if (DateTime.Now.Month >= 1 && DateTime.Now.Month <= 4)
                return new DateTime(DateTime.Now.Year, 1, 1);
            else
                if (DateTime.Now.Month >= 5 && DateTime.Now.Month <= 8)
                    return new DateTime(DateTime.Now.Year, 5, 1);
                else
                    return new DateTime(DateTime.Now.Year, 9, 1);
        }

        private static DateTime GetTermEnd()
        {
            if (DateTime.Now.Month >= 1 && DateTime.Now.Month <= 4)
                return new DateTime(DateTime.Now.Year, 4, 30, 23, 59, 59);
            else
                if (DateTime.Now.Month >= 5 && DateTime.Now.Month <= 8)
                    return new DateTime(DateTime.Now.Year, 8, 31, 23, 59, 59);
                else
                    return new DateTime(DateTime.Now.Year, 12, 31, 23, 59, 59);
        }

        public static Task<ClassBalancesListModel> GetBalancesList(ClassModel selectedClass)
        {
            return Task.Run<ClassBalancesListModel>(async () =>
            {
                ClassBalancesListModel cf = new ClassBalancesListModel();

                cf.ClassID = selectedClass.ClassID;
                cf.NameOfClass = selectedClass.NameOfClass;
                cf.Date = DateTime.Now;
                cf.Total = 0;
                cf.Entries = await GetClassBalancesListAsync(selectedClass.ClassID);
                cf.Entries.CollectionChanged += (o, e) =>
                {
                    foreach (var v in cf.Entries)
                        cf.Total += v.Balance;
                };
                foreach (var v in cf.Entries)
                    cf.Total += v.Balance;
                return cf;
            });
        }

        private static Task<ObservableCollection<StudentFeesDefaultModel>> GetClassBalancesListAsync(int classID)
        {
            return Task.Run<ObservableCollection<StudentFeesDefaultModel>>(() =>
            {
                ObservableCollection<StudentFeesDefaultModel> temp = new ObservableCollection<StudentFeesDefaultModel>();
                string selectStr = "SELECT StudentID, FirstName+' '+LastName+' '+MiddleName, GuardianPhoneNo,dbo.GetCurrentBalance(StudentID) FROM [Institution].[Student] " +
                    " WHERE ClassID=" + classID + " AND StudentID NOT IN (SELECT StudentID FROM [Institution].[StudentClearance])" +
                    " AND StudentID NOT IN (SELECT StudentID FROM [Institution].[StudentTransfer])";
                DataTable dt = DataAccessHelper.ExecuteNonQueryWithResultTable(selectStr);
                StudentFeesDefaultModel stdf;
                foreach (DataRow dtr in dt.Rows)
                {
                    stdf = new StudentFeesDefaultModel();

                    stdf.StudentID = int.Parse(dtr[0].ToString());
                    stdf.NameOfStudent = dtr[1].ToString();
                    stdf.GuardianPhoneNo = dtr[2].ToString();
                    stdf.Balance = decimal.Parse(dtr[3].ToString());
                    temp.Add(stdf);
                }

                return temp;
            });
        }

        public static Task<ObservableCollection<StudentBaseModel>> GetClassStudents(int classID)
        {
            return Task.Run<ObservableCollection<StudentBaseModel>>(() =>
            {
                ObservableCollection<StudentBaseModel> temp = new ObservableCollection<StudentBaseModel>();
                string selectStr = "SELECT StudentID, FirstName+' '+MiddleName+' '+LastName FROM [Institution].[Student] WHERE ClassID =" + classID +
                    " AND StudentID NOT IN (SELECT StudentID FROM [Institution].[StudentClearance]) AND " +
                    "StudentID NOT IN (SELECT StudentID FROM [Institution].[StudentTransfer])";

                var dt = DataAccessHelper.ExecuteNonQueryWithResultTable(selectStr);
                if (dt.Rows.Count == 0)
                    return temp;
                StudentBaseModel st;
                foreach (DataRow dtr in dt.Rows)
                {
                    st = new StudentBaseModel(int.Parse(dtr[0].ToString()), dtr[1].ToString());
                    temp.Add(st);
                }
                return temp;

            });
        }

        public static Task<SaleModel> GetSaleAsync(int feesPaymentID)
        {
            return Task.Run<SaleModel>(() =>
            {
                SaleModel temp = new SaleModel();
                string selectStr = "SELECT SaleID,CustomerID,EmployeeID,OrderDate,TotalAmt FROM [Sales].[SaleHeader]" +
                    " WHERE PaymentID=" + feesPaymentID;

                DataTable dt = DataAccessHelper.ExecuteNonQueryWithResultTable(selectStr);

                if (dt.Rows.Count > 0)
                {
                    DataRow dtr = dt.Rows[0];
                    temp.SaleID = int.Parse(dtr[0].ToString());
                    temp.CustomerID = int.Parse(dtr[1].ToString());
                    temp.EmployeeID = int.Parse(dtr[2].ToString());
                    temp.DateAdded = DateTime.Parse(dtr[3].ToString());
                    temp.OrderTotal = decimal.Parse(dtr[4].ToString());
                    temp.SaleItems = GetSaleItems(temp.SaleID);
                }

                return temp;

            }); ;
        }

        private static ObservableCollection<FeesStructureEntryModel> GetSaleItems(int saleID)
        {
            ObservableCollection<FeesStructureEntryModel> temp = new ObservableCollection<FeesStructureEntryModel>();
            string selectStr = "SELECT Name,Amount FROM [Sales].[SaleDetail]" +
                       " WHERE SaleID=" + saleID;

            DataTable dt = DataAccessHelper.ExecuteNonQueryWithResultTable(selectStr);
            FeesStructureEntryModel fsem;
            foreach (DataRow dtr in dt.Rows)
            {
                fsem = new FeesStructureEntryModel();
                fsem.Name = dtr[0].ToString();
                fsem.Amount = decimal.Parse(dtr[1].ToString());
                temp.Add(fsem);
            }

            return temp;
        }

        public static Task<SaleModel> GetThisTermInvoice(int studentID)
        {
            return Task.Run<SaleModel>(() =>
            {
                SaleModel temp = new SaleModel();
                int term = GetTerm();

                DateTime? startTime = null;
                DateTime? endTime = null;

                string selectStr = "SELECT SaleID,EmployeeID,PaymentID,OrderDate,TotalAmt FROM [Sales].[SaleHeader]" +
                    " WHERE CustomerID=" + studentID +
                    " AND OrderDate BETWEEN '";
                switch (term)
                {
                    case 1: startTime = new DateTime(DateTime.Now.Year, 1, 1); endTime = new DateTime(DateTime.Now.Year, 4, 30); break;
                    case 2: startTime = new DateTime(DateTime.Now.Year, 5, 1); endTime = new DateTime(DateTime.Now.Year, 8, 31); break;
                    case 3: startTime = new DateTime(DateTime.Now.Year, 9, 1); endTime = new DateTime(DateTime.Now.Year, 12, 31); break;
                }
                selectStr += startTime.Value.Day.ToString() + "/" + startTime.Value.Month.ToString() + "/" + startTime.Value.Year.ToString() + " 00:00:00.000' AND '"
                + endTime.Value.Day.ToString() + "/" + endTime.Value.Month.ToString() + "/" + endTime.Value.Year.ToString() + " 23:59:59.998'";

                DataTable dt = DataAccessHelper.ExecuteNonQueryWithResultTable(selectStr);
                if (dt.Rows.Count > 0)
                {
                    DataRow dtr = dt.Rows[0];
                    temp.SaleID = int.Parse(dtr[0].ToString());
                    temp.CustomerID = studentID;
                    temp.EmployeeID = int.Parse(dtr[1].ToString());
                    temp.PaymentID = int.Parse(dtr[2].ToString());
                    temp.DateAdded = DateTime.Parse(dtr[3].ToString());
                    temp.OrderTotal = decimal.Parse(dtr[4].ToString());
                    temp.SaleItems = GetSaleItems(temp.SaleID);
                }

                return temp;
            });

        }

        public static Task<ClassStudentListModel> GetClassStudentListAsync(ClassModel selectedClass)
        {
            return Task.Run<ClassStudentListModel>(() =>
            {
                ClassStudentListModel temp = new ClassStudentListModel();
                temp.ClassID = selectedClass.ClassID;
                temp.NameOfClass = selectedClass.NameOfClass;
                string selectStr = "SELECT StudentID,FirstName,LastName,MiddleName FROM [Institution].[Student] WHERE ClassID=" + selectedClass.ClassID +
                    " AND StudentID NOT IN (SELECT StudentID FROM [Institution].[StudentClearance])" +
                    " AND StudentID NOT IN (SELECT StudentID FROM [Institution].[StudentTransfer])";
                DataTable dt = DataAccessHelper.ExecuteNonQueryWithResultTable(selectStr);
                StudentBaseModel stud;
                foreach (DataRow dtr in dt.Rows)
                {
                    stud = new StudentBaseModel();
                    stud.StudentID = int.Parse(dtr[0].ToString());
                    stud.NameOfStudent = dtr[2].ToString() + " " + dtr[3].ToString() + " " + dtr[1].ToString();
                    temp.Entries.Add(stud);
                }

                return temp;
            });
        }

        public static Task<ClassStudentListModel> GetCombinedClassStudentListAsync(CombinedClassModel currentClass)
        {
            return Task.Run<ClassStudentListModel>(() =>
            {
                ClassStudentListModel temp = new ClassStudentListModel();
                temp.ClassID = 0;
                temp.NameOfClass = currentClass.Description;
                string selectStr = "SELECT StudentID,FirstName+' '+LastName+' '+MiddleName FROM [Institution].[Student] WHERE" +
                    " ClassID IN (SELECT ClassID FROM [Institution].[Class] WHERE UPPER(NameOfClass) LIKE '%" +
                    currentClass.Description.ToUpper() + "%')";
                DataTable dt = DataAccessHelper.ExecuteNonQueryWithResultTable(selectStr);
                StudentBaseModel stud;
                foreach (DataRow dtr in dt.Rows)
                {
                    stud = new StudentBaseModel();
                    stud.StudentID = int.Parse(dtr[0].ToString());
                    stud.NameOfStudent = dtr[1].ToString();
                    temp.Entries.Add(stud);
                }

                return temp;
            });
        }



        private static ObservableCollection<StudentExamResultEntryModel> GetTranscriptEntries(int studentID, IEnumerable<ExamWeightModel> exams)
        {
            ObservableCollection<StudentExamResultEntryModel> temp = new ObservableCollection<StudentExamResultEntryModel>();

            int e1, e2, e3;
            e1 = 0; e2 = 0; e3 = 0;
            if (exams.Any(o => o.Weight == 1))
                e1 = exams.Where(o => o.Weight == 1).ElementAt(0).ExamID;
            if (exams.Any(o => o.Weight == 2))
                e2 = exams.Where(o => o.Weight == 2).ElementAt(0).ExamID;
            if (exams.Any(o => o.Weight == 3))
                e3 = exams.Where(o => o.Weight == 3).ElementAt(0).ExamID;

            string selectStr = "SELECT sub.NameOfSubject, ISNULL(dbo.GetExamSubjectScore(" + studentID + "," + e1 +
                ",sssd.SubjectID),0),ISNULL(dbo.GetExamSubjectScore(" + studentID + "," + e2 + ",sssd.SubjectID),0),ISNULL(dbo.GetExamSubjectScore(" + 
                studentID + "," + e3 + ",sssd.SubjectID),0),sub.Tutor,sub.Code FROM " +
                "[Institution].[StudentSubjectSelectionDetail] sssd LEFT OUTER JOIN [Institution].[StudentSubjectSelectionHeader] sssh ON " +
                "(sssd.StudentSubjectSelectionID=sssh.StudentSubjectSelectionID) LEFT OUTER JOIN [Institution].[Subject] sub ON (sssd.SubjectID=sub.SubjectID) " +
                "WHERE sssh.StudentID=" + studentID;
            DataTable dt = DataAccessHelper.ExecuteNonQueryWithResultTable(selectStr);
            StudentExamResultEntryModel set;
            foreach (DataRow dtr in dt.Rows)
            {
                set = new StudentExamResultEntryModel();
                set.NameOfSubject = dtr[0].ToString();
                set.Cat1Score = string.IsNullOrWhiteSpace(dtr[1].ToString()) ? null : (decimal?)decimal.Parse(dtr[1].ToString());
                set.Cat2Score = string.IsNullOrWhiteSpace(dtr[2].ToString()) ? null : (decimal?)decimal.Parse(dtr[2].ToString());
                set.ExamScore = string.IsNullOrWhiteSpace(dtr[3].ToString()) ? null : (decimal?)decimal.Parse(dtr[3].ToString());
                set.MeanScore = (decimal)set.Cat1Score + (decimal)set.Cat2Score + (decimal)set.ExamScore;
                set.Code = int.Parse(dtr[4].ToString());
                set.Tutor = dtr[5].ToString();
                set.Grade = CalculateGrade(set.MeanScore);
                set.Points = CalculatePoints(set.Grade);
                temp.Add(set);
            }

            return temp;
        }

        public static Task<bool> SaveNewStudentTranscript(StudentTranscriptModel transcript)
        {
            return Task.Run<bool>(() =>
            {
                bool succ = false;
                try
                {
                    string insertStr = "";
                    if (transcript.StudentTranscriptID != 0)
                    {
                        insertStr = "BEGIN TRANSACTION\r\n" +
                            "IF EXISTS (SELECT * FROM [Institution].[StudentTranscriptHeader] WHERE DateSaved >='" + GetTermStart().ToString("g") + "' AND '" + GetTermEnd().ToString("g") +
                            "' AND StudentTranscriptID=" + transcript.StudentTranscriptID + ")\r\nBEGIN\r\n" +
                            "UPDATE [Institution].[StudentTranscriptHeader] SET Responsibilities='" + transcript.Responsibilities + "'" +
                            ",ClubsAndSport='" + transcript.ClubsAndSport + "'" +
                            ",Boarding='" + transcript.Boarding + "'" +
                            ",ClassTeacher='" + transcript.ClassTeacher + "'" +
                            ",ClassTeacherComments='" + transcript.ClassTeacherComments + "'" +
                            ",Principal='" + transcript.Principal + "'" +
                            ",PrincipalComments='" + transcript.PrincipalComments + "'" +
                            ",OpeningDay='" + transcript.OpeningDay.ToString("g") + "'" +
                            ",ClosingDay='" + transcript.ClosingDay.ToString("g") + "'" +
                            ",DateSaved='" + DateTime.Now.ToString("g") + "' WHERE StudentTranscriptID= " + transcript.StudentTranscriptID +
                            "\r\nEND\r\nELSE\r\nBEGIN\r\n" +
                          "declare @id int; " +
                            "SET @id = [dbo].GetNewID('Institution.StudentTranscriptHeader') " +
                            "INSERT INTO [Institution].[StudentTranscriptHeader] (StudentTranscriptID,StudentID,ClassPosition,OverAllPosition,TotalMarks,Points,MeanGrade," +
                            "Responsibilities,ClubsAndSport,Boarding,ClassTeacher,ClassTeacherComments,Principal,PrincipalComments,OpeningDay,ClosingDay,CAT1Score,CAT2Score," +
                            "ExamScore,Term1Pos,Term2Pos,Term3Pos,DateSaved) VALUES (@id," + transcript.StudentID + ",'" + transcript.ClassPosition + "','" + transcript.OverAllPosition + "','" +
                            transcript.TotalMarks + "'," + transcript.Points + ",'" + transcript.MeanGrade + "','" + transcript.Responsibilities + "','" + transcript.ClubsAndSport + "','" +
                            transcript.Boarding + "','" + transcript.ClassTeacher + "','" + transcript.ClassTeacherComments + "','" + transcript.Principal + "','" + transcript.PrincipalComments + "','" +
                            transcript.OpeningDay.ToString("g") + "','" + transcript.ClosingDay.ToString("g") + "'," + transcript.CAT1Score + "," + transcript.CAT2Score + "," + transcript.ExamScore + ",'" +
                            transcript.Term1Pos + "','" + transcript.Term2Pos + "','" + transcript.Term3Pos + "','" + transcript.DateSaved.ToString("g") + "')\r\nEND\r\nCOMMIT";
                    }
                    return DataAccessHelper.ExecuteNonQuery(insertStr);
                }
                catch { }
                return succ;
            });
        }

        public async static Task<StudentTranscriptModel> GetStudentTranscript(int studentID, IEnumerable<ExamWeightModel> exams, IEnumerable<ClassModel> classes)
        {
            var c =await GetClassIDFromStudentID(studentID);
            int e1,e2,e3;
            e1=0;e2=0;e3=0;
            if (exams.Any(o=>o.Weight==1))
                e1 = exams.Where(o=>o.Weight==1).ElementAt(0).ExamID;
            if (exams.Any(o=>o.Weight==2))
                e2 = exams.Where(o=>o.Weight==2).ElementAt(0).ExamID;
            if (exams.Any(o=>o.Weight==3))
                e3 = exams.Where(o=>o.Weight==3).ElementAt(0).ExamID;

            string cStr = "0,";
            foreach(var t in classes)
                cStr+=t.ClassID+",";
                cStr=cStr.Remove(cStr.Length-1);

            string exStr = "0,";
            foreach(var ex in exams)
                exStr+=ex.ExamID;
            exStr=exStr.Remove(exStr.Length-1);
                string selectStr = "SELECT s.StudentID, s.NameOfStudent, c.NameOfClass,(SELECT CONVERT(varchar(50),row_no)+'/'+CONVERT(varchar(50),no_of_students) "+
                    "FROM (SELECT ROW_NUMBER() OVER(ORDER BY dbo.GetExamTotalScore(StudentID,"+e1+")+dbo.GetExamTotalScore(StudentID,"+e2+
                    ")+dbo.GetExamTotalScore(StudentID,"+e3+") DESC) row_no, StudentID, (SELECT COUNT(*) FROM [Institution].[Student] WHERE ClassID ="+c+
                    ") no_of_students FROM [Institution].[Student] WHERE CLassID="+c+" group by StudentID ) x WHERE x.StudentID=s.StudentID)"+
                    " ClassPosition,(SELECT CONVERT(varchar(50),row_no)+'/'+CONVERT(varchar(50),no_of_students) FROM "+
                    "(SELECT ROW_NUMBER() OVER(ORDER BY ISNULL(dbo.GetExamTotalScore(StudentID,"+e1+"),0)+ISNULL(dbo.GetExamTotalScore(StudentID,"+e2+
                    "),0)+ISNULL(dbo.GetExamTotalScore(StudentID,"+e3+"),0) DESC) row_no, StudentID,(SELECT COUNT(*) FROM [Institution].[Student] "+
                    "WHERE ClassID IN(7,8)) no_of_students FROM [Institution].[Student] WHERE CLassID IN ("+cStr+") GROUP by StudentID ) x "+
                    "WHERE x.StudentID=s.StudentID) OverAllPosition,dbo.GetExamTotalScore(s.StudentID,"+e1+") Exam1Score,"+
                    "dbo.GetExamTotalScore(s.StudentID,"+e2+")Exam2Score,dbo.GetExamTotalScore(s.StudentID,"+e3+")Exam3Score,ISNULL(sth.StudentTranscriptID,0),"+
                    "Responsibilities,ClubsAndSport, Boarding, ClassTeacher,ClassTeacherComments,Principal,PrincipalComments,OpeningDay,ClosingDay,"+
                    "Term1Pos,Term2Pos,Term3Pos,DateSaved FROM [Institution].[Student] s LEFT OUTER JOIN [Institution].[Class] c ON(s.ClassID=c.ClassID) "+
                    "LEFT OUTER JOIN [Institution].[StudentTranscriptHeader] sth ON(sth.StudentID=s.StudentID AND (sth.Exam1ID IN ("+exStr+
                    ") OR sth.Exam2ID IN ("+exStr+") OR sth.Exam3ID IN ("+exStr+"))) WHERE s.StudentID="+studentID;

                DataTable dt = DataAccessHelper.ExecuteNonQueryWithResultTable(selectStr);               
                     
                StudentTranscriptModel temp = new StudentTranscriptModel();
                temp.StudentID =int.Parse( dt.Rows[0][0].ToString());
                temp.NameOfStudent = dt.Rows[0][1].ToString();
                temp.NameOfClass = dt.Rows[0][2].ToString();
                temp.ClassPosition = dt.Rows[0][3].ToString();
                temp.OverAllPosition = dt.Rows[0][4].ToString();

                temp.CAT1Score = string.IsNullOrWhiteSpace(dt.Rows[0][5].ToString()) ? null : (decimal?)decimal.Parse(dt.Rows[0][4].ToString());
                temp.CAT2Score = string.IsNullOrWhiteSpace(dt.Rows[0][6].ToString()) ? null : (decimal?)decimal.Parse(dt.Rows[0][4].ToString());
                temp.ExamScore = string.IsNullOrWhiteSpace(dt.Rows[0][7].ToString()) ? null : (decimal?)decimal.Parse(dt.Rows[0][4].ToString());
                temp.MeanScore = (temp.CAT1Score.HasValue ? temp.CAT1Score.Value : 0) + (temp.CAT2Score.HasValue ? temp.CAT2Score.Value : 0) + (temp.ExamScore.HasValue ? temp.ExamScore.Value : 0);
                temp.StudentTranscriptID = int.Parse(dt.Rows[0][8].ToString());
                temp.Responsibilities = dt.Rows[0][9].ToString();
                temp.ClubsAndSport = dt.Rows[0][10].ToString();
                temp.Boarding = dt.Rows[0][11].ToString();
                temp.ClassTeacher = dt.Rows[0][12].ToString();
                temp.ClassTeacherComments = dt.Rows[0][13].ToString();
                temp.Principal = dt.Rows[0][14].ToString();
                temp.PrincipalComments = dt.Rows[0][15].ToString();
                temp.OpeningDay = string.IsNullOrWhiteSpace(dt.Rows[0][16].ToString())?DateTime.Now: DateTime.Parse(dt.Rows[0][16].ToString());
                temp.ClosingDay = string.IsNullOrWhiteSpace(dt.Rows[0][17].ToString()) ? DateTime.Now : DateTime.Parse(dt.Rows[0][17].ToString());
                temp.Term1Pos = dt.Rows[0][18].ToString();
                temp.Term2Pos = dt.Rows[0][19].ToString();
                temp.Term3Pos = dt.Rows[0][20].ToString();
                temp.DateSaved = string.IsNullOrWhiteSpace(dt.Rows[0][21].ToString()) ? DateTime.Now : DateTime.Parse(dt.Rows[0][21].ToString());

                temp.Entries = GetTranscriptEntries(studentID,exams);
                temp.MeanGrade = GetTranscriptGrade(temp.Entries);
                temp.Points = CalculatePoints(temp.MeanGrade);
                


                
                return temp;
           
        }
        internal static string GetSubjectCode(string nameOfSubject)
        {
            string abbrev = nameOfSubject.ToLowerInvariant().Substring(0, 3);

            switch (abbrev)
            {
                case "eng": return "101";
                case "kis": return "102";
                case "mat": return "121";
                case "bio": return "231";
                case "che": return "233";
                case "phy": return "232";
                case "geo": return "312";
                case "his": return "311";
                case "cre": return "313";
                case "bus": return "565";
                case "agr": return "443";
            }
            return "";
        }
        private static string GetTranscriptGrade(ObservableCollection<StudentExamResultEntryModel> entries)
        {
            return "E";
        }

        
        private static string GetTranscriptGrade(decimal totalMarks, int noOfSubjects)
        {
            decimal avg = noOfSubjects > 0 ? totalMarks / noOfSubjects : 0;
            return CalculateGrade(avg);
        }


        public static Task<bool> SaveNewBookAsync(BookModel book)
        {
            return Task.Run<bool>(() =>
            {
                string INSERTSTR =
                    "INSERT INTO [Institution].[Book] (Name,ISBN,Author,Price,Publisher,SPhoto) " +
                    "VALUES('" +
                    book.Title + "','" +
                    book.ISBN + "','" +
                    book.Author + "'," +
                    book.Price + ",'" +
                    book.Publisher + "',@Photo)";

                ObservableCollection<SqlParameter> paramColl = new ObservableCollection<SqlParameter>();

                paramColl.Add(new SqlParameter("@Photo", book.SPhoto));

                return DataAccessHelper.ExecuteNonQueryWithParameters(INSERTSTR, paramColl);
            });
        }

        public static Task<int> GetLastPaymentIDAsync(int studentID, DateTime datePaid)
        {

            return Task.Run<int>(() =>
            {
                string selectStr =
                    "SELECT FeesPaymentID FROM [Institution].[FeesPayment] WHERE StudentID=" + studentID + " AND DatePaid='" + datePaid.ToString("g") + "'";
                int ft;
                int.TryParse(DataAccessHelper.ExecuteScalar(selectStr), out ft);
                return ft;
            });
        }

        public static Task<bool> SaveNewPaymentVoucher(PaymentVoucherModel newVoucher)
        {
            return Task.Run<bool>(() =>
            {
                string insertStr = "BEGIN TRANSACTION\r\n" +
                    "declare @id int; SET @id = [dbo].GetNewID('Institution.PayoutHeader')\r\n" +

                    "INSERT INTO [Institution].[PayoutHeader] (PayoutID,Payee,Address,TotalPaid)" +
                                " VALUES (@id,'" + newVoucher.NameOfPayee + "','" + newVoucher.Address + "','" + newVoucher.Total + "')\r\n";
                foreach (var d in newVoucher.Entries)
                {
                    insertStr += "INSERT INTO [Institution].[PayoutDetail] (PayoutID,Description,DatePaid,Amount)" +
                        " VALUES(@id,'" + d.Description + "','" + d.DatePaid.ToString("g") + "','" + d.Amount + "')\r\n";
                }

                insertStr += " COMMIT";

                return DataAccessHelper.ExecuteNonQuery(insertStr);
            });
        }


        public static Task<ObservableCollection<PaymentVoucherModel>> GetAllPaymentVouchersAsync()
        {
            return Task.Run<ObservableCollection<PaymentVoucherModel>>(() =>
            {
                ObservableCollection<PaymentVoucherModel> temp = new ObservableCollection<PaymentVoucherModel>();
                string selectStr =
                    "SELECT PayoutID,Payee,Address,TotalPaid FROM [Institution].[PayoutHeader]";
                DataTable dt = DataAccessHelper.ExecuteNonQueryWithResultTable(selectStr);
                PaymentVoucherModel cls;
                foreach (DataRow dtr in dt.Rows)
                {
                    cls = new PaymentVoucherModel();
                    cls.PaymentVoucherID = int.Parse(dtr[0].ToString());
                    cls.NameOfPayee = dtr[1].ToString();
                    cls.Address = dtr[2].ToString();
                    cls.Total = decimal.Parse(dtr[3].ToString());
                    cls.Entries = GetPaymentVoucherEntries(cls.PaymentVoucherID);
                    temp.Add(cls);
                }
                return temp;
            });
        }

        private static ObservableCollection<PaymentVoucherEntryModel> GetPaymentVoucherEntries(int paymentVoucherID)
        {
            ObservableCollection<PaymentVoucherEntryModel> temp = new ObservableCollection<PaymentVoucherEntryModel>();
            string selectStr =
                "SELECT Description,DatePaid,Amount FROM [Institution].[PayoutDetail]";
            DataTable dt = DataAccessHelper.ExecuteNonQueryWithResultTable(selectStr);
            PaymentVoucherEntryModel cls;
            foreach (DataRow dtr in dt.Rows)
            {
                cls = new PaymentVoucherEntryModel();
                cls.Description = dtr[0].ToString();
                cls.DatePaid = DateTime.Parse(dtr[1].ToString());
                cls.Amount = decimal.Parse(dtr[2].ToString());
                temp.Add(cls);
            }
            return temp;
        }

        public static Task<AggregateResultModel> GetAggregateResultAsync(ClassModel selectedClass, ExamModel selectedExam)
        {
            return Task.Run<AggregateResultModel>(() =>
            {
                AggregateResultModel temp = new AggregateResultModel();
                temp.NameOfClass = selectedClass.NameOfClass;
                temp.NameOfExam = selectedExam.NameOfExam;

                string selectStr = "SELECT AVG(ISNULL(dbo.GetWeightedExamTotalScore(s.StudentID,erh.ExamID,100),0)) FROM [Institution].[ExamResultDetail] erd LEFT OUTER JOIN [Institution].[ExamResultHeader] erh ON " +
                    "(erd.ExamResultID=erh.ExamResultID) LEFT OUTER JOIN [Institution].[Student] s ON (erh.StudentID=s.StudentID) WHERE erh.ExamID=" + selectedExam.ExamID + 
                    " AND s.ClassID=" + selectedClass.ClassID + " AND erh.IsActive=1";

                temp.TotalScore = decimal.Parse(DataAccessHelper.ExecuteScalar(selectStr));

                selectStr = "SELECT AVG(ISNULL(dbo.GetWeightedExamSubjectScore(s.StudentID,erh.ExamID,sub.SubjectID,100),0)) FROM [Institution].[ExamResultDetail] erd LEFT OUTER JOIN [Institution].[ExamResultHeader] erh ON " +
                    "(erd.ExamResultID=erh.ExamResultID) LEFT OUTER JOIN [Institution].[Student] s ON (erh.StudentID=s.StudentID)"+
                    " LEFT OUTER JOIN [Institution].[Subject] sub ON (erd.SubjectID=sub.SubjectID) WHERE erh.ExamID=" + selectedExam.ExamID +
                    " AND s.ClassID=" + selectedClass.ClassID + " AND erh.IsActive=1";

                temp.MeanScore = decimal.Parse(DataAccessHelper.ExecuteScalar(selectStr));
                temp.MeanGrade = CalculateGrade(temp.MeanScore);
                temp.Points = CalculatePoints(temp.MeanGrade);
                temp.Entries = GetAggregateResultEntries(selectedClass, selectedExam);
                return temp;
            });
        }

        private static ObservableCollection<AggregateResultEntryModel> GetAggregateResultEntries(ClassModel selectedClass, ExamModel selectedExam)
        {
            ObservableCollection<AggregateResultEntryModel> temp = new ObservableCollection<AggregateResultEntryModel>();

            string selectStr = "SELECT y.SubjectID,y.NameOfSubject, AVG(dbo.GetWeightedExamSubjectScore(erh.StudentID,erh.EXamID,y.SubjectID,100)) FROM " +
                "(SELECT ssd.SubjectID,s.NameOfSubject FROM [Institution].[SubjectSetupDetail] ssd " +
                "LEFT OUTER JOIN [Institution].[Subject] s on (ssd.SubjectID = s.SubjectID) LEFT OUTER JOIN [Institution].[SubjectSetupHeader] ssh on " +
                "(ssh.SubjectSetupID = ssd.SubjectSetupID) WHERE ssh.ClassID=" + selectedClass.ClassID + " AND ssh.IsActive=1) y LEFT OUTER JOIN "+
                "[Institution].[ExamResultDetail] erd ON (y.SubjectID=erd.SubjectID) LEFT OUTER JOIN [Institution].[ExamResultHeader] erh ON (erd.ExamresultID=erh.ExamResultID)"+
                " LEFT OUTER JOIN [Institution].[Student] s ON(erh.StudentID=erh.StudentID) WHERE erh.ExamID=" + selectedExam.ExamID + " AND s.ClassID=" + 
                selectedClass.ClassID + " GROUP BY y.SubjectID,y.NameOfSubject";
            
            DataTable dt = DataAccessHelper.ExecuteNonQueryWithResultTable(selectStr);
            AggregateResultEntryModel cls;
            foreach (DataRow dtr in dt.Rows)
            {
                cls = new AggregateResultEntryModel();
                cls.NameOfSubject = dtr[1].ToString();
                cls.MeanScore = decimal.Parse(dtr[2].ToString());
                cls.MeanGrade = CalculateGrade(cls.MeanScore);
                cls.Points = CalculatePoints(cls.MeanGrade);
                temp.Add(cls);
            }
            return temp;
        }

        public static Task<ObservableCollection<BookModel>> GetAllBooksAsync()
        {
            return Task.Run<ObservableCollection<BookModel>>(() =>
            {
                ObservableCollection<BookModel> temp = new ObservableCollection<BookModel>();
                string selectStr = "SELECT ISBN, Name,Author,Publisher,SPhoto,BookID,Price FROM [Institution].[Book]";
                DataTable dt = DataAccessHelper.ExecuteNonQueryWithResultTable(selectStr);
                BookModel book;
                foreach (DataRow dtr in dt.Rows)
                {
                    book = new BookModel();
                    book.ISBN = dtr[0].ToString();
                    book.Title = dtr[1].ToString();
                    book.Author = dtr[2].ToString();
                    book.Publisher = dtr[3].ToString();
                    book.SPhoto = (dtr[4] != null && !(dtr[4] is DBNull)) ? (byte[])dtr[4] : new byte[0];
                    book.BookID = int.Parse(dtr[5].ToString());
                    book.Price = decimal.Parse(dtr[6].ToString());
                    temp.Add(book);
                }
                return temp;
            });
        }

        public static Task<bool> SaveNewBookIssueAsync(BookIssueModel bim)
        {
            return Task.Run<bool>(() =>
            {
                string insertStr = "BEGIN TRANSACTION\r\n" +
                    "declare @id int; " +
                     "SET @id = [dbo].GetNewID('Institution.BookIssueHeader') " +
                    "INSERT INTO [Institution].[BookIssueHeader] (BookIssueID,StudentID,DateIssued)" +
                                " VALUES (@id," + bim.StudentID + ",'" + bim.DateIssued.ToString("g") + "')\r\n";

                foreach (var entry in bim.Entries)
                {
                    insertStr +=
                    "INSERT INTO [Institution].[BookIssueDetail] (BookIssueID,BookID)" +
                       " VALUES (@id," + entry.BookID + ")\r\n";
                }
                insertStr += " COMMIT";

                return DataAccessHelper.ExecuteNonQuery(insertStr);
            });
        }

        public static Task<ObservableCollection<BookModel>> GetUnreturnedBooksAsync(int studenID)
        {
            return Task.Run<ObservableCollection<BookModel>>(() =>
            {
                ObservableCollection<BookModel> temp = new ObservableCollection<BookModel>();
                string selectStr = "SELECT x.BookID,b.ISBN,b.Name,b.Author,b.Publisher,b.SPhoto,b.Price FROM ((SELECT bid.BookID FROM [Institution].[BookIssueDetail]" +
                    " bid INNER JOIN [Institution].[BookIssueHeader] bih ON(bid.BookIssueID=bih.BookIssueID) " +
                    "WHERE bih.StudentID=" + studenID + " AND NOT EXISTS(SELECT brd.BookID FROM [Institution].[BookReturnDetail] brd " +
                    "INNER JOIN [Institution].[BookReturnHeader] brh ON(brd.BookReturnID=brh.BookReturnID) " +
                    "WHERE brh.DateReturned>bih.DateIssued AND brd.BookID=bid.BookID AND brh.StudentID=" + studenID + ")) x LEFT OUTER JOIN [Institution].[Book] b " +
                    "ON (x.BookID=b.BookID))";
                DataTable dt = DataAccessHelper.ExecuteNonQueryWithResultTable(selectStr);
                BookModel book;
                foreach (DataRow dtr in dt.Rows)
                {
                    book = new BookModel();
                    book.BookID = int.Parse(dtr[0].ToString());
                    book.ISBN = dtr[1].ToString();
                    book.Title = dtr[2].ToString();
                    book.Author = dtr[3].ToString();
                    book.Publisher = dtr[4].ToString();
                    book.SPhoto = (dtr[5] != null && !(dtr[5] is DBNull)) ? (byte[])dtr[5] : new byte[0];
                    book.Price = decimal.Parse(dtr[6].ToString());
                    temp.Add(book);
                }
                return temp;
            });
        }

        public static Task<bool> SaveNewBookReturnAsync(BookReturnModel bim)
        {
            return Task.Run<bool>(() =>
            {
                string insertStr = "BEGIN TRANSACTION\r\n" +
                    "declare @id int; " +
                     "SET @id = [dbo].GetNewID('Institution.BookReturnHeader') " +
                    "INSERT INTO [Institution].[BookReturnHeader] (BookReturnID,StudentID,DateReturned)" +
                                " VALUES (@id," + bim.StudentID + ",'" + bim.DateReturned.ToString("g") + "')\r\n";

                foreach (var entry in bim.Entries)
                {
                    insertStr +=
                    "INSERT INTO [Institution].[BookReturnDetail] (BookReturnID,BookID)" +
                       " VALUES (@id," + entry.BookID + ")\r\n";
                }
                insertStr += " COMMIT";

                return DataAccessHelper.ExecuteNonQuery(insertStr);
            });
        }

        internal static BookModel GetBook(int bookID)
        {

            string selectStr = "SELECT BookID,ISBN,Name,Author,Publisher,SPhoto,b.Price FROM [Institution].[Book] WHERE BookID=" + bookID;
            DataTable dt = DataAccessHelper.ExecuteNonQueryWithResultTable(selectStr);
            if (dt.Rows.Count == 0)
                return new BookModel();
            BookModel book = new BookModel();
            DataRow dtr = dt.Rows[0];
            book = new BookModel();
            book.BookID = int.Parse(dtr[0].ToString());
            book.ISBN = dtr[1].ToString();
            book.Title = dtr[2].ToString();
            book.Author = dtr[3].ToString();
            book.Publisher = dtr[4].ToString();
            book.SPhoto = (dtr[5] != null && !(dtr[5] is DBNull)) ? (byte[])dtr[5] : new byte[0];
            book.Price = decimal.Parse(dtr[6].ToString());
            return book;
        }

        public static Task<bool> UpdateBookAsync(BookSelectModel book)
        {
            return Task.Run<bool>(() =>
            {
                string upDateStr = "UPDATE [Institution].[Book] SET" +
                    " ISBN='" + book.ISBN +
                    "', Name='" + book.Title +
                    "', Author='" + book.Author +
                    "', Publisher='" + book.Publisher +
                    "', Price='" + book.Price +
                    "', SPhoto=@photo WHERE BookID=" + book.BookID;

                ObservableCollection<SqlParameter> paramColl = new ObservableCollection<SqlParameter>();

                paramColl.Add(new SqlParameter("@photo", book.SPhoto));

                bool succ = DataAccessHelper.ExecuteNonQueryWithParameters(upDateStr, paramColl);
                return succ;
            });
        }

        public static Task<bool> AssignStudentNewClass(int studentID, int newClassID)
        {
            return Task.Run<bool>(() =>
            {
                string insertStr = "INSERT INTO [Institution].[CurrentClass] (StudentID,ClassID,IsActive) " +
                    "VALUES(" + studentID + "," + newClassID + ",1)";
                return DataAccessHelper.ExecuteNonQuery(insertStr);
            });
        }

        public static Task<bool> AssignClassNewClass(int classID, int newClassID)
        {
            return Task.Run<bool>(() =>
            {
                string selectStr = "SELECT StudentID FROM [Institution].[Student] WHERE ClassID =" + classID;
                var ss = DataAccessHelper.CopyFromDBtoObservableCollection(selectStr);
                string insertStr = "";
                foreach (string studentID in ss)
                {
                    insertStr += "INSERT INTO [Institution].[CurrentClass] (StudentID,ClassID,IsActive) " +
                    "VALUES(" + studentID + "," + newClassID + ",1)\r\n";
                }
                return DataAccessHelper.ExecuteNonQuery(insertStr);
            });
        }

        internal static StudentBaseModel GetBedNoUser(string bedNo)
        {
            StudentBaseModel currentStudent = new StudentBaseModel();
            try
            {
                string SELECTSTR =
                       "SELECT StudentID, FirstName +' '+LastName+' '+MiddleName FROM [Institution].[Student] WHERE BedNo='" + bedNo + "'";
                DataTable r = DataAccessHelper.ExecuteNonQueryWithResultTable(SELECTSTR);
                if (r.Rows.Count != 0)
                {
                    currentStudent.StudentID = int.Parse(r.Rows[0][0].ToString());
                    currentStudent.NameOfStudent = r.Rows[0][1].ToString();
                }
            }
            catch { }
            return currentStudent;
        }



        public static Task<LeavingCertificateModel> GetStudentLeavingCert(StudentBaseModel student)
        {
            return Task.Run<LeavingCertificateModel>(() =>
            {
                LeavingCertificateModel temp = new LeavingCertificateModel();
                try
                {
                    string SELECTSTR =
                           "SELECT DateOfIssue,DateOfBirth,DateOfAdmission,DateOfLeaving,Nationality,ClassEntered,ClassLeft,Remarks " +
                           "FROM [Institution].[LeavingCertificate] WHERE StudentID=" + student.StudentID;
                    DataTable r = DataAccessHelper.ExecuteNonQueryWithResultTable(SELECTSTR);
                    if (r.Rows.Count != 0)
                    {
                        temp.StudentID = student.StudentID;
                        temp.NameOfStudent = student.NameOfStudent;
                        temp.DateOfIssue = DateTime.Parse(r.Rows[0][0].ToString());
                        temp.DateOfBirth = DateTime.Parse(r.Rows[0][1].ToString());
                        temp.DateOfAdmission = DateTime.Parse(r.Rows[0][2].ToString());
                        temp.DateOfLeaving = DateTime.Parse(r.Rows[0][3].ToString());
                        temp.Nationality = r.Rows[0][4].ToString();
                        temp.ClassEntered = r.Rows[0][5].ToString();
                        temp.ClassLeft = r.Rows[0][6].ToString();
                        temp.Remarks = r.Rows[0][7].ToString();
                    }
                }
                catch { }
                return temp;
            });
        }

        public static Task<ObservableCollection<StudentTranscriptModel>> GetClassTranscriptsAsync(int classID,IEnumerable<ExamWeightModel> exams, IEnumerable<ClassModel> classes)
        {
            return Task.Run<ObservableCollection<StudentTranscriptModel>>(() =>
            {
                ObservableCollection<StudentTranscriptModel> tempCls = new ObservableCollection<StudentTranscriptModel>();
                int e1, e2, e3;
                e1 = 0; e2 = 0; e3 = 0;
                if (exams.Any(o => o.Weight == 1))
                    e1 = exams.Where(o => o.Weight == 1).ElementAt(0).ExamID;
                if (exams.Any(o => o.Weight == 2))
                    e2 = exams.Where(o => o.Weight == 2).ElementAt(0).ExamID;
                if (exams.Any(o => o.Weight == 3))
                    e3 = exams.Where(o => o.Weight == 3).ElementAt(0).ExamID;

                string cStr = "0,";
                foreach (var t in classes)
                    cStr += t.ClassID + ",";
                cStr = cStr.Remove(cStr.Length - 1);

                string exStr = "0,";
                foreach (var ex in exams)
                    exStr += ex.ExamID;
                exStr = exStr.Remove(exStr.Length - 1);
                string selectStr = "SELECT s.StudentID, s.NameOfStudent, c.NameOfClass,(SELECT CONVERT(varchar(50),row_no)+'/'+CONVERT(varchar(50),no_of_students) " +
                    "FROM (SELECT ROW_NUMBER() OVER(ORDER BY dbo.GetExamTotalScore(StudentID," + e1 + ")+dbo.GetExamTotalScore(StudentID," + e2 +
                    ")+dbo.GetExamTotalScore(StudentID," + e3 + ") DESC) row_no, StudentID, (SELECT COUNT(*) FROM [Institution].[Student] WHERE ClassID =" + classID +
                    ") no_of_students FROM [Institution].[Student] WHERE CLassID=" + classID + " group by StudentID ) x WHERE x.StudentID=s.StudentID)" +
                    " ClassPosition,(SELECT CONVERT(varchar(50),row_no)+'/'+CONVERT(varchar(50),no_of_students) FROM " +
                    "(SELECT ROW_NUMBER() OVER(ORDER BY ISNULL(dbo.GetExamTotalScore(StudentID," + e1 + "),0)+ISNULL(dbo.GetExamTotalScore(StudentID," + e2 +
                    "),0)+ISNULL(dbo.GetExamTotalScore(StudentID," + e3 + "),0) DESC) row_no, StudentID,(SELECT COUNT(*) FROM [Institution].[Student] " +
                    "WHERE ClassID IN(7,8)) no_of_students FROM [Institution].[Student] WHERE CLassID IN (" + cStr + ") GROUP by StudentID ) x " +
                    "WHERE x.StudentID=s.StudentID) OverAllPosition,dbo.GetExamTotalScore(s.StudentID," + e1 + ") Exam1Score," +
                    "dbo.GetExamTotalScore(s.StudentID," + e2 + ")Exam2Score,dbo.GetExamTotalScore(s.StudentID," + e3 + ")Exam3Score,ISNULL(sth.StudentTranscriptID,0)," +
                    "Responsibilities,ClubsAndSport, Boarding, ClassTeacher,ClassTeacherComments,Principal,PrincipalComments,OpeningDay,ClosingDay," +
                    "Term1Pos,Term2Pos,Term3Pos,DateSaved FROM [Institution].[Student] s LEFT OUTER JOIN [Institution].[Class] c ON(s.ClassID=c.ClassID) " +
                    "LEFT OUTER JOIN [Institution].[StudentTranscriptHeader] sth ON(sth.StudentID=s.StudentID AND (sth.Exam1ID IN (" + exStr +
                    ") OR sth.Exam2ID IN (" + exStr + ") OR sth.Exam3ID IN (" + exStr + "))) WHERE s.ClassID=" + classID;

                DataTable dt = DataAccessHelper.ExecuteNonQueryWithResultTable(selectStr);

                StudentTranscriptModel temp;
                foreach (DataRow dtr in dt.Rows)
                {
                    temp = new StudentTranscriptModel();
                    temp.StudentID = int.Parse(dtr[0].ToString());
                    temp.NameOfStudent = dtr[1].ToString();
                    temp.NameOfClass = dtr[2].ToString();
                    temp.ClassPosition = dtr[3].ToString();
                    temp.OverAllPosition = dtr[4].ToString();

                    temp.CAT1Score = string.IsNullOrWhiteSpace(dtr[5].ToString()) ? null : (decimal?)decimal.Parse(dtr[4].ToString());
                    temp.CAT2Score = string.IsNullOrWhiteSpace(dtr[6].ToString()) ? null : (decimal?)decimal.Parse(dtr[4].ToString());
                    temp.ExamScore = string.IsNullOrWhiteSpace(dtr[7].ToString()) ? null : (decimal?)decimal.Parse(dtr[4].ToString());
                    temp.MeanScore = (temp.CAT1Score.HasValue ? temp.CAT1Score.Value : 0) + (temp.CAT2Score.HasValue ? temp.CAT2Score.Value : 0) + (temp.ExamScore.HasValue ? temp.ExamScore.Value : 0);
                    temp.StudentTranscriptID = int.Parse(dtr[8].ToString());
                    temp.Responsibilities = dtr[9].ToString();
                    temp.ClubsAndSport = dtr[10].ToString();
                    temp.Boarding = dtr[11].ToString();
                    temp.ClassTeacher = dtr[12].ToString();
                    temp.ClassTeacherComments = dtr[13].ToString();
                    temp.Principal = dtr[14].ToString();
                    temp.PrincipalComments = dtr[15].ToString();
                    temp.OpeningDay = string.IsNullOrWhiteSpace(dtr[16].ToString()) ? DateTime.Now : DateTime.Parse(dtr[16].ToString());
                    temp.ClosingDay = string.IsNullOrWhiteSpace(dtr[17].ToString()) ? DateTime.Now : DateTime.Parse(dtr[17].ToString());
                    temp.Term1Pos = dtr[18].ToString();
                    temp.Term2Pos = dtr[19].ToString();
                    temp.Term3Pos = dtr[20].ToString();
                    temp.DateSaved = string.IsNullOrWhiteSpace(dtr[21].ToString())?DateTime.Now: DateTime.Parse(dtr[21].ToString());

                    temp.Entries = GetTranscriptEntries(temp.StudentID, exams);
                    temp.MeanGrade = GetTranscriptGrade(temp.Entries);
                    temp.Points = CalculatePoints(temp.MeanGrade);
                    tempCls.Add(temp);
                }


                return tempCls;
            });
        }

        public static Task<bool> SaveNewDiscipline(DisciplineModel discipline)
        {
            return Task.Run<bool>(() =>
            {
                string insertStr = "INSERT INTO [Institution].[Discipline] (StudentID,Issue,DateAdded,SPhoto)" +
                            " VALUES (" + discipline.StudentID + ",'" + discipline.Issue + "','"
                            + discipline.DateAdded.ToString("g") + "',@sPhoto)";
                var d = new ObservableCollection<SqlParameter> { new SqlParameter("@sPhoto", discipline.SPhoto) };
                return DataAccessHelper.ExecuteNonQueryWithParameters(insertStr, d);
            });
        }

        public static Task<ObservableCollection<DisciplineModel>> GetStudentDiscipline(StudentBaseModel student, DateTime? start, DateTime? end)
        {
            return Task.Run<ObservableCollection<DisciplineModel>>(() =>
            {
                ObservableCollection<DisciplineModel> temp = new ObservableCollection<DisciplineModel>();
                try
                {
                    string SELECTSTR =
                           "SELECT Issue,DateAdded,SPhoto FROM [Institution].[Discipline] ";
                    if (student.StudentID > 0)
                        SELECTSTR += "WHERE StudentID=" + student.StudentID;
                    if ((start != null) && (end != null))
                    {
                        if (student.StudentID > 0)
                            SELECTSTR += " AND DateAdded BETWEEN '" + start.Value.ToString("dd-MM-yyyy") + " 00:00:00.000' AND '" +
                                end.Value.ToString("dd-MM-yyyy") + " 23:59:59.998'";
                        else
                            SELECTSTR += " WHERE DateAdded BETWEEN '" + start.Value.ToString("dd-MM-yyyy") + " 00:00:00.000' AND '" +
                                    end.Value.ToString("dd-MM-yyyy") + " 23:59:59.998'";
                    }
                    DataTable r = DataAccessHelper.ExecuteNonQueryWithResultTable(SELECTSTR);
                    DisciplineModel dr;
                    foreach (DataRow dtr in r.Rows)
                    {
                        dr = new DisciplineModel();
                        dr.StudentID = student.StudentID;
                        dr.NameOfStudent = student.NameOfStudent;
                        dr.Issue = dtr[0].ToString();
                        dr.DateAdded = DateTime.Parse(dtr[1].ToString());
                        dr.SPhoto = (byte[])dtr[2];
                        temp.Add(dr);
                    }
                }
                catch { }
                return temp;
            });
        }

        public static Task<bool> SaveNewCombinedExamAsync(ExamModel newExam, CombinedClassModel selectedCombinedClass)
        {
            return Task.Run<bool>(() =>
            {
                string insertStr = "BEGIN TRANSACTION\r\nDECLARE @id int; SET @id = dbo.GetNewID('Institution.ExamHeader')\r\n";
                foreach (var f in selectedCombinedClass.Entries)
                {
                    insertStr += "INSERT INTO [Institution].[ExamHeader] (ExamID,ClassID,NameOfExam,ExamDateTime)" +
                                " VALUES (@id," + f.ClassID + ",'" + newExam.NameOfExam + "','" + DateTime.Now.ToString("g") + "')\r\n" +
                                "SET @id = dbo.GetNewID('Institution.ExamHeader')\r\n";

                    foreach (ExamSubjectEntryModel entry in newExam.Entries)
                        insertStr += "INSERT INTO [Institution].[ExamDetail] (ExamID,SubjectID,ExamDateTime)" +
                            " VALUES (@id," + entry.SubjectID +
                            ",'" + entry.ExamDateTime.ToString("g", new CultureInfo("en-GB")) +
                            "')\r\n";
                    insertStr += " COMMIT";
                }

                return DataAccessHelper.ExecuteNonQuery(insertStr);
            });
        }

        public static Task<bool> SaveNewItemIssueAsync(IssueModel newIssue)
        {
            return Task.Run<bool>(() =>
            {
                string insertStr = "BEGIN TRANSACTION\r\nDECLARE @id int; SET @id = dbo.GetNewID('Sales.ItemIssueHeader')\r\n" +
                   "INSERT INTO [Sales].[ItemIssueHeader] (ItemIssueID,Description,DateIssued,IsCancelled) " +
                   "VALUES(@id,'" + newIssue.Description + "','" + newIssue.DateIssued.ToString("g") + "'," +
                   (newIssue.IsCancelled ? "1" : "0") + ")";

                foreach (ItemIssueModel obs in newIssue.Items)
                {
                    insertStr += "\r\nINSERT INTO [Sales].[ItemIssueDetail] (ItemIssueID,ItemID,Quantity) " +
                        "VALUES(@id," + obs.ItemID + "," + obs.Quantity + ")";
                }
                insertStr += "\r\nCOMMIT";
                DataAccessHelper.ExecuteNonQuery(insertStr);
                return true;
            });
        }


        public static Task<ObservableCollection<VoteHeadModel>> GetVoteHeadsSummaryByClass(int classID)
        {
            return Task.Run<ObservableCollection<VoteHeadModel>>(() =>
            {
                DateTime s = GetTermStart();
                DateTime e = GetTermEnd();
                string selectStr = "SELECT sd.Name,ISNULL(SUM(sd.Amount),0) FROM [Sales].[SaleDetail] sd LEFT OUTER JOIN [Sales].[SaleHeader] sh " +
                    "ON (sd.SaleID=sh.SaleID)" +
                    " WHERE CONVERT(INT,CustomerID) IN (SELECT StudentID FROM [Institution].[Student] WHERE CLassID=" + classID +
                    ") AND sh.OrderDate BETWEEN '" +
       s.Day.ToString() + "/" + s.Month.ToString() + "/" + s.Year.ToString() + " 00:00:00.000' AND '"
       + e.Day.ToString() + "/" + e.Month.ToString() + "/" + e.Year.ToString() + " 23:59:59.998' GROUP BY sd.Name";

                DataTable dt = DataAccessHelper.ExecuteNonQueryWithResultTable(selectStr);

                ObservableCollection<VoteHeadModel> temp = new ObservableCollection<VoteHeadModel>();
                VoteHeadModel v;
                foreach (DataRow dtr in dt.Rows)
                {
                    v = new VoteHeadModel();
                    v.Name = dtr[0].ToString();
                    v.Amount = decimal.Parse(dtr[1].ToString());
                    temp.Add(v);
                }

                return temp;
            });
        }

        public static Task<bool> RemoveExam(int examID)
        {
            return Task.Run<bool>(() =>
            {
                string delStr = "BEGIN TRANSACTION\r\n";
                delStr += "DELETE FROM [Institution].[ExamClassDetail] WHERE ExamID=" + examID + "\r\n" +
                        "DELETE FROM [Institution].[ExamResultDetail] WHERE ExamResultID IN (SELECT ExamResultID FROM [Institution].[ExamResultHeader] WHERE ExamID=" + examID + ")\r\n" +
                                "DELETE FROM [Institution].[ExamResultHeader] WHERE ExamID=" + examID + "\r\n" +
                                "DELETE FROM [Institution].[ExamDetail] WHERE ExamID=" + examID + "\r\n" +
                                "DELETE FROM [Institution].[ExamHeader] WHERE ExamID=" + examID + "\r\n";

                delStr += " COMMIT";
                return DataAccessHelper.ExecuteNonQuery(delStr);
            });
        }

        internal static decimal ConvertScoreToOutOf(decimal score, decimal oldOutOf, decimal newOutOf)
        {
            return (newOutOf / oldOutOf) * score;
        }

        public async static Task<ExamResultClassModel> GetClassCombinedExamResultAsync(int classID, ObservableCollection<ExamWeightModel> exams)
        {
            ExamResultClassModel temp = new ExamResultClassModel();
            var subs = await GetSubjectsRegistredToClassAsync(classID);
            string selectStr = "SELECT StudentID, NameOfStudent,";

            foreach (var su in subs)
            {
                selectStr += "(";
                foreach (var ex in exams)
                    selectStr += "dbo.GetWeightedExamSubjectScore(StudentID," + ex.ExamID + "," + su.SubjectID + "," + ex.Weight + ")+";
                selectStr = selectStr.Remove(selectStr.Length - 1);
                selectStr += "),";
            }
            selectStr = selectStr.Remove(selectStr.Length - 1);

            selectStr += " FROM [Institution].[Student] WHERE ClassID=" + classID + " AND IsACtive=1";
            DataTable dt = DataAccessHelper.ExecuteNonQueryWithResultTable(selectStr);
            ExamResultStudentModel stud;
            ExamResultSubjectEntryModel entry;
            foreach(DataRow dtr in dt.Rows)
            {
                stud = new ExamResultStudentModel();
                stud.StudentID = int.Parse(dtr[0].ToString());
                stud.NameOfStudent = dtr[1].ToString();
                for (int i = 2; i < dtr.ItemArray.Length; i++)
                {
                    entry = new ExamResultSubjectEntryModel();
                    entry.SubjectID = subs[i - 2].SubjectID;
                    entry.NameOfSubject = subs[i - 2].NameOfSubject;
                    entry.Code = subs[i - 2].Code;
                    entry.Score = decimal.Parse(dtr[i].ToString());
                    entry.MaximumScore = 100;
                    stud.Entries.Add(entry);
                }
                temp.Entries.Add(stud);
            }
            return temp;

        }

        public async static Task<ExamResultClassModel> GetCombinedClassCombinedExamResultAsync(ObservableCollection<ClassModel> classes, ObservableCollection<ExamWeightModel> exams)
        {
            ExamResultClassModel temp = new ExamResultClassModel();
            var subs = await GetSubjectsRegistredToClassAsync(classes[0].ClassID);
            string selectStr = "SELECT StudentID, NameOfStudent,";
            string classStr = "0,";

            foreach (var c in classes)
                classStr += c.ClassID + ",";
            classStr = classStr.Remove(classStr.Length - 1);
            foreach (var su in subs)
            {
                selectStr += "(";
                foreach (var ex in exams)
                    selectStr += "dbo.GetWeightedExamSubjectScore(StudentID," + ex.ExamID + "," + su.SubjectID + "," + ex.Weight + ")+";
                selectStr = selectStr.Remove(selectStr.Length - 1);
                selectStr += "),";
            }
            selectStr = selectStr.Remove(selectStr.Length - 1);

            selectStr += " FROM [Institution].[Student] WHERE ClassID IN (" + classStr + ") AND IsACtive=1";
            DataTable dt = DataAccessHelper.ExecuteNonQueryWithResultTable(selectStr);
            ExamResultStudentModel stud;
            ExamResultSubjectEntryModel entry;
            foreach (DataRow dtr in dt.Rows)
            {
                stud = new ExamResultStudentModel();
                stud.StudentID = int.Parse(dtr[0].ToString());
                stud.NameOfStudent = dtr[1].ToString();
                for (int i = 2; i < dtr.ItemArray.Length; i++)
                {
                    entry = new ExamResultSubjectEntryModel();
                    entry.SubjectID = subs[i - 2].SubjectID;
                    entry.NameOfSubject = subs[i - 2].NameOfSubject;
                    entry.Code = subs[i - 2].Code;
                    entry.Score = decimal.Parse(dtr[i].ToString());
                    entry.MaximumScore = 100;
                    stud.Entries.Add(entry);
                }
                temp.Entries.Add(stud);
            }
            return temp;

        }
    }
}
