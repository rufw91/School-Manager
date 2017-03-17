using Helper.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Media;
using UmanyiSMS.Lib.Controllers;
using UmanyiSMS.Modules.Exams.Models;

namespace UmanyiSMS.Modules.Exams.Controller
{
    public class DocumentHelper:DocumentHelperBase
    {
        private DocumentHelper(object workObject)
        {
            if (workObject == null)
                throw new ArgumentNullException("workObject", "workObject cannot be null.");
            InitVars(myWorkObject);
            AddPagesToDocument(noOfPages,ResourceString);
            AddDataToDocument();
        }

        public static FixedDocument GenerateDocument(object workObject)
        {
            new DocumentHelper(workObject);
            return doc;
        }

        private void InitVars(object workObject)
        {
            myWorkObject = workObject;
            noOfPages = GetNoOfPages(workObject);
            ResourceString = GetResString(workObject);
            doc = new FixedDocument();
        }
        
        protected void AddDataToDocument()
        {
            if (myWorkObject is StudentTranscriptModel)
                GenerateTranscript();
            if (myWorkObject is StudentTranscriptModel2)
                GenerateTranscript3();
            //if (workObject is ReportFormModel)
                //return 1;
            if (myWorkObject is ClassReportFormModel)
                GenerateClassTranscripts();
            if (myWorkObject is ClassTranscriptsModel2)
                GenerateClassTranscripts();
            if (myWorkObject is ClassTranscriptsModel)
                GenerateClassTranscripts();
            if (myWorkObject is ClassStudentsExamResultModel)
                GenerateClassExamResults();
            if (myWorkObject is ClassExamResultModel)
                GenerateClassMarkList();

            
                
             throw new ArgumentException();
            
        }

        protected string GetResString(object docType)
        {
            string resString = "";
            switch (docType)
            {
                case DocType.Transcript: resString = Helper.Properties.Resources.Transcript; break;
                case DocType.Transcript2: resString = Helper.Properties.Resources.Transcript2; break;
                case DocType.Transcript3: resString = Helper.Properties.Resources.Transcript4; break;
                case DocType.ClassTranscripts: resString = Helper.Properties.Resources.Transcript2; break;
                case DocType.ClassTranscripts2: resString = Helper.Properties.Resources.Transcript4; break;
                case DocType.ClassExamResults: resString = Helper.Properties.Resources.Transcript; break;
                case DocType.ClassMarkList: resString = Helper.Properties.Resources.ClassMarkList; break;
                case DocType.AggregateResult: resString = Helper.Properties.Resources.AggregateResult; break;
                default: throw new ArgumentException();
            }
            return resString;
        }
        
        private static int GetNoOfPages(object workObject)
        {
            if (workObject is StudentTranscriptModel)
                return 1;
            if (workObject is StudentTranscriptModel2)
                return 1;
            if (workObject is ReportFormModel)
                return 1;
            if (workObject is ClassReportFormModel)
                return (workObject as ClassReportFormModel).Count;
            if (workObject is ClassTranscriptsModel2)
                return (workObject as ClassTranscriptsModel2).Entries.Count;
            if (workObject is ClassTranscriptsModel)
                return (workObject as ClassTranscriptsModel).Entries.Count;
            if (workObject is ClassStudentsExamResultModel)
                return (workObject as ClassStudentsExamResultModel).Entries.Count;

            return 0;
        }


        #region Aggregate Result

        private void AddAGClass(string nameOfClass, int pageNo)
        {
            AddText(nameOfClass, "Arial", 14, true, 0, Colors.Black, 100, 85, pageNo);
        }
        private void AddAGDate(DateTime dateTime, int pageNo)
        {
            AddText(dateTime.ToString("dd-MMM-yyyy"), "Arial", 14, true, 0, Colors.Black, 650, 65, pageNo);
        }
        private void AddAGExam(string nameOfExam, int pageNo)
        {
            AddText(nameOfExam, "Arial", 14, true, 0, Colors.Black, 100, 120, pageNo);
        }
        private void AddAGMeanScore(decimal meanScore, int pageNo)
        {
            AddText(meanScore.ToString("N4"), "Arial", 14, true, 0, Colors.Black, 180, 165, pageNo);
        }
        private void AddAGMeanGrade(string meanGrade, int pageNo)
        {
            AddText(meanGrade, "Arial", 14, true, 0, Colors.Black, 160, 210, pageNo);
        }
        private void AddAGPoints(int points, int pageNo)
        {
            AddText(points.ToString(), "Arial", 14, true, 0, Colors.Black, 110, 255, pageNo);
        }
        private void AddAGEntry(AggregateResultEntryModel item, int itemIndex, int pageNo)
        {
            double fontsize = 14;
            int pageRelativeIndex = itemIndex - itemsPerPage * pageNo;
            double yPos = 355 + pageRelativeIndex * 25;

            AddText(item.NameOfSubject, "Arial", 14, false, 0, Colors.Black, 45, yPos, pageNo);
            AddText(item.MeanScore.ToString("N4"), "Arial", fontsize, false, 0, Colors.Black, 285, yPos, pageNo);
            AddText(item.MeanGrade, "Arial", fontsize, false, 0, Colors.Black, 475, yPos, pageNo);
            AddText(item.Points.ToString(), "Arial", fontsize, false, 0, Colors.Black, 630, yPos, pageNo);
        }
        private void AddAGEntries(ObservableCollection<AggregateResultEntryModel> psi, int pageNo)
        {

            int startIndex = pageNo * itemsPerPage;
            int endIndex = startIndex + itemsPerPage - 1;
            if (startIndex >= psi.Count)
                return;
            if (endIndex >= psi.Count)
                endIndex = psi.Count - 1;

            for (int i = startIndex; i <= endIndex; i++)
                AddAGEntry(psi[i], i, pageNo);
        }

