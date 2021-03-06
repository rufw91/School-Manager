﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Data.SqlClient;
using System.Threading.Tasks;
using UmanyiSMS.Lib.Controllers;
using UmanyiSMS.Lib.Presentation;
using UmanyiSMS.Modules.Exams.Models;
using UmanyiSMS.Modules.Institution.Models;

namespace UmanyiSMS.Modules.Exams.Controller
{
    public class DataController
    {
        public static Task<ObservableCollection<ExamModel>> GetExamsByClass(int classID, TermModel term)
        {
            return Task.Factory.StartNew<ObservableCollection<ExamModel>>(delegate
            {
                ObservableCollection<ExamModel> observableCollection = new ObservableCollection<ExamModel>();
                string commandText = "SELECT ecd.ExamID,eh.NameOfExam,eh.ExamDatetime,ISNULL(eh.OutOf,100) FROM [ExamHeader] eh " +
                    "LEFT OUTER JOIN[ExamClassDetail] ecd ON (ecd.ExamID=eh.ExamID) WHERE ecd.ClassID=@classID " +
                    " AND eh.ExamDateTime >= @startD AND eh.ExamDateTime < @endD";
                var paramColl = new List<SqlParameter>();
                paramColl.Add(new SqlParameter("@startD", term.StartDate.Date));
                paramColl.Add(new SqlParameter("@endD", term.EndDate.Date.AddDays(1)));
                paramColl.Add(new SqlParameter("@classID", classID));
                DataTable dataTable = DataAccessHelper.Helper.ExecuteNonQueryWithResultTable(commandText, paramColl);

                foreach (DataRow dataRow in dataTable.Rows)
                {
                    ExamModel examModel = new ExamModel();
                    examModel.ExamID = int.Parse(dataRow[0].ToString());
                    examModel.NameOfExam = dataRow[1].ToString();
                    examModel.OutOf = decimal.Parse(dataRow[3].ToString());
                    examModel.Entries = GetExamEntries(examModel.ExamID, examModel.OutOf).Result;
                    observableCollection.Add(examModel);
                }
                return observableCollection;
            });
        }

        private static Task<ObservableCollection<SubjectModel>> GetExamEntries(int examID, decimal outOf)
        {
            return Task.Factory.StartNew<ObservableCollection<SubjectModel>>(delegate
            {
                string commandText = "SELECT ed.SubjectID, s.NameOfSubject FROM [ExamDetail] ed LEFT OUTER JOIN [Subject] s ON (ed.SubjectID = s.SubjectID) WHERE ed.ExamID =" + examID + " ORDER BY s.[Code]";
                DataTable dataTable = DataAccessHelper.Helper.ExecuteNonQueryWithResult(commandText);
                var observableCollection = new ObservableCollection<SubjectModel>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    observableCollection.Add(new SubjectModel
                    {
                        SubjectID = int.Parse(dataRow[0].ToString()),
                        NameOfSubject = dataRow[1].ToString(),
                        MaximumScore = outOf
                    });
                }
                return observableCollection;
            });
        }

        public static async Task<ExamModel> GetExamAsync(int examID)
        {
            ExamModel examModel = new ExamModel();
            string commandText = "SELECT NameOfExam,OutOf FROM [ExamHeader] WHERE ExamID=" + examID;
            DataTable dataTable = DataAccessHelper.Helper.ExecuteNonQueryWithResult(commandText);
            ExamModel result;
            if (dataTable.Rows.Count <= 0)
            {
                result = examModel;
            }
            else
            {
                examModel.ExamID = examID;
                examModel.NameOfExam = dataTable.Rows[0][0].ToString();
                examModel.Classes = await GetExamClasses(examID);
                examModel.OutOf = decimal.Parse(dataTable.Rows[0][1].ToString());
                examModel.Entries = await GetExamEntries(examID, examModel.OutOf);
                result = examModel;
            }
            return result;
        }

        internal static decimal ConvertScoreToOutOf(decimal score, decimal oldOutOf, decimal newOutOf)
        {
            return newOutOf / oldOutOf * score;
        }

