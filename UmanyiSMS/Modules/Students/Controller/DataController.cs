using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UmanyiSMS.Lib;
using UmanyiSMS.Lib.Controllers;
using UmanyiSMS.Modules.Institution.Models;
using UmanyiSMS.Modules.Students.Models;

namespace UmanyiSMS.Modules.Students.Controller
{
    public class DataController
    {
        public static Task<bool> SaveNewLeavingCertificateAsync(LeavingCertificateModel leavingCertificate)
        {
            return Task.Factory.StartNew<bool>(delegate
            {
                string commandText = string.Concat(new object[]
                {
                    "BEGIN TRANSACTION\r\nDELETE FROM [LeavingCertificate] WHERE StudentID=",
                    leavingCertificate.StudentID,
                    "\r\nINSERT INTO [LeavingCertificate] (LeavingCertificateID,StudentID,DateOfIssue,DateOfBirth,DateOfAdmission,DateOfLeaving,Nationality,ClassEntered,ClassLeft,Remarks) VALUES (dbo.GetNewID('dbo.LeavingCertificate'),",
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
                return DataAccessHelper.Helper.ExecuteNonQuery(commandText);
            });
        }


        public static Task<LeavingCertificateModel> GetStudentLeavingCert(StudentBaseModel student)
        {
            return Task.Factory.StartNew<LeavingCertificateModel>(delegate
            {
                LeavingCertificateModel leavingCertificateModel = new LeavingCertificateModel();
                try
                {
                    string commandText = "SELECT DateOfIssue,DateOfBirth,DateOfAdmission,DateOfLeaving,Nationality,ClassEntered,ClassLeft,Remarks FROM [LeavingCertificate] WHERE StudentID=" + student.StudentID;
                    DataTable dataTable = DataAccessHelper.Helper.ExecuteNonQueryWithResult(commandText);
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

        public static bool StudentIsCleared(int studentID)
        {
            string commandText = "IF EXISTS (SELECT StudentID FROM [StudentClearance] WHERE StudentID =" + studentID + ")\r\nSELECT 'true' ELSE SELECT 'false'";
            string value = DataAccessHelper.Helper.ExecuteScalar(commandText);
            return bool.Parse(value);
        }

        public static Task<bool> AssignStudentNewClass(int studentID, int newClassID, int prevYear, int newYear)
        {
            return Task.Factory.StartNew<bool>(delegate
            {
                string commandText = string.Concat(new object[]
                {
                    "IF NOT EXISTS (SELECT * FROM [StudentClass] WHERE StudentID=@sid AND [Year]=@yer)\r\n",
                    "INSERT INTO [StudentClass] (StudentID,ClassID,[Year]) VALUES(@sid,@cid,@yer)\r\n",
                    "ELSE UPDATE [StudentClass] SET ClassID = @cid WHERE StudentID=@sid AND [Year]=@yer"
            });
                var paramColl = new List<SqlParameter>();
                paramColl.Add(new SqlParameter("@sid", studentID));
                paramColl.Add(new SqlParameter("@cid", newClassID));
                paramColl.Add(new SqlParameter("@pyer", prevYear));
                paramColl.Add(new SqlParameter("@yer", newYear));
                return DataAccessHelper.Helper.ExecuteNonQuery(commandText, paramColl);
            });
        }

        public static Task<bool> AssignClassNewClass(int classID, int newClassID, int prevYear, int newYear)
        {
            return Task.Factory.StartNew<bool>(delegate
            {
                string commandText = "SELECT DISTINCT StudentID FROM [StudentClass] WHERE [Year]=@pyer AND ClassID =@cid";
                var pc = new List<SqlParameter>();
                pc.Add(new SqlParameter("@cid", newClassID));
                pc.Add(new SqlParameter("@pyer", prevYear));
                List<string> observableCollection = DataAccessHelper.Helper.CopyFirstColumnToList(commandText, pc);
                commandText = "";
                var paramColl = new List<SqlParameter>();

                paramColl.Add(new SqlParameter("@cid", newClassID));
                paramColl.Add(new SqlParameter("@pyer", prevYear));
                paramColl.Add(new SqlParameter("@yer", newYear));
                int index = 0;
                foreach (string current in observableCollection)
                {
                    paramColl.Add(new SqlParameter("@sid" + index, current));
                    commandText +=
                    "IF NOT EXISTS (SELECT * FROM [StudentClass] WHERE [Year]=@yer AND StudentID=@sid" + index + ")\r\n " +
                    "INSERT INTO [StudentClass] (StudentID,ClassID,[Year]) VALUES(@sid" +
                    index + ",@cid,@yer)\r\n" +
                    "ELSE UPDATE [StudentClass] SET ClassID = @cid WHERE StudentID=@sid" + index + " AND [Year]=@yer";
                    index++;
                }

                return DataAccessHelper.Helper.ExecuteNonQuery(commandText, paramColl);
            });
        }


        public static Task<int> GetClassIDFromStudentID(int selectedStudentID)
        {
            return Task.Factory.StartNew<int>(delegate
            {
                string commandText = string.Concat(new object[]
                {
                    "IF EXISTS(SELECT cs.ClassID FROM [Student]s LEFT OUTER JOIN [StudentClass] cs ON (s.StudentID=cs.StudentID AND cs.[Year]=DATEPART(year,sysdatetime())) WHERE s.StudentID = ",
                    selectedStudentID,
                    ")\r\nSELECT ISNULL(cs.ClassID,0) FROM [Student]s LEFT OUTER JOIN [StudentClass] cs ON (s.StudentID=cs.StudentID AND cs.[Year]=DATEPART(year,sysdatetime())) WHERE s.StudentID = ",
                    selectedStudentID,
                    "\r\nELSE SELECT 0"
                });
                string s = DataAccessHelper.Helper.ExecuteScalar(commandText);
                return int.Parse(s);
            });
        }

        public static Task<StudentSubjectSelectionModel> GetStudentSubjectSelection(int studentID)
        {
            return Task.Factory.StartNew<StudentSubjectSelectionModel>(delegate
            {
                StudentSubjectSelectionModel studentSubjectSelectionModel = new StudentSubjectSelectionModel();
                string commandText = "SELECT sub.NameOfSubject,sssd.SubjectID FROM [StudentSubjectSelectionDetail] sssd LEFT OUTER JOIN [StudentSubjectSelectionHeader] sssh ON(sssd.StudentSubjectSelectionID = sssh.StudentSubjectSelectionID) LEFT OUTER JOIN [Subject] sub ON(sssd.SubjectID=sub.SubjectID) WHERE sssh.[Year]=DATEPART(YEAR,sysdatetime()) AND sssh.StudentID=" + studentID + " ORDER BY sub.[Code]";
                DataTable dataTable = DataAccessHelper.Helper.ExecuteNonQueryWithResult(commandText);
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

        public static Task<StudentModel> GetStudentAsync(int studentID)
        {
            return Task.Factory.StartNew<StudentModel>(() => GetStudent(studentID));
        }

        public static StudentModel GetStudent(int studentID)
        {

            StudentModel studentModel = new StudentModel();
            string commandText = "SELECT FirstName,LastName,MiddleName,ISNULL(cs.ClassID,0),DateOfBirth,DateOfAdmission,NameOfGuardian,GuardianPhoneNo,Email,Address,City,PostalCode," +
                "PreviousInstitution,KCPEScore,SPhoto,PreviousBalance,Gender,s.IsActive FROM [Student] s LEFT OUTER JOIN " +
                "[StudentClass] cs ON (s.StudentID=cs.StudentID AND cs.[Year]=@acYear) WHERE s.StudentID=@sid";
            var paramColl = new List<SqlParameter>();
            paramColl.Add(new SqlParameter("@acYear", DateTime.Now.Year));
            paramColl.Add(new SqlParameter("@sid", studentID));
            DataTable dataTable = DataAccessHelper.Helper.ExecuteNonQueryWithResultTable(commandText, paramColl);
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
                studentModel.SPhoto = (byte[])dataTable.Rows[0][14];
                studentModel.PrevBalance = decimal.Parse(dataTable.Rows[0][15].ToString());
                studentModel.Gender = (Gender)Enum.Parse(typeof(Gender), dataTable.Rows[0][16].ToString());
                studentModel.IsActive = bool.Parse(dataTable.Rows[0][17].ToString());
            }
            return studentModel;
        }

        public static Task<ClassStudentListModel> GetClassStudentListAsync(ClassModel selectedClass)
        {
            return Task.Factory.StartNew<ClassStudentListModel>(delegate
            {
                ClassStudentListModel classStudentListModel = new ClassStudentListModel();
                classStudentListModel.ClassID = selectedClass.ClassID;
                classStudentListModel.NameOfClass = selectedClass.NameOfClass;
                string commandText = "SELECT s.StudentID,FirstName,LastName,MiddleName FROM [Student] s " +
                "LEFT OUTER JOIN [StudentClass] cs ON (s.StudentID=cs.StudentID AND cs.[Year]=DATEPART(year,sysdatetime())) WHERE cs.ClassID=@cid AND s.IsActive=1";
                var paramColl = new List<SqlParameter>();
                paramColl.Add(new SqlParameter("@cid", selectedClass.ClassID));
                DataTable dataTable = DataAccessHelper.Helper.ExecuteNonQueryWithResultTable(commandText, paramColl);
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
            return Task.Factory.StartNew<ClassStudentListModel>(delegate
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
                string commandText = "SELECT s.StudentID,FirstName+' '+LastName+' '+MiddleName FROM [Student]s " +
                "LEFT OUTER JOIN [StudentClass] cs ON (s.StudentID=cs.StudentID AND cs.[Year]=DATEPART(year,sysdatetime()))" +
                "WHERE cs.ClassID IN (" + text + ") AND s.IsActive=1";
                DataTable dataTable = DataAccessHelper.Helper.ExecuteNonQueryWithResult(commandText);
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

        public static Task<bool> UpdateStudentAsync(StudentModel student)
        {
            return Task.Factory.StartNew<bool>(delegate
            {
                string commandText = string.Concat(new object[]
                {
                    "UPDATE [Student] SET FirstName='",
                    student.FirstName,
                    "', LastName='",
                    student.LastName,
                    "', MiddleName='",
                    student.MiddleName,
                    "', Gender='",
                    student.Gender.ToString(),
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
                    
                    ", PreviousBalance='",
                    student.PrevBalance,
                    "', SPhoto=@photo WHERE StudentID=",
                    student.StudentID
                });
                return DataAccessHelper.Helper.ExecuteNonQuery(commandText, new ObservableCollection<SqlParameter>
                {
                    new SqlParameter("@photo", student.SPhoto)
                });
            });
        }

        public static Task<bool> SetStudentActiveAsync(int studentID)
        {
            return Task.Factory.StartNew<bool>(delegate
            {
                string text = "BEGIN TRANSACTION\r\n"+
                "DELETE FROM [StudentClearance] WHERE StudentID=@sid\r\nCOMMIT";
                var paramColl = new List<SqlParameter>() { new SqlParameter("@sid",studentID)};
                return DataAccessHelper.Helper.ExecuteNonQuery(text,paramColl);
            });
        }

        public static Task<bool> SaveNewStudentAsync(StudentModel student)
        {
            return Task.Factory.StartNew<bool>(delegate
            {
                bool flag = student.StudentID == 0;
                string text = "BEGIN TRANSACTION\r\nDECLARE @id INT; SET @id=dbo.GetNewID('dbo.Student')INSERT INTO [Student] (StudentID,FirstName,LastName,MiddleName,Gender,DateOfBirth,DateOfAdmission,NameOfGuardian,GuardianPhoneNo,Email,Address,City,PostalCode";
                
                string text2 = text;
                text = string.Concat(new string[]
                {
                    text2,
                    ",PreviousInstitution,KCPEScore,PreviousBalance,SPhoto) VALUES(",
                    flag ? "@id" : "@studID",
                    ",@firstName,@lastName,@middleName,@gender,@dob,@doa,@nameOfGuardian,@guardianPhoneNo,@email,@address,@city,@postalCode,",
                    "@prevInstitution,@kcpeScore,@prevBalance,@photo)\r\nINSERT INTO [StudentClass] (StudentID,ClassID,Year) " +
                "VALUES(" + (flag ? "@id" : "@studID") + ",@classID,@yer)\r\nCOMMIT"
                });
                return DataAccessHelper.Helper.ExecuteNonQuery(text, new ObservableCollection<SqlParameter>
                {
                    new SqlParameter("@studID", student.StudentID),
                    new SqlParameter("@firstName", student.FirstName),
                    new SqlParameter("@middleName", student.MiddleName),
                    new SqlParameter("@lastName", student.LastName),
                    new SqlParameter("@gender", student.Gender.ToString()),
                    new SqlParameter("@dob", student.DateOfBirth.ToString("g")),
                    new SqlParameter("@doa", student.DateOfAdmission),
                    new SqlParameter("@nameOfGuardian", student.NameOfGuardian),
                    new SqlParameter("@guardianPhoneNo", student.GuardianPhoneNo),
                    new SqlParameter("@email", student.Email),
                    new SqlParameter("@address", student.Address),
                    new SqlParameter("@city", student.City),
                    new SqlParameter("@postalCode", student.PostalCode),
                    new SqlParameter("@prevInstitution", student.PrevInstitution),
                    new SqlParameter("@kcpeScore", student.KCPEScore),
                    new SqlParameter("@prevBalance", student.PrevBalance),
                    new SqlParameter("@photo", student.SPhoto),
                    new SqlParameter("@classID", student.ClassID),
                    new SqlParameter("@yer", DateTime.Now.Year)
                });
            });
        }

        public static Task<bool> SaveNewStudentClearanceAsync(StudentClearanceModel student)
        {
            return Task.Factory.StartNew<bool>(delegate
            {
                try
                {
                    string text = "BEGIN TRANSACTION\r\n" +
                            "INSERT INTO [StudentClearance] (StudentID,DateCleared) VALUES (" +
                            student.StudentID + ",'" + student.DateCleared.ToString("g") + "')\r\n" + " COMMIT";
                    var succ = DataAccessHelper.Helper.ExecuteNonQuery(text);
                }
                catch { return false; }
                return true;
            });
        }

        public static Task<ObservableCollection<StudentListModel>> GetAllStudentsListAsync()
        {
            return Task.Factory.StartNew<ObservableCollection<StudentListModel>>(delegate
            {
                string commandText = "SELECT s.StudentID,s.FirstName,s.LastName,s.MiddleName,cs.ClassID, c.NameOfClass,s.DateOfBirth," +
                "s.DateOfAdmission,s.NameOfGuardian,s.GuardianPhoneNo,s.Address,s.City,s.PostalCode,s.PreviousInstitution,s.KCPEScore, " +
                "s.PreviousBalance, s.IsActive,s.Gender, s.SPhoto FROM [Student] s LEFT OUTER JOIN [StudentClass] cs" +
                " ON (s.StudentID = cs.StudentID AND cs.[Year]=DATEPART(year,SYSDATETIME()))LEFT OUTER JOIN [Class] c ON(cs.ClassID=c.ClassID)";
                ObservableCollection<StudentListModel> observableCollection = new ObservableCollection<StudentListModel>();
                DataTable dataTable =null;
                
				try {
					dataTable = DataAccessHelper.Helper.ExecuteNonQueryWithResult(commandText);
				} 
                catch {
					return new ObservableCollection<StudentListModel>();
				}
                
                if (dataTable.Rows.Count != 0)
                {
                    foreach (DataRow dataRow in dataTable.Rows)
                    {

                        StudentListModel stud = new StudentListModel();
                        stud.StudentID = int.Parse(dataRow[0].ToString());
                        stud.FirstName = dataRow[1].ToString();
                        stud.LastName = dataRow[2].ToString();
                        stud.MiddleName = dataRow[3].ToString();
                        stud.ClassID = string.IsNullOrWhiteSpace(dataRow[4].ToString()) ? 0 : int.Parse(dataRow[4].ToString());
                        stud.NameOfClass = dataRow[5].ToString();
                        stud.DateOfBirth = DateTime.Parse(dataRow[6].ToString());
                        stud.DateOfAdmission = DateTime.Parse(dataRow[7].ToString());
                        stud.NameOfGuardian = dataRow[8].ToString();
                        stud.GuardianPhoneNo = dataRow[9].ToString();
                        stud.Address = dataRow[10].ToString();
                        stud.City = dataRow[11].ToString();
                        stud.PostalCode = dataRow[12].ToString();
                        stud.PrevInstitution = dataRow[13].ToString();
                        stud.KCPEScore = string.IsNullOrWhiteSpace(dataRow[14].ToString()) ? 0 : int.Parse(dataRow[14].ToString());
                        stud.PrevBalance = decimal.Parse(dataRow[15].ToString());
                        stud.IsActive = bool.Parse(dataRow[16].ToString());
                        stud.Gender = (Gender)Enum.Parse(typeof(Gender), dataRow[17].ToString());
                        stud.SPhoto = (byte[])dataRow[18];
                        observableCollection.Add(stud);
                    }
                }
                return observableCollection;
            });
        }

        public static bool SearchAllStudentProperties(StudentModel student, string searchText)
        {
            Regex.CacheSize = 14;
            return Regex.Match(student.StudentID.ToString(), searchText, RegexOptions.IgnoreCase).Success || Regex.Match(student.FirstName, searchText, RegexOptions.IgnoreCase).Success || Regex.Match(student.LastName, searchText, RegexOptions.IgnoreCase).Success || Regex.Match(student.MiddleName, searchText, RegexOptions.IgnoreCase).Success || Regex.Match(student.NameOfGuardian, searchText, RegexOptions.IgnoreCase).Success || Regex.Match(student.Address, searchText, RegexOptions.IgnoreCase).Success || Regex.Match(student.City, searchText, RegexOptions.IgnoreCase).Success || Regex.Match(student.ClassID.ToString(), searchText, RegexOptions.IgnoreCase).Success || Regex.Match(student.Email, searchText, RegexOptions.IgnoreCase).Success || Regex.Match(student.NameOfStudent, searchText, RegexOptions.IgnoreCase).Success || Regex.Match(student.PostalCode, searchText, RegexOptions.IgnoreCase).Success || Regex.Match(student.PrevInstitution, searchText, RegexOptions.IgnoreCase).Success;
        }

        public static Task<ObservableCollection<StudentSubjectSelectionModel>> GetClassStudentSubjectSelection(int classID)
        {
            return Task.Factory.StartNew<ObservableCollection<StudentSubjectSelectionModel>>(delegate
            {
                ObservableCollection<StudentSubjectSelectionModel> observableCollection = new ObservableCollection<StudentSubjectSelectionModel>();
                ObservableCollection<SubjectModel> result = Institution.Controller.DataController.GetInstitutionSubjectsAsync().Result;
                string text = "SELECT s.StudentID, s.NameOfStudent,";
                foreach (SubjectModel current in result)
                {
                    object obj = text;
                    text = string.Concat(new object[]
                    {
                        obj,
                        "dbo.GetSubjectSelection(",
                        current.SubjectID,
                        ",s.StudentID),"
                    });
                }
                text = text.Remove(text.Length - 1);
                text = text + " FROM [Student]s LEFT OUTER JOIN [StudentClass] cs ON (s.StudentID=cs.StudentID AND cs.[Year]=DATEPART(year,sysdatetime())) WHERE s.IsActive=1 AND cs.ClassID=" + classID;
                DataTable dataTable = DataAccessHelper.Helper.ExecuteNonQueryWithResult(text);
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

        public static Task<bool> SaveNewSubjectSelection(ObservableCollection<StudentSubjectSelectionModel> subjectSelections)
        {
            return Task.Factory.StartNew<bool>(delegate
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
                    text += "SET @id = dbo.GetNewID('dbo.StudentSubjectSelectionHeader')\r\n";
                    object obj = text;
                    text = string.Concat(new object[]
                    {
                        obj,
                        "IF NOT EXISTS (SELECT * FROM [StudentSubjectSelectionHeader] WHERE [Year]=",current.Year,
                        " AND StudentID=",
                        current.StudentID,
                        ")\r\n"
                    });
                    obj = text;
                    text = string.Concat(new object[]
                    {
                        obj,
                        "INSERT INTO [StudentSubjectSelectionHeader] (StudentSubjectSelectionID,StudentID,[Year]) VALUES (@id,",
                        current.StudentID,
                        ",",current.Year,")\r\n"
                    });
                    obj = text;
                    text = string.Concat(new object[]
                    {
                        obj,
                        "ELSE SET @id=(SELECT StudentSubjectSelectionID FROM [StudentSubjectSelectionHeader] WHERE StudentID=",
                        current.StudentID,
                        " AND [Year]=",current.Year,")\r\nDELETE FROM [StudentSubjectSelectionDetail] WHERE StudentSubjectSelectionID=@id\r\n"
                    });
                    foreach (StudentSubjectSelectionEntryModel current3 in current.Entries)
                    {
                        obj = text;

                        text = string.Concat(new object[]
                        {
                            obj,
                            "INSERT INTO [StudentSubjectSelectionDetail] (StudentSubjectSelectionID,SubjectID) VALUES (@id,",
                            current3.SubjectID,
                            ")\r\n"
                        });
                    }
                    text += " COMMIT";
                    flag = (flag && DataAccessHelper.Helper.ExecuteNonQuery(text));
                }
                return flag;
            });
        }

        public static Task<bool> SaveNewSubjectSelection(StudentSubjectSelectionModel subjectSelection)
        {
            return Task.Factory.StartNew<bool>(delegate
            {
                string text = "BEGIN TRANSACTION\r\nDECLARE @id int; \r\n ";
                string text2 = "0,";
                foreach (StudentSubjectSelectionEntryModel current in subjectSelection.Entries)
                {
                    text2 = text2 + current.SubjectID + ",";
                }
                text2 = text2.Remove(text2.Length - 1);
                text += "SET @id = dbo.GetNewID('dbo.StudentSubjectSelectionHeader')\r\n";
                object obj = text;
                text = string.Concat(new object[]
                {
                    obj,
                    "IF NOT EXISTS (SELECT * FROM [StudentSubjectSelectionHeader] WHERE Year=",subjectSelection.Year," AND StudentID=",
                    subjectSelection.StudentID,
                    ")\r\n"
                });
                obj = text;
                text = string.Concat(new object[]
                {
                    obj,
                    "INSERT INTO [StudentSubjectSelectionHeader] (StudentSubjectSelectionID,StudentID,[Year]) VALUES (@id,",
                    subjectSelection.StudentID,
                    ",",subjectSelection.Year,")\r\n"
                });
                obj = text;
                text = string.Concat(new object[]
                {
                    obj,
                    "ELSE SET @id=(SELECT StudentSubjectSelectionID FROM [StudentSubjectSelectionHeader] WHERE StudentID=",
                    subjectSelection.StudentID,
                    " AND Year=",subjectSelection.Year,")\r\n"
                });
                text += "DELETE FROM[StudentSubjectSelectionDetail] WHERE StudentSubjectSelectionID = @id\r\n";
                foreach (StudentSubjectSelectionEntryModel current2 in subjectSelection.Entries)
                {
                    obj = text;
                    text = string.Concat(new object[]
                    {
                        obj,
                        "INSERT INTO [StudentSubjectSelectionDetail] (StudentSubjectSelectionID,SubjectID) VALUES (@id,",
                        current2.SubjectID,
                        ")\r\n"
                    });
                }

                text += " COMMIT";
                return DataAccessHelper.Helper.ExecuteNonQuery(text);
            });
        }


    }
}