        private void GenerateAggregateResult()
        {
            AggregateResultModel si = myWorkObject as AggregateResultModel;

            int pageNo;
            for (pageNo = 0; pageNo < noOfPages; pageNo++)
            {
                AddAGClass(si.NameOfClass, pageNo);
                AddAGDate(DateTime.Now, pageNo);
                AddAGExam(si.NameOfExam, pageNo);
                AddAGMeanScore(si.MeanScore, pageNo);
                AddAGMeanGrade(si.MeanGrade, pageNo);
                AddAGPoints(si.Points, pageNo);
                AddAGEntries(si.Entries, pageNo);
            }
        }
        #endregion

        #region Class Mark List

        private void AddCMLClass(string nameOfClass, int pageNo)
        {
            AddText(nameOfClass, "Arial", 14, true, 0, Colors.Black, 70, 70, pageNo);
        }
        private void AddCMLExam(string nameOfExam, int pageNo)
        {
            AddText(nameOfExam, "Arial", 14, true, 0, Colors.Black, 350, 75, pageNo);
        }
        private void AddCMLSubjects(DataTable entries, int pageNo)
        {
            double xPos = 255;
            double count = 0;
            if (entries.Columns.Count < 2)
                return;
            for (int i = 2; i < entries.Columns.Count - 3; i++)
            {
                AddText(entries.Columns[i].ColumnName.ToUpper().Length < 3 ? entries.Columns[i].ColumnName.ToUpper() :
                    entries.Columns[i].ColumnName.ToUpper().Substring(0, 3), "Times New Roman", 14, true, 0, Colors.Black,
                    xPos + count * 35, 110, pageNo);
                count++;
            }
        }
        private void AddCMLStudent(DataRow item, int itemIndex, int pageNo)
        {
            int pageRelativeIndex = itemIndex - itemsPerPage * pageNo;
            double yPos = 140 + pageRelativeIndex * 26;
            double xPos = 255;
            double count = 0;
            AddText(item["Student ID"].ToString(), "Times New Roman", 12, false, 0, Colors.Black, 16, yPos, pageNo);
            AddText(item["Name"].ToString().Length > 25 ? item["Name"].ToString().Substring(0, 25) : item["Name"].ToString()
                , "Times New Roman", 12, false, 0, Colors.Black, 50, yPos, pageNo);
            for (int i = 2; i < item.ItemArray.Length - 3; i++)
            {
                AddText(item[i].ToString(), "Times New Roman", 12, false, 0, Colors.Black, xPos + count * 35, yPos, pageNo);
                count++;
            }
            AddText(item["Grade"].ToString(), "Times New Roman", 12, false, 0, Colors.Black, 680, yPos, pageNo);
            AddText(item["Total"].ToString(), "Times New Roman", 12, false, 0, Colors.Black, 715, yPos, pageNo);
            AddText(item["Position"].ToString(), "Times New Roman", 12, false, 0, Colors.Black, 754, yPos, pageNo);
        }
        private void AddCMLStudents(DataTable psi, int pageNo)
        {
            int startIndex = pageNo * itemsPerPage;
            int endIndex = startIndex + itemsPerPage - 1;
            if (startIndex >= psi.Rows.Count)
                return;
            if (endIndex >= psi.Rows.Count)
                endIndex = psi.Rows.Count - 1;

            for (int i = startIndex; i <= endIndex; i++)
                AddCMLStudent(psi.Rows[i], i, pageNo);
        }

        private void GenerateClassMarkList()
        {
            ClassExamResultModel si = myWorkObject as ClassExamResultModel;

            int pageNo;
            for (pageNo = 0; pageNo < noOfPages; pageNo++)
            {
                AddCMLExam(si.NameOfExam, pageNo);
                AddCMLClass(si.NameOfClass, pageNo);
                AddCMLSubjects(si.Entries, pageNo);
                AddCMLStudents(si.Entries, pageNo);
            }
        }
        #endregion

        #region ClassExamResults
        private void GenerateClassExamResults()
        {
            ClassStudentsExamResultModel si = myWorkObject as ClassStudentsExamResultModel;

            int pageNo;
            for (pageNo = 0; pageNo < noOfPages; pageNo++)
            {
                AddTRStudentID(si.Entries[pageNo].StudentID, pageNo);
                AddTRName(si.Entries[pageNo].NameOfStudent, pageNo);
                AddTRClassName(si.Entries[pageNo].NameOfClass, pageNo);
                AddTRExamName(si.Entries[pageNo].NameOfExam, pageNo);
                AddTRClassPosition(si.Entries[pageNo].ClassPosition, pageNo);
                AddTRPointsPosition(si.Entries[pageNo].Points, pageNo);
                AddTRTotalMarks(si.Entries[pageNo].TotalMarks, pageNo);
                AddTRMeanGrade(si.Entries[pageNo].MeanGrade, pageNo);
                AddTRSubjectScores(si.Entries[pageNo].Entries, pageNo);
            }
        }
        #endregion

