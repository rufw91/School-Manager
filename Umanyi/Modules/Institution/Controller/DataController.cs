using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using UmanyiSMS.Lib.Controllers;
using UmanyiSMS.Lib.Presentation;
using UmanyiSMS.Modules.Institution.Models;

namespace UmanyiSMS.Modules.Institution.Controller
{
    public class DataController
    {
        
        public static Task<Dictionary<int, DateTime?[]>> GetTermDatesAsync(int schoolYear)
        {
            return Task.Factory.StartNew<Dictionary<int, DateTime?[]>>(delegate
            {
                Dictionary<int, DateTime?[]> temp = new Dictionary<int, DateTime?[]>();

                string selectStr = "SELECT Value,Value3,Value4 FROM [Settings] WHERE [Type] ='TermSettings' AND [Key]=@syear";
                var pc1 = new ObservableCollection<SqlParameter>();
                pc1.Add(new SqlParameter("@syear", schoolYear.ToString()));
                DataTable dt = DataAccessHelper.Helper.ExecuteNonQueryWithResultTable(selectStr, pc1);
                foreach (DataRow dtr in dt.Rows)
                {
                    DateTime?[] dys = new DateTime?[2];
                    dys[0] = DateTime.Parse(dtr[1].ToString());
                    dys[1] = DateTime.Parse(dtr[2].ToString());
                    temp.Add(int.Parse(dtr[0].ToString()), dys);
                }
                if (temp.Count == 0)
                {
                    for (int i = 1; i < 4; i++)
                        temp.Add(i, new DateTime?[2]);

                    temp[1][0] = new DateTime(schoolYear, 1, 1);
                    temp[1][1] = new DateTime(schoolYear, 4, 30);

                    temp[2][0] = new DateTime(schoolYear, 5, 1);
                    temp[2][1] = new DateTime(schoolYear, 8, 31);

                    temp[3][0] = new DateTime(schoolYear, 9, 1);
                    temp[3][1] = new DateTime(schoolYear, 12, 31);

                }
                return temp;
            });
        }

        public static string GetRemark(decimal score, bool isSwahili)
        {
            int num = CalculatePoints(CalculateGrade(score));
            if (!isSwahili)

                return App.AppExamSettings.GradeRemarks[12 - num];
            else
                return App.AppExamSettings.SwahiliGradeRemarks[12 - num];
        }

        internal static string CalculateGrade(decimal scoreNew)
        {
            decimal num = decimal.Ceiling(scoreNew);           
         return CalculateGradeFromPoints(12-App.AppExamSettings.GradeRanges.IndexOf(App.AppExamSettings.GradeRanges.First(o => num >= o.Key && num <= o.Value)));            
        }


        internal static int GetTerm(DateTime dateTime)
        {
            var res = GetTermDatesAsync(dateTime.Year).Result.Where(o => o.Value[0] < dateTime && o.Value[1] > dateTime);
            if (res != null || res.Count() == 1)
                return res.First().Key;
            else return -1;
        }

        internal static TermModel GetTerm(int index)
        {
            TermModel term = new TermModel();
            if (index > 0)
            {
                var res = GetTermDatesAsync(DateTime.Now.Year).Result[index];
                term.TermID = index;
                term.Description = "TERM " + index;
                term.StartDate = res[0].Value;
                term.EndDate = res[1].Value;
                return term;
            }
            else
            {
                var res = GetTermDatesAsync(DateTime.Now.Year - 1).Result[3];
                term.TermID = index;
                term.Description = "TERM 3";
                term.StartDate = res[0].Value;
                term.EndDate = res[1].Value;
                return term;
            }
        }

