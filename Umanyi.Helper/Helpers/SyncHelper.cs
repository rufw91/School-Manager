using Helper.Models;
using Helper.Models.Sync;
using Helper.Presentation;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Helper
{
    public static class SyncHelper
    {
        static readonly Uri uri = new Uri(((IApp)Application.Current).AppInfo.SyncAddress);

        static List<SyncStudentModel> students = new List<SyncStudentModel>();

        public static async Task Sync(IProgress<SyncOperationProgress> progressReporter)
        {
            await SyncPrepareSync(progressReporter);
            await SyncData(progressReporter);
        }

        private static Task SyncPrepareSync(IProgress<SyncOperationProgress> progressReporter)
        {
            return Task.Run(() =>
                {
                    
                    progressReporter.Report(new SyncOperationProgress() { Completed = false, CurrentItem = 1, Percentage=0 });
                    IEnumerable<string> sts = DataAccessHelper.CopyFromDBtoObservableCollection("SELECT StudentID FROM [Institution].[Student] WHERE IsActive=1");
                    decimal max = sts.Count();
                    decimal index=0;
                    foreach (var s in sts)
                    {
                        index++;
                        AddStudentSyncData(int.Parse(s));
                        progressReporter.Report(new SyncOperationProgress() { Completed = false, CurrentItem = 1, Percentage = ((index*100)/max) });
                    }
                    progressReporter.Report(new SyncOperationProgress() { Completed = true, CurrentItem = 1, Succeeded = true });
                });
        }

        private async static Task SyncData(IProgress<SyncOperationProgress> progressReporter)
        {
            await Task.Run(() =>
                {
                    progressReporter.Report(new SyncOperationProgress() { Completed = false, CurrentItem = 2 });

                    JObject j;

                    bool succ = true;
                    decimal max = students.Count();
                    decimal index = 0;
                    int fails = 0;
                    foreach (var s in students)
                    {
                        index++;
                        try
                        {
                            if (fails > 5)
                                break;
                            var client = new HttpClient();
                            j = new JObject();
                            j["tag"] = "SyncStudents";
                            j["data"] = JToken.FromObject(s);

                            var values = new StringContent(j.ToString(Formatting.Indented), Encoding.UTF8, "application/json");
                            var response = client.PostAsync(uri, values).Result;

                            HttpResponseMessage mes = response.EnsureSuccessStatusCode();
                            string res = mes.Content.ReadAsStringAsync().Result;

                            JObject jo = JObject.Parse(res);
                            succ = succ && true;
                            progressReporter.Report(new SyncOperationProgress() { Completed = false, CurrentItem = 2, Percentage = ((index * 100) / max) });
                        }
                        catch { fails++; succ = succ && false; continue; }
                    }

                    progressReporter.Report(new SyncOperationProgress() { Completed = true, CurrentItem = 2, Succeeded = succ });
                });
        }
        /*
        private static void SyncData2(IProgress<SyncOperationProgress> progressReporter)
        {
            progressReporter.Report(new SyncOperationProgress() { Completed = false, CurrentItem = 2 });

            JObject j;

            bool succ = true;
            foreach (var s in students)
            {
                try
                {
                    j = new JObject();
                    j["tag"] = "SyncStudents";
                    j["data"] = JToken.FromObject(s);

                    string resex="";
                    var req = WebRequest.Create("http://127.0.0.1/mobile_api/");
                    req.Proxy = null;
                    req.Method = "POST";
                    req.ContentType = "application/json";

                    byte[] reqData = Encoding.UTF8.GetBytes(j.ToString(Formatting.Indented));
                    req.ContentLength = reqData.Length;
                    using (Stream reqStream = req.GetRequestStream())
                        reqStream.Write(reqData, 0, reqData.Length);
                    using (WebResponse res = req.GetResponse())
                    using (Stream resSteam = res.GetResponseStream())
                    using (StreamReader sr = new StreamReader(resSteam))
                        resex = sr.ReadToEnd();
                   
                    JObject jo = JObject.Parse(resex);
                    succ = succ && true;
                }
                catch { succ = succ && false; }
            }

            progressReporter.Report(new SyncOperationProgress() { Completed = true, CurrentItem = 2, Succeeded = succ });
        }
        */
        private static void AddStudentSyncData(int studentID)
        {            
            SyncStudentModel ssm = new SyncStudentModel();
            ssm.SchoolID = "mbee";
            var student = DataAccess.GetStudent(studentID);
            ssm.StudentID = student.StudentID.ToString();
            ssm.NameOfStudent = student.NameOfStudent;
            ssm.Address = student.Address;
            ssm.NameOfGuardian = student.NameOfGuardian;
            ssm.GuardianPhoneNo = student.GuardianPhoneNo;
            ssm.City = student.City;
            ssm.FirstName = student.FirstName;
            ssm.MiddleName = student.MiddleName;
            ssm.LastName = student.LastName;
            ssm.SPhoto = (student.SPhoto!=null)?((student.SPhoto.Length<1000)?null:student.SPhoto):null;
            ssm.Email = student.Email;
            ssm.PostalCode = student.PostalCode;
            ssm.PrevInstitution = student.PrevInstitution;
            ssm.BedNo = student.BedNo;
            ssm.NameOfClass = DataAccess.GetClass(student.ClassID).NameOfClass;
            ssm.DateOfAdmission = student.DateOfAdmission.ToString("g");
            ssm.DateOfBirth = student.DateOfBirth.ToString("g");
            //ssm.NameOfDormitory = DataAccess.getd student.DormitoryID;
            ssm.PrevBalance = student.PrevBalance.ToString();
            ssm.IsActive = student.IsActive;
            ssm.IsBoarder = student.IsBoarder;
            ssm.Gender = student.Gender.ToString();
            ssm.KCPEScore = student.KCPEScore.ToString();

            var stmt = DataAccess.GetFeesStatementAsync(studentID, new DateTime(2015, 1, 1), DateTime.Now).Result;
            ssm.Statement = new SyncStatementModel();
            ssm.Statement.TotalDue= stmt.TotalDue.ToString();
            ssm.Statement.TotalPayments = stmt.TotalPayments.ToString();
            ssm.Statement.TotalSales = stmt.TotalSales.ToString();
            ssm.Statement.Transactions = new List<SyncTransactionModel>();

            foreach (var tr in stmt.Transactions)
                ssm.Statement.Transactions.Add(new SyncTransactionModel() 
                {
                    TransactionAmt = tr.TransactionAmt.ToString(),
                    TransactionDateTime = tr.TransactionDateTime.ToString("g"),
                    TransactionType = tr.TransactionType.ToString()
                });

            if (ssm.Statement.TotalDue == "0" && ssm.Statement.TotalPayments == "0" && ssm.Statement.TotalSales == "0" && ssm.Statement.Transactions.Count == 0)
                ssm.Statement = null;

            var t = DataAccess.GetExamsByClass(student.ClassID).Result.OrderByDescending(o => o.ExamDateTime);
            var exRess = new List<SyncExamResultModel>();
            SyncExamResultModel synEx;
            foreach( var e in t)
            {
                synEx = new SyncExamResultModel();
                var pfx= DataAccess.GetStudentExamResultAync(student.StudentID, e.ExamID, e.OutOf).Result;
                synEx.NameOfExam = e.NameOfExam;

                decimal totalScore = 0;
                int totalPoints = 0;

                synEx.ClassPosition = DataAccess.GetClassPosition(student.StudentID, e.ExamID);
                synEx.OverAllPosition = DataAccess.GetOverallPosition(student.StudentID, e.ExamID);
                foreach (var v in pfx.Entries)
                    synEx.Entries.Add(new SyncExamResultEntryModel() 
                    { 
                        Score=v.Score.ToString("N0"), 
                        NameOfSubject = v.NameOfSubject, 
                         Grade = DataAccess.CalculateGrade(decimal.Parse(v.Score.ToString("N0"))),
                          Points = DataAccess.CalculatePoints(DataAccess.CalculateGrade(decimal.Parse(v.Score.ToString("N0")))).ToString(),
                           Remarks = v.Remarks
                    });
                
                foreach (var v in synEx.Entries)
                    totalScore += decimal.Parse(v.Score);
                foreach (var f in synEx.Entries)
                    totalPoints += int.Parse(f.Points);

                synEx.MeanGrade = (synEx.Entries.Count > 0) ? DataAccess.CalculateGradeFromPoints(decimal.Ceiling(totalPoints / synEx.Entries.Count)) : "E";
                synEx.TotalScore = totalScore.ToString();
                synEx.Points = DataAccess.CalculatePoints(synEx.MeanGrade).ToString();
                exRess.Add(synEx);
            }

            ssm.ExamResults = exRess;
            ssm.Transcript = GetStudentTranscript(student.StudentID, t);
            students.Add(ssm);
        }

        private static SyncTranscriptModel GetStudentTranscript(int studentID, IEnumerable<ExamModel> exams)
        {
            SyncTranscriptModel trs = new SyncTranscriptModel();
            string examsStr = "0,";
            foreach (var e in exams)
                examsStr += e.ExamID + ",";
            examsStr = examsStr.Remove(examsStr.Length - 1);

            string selectStr = "SELECT ted.Exam1ID, ted.Exam2ID, ted.Exam3ID,ted.Exam1Weight,ted.Exam2Weight,ted.Exam3Weight " +
                "FROM [Institution].[StudentTranscriptExamDetail] ted LEFT OUTER JOIN " +
                "[Institution].[StudentTranscriptHeader] t ON (ted.StudentTranscriptID=t.StudentTranscriptID) WHERE t.StudentID=434 " +
                "AND t.IsActive=1 AND (ted.Exam1ID IN (" + examsStr + ")OR ted.Exam2ID IN (" + examsStr + ") OR ted.Exam3ID IN (" + examsStr + "))";

             DataTable dt =DataAccessHelper.ExecuteNonQueryWithResultTable(selectStr);
             List<TranscriptInfo> trss = new List<TranscriptInfo>();
             List<ExamWeightModel> examsss = new List<ExamWeightModel>();
            foreach(DataRow dtr in dt.Rows)
            {                
                TranscriptInfo tr = new TranscriptInfo();
                tr.Exam1ID = int.Parse(string.IsNullOrWhiteSpace(dtr[1].ToString()) ? "0" : dtr[0].ToString());
                tr.Exam2ID = int.Parse(string.IsNullOrWhiteSpace(dtr[2].ToString()) ? "0" : dtr[1].ToString());
                tr.Exam3ID = int.Parse(string.IsNullOrWhiteSpace(dtr[3].ToString()) ? "0" : dtr[2].ToString());
                tr.Exam1Weight = decimal.Parse(string.IsNullOrWhiteSpace(dtr[4].ToString()) ? "0" : dtr[3].ToString());
                tr.Exam2Weight = decimal.Parse(string.IsNullOrWhiteSpace(dtr[5].ToString()) ? "0" : dtr[4].ToString());
                tr.Exam3Weight = decimal.Parse(string.IsNullOrWhiteSpace(dtr[6].ToString()) ? "0" : dtr[5].ToString());
                trss.Add(tr);
            }

            if (trss.Count>0)
            {
                if (trss.Any(o=>o.Exam1ID>0))
                examsss.Add(new ExamWeightModel() {
                ExamID = trss[0].Exam1ID,
                Weight = trss[0].Exam1Weight,
                Index = 1
                });
                if (trss.Any(o => o.Exam3ID > 0))
                examsss.Add(new ExamWeightModel()
                {
                    ExamID = trss[0].Exam3ID,
                    Weight = trss[0].Exam3Weight,
                    Index = 3
                });
                if (trss.Any(o => o.Exam2ID > 0))
                examsss.Add(new ExamWeightModel()
                {
                    ExamID = trss[0].Exam2ID,
                    Weight = trss[0].Exam2Weight,
                    Index = 2
                });
            }

            var c = DataAccess.GetClassIDFromStudentID(studentID).Result;

            IEnumerable<ClassModel> classes = new List<ClassModel>();
            var ft = DataAccess.GetAllCombinedClassesAsync().Result;
            var dx = ft.Where(o2 => o2.Entries.Any(o1 => o1.ClassID == c));
            classes = dx.ElementAt(0).Entries;

            if (examsss.Count > 0)
            {
                var newTranscript = DataAccess.GetStudentTranscript2(studentID, examsss, classes).Result;
                if (newTranscript.StudentTranscriptID == 0)
                    return null;

                trs.Term1TotalScore = newTranscript.Term1TotalScore;
                trs.Term2TotalScore = newTranscript.Term2TotalScore;
                trs.Term3TotalScore = newTranscript.Term3TotalScore;
                trs.Term1AvgPts = newTranscript.Term1AvgPts.ToString("N2");
                trs.Term2AvgPts = newTranscript.Term2AvgPts.ToString("N2");
                trs.Term3AvgPts = newTranscript.Term3AvgPts.ToString("N2");

                trs.Term1PtsChange = newTranscript.Term1PtsChange.ToString("N2");
                trs.Term2PtsChange = newTranscript.Term2PtsChange.ToString("N2");
                trs.Term3PtsChange = newTranscript.Term3PtsChange.ToString("N2");

                trs.Term1TotalPoints = newTranscript.Term1TotalPoints;
                trs.Term2TotalPoints = newTranscript.Term2TotalPoints;
                trs.Term3TotalPoints = newTranscript.Term3TotalPoints;
                trs.Term1MeanScore = newTranscript.Term1MeanScore.ToString("N2");
                trs.Term2MeanScore = newTranscript.Term2MeanScore.ToString("N2");
                trs.Term3MeanScore = newTranscript.Term3MeanScore.ToString("N2");

                trs.Term1Grade = newTranscript.Term1Grade;
                trs.Term2Grade = newTranscript.Term2Grade;
                trs.Term3Grade = newTranscript.Term3Grade;

                trs.Term1OverallPos = newTranscript.Term1OverallPos;
                trs.Term2OverallPos = newTranscript.Term2OverallPos;
                trs.Term3OverallPos = newTranscript.Term3OverallPos;

                trs.KCPEScore = newTranscript.KCPEScore;
                trs.ClassTeacherComments = newTranscript.ClassTeacherComments;
                
                trs.PrincipalComments = newTranscript.PrincipalComments;
                trs.OpeningDay = newTranscript.OpeningDay.ToString();
                trs.ClosingDay = newTranscript.ClosingDay.ToString();
                trs.Term1Pos = newTranscript.Term1Pos;
                trs.Term2Pos = newTranscript.Term2Pos;
                trs.Term3Pos = newTranscript.Term3Pos;
                trs.MeanGrade = newTranscript.MeanGrade;
                trs.CAT1Grade = newTranscript.CAT1Grade;
                trs.CAT2Grade = newTranscript.CAT2Grade;
                trs.ExamGrade = newTranscript.ExamGrade;

                trs.Term1Entries = ConvertEntries(newTranscript.Term1Entries);
                trs.Term2Entries = ConvertEntries(newTranscript.Term2Entries);
                trs.Term3Entries = ConvertEntries(newTranscript.Term3Entries);
                trs.PrevYearEntries = ConvertEntries(newTranscript.PrevYearEntries);
            }

            return trs;
        }

        private static List<SyncTranscriptEntryModel> ConvertEntries(IEnumerable<StudentExamResultEntryModel> origEntries)
        {
            List<SyncTranscriptEntryModel> temp = new List<SyncTranscriptEntryModel>();
            if (origEntries==null||origEntries.Count()==0)
            return temp;

            foreach (var e in origEntries)
                temp.Add(new SyncTranscriptEntryModel() {
                 Cat1Score = e.Cat1Score.ToString(),
                 Cat2Score = e.Cat2Score.ToString(),
                 Code = e.Code.ToString(),
                 ExamScore = e.ExamScore.ToString(),
                 Grade = e.Grade,
                MeanScore =  e.MeanScore.ToString("N2"),
                 NameOfSubject = e.NameOfSubject,
                 Points = e.Points.ToString(),
                 Remarks = e.Remarks,
                 Tutor = e.Tutor});
            return temp;
        }

        private class TranscriptInfo
        {
            public int Exam1ID
            { get; set; }

            public int Exam2ID
            { get; set; }

            public int Exam3ID
            { get; set; }
            
            public decimal Exam1Weight
            { get; set; }

            public decimal Exam2Weight
            { get; set; }

            public decimal Exam3Weight
            { get; set; }

        }
    }
}