        #region ClassTranscripts
        private void GenerateClassTranscripts()
        {
            ClassTranscriptsModel si = myWorkObject as ClassTranscriptsModel;

            int pageNo;
            for (pageNo = 0; pageNo < noOfPages; pageNo++)
            {
                si.Entries[pageNo].Entries = new ObservableCollection<StudentExamResultEntryModel>(si.Entries[pageNo].Entries.OrderBy(o => o.Code));
                AddTR2StudentID(si.Entries[pageNo].StudentID, pageNo);
                AddTR2Name(si.Entries[pageNo].NameOfStudent, pageNo);
                AddTR2ClassName(si.Entries[pageNo].NameOfClass, pageNo);
                AddTR2ClassPosition(si.Entries[pageNo].ClassPosition, pageNo);
                AddTR2KCPEScore(si.Entries[pageNo].KCPEScore, pageNo);
                AddTR2OverAllPosition(si.Entries[pageNo].OverAllPosition, pageNo);
                AddTR2TotalMarks(si.Entries[pageNo].TotalMarks, pageNo);
                AddTR2MeanGrade(si.Entries[pageNo].MeanGrade, pageNo);
                AddTR2SubjectScores(si.Entries[pageNo].Entries, pageNo);
                AddTR2Boarding(si.Entries[pageNo].Boarding, pageNo);
                AddTR2Responsibilities(si.Entries[pageNo].Responsibilities, pageNo);
                AddTR2Clubs(si.Entries[pageNo].ClubsAndSport, pageNo);
                AddTR2Principal(si.Entries[pageNo].Principal, pageNo);
                AddTR2PrincipalComments(si.Entries[pageNo].PrincipalComments, pageNo);
                AddTR2ClassTR(si.Entries[pageNo].ClassTeacher, pageNo);
                AddTR2Opening(si.Entries[pageNo].OpeningDay, pageNo);
                AddTR2Closing(si.Entries[pageNo].ClosingDay, pageNo);
                AddTR2ClustPoints(si.Entries[pageNo].Points, pageNo);
                AddTR2ClassTRComments(si.Entries[pageNo].ClassTeacherComments, pageNo);
                TR2DrawGraph(DataController.CalculateGrade(decimal.Ceiling(Convert.ToDecimal(si.Entries[pageNo].KCPEScore) / 5m)),
                   si.Entries[pageNo].CAT1Grade,
                   si.Entries[pageNo].CAT2Grade,
                   si.Entries[pageNo].ExamGrade,
                   pageNo);
            }
        }
        #endregion

        #region ClassTranscripts2
        private void GenerateClassTranscripts2()
        {
            ClassTranscriptsModel2 si = myWorkObject as ClassTranscriptsModel2;
            int pageNo;
            for (pageNo = 0; pageNo < noOfPages; pageNo++)
            {
                si.Entries[pageNo].Entries = new ObservableCollection<StudentExamResultEntryModel>(si.Entries[pageNo].Entries.OrderBy(o => o.Code));
                AddTR3StudentID(si.Entries[pageNo].StudentID, pageNo);
                AddTR3Name(si.Entries[pageNo].NameOfStudent, pageNo);
                AddTR3ClassName(si.Entries[pageNo].NameOfClass, pageNo);
                AddTR3KCPEScore(si.Entries[pageNo].KCPEScore, pageNo);

                AddTR3SubjectScores(si.Entries[pageNo].Entries, pageNo);

                AddTR3TotalMarks(si.Entries[pageNo].TotalMarks, pageNo);
                AddTR3OutOf(si.Entries[pageNo].Entries.Count * 100, pageNo);
                if (si.Entries[pageNo].Term1AvgPts > 0)
                {
                    AddTR3Term1AvgPts(si.Entries[pageNo].Term1AvgPts, pageNo);
                    AddTR3Term1TotalScore(si.Entries[pageNo].Term1TotalScore, pageNo);
                    AddTR3Term1PtsChange(si.Entries[pageNo].Term1PtsChange, pageNo);
                    AddTR3Term1TotalPts(si.Entries[pageNo].Term1TotalPoints, pageNo);
                    AddTR3Term1Score(si.Entries[pageNo].Term1MeanScore, pageNo);
                    AddTR3Term1Grade(si.Entries[pageNo].Term1Grade, pageNo);
                    AddTR3Term1ClassPOS(si.Entries[pageNo].Term1Pos, pageNo);
                    AddTR3Term1CombinedClassPOS(si.Entries[pageNo].Term1OverallPos, pageNo);

                }
                if (si.Entries[pageNo].Term2AvgPts > 0)
                {
                    AddTR3Term2TotalScore(si.Entries[pageNo].Term2TotalScore, pageNo);
                    AddTR3Term2AvgPts(si.Entries[pageNo].Term2AvgPts, pageNo);
                    AddTR3Term2PtsChange(si.Entries[pageNo].Term2PtsChange, pageNo);
                    AddTR3Term2TotalPts(si.Entries[pageNo].Term2TotalPoints, pageNo);
                    AddTR3Term2Score(si.Entries[pageNo].Term2MeanScore, pageNo);
                    AddTR3Term2Grade(si.Entries[pageNo].Term2Grade, pageNo);
                    AddTR3Term2ClassPOS(si.Entries[pageNo].Term2Pos, pageNo);
                    AddTR3Term2CombinedClassPOS(si.Entries[pageNo].Term2OverallPos, pageNo);
                }
                if (si.Entries[pageNo].Term3AvgPts > 0)
                {
                    AddTR3Term3TotalScore(si.Entries[pageNo].Term3TotalScore, pageNo);
                    AddTR3Term3AvgPts(si.Entries[pageNo].Term3AvgPts, pageNo);
                    AddTR3Term3PtsChange(si.Entries[pageNo].Term3PtsChange, pageNo);
                    AddTR3Term3TotalPts(si.Entries[pageNo].Term3TotalPoints, pageNo);
                    AddTR3Term3Score(si.Entries[pageNo].Term3MeanScore, pageNo);
                    AddTR3Term3Grade(si.Entries[pageNo].Term3Grade, pageNo);
                    AddTR3Term3ClassPOS(si.Entries[pageNo].Term3Pos, pageNo);
                    AddTR3Term3CombinedClassPOS(si.Entries[pageNo].Term3OverallPos, pageNo);
                }



                AddTR3ClassTRComments(si.Entries[pageNo].ClassTeacherComments, pageNo);
                AddTR3PrincipalComments(si.Entries[pageNo].PrincipalComments, pageNo);

                AddTR3Opening(si.Entries[pageNo].OpeningDay, pageNo);
                AddTR3Closing(si.Entries[pageNo].ClosingDay, pageNo);
            }
        }
        #endregion

