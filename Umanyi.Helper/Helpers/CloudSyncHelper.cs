using Helper.Models;
using Helper.Models.Sync;
using Helper.Presentation;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Helper
{
    public class CloudSyncHelper : SyncHelper
    {
        protected enum SyncOptions
        {
            Basic,
            Exams,
            Fees
        }

        private class TranscriptInfo
        {
            public int Exam1ID
            {
                get;
                set;
            }

            public int Exam2ID
            {
                get;
                set;
            }

            public int Exam3ID
            {
                get;
                set;
            }

            public decimal Exam1Weight
            {
                get;
                set;
            }

            public decimal Exam2Weight
            {
                get;
                set;
            }

            public decimal Exam3Weight
            {
                get;
                set;
            }
        }

        private readonly Uri uri = new Uri(((IApp)Application.Current).AppInfo.SyncAddress);

        private List<SyncStudentModel> students = new List<SyncStudentModel>();

        protected List<SyncStudentModel> Students
        {
            get
            {
                return this.students;
            }
        }

        public override async Task Sync(IProgress<SyncOperationProgress> progressReporter)
        {
            await this.SyncPrepareSync(progressReporter, new CloudSyncHelper.SyncOptions[]
            {
                CloudSyncHelper.SyncOptions.Exams,
                CloudSyncHelper.SyncOptions.Fees
            });
            await this.SyncData(progressReporter);
        }

        protected Task SyncPrepareSync(IProgress<SyncOperationProgress> progressReporter, params CloudSyncHelper.SyncOptions[] options)
        {
            return
#if NET4
 Task.Factory.StartNew
#else
                Task.Run
#endif
(delegate
            {
                progressReporter.Report(new SyncOperationProgress
                {
                    Completed = false,
                    CurrentItem = 1,
                    Percentage = 0m
                });
                IEnumerable<string> enumerable = DataAccessHelper.Helper.CopyFirstColumnToList("SELECT StudentID FROM [Institution].[Student] WHERE IsActive=1");
                decimal d = enumerable.Count<string>();
                decimal num = 0m;
                foreach (string current in enumerable)
                {
                    num = ++num;
                    this.AddStudentSyncData(int.Parse(current), options);
                    progressReporter.Report(new SyncOperationProgress
                    {
                        Completed = false,
                        CurrentItem = 1,
                        Percentage = num * 100m / d
                    });
                }
                progressReporter.Report(new SyncOperationProgress
                {
                    Completed = true,
                    CurrentItem = 1,
                    Succeeded = true
                });
            });
        }

        private void AddStudentSyncData(int studentID, params CloudSyncHelper.SyncOptions[] options)
        {
            if (options == null)
            {
                CloudSyncHelper.SyncOptions[] array = new CloudSyncHelper.SyncOptions[1];
                options = array;
            }
            SyncStudentModel syncStudentModel = new SyncStudentModel();
            syncStudentModel.SchoolID = "mbee";
            StudentModel student = DataAccess.GetStudent(studentID);
            syncStudentModel.StudentID = student.StudentID.ToString();
            syncStudentModel.NameOfStudent = student.NameOfStudent;
            syncStudentModel.Address = student.Address;
            syncStudentModel.NameOfGuardian = student.NameOfGuardian;
            syncStudentModel.GuardianPhoneNo = student.GuardianPhoneNo;
            syncStudentModel.City = student.City;
            syncStudentModel.FirstName = student.FirstName;
            syncStudentModel.MiddleName = student.MiddleName;
            syncStudentModel.LastName = student.LastName;
            syncStudentModel.SPhoto = ((student.SPhoto != null) ? ((student.SPhoto.Length < 1000) ? null : student.SPhoto) : null);
            syncStudentModel.Email = student.Email;
            syncStudentModel.PostalCode = student.PostalCode;
            syncStudentModel.PrevInstitution = student.PrevInstitution;
            syncStudentModel.BedNo = student.BedNo;
            syncStudentModel.NameOfClass = DataAccess.GetClass(student.ClassID).NameOfClass;
            syncStudentModel.DateOfAdmission = student.DateOfAdmission.ToString("g");
            syncStudentModel.DateOfBirth = student.DateOfBirth.ToString("g");
            syncStudentModel.PrevBalance = student.PrevBalance.ToString();
            syncStudentModel.IsActive = student.IsActive;
            syncStudentModel.IsBoarder = student.IsBoarder;
            syncStudentModel.Gender = student.Gender.ToString();
            syncStudentModel.KCPEScore = student.KCPEScore.ToString();
            if (options.Contains(CloudSyncHelper.SyncOptions.Fees))
            {
                syncStudentModel.Statement = new SyncStatementModel();
                FeesStatementModel result = DataAccess.GetFeesStatementAsync(studentID, new DateTime?(new DateTime(2015, 1, 1)), new DateTime?(DateTime.Now)).Result;
                syncStudentModel.Statement.TotalDue = result.TotalDue.ToString();
                syncStudentModel.Statement.TotalPayments = result.TotalPayments.ToString();
                syncStudentModel.Statement.TotalSales = result.TotalSales.ToString();
                syncStudentModel.Statement.Transactions = new List<SyncTransactionModel>();
                foreach (TransactionModel current in result.Transactions)
                {
                    syncStudentModel.Statement.Transactions.Add(new SyncTransactionModel
                    {
                        TransactionAmt = current.TransactionAmt.ToString(),
                        TransactionDateTime = current.TransactionDateTime.ToString("g"),
                        TransactionType = current.TransactionType.ToString()
                    });
                }
                if (syncStudentModel.Statement.TotalDue == "0" && syncStudentModel.Statement.TotalPayments == "0" && syncStudentModel.Statement.TotalSales == "0" && syncStudentModel.Statement.Transactions.Count == 0)
                {
                    syncStudentModel.Statement = null;
                }
            }
            if (options.Contains(CloudSyncHelper.SyncOptions.Exams))
            {
                IOrderedEnumerable<ExamModel> orderedEnumerable = from o in new List<ExamModel>()
                                                                  orderby o.ExamDateTime descending
                                                                  select o;
                List<SyncExamResultModel> list = new List<SyncExamResultModel>();
                foreach (ExamModel current2 in orderedEnumerable)
                {
                    SyncExamResultModel syncExamResultModel = new SyncExamResultModel();
                    ExamResultStudentModel result2 = DataAccess.GetStudentExamResultAync(student.StudentID, current2.ExamID, current2.OutOf).Result;
                    syncExamResultModel.NameOfExam = current2.NameOfExam;
                    decimal d = 0m;
                    int num = 0;
                    syncExamResultModel.ClassPosition = DataAccess.GetClassPosition(student.StudentID, current2.ExamID);
                    syncExamResultModel.OverAllPosition = DataAccess.GetOverallPosition(student.StudentID, current2.ExamID);
                    foreach (ExamResultSubjectEntryModel current3 in result2.Entries)
                    {
                        syncExamResultModel.Entries.Add(new SyncExamResultEntryModel
                        {
                            Score = current3.Score.ToString("N0"),
                            NameOfSubject = current3.NameOfSubject,
                            Grade = DataAccess.CalculateGrade(decimal.Parse(current3.Score.ToString("N0"))),
                            Points = DataAccess.CalculatePoints(DataAccess.CalculateGrade(decimal.Parse(current3.Score.ToString("N0")))).ToString(),
                            Remarks = current3.Remarks
                        });
                    }
                    foreach (SyncExamResultEntryModel current4 in syncExamResultModel.Entries)
                    {
                        d += decimal.Parse(current4.Score);
                    }
                    foreach (SyncExamResultEntryModel current5 in syncExamResultModel.Entries)
                    {
                        num += int.Parse(current5.Points);
                    }
                    syncExamResultModel.MeanGrade = ((syncExamResultModel.Entries.Count > 0) ? DataAccess.CalculateGradeFromPoints(decimal.Ceiling(num / syncExamResultModel.Entries.Count)) : "E");
                    syncExamResultModel.TotalScore = d.ToString();
                    syncExamResultModel.Points = DataAccess.CalculatePoints(syncExamResultModel.MeanGrade).ToString();
                    list.Add(syncExamResultModel);
                }
                syncStudentModel.ExamResults = list;
                syncStudentModel.Transcript = this.GetStudentTranscript(student.StudentID, orderedEnumerable);
            }
            this.students.Add(syncStudentModel);
        }

        private SyncTranscriptModel GetStudentTranscript(int studentID, IEnumerable<ExamModel> exams)
        {
            SyncTranscriptModel syncTranscriptModel = new SyncTranscriptModel();
            string text = "0,";
            foreach (ExamModel current in exams)
            {
                text = text + current.ExamID + ",";
            }
            text = text.Remove(text.Length - 1);
            string commandText = string.Concat(new string[]
            {
                "SELECT ted.Exam1ID, ted.Exam2ID, ted.Exam3ID,ted.Exam1Weight,ted.Exam2Weight,ted.Exam3Weight FROM [Institution].[StudentTranscriptExamDetail] ted LEFT OUTER JOIN [Institution].[StudentTranscriptHeader] t ON (ted.StudentTranscriptID=t.StudentTranscriptID) WHERE t.StudentID=434 AND t.IsActive=1 AND (ted.Exam1ID IN (",
                text,
                ")OR ted.Exam2ID IN (",
                text,
                ") OR ted.Exam3ID IN (",
                text,
                "))"
            });
            DataTable dataTable = DataAccessHelper.Helper.ExecuteNonQueryWithResultTable(commandText);
            List<CloudSyncHelper.TranscriptInfo> list = new List<CloudSyncHelper.TranscriptInfo>();
            List<ExamWeightModel> list2 = new List<ExamWeightModel>();
            foreach (DataRow dataRow in dataTable.Rows)
            {
                list.Add(new CloudSyncHelper.TranscriptInfo
                {
                    Exam1ID = int.Parse(string.IsNullOrWhiteSpace(dataRow[1].ToString()) ? "0" : dataRow[0].ToString()),
                    Exam2ID = int.Parse(string.IsNullOrWhiteSpace(dataRow[2].ToString()) ? "0" : dataRow[1].ToString()),
                    Exam3ID = int.Parse(string.IsNullOrWhiteSpace(dataRow[3].ToString()) ? "0" : dataRow[2].ToString()),
                    Exam1Weight = decimal.Parse(string.IsNullOrWhiteSpace(dataRow[4].ToString()) ? "0" : dataRow[3].ToString()),
                    Exam2Weight = decimal.Parse(string.IsNullOrWhiteSpace(dataRow[5].ToString()) ? "0" : dataRow[4].ToString()),
                    Exam3Weight = decimal.Parse(string.IsNullOrWhiteSpace(dataRow[6].ToString()) ? "0" : dataRow[5].ToString())
                });
            }
            if (list.Count > 0)
            {
                if (list.Any((CloudSyncHelper.TranscriptInfo o) => o.Exam1ID > 0))
                {
                    list2.Add(new ExamWeightModel
                    {
                        ExamID = list[0].Exam1ID,
                        Weight = list[0].Exam1Weight,
                        Index = 1
                    });
                }
                if (list.Any((CloudSyncHelper.TranscriptInfo o) => o.Exam3ID > 0))
                {
                    list2.Add(new ExamWeightModel
                    {
                        ExamID = list[0].Exam3ID,
                        Weight = list[0].Exam3Weight,
                        Index = 3
                    });
                }
                if (list.Any((CloudSyncHelper.TranscriptInfo o) => o.Exam2ID > 0))
                {
                    list2.Add(new ExamWeightModel
                    {
                        ExamID = list[0].Exam2ID,
                        Weight = list[0].Exam2Weight,
                        Index = 2
                    });
                }
            }
            int c = DataAccess.GetClassIDFromStudentID(studentID).Result;
            IEnumerable<ClassModel> classes = new List<ClassModel>();
            ObservableCollection<CombinedClassModel> result = DataAccess.GetAllCombinedClassesAsync().Result;
            IEnumerable<CombinedClassModel> source = from o2 in result
                                                     where o2.Entries.Any((ClassModel o1) => o1.ClassID == c)
                                                     select o2;
            classes = source.ElementAt(0).Entries;
            SyncTranscriptModel result3;
            if (list2.Count > 0)
            {
                StudentTranscriptModel2 result2 = null;//DataAccess.GetStudentTranscript2(studentID, list2, classes).Result;
                if (result2.StudentTranscriptID == 0)
                {
                    result3 = null;
                    return result3;
                }
                syncTranscriptModel.Term1TotalScore = result2.Term1TotalScore;
                syncTranscriptModel.Term2TotalScore = result2.Term2TotalScore;
                syncTranscriptModel.Term3TotalScore = result2.Term3TotalScore;
                syncTranscriptModel.Term1AvgPts = result2.Term1AvgPts.ToString("N2");
                syncTranscriptModel.Term2AvgPts = result2.Term2AvgPts.ToString("N2");
                syncTranscriptModel.Term3AvgPts = result2.Term3AvgPts.ToString("N2");
                syncTranscriptModel.Term1PtsChange = result2.Term1PtsChange.ToString("N2");
                syncTranscriptModel.Term2PtsChange = result2.Term2PtsChange.ToString("N2");
                syncTranscriptModel.Term3PtsChange = result2.Term3PtsChange.ToString("N2");
                syncTranscriptModel.Term1TotalPoints = result2.Term1TotalPoints;
                syncTranscriptModel.Term2TotalPoints = result2.Term2TotalPoints;
                syncTranscriptModel.Term3TotalPoints = result2.Term3TotalPoints;
                syncTranscriptModel.Term1MeanScore = result2.Term1MeanScore.ToString("N2");
                syncTranscriptModel.Term2MeanScore = result2.Term2MeanScore.ToString("N2");
                syncTranscriptModel.Term3MeanScore = result2.Term3MeanScore.ToString("N2");
                syncTranscriptModel.Term1Grade = result2.Term1Grade;
                syncTranscriptModel.Term2Grade = result2.Term2Grade;
                syncTranscriptModel.Term3Grade = result2.Term3Grade;
                syncTranscriptModel.Term1OverallPos = result2.Term1OverallPos;
                syncTranscriptModel.Term2OverallPos = result2.Term2OverallPos;
                syncTranscriptModel.Term3OverallPos = result2.Term3OverallPos;
                syncTranscriptModel.KCPEScore = result2.KCPEScore;
                syncTranscriptModel.ClassTeacherComments = result2.ClassTeacherComments;
                syncTranscriptModel.PrincipalComments = result2.PrincipalComments;
                syncTranscriptModel.OpeningDay = result2.OpeningDay.ToString();
                syncTranscriptModel.ClosingDay = result2.ClosingDay.ToString();
                syncTranscriptModel.Term1Pos = result2.Term1Pos;
                syncTranscriptModel.Term2Pos = result2.Term2Pos;
                syncTranscriptModel.Term3Pos = result2.Term3Pos;
                syncTranscriptModel.MeanGrade = result2.MeanGrade;
                syncTranscriptModel.CAT1Grade = result2.CAT1Grade;
                syncTranscriptModel.CAT2Grade = result2.CAT2Grade;
                syncTranscriptModel.ExamGrade = result2.ExamGrade;
                syncTranscriptModel.Term1Entries = this.ConvertEntries(result2.Term1Entries);
                syncTranscriptModel.Term2Entries = this.ConvertEntries(result2.Term2Entries);
                syncTranscriptModel.Term3Entries = this.ConvertEntries(result2.Term3Entries);
                syncTranscriptModel.PrevYearEntries = this.ConvertEntries(result2.PrevYearEntries);
            }
            result3 = syncTranscriptModel;
            return result3;
        }

        private List<SyncTranscriptEntryModel> ConvertEntries(IEnumerable<StudentExamResultEntryModel> origEntries)
        {
            List<SyncTranscriptEntryModel> list = new List<SyncTranscriptEntryModel>();
            List<SyncTranscriptEntryModel> result;
            if (origEntries == null || origEntries.Count<StudentExamResultEntryModel>() == 0)
            {
                result = list;
            }
            else
            {
                foreach (StudentExamResultEntryModel current in origEntries)
                {
                    list.Add(new SyncTranscriptEntryModel
                    {
                        Cat1Score = current.Cat1Score.ToString(),
                        Cat2Score = current.Cat2Score.ToString(),
                        Code = current.Code.ToString(),
                        ExamScore = current.ExamScore.ToString(),
                        Grade = current.Grade,
                        MeanScore = current.MeanScore.ToString("N2"),
                        NameOfSubject = current.NameOfSubject,
                        Points = current.Points.ToString(),
                        Remarks = current.Remarks,
                        Tutor = current.Tutor
                    });
                }
                result = list;
            }
            return result;
        }

        private async Task SyncData(IProgress<SyncOperationProgress> progressReporter)
        {
            await
#if NET4
 Task.Factory.StartNew
#else
                Task.Run
#endif
(delegate
            {
                progressReporter.Report(new SyncOperationProgress
                {
                    Completed = false,
                    CurrentItem = 2
                });
                bool flag = true;
                decimal d = this.Students.Count<SyncStudentModel>();
                decimal num = 0m;
                int num2 = 0;
                foreach (SyncStudentModel current in this.Students)
                {
                    num = ++num;
                    try
                    {
                        if (num2 > 5)
                        {
                            break;
                        }
                        HttpClient httpClient = new HttpClient();
                        JObject jObject = new JObject();
                        jObject["tag"] = "SyncStudents";
                        jObject["data"] = JToken.FromObject(current);
                        StringContent content = new StringContent(jObject.ToString(Formatting.Indented, new JsonConverter[0]), Encoding.UTF8, "application/json");
                        HttpResponseMessage result = httpClient.PostAsync(this.uri, content).Result;
                        HttpResponseMessage httpResponseMessage = result.EnsureSuccessStatusCode();
                        string result2 = httpResponseMessage.Content.ReadAsStringAsync().Result;
                        JObject jObject2 = JObject.Parse(result2);
                        progressReporter.Report(new SyncOperationProgress
                        {
                            Completed = false,
                            CurrentItem = 2,
                            Percentage = num * 100m / d
                        });
                    }
                    catch
                    {
                        num2++;
                        flag = false;
                    }
                }
                progressReporter.Report(new SyncOperationProgress
                {
                    Completed = true,
                    CurrentItem = 2,
                    Succeeded = flag
                });
            });
        }
    }
}