        internal static int CalculatePoints(decimal score)
        {
            return CalculatePoints(CalculateGrade(score));
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

        
        public static Task<ExamSettingsModel> GetExamSettingsAsync()
        {
            return Task.Factory.StartNew<ExamSettingsModel>(delegate
            {
                ExamSettingsModel settings = new ExamSettingsModel();
                string text = "SELECT [Key],Value,Value2 FROM [Settings] WHERE [Type]='ExamSettings'";
                DataTable dt = DataAccessHelper.Helper.ExecuteNonQueryWithResultTable(text);
                var tx = new List<IEnumerable<string>>();
                if (dt.Rows.Count == 0)
                    return new ExamSettingsModel();
                foreach (DataRow dtr in dt.Rows)
                {
                    List<string> rw = new List<string>();
                    foreach (var c in dtr.ItemArray)
                    {
                        rw.Add(c.GetType() == typeof(DBNull) ? "" : c.ToString());
                    }
                    tx.Add(rw);
                }
                var grs = tx.Where(o => o.Any(p => p.Contains("GradeRange")));
                var grrs = tx.Where(o => o.Any(p => p.Contains("GradeRemark")));
                var b7 = tx.First(o => o.Any(p => p.ToString().Equals("Best7Subjects"))).ToList();
                var mgc = tx.First(o => o.Any(p => p.ToString().Equals("MeanGradeCalculation"))).ToList();

                settings.GradeRanges.Clear();
                settings.GradeRemarks.Clear();
                List<string> row;
                List<string> row2;
                for (int i = 0; i < 12; i++)
                {
                    row = grs.First(o => o.Any(p => p.ToString().Equals("GradeRange" + i))).ToList();
                    row2 = grrs.First(o => o.Any(p => p.ToString().Equals("GradeRemark" + i))).ToList();
                    settings.GradeRanges.Add(new BasicPair<int, int>(int.Parse(row[1]), int.Parse(row[2])));
                    settings.GradeRemarks.Add(row2[1]);
                }

                settings.Best7Subjects = int.Parse(b7[1].ToString());
                settings.MeanGradeCalculation = int.Parse(mgc[1].ToString());
                return settings;
            });
        }

        

        public static Task<ObservableCollection<ClassModel>> GetAllClassesAsync()
        {
            return Task.Factory.StartNew(delegate
            {
                ObservableCollection<ClassModel> observableCollection = new ObservableCollection<ClassModel>();
                string commandText = "SELECT ClassID,NameOfClass FROM [Class]";
                DataTable dataTable = DataAccessHelper.Helper.ExecuteNonQueryWithResultTable(commandText);
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

        public static Task<ObservableCollection<CombinedClassModel>> GetAllCombinedClassesAsync()
        {
            return Task.Factory.StartNew(delegate
            {
                ObservableCollection<CombinedClassModel> observableCollection = new ObservableCollection<CombinedClassModel>();
                ObservableCollection<ClassModel> observableCollection2 = new ObservableCollection<ClassModel>();
                string commandText = "SELECT ClassID,NameOfClass FROM [Class]";
                DataTable dataTable = DataAccessHelper.Helper.ExecuteNonQueryWithResultTable(commandText);
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

        public static Task<bool> SaveAcademicYearAsync(AcademicYearModel newYear)
        {
            return Task.Factory.StartNew<bool>(delegate
            {
                List<SqlParameter> paramColl = new List<SqlParameter>();

                string insertStr = "BEGIN TRANSACTION\r\n";
                insertStr += "IF EXISTS(SELECT * FROM [Settings] WHERE [Type] ='AYSettings' AND [Key]=@syear)\r\n" +
                         "UPDATE [Settings] SET Value=@yer WHERE [Type] ='AYSettings' AND [Key]=@syear\r\n" +
                       "ELSE\r\nINSERT INTO [Settings] ([Type],[Key],Value) VALUES('AYSettings',@syear,@yer)\r\n";
                insertStr += "IF EXISTS(SELECT * FROM [Settings] WHERE [Type] ='TermSettings' AND [Key]=@syear)\r\n" +
                         "DELETE FROM [Settings] WHERE [Type] ='TermSettings' AND [Key]=@syear\r\n";
                int index = 1;
                foreach (var t in newYear.AllTerms)
                {
                    insertStr += "INSERT INTO [Settings] ([Type],[Key],Value,Value2,Value3,Value4) VALUES('TermSettings',@syear,@tid" + index + ",@desc" + index + ",@std" + index + ",@edd" + index + ")\r\n";

                    paramColl.Add(new SqlParameter("@tid" + index, t.TermID));
                    paramColl.Add(new SqlParameter("@desc" + index, t.Description));
                    paramColl.Add(new SqlParameter("@std" + index, t.StartDate.ToString("dd-MM-yyyy HH:mm:ss")));
                    paramColl.Add(new SqlParameter("@edd" + index, t.EndDate.ToString("dd-MM-yyyy HH:mm:ss")));
                    index++;
                }

                paramColl.Add(new SqlParameter("@syear", newYear.Description));
                paramColl.Add(new SqlParameter("@yer", newYear.Year));

                insertStr += "\r\nCOMMIT";
                bool succ = DataAccessHelper.Helper.ExecuteNonQuery(insertStr, paramColl);
                return succ;
            });
        }

        public async static Task<ObservableCollection<TermModel>> GetAllTermsAsync()
        {
            return (await GetAcademicYearAsync(DateTime.Now)).AllTerms;
        }

        public static Task<AcademicYearModel> GetAcademicYearAsync(DateTime dateTime)
        {
            return Task.Factory.StartNew<AcademicYearModel>(delegate
            {
                AcademicYearModel ay = new AcademicYearModel();
                string year = dateTime.Year.ToString();
                string selectStr = "SELECT Value,Value2 FROM [Settings] WHERE [Type] ='AYSettings' AND [Key]=@syear";
                List<SqlParameter> paramColl = new List<SqlParameter>();
                paramColl.Add(new SqlParameter("@syear", year));
                var dt = DataAccessHelper.Helper.ExecuteNonQueryWithResultTable(selectStr, paramColl);
                if (dt.Rows.Count == 0)
                    return GetDetfaultAY();

                ay.Description = dateTime.Year.ToString();
                ay.Year = dateTime.Year;
                selectStr = "SELECT Value,Value2,Value3,Value4 FROM [Settings] WHERE [Type] ='TermSettings' AND [Key]=@syear";
                var dt2 = DataAccessHelper.Helper.ExecuteNonQueryWithResultTable(selectStr, new List<SqlParameter>() { new SqlParameter("@syear", year) });
                foreach (DataRow dtr in dt2.Rows)
                {
                    ay.AllTerms.Add(new TermModel()
                    {
                        TermID = int.Parse(dtr[0].ToString()),
                        Description = dtr[1].ToString(),
                        StartDate = DateTime.Parse(dtr[2].ToString()),
                        EndDate = DateTime.Parse(dtr[3].ToString())
                    });
                }
                ay.NoOfTerms = dt2.Rows.Count;
                return ay;
            });
        }

        private static AcademicYearModel GetDetfaultAY()
        {
            AcademicYearModel ay = new AcademicYearModel();
            ay.NoOfTerms = 3;
            ay.AllTerms.Add(new TermModel() { Description = "TERM 1", TermID = 1, StartDate = new DateTime(DateTime.Now.Year, 1, 1), EndDate = new DateTime(DateTime.Now.Year, 4, 30) });
            ay.AllTerms.Add(new TermModel() { Description = "TERM 2", TermID = 2, StartDate = new DateTime(DateTime.Now.Year, 5, 1), EndDate = new DateTime(DateTime.Now.Year, 8, 31) });
            ay.AllTerms.Add(new TermModel() { Description = "TERM 3", TermID = 3, StartDate = new DateTime(DateTime.Now.Year, 9, 1), EndDate = new DateTime(DateTime.Now.Year, 12, 31) });
            return ay;
        }

        public static Task<ClassModel> GetClassAsync(int classID)
        {
            return Task.Factory.StartNew<ClassModel>(() => GetClass(classID));
        }

        public static ClassModel GetClass(int classID)
        {
            ClassModel classModel = new ClassModel();
            string commandText = "SELECT ClassID,NameOfClass FROM [Class] WHERE ClassID=" + classID;
            DataTable dataTable = DataAccessHelper.Helper.ExecuteNonQueryWithResultTable(commandText);
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

        public static Task<ObservableCollection<SubjectModel>> GetInstitutionSubjectsAsync()
        {
            return Task.Factory.StartNew<ObservableCollection<SubjectModel>>(delegate
            {
                ObservableCollection<SubjectModel> observableCollection = new ObservableCollection<SubjectModel>();
                string commandText = "SELECT SubjectID,NameOfSubject,Code,IsOptional FROM [Subject] ORDER BY Code";
                DataTable dataTable = DataAccessHelper.Helper.ExecuteNonQueryWithResultTable(commandText);
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

        public static Task<bool> SaveNewClassSetupAsync(ClassesSetupModel classSetup)
        {
            return Task.Factory.StartNew<bool>(delegate
            {
                string text = "BEGIN TRANSACTION\r\nDECLARE @id2 int\r\n";
                int index = 0;
                string text2 = "'0',";
                foreach (var c2 in classSetup.Entries)
                {
                    text2 = text2 + "@nam" + index+",";
                    index++;
                }
                text2 = text2.Remove(text2.Length - 1);
                index = 0;
                var paramColl = new List<SqlParameter>();
                foreach (ClassesSetupEntryModel current in classSetup.Entries)
                {
                    object obj = text;
                    
                    text = string.Concat(new object[]
                    {
                        obj,
                        "IF NOT EXISTS (SELECT * FROM [Class] WHERE ClassID=@cid",index," AND NameOfClass=@nam",index,
                        ")\r\nBEGIN\r\nSET @id2 = [dbo].GetNewID('dbo.Class')\r\n ",
                        "INSERT INTO [Class] (ClassID,NameOfClass) VALUES (@id2,@nam" ,index,")\r\nEND\r\n"
                    });
                    
                    paramColl.Add(new SqlParameter("@cid" + index, current.ClassID));
                    paramColl.Add(new SqlParameter("@nam" + index, current.NameOfClass));

                    index++;
                }
                text += "DELETE FROM [Class] WHERE NameOfClass NOT IN (" + text2+") COMMIT";
                return DataAccessHelper.Helper.ExecuteNonQuery(text,paramColl);
            });
        }

        public static Task<bool> SaveNewExamSettingsAsync(ExamSettingsModel settings)
        {
            return Task.Factory.StartNew<bool>(delegate
            {
                string text = "BEGIN TRANSACTION\r\n DELETE FROM [Settings] WHERE [Type]='ExamSettings'";
                for (int i = 0; i < settings.GradeRanges.Count; i++)
                    text += "INSERT INTO [Settings] ([Type],[Key],Value,Value2) VALUES ('ExamSettings','GradeRange" + i
                        + "','" + settings.GradeRanges[i].Key + "','" + settings.GradeRanges[i].Value + "')";
                for (int i = 0; i < settings.GradeRemarks.Count; i++)
                    text += "INSERT INTO [Settings] ([Type],[Key],Value) VALUES ('ExamSettings','GradeRemark" + i
                        + "','" + settings.GradeRemarks[i] + "')";
                text += "INSERT INTO [Settings] ([Type],[Key],Value) VALUES ('ExamSettings','Best7Subjects','" + settings.Best7Subjects + "')";
                text += "INSERT INTO [Settings] ([Type],[Key],Value) VALUES ('ExamSettings','MeanGradeCalculation','" + settings.MeanGradeCalculation + "')\r\n";

                text += " COMMIT";


                return DataAccessHelper.Helper.ExecuteNonQuery(text);
            });
        }

        public static Task<bool> SaveNewInstitutionSubjectSetup(IEnumerable<SubjectModel> selectedSubjects)
        {
            return Task.Factory.StartNew<bool>(delegate
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
                        "IF NOT EXISTS (SELECT * FROM[Subject] WHERE NameOfSubject='",
                        current.NameOfSubject,
                        "')INSERT INTO [Subject] (SubjectID, NameOfSubject, Code, MaximumScore, IsOptional) VALUES(dbo.GetNewID('dbo.Subject'), '",
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
                text = text + "DELETE FROM [Subject] WHERE NameOfSubject NOT IN (" + text2 + ")\r\n";
                text += "COMMIT";
                return DataAccessHelper.Helper.ExecuteNonQuery(text);
            });
        }


    }
}