        #region Transcript

        private void AddTRStudentID(int studentID, int pageNo)
        {
            AddText(studentID.ToString(), "Arial", 14, false, 0, Colors.Black, 95, 170, pageNo);
        }
        private void AddTRName(string nameOfStudent, int pageNo)
        {
            AddText(nameOfStudent, "Arial", 14, false, 0, Colors.Black, 247, 170, pageNo);
        }
        private void AddTRClassName(string className, int pageNo)
        {
            AddText(className, "Arial", 14, true, 0, Colors.Black, 619, 170, pageNo);
        }
        private void AddTRClassPosition(string classPosition, int pageNo)
        {
            AddText(classPosition, "Arial", 14, true, 0, Colors.Black, 75, 580, pageNo);
        }

        private void AddTRTotalMarks(decimal totalMarks, int pageNo)
        {
            AddText(totalMarks.ToString(), "Arial", 14, true, 0, Colors.Black, 250, 580, pageNo);
        }
        private void AddTRPointsPosition(decimal points, int pageNo)
        {
            AddText(points.ToString("N0"), "Arial", 14, true, 0, Colors.Black, 570, 580, pageNo);
        }
        private void AddTRMeanGrade(string meanGrade, int pageNo)
        {
            AddText(meanGrade, "Arial", 14, true, 0, Colors.Black, 410, 580, pageNo);
        }
        private void AddTRExamName(string nameOfExam, int pageNo)
        {
            AddText(nameOfExam, "Arial", 14, true, 0, Colors.Black, 350, 145, pageNo);
        }
        private void AddTRSubjectScore(StudentTranscriptSubjectModel item, int itemIndex, int pageNo)
        {
            double fontsize = 14;
            int pageRelativeIndex = itemIndex;
            double yPos = 255 + pageRelativeIndex * 21;

            AddText(item.Code, "Arial", 14, false, 0, Colors.Black, 40, yPos, pageNo);
            AddText(item.NameOfSubject, "Arial", 14, false, 0, Colors.Black, 130, yPos, pageNo);
            AddText(item.Score.ToString(), "Arial", fontsize, false, 0, Colors.Black, 300, yPos, pageNo);
            AddText(item.Grade, "Arial", fontsize, false, 0, Colors.Black, 375, yPos, pageNo);
            AddText(item.Points.ToString(), "Arial", fontsize, false, 0, Colors.Black, 430, yPos, pageNo);
            AddText(item.Remarks, "Arial", fontsize, false, 0, Colors.Black, 480, yPos, pageNo);
            AddText(item.Tutor, "Arial", fontsize, false, 0, Colors.Black, 705, yPos, pageNo);
        }
        private void AddTRSubjectScores(ObservableCollection<StudentTranscriptSubjectModel> psi, int pageNo)
        {
            for (int i = 0; i <= psi.Count - 1; i++)
                AddTRSubjectScore(psi[i], i, pageNo);
        }

        private void GenerateTranscript()
        {
            StudentExamResultModel si = myWorkObject as StudentExamResultModel;
            int pageNo;
            for (pageNo = 0; pageNo < noOfPages; pageNo++)
            {
                AddTRStudentID(si.StudentID, pageNo);
                AddTRName(si.NameOfStudent, pageNo);
                AddTRClassName(si.NameOfClass, pageNo);
                AddTRExamName(si.NameOfExam, pageNo);
                AddTRClassPosition(si.ClassPosition, pageNo);
                AddTRPointsPosition(si.Points, pageNo);
                AddTRTotalMarks(si.TotalMarks, pageNo);
                AddTRMeanGrade(si.MeanGrade, pageNo);
                AddTRSubjectScores(si.Entries, pageNo);
            }
        }
        #endregion

        #region Transcript2
        private void AddTR2Responsibilities(string responsibilities, int pageNo)
        {
            AddTextWithWrap(responsibilities, "Arial", 200, 40, 14, false, 0, Colors.Black, 30, 670, pageNo);
        }
        private void AddTR2Clubs(string clubs, int pageNo)
        {
            AddTextWithWrap(clubs, "Arial", 200, 40, 14, false, 0, Colors.Black, 30, 740, pageNo);
        }
        private void AddTR2Boarding(string boarding, int pageNo)
        {
            AddTextWithWrap(boarding, "Arial", 200, 60, 14, false, 0, Colors.Black, 30, 810, pageNo);
        }
        private void AddTR2ClassTR(string classTR, int pageNo)
        {
            AddTextWithWrap(classTR, "Arial", 200, 30, 14, false, 0, Colors.Black, 30, 940, pageNo);
        }
        private void AddTR2ClassTRComments(string classTRComments, int pageNo)
        {
            AddTextWithWrap(classTRComments, "Arial", 524, 30, 14, false, 0, Colors.Black, 250, 940, pageNo);
        }
        private void AddTR2Principal(string principal, int pageNo)
        {
            AddTextWithWrap(principal, "Arial", 200, 30, 14, false, 0, Colors.Black, 30, 1010, pageNo);
        }
        private void AddTR2PrincipalComments(string principalComments, int pageNo)
        {
            AddTextWithWrap(principalComments, "Arial", 524, 30, 14, false, 0, Colors.Black, 30, 1010, pageNo);
        }
        private void AddTR2Opening(DateTime opening, int pageNo)
        {
            AddText(opening.ToString("dd-MM-yyyy"), "Arial", 14, false, 0, Colors.Black, 350, 1055, pageNo);
        }
        private void AddTR2Closing(DateTime closing, int pageNo)
        {
            AddText(closing.ToString("dd-MM-yyyy"), "Arial", 14, false, 0, Colors.Black, 120, 1055, pageNo);
        }
        private void AddTR2StudentID(int studentID, int pageNo)
        {
            AddText(studentID.ToString(), "Arial", 14, false, 0, Colors.Black, 100, 135, pageNo);
        }
        private void AddTR2Name(string nameOfStudent, int pageNo)
        {
            AddText(nameOfStudent, "Arial", 14, false, 0, Colors.Black, 255, 135, pageNo);
        }
        private void AddTR2ClassName(string className, int pageNo)
        {
            AddText(className, "Arial", 14, true, 0, Colors.Black, 630, 135, pageNo);
        }
        private void AddTR2KCPEScore(int kcpeScore, int pageNo)
        {
            AddText(kcpeScore.ToString(), "Arial", 14, true, 0, Colors.Black, 135, 173, pageNo);
        }