        private static Task<ObservableCollection<ClassModel>> GetExamClasses(int examID)
        {
            return Task.Factory.StartNew<ObservableCollection<ClassModel>>(delegate
            {
                string commandText = "SELECT ecd.ClassID, c.NameOfClass FROM [ExamClassDetail] ecd LEFT OUTER JOIN [Class] c ON (ecd.ClassID = c.ClassID) WHERE ecd.ExamID =" + examID;
                DataTable dataTable = DataAccessHelper.Helper.ExecuteNonQueryWithResult(commandText);
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

        public static Task<AggregateResultModel> GetAggregateResultAsync(ClassModel selectedClass, ExamModel selectedExam)
        {
            return Task.Factory.StartNew<AggregateResultModel>(delegate
            {
                AggregateResultModel aggregateResultModel = new AggregateResultModel();
                aggregateResultModel.NameOfClass = selectedClass.NameOfClass;
                aggregateResultModel.NameOfExam = selectedExam.NameOfExam;
                string commandText = string.Concat(new object[]
                {
                    "SELECT ISNULL(AVG(x.[Average]),0) FROM (SELECT sub.SubjectID,sub.NameOfSubject,ROUND(AVG(erd.Score),4) [Average] FROM [ExamDetail] ed INNER JOIN [StudentSubjectSelectionDetail] sssd on (sssd.SubjectID = ed.SubjectID) LEFT OUTER JOIN [ExamHeader] eh ON (eh.ExamID=ed.ExamID) LEFT OUTER JOIN [ExamResultHeader] erh ON (erh.ExamID=eh.ExamID) INNER JOIN [StudentSubjectSelectionHeader] sssh on (sssh.StudentID = erh.StudentID AND sssd.StudentSubjectSelectionID= sssh.StudentSubjectSelectionID) INNER JOIN [ExamResultDetail] erd ON (sssd.SubjectID=erd.SubjectID AND erd.ExamResultID=erh.ExamResultID) LEFT OUTER JOIN [Subject] sub ON(sssd.SubjectID=sub.SubjectID) LEFT OUTER JOIN [StudentClass] sc ON(erh.StudentID=sc.StudentID) WHERE sc.ClassID=",
                    selectedClass.ClassID,
                    " AND sc.[Year]=DATEPART(year,SYSDATETIME()) AND sssh.[Year]=DATEPART(year,SYSDATETIME()) AND erh.ExamID=",
                    selectedExam.ExamID,
                    " GROUP BY sub.SubjectID,sub.NameOfSubject) x"
                });
                aggregateResultModel.MeanScore = decimal.Parse(DataAccessHelper.Helper.ExecuteScalar(commandText));
                aggregateResultModel.MeanGrade = Institution.Controller.DataController.CalculateGrade(decimal.Parse((aggregateResultModel.MeanScore * 100m / selectedExam.OutOf).ToString("N2")));
                aggregateResultModel.Points = Institution.Controller.DataController.CalculatePoints(aggregateResultModel.MeanGrade);
                aggregateResultModel.Entries = GetAggregateResultEntries(selectedClass, selectedExam);
                return aggregateResultModel;
            });
        }

        private static ObservableCollection<AggregateResultEntryModel> GetAggregateResultEntries(ClassModel selectedClass, ExamModel selectedExam)
        {
            ObservableCollection<AggregateResultEntryModel> observableCollection = new ObservableCollection<AggregateResultEntryModel>();
            string commandText = string.Concat(new object[]
            {
                "SELECT sub.SubjectID,sub.NameOfSubject,ROUND(AVG(erd.Score),4) FROM [ExamDetail] ed INNER JOIN [StudentSubjectSelectionDetail] sssd on (sssd.SubjectID = ed.SubjectID) LEFT OUTER JOIN [ExamHeader] eh ON (eh.ExamID=ed.ExamID) LEFT OUTER JOIN [ExamResultHeader] erh ON (erh.ExamID=eh.ExamID) INNER JOIN [StudentSubjectSelectionHeader] sssh on (sssh.StudentID = erh.StudentID AND sssd.StudentSubjectSelectionID= sssh.StudentSubjectSelectionID) INNER JOIN [ExamResultDetail] erd ON (sssd.SubjectID=erd.SubjectID AND erd.ExamResultID=erh.ExamResultID) LEFT OUTER JOIN [Subject] sub ON(sssd.SubjectID=sub.SubjectID) LEFT OUTER JOIN [StudentClass] sc ON(erh.StudentID=sc.StudentID) WHERE sc.ClassID=",
                selectedClass.ClassID,
                " AND sc.[Year]=DATEPART(year,SYSDATETIME()) AND sssh.[Year]=DATEPART(year,SYSDATETIME()) AND erh.ExamID=",
                selectedExam.ExamID,
                " GROUP BY sub.SubjectID,sub.NameOfSubject ORDER BY ROUND(AVG(erd.Score),4) DESC"
            });
            DataTable dataTable = DataAccessHelper.Helper.ExecuteNonQueryWithResult(commandText);
            foreach (DataRow dataRow in dataTable.Rows)
            {
                AggregateResultEntryModel aggregateResultEntryModel = new AggregateResultEntryModel();
                aggregateResultEntryModel.NameOfSubject = dataRow[1].ToString();
                aggregateResultEntryModel.MeanScore = decimal.Parse(dataRow[2].ToString());
                aggregateResultEntryModel.MeanGrade =Institution.Controller.DataController.CalculateGrade(decimal.Parse((aggregateResultEntryModel.MeanScore * 100m / selectedExam.OutOf).ToString("N2")));
                aggregateResultEntryModel.Points = Institution.Controller.DataController.CalculatePoints(aggregateResultEntryModel.MeanGrade);
                observableCollection.Add(aggregateResultEntryModel);
            }
            return observableCollection;
        }


        private static ObservableCollection<AggregateResultEntryModel> GetAggregateResultEntries(ObservableCollection<ClassModel> classes, ExamModel selectedExam)
        {
            ObservableCollection<AggregateResultEntryModel> observableCollection = new ObservableCollection<AggregateResultEntryModel>();
            string commandText = "SELECT sub.SubjectID,sub.NameOfSubject,ROUND(AVG(erd.Score),4) FROM [ExamDetail] ed INNER JOIN [StudentSubjectSelectionDetail] sssd on (sssd.SubjectID = ed.SubjectID) LEFT OUTER JOIN [ExamHeader] eh ON (eh.ExamID=ed.ExamID) LEFT OUTER JOIN [ExamResultHeader] erh ON (erh.ExamID=eh.ExamID) INNER JOIN [StudentSubjectSelectionHeader] sssh on (sssh.StudentID = erh.StudentID AND sssd.StudentSubjectSelectionID= sssh.StudentSubjectSelectionID) INNER JOIN [ExamResultDetail] erd ON (sssd.SubjectID=erd.SubjectID AND erd.ExamResultID=erh.ExamResultID) LEFT OUTER JOIN [Subject] sub ON(sssd.SubjectID=sub.SubjectID) LEFT OUTER JOIN [StudentClass] sc ON(erh.StudentID=sc.StudentID) WHERE sc.[Year]=DATEPART(year,SYSDATETIME()) AND sssh.[Year]=DATEPART(year,SYSDATETIME())  AND erh.ExamID=" + selectedExam.ExamID + " GROUP BY sub.SubjectID,sub.NameOfSubject ORDER BY ROUND(AVG(erd.Score),4) DESC";
            DataTable dataTable = DataAccessHelper.Helper.ExecuteNonQueryWithResult(commandText);
            foreach (DataRow dataRow in dataTable.Rows)
            {
                AggregateResultEntryModel aggregateResultEntryModel = new AggregateResultEntryModel();
                aggregateResultEntryModel.NameOfSubject = dataRow[1].ToString();
                aggregateResultEntryModel.MeanScore = decimal.Parse(dataRow[2].ToString());
                aggregateResultEntryModel.MeanGrade = Institution.Controller.DataController.CalculateGrade(decimal.Parse((aggregateResultEntryModel.MeanScore * 100m / selectedExam.OutOf).ToString("N2")));
                aggregateResultEntryModel.Points = Institution.Controller.DataController.CalculatePoints(aggregateResultEntryModel.MeanGrade);
                observableCollection.Add(aggregateResultEntryModel);
            }
            return observableCollection;
        }


        public static Task<AggregateResultModel> GetAggregateResultAsync(CombinedClassModel selectedCombinedClass, ExamModel selectedExam)
        {
            return Task.Factory.StartNew(delegate
            {
                AggregateResultModel aggregateResultModel = new AggregateResultModel();
                aggregateResultModel.NameOfClass = selectedCombinedClass.Description;
                aggregateResultModel.NameOfExam = selectedExam.NameOfExam;
                string commandText = "SELECT ISNULL(AVG(x.[Average]),0) FROM (SELECT sub.SubjectID,sub.NameOfSubject,ROUND(AVG(erd.Score),4) [Average] FROM [ExamDetail] ed INNER JOIN [StudentSubjectSelectionDetail] sssd on (sssd.SubjectID = ed.SubjectID) LEFT OUTER JOIN [ExamHeader] eh ON (eh.ExamID=ed.ExamID) LEFT OUTER JOIN [ExamResultHeader] erh ON (erh.ExamID=eh.ExamID) INNER JOIN [StudentSubjectSelectionHeader] sssh on (sssh.StudentID = erh.StudentID AND sssd.StudentSubjectSelectionID= sssh.StudentSubjectSelectionID) INNER JOIN [ExamResultDetail] erd ON (sssd.SubjectID=erd.SubjectID AND erd.ExamResultID=erh.ExamResultID) LEFT OUTER JOIN [Subject] sub ON(sssd.SubjectID=sub.SubjectID) LEFT OUTER JOIN [StudentClass] sc ON(erh.StudentID=sc.StudentID) WHERE sc.[Year]=DATEPART(year,SYSDATETIME()) AND sssh.[Year]=DATEPART(year,SYSDATETIME())  AND erh.ExamID=" + selectedExam.ExamID + " GROUP BY sub.SubjectID,sub.NameOfSubject) x";
                aggregateResultModel.MeanScore = decimal.Parse(DataAccessHelper.Helper.ExecuteScalar(commandText));
                aggregateResultModel.MeanGrade =Institution.Controller.DataController.CalculateGrade(decimal.Parse((aggregateResultModel.MeanScore * 100m / selectedExam.OutOf).ToString("N2")));
                aggregateResultModel.Points = Institution.Controller.DataController.CalculatePoints(aggregateResultModel.MeanGrade);
                aggregateResultModel.Entries = GetAggregateResultEntries(selectedCombinedClass.Entries, selectedExam);
                return aggregateResultModel;
            });
        }

        public static Task<AggregateResultModel> GetCombinedAggregateResultAsync(ClassModel selectedClass, ObservableCollection<ExamWeightModel> exams)
        {
            return Task.Factory.StartNew<AggregateResultModel>(delegate
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
                        "/eh.OutOf)),4) [Average] FROM [ExamDetail] ed INNER JOIN [StudentSubjectSelectionDetail] sssd on (sssd.SubjectID = ed.SubjectID) LEFT OUTER JOIN [ExamHeader] eh ON (eh.ExamID=ed.ExamID) LEFT OUTER JOIN [ExamResultHeader] erh ON (erh.ExamID=eh.ExamID) INNER JOIN [StudentSubjectSelectionHeader] sssh on (sssh.StudentID = erh.StudentID AND sssd.StudentSubjectSelectionID= sssh.StudentSubjectSelectionID) INNER JOIN [ExamResultDetail] erd ON (sssd.SubjectID=erd.SubjectID AND erd.ExamResultID=erh.ExamResultID) LEFT OUTER JOIN [Subject] sub ON(sssd.SubjectID=sub.SubjectID) LEFT OUTER JOIN [StudentClass] sc ON(erh.StudentID=sc.StudentID) WHERE sc.ClassID=",
                        selectedClass.ClassID,
                        " AND sc.[Year]=DATEPART(year,SYSDATETIME()) AND sssh.[Year]=DATEPART(year,SYSDATETIME())  AND erh.ExamID=",
                        current.ExamID,
                        " GROUP BY sub.SubjectID,sub.NameOfSubject) x"
                    });
                    aggregateResultModel.MeanScore += decimal.Parse(DataAccessHelper.Helper.ExecuteScalar(commandText));
                }
                aggregateResultModel.MeanGrade = Institution.Controller.DataController.CalculateGrade(aggregateResultModel.MeanScore);
                aggregateResultModel.Points = Institution.Controller.DataController.CalculatePoints(aggregateResultModel.MeanGrade);
                aggregateResultModel.Entries = GetCombinedAggregateResultEntries(selectedClass, exams);
                return aggregateResultModel;
            });
        }

        public static Task<AggregateResultModel> GetCombinedAggregateResultAsync(CombinedClassModel selectedCombinedClass, ObservableCollection<ExamWeightModel> exams)
        {
            return Task.Factory.StartNew<AggregateResultModel>(delegate
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
                        "/eh.OutOf)),4) [Average] FROM [ExamDetail] ed INNER JOIN [StudentSubjectSelectionDetail] sssd on (sssd.SubjectID = ed.SubjectID) LEFT OUTER JOIN [ExamHeader] eh ON (eh.ExamID=ed.ExamID) LEFT OUTER JOIN [ExamResultHeader] erh ON (erh.ExamID=eh.ExamID) INNER JOIN [StudentSubjectSelectionHeader] sssh on (sssh.StudentID = erh.StudentID AND sssd.StudentSubjectSelectionID= sssh.StudentSubjectSelectionID) INNER JOIN [ExamResultDetail] erd ON (sssd.SubjectID=erd.SubjectID AND erd.ExamResultID=erh.ExamResultID) LEFT OUTER JOIN [Subject] sub ON(sssd.SubjectID=sub.SubjectID) LEFT OUTER JOIN [StudentClass] sc ON(erh.StudentID=sc.StudentID) WHERE sc.[Year]=DATEPART(year,SYSDATETIME()) AND sssh.[Year]=DATEPART(year,SYSDATETIME()) AND erh.ExamID=",
                        current.ExamID,
                        " GROUP BY sub.SubjectID,sub.NameOfSubject) x"
                    });
                    aggregateResultModel.MeanScore += decimal.Parse(DataAccessHelper.Helper.ExecuteScalar(commandText));
                }
                aggregateResultModel.MeanGrade = Institution.Controller.DataController.CalculateGrade(aggregateResultModel.MeanScore);
                aggregateResultModel.Points = Institution.Controller.DataController.CalculatePoints(aggregateResultModel.MeanGrade);
                aggregateResultModel.Entries = GetCombinedAggregateResultEntries(selectedCombinedClass.Entries, exams);
                return aggregateResultModel;
            });
        }

        private static ObservableCollection<AggregateResultEntryModel> GetCombinedAggregateResultEntries(ClassModel selectedClass, ObservableCollection<ExamWeightModel> exams)
        {
            ObservableCollection<AggregateResultEntryModel> temp = new ObservableCollection<AggregateResultEntryModel>();

            foreach (var e in exams)
            {
                string selectStr = "SELECT sub.SubjectID,sub.NameOfSubject,ROUND(AVG((erd.Score*" + e.Weight + "/eh.OutOf)),4) FROM [ExamDetail] ed INNER JOIN " +
                    "[StudentSubjectSelectionDetail] sssd on (sssd.SubjectID = ed.SubjectID) LEFT OUTER JOIN [ExamHeader] eh " +
                    "ON (eh.ExamID=ed.ExamID) LEFT OUTER JOIN [ExamResultHeader] erh ON (erh.ExamID=eh.ExamID) INNER JOIN " +
                    "[StudentSubjectSelectionHeader] sssh on (sssh.StudentID = erh.StudentID AND sssd.StudentSubjectSelectionID= sssh.StudentSubjectSelectionID)" +
                    " INNER JOIN [ExamResultDetail] erd ON (sssd.SubjectID=erd.SubjectID AND erd.ExamResultID=erh.ExamResultID) LEFT OUTER JOIN " +
                    "[Subject] sub ON(sssd.SubjectID=sub.SubjectID) LEFT OUTER JOIN [StudentClass] sc ON(erh.StudentID=sc.StudentID) " +
                    "WHERE sc.ClassID=" + selectedClass.ClassID + " AND sc.[Year]=DATEPART(year,SYSDATETIME()) AND sssh.[Year]=DATEPART(year,SYSDATETIME()) AND erh.ExamID=" + e.ExamID +
                    " GROUP BY sub.SubjectID,sub.NameOfSubject ORDER BY ROUND(AVG((erd.Score*" + e.Weight + "/eh.OutOf)),4) DESC";

                DataTable dt = DataAccessHelper.Helper.ExecuteNonQueryWithResult(selectStr);
                AggregateResultEntryModel cls;
                foreach (DataRow dtr in dt.Rows)
                {
                    cls = new AggregateResultEntryModel();

                    cls.NameOfSubject = dtr[1].ToString();
                    cls.MeanScore = decimal.Parse(dtr[2].ToString());
                    cls.MeanGrade = Institution.Controller.DataController.CalculateGrade(cls.MeanScore * 100 / e.Weight);
                    cls.Points = Institution.Controller.DataController.CalculatePoints(cls.MeanGrade);
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
            ObservableCollection<AggregateResultEntryModel> temp = new ObservableCollection<AggregateResultEntryModel>();
            foreach (ExamWeightModel current in exams)
            {
                string commandText = string.Concat(new object[]
                {
                    "SELECT sub.SubjectID,sub.NameOfSubject,ROUND(AVG((erd.Score*",
                    current.Weight,
                    "/eh.OutOf)),4) FROM [ExamDetail] ed INNER JOIN [StudentSubjectSelectionDetail] sssd on (sssd.SubjectID = ed.SubjectID) LEFT OUTER JOIN [ExamHeader] eh ON (eh.ExamID=ed.ExamID) LEFT OUTER JOIN [ExamResultHeader] erh ON (erh.ExamID=eh.ExamID) INNER JOIN [StudentSubjectSelectionHeader] sssh on (sssh.StudentID = erh.StudentID AND sssd.StudentSubjectSelectionID= sssh.StudentSubjectSelectionID) INNER JOIN [ExamResultDetail] erd ON (sssd.SubjectID=erd.SubjectID AND erd.ExamResultID=erh.ExamResultID) LEFT OUTER JOIN [Subject] sub ON(sssd.SubjectID=sub.SubjectID) LEFT OUTER JOIN [StudentClass] sc ON(erh.StudentID=sc.StudentID) WHERE sc.[Year]=DATEPART(year,SYSDATETIME()) AND sssh.[Year]=DATEPART(year,SYSDATETIME()) AND erh.ExamID=",
                    current.ExamID,
                    " GROUP BY sub.SubjectID,sub.NameOfSubject ORDER BY ROUND(AVG((erd.Score*",
                    current.Weight,
                    "/eh.OutOf)),4) DESC"
                });
                DataTable dataTable = DataAccessHelper.Helper.ExecuteNonQueryWithResult(commandText);
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    AggregateResultEntryModel aggregateResultEntryModel = new AggregateResultEntryModel();
                    aggregateResultEntryModel.NameOfSubject = dataRow[1].ToString();
                    aggregateResultEntryModel.MeanScore = decimal.Parse(dataRow[2].ToString());
                    aggregateResultEntryModel.MeanGrade = Institution.Controller.DataController.CalculateGrade(decimal.Parse((aggregateResultEntryModel.MeanScore * 100m / current.Weight).ToString("N2")));
                    aggregateResultEntryModel.Points = Institution.Controller.DataController.CalculatePoints(aggregateResultEntryModel.MeanGrade);

                    temp.Add(aggregateResultEntryModel);
                }
            }
            ObservableCollection<AggregateResultEntryModel> observableCollection = new ObservableCollection<AggregateResultEntryModel>();
            int i;
            for (i = 0; i < temp.Count; i++)
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
        
        
        public static Task<ReportFormModel> GetStudentReportFormAsync(int studentID, IEnumerable<ExamWeightModel> exams)
        {
            return Task.Factory.StartNew(() =>
            {
                int classID = Students.Controller.DataController.GetClassIDFromStudentID(studentID).Result;
                var nameOfClass = Institution.Controller.DataController.GetClass(classID).NameOfClass;
                var subjects = Institution.Controller.DataController.GetInstitutionSubjectsAsync().Result;
                                
                int e1 = exams.Any(o => o.Index == 1) ? exams.First(o => o.Index == 1).ExamID : 0;
                int e2 = exams.Any(o => o.Index == 2) ? exams.First(o => o.Index == 2).ExamID : 0;
                int e3 = exams.Any(o => o.Index == 3) ? exams.First(o => o.Index == 3).ExamID : 0;

                decimal e1w = exams.Any(o => o.Index == 1) ? exams.First(o => o.Index == 1).Weight : 0m;
                decimal e2w = exams.Any(o => o.Index == 2) ? exams.First(o => o.Index == 2).Weight : 0m;
                decimal e3w = exams.Any(o => o.Index == 3) ? exams.First(o => o.Index == 3).Weight : 0m;

                string selectStr = "DECLARE @classSCount varchar(50)= '/'+CONVERT(varchar(50),(SELECT COUNT(DISTINCT esd.StudentID) FROM [ExamStudentDetail]esd LEFT OUTER JOIN [StudentClass]sc ON (sc.StudentID=esd.StudentID)" +
                "WHERE sc.ClassID=@cid AND sc.[Year] = DATEPART(year,sysdatetime()) AND esd.ExamID IN (";
                foreach (var t in exams)
                    selectStr += t.ExamID + ",";
                selectStr = selectStr.Remove(selectStr.Length - 1);
                selectStr += ")))\r\n"+

                "DECLARE @streamSCount varchar(50) ='/'+CONVERT(varchar(50),(SELECT COUNT(DISTINCT StudentID) FROM [ExamStudentDetail]esd WHERE ExamID IN (";
                foreach (var t in exams)
                    selectStr += t.ExamID + ",";
                selectStr = selectStr.Remove(selectStr.Length - 1);
                selectStr += ")))"+


                "SELECT * FROM (" +


"SELECT esd.StudentID,s.NameOfStudent, cc.ClassID,sub.NameOfSubject, sssd.SubjectID,sum(ex1.Score) Exam1Score, sum(ex2.Score) Exam2Score, sum(ex3.Score) Exam3Score," +
"CONVERT(varchar(50),subjectRank.SubjectRank)+@streamSCount SubjectRank, CONVERT(varchar(50),streamRank.StreamRank)+@streamSCount StreamRank,CONVERT(varchar(50),classRank.ClassRank)+@classSCount ClassRank " +
"FROM[ExamStudentDetail] esd " +
"LEFT OUTER JOIN[StudentClass] cc ON(esd.StudentID = cc.StudentID AND cc.Year = DATEPART(YEAR, SYSDATETIME()))" +
"LEFT OUTER JOIN[Student] s ON(s.StudentID = esd.StudentID)" +
"LEFT OUTER JOIN[StudentSubjectSelectionHeader] sssh ON(sssh.StudentID = esd.StudentID AND sssh.Year = DATEPART(year, SYSDATETIME()))" +
"LEFT OUTER JOIN[StudentSubjectSelectionDetail] sssd ON(sssh.StudentSubjectSelectionID = sssd.StudentSubjectSelectionID)" +
"LEFT OUTER JOIN[Subject] sub ON(sub.SubjectID = sssd.SubjectID)" +
"LEFT OUTER JOIN" +
"(SELECT StudentID, erh.ExamID, SubjectID, CONVERT(decimal(18, 2), Score * (@e1w / eh.OutOf), 2)Score FROM[ExamResultHeader] erh " +
"LEFT OUTER JOIN[ExamResultDetail] erd ON(erd.ExamResultID = erh.ExamResultID)" +
"LEFT OUTER JOIN[ExamHeader] eh ON(erh.ExamID = eh.ExamID)" +
"WHERE erh.ExamID = @e1) ex1 " +
"ON(ex1.ExamID = esd.ExamID AND ex1.StudentID = esd.StudentID AND ex1.SubjectID = sssd.SubjectID)" +
"LEFT OUTER JOIN" +
"(SELECT StudentID, erh.ExamID, SubjectID, CONVERT(decimal(18, 2), Score * (@e2w / eh.OutOf), 2)Score FROM[ExamResultHeader] erh " +
"LEFT OUTER JOIN[ExamResultDetail] erd ON(erd.ExamResultID = erh.ExamResultID)" +
"LEFT OUTER JOIN[ExamHeader] eh ON(erh.ExamID = eh.ExamID)" +
"WHERE erh.ExamID = @e2) ex2 " +
"ON(ex2.ExamID = esd.ExamID AND ex2.StudentID = esd.StudentID AND ex2.SubjectID = sssd.SubjectID)" +
"LEFT OUTER JOIN" +
"(SELECT StudentID, erh.ExamID, SubjectID, CONVERT(decimal(18, 2), Score * (@e3w / eh.OutOf), 2)Score FROM[ExamResultHeader] erh " +
"LEFT OUTER JOIN[ExamResultDetail] erd ON(erd.ExamResultID = erh.ExamResultID)" +
"LEFT OUTER JOIN[ExamHeader] eh ON(erh.ExamID = eh.ExamID)" +
"WHERE erh.ExamID = @e3) ex3 " +
"ON(ex3.ExamID = esd.ExamID AND ex3.StudentID = esd.StudentID AND ex3.SubjectID = sssd.SubjectID)" +
"LEFT OUTER JOIN" +

//
"(SELECT StudentID, SubjectID, SubjectRank FROM" +
"(SELECT StudentID,";
                foreach (var sub in subjects)

                    selectStr += "DENSE_RANK() OVER(order by ISNULL([" + sub.SubjectID + "], 0)desc)[" + sub.SubjectID + "],";
                selectStr = selectStr.Remove(selectStr.Length - 1) + " ";
                selectStr +=
                "FROM(" +
                "SELECT esd.StudentID, s.SubjectID, sum(ISNULL(ex1.Score,0))+SUM(ISNULL(ex2.Score,0))+SUM(ISNULL(ex3.Score,0)) Exam1Score FROM " +
                "[ExamStudentDetail] esd " +
                "LEFT OUTER JOIN" +
                "(SELECT StudentID, erh.ExamID, SubjectID, CONVERT(decimal(18,2),Score*(@e1w/eh.OutOf),2)Score FROM[ExamResultHeader] erh " +
                "LEFT OUTER JOIN[ExamResultDetail] " +
                "erd ON(erd.ExamResultID = erh.ExamResultID) LEFT OUTER JOIN[ExamHeader] eh ON(erh.ExamID = eh.ExamID)" +
                "WHERE erh.ExamID=@e1) ex1 ON(ex1.ExamID = esd.ExamID AND ex1.StudentID = esd.StudentID) " +
                "LEFT OUTER JOIN" +
                "(SELECT StudentID, erh.ExamID, SubjectID, CONVERT(decimal(18,2),Score*(@e2w/eh.OutOf),2)Score FROM[ExamResultHeader]" +
                        "erh " +
                  "LEFT OUTER JOIN[ExamResultDetail] " +
                        "erd ON(erd.ExamResultID = erh.ExamResultID)" +
                "LEFT OUTER JOIN[ExamHeader]" +
                        "eh ON(erh.ExamID = eh.ExamID)" +
                "WHERE erh.ExamID=@e2) ex2 " +
                "ON(ex2.ExamID = esd.ExamID AND ex2.StudentID = esd.StudentID)" +
                "LEFT OUTER JOIN" +
                "(SELECT StudentID, erh.ExamID, SubjectID, CONVERT(decimal(18,2),Score*(@e3w/eh.OutOf),2)Score FROM[ExamResultHeader]" +
                        "erh " +
                  "LEFT OUTER JOIN[ExamResultDetail]" +
                        "erd ON(erd.ExamResultID = erh.ExamResultID)" +
                "LEFT OUTER JOIN[ExamHeader]" +
                        "eh ON(erh.ExamID = eh.ExamID)" +
                "WHERE erh.ExamID=@e3) ex3 " +
                "ON(ex3.ExamID = esd.ExamID AND ex3.StudentID = esd.StudentID)" +
                "LEFT OUTER JOIN[Subject]" +
                        "s ON(s.SubjectID = ex1.SubjectID OR s.SubjectID = ex2.SubjectID OR s.SubjectID = ex3.SubjectID)" +
                "WHERE esd.ExamID = @e1 OR esd.ExamID = @e2 OR esd.ExamID = @e3 " +
                "GROUP BY esd.StudentID, s.SubjectID" +
                ") as s " +
                "PIVOT" +
                "(" +
                    "SUM(Exam1Score) " +
                    "FOR[SubjectID] IN(";
                foreach (var sub in subjects)
                    selectStr += "[" + sub.SubjectID + "],";
                selectStr = selectStr.Remove(selectStr.Length - 1) + "))AS pvt )s2 " +

    "UNPIVOT(SubjectRank FOR [SubjectID] in (";
                foreach (var sub in subjects)
                    selectStr += "[" + sub.SubjectID + "],";
                selectStr = selectStr.Remove(selectStr.Length - 1) +
    ")) as unpvt" +
") subjectRank ON(esd.StudentID = subjectRank.StudentID AND sssd.SubjectID = subjectRank.SubjectID)" +
//</SubjectRank>
"LEFT OUTER JOIN " +
//<ClassRank>
"(SELECT StudentID,DENSE_RANK() OVER(order by";
                foreach (var sub in subjects)
                    selectStr += " ISNULL([" + sub.SubjectID + "], 0) +";
                selectStr = selectStr.Remove(selectStr.Length - 1) +
    "desc) classRank " +
    "FROM(" +
    "SELECT esd.StudentID, s.SubjectID, sum(ISNULL(ex1.Score, 0)) + SUM(ISNULL(ex2.Score, 0)) + SUM(ISNULL(ex3.Score, 0)) Exam1Score FROM " +
    "[ExamStudentDetail] esd LEFT OUTER JOIN [StudentClass] cs " +
    "ON(cs.StudentID = esd.StudentID AND cs.[Year] = DATEPART(YEAR, SYSDATETIME()))" +
    "LEFT OUTER JOIN" +
    "(SELECT StudentID, erh.ExamID, SubjectID, CONVERT(decimal(18, 2), Score * (@e1w / eh.OutOf), 2)Score FROM[ExamResultHeader] erh " +
    "LEFT OUTER JOIN[ExamResultDetail] erd ON(erd.ExamResultID = erh.ExamResultID)" +
    "LEFT OUTER JOIN[ExamHeader] eh ON(erh.ExamID = eh.ExamID)" +
    "WHERE erh.ExamID = @e1) ex1 " +
    "ON(ex1.ExamID = esd.ExamID AND ex1.StudentID = esd.StudentID)" +
    "LEFT OUTER JOIN" +
    "(SELECT StudentID, erh.ExamID, SubjectID, CONVERT(decimal(18, 2), Score * (@e2w / eh.OutOf), 2)Score FROM[ExamResultHeader] erh " +
    "LEFT OUTER JOIN[ExamResultDetail] erd ON(erd.ExamResultID = erh.ExamResultID)" +
    "LEFT OUTER JOIN[ExamHeader] eh ON(erh.ExamID = eh.ExamID)" +
    "WHERE erh.ExamID = @e2) ex2 " +
    "ON(ex2.ExamID = esd.ExamID AND ex2.StudentID = esd.StudentID)" +
    "LEFT OUTER JOIN" +
    "(SELECT StudentID, erh.ExamID, SubjectID, CONVERT(decimal(18, 2), Score * (@e3w / eh.OutOf), 2)Score FROM[ExamResultHeader] erh " +
    "LEFT OUTER JOIN[ExamResultDetail] erd ON(erd.ExamResultID = erh.ExamResultID)" +
    "LEFT OUTER JOIN[ExamHeader] eh ON(erh.ExamID = eh.ExamID)" +
    "WHERE erh.ExamID = @e3) ex3 " +
    "ON(ex3.ExamID = esd.ExamID AND ex3.StudentID = esd.StudentID)" +
    "LEFT OUTER JOIN[Subject] s ON(s.SubjectID = ex1.SubjectID OR s.SubjectID = ex2.SubjectID OR s.SubjectID = ex3.SubjectID)" +
    "WHERE cs.ClassID = 1 AND(esd.ExamID = @e1 OR esd.ExamID = @e2 OR esd.ExamID = @e3)" +
    "GROUP BY esd.StudentID, s.SubjectID" +
    ") as s " +
    "PIVOT" +
    "(" +
    "   SUM(Exam1Score)" +
    "    FOR[SubjectID] IN(";
                foreach (var sub in subjects)
                    selectStr += "[" + sub.SubjectID + "],";
                selectStr = selectStr.Remove(selectStr.Length - 1) +
               "))AS pvt ) classRank ON (classRank.StudentID = s.StudentID)" +
    //</ClassRank
    "LEFT OUTER JOIN" +
    //<StreamRank>
    "(SELECT StudentID,DENSE_RANK() OVER(order by";
                foreach (var sub in subjects)
                    selectStr += " ISNULL([" + sub.SubjectID + "], 0) +";
                selectStr = selectStr.Remove(selectStr.Length - 1) +
    "desc) StreamRank FROM(" +
"SELECT esd.StudentID, s.SubjectID, sum(ISNULL(ex1.Score, 0)) + SUM(ISNULL(ex2.Score, 0)) + SUM(ISNULL(ex3.Score, 0)) Exam1Score FROM[ExamStudentDetail] esd " +
"LEFT OUTER JOIN" +
"(SELECT StudentID, erh.ExamID, SubjectID, CONVERT(decimal(18, 2), Score * (@e1w / eh.OutOf), 2)Score FROM[ExamResultHeader] erh " +
"LEFT OUTER JOIN[ExamResultDetail] erd ON(erd.ExamResultID = erh.ExamResultID)" +
"LEFT OUTER JOIN[ExamHeader] eh ON(erh.ExamID = eh.ExamID)" +
"WHERE erh.ExamID = @e1) ex1 " +
"ON(ex1.ExamID = esd.ExamID AND ex1.StudentID = esd.StudentID)" +
"LEFT OUTER JOIN" +
"(SELECT StudentID, erh.ExamID, SubjectID, CONVERT(decimal(18, 2), Score * (@e2w / eh.OutOf), 2)Score FROM[ExamResultHeader] erh " +
"LEFT OUTER JOIN[ExamResultDetail] erd ON(erd.ExamResultID = erh.ExamResultID)" +
"LEFT OUTER JOIN[ExamHeader] eh ON(erh.ExamID = eh.ExamID)" +
"WHERE erh.ExamID = @e2) ex2 " +
"ON(ex2.ExamID = esd.ExamID AND ex2.StudentID = esd.StudentID)" +
"LEFT OUTER JOIN" +
"(SELECT StudentID, erh.ExamID, SubjectID, CONVERT(decimal(18, 2), Score * (@e3w / eh.OutOf), 2)Score FROM[ExamResultHeader] erh " +
"LEFT OUTER JOIN[ExamResultDetail] erd ON(erd.ExamResultID = erh.ExamResultID)" +
"LEFT OUTER JOIN[ExamHeader] eh ON(erh.ExamID = eh.ExamID)" +
"WHERE erh.ExamID = @e3) ex3 " +
"ON(ex3.ExamID = esd.ExamID AND ex3.StudentID = esd.StudentID)" +
"LEFT OUTER JOIN[Subject] s ON(s.SubjectID = ex1.SubjectID OR s.SubjectID = ex2.SubjectID OR s.SubjectID = ex3.SubjectID)" +
"WHERE esd.ExamID = @e1 OR esd.ExamID = @e2 OR esd.ExamID = @e3 " +
"GROUP BY esd.StudentID, s.SubjectID" +
") as s " +
"PIVOT" +
"(" +
"    SUM(Exam1Score)" +
"    FOR[SubjectID] IN(";
                foreach (var sub in subjects)
                    selectStr += "[" + sub.SubjectID + "],";
                selectStr = selectStr.Remove(selectStr.Length - 1) + "))AS pvt )streamRank ON (streamRank.StudentID = esd.StudentID)" +
//</StreamRank>
"WHERE cc.ClassID = @cid AND(esd.ExamID = @e1 OR esd.ExamID = @e2 OR esd.ExamID = @e3)" +
"GROUP BY esd.StudentID, sub.NameOfSubject, sssd.SubjectID, s.NameOfStudent, cc.ClassID, streamRank.StreamRank, classRank.ClassRank,subjectRank.SubjectRank" +
")stx WHERE stx.StudentID=@sid ORDER BY StudentID,SubjectID";

                var paramColl = new List<SqlParameter>();
                paramColl.Add(new SqlParameter("@sid", studentID));
                paramColl.Add(new SqlParameter("@e1", e1));
                paramColl.Add(new SqlParameter("@e2", e2));
                paramColl.Add(new SqlParameter("@e3", e3));
                paramColl.Add(new SqlParameter("@e1w", e1w));
                paramColl.Add(new SqlParameter("@e2w", e2w));
                paramColl.Add(new SqlParameter("@e3w", e3w));
                paramColl.Add(new SqlParameter("@cid", classID));

                var dt = DataAccessHelper.Helper.ExecuteNonQueryWithResultTable(selectStr, paramColl);

                ReportFormModel rpm = new ReportFormModel();
                rpm.NameOfClass = nameOfClass;
                ReportFormSubjectModel l;
                if (dt.Rows.Count>0)
                {
                    rpm.StreamRank = dt.Rows[0][10].ToString();
                    rpm.ClassRank = dt.Rows[0][9].ToString();
                }
                foreach (DataRow dtr in dt.Rows)
                {
                    l = new ReportFormSubjectModel();
                   
                    l.SubjectID = int.Parse(dtr[4].ToString());
                    l.NameOfSubject = dtr[3].ToString();
                    l.Code = SubjectModel.AllSubjects.First(o => o.NameOfSubject == l.NameOfSubject).Code;
                    l.Exam1Score = dtr[5].ToString();
                    l.Exam2Score = dtr[6].ToString();
                    l.Exam3Score = dtr[7].ToString();
                    l.StreamRank = dtr[8].ToString();                    
                    l.MeanScore = (string.IsNullOrWhiteSpace(l.Exam1Score) ? 0 : decimal.Parse(l.Exam1Score)) +
                    (string.IsNullOrWhiteSpace(l.Exam2Score) ? 0 : decimal.Parse(l.Exam2Score)) +
                    (string.IsNullOrWhiteSpace(l.Exam3Score) ? 0 : decimal.Parse(l.Exam3Score));
                    l.Grade = Institution.Controller.DataController.CalculateGrade(l.MeanScore);
                    l.Remarks = Institution.Controller.DataController.GetRemark(l.MeanScore,(l.Code==102));

                    rpm.TotalMarks += l.MeanScore;
                    rpm.TotalPoints += Institution.Controller.DataController.CalculatePoints(l.Grade);
                    rpm.SubjectEntries.Add(l);
                }
                rpm.MeanScore = rpm.SubjectEntries.Count == 0 ? 0 : rpm.TotalMarks / rpm.SubjectEntries.Count;
                rpm.AvgPoints = rpm.SubjectEntries.Count == 0 ? 1 : rpm.TotalPoints / rpm.SubjectEntries.Count;
                rpm.MeanGrade = Institution.Controller.DataController.CalculateGrade(rpm.MeanScore);
                return rpm;
            });
        }

        public static Task<ReportForm3Model> GetStudentReportForm3Async(int studentID, IEnumerable<ExamWeightModel> exams)
        {
            return Task.Factory.StartNew(() =>
            {
                int classID = Students.Controller.DataController.GetClassIDFromStudentID(studentID).Result;
                var nameOfClass = Institution.Controller.DataController.GetClass(classID).NameOfClass;
                var subjects = Institution.Controller.DataController.GetInstitutionSubjectsAsync().Result;

                int e1 = exams.Any(o => o.Index == 1) ? exams.First(o => o.Index == 1).ExamID : 0;
                int e2 = exams.Any(o => o.Index == 2) ? exams.First(o => o.Index == 2).ExamID : 0;
                int e3 = exams.Any(o => o.Index == 3) ? exams.First(o => o.Index == 3).ExamID : 0;

                decimal e1w = exams.Any(o => o.Index == 1) ? exams.First(o => o.Index == 1).Weight : 0m;
                decimal e2w = exams.Any(o => o.Index == 2) ? exams.First(o => o.Index == 2).Weight : 0m;
                decimal e3w = exams.Any(o => o.Index == 3) ? exams.First(o => o.Index == 3).Weight : 0m;

                string selectStr = "DECLARE @classSCount varchar(50)= '/'+CONVERT(varchar(50),(SELECT COUNT(DISTINCT esd.StudentID) FROM [ExamStudentDetail]esd LEFT OUTER JOIN [StudentClass]sc ON (sc.StudentID=esd.StudentID)" +
                "WHERE sc.ClassID=@cid AND sc.[Year] = DATEPART(year,sysdatetime()) AND esd.ExamID IN (";
                foreach (var t in exams)
                    selectStr += t.ExamID + ",";
                selectStr = selectStr.Remove(selectStr.Length - 1);
                selectStr += ")))\r\n" +

                "DECLARE @streamSCount varchar(50) ='/'+CONVERT(varchar(50),(SELECT COUNT(DISTINCT StudentID) FROM [ExamStudentDetail]esd WHERE ExamID IN (";
                foreach (var t in exams)
                    selectStr += t.ExamID + ",";
                selectStr = selectStr.Remove(selectStr.Length - 1);
                selectStr += ")))" +


                "SELECT * FROM (" +


"SELECT esd.StudentID,s.NameOfStudent, cc.ClassID,sub.NameOfSubject, sssd.SubjectID,sum(ex1.Score) Exam1Score, sum(ex2.Score) Exam2Score, sum(ex3.Score) Exam3Score," +
"CONVERT(varchar(50),subjectRank.SubjectRank)+@streamSCount SubjectRank, CONVERT(varchar(50),streamRank.StreamRank)+@streamSCount StreamRank,CONVERT(varchar(50),classRank.ClassRank)+@classSCount ClassRank " +
", s.KCPEScore FROM[ExamStudentDetail] esd " +
"LEFT OUTER JOIN[StudentClass] cc ON(esd.StudentID = cc.StudentID AND cc.Year = DATEPART(YEAR, SYSDATETIME()))" +
"LEFT OUTER JOIN[Student] s ON(s.StudentID = esd.StudentID)" +
"LEFT OUTER JOIN[StudentSubjectSelectionHeader] sssh ON(sssh.StudentID = esd.StudentID AND sssh.Year = DATEPART(year, SYSDATETIME()))" +
"LEFT OUTER JOIN[StudentSubjectSelectionDetail] sssd ON(sssh.StudentSubjectSelectionID = sssd.StudentSubjectSelectionID)" +
"LEFT OUTER JOIN[Subject] sub ON(sub.SubjectID = sssd.SubjectID)" +
"LEFT OUTER JOIN" +
"(SELECT StudentID, erh.ExamID, SubjectID, CONVERT(decimal(18, 2), Score * (@e1w / eh.OutOf), 2)Score FROM[ExamResultHeader] erh " +
"LEFT OUTER JOIN[ExamResultDetail] erd ON(erd.ExamResultID = erh.ExamResultID)" +
"LEFT OUTER JOIN[ExamHeader] eh ON(erh.ExamID = eh.ExamID)" +
"WHERE erh.ExamID = @e1) ex1 " +
"ON(ex1.ExamID = esd.ExamID AND ex1.StudentID = esd.StudentID AND ex1.SubjectID = sssd.SubjectID)" +
"LEFT OUTER JOIN" +
"(SELECT StudentID, erh.ExamID, SubjectID, CONVERT(decimal(18, 2), Score * (@e2w / eh.OutOf), 2)Score FROM[ExamResultHeader] erh " +
"LEFT OUTER JOIN[ExamResultDetail] erd ON(erd.ExamResultID = erh.ExamResultID)" +
"LEFT OUTER JOIN[ExamHeader] eh ON(erh.ExamID = eh.ExamID)" +
"WHERE erh.ExamID = @e2) ex2 " +
"ON(ex2.ExamID = esd.ExamID AND ex2.StudentID = esd.StudentID AND ex2.SubjectID = sssd.SubjectID)" +
"LEFT OUTER JOIN" +
"(SELECT StudentID, erh.ExamID, SubjectID, CONVERT(decimal(18, 2), Score * (@e3w / eh.OutOf), 2)Score FROM[ExamResultHeader] erh " +
"LEFT OUTER JOIN[ExamResultDetail] erd ON(erd.ExamResultID = erh.ExamResultID)" +
"LEFT OUTER JOIN[ExamHeader] eh ON(erh.ExamID = eh.ExamID)" +
"WHERE erh.ExamID = @e3) ex3 " +
"ON(ex3.ExamID = esd.ExamID AND ex3.StudentID = esd.StudentID AND ex3.SubjectID = sssd.SubjectID)" +
"LEFT OUTER JOIN" +

//
"(SELECT StudentID, SubjectID, SubjectRank FROM" +
"(SELECT StudentID,";
                foreach (var sub in subjects)

                    selectStr += "DENSE_RANK() OVER(order by ISNULL([" + sub.SubjectID + "], 0)desc)[" + sub.SubjectID + "],";
                selectStr = selectStr.Remove(selectStr.Length - 1) + " ";
                selectStr +=
                "FROM(" +
                "SELECT esd.StudentID, s.SubjectID, sum(ISNULL(ex1.Score,0))+SUM(ISNULL(ex2.Score,0))+SUM(ISNULL(ex3.Score,0)) Exam1Score FROM " +
                "[ExamStudentDetail] esd " +
                "LEFT OUTER JOIN" +
                "(SELECT StudentID, erh.ExamID, SubjectID, CONVERT(decimal(18,2),Score*(@e1w/eh.OutOf),2)Score FROM[ExamResultHeader] erh " +
                "LEFT OUTER JOIN[ExamResultDetail] " +
                "erd ON(erd.ExamResultID = erh.ExamResultID) LEFT OUTER JOIN[ExamHeader] eh ON(erh.ExamID = eh.ExamID)" +
                "WHERE erh.ExamID=@e1) ex1 ON(ex1.ExamID = esd.ExamID AND ex1.StudentID = esd.StudentID) " +
                "LEFT OUTER JOIN" +
                "(SELECT StudentID, erh.ExamID, SubjectID, CONVERT(decimal(18,2),Score*(@e2w/eh.OutOf),2)Score FROM[ExamResultHeader]" +
                        "erh " +
                  "LEFT OUTER JOIN[ExamResultDetail] " +
                        "erd ON(erd.ExamResultID = erh.ExamResultID)" +
                "LEFT OUTER JOIN[ExamHeader]" +
                        "eh ON(erh.ExamID = eh.ExamID)" +
                "WHERE erh.ExamID=@e2) ex2 " +
                "ON(ex2.ExamID = esd.ExamID AND ex2.StudentID = esd.StudentID)" +
                "LEFT OUTER JOIN" +
                "(SELECT StudentID, erh.ExamID, SubjectID, CONVERT(decimal(18,2),Score*(@e3w/eh.OutOf),2)Score FROM[ExamResultHeader]" +
                        "erh " +
                  "LEFT OUTER JOIN[ExamResultDetail]" +
                        "erd ON(erd.ExamResultID = erh.ExamResultID)" +
                "LEFT OUTER JOIN[ExamHeader]" +
                        "eh ON(erh.ExamID = eh.ExamID)" +
                "WHERE erh.ExamID=@e3) ex3 " +
                "ON(ex3.ExamID = esd.ExamID AND ex3.StudentID = esd.StudentID)" +
                "LEFT OUTER JOIN[Subject]" +
                        "s ON(s.SubjectID = ex1.SubjectID OR s.SubjectID = ex2.SubjectID OR s.SubjectID = ex3.SubjectID)" +
                "WHERE esd.ExamID = @e1 OR esd.ExamID = @e2 OR esd.ExamID = @e3 " +
                "GROUP BY esd.StudentID, s.SubjectID" +
                ") as s " +
                "PIVOT" +
                "(" +
                    "SUM(Exam1Score) " +
                    "FOR[SubjectID] IN(";
                foreach (var sub in subjects)
                    selectStr += "[" + sub.SubjectID + "],";
                selectStr = selectStr.Remove(selectStr.Length - 1) + "))AS pvt )s2 " +

    "UNPIVOT(SubjectRank FOR [SubjectID] in (";
                foreach (var sub in subjects)
                    selectStr += "[" + sub.SubjectID + "],";
                selectStr = selectStr.Remove(selectStr.Length - 1) +
    ")) as unpvt" +
") subjectRank ON(esd.StudentID = subjectRank.StudentID AND sssd.SubjectID = subjectRank.SubjectID)" +
//</SubjectRank>
"LEFT OUTER JOIN " +
//<ClassRank>
"(SELECT StudentID,DENSE_RANK() OVER(order by";
                foreach (var sub in subjects)
                    selectStr += " ISNULL([" + sub.SubjectID + "], 0) +";
                selectStr = selectStr.Remove(selectStr.Length - 1) +
    "desc) classRank " +
    "FROM(" +
    "SELECT esd.StudentID, s.SubjectID, sum(ISNULL(ex1.Score, 0)) + SUM(ISNULL(ex2.Score, 0)) + SUM(ISNULL(ex3.Score, 0)) Exam1Score FROM " +
    "[ExamStudentDetail] esd LEFT OUTER JOIN [StudentClass] cs " +
    "ON(cs.StudentID = esd.StudentID AND cs.[Year] = DATEPART(YEAR, SYSDATETIME()))" +
    "LEFT OUTER JOIN" +
    "(SELECT StudentID, erh.ExamID, SubjectID, CONVERT(decimal(18, 2), Score * (@e1w / eh.OutOf), 2)Score FROM[ExamResultHeader] erh " +
    "LEFT OUTER JOIN[ExamResultDetail] erd ON(erd.ExamResultID = erh.ExamResultID)" +
    "LEFT OUTER JOIN[ExamHeader] eh ON(erh.ExamID = eh.ExamID)" +
    "WHERE erh.ExamID = @e1) ex1 " +
    "ON(ex1.ExamID = esd.ExamID AND ex1.StudentID = esd.StudentID)" +
    "LEFT OUTER JOIN" +
    "(SELECT StudentID, erh.ExamID, SubjectID, CONVERT(decimal(18, 2), Score * (@e2w / eh.OutOf), 2)Score FROM[ExamResultHeader] erh " +
    "LEFT OUTER JOIN[ExamResultDetail] erd ON(erd.ExamResultID = erh.ExamResultID)" +
    "LEFT OUTER JOIN[ExamHeader] eh ON(erh.ExamID = eh.ExamID)" +
    "WHERE erh.ExamID = @e2) ex2 " +
    "ON(ex2.ExamID = esd.ExamID AND ex2.StudentID = esd.StudentID)" +
    "LEFT OUTER JOIN" +
    "(SELECT StudentID, erh.ExamID, SubjectID, CONVERT(decimal(18, 2), Score * (@e3w / eh.OutOf), 2)Score FROM[ExamResultHeader] erh " +
    "LEFT OUTER JOIN[ExamResultDetail] erd ON(erd.ExamResultID = erh.ExamResultID)" +
    "LEFT OUTER JOIN[ExamHeader] eh ON(erh.ExamID = eh.ExamID)" +
    "WHERE erh.ExamID = @e3) ex3 " +
    "ON(ex3.ExamID = esd.ExamID AND ex3.StudentID = esd.StudentID)" +
    "LEFT OUTER JOIN[Subject] s ON(s.SubjectID = ex1.SubjectID OR s.SubjectID = ex2.SubjectID OR s.SubjectID = ex3.SubjectID)" +
    "WHERE cs.ClassID = 1 AND(esd.ExamID = @e1 OR esd.ExamID = @e2 OR esd.ExamID = @e3)" +
    "GROUP BY esd.StudentID, s.SubjectID" +
    ") as s " +
    "PIVOT" +
    "(" +
    "   SUM(Exam1Score)" +
    "    FOR[SubjectID] IN(";
                foreach (var sub in subjects)
                    selectStr += "[" + sub.SubjectID + "],";
                selectStr = selectStr.Remove(selectStr.Length - 1) +
               "))AS pvt ) classRank ON (classRank.StudentID = s.StudentID)" +
    //</ClassRank
    "LEFT OUTER JOIN" +
    //<StreamRank>
    "(SELECT StudentID,DENSE_RANK() OVER(order by";
                foreach (var sub in subjects)
                    selectStr += " ISNULL([" + sub.SubjectID + "], 0) +";
                selectStr = selectStr.Remove(selectStr.Length - 1) +
    "desc) StreamRank FROM(" +
"SELECT esd.StudentID, s.SubjectID, sum(ISNULL(ex1.Score, 0)) + SUM(ISNULL(ex2.Score, 0)) + SUM(ISNULL(ex3.Score, 0)) Exam1Score FROM[ExamStudentDetail] esd " +
"LEFT OUTER JOIN" +
"(SELECT StudentID, erh.ExamID, SubjectID, CONVERT(decimal(18, 2), Score * (@e1w / eh.OutOf), 2)Score FROM[ExamResultHeader] erh " +
"LEFT OUTER JOIN[ExamResultDetail] erd ON(erd.ExamResultID = erh.ExamResultID)" +
"LEFT OUTER JOIN[ExamHeader] eh ON(erh.ExamID = eh.ExamID)" +
"WHERE erh.ExamID = @e1) ex1 " +
"ON(ex1.ExamID = esd.ExamID AND ex1.StudentID = esd.StudentID)" +
"LEFT OUTER JOIN" +
"(SELECT StudentID, erh.ExamID, SubjectID, CONVERT(decimal(18, 2), Score * (@e2w / eh.OutOf), 2)Score FROM[ExamResultHeader] erh " +
"LEFT OUTER JOIN[ExamResultDetail] erd ON(erd.ExamResultID = erh.ExamResultID)" +
"LEFT OUTER JOIN[ExamHeader] eh ON(erh.ExamID = eh.ExamID)" +
"WHERE erh.ExamID = @e2) ex2 " +
"ON(ex2.ExamID = esd.ExamID AND ex2.StudentID = esd.StudentID)" +
"LEFT OUTER JOIN" +
"(SELECT StudentID, erh.ExamID, SubjectID, CONVERT(decimal(18, 2), Score * (@e3w / eh.OutOf), 2)Score FROM[ExamResultHeader] erh " +
"LEFT OUTER JOIN[ExamResultDetail] erd ON(erd.ExamResultID = erh.ExamResultID)" +
"LEFT OUTER JOIN[ExamHeader] eh ON(erh.ExamID = eh.ExamID)" +
"WHERE erh.ExamID = @e3) ex3 " +
"ON(ex3.ExamID = esd.ExamID AND ex3.StudentID = esd.StudentID)" +
"LEFT OUTER JOIN[Subject] s ON(s.SubjectID = ex1.SubjectID OR s.SubjectID = ex2.SubjectID OR s.SubjectID = ex3.SubjectID)" +
"WHERE esd.ExamID = @e1 OR esd.ExamID = @e2 OR esd.ExamID = @e3 " +
"GROUP BY esd.StudentID, s.SubjectID" +
") as s " +
"PIVOT" +
"(" +
"    SUM(Exam1Score)" +
"    FOR[SubjectID] IN(";
                foreach (var sub in subjects)
                    selectStr += "[" + sub.SubjectID + "],";
                selectStr = selectStr.Remove(selectStr.Length - 1) + "))AS pvt )streamRank ON (streamRank.StudentID = esd.StudentID)" +
//</StreamRank>
"WHERE cc.ClassID = @cid AND(esd.ExamID = @e1 OR esd.ExamID = @e2 OR esd.ExamID = @e3)" +
"GROUP BY esd.StudentID, sub.NameOfSubject, sssd.SubjectID, s.NameOfStudent, cc.ClassID, streamRank.StreamRank, classRank.ClassRank,subjectRank.SubjectRank,s.KCPEScore" +
")stx WHERE stx.StudentID=@sid ORDER BY StudentID,SubjectID";

                var paramColl = new List<SqlParameter>();
                paramColl.Add(new SqlParameter("@sid", studentID));
                paramColl.Add(new SqlParameter("@e1", e1));
                paramColl.Add(new SqlParameter("@e2", e2));
                paramColl.Add(new SqlParameter("@e3", e3));
                paramColl.Add(new SqlParameter("@e1w", e1w));
                paramColl.Add(new SqlParameter("@e2w", e2w));
                paramColl.Add(new SqlParameter("@e3w", e3w));
                paramColl.Add(new SqlParameter("@cid", classID));

                var dt = DataAccessHelper.Helper.ExecuteNonQueryWithResultTable(selectStr, paramColl);

                ReportForm3Model rpm = new ReportForm3Model();
                rpm.NameOfClass = nameOfClass;
                ReportFormSubjectModel l;
                if (dt.Rows.Count > 0)
                {
                    rpm.StreamRank = dt.Rows[0][10].ToString();
                    rpm.ClassRank = dt.Rows[0][9].ToString();
                    decimal yu;
                    rpm.KCPEGrade = !decimal.TryParse(dt.Rows[0][11].ToString(), out yu) ? "E" : Institution.Controller.DataController.CalculateGrade(yu*100 / 500);
                    rpm.KCPEScore = yu;
                }
                foreach (DataRow dtr in dt.Rows)
                {
                    l = new ReportFormSubjectModel();

                    l.SubjectID = int.Parse(dtr[4].ToString());
                    l.NameOfSubject = dtr[3].ToString();
                    l.Code = SubjectModel.AllSubjects.First(o => o.NameOfSubject == l.NameOfSubject).Code;
                    l.Exam1Score = dtr[5].ToString();
                    l.Exam2Score = dtr[6].ToString();
                    l.Exam3Score = dtr[7].ToString();
                    l.StreamRank = dtr[8].ToString();
                    l.MeanScore = (string.IsNullOrWhiteSpace(l.Exam1Score) ? 0 : decimal.Parse(l.Exam1Score)) +
                    (string.IsNullOrWhiteSpace(l.Exam2Score) ? 0 : decimal.Parse(l.Exam2Score)) +
                    (string.IsNullOrWhiteSpace(l.Exam3Score) ? 0 : decimal.Parse(l.Exam3Score));
                    l.Grade = Institution.Controller.DataController.CalculateGrade(l.MeanScore);
                    l.Remarks = Institution.Controller.DataController.GetRemark(l.MeanScore, (l.Code == 102));
                    rpm.TotalMarks += l.MeanScore;
                    rpm.TotalPoints += Institution.Controller.DataController.CalculatePoints(l.Grade);
                    rpm.SubjectEntries.Add(l);
                }
                rpm.MeanScore = rpm.SubjectEntries.Count == 0 ? 0 : rpm.TotalMarks / rpm.SubjectEntries.Count;
                if (App.AppExamSettings.MeanGradeCalculation == 1)
                    rpm.MeanGrade = Institution.Controller.DataController.CalculateGrade(rpm.MeanScore);
                else
                    rpm.MeanGrade = Institution.Controller.DataController.CalculateGradeFromPoints(rpm.TotalPoints == 0 ? 1 : rpm.TotalPoints / rpm.SubjectEntries.Count);
                       
                var v = GetReportFormGrades(rpm.SubjectEntries, new List<ExamWeightModel>(exams));
                rpm.CAT1Grade = v[0];
                rpm.CAT2Grade = v[1];
                rpm.ExamGrade = v[2];
                return rpm;
            });
        }

        private static string[] GetReportFormGrades(ObservableCollection<ReportFormSubjectModel> subjects, List<ExamWeightModel> weights)
        {
            string[] grades = new string[3] { "E", "E", "E" };
            decimal ex1TotalPts = 0, ex2TotalPts=0, ex3TotalPts = 0;
            decimal ex1TotalScore = 0, ex2TotalScore = 0, ex3TotalScore = 0;
            foreach (var y in subjects)
            {
                var ex1 = string.IsNullOrWhiteSpace(y.Exam1Score) ? 0m : decimal.Parse(y.Exam1Score) * (100 / weights.First(o => o.Index == 1).Weight);
                ex1TotalPts += Institution.Controller.DataController.CalculatePoints(ex1);
                ex1TotalScore += ex1;
                var ex2 = string.IsNullOrWhiteSpace(y.Exam2Score) ? 0m : decimal.Parse(y.Exam2Score) * (100 / weights.First(o => o.Index == 2).Weight);
                ex2TotalPts += Institution.Controller.DataController.CalculatePoints(ex2);
                ex2TotalScore += ex2;
                var ex3 = string.IsNullOrWhiteSpace(y.Exam3Score) ? 0m : decimal.Parse(y.Exam3Score) * (100 / weights.First(o => o.Index == 3).Weight);
                ex3TotalPts += Institution.Controller.DataController.CalculatePoints(ex3);
                ex3TotalScore += ex3;
            }
            if (App.AppExamSettings.MeanGradeCalculation==0)
            {
                if (subjects.Count == 0)
                    return grades;
                grades[0]=Institution.Controller.DataController.CalculateGradeFromPoints(ex1TotalPts / subjects.Count);
                grades[1] = Institution.Controller.DataController.CalculateGradeFromPoints(ex2TotalPts / subjects.Count);
                grades[2] = Institution.Controller.DataController.CalculateGradeFromPoints(ex3TotalPts / subjects.Count);
            }
            else
            {
                if (subjects.Count == 0)
                    return grades;
                grades[0] = Institution.Controller.DataController.CalculateGrade(ex1TotalScore / subjects.Count);
                grades[1] = Institution.Controller.DataController.CalculateGrade(ex1TotalScore / subjects.Count);
                grades[2] = Institution.Controller.DataController.CalculateGrade(ex1TotalScore / subjects.Count);
            }            
            return grades;
        }

        public static Task<ClassReportFormModel> GetClassReportFormsAsync(int classID, IEnumerable<ExamWeightModel> exams, IProgress<OperationProgress> progress)
        {
            return Task.Factory.StartNew(() =>
            {
                var nameOfClass = Institution.Controller.DataController.GetClass(classID).NameOfClass;
                var subjects = Institution.Controller.DataController.GetInstitutionSubjectsAsync().Result;

                int e1 = exams.Any(o => o.Index == 1) ? exams.First(o => o.Index == 1).ExamID : 0;
                int e2 = exams.Any(o => o.Index == 2) ? exams.First(o => o.Index == 2).ExamID : 0;
                int e3 = exams.Any(o => o.Index == 3) ? exams.First(o => o.Index == 3).ExamID : 0;

                decimal e1w = exams.Any(o => o.Index == 1) ? exams.First(o => o.Index == 1).Weight : 0m;
                decimal e2w = exams.Any(o => o.Index == 2) ? exams.First(o => o.Index == 2).Weight : 0m;
                decimal e3w = exams.Any(o => o.Index == 3) ? exams.First(o => o.Index == 3).Weight : 0m;

                string selectStr =
                "DECLARE @classSCount varchar(50)= '/'+CONVERT(varchar(50),(SELECT COUNT(DISTINCT esd.StudentID) FROM [ExamStudentDetail]esd LEFT OUTER JOIN [StudentClass]sc ON (sc.StudentID=esd.StudentID)" +
                "WHERE sc.ClassID=@cid AND sc.[Year] = DATEPART(year,sysdatetime()) AND esd.ExamID IN (";
                foreach (var t in exams)
                    selectStr += t.ExamID + ",";
                selectStr = selectStr.Remove(selectStr.Length - 1);
                selectStr += ")))\r\n" +

                "DECLARE @streamSCount varchar(50)='/'+CONVERT(varchar(50),(SELECT COUNT(DISTINCT StudentID) FROM [ExamStudentDetail]esd WHERE ExamID IN (";
                foreach (var t in exams)
                    selectStr += t.ExamID + ",";
                selectStr = selectStr.Remove(selectStr.Length - 1);
                selectStr += ")))" +


                "SELECT * FROM (" +


"SELECT esd.StudentID,s.NameOfStudent, cc.ClassID,sub.NameOfSubject, sssd.SubjectID,sum(ex1.Score) Exam1Score, sum(ex2.Score) Exam2Score, sum(ex3.Score) Exam3Score," +
"CONVERT(varchar(50),subjectRank.SubjectRank)+@streamSCount SubjectRank, CONVERT(varchar(50),streamRank.StreamRank)+@streamSCount StreamRank,CONVERT(varchar(50),classRank.ClassRank)+@classSCount ClassRank " +
"FROM[ExamStudentDetail] esd " +
"LEFT OUTER JOIN[StudentClass] cc ON(esd.StudentID = cc.StudentID AND cc.Year = DATEPART(YEAR, SYSDATETIME()))" +
"LEFT OUTER JOIN[Student] s ON(s.StudentID = esd.StudentID)" +
"LEFT OUTER JOIN[StudentSubjectSelectionHeader] sssh ON(sssh.StudentID = esd.StudentID AND sssh.Year = DATEPART(year, SYSDATETIME()))" +
"LEFT OUTER JOIN[StudentSubjectSelectionDetail] sssd ON(sssh.StudentSubjectSelectionID = sssd.StudentSubjectSelectionID)" +
"LEFT OUTER JOIN[Subject] sub ON(sub.SubjectID = sssd.SubjectID)" +
"LEFT OUTER JOIN" +
"(SELECT StudentID, erh.ExamID, SubjectID, CONVERT(decimal(18, 2), Score * (@e1w / eh.OutOf), 2)Score FROM[ExamResultHeader] erh " +
"LEFT OUTER JOIN[ExamResultDetail] erd ON(erd.ExamResultID = erh.ExamResultID)" +
"LEFT OUTER JOIN[ExamHeader] eh ON(erh.ExamID = eh.ExamID)" +
"WHERE erh.ExamID = @e1) ex1 " +
"ON(ex1.ExamID = esd.ExamID AND ex1.StudentID = esd.StudentID AND ex1.SubjectID = sssd.SubjectID)" +
"LEFT OUTER JOIN" +
"(SELECT StudentID, erh.ExamID, SubjectID, CONVERT(decimal(18, 2), Score * (@e2w / eh.OutOf), 2)Score FROM[ExamResultHeader] erh " +
"LEFT OUTER JOIN[ExamResultDetail] erd ON(erd.ExamResultID = erh.ExamResultID)" +
"LEFT OUTER JOIN[ExamHeader] eh ON(erh.ExamID = eh.ExamID)" +
"WHERE erh.ExamID = @e2) ex2 " +
"ON(ex2.ExamID = esd.ExamID AND ex2.StudentID = esd.StudentID AND ex2.SubjectID = sssd.SubjectID)" +
"LEFT OUTER JOIN" +
"(SELECT StudentID, erh.ExamID, SubjectID, CONVERT(decimal(18, 2), Score * (@e3w / eh.OutOf), 2)Score FROM[ExamResultHeader] erh " +
"LEFT OUTER JOIN[ExamResultDetail] erd ON(erd.ExamResultID = erh.ExamResultID)" +
"LEFT OUTER JOIN[ExamHeader] eh ON(erh.ExamID = eh.ExamID)" +
"WHERE erh.ExamID = @e3) ex3 " +
"ON(ex3.ExamID = esd.ExamID AND ex3.StudentID = esd.StudentID AND ex3.SubjectID = sssd.SubjectID)" +
"LEFT OUTER JOIN" +

//SUBJECT RANK
"(SELECT StudentID, SubjectID, SubjectRank FROM" +
"(SELECT StudentID,";
                foreach (var sub in subjects)

                    selectStr += "DENSE_RANK() OVER(order by ISNULL([" + sub.SubjectID + "], 0)desc)[" + sub.SubjectID + "],";
                selectStr = selectStr.Remove(selectStr.Length - 1) + " ";
                selectStr +=
                "FROM(" +
                "SELECT esd.StudentID, s.SubjectID, sum(ISNULL(ex1.Score,0))+SUM(ISNULL(ex2.Score,0))+SUM(ISNULL(ex3.Score,0)) Exam1Score FROM " +
                "[ExamStudentDetail] esd " +
                "LEFT OUTER JOIN" +
                "(SELECT StudentID, erh.ExamID, SubjectID, CONVERT(decimal(18,2),Score*(@e1w/eh.OutOf),2)Score FROM[ExamResultHeader] erh " +
                "LEFT OUTER JOIN[ExamResultDetail] " +
                "erd ON(erd.ExamResultID = erh.ExamResultID) LEFT OUTER JOIN[ExamHeader] eh ON(erh.ExamID = eh.ExamID)" +
                "WHERE erh.ExamID=@e1) ex1 ON(ex1.ExamID = esd.ExamID AND ex1.StudentID = esd.StudentID) " +
                "LEFT OUTER JOIN" +
                "(SELECT StudentID, erh.ExamID, SubjectID, CONVERT(decimal(18,2),Score*(@e2w/eh.OutOf),2)Score FROM[ExamResultHeader]" +
                        "erh " +
                  "LEFT OUTER JOIN[ExamResultDetail] " +
                        "erd ON(erd.ExamResultID = erh.ExamResultID)" +
                "LEFT OUTER JOIN[ExamHeader]" +
                        "eh ON(erh.ExamID = eh.ExamID)" +
                "WHERE erh.ExamID=@e2) ex2 " +
                "ON(ex2.ExamID = esd.ExamID AND ex2.StudentID = esd.StudentID)" +
                "LEFT OUTER JOIN" +
                "(SELECT StudentID, erh.ExamID, SubjectID, CONVERT(decimal(18,2),Score*(@e3w/eh.OutOf),2)Score FROM[ExamResultHeader]" +
                        "erh " +
                  "LEFT OUTER JOIN[ExamResultDetail]" +
                        "erd ON(erd.ExamResultID = erh.ExamResultID)" +
                "LEFT OUTER JOIN[ExamHeader]" +
                        "eh ON(erh.ExamID = eh.ExamID)" +
                "WHERE erh.ExamID=@e3) ex3 " +
                "ON(ex3.ExamID = esd.ExamID AND ex3.StudentID = esd.StudentID)" +
                "LEFT OUTER JOIN[Subject]" +
                        "s ON(s.SubjectID = ex1.SubjectID OR s.SubjectID = ex2.SubjectID OR s.SubjectID = ex3.SubjectID)" +
                "WHERE esd.ExamID = @e1 OR esd.ExamID = @e2 OR esd.ExamID = @e3 " +
                "GROUP BY esd.StudentID, s.SubjectID" +
                ") as s " +
                "PIVOT" +
                "(" +
                    "SUM(Exam1Score) " +
                    "FOR[SubjectID] IN(";
                foreach (var sub in subjects)
                    selectStr += "[" + sub.SubjectID + "],";
                selectStr = selectStr.Remove(selectStr.Length - 1) + "))AS pvt )s2 " +

    "UNPIVOT(SubjectRank FOR [SubjectID] in (";
                foreach (var sub in subjects)
                    selectStr += "[" + sub.SubjectID + "],";
                selectStr = selectStr.Remove(selectStr.Length - 1) +
    ")) as unpvt" +
") subjectRank ON(esd.StudentID = subjectRank.StudentID AND sssd.SubjectID = subjectRank.SubjectID)" +
//</SubjectRank>
"LEFT OUTER JOIN " +
//<ClassRank>
"(SELECT StudentID,DENSE_RANK() OVER(order by";
                foreach (var sub in subjects)
                    selectStr += " ISNULL([" + sub.SubjectID + "], 0) +";
                selectStr = selectStr.Remove(selectStr.Length - 1) +
    "desc) classRank " +
    "FROM(" +
    "SELECT esd.StudentID, s.SubjectID, sum(ISNULL(ex1.Score, 0)) + SUM(ISNULL(ex2.Score, 0)) + SUM(ISNULL(ex3.Score, 0)) Exam1Score FROM " +
    "[StudentClass] cs " +
    "LEFT OUTER JOIN[ExamStudentDetail] esd " +
    "ON(cs.StudentID = esd.StudentID AND cs.[Year] = DATEPART(YEAR, SYSDATETIME()))" +
    "LEFT OUTER JOIN" +
    "(SELECT StudentID, erh.ExamID, SubjectID, CONVERT(decimal(18, 2), Score * (@e1w / eh.OutOf), 2)Score FROM[ExamResultHeader] erh " +
    "LEFT OUTER JOIN[ExamResultDetail] erd ON(erd.ExamResultID = erh.ExamResultID)" +
    "LEFT OUTER JOIN[ExamHeader] eh ON(erh.ExamID = eh.ExamID)" +
    "WHERE erh.ExamID = @e1) ex1 " +
    "ON(ex1.ExamID = esd.ExamID AND ex1.StudentID = esd.StudentID)" +
    "LEFT OUTER JOIN" +
    "(SELECT StudentID, erh.ExamID, SubjectID, CONVERT(decimal(18, 2), Score * (@e2w / eh.OutOf), 2)Score FROM[ExamResultHeader] erh " +
    "LEFT OUTER JOIN[ExamResultDetail] erd ON(erd.ExamResultID = erh.ExamResultID)" +
    "LEFT OUTER JOIN[ExamHeader] eh ON(erh.ExamID = eh.ExamID)" +
    "WHERE erh.ExamID = @e2) ex2 " +
    "ON(ex2.ExamID = esd.ExamID AND ex2.StudentID = esd.StudentID)" +
    "LEFT OUTER JOIN" +
    "(SELECT StudentID, erh.ExamID, SubjectID, CONVERT(decimal(18, 2), Score * (@e3w / eh.OutOf), 2)Score FROM[ExamResultHeader] erh " +
    "LEFT OUTER JOIN[ExamResultDetail] erd ON(erd.ExamResultID = erh.ExamResultID)" +
    "LEFT OUTER JOIN[ExamHeader] eh ON(erh.ExamID = eh.ExamID)" +
    "WHERE erh.ExamID = @e3) ex3 " +
    "ON(ex3.ExamID = esd.ExamID AND ex3.StudentID = esd.StudentID)" +
    "LEFT OUTER JOIN[Subject] s ON(s.SubjectID = ex1.SubjectID OR s.SubjectID = ex2.SubjectID OR s.SubjectID = ex3.SubjectID)" +
    "WHERE cs.ClassID = 1 AND(esd.ExamID = @e1 OR esd.ExamID = @e2 OR esd.ExamID = @e3)" +
    "GROUP BY esd.StudentID, s.SubjectID" +
    ") as s " +
    "PIVOT" +
    "(" +
    "   SUM(Exam1Score)" +
    "    FOR[SubjectID] IN(";
                foreach (var sub in subjects)
                    selectStr += "[" + sub.SubjectID + "],";
                selectStr = selectStr.Remove(selectStr.Length - 1) +
               "))AS pvt ) classRank ON (classRank.StudentID = s.StudentID)" +
    //</ClassRank
    "LEFT OUTER JOIN" +
    //<StreamRank>
    "(SELECT StudentID,DENSE_RANK() OVER(order by";
                foreach (var sub in subjects)
                    selectStr += " ISNULL([" + sub.SubjectID + "], 0) +";
                selectStr = selectStr.Remove(selectStr.Length - 1) +
    "desc) StreamRank FROM(" +
"SELECT esd.StudentID, s.SubjectID, sum(ISNULL(ex1.Score, 0)) + SUM(ISNULL(ex2.Score, 0)) + SUM(ISNULL(ex3.Score, 0)) Exam1Score FROM[ExamStudentDetail] esd " +
"LEFT OUTER JOIN" +
"(SELECT StudentID, erh.ExamID, SubjectID, CONVERT(decimal(18, 2), Score * (@e1w / eh.OutOf), 2)Score FROM[ExamResultHeader] erh " +
"LEFT OUTER JOIN[ExamResultDetail] erd ON(erd.ExamResultID = erh.ExamResultID)" +
"LEFT OUTER JOIN[ExamHeader] eh ON(erh.ExamID = eh.ExamID)" +
"WHERE erh.ExamID = @e1) ex1 " +
"ON(ex1.ExamID = esd.ExamID AND ex1.StudentID = esd.StudentID)" +
"LEFT OUTER JOIN" +
"(SELECT StudentID, erh.ExamID, SubjectID, CONVERT(decimal(18, 2), Score * (@e2w / eh.OutOf), 2)Score FROM[ExamResultHeader] erh " +
"LEFT OUTER JOIN[ExamResultDetail] erd ON(erd.ExamResultID = erh.ExamResultID)" +
"LEFT OUTER JOIN[ExamHeader] eh ON(erh.ExamID = eh.ExamID)" +
"WHERE erh.ExamID = @e2) ex2 " +
"ON(ex2.ExamID = esd.ExamID AND ex2.StudentID = esd.StudentID)" +
"LEFT OUTER JOIN" +
"(SELECT StudentID, erh.ExamID, SubjectID, CONVERT(decimal(18, 2), Score * (@e3w / eh.OutOf), 2)Score FROM[ExamResultHeader] erh " +
"LEFT OUTER JOIN[ExamResultDetail] erd ON(erd.ExamResultID = erh.ExamResultID)" +
"LEFT OUTER JOIN[ExamHeader] eh ON(erh.ExamID = eh.ExamID)" +
"WHERE erh.ExamID = @e3) ex3 " +
"ON(ex3.ExamID = esd.ExamID AND ex3.StudentID = esd.StudentID)" +
"LEFT OUTER JOIN[Subject] s ON(s.SubjectID = ex1.SubjectID OR s.SubjectID = ex2.SubjectID OR s.SubjectID = ex3.SubjectID)" +
"WHERE esd.ExamID = @e1 OR esd.ExamID = @e2 OR esd.ExamID = @e3 " +
"GROUP BY esd.StudentID, s.SubjectID" +
") as s " +
"PIVOT" +
"(" +
"    SUM(Exam1Score)" +
"    FOR[SubjectID] IN(";
                foreach (var sub in subjects)
                    selectStr += "[" + sub.SubjectID + "],";
                selectStr = selectStr.Remove(selectStr.Length - 1) + "))AS pvt )streamRank ON (streamRank.StudentID = esd.StudentID)" +
//</StreamRank>
"WHERE cc.ClassID = @cid AND(esd.ExamID = @e1 OR esd.ExamID = @e2 OR esd.ExamID = @e3)" +
"GROUP BY esd.StudentID, sub.NameOfSubject, sssd.SubjectID, s.NameOfStudent, cc.ClassID, streamRank.StreamRank, classRank.ClassRank,subjectRank.SubjectRank" +
")stx WHERE stx.ClassID=@cid ORDER BY StudentID,SubjectID";

                var paramColl = new List<SqlParameter>();
                paramColl.Add(new SqlParameter("@e1", e1));
                paramColl.Add(new SqlParameter("@e2", e2));
                paramColl.Add(new SqlParameter("@e3", e3));
                paramColl.Add(new SqlParameter("@e1w", e1w));
                paramColl.Add(new SqlParameter("@e2w", e2w));
                paramColl.Add(new SqlParameter("@e3w", e3w));
                paramColl.Add(new SqlParameter("@cid", classID));

                var dt = DataAccessHelper.Helper.ExecuteNonQueryWithResultTable(selectStr, paramColl);

                ClassReportFormModel rpm = new ClassReportFormModel();
                var last_id = 0;
                bool isNew = false;
                ReportFormModel temp = null;
                ReportFormSubjectModel l;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    try
                    {
                        isNew = int.Parse(dt.Rows[i][0].ToString()) != last_id;

                        if (isNew)
                        {
                            if (temp != null)
                                rpm.Add(temp);
                            last_id = int.Parse(dt.Rows[i][0].ToString());
                            temp = new ReportFormModel();
                            temp.StudentID = int.Parse(dt.Rows[i][0].ToString());
                            temp.NameOfStudent = dt.Rows[i][1].ToString();
                            temp.NameOfClass = nameOfClass;
                            temp.StreamRank = dt.Rows[i][10].ToString();
                            temp.ClassRank = dt.Rows[i][9].ToString();
                        }
                        l = new ReportFormSubjectModel();
                        l.SubjectID = int.Parse(dt.Rows[i][4].ToString());
                        l.NameOfSubject = dt.Rows[i][3].ToString();
                        l.Exam1Score = dt.Rows[i][5].ToString();
                        l.Exam2Score = dt.Rows[i][6].ToString();
                        l.Exam3Score = dt.Rows[i][7].ToString();
                        l.StreamRank = dt.Rows[i][8].ToString();
                        l.Code = SubjectModel.AllSubjects.First(o => o.NameOfSubject == l.NameOfSubject).Code;
                        l.MeanScore = (string.IsNullOrWhiteSpace(l.Exam1Score) ? 0 : decimal.Parse(l.Exam1Score)) +
                        (string.IsNullOrWhiteSpace(l.Exam2Score) ? 0 : decimal.Parse(l.Exam2Score)) +
                        (string.IsNullOrWhiteSpace(l.Exam3Score) ? 0 : decimal.Parse(l.Exam3Score));

                        
                        l.Grade = Institution.Controller.DataController.CalculateGrade(l.MeanScore);
                        l.Remarks = Institution.Controller.DataController.GetRemark(l.MeanScore, (l.Code == 102));

                        temp.TotalMarks += l.MeanScore;
                        temp.SubjectEntries.Add(l);
                        temp.TotalPoints += Institution.Controller.DataController.CalculatePoints(l.Grade);
                        temp.MeanScore = temp.SubjectEntries.Count == 0 ? 0 : temp.TotalMarks / temp.SubjectEntries.Count;
                        temp.AvgPoints = temp.SubjectEntries.Count == 0 ? 1 : temp.TotalPoints / temp.SubjectEntries.Count;
                        temp.MeanGrade = Institution.Controller.DataController.CalculateGrade(temp.MeanScore);
                        if (i == dt.Rows.Count - 1)
                            rpm.Add(temp);
                    }
                    catch (Exception ex){ Log.E(ex.ToString(), null); }
                }
                return rpm;
            });
        }

        public static Task<ClassReportForm3Model> GetClassReportForms3Async(int classID, IEnumerable<ExamWeightModel> exams, IProgress<OperationProgress> progress)
        {
            return Task.Factory.StartNew(() =>
            {
                var nameOfClass = Institution.Controller.DataController.GetClass(classID).NameOfClass;
                var subjects = Institution.Controller.DataController.GetInstitutionSubjectsAsync().Result;

                int e1 = exams.Any(o => o.Index == 1) ? exams.First(o => o.Index == 1).ExamID : 0;
                int e2 = exams.Any(o => o.Index == 2) ? exams.First(o => o.Index == 2).ExamID : 0;
                int e3 = exams.Any(o => o.Index == 3) ? exams.First(o => o.Index == 3).ExamID : 0;

                decimal e1w = exams.Any(o => o.Index == 1) ? exams.First(o => o.Index == 1).Weight : 0m;
                decimal e2w = exams.Any(o => o.Index == 2) ? exams.First(o => o.Index == 2).Weight : 0m;
                decimal e3w = exams.Any(o => o.Index == 3) ? exams.First(o => o.Index == 3).Weight : 0m;

                string selectStr =
                "DECLARE @classSCount varchar(50)= '/'+CONVERT(varchar(50),(SELECT COUNT(DISTINCT esd.StudentID) FROM [ExamStudentDetail]esd LEFT OUTER JOIN [StudentClass]sc ON (sc.StudentID=esd.StudentID)" +
                "WHERE sc.ClassID=@cid AND sc.[Year] = DATEPART(year,sysdatetime()) AND esd.ExamID IN (";
                foreach (var t in exams)
                    selectStr += t.ExamID + ",";
                selectStr = selectStr.Remove(selectStr.Length - 1);
                selectStr += ")))\r\n" +

                "DECLARE @streamSCount varchar(50)='/'+CONVERT(varchar(50),(SELECT COUNT(DISTINCT StudentID) FROM [ExamStudentDetail]esd WHERE ExamID IN (";
                foreach (var t in exams)
                    selectStr += t.ExamID + ",";
                selectStr = selectStr.Remove(selectStr.Length - 1);
                selectStr += ")))" +


                "SELECT * FROM (" +


"SELECT esd.StudentID,s.NameOfStudent, cc.ClassID,sub.NameOfSubject, sssd.SubjectID,sum(ex1.Score) Exam1Score, sum(ex2.Score) Exam2Score, sum(ex3.Score) Exam3Score," +
"CONVERT(varchar(50),subjectRank.SubjectRank)+@streamSCount SubjectRank, CONVERT(varchar(50),streamRank.StreamRank)+@streamSCount StreamRank,CONVERT(varchar(50),classRank.ClassRank)+@classSCount ClassRank " +
", s.KCPEScore FROM[ExamStudentDetail] esd " +
"LEFT OUTER JOIN[StudentClass] cc ON(esd.StudentID = cc.StudentID AND cc.Year = DATEPART(YEAR, SYSDATETIME()))" +
"LEFT OUTER JOIN[Student] s ON(s.StudentID = esd.StudentID)" +
"LEFT OUTER JOIN[StudentSubjectSelectionHeader] sssh ON(sssh.StudentID = esd.StudentID AND sssh.Year = DATEPART(year, SYSDATETIME()))" +
"LEFT OUTER JOIN[StudentSubjectSelectionDetail] sssd ON(sssh.StudentSubjectSelectionID = sssd.StudentSubjectSelectionID)" +
"LEFT OUTER JOIN[Subject] sub ON(sub.SubjectID = sssd.SubjectID)" +
"LEFT OUTER JOIN" +
"(SELECT StudentID, erh.ExamID, SubjectID, CONVERT(decimal(18, 2), Score * (@e1w / eh.OutOf), 2)Score FROM[ExamResultHeader] erh " +
"LEFT OUTER JOIN[ExamResultDetail] erd ON(erd.ExamResultID = erh.ExamResultID)" +
"LEFT OUTER JOIN[ExamHeader] eh ON(erh.ExamID = eh.ExamID)" +
"WHERE erh.ExamID = @e1) ex1 " +
"ON(ex1.ExamID = esd.ExamID AND ex1.StudentID = esd.StudentID AND ex1.SubjectID = sssd.SubjectID)" +
"LEFT OUTER JOIN" +
"(SELECT StudentID, erh.ExamID, SubjectID, CONVERT(decimal(18, 2), Score * (@e2w / eh.OutOf), 2)Score FROM[ExamResultHeader] erh " +
"LEFT OUTER JOIN[ExamResultDetail] erd ON(erd.ExamResultID = erh.ExamResultID)" +
"LEFT OUTER JOIN[ExamHeader] eh ON(erh.ExamID = eh.ExamID)" +
"WHERE erh.ExamID = @e2) ex2 " +
"ON(ex2.ExamID = esd.ExamID AND ex2.StudentID = esd.StudentID AND ex2.SubjectID = sssd.SubjectID)" +
"LEFT OUTER JOIN" +
"(SELECT StudentID, erh.ExamID, SubjectID, CONVERT(decimal(18, 2), Score * (@e3w / eh.OutOf), 2)Score FROM[ExamResultHeader] erh " +
"LEFT OUTER JOIN[ExamResultDetail] erd ON(erd.ExamResultID = erh.ExamResultID)" +
"LEFT OUTER JOIN[ExamHeader] eh ON(erh.ExamID = eh.ExamID)" +
"WHERE erh.ExamID = @e3) ex3 " +
"ON(ex3.ExamID = esd.ExamID AND ex3.StudentID = esd.StudentID AND ex3.SubjectID = sssd.SubjectID)" +
"LEFT OUTER JOIN" +

//SUBJECT RANK
"(SELECT StudentID, SubjectID, SubjectRank FROM" +
"(SELECT StudentID,";
                foreach (var sub in subjects)

                    selectStr += "DENSE_RANK() OVER(order by ISNULL([" + sub.SubjectID + "], 0)desc)[" + sub.SubjectID + "],";
                selectStr = selectStr.Remove(selectStr.Length - 1) + " ";
                selectStr +=
                "FROM(" +
                "SELECT esd.StudentID, s.SubjectID, sum(ISNULL(ex1.Score,0))+SUM(ISNULL(ex2.Score,0))+SUM(ISNULL(ex3.Score,0)) Exam1Score FROM " +
                "[ExamStudentDetail] esd " +
                "LEFT OUTER JOIN" +
                "(SELECT StudentID, erh.ExamID, SubjectID, CONVERT(decimal(18,2),Score*(@e1w/eh.OutOf),2)Score FROM[ExamResultHeader] erh " +
                "LEFT OUTER JOIN[ExamResultDetail] " +
                "erd ON(erd.ExamResultID = erh.ExamResultID) LEFT OUTER JOIN[ExamHeader] eh ON(erh.ExamID = eh.ExamID)" +
                "WHERE erh.ExamID=@e1) ex1 ON(ex1.ExamID = esd.ExamID AND ex1.StudentID = esd.StudentID) " +
                "LEFT OUTER JOIN" +
                "(SELECT StudentID, erh.ExamID, SubjectID, CONVERT(decimal(18,2),Score*(@e2w/eh.OutOf),2)Score FROM[ExamResultHeader]" +
                        "erh " +
                  "LEFT OUTER JOIN[ExamResultDetail] " +
                        "erd ON(erd.ExamResultID = erh.ExamResultID)" +
                "LEFT OUTER JOIN[ExamHeader]" +
                        "eh ON(erh.ExamID = eh.ExamID)" +
                "WHERE erh.ExamID=@e2) ex2 " +
                "ON(ex2.ExamID = esd.ExamID AND ex2.StudentID = esd.StudentID)" +
                "LEFT OUTER JOIN" +
                "(SELECT StudentID, erh.ExamID, SubjectID, CONVERT(decimal(18,2),Score*(@e3w/eh.OutOf),2)Score FROM[ExamResultHeader]" +
                        "erh " +
                  "LEFT OUTER JOIN[ExamResultDetail]" +
                        "erd ON(erd.ExamResultID = erh.ExamResultID)" +
                "LEFT OUTER JOIN[ExamHeader]" +
                        "eh ON(erh.ExamID = eh.ExamID)" +
                "WHERE erh.ExamID=@e3) ex3 " +
                "ON(ex3.ExamID = esd.ExamID AND ex3.StudentID = esd.StudentID)" +
                "LEFT OUTER JOIN[Subject]" +
                        "s ON(s.SubjectID = ex1.SubjectID OR s.SubjectID = ex2.SubjectID OR s.SubjectID = ex3.SubjectID)" +
                "WHERE esd.ExamID = @e1 OR esd.ExamID = @e2 OR esd.ExamID = @e3 " +
                "GROUP BY esd.StudentID, s.SubjectID" +
                ") as s " +
                "PIVOT" +
                "(" +
                    "SUM(Exam1Score) " +
                    "FOR[SubjectID] IN(";
                foreach (var sub in subjects)
                    selectStr += "[" + sub.SubjectID + "],";
                selectStr = selectStr.Remove(selectStr.Length - 1) + "))AS pvt )s2 " +

    "UNPIVOT(SubjectRank FOR [SubjectID] in (";
                foreach (var sub in subjects)
                    selectStr += "[" + sub.SubjectID + "],";
                selectStr = selectStr.Remove(selectStr.Length - 1) +
    ")) as unpvt" +
") subjectRank ON(esd.StudentID = subjectRank.StudentID AND sssd.SubjectID = subjectRank.SubjectID)" +
//</SubjectRank>
"LEFT OUTER JOIN " +
//<ClassRank>
"(SELECT StudentID,DENSE_RANK() OVER(order by";
                foreach (var sub in subjects)
                    selectStr += " ISNULL([" + sub.SubjectID + "], 0) +";
                selectStr = selectStr.Remove(selectStr.Length - 1) +
    "desc) classRank " +
    "FROM(" +
    "SELECT esd.StudentID, s.SubjectID, sum(ISNULL(ex1.Score, 0)) + SUM(ISNULL(ex2.Score, 0)) + SUM(ISNULL(ex3.Score, 0)) Exam1Score FROM " +
    "[StudentClass] cs " +
    "LEFT OUTER JOIN[ExamStudentDetail] esd " +
    "ON(cs.StudentID = esd.StudentID AND cs.[Year] = DATEPART(YEAR, SYSDATETIME()))" +
    "LEFT OUTER JOIN" +
    "(SELECT StudentID, erh.ExamID, SubjectID, CONVERT(decimal(18, 2), Score * (@e1w / eh.OutOf), 2)Score FROM[ExamResultHeader] erh " +
    "LEFT OUTER JOIN[ExamResultDetail] erd ON(erd.ExamResultID = erh.ExamResultID)" +
    "LEFT OUTER JOIN[ExamHeader] eh ON(erh.ExamID = eh.ExamID)" +
    "WHERE erh.ExamID = @e1) ex1 " +
    "ON(ex1.ExamID = esd.ExamID AND ex1.StudentID = esd.StudentID)" +
    "LEFT OUTER JOIN" +
    "(SELECT StudentID, erh.ExamID, SubjectID, CONVERT(decimal(18, 2), Score * (@e2w / eh.OutOf), 2)Score FROM[ExamResultHeader] erh " +
    "LEFT OUTER JOIN[ExamResultDetail] erd ON(erd.ExamResultID = erh.ExamResultID)" +
    "LEFT OUTER JOIN[ExamHeader] eh ON(erh.ExamID = eh.ExamID)" +
    "WHERE erh.ExamID = @e2) ex2 " +
    "ON(ex2.ExamID = esd.ExamID AND ex2.StudentID = esd.StudentID)" +
    "LEFT OUTER JOIN" +
    "(SELECT StudentID, erh.ExamID, SubjectID, CONVERT(decimal(18, 2), Score * (@e3w / eh.OutOf), 2)Score FROM[ExamResultHeader] erh " +
    "LEFT OUTER JOIN[ExamResultDetail] erd ON(erd.ExamResultID = erh.ExamResultID)" +
    "LEFT OUTER JOIN[ExamHeader] eh ON(erh.ExamID = eh.ExamID)" +
    "WHERE erh.ExamID = @e3) ex3 " +
    "ON(ex3.ExamID = esd.ExamID AND ex3.StudentID = esd.StudentID)" +
    "LEFT OUTER JOIN[Subject] s ON(s.SubjectID = ex1.SubjectID OR s.SubjectID = ex2.SubjectID OR s.SubjectID = ex3.SubjectID)" +
    "WHERE cs.ClassID = 1 AND(esd.ExamID = @e1 OR esd.ExamID = @e2 OR esd.ExamID = @e3)" +
    "GROUP BY esd.StudentID, s.SubjectID" +
    ") as s " +
    "PIVOT" +
    "(" +
    "   SUM(Exam1Score)" +
    "    FOR[SubjectID] IN(";
                foreach (var sub in subjects)
                    selectStr += "[" + sub.SubjectID + "],";
                selectStr = selectStr.Remove(selectStr.Length - 1) +
               "))AS pvt ) classRank ON (classRank.StudentID = s.StudentID)" +
    //</ClassRank
    "LEFT OUTER JOIN" +
    //<StreamRank>
    "(SELECT StudentID,DENSE_RANK() OVER(order by";
                foreach (var sub in subjects)
                    selectStr += " ISNULL([" + sub.SubjectID + "], 0) +";
                selectStr = selectStr.Remove(selectStr.Length - 1) +
    "desc) StreamRank FROM(" +
"SELECT esd.StudentID, s.SubjectID, sum(ISNULL(ex1.Score, 0)) + SUM(ISNULL(ex2.Score, 0)) + SUM(ISNULL(ex3.Score, 0)) Exam1Score FROM[ExamStudentDetail] esd " +
"LEFT OUTER JOIN" +
"(SELECT StudentID, erh.ExamID, SubjectID, CONVERT(decimal(18, 2), Score * (@e1w / eh.OutOf), 2)Score FROM[ExamResultHeader] erh " +
"LEFT OUTER JOIN[ExamResultDetail] erd ON(erd.ExamResultID = erh.ExamResultID)" +
"LEFT OUTER JOIN[ExamHeader] eh ON(erh.ExamID = eh.ExamID)" +
"WHERE erh.ExamID = @e1) ex1 " +
"ON(ex1.ExamID = esd.ExamID AND ex1.StudentID = esd.StudentID)" +
"LEFT OUTER JOIN" +
"(SELECT StudentID, erh.ExamID, SubjectID, CONVERT(decimal(18, 2), Score * (@e2w / eh.OutOf), 2)Score FROM[ExamResultHeader] erh " +
"LEFT OUTER JOIN[ExamResultDetail] erd ON(erd.ExamResultID = erh.ExamResultID)" +
"LEFT OUTER JOIN[ExamHeader] eh ON(erh.ExamID = eh.ExamID)" +
"WHERE erh.ExamID = @e2) ex2 " +
"ON(ex2.ExamID = esd.ExamID AND ex2.StudentID = esd.StudentID)" +
"LEFT OUTER JOIN" +
"(SELECT StudentID, erh.ExamID, SubjectID, CONVERT(decimal(18, 2), Score * (@e3w / eh.OutOf), 2)Score FROM[ExamResultHeader] erh " +
"LEFT OUTER JOIN[ExamResultDetail] erd ON(erd.ExamResultID = erh.ExamResultID)" +
"LEFT OUTER JOIN[ExamHeader] eh ON(erh.ExamID = eh.ExamID)" +
"WHERE erh.ExamID = @e3) ex3 " +
"ON(ex3.ExamID = esd.ExamID AND ex3.StudentID = esd.StudentID)" +
"LEFT OUTER JOIN[Subject] s ON(s.SubjectID = ex1.SubjectID OR s.SubjectID = ex2.SubjectID OR s.SubjectID = ex3.SubjectID)" +
"WHERE esd.ExamID = @e1 OR esd.ExamID = @e2 OR esd.ExamID = @e3 " +
"GROUP BY esd.StudentID, s.SubjectID" +
") as s " +
"PIVOT" +
"(" +
"    SUM(Exam1Score)" +
"    FOR[SubjectID] IN(";
                foreach (var sub in subjects)
                    selectStr += "[" + sub.SubjectID + "],";
                selectStr = selectStr.Remove(selectStr.Length - 1) + "))AS pvt )streamRank ON (streamRank.StudentID = esd.StudentID)" +
//</StreamRank>
"WHERE cc.ClassID = @cid AND(esd.ExamID = @e1 OR esd.ExamID = @e2 OR esd.ExamID = @e3)" +
"GROUP BY esd.StudentID, sub.NameOfSubject, sssd.SubjectID, s.NameOfStudent, cc.ClassID, streamRank.StreamRank, classRank.ClassRank,subjectRank.SubjectRank,s.KCPEScore" +
")stx WHERE stx.ClassID=@cid ORDER BY StudentID,SubjectID";

                var paramColl = new List<SqlParameter>();
                paramColl.Add(new SqlParameter("@e1", e1));
                paramColl.Add(new SqlParameter("@e2", e2));
                paramColl.Add(new SqlParameter("@e3", e3));
                paramColl.Add(new SqlParameter("@e1w", e1w));
                paramColl.Add(new SqlParameter("@e2w", e2w));
                paramColl.Add(new SqlParameter("@e3w", e3w));
                paramColl.Add(new SqlParameter("@cid", classID));

                var dt = DataAccessHelper.Helper.ExecuteNonQueryWithResultTable(selectStr, paramColl);

                ClassReportForm3Model rpm = new ClassReportForm3Model();
                var last_id = 0;
                bool isNew = false;
                ReportForm3Model temp = null;
                ReportFormSubjectModel l;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    try
                    {
                        isNew = int.Parse(dt.Rows[i][0].ToString()) != last_id;

                        if (isNew)
                        {
                            if (temp != null)
                                rpm.Add(temp);
                            last_id = int.Parse(dt.Rows[i][0].ToString());
                            temp = new ReportForm3Model();
                            temp.StudentID = int.Parse(dt.Rows[i][0].ToString());
                            temp.NameOfStudent = dt.Rows[i][1].ToString();
                            temp.NameOfClass = nameOfClass;
                            temp.StreamRank = dt.Rows[i][10].ToString();
                            temp.ClassRank = dt.Rows[i][9].ToString();
                            decimal yu;
                            
                            temp.KCPEGrade = !decimal.TryParse(dt.Rows[i][11].ToString(), out yu) ? "E" : Institution.Controller.DataController.CalculateGrade(yu*100 / 500);
                            temp.KCPEScore = yu;
                        }
                        l = new ReportFormSubjectModel();
                        l.SubjectID = int.Parse(dt.Rows[i][4].ToString());
                        l.NameOfSubject = dt.Rows[i][3].ToString();
                        l.Exam1Score = dt.Rows[i][5].ToString();
                        l.Exam2Score = dt.Rows[i][6].ToString();
                        l.Exam3Score = dt.Rows[i][7].ToString();
                        l.StreamRank = dt.Rows[i][8].ToString();
                        
                        l.Code = SubjectModel.AllSubjects.First(o => o.NameOfSubject == l.NameOfSubject).Code;
                        l.MeanScore = (string.IsNullOrWhiteSpace(l.Exam1Score) ? 0 : decimal.Parse(l.Exam1Score)) +
                        (string.IsNullOrWhiteSpace(l.Exam2Score) ? 0 : decimal.Parse(l.Exam2Score)) +
                        (string.IsNullOrWhiteSpace(l.Exam3Score) ? 0 : decimal.Parse(l.Exam3Score));


                        l.Grade = Institution.Controller.DataController.CalculateGrade(l.MeanScore);
                        l.Remarks = Institution.Controller.DataController.GetRemark(l.MeanScore, (l.Code == 102));
                        
                        temp.TotalMarks += l.MeanScore;
                        temp.SubjectEntries.Add(l);
                        temp.TotalPoints += Institution.Controller.DataController.CalculatePoints(l.Grade);
                        temp.MeanScore = temp.SubjectEntries.Count == 0 ? 0 : temp.TotalMarks / temp.SubjectEntries.Count;
                        

                        if (App.AppExamSettings.MeanGradeCalculation == 1)
                            temp.MeanGrade = Institution.Controller.DataController.CalculateGrade(temp.MeanScore);
                        else
                            temp.MeanGrade = Institution.Controller.DataController.CalculateGradeFromPoints(temp.TotalPoints == 0 ? 1 : temp.TotalPoints / temp.SubjectEntries.Count);
                        var v = GetReportFormGrades(temp.SubjectEntries, new List<ExamWeightModel>(exams));
                        temp.CAT1Grade = v[0];
                        temp.CAT2Grade = v[1];
                        temp.ExamGrade = v[2];
                        if (i == dt.Rows.Count - 1)
                            rpm.Add(temp);
                    }
                    catch (Exception ex) { Log.E(ex.ToString(), null); }
                }
                return rpm;
            });
        }

        public static Task<ObservableCollection<ExamResultStudentSubjectEntryModel>> GetStudentSubjectsResults(int classID, int examID, int subjectID, decimal outOf)
        {
            return Task.Factory.StartNew(delegate
            {
                ObservableCollection<ExamResultStudentSubjectEntryModel> observableCollection = new ObservableCollection<ExamResultStudentSubjectEntryModel>();
                string commandText = string.Concat(new object[]
                {
                    "SELECT s.StudentID, s.NameOfStudent, ISNULL(erd.Score,0),ISNULL(erd.Remarks,''),ISNULL(erh.ExamResultID,0), sub.NameOfSubject FROM ",
                    "[StudentSubjectSelectionDetail] sssd LEFT OUTER JOIN [StudentSubjectSelectionHeader] sssh ",
                    "ON (sssd.StudentSubjectSelectionID=sssh.StudentSubjectSelectionID) LEFT OUTER JOIN [Student] s ",
                    "ON (sssh.StudentID=s.StudentID AND sssh.[Year]=DATEPART(YEAR,SYSDATETIME())) LEFT OUTER JOIN (SELECT * FROM [ExamResultHeader] WHERE ExamID=@eid) erh ",
                    "ON (sssh.StudentID=erh.StudentID) LEFT OUTER JOIN [ExamresultDetail] erd ",
                    "ON (erh.ExamresultID=erd.ExamResultID AND sssd.SubjectID=erd.SubjectID) LEFT OUTER JOIN [Subject] sub ",
                    "ON (sssd.SubjectID=sub.SubjectID) LEFT OUTER JOIN [StudentClass] cs ",
                    "ON (sssh.StudentID=cs.StudentID AND cs.[Year]=DATEPART(YEAR,SYSDATETIME())) WHERE sssd.SubjectID=@sid AND cs.ClassID=@cid AND cs.[Year]=DATEPART(YEAR,SYSDATETIME())",
                    "AND sssh.[Year]=DATEPART(YEAR,SYSDATETIME()) AND s.IsActive=1 ORDER BY s.StudentID"
                });
                var paramColl = new List<SqlParameter>();
                paramColl.Add(new SqlParameter("@eid", examID));
                paramColl.Add(new SqlParameter("@cid", classID));
                paramColl.Add(new SqlParameter("@sid", subjectID));

                DataTable dataTable = DataAccessHelper.Helper.ExecuteNonQueryWithResultTable(commandText, paramColl);
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

        public static Task<bool> SaveNewExamResultAsync(ExamResultStudentModel newResult)
        {
            return Task.Factory.StartNew<bool>(delegate
            {
                string text = "BEGIN TRANSACTION\r\nDECLARE @id int; \r\n ";
                text += "SET @id = dbo.GetNewID('dbo.ExamResultHeader')\r\n";
                object obj = text;
                text = string.Concat(new object[]
                {
                    obj,
                    "IF NOT EXISTS (SELECT * FROM [ExamResultHeader] WHERE ExamID=",
                    newResult.ExamID,
                    " AND StudentID=",
                    newResult.StudentID,
                    ")\r\n"
                });
                obj = text;
                text = string.Concat(new object[]
                {
                    obj,
                    "INSERT INTO [ExamResultHeader] (ExamResultID,ExamID,StudentID) VALUES (@id,",
                    newResult.ExamID,
                    ",",
                    newResult.StudentID,
                    ")\r\n"
                });
                obj = text;
                text = string.Concat(new object[]
                {
                    obj,
                    "ELSE SET @id=(SELECT ExamResultID FROM [ExamResultHeader] WHERE ExamID=",
                    newResult.ExamID,
                    " AND StudentID=",
                    newResult.StudentID,
                    ")\r\n"
                });
                foreach (ExamResultSubjectEntryModel current in newResult.Entries)
                {
                    obj = text;
                    text = string.Concat(new object[]
                    {
                        obj,
                        "IF NOT EXISTS (SELECT * FROM [ExamResultDetail] WHERE ExamResultID=@id AND SubjectID=",
                        current.SubjectID,
                        ")\r\n"
                    });
                    obj = text;
                    text = string.Concat(new object[]
                    {
                        obj,
                        "INSERT INTO [ExamResultDetail] (ExamResultID,SubjectID,Score,Remarks,Tutor) VALUES (@id,",
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
                        "ELSE UPDATE [ExamResultDetail] SET Score='",
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
                return DataAccessHelper.Helper.ExecuteNonQuery(text);
            });
        }

        public static Task<bool> SaveNewExamResultAsync(ObservableCollection<ExamResultStudentModel> newResult)
        {
            return Task.Factory.StartNew<bool>(delegate
            {
                string text = "BEGIN TRANSACTION\r\nDECLARE @id int; \r\n ";
                int count1 = 0;
                var paramColl = new List<SqlParameter>();

                foreach (ExamResultStudentModel current in newResult)
                {
                    count1++;
                    text += "SET @id = dbo.GetNewID('dbo.ExamResultHeader')\r\n";
                    object obj = text;
                    if (count1 < 2)
                        paramColl.Add(new SqlParameter("@eid", current.ExamID));
                    paramColl.Add(new SqlParameter("@sid" + count1, current.StudentID));
                    text = string.Concat(new object[]
                    {
                        obj,
                        "IF NOT EXISTS (SELECT * FROM [ExamResultHeader] WHERE ExamID=@eid",
                        " AND StudentID=@sid",count1,
                        " )\r\n"
                    });
                    obj = text;
                    text = string.Concat(new object[]
                    {
                        obj,
                        "INSERT INTO [ExamResultHeader] (ExamResultID,ExamID,StudentID) VALUES (@id,@eid",
                        ",@sid",count1,
                        ")\r\n"
                    });
                    obj = text;
                    text = string.Concat(new object[]
                    {
                        obj,
                        "ELSE SET @id=(SELECT ExamResultID FROM [ExamResultHeader] WHERE ExamID=@eid",
                        " AND StudentID=@sid",count1,
                        ")\r\n"
                    });
                    var current2 = current.Entries[0];

                    obj = text;
                    if (count1 < 2)
                        paramColl.Add(new SqlParameter("@subid", current2.SubjectID));
                    paramColl.Add(new SqlParameter("@subScore" + count1, current2.Score));
                    paramColl.Add(new SqlParameter("@subRemark" + count1, current2.Remarks));
                    paramColl.Add(new SqlParameter("@subTutor" + count1, current2.Tutor));
                    text = string.Concat(new object[]
                    {
                            obj,
                            "IF NOT EXISTS (SELECT * FROM [ExamResultDetail] WHERE ExamResultID=@id AND SubjectID=@subid",
                            ")\r\n"
                    });
                    obj = text;
                    text = string.Concat(new object[]
                    {
                            obj,
                            "INSERT INTO [ExamResultDetail] (ExamResultID,SubjectID,Score,Remarks,Tutor) VALUES (@id,@subid",
                            ",@subScore",count1,",@subRemark",count1,",@subTutor",count1,
                            ")\r\n"
                    });
                    obj = text;
                    text = string.Concat(new object[]
                    {
                            obj,
                            "ELSE UPDATE [ExamResultDetail] SET Score=@subScore",count1,
                            ", Remarks=@subRemark",count1,
                            ", Tutor=@subTutor",count1,
                            " WHERE ExamResultID=@id AND SubjectID=@subid\r\n"
                    });

                }
                text += " COMMIT";
                return DataAccessHelper.Helper.ExecuteNonQuery(text, paramColl);
            });

        }

        public static Task<ExamResultStudentModel> GetStudentExamResultAync(int studentID, int examID, decimal outOf)
        {
            string selectStr = string.Concat(new object[]
            {
                "SELECT sssd.SubjectID, s.NameOfSubject, ISNULL(erd.Score,0), erd.Remarks,s.Code,erh.ExamResultID FROM [StudentSubjectSelectionDetail] sssd LEFT OUTER JOIN [StudentSubjectSelectionHeader] sssh ON(sssd.StudentSubjectSelectionID=sssh.StudentSubjectSelectionID) LEFT OUTER JOIN [ExamResultHeader] erh ON (sssh.StudentID=erh.StudentID) LEFT OUTER JOIN [ExamResultDetail] erd ON (erh.ExamResultID = erd.ExamResultID AND erd.SubjectID=sssd.SubjectID) LEFT OUTER JOIN [Subject] s ON(sssd.SubjectID=s.SubjectID) LEFT OUTER JOIN [Student] st ON (sssh.StudentID = st.StudentID) LEFT OUTER JOIN [StudentClass] cs ON (st.StudentID=cs.StudentID AND cs.[Year]=DATEPART(year,sysdatetime())) WHERE sssh.[Year]=DATEPART(year,sysdatetime()) AND sssh.StudentID=",
                studentID,
                " AND erh.ExamID=",
                examID,
                " ORDER BY s.[Code]"
            });
            return Task.Factory.StartNew<ExamResultStudentModel>(delegate
            {
                ExamResultStudentModel examResultStudentModel = new ExamResultStudentModel();
                examResultStudentModel.StudentID = studentID;
                examResultStudentModel.ExamID = examID;
                DataTable dataTable = DataAccessHelper.Helper.ExecuteNonQueryWithResult(selectStr);
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    ExamResultSubjectEntryModel examResultSubjectEntryModel = new ExamResultSubjectEntryModel();
                    examResultSubjectEntryModel.SubjectID = int.Parse(dataRow[0].ToString());
                    examResultSubjectEntryModel.NameOfSubject = dataRow[1].ToString();
                    examResultSubjectEntryModel.Remarks = dataRow[3].ToString();
                    examResultSubjectEntryModel.OutOf = outOf;
                    examResultSubjectEntryModel.Score = (string.IsNullOrWhiteSpace(dataRow[2].ToString()) ? 0m : decimal.Parse(dataRow[2].ToString()));
                    examResultSubjectEntryModel.Grade = Institution.Controller.DataController.CalculateGrade(decimal.Parse(((100m / outOf) * examResultSubjectEntryModel.Score).ToString("N2")));
                    examResultSubjectEntryModel.Points = Institution.Controller.DataController.CalculatePoints(examResultSubjectEntryModel.Grade);
                    examResultSubjectEntryModel.Code = int.Parse(dataRow[4].ToString());
                    examResultSubjectEntryModel.ExamResultID = int.Parse(dataRow[5].ToString());
                    examResultStudentModel.Total += examResultSubjectEntryModel.Score;
                    examResultStudentModel.TotalPoints += examResultSubjectEntryModel.Points;
                    examResultStudentModel.Entries.Add(examResultSubjectEntryModel);
                }
                examResultStudentModel.TotalPoints = dataTable.Rows.Count == 0 ? 1 : decimal.Parse((examResultStudentModel.TotalPoints / dataTable.Rows.Count).ToString("N2"));
                if (App.AppExamSettings.MeanGradeCalculation == 0)
                    examResultStudentModel.MeanGrade = Institution.Controller.DataController.CalculateGradeFromPoints(examResultStudentModel.TotalPoints);
                else
                    examResultStudentModel.MeanGrade = Institution.Controller.DataController.CalculateGrade(examResultStudentModel.Entries.Count == 0 ? 0 : examResultStudentModel.Total / examResultStudentModel.Entries.Count);
                return examResultStudentModel;
            });
        }

        public static Task<ExamResultClassModel> GetClassExamResultAsync(int classID, int examID, decimal outOf)
        {
            return Task.Factory.StartNew<ExamResultClassModel>(delegate
            {
                ExamResultClassModel examResultClassModel = new ExamResultClassModel();
                examResultClassModel.ClassID = classID;
                examResultClassModel.NameOfClass = Institution.Controller.DataController.GetClass(classID).NameOfClass;
                ObservableCollection<SubjectModel> result = Institution.Controller.DataController.GetInstitutionSubjectsAsync().Result;
                string text = "SELECT s.StudentID, NameOfStudent,";
                object obj;
                foreach (SubjectModel current in result)
                {
                    obj = text;
                    text = string.Concat(new object[]
                    {
                        obj,
                        "dbo.GetWeightedExamSubjectScore(s.StudentID,",
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
                    " FROM [Student]s LEFT OUTER JOIN [StudentClass] cs ON (s.StudentID=cs.StudentID AND cs.[Year]=DATEPART(year,sysdatetime())) WHERE cs.ClassID=",
                    classID,
                    " AND s.IsACtive=1"
                });
                DataTable dataTable = DataAccessHelper.Helper.ExecuteNonQueryWithResult(text);
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
                            examResultSubjectEntryModel.Grade = Institution.Controller.DataController.CalculateGrade(decimal.Parse(((100m / outOf) * examResultSubjectEntryModel.Score).ToString("N2")));
                            examResultSubjectEntryModel.Points = Institution.Controller.DataController.CalculatePoints(examResultSubjectEntryModel.Grade);
                            examResultSubjectEntryModel.OutOf = outOf;
                            examResultStudentModel.Entries.Add(examResultSubjectEntryModel);
                            RemoveLowestOptional(examResultStudentModel.Entries, examResultClassModel.NameOfClass);
                        }
                    }

                    foreach (ExamResultSubjectEntryModel current in examResultStudentModel.Entries)
                    {
                        examResultStudentModel.Total += current.Score;
                        examResultStudentModel.TotalPoints += current.Points;
                    }
                    if (App.AppExamSettings.MeanGradeCalculation == 0)
                        examResultStudentModel.MeanGrade = Institution.Controller.DataController.CalculateGradeFromPoints(examResultStudentModel.Entries.Count == 0 ? 1 : decimal.Parse((examResultStudentModel.TotalPoints / examResultStudentModel.Entries.Count).ToString("N2")));
                    else
                        examResultStudentModel.MeanGrade = Institution.Controller.DataController.CalculateGrade(examResultStudentModel.Entries.Count == 0 ? 0 : (examResultStudentModel.Total*100 / (examResultStudentModel.Entries.Count*outOf)));
                    
                    examResultClassModel.Entries.Add(examResultStudentModel);
                }
                return examResultClassModel;
            });
        }

        private static void RemoveLowestOptional(ObservableCollection<ExamResultSubjectEntryModel> subjects, string className)
        {
            bool getBest7 = false;
            switch (App.AppExamSettings.Best7Subjects)
            {
                case 0: getBest7 = className.ToLowerInvariant().Contains("form 4"); break;
                case 1: getBest7 = className.ToLowerInvariant().Contains("form 4") || className.ToLowerInvariant().Contains("form 3"); break;
                case 2: getBest7 = true; break;
                case 3: getBest7 = false; break;
            }

            List<int> optionals = new List<int>
            {
                311,
                312,
                443,
                565
            };
            var opts = (from a in subjects
                        where optionals.Contains(a.Code)
                        select a);

            if (opts.Count() > 0 && getBest7)
            {
                decimal min = opts.Min((ExamResultSubjectEntryModel o) => o.Score);
                var item = subjects.First((ExamResultSubjectEntryModel a) => optionals.Contains(a.Code) && a.Score == min);
                if (subjects.Count < 11 && subjects.Count > 7)
                {
                    subjects.Remove(item);
                }
            }
        }

        public static Task<ExamResultClassDisplayModel> GetClassExamResultForTranscriptAsync(int classID, int examID, decimal outOf)
        {
            return Task.Factory.StartNew<ExamResultClassDisplayModel>(delegate
            {
                ExamResultClassDisplayModel classStudentsExamResultModel = new ExamResultClassDisplayModel();
                ObservableCollection<SubjectModel> result = Institution.Controller.DataController.GetInstitutionSubjectsAsync().Result;
                string text = "SELECT s.StudentID, NameOfStudent,";
                object obj;
                foreach (SubjectModel current in result)
                {
                    obj = text;
                    text = string.Concat(new object[]
                    {
                        obj,
                        "dbo.GetWeightedExamSubjectScore(s.StudentID,",
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
                    " FROM [Student] s LEFT OUTER JOIN [StudentClass] cs ON (s.StudentID=cs.StudentID AND cs.[Year]=DATEPART(year,sysdatetime())) WHERE cs.ClassID=",
                    classID,
                    " AND IsACtive=1"
                });
                DataTable dataTable = DataAccessHelper.Helper.ExecuteNonQueryWithResult(text);
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    var studentExamResultModel = new ExamResultStudentDisplayModel();
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
                            studentExamResultModel.Entries.Add(examResultSubjectEntryModel);
                        }
                    }
                    decimal num=0,num2 = 0;
                    studentExamResultModel.OverAllPosition = GetOverallPosition(studentExamResultModel.StudentID, examID);
                    foreach (var current in studentExamResultModel.Entries)
                    {
                        num += current.Score;
                    }
                    foreach (ExamResultSubjectEntryModel current2 in studentExamResultModel.Entries)
                    {
                        num2 += current2.Points;
                    }
                    studentExamResultModel.MeanGrade = ((studentExamResultModel.Entries.Count > 0) ? Institution.Controller.DataController.CalculateGradeFromPoints((num2 + (studentExamResultModel.Entries.Count - 1)) / studentExamResultModel.Entries.Count) : "E");
                    studentExamResultModel.Total= num;
                    studentExamResultModel.TotalPoints = Institution.Controller.DataController.CalculatePoints(studentExamResultModel.MeanGrade);
                    classStudentsExamResultModel.Entries.Add(studentExamResultModel);
                }
                return classStudentsExamResultModel;
            });
        }
        
        public static Task<bool> SaveNewExamAsync(ExamModel newExam)
        {
            return Task.Factory.StartNew<bool>(delegate
            {
                var paramColl = new List<SqlParameter>();
                string text = string.Concat(new object[]
                {
                    "BEGIN TRANSACTION\r\nDECLARE @id int;\r\n SET @id = dbo.GetNewID('dbo.ExamHeader') INSERT INTO [ExamHeader] (ExamID,NameOfExam,OutOf,ExamDateTime) ",
                    "VALUES (@id,@name,@ouf,@dtime)\r\n"
                });
                paramColl.Add(new SqlParameter("@name", newExam.NameOfExam));
                paramColl.Add(new SqlParameter("@ouf", newExam.OutOf));
                paramColl.Add(new SqlParameter("@dtime", newExam.ExamDateTime));
                int index = 0;
                foreach (ClassModel current in newExam.Classes)
                {
                    object obj = text;
                    text = string.Concat(new object[]
                    {
                        obj,
                        "INSERT INTO [ExamClassDetail] (ExamID,ClassID) VALUES (@id,@cls"+index+")\r\n"
                    });
                    paramColl.Add(new SqlParameter("@cls" + index, current.ClassID));
                    string selecteStr = "SELECT s.StudentID FROM [Student]s LEFT OUTER JOIN [StudentClass] cs ON (s.StudentID=cs.StudentID AND cs.[Year]=DATEPART(year,sysdatetime())) WHERE cs.ClassID =@cls" + index;
                    var pms = new List<SqlParameter>() { new SqlParameter("@cls" + index, current.ClassID) };
                    List<string> list = DataAccessHelper.Helper.CopyFirstColumnToList(selecteStr, pms);
                    foreach (var t in list)
                    {
                        text += "IF NOT EXISTS (SELECT * FROM [ExamStudentDetail] WHERE StudentID=" + t + " AND ExamID=@id)\r\n" +
                            "INSERT INTO [ExamStudentDetail] (ExamID,StudentID) VALUES (@id" +
                            "," + t +
                            ")\r\n";
                    }
                    index++;
                }
                foreach (var current2 in newExam.Entries)
                {
                    object obj = text;
                    text = string.Concat(new object[]
                    {
                        obj,
                        "INSERT INTO [ExamDetail] (ExamID,SubjectID) VALUES (@id,",
                        current2.SubjectID,")\r\n"
                    });
                }

                text += " COMMIT";


                return DataAccessHelper.Helper.ExecuteNonQuery(text, paramColl);
            });
        }

        public static Task<bool> RemoveExamAsync(int examID)
        {
            return Task.Factory.StartNew<bool>(delegate
            {
                string text = "BEGIN TRANSACTION\r\n";
                object obj = text;
                text = string.Concat(new object[]
                {
                    obj,
                    "DELETE FROM [ExamClassDetail] WHERE ExamID=",
                    examID,
                    "\r\nDELETE FROM [ExamResultDetail] WHERE ExamResultID IN (SELECT ExamResultID FROM [ExamResultHeader] WHERE ExamID=",
                    examID,
                    ")\r\nDELETE FROM [ExamResultHeader] WHERE ExamID=",
                    examID,
                    "\r\nDELETE FROM [ExamDetail] WHERE ExamID=",
                    examID,
                     "\r\nDELETE FROM [ExamStudentDetail] WHERE ExamID=",
                    examID,
                    "\r\nDELETE FROM [ExamHeader] WHERE ExamID=",
                    examID,
                    "\r\n"
                });
                text += " COMMIT";
                return DataAccessHelper.Helper.ExecuteNonQuery(text);
            });
        }
        
        internal static string GetClassPosition(int studentID, int examID)
        {
            int classIDFromStudent = Students.Controller.DataController.GetClassIDFromStudentID(studentID).Result;
            string commandText = string.Concat(new object[]
            {
                "SELECT row_no,no_of_students FROM(SELECT ROW_NUMBER() OVER(ORDER BY ISNULL(SUM(ISNULL(CONVERT(decimal,erd.Score),0)),0) DESC) row_no, res.StudentID,(SELECT COUNT(*) FROM [Student]s LEFT OUTER JOIN [StudentClass]sc ON (s.StudentID=sc.StudentID) WHERE sc.[Year]=DATEPART(year,sysdatetime()) AND sc.ClassID =",
                classIDFromStudent,
                ")no_of_students FROM [ExamResultDetail] erd RIGHT OUTER JOIN (SELECT StudentID,ExamResultID FROM [ExamResultHeader] WHERE ExamID =",
                examID,
                " AND StudentID IN (SELECT s.StudentID FROM [Student]s LEFT OUTER JOIN [StudentClass]sc ON (s.StudentID=sc.StudentID) WHERE sc.[Year]=DATEPART(year,sysdatetime()) AND sc.ClassID =",
                classIDFromStudent,
                ")) res ON (erd.ExamResultID=res.ExamResultID) GROUP BY res.StudentID )x WHERE x.StudentID=",
                studentID
            });
            DataTable dataTable = DataAccessHelper.Helper.ExecuteNonQueryWithResult(commandText);
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
            char c = Institution.Controller.DataController.GetClass(Students.Controller.DataController.GetClassIDFromStudentID(studentID).Result).NameOfClass[5];
            string commandText = string.Concat(new object[]
            {
                "SELECT row_no,studs FROM(SELECT ROW_NUMBER() OVER(ORDER BY ISNULL(SUM(ISNULL(CONVERT(decimal,erd.Score),0)),0) DESC) row_no, res.StudentID, (SELECT COUNT(*) FROM [Student]s LEFT OUTER JOIN [StudentClass]sc ON (s.StudentID=sc.StudentID) WHERE sc.[Year]=DATEPART(year,sysdatetime()) AND sc.ClassID IN(SELECT ClassID FROM [Class] WHERE NameOfClass LIKE '%",
                c,
                "%'))studs FROM [ExamResultDetail] erd RIGHT OUTER JOIN (SELECT StudentID,ExamResultID FROM [ExamResultHeader] WHERE ExamID =",
                examID,
                ") res ON (erd.ExamResultID=res.ExamResultID) GROUP BY res.StudentID)x WHERE x.StudentID=",
                studentID
            });
            DataTable dataTable = DataAccessHelper.Helper.ExecuteNonQueryWithResult(commandText);
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


        public static async Task<ExamResultClassModel> GetClassCombinedExamResultAsync(int classID, ObservableCollection<ExamWeightModel> exams)
        {
            ExamResultClassModel examResultClassModel = new ExamResultClassModel();
            examResultClassModel.ClassID = classID;
            examResultClassModel.NameOfClass = Institution.Controller.DataController.GetClass(classID).NameOfClass;
            ObservableCollection<SubjectModel> observableCollection = await Institution.Controller.DataController.GetInstitutionSubjectsAsync();
            string text = "SELECT s.StudentID, NameOfStudent,";
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
                            "dbo.GetWeightedExamSubjectScore(s.StudentID,",
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
                " FROM [Student]s LEFT OUTER JOIN [StudentClass] cs ON (s.StudentID=cs.StudentID AND cs.[Year]=DATEPART(year,sysdatetime())) WHERE cs.ClassID=",
                classID,
                " AND s.IsACtive=1"
            });
            DataTable dataTable = DataAccessHelper.Helper.ExecuteNonQueryWithResult(text);
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
                        examResultSubjectEntryModel.Grade = Institution.Controller.DataController.CalculateGrade(examResultSubjectEntryModel.Score);
                        examResultSubjectEntryModel.Points = Institution.Controller.DataController.CalculatePoints(examResultSubjectEntryModel.Grade);
                        examResultSubjectEntryModel.MaximumScore = 100m;                        
                        examResultStudentModel.Total += examResultSubjectEntryModel.Score;
                        examResultStudentModel.TotalPoints += examResultSubjectEntryModel.Points;
                        examResultStudentModel.Entries.Add(examResultSubjectEntryModel);
                        RemoveLowestOptional(examResultStudentModel.Entries, examResultClassModel.NameOfClass);
                    }
                }
                if (App.AppExamSettings.MeanGradeCalculation == 0)
                    examResultStudentModel.MeanGrade = Institution.Controller.DataController.CalculateGradeFromPoints(examResultStudentModel.Entries.Count == 0 ? 1 : decimal.Parse((examResultStudentModel.TotalPoints / examResultStudentModel.Entries.Count).ToString("N2")));
                else
                    examResultStudentModel.MeanGrade = Institution.Controller.DataController.CalculateGrade(examResultStudentModel.Entries.Count == 0 ? 0 : examResultStudentModel.Total / examResultStudentModel.Entries.Count);
                examResultClassModel.Entries.Add(examResultStudentModel);
            }
            return examResultClassModel;
        }

        public static async Task<ExamResultClassModel> GetCombinedClassCombinedExamResultAsync(ObservableCollection<ClassModel> classes, ObservableCollection<ExamWeightModel> exams)
        {
            ExamResultClassModel examResultClassModel = new ExamResultClassModel();

            examResultClassModel.NameOfClass = classes.First().NameOfClass;
            ObservableCollection<SubjectModel> observableCollection = await Institution.Controller.DataController.GetInstitutionSubjectsAsync();
            string text = "SELECT s.StudentID, NameOfStudent,";
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
                            "dbo.GetWeightedExamSubjectScore(s.StudentID,",
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
            text = text + " FROM [Student]s LEFT OUTER JOIN [StudentClass] cs ON (s.StudentID=cs.StudentID AND cs.[Year]=DATEPART(year,sysdatetime())) WHERE ClassID IN (" + text2 + ") AND IsACtive=1";
            DataTable dataTable = DataAccessHelper.Helper.ExecuteNonQueryWithResult(text);
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
                        examResultSubjectEntryModel.Grade = Institution.Controller.DataController.CalculateGrade(examResultSubjectEntryModel.Score);
                        examResultSubjectEntryModel.Points = Institution.Controller.DataController.CalculatePoints(examResultSubjectEntryModel.Grade);
                        examResultSubjectEntryModel.MaximumScore = 100m;
                        examResultStudentModel.Total += examResultSubjectEntryModel.Score;
                        examResultStudentModel.TotalPoints += examResultSubjectEntryModel.Points;
                        examResultStudentModel.Entries.Add(examResultSubjectEntryModel);
                        RemoveLowestOptional(examResultStudentModel.Entries, examResultClassModel.NameOfClass);
                    }
                }
                if (App.AppExamSettings.MeanGradeCalculation == 0)
                    examResultStudentModel.MeanGrade = Institution.Controller.DataController.CalculateGradeFromPoints(examResultStudentModel.Entries.Count == 0 ? 1 : decimal.Parse((examResultStudentModel.TotalPoints / examResultStudentModel.Entries.Count).ToString("N2")));
                else
                    examResultStudentModel.MeanGrade = Institution.Controller.DataController.CalculateGrade(examResultStudentModel.Entries.Count == 0 ? 0 : examResultStudentModel.Total / examResultStudentModel.Entries.Count);
                examResultClassModel.Entries.Add(examResultStudentModel);
            }
            return examResultClassModel;
        }


    }
}
