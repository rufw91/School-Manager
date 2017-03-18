using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using UmanyiSMS.Lib.Controllers;
using UmanyiSMS.Modules.Exams.Models;

namespace UmanyiSMS.Modules.Exams.Controller
{
    public class DocumentHelper : DocumentHelperBase
    {
        private DocumentHelper(object workObject)
            :base(workObject)
        {            
        }

        public static FixedDocument GenerateDocument(object workObject)
        {
            new DocumentHelper(workObject);
            return Document;
        }

        protected override void AddDataToDocument()
        {
            if (MyWorkObject is StudentTranscriptModel)
                GenerateTranscript();
            //if (MyWorkObject is StudentTranscriptModel2)
                //GenerateTranscript3();
            //if (workObject is ReportFormModel)
            //return 1;
            if (MyWorkObject is ClassReportFormModel)
                GenerateClassTranscripts();
           // if (MyWorkObject is ClassTranscriptsModel2)
             //   GenerateClassTranscripts();
            if (MyWorkObject is ClassTranscriptsModel)
                GenerateClassTranscripts();
            if (MyWorkObject is ClassStudentsExamResultModel)
                GenerateClassExamResults();
            if (MyWorkObject is ClassExamResultModel)
                GenerateClassMarkList();

            throw new ArgumentException();

        }

        protected override string GetResString()
        {
            if (MyWorkObject is StudentTranscriptModel)
                return GetResourceString(null);
            if (MyWorkObject is ReportFormModel)
                return GetResourceString(null);
            if (MyWorkObject is ClassReportFormModel)
                return GetResourceString(null);
            if (MyWorkObject is ClassTranscriptsModel)
                return GetResourceString(null);
            if (MyWorkObject is ClassStudentsExamResultModel)
                return GetResourceString(null);

            return "";
        }

        protected override int GetNoOfPages()
        {
            if (MyWorkObject is StudentTranscriptModel)
                return 1;
            if (MyWorkObject is ReportFormModel)
                return 1;
            if (MyWorkObject is ClassReportFormModel)
                return (MyWorkObject as ClassReportFormModel).Count;
            if (MyWorkObject is ClassTranscriptsModel)
                return (MyWorkObject as ClassTranscriptsModel).Entries.Count;
            if (MyWorkObject is ClassStudentsExamResultModel)
                return (MyWorkObject as ClassStudentsExamResultModel).Entries.Count;

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
            int pageRelativeIndex = itemIndex - ItemsPerPage * pageNo;
            double yPos = 355 + pageRelativeIndex * 25;

            AddText(item.NameOfSubject, "Arial", 14, false, 0, Colors.Black, 45, yPos, pageNo);
            AddText(item.MeanScore.ToString("N4"), "Arial", fontsize, false, 0, Colors.Black, 285, yPos, pageNo);
            AddText(item.MeanGrade, "Arial", fontsize, false, 0, Colors.Black, 475, yPos, pageNo);
            AddText(item.Points.ToString(), "Arial", fontsize, false, 0, Colors.Black, 630, yPos, pageNo);
        }
        private void AddAGEntries(ObservableCollection<AggregateResultEntryModel> psi, int pageNo)
        {

            int startIndex = pageNo * ItemsPerPage;
            int endIndex = startIndex + ItemsPerPage - 1;
            if (startIndex >= psi.Count)
                return;
            if (endIndex >= psi.Count)
                endIndex = psi.Count - 1;

            for (int i = startIndex; i <= endIndex; i++)
                AddAGEntry(psi[i], i, pageNo);
        }

        private void GenerateAggregateResult()
        {
            AggregateResultModel si = MyWorkObject as AggregateResultModel;

            int pageNo;
            for (pageNo = 0; pageNo < NoOfPages; pageNo++)
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
            int pageRelativeIndex = itemIndex - ItemsPerPage * pageNo;
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
            int startIndex = pageNo * ItemsPerPage;
            int endIndex = startIndex + ItemsPerPage - 1;
            if (startIndex >= psi.Rows.Count)
                return;
            if (endIndex >= psi.Rows.Count)
                endIndex = psi.Rows.Count - 1;

            for (int i = startIndex; i <= endIndex; i++)
                AddCMLStudent(psi.Rows[i], i, pageNo);
        }

        private void GenerateClassMarkList()
        {
            ClassExamResultModel si = MyWorkObject as ClassExamResultModel;

            int pageNo;
            for (pageNo = 0; pageNo < NoOfPages; pageNo++)
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
            ClassStudentsExamResultModel si = MyWorkObject as ClassStudentsExamResultModel;

            int pageNo;
            for (pageNo = 0; pageNo < NoOfPages; pageNo++)
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
            ClassTranscriptsModel si = MyWorkObject as ClassTranscriptsModel;

            int pageNo;
            for (pageNo = 0; pageNo < NoOfPages; pageNo++)
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
            StudentExamResultModel si = MyWorkObject as StudentExamResultModel;
            int pageNo;
            for (pageNo = 0; pageNo < NoOfPages; pageNo++)
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

            Grid g = Document.Pages[pageNo].Child.Children[0] as Grid;
            g.Children.Add(bd1);
            g.Children.Add(bd2);
            g.Children.Add(bd3);
            g.Children.Add(bd4);
        }

        private void GenerateTranscript2()
        {
            StudentTranscriptModel si = MyWorkObject as StudentTranscriptModel;

            int pageNo;
            for (pageNo = 0; pageNo < NoOfPages; pageNo++)
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

                var t = si.CAT1Grade;
            }
        }

        #endregion
        
    }
}