        private void AddTR2Image(byte[] image, int pageNo)
        {
            AddImage(image, double.NaN, double.NaN, 642, 10, 0, pageNo);
        }

        private void AddTR2ClassPosition(string classPosition, int pageNo)
        {
            AddText(classPosition, "Arial", 14, true, 0, Colors.Black, 75, 580, pageNo);
        }
        private void AddTR2TotalMarks(decimal totalMarks, int pageNo)
        {
            AddText(totalMarks.ToString("N2"), "Arial", 14, true, 0, Colors.Black, 250, 580, pageNo);
        }
        private void AddTR2OverAllPosition(string meanGrade, int pageNo)
        {
            AddText(meanGrade, "Arial", 14, true, 0, Colors.Black, 650, 580, pageNo);
        }
        private void AddTR2MeanGrade(string meanGrade, int pageNo)
        {
            AddText(meanGrade, "Arial", 14, true, 0, Colors.Black, 410, 580, pageNo);
        }
        private void AddTR2ClustPoints(decimal point, int pageNo)
        {
            AddText(point.ToString("N2"), "Arial", 14, true, 0, Colors.Black, 495, 580, pageNo);
        }

        private void AddTR2SubjectScore(StudentExamResultEntryModel item, int itemIndex, int pageNo)
        {
            double fontsize = 14;
            int pageRelativeIndex = itemIndex;
            double yPos = 255 + pageRelativeIndex * 21;

            AddText(item.Code.ToString(), "Arial", 14, false, 0, Colors.Black, 40, yPos, pageNo);
            AddText(item.NameOfSubject, "Arial", 14, false, 0, Colors.Black, 100, yPos, pageNo);
            if (item.Cat1Score.HasValue)
                AddText(item.Cat1Score.Value.ToString("N0"), "Arial", fontsize, false, 0, Colors.Black, 242, yPos, pageNo);
            if (item.Cat2Score.HasValue)
                AddText(item.Cat2Score.Value.ToString("N0"), "Arial", fontsize, false, 0, Colors.Black, 302, yPos, pageNo);
            if (item.ExamScore.HasValue)
                AddText(item.ExamScore.Value.ToString("N0"), "Arial", fontsize, false, 0, Colors.Black, 362, yPos, pageNo);
            AddText(item.MeanScore.ToString("N0"), "Arial", fontsize, false, 0, Colors.Black, 422, yPos, pageNo);
            AddText(item.Grade, "Arial", fontsize, false, 0, Colors.Black, 505, yPos, pageNo);
            AddText(item.Points.ToString(), "Arial", fontsize, false, 0, Colors.Black, 562, yPos, pageNo);
            AddText(item.Remarks, "Arial", fontsize, false, 0, Colors.Black, 620, yPos, pageNo);
            AddText(item.Tutor, "Arial", fontsize, false, 0, Colors.Black, 715, yPos, pageNo);
        }
        private void AddTR2SubjectScores(ObservableCollection<StudentExamResultEntryModel> psi, int pageNo)
        {
            for (int i = 0; i <= psi.Count - 1; i++)
                AddTR2SubjectScore(psi[i], i, pageNo);
        }

        private static double GetYHeight(string grade)
        {
            switch (grade)
            {
                case "A": return 188;
                case "A-": return 171;
                case "B+": return 155;
                case "B": return 140;
                case "B-": return 123;
                case "C+": return 107;
                case "C": return 92;
                case "C-": return 75;
                case "D+": return 59;
                case "D": return 44;
                case "D-": return 27;
                case "E": return 12;
            }
            throw new ArgumentOutOfRangeException("Invalid grade value: " + grade);
        }

        private static void TR2DrawGraph(string kcpeGrade, string cat1Grade, string cat2Grade, string examGrade, int pageNo)
        {

            Border bd1, bd2, bd3, bd4;
            bd1 = new Border();
            bd1.Width = 10;
            bd1.Height = GetYHeight(kcpeGrade);
            bd1.HorizontalAlignment = HorizontalAlignment.Left;
            bd1.VerticalAlignment = VerticalAlignment.Bottom;
            bd1.Background = new SolidColorBrush(Colors.Gray);
            bd1.Margin = new Thickness(285, 0, 0, 271);

            bd2 = new Border();
            bd2.Width = 10;
            bd2.Height = GetYHeight(cat1Grade);
            bd2.HorizontalAlignment = HorizontalAlignment.Left;
            bd2.VerticalAlignment = VerticalAlignment.Bottom;
            bd2.Background = new SolidColorBrush(Colors.Gray);
            bd2.Margin = new Thickness(330, 0, 0, 271);

            bd3 = new Border();
            bd3.Width = 10;
            bd3.Height = GetYHeight(cat2Grade);
            bd3.HorizontalAlignment = HorizontalAlignment.Left;
            bd3.VerticalAlignment = VerticalAlignment.Bottom;
            bd3.Background = new SolidColorBrush(Colors.Gray);
            bd3.Margin = new Thickness(380, 0, 0, 271);

            bd4 = new Border();
            bd4.Width = 10;
            bd4.Height = GetYHeight(examGrade);
            bd4.HorizontalAlignment = HorizontalAlignment.Left;
            bd4.VerticalAlignment = VerticalAlignment.Bottom;
            bd4.Background = new SolidColorBrush(Colors.Gray);
            bd4.Margin = new Thickness(428, 0, 0, 271);

            Grid g = doc.Pages[pageNo].Child.Children[0] as Grid;
            g.Children.Add(bd1);
            g.Children.Add(bd2);
            g.Children.Add(bd3);
            g.Children.Add(bd4);
        }

        private void GenerateTranscript2()
        {
            StudentTranscriptModel si = myWorkObject as StudentTranscriptModel;

            int pageNo;
            for (pageNo = 0; pageNo < noOfPages; pageNo++)
            {
                si.Entries = new ObservableCollection<StudentExamResultEntryModel>(si.Entries.OrderBy(o => o.Code));
                AddTR2StudentID(si.StudentID, pageNo);
                AddTR2Name(si.NameOfStudent, pageNo);
                AddTR2ClassName(si.NameOfClass, pageNo);
                AddTR2ClassPosition(si.ClassPosition, pageNo);
                AddTR2KCPEScore(si.KCPEScore, pageNo);
                AddTR2OverAllPosition(si.OverAllPosition, pageNo);
                AddTR2TotalMarks(si.TotalMarks, pageNo);
                AddTR2MeanGrade(si.MeanGrade, pageNo);
                AddTR2SubjectScores(si.Entries, pageNo);
                AddTR2Boarding(si.Boarding, pageNo);
                AddTR2Responsibilities(si.Responsibilities, pageNo);
                AddTR2Clubs(si.ClubsAndSport, pageNo);
                AddTR2Principal(si.Principal, pageNo);
                AddTR2PrincipalComments(si.PrincipalComments, pageNo);
                AddTR2ClassTR(si.ClassTeacher, pageNo);
                AddTR2Opening(si.OpeningDay, pageNo);
                AddTR2Closing(si.ClosingDay, pageNo);
                AddTR2ClustPoints(si.Points, pageNo);
                AddTR2ClassTRComments(si.ClassTeacherComments, pageNo);
                AddTR2Image(si.SPhoto, pageNo);
                TR2DrawGraph(DataController.CalculateGrade(decimal.Ceiling(Convert.ToDecimal(si.KCPEScore) / 5m)),
                    si.CAT1Grade,
                    si.CAT2Grade,
                    si.ExamGrade,
                    pageNo);
                var t = si.CAT1Grade;
            }
        }

        #endregion

        #region Transcript3
        private void AddTR3StudentID(int studentID, int pageNo)
        {
            AddText(studentID.ToString(), "Arial", 14, true, 0, Colors.Black, 135, 270, pageNo);
        }
        private void AddTR3Name(string nameOfStudent, int pageNo)
        {
            AddText(nameOfStudent, "Arial", 14, true, 0, Colors.Black, 120, 237, pageNo);
        }
        private void AddTR3ClassName(string className, int pageNo)
        {
            AddText(className, "Arial", 14, true, 0, Colors.Black, 535, 237, pageNo);
        }
        private void AddTR3KCPEScore(int kcpeScore, int pageNo)
        {
            AddText(kcpeScore.ToString(), "Arial", 14, true, 0, Colors.Black, 580, 270, pageNo);
        }

        private void AddTR3Image(byte[] image, int pageNo)
        {
            AddImage(image, 135, 150, 640, 17, 0, pageNo);
        }

        private void AddTR3SubjectScore(StudentExamResultEntryModel item, int itemIndex, int pageNo)
        {
            double fontsize = 14;
            int pageRelativeIndex = itemIndex;
            double yPos = 340 + pageRelativeIndex * 25;

            AddText(item.Code.ToString(), "Arial", 14, true, 0, Colors.Black, 33, yPos, pageNo);
            AddText(item.NameOfSubject, "Arial", 14, true, 0, Colors.Black, 85, yPos, pageNo);
            if (item.Cat1Score.HasValue)
                AddText(item.Cat1Score.Value.ToString("N0"), "Arial", fontsize, true, 0, Colors.Black, 270, yPos, pageNo);
            if (item.Cat2Score.HasValue)
                AddText(item.Cat2Score.Value.ToString("N0"), "Arial", fontsize, true, 0, Colors.Black, 320, yPos, pageNo);
            if (item.ExamScore.HasValue)
                AddText(item.ExamScore.Value.ToString("N0"), "Arial", fontsize, true, 0, Colors.Black, 370, yPos, pageNo);
            AddText(item.MeanScore.ToString("N0"), "Arial", fontsize, true, 0, Colors.Black, 420, yPos, pageNo);
            AddText(item.Grade, "Arial", fontsize, true, 0, Colors.Black, 520, yPos, pageNo);
            AddText(item.Points.ToString(), "Arial", fontsize, true, 0, Colors.Black, 470, yPos, pageNo);
            AddText(item.Remarks, "Arial", fontsize, true, 0, Colors.Black, 560, yPos, pageNo);
            AddText(item.Tutor, "Arial", fontsize, true, 0, Colors.Black, 725, yPos, pageNo);
        }
        private void AddTR3SubjectScores(ObservableCollection<StudentExamResultEntryModel> psi, int pageNo)
        {
            for (int i = 0; i <= psi.Count - 1; i++)
                AddTR3SubjectScore(psi[i], i, pageNo);
        }

        private void AddTR3TotalMarks(decimal totalMarks, int pageNo)
        {
            AddText(totalMarks.ToString("N2"), "Arial", 14, true, 0, Colors.Black, 410, 616, pageNo);
        }
        private void AddTR3OutOf(decimal outOf, int pageNo)
        {
            AddText(outOf.ToString("N2"), "Arial", 14, true, 0, Colors.Black, 410, 640, pageNo);
        }

        private void AddTR3Term1TotalScore(string term1TotalScore, int pageNo)
        {
            AddText(term1TotalScore, "Arial", 14, true, 0, Colors.Black, 150, 692, pageNo);
        }
        private void AddTR3Term2TotalScore(string term2TotalScore, int pageNo)
        {
            AddText(term2TotalScore, "Arial", 14, true, 0, Colors.Black, 150, 712, pageNo);
        }
        private void AddTR3Term3TotalScore(string term3TotalScore, int pageNo)
        {
            AddText(term3TotalScore, "Arial", 14, true, 0, Colors.Black, 150, 732, pageNo);
        }
        private void AddTR3Term1AvgPts(decimal term1Avgpts, int pageNo)
        {
            AddText(term1Avgpts > 0 ? term1Avgpts.ToString("N2") : "-", "Arial", 14, true, 0, Colors.Black, 267, 692, pageNo);
        }
        private void AddTR3Term2AvgPts(decimal term2Avgpts, int pageNo)
        {
            AddText(term2Avgpts > 0 ? term2Avgpts.ToString("N2") : "-", "Arial", 14, true, 0, Colors.Black, 267, 712, pageNo);
        }
        private void AddTR3Term3AvgPts(decimal term3Avgpts, int pageNo)
        {
            AddText(term3Avgpts > 0 ? term3Avgpts.ToString("N2") : "-", "Arial", 14, true, 0, Colors.Black, 267, 732, pageNo);
        }
        private void AddTR3Term1PtsChange(decimal term1PtsChange, int pageNo)
        {
            if (term1PtsChange > 0)
                AddText("+" + term1PtsChange.ToString("N2"), "Arial", 14, true, 0, Colors.Black, 315, 692, pageNo);
            else if (term1PtsChange < 0)
                AddText(term1PtsChange.ToString("N2"), "Arial", 14, true, 0, Colors.Black, 315, 692, pageNo);
            else
                AddText(term1PtsChange.ToString("N2"), "Arial", 14, true, 0, Colors.Black, 320, 692, pageNo);
        }
        private void AddTR3Term2PtsChange(decimal term2PtsChange, int pageNo)
        {
            if (term2PtsChange > 0)
                AddText("+" + term2PtsChange.ToString("N2"), "Arial", 14, true, 0, Colors.Black, 315, 712, pageNo);
            else if (term2PtsChange < 0)
                AddText(term2PtsChange.ToString("N2"), "Arial", 14, true, 0, Colors.Black, 315, 712, pageNo);
            else
                AddText(term2PtsChange.ToString("N2"), "Arial", 14, true, 0, Colors.Black, 320, 712, pageNo);
        }
        private void AddTR3Term3PtsChange(decimal term3PtsChange, int pageNo)
        {
            if (term3PtsChange > 0)
                AddText("+" + term3PtsChange.ToString("N2"), "Arial", 14, true, 0, Colors.Black, 315, 732, pageNo);
            else if (term3PtsChange < 0)
                AddText(term3PtsChange.ToString("N2"), "Arial", 14, true, 0, Colors.Black, 315, 732, pageNo);
            else
                AddText(term3PtsChange.ToString("N2"), "Arial", 14, true, 0, Colors.Black, 320, 732, pageNo);
        }
        private void AddTR3Term1TotalPts(string term1TotalPts, int pageNo)
        {
            AddText(term1TotalPts, "Arial", 14, true, 0, Colors.Black, 380, 692, pageNo);
        }
        private void AddTR3Term2TotalPts(string term2TotalPts, int pageNo)
        {
            AddText(term2TotalPts, "Arial", 14, true, 0, Colors.Black, 380, 712, pageNo);
        }
        private void AddTR3Term3TotalPts(string term3TotalPts, int pageNo)
        {
            AddText(term3TotalPts, "Arial", 14, true, 0, Colors.Black, 380, 732, pageNo);
        }
        private void AddTR3Term1Score(decimal term1Score, int pageNo)
        {
            AddText(term1Score.ToString("N2"), "Arial", 14, true, 0, Colors.Black, 485, 692, pageNo);
        }
        private void AddTR3Term2Score(decimal term2Score, int pageNo)
        {
            AddText(term2Score.ToString("N2"), "Arial", 14, true, 0, Colors.Black, 485, 712, pageNo);
        }
        private void AddTR3Term3Score(decimal term3Score, int pageNo)
        {
            AddText(term3Score.ToString("N2"), "Arial", 14, true, 0, Colors.Black, 485, 732, pageNo);
        }
        private void AddTR3Term1Grade(string term1Grade, int pageNo)
        {
            AddText(term1Grade, "Arial", 14, true, 0, Colors.Black, 584, 692, pageNo);
        }
        private void AddTR3Term2Grade(string term2Grade, int pageNo)
        {
            AddText(term2Grade, "Arial", 14, true, 0, Colors.Black, 584, 712, pageNo);
        }
        private void AddTR3Term3Grade(string term3Grade, int pageNo)
        {
            AddText(term3Grade, "Arial", 14, true, 0, Colors.Black, 584, 732, pageNo);
        }
        private void AddTR3Term1ClassPOS(string term1POS, int pageNo)
        {
            AddText(term1POS, "Arial", 14, true, 0, Colors.Black, 640, 692, pageNo);
        }
        private void AddTR3Term2ClassPOS(string term2POS, int pageNo)
        {
            AddText(term2POS, "Arial", 14, true, 0, Colors.Black, 640, 712, pageNo);
        }
        private void AddTR3Term3ClassPOS(string term3POS, int pageNo)
        {
            AddText(term3POS, "Arial", 14, true, 0, Colors.Black, 640, 732, pageNo);
        }
        private void AddTR3Term1CombinedClassPOS(string term1POS, int pageNo)
        {
            AddText(term1POS, "Arial", 14, true, 0, Colors.Black, 705, 692, pageNo);
        }
        private void AddTR3Term2CombinedClassPOS(string term1POS, int pageNo)
        {
            AddText(term1POS, "Arial", 14, true, 0, Colors.Black, 705, 712, pageNo);
        }
        private void AddTR3Term3CombinedClassPOS(string term1POS, int pageNo)
        {
            AddText(term1POS, "Arial", 14, true, 0, Colors.Black, 705, 732, pageNo);
        }


        private void AddTR3ClassTRComments(string classTRComments, int pageNo)
        {
            AddTextWithWrap(classTRComments, "Arial", 524, 30, 14, true, 0, Colors.Black, 50, 860, pageNo);
        }
        private void AddTR3PrincipalComments(string principalComments, int pageNo)
        {
            AddTextWithWrap(principalComments, "Arial", 445, 100, 14, true, 0, Colors.Black, 50, 945, pageNo);
        }
        private void AddTR3Opening(DateTime opening, int pageNo)
        {
            AddText(opening.ToString("dd MMM yyyy"), "Arial", 14, true, 0, Colors.Black, 545, 1017, pageNo);
        }
        private void AddTR3Closing(DateTime closing, int pageNo)
        {
            AddText(closing.ToString("dd MMM yyyy"), "Arial", 14, true, 0, Colors.Black, 545, 950, pageNo);
        }

        private void GenerateTranscript3()
        {
            StudentTranscriptModel2 si = myWorkObject as StudentTranscriptModel2;

            int pageNo;
            for (pageNo = 0; pageNo < noOfPages; pageNo++)
            {
                si.Entries = new ObservableCollection<StudentExamResultEntryModel>(si.Entries.OrderBy(o => o.Code));
                AddTR3StudentID(si.StudentID, pageNo);
                AddTR3Name(si.NameOfStudent, pageNo);
                AddTR3ClassName(si.NameOfClass, pageNo);
                AddTR3KCPEScore(si.KCPEScore, pageNo);
                AddTR3Image(si.SPhoto, pageNo);
                AddTR3SubjectScores(si.Entries, pageNo);

                AddTR3TotalMarks(si.TotalMarks, pageNo);
                AddTR3OutOf(si.Entries.Count * 100, pageNo);
                if (si.Term1AvgPts > 0)
                {
                    AddTR3Term1AvgPts(si.Term1AvgPts, pageNo);
                    AddTR3Term1TotalScore(si.Term1TotalScore, pageNo);
                    AddTR3Term1PtsChange(si.Term1PtsChange, pageNo);
                    AddTR3Term1TotalPts(si.Term1TotalPoints, pageNo);
                    AddTR3Term1Score(si.Term1MeanScore, pageNo);
                    AddTR3Term1Grade(si.Term1Grade, pageNo);
                    AddTR3Term1ClassPOS(si.Term1Pos, pageNo);
                    AddTR3Term1CombinedClassPOS(si.Term1OverallPos, pageNo);

                }
                if (si.Term2AvgPts > 0)
                {
                    AddTR3Term2TotalScore(si.Term2TotalScore, pageNo);
                    AddTR3Term2AvgPts(si.Term2AvgPts, pageNo);
                    AddTR3Term2PtsChange(si.Term2PtsChange, pageNo);
                    AddTR3Term2TotalPts(si.Term2TotalPoints, pageNo);
                    AddTR3Term2Score(si.Term2MeanScore, pageNo);
                    AddTR3Term2Grade(si.Term2Grade, pageNo);
                    AddTR3Term2ClassPOS(si.Term2Pos, pageNo);
                    AddTR3Term2CombinedClassPOS(si.Term2OverallPos, pageNo);
                }
                if (si.Term3AvgPts > 0)
                {
                    AddTR3Term3TotalScore(si.Term3TotalScore, pageNo);
                    AddTR3Term3AvgPts(si.Term3AvgPts, pageNo);
                    AddTR3Term3PtsChange(si.Term3PtsChange, pageNo);
                    AddTR3Term3TotalPts(si.Term3TotalPoints, pageNo);
                    AddTR3Term3Score(si.Term3MeanScore, pageNo);
                    AddTR3Term3Grade(si.Term3Grade, pageNo);
                    AddTR3Term3ClassPOS(si.Term3Pos, pageNo);
                    AddTR3Term3CombinedClassPOS(si.Term3OverallPos, pageNo);
                }



                AddTR3ClassTRComments(si.ClassTeacherComments, pageNo);
                AddTR3PrincipalComments(si.PrincipalComments, pageNo);

                AddTR3Opening(si.OpeningDay, pageNo);
                AddTR3Closing(si.ClosingDay, pageNo);
            }
        }

        #endregion
    }
}
