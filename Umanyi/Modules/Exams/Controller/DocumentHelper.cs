using System;
using System.Collections.Generic;
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
            if (MyWorkObject is AggregateResultModel)
                GenerateAggregateResult();
            else if (MyWorkObject is StudentExamResultModel)
             GenerateTranscript();
           else if (MyWorkObject is ReportFormModel)
                GenerateReportForm();
            else if (MyWorkObject is ClassReportFormModel)
                GenerateClassReportForms();
            else if (MyWorkObject is ClassStudentsExamResultModel)
                GenerateClassExamResults();
            else if (MyWorkObject is ClassExamResultModel)
                GenerateClassMarkList();

            else throw new ArgumentException();

        }

        protected override string GetResString()
        {
            if (MyWorkObject is AggregateResultModel)
                return GetResourceString(new Uri("pack://application:,,,/UmanyiSMS;component/Modules/Exams/Resources/AggregateResult.xaml"));
            if (MyWorkObject is ReportFormModel)
                return GetResourceString(new Uri("pack://application:,,,/UmanyiSMS;component/Modules/Exams/Resources/ReportForm.xaml"));
            if (MyWorkObject is ClassReportFormModel)
                return GetResourceString(new Uri("pack://application:,,,/UmanyiSMS;component/Modules/Exams/Resources/ReportForm.xaml"));
           if (MyWorkObject is ClassStudentsExamResultModel)
                return GetResourceString(new Uri("pack://application:,,,/UmanyiSMS;component/Modules/Exams/Resources/Transcript.xaml"));
            if (MyWorkObject is ClassExamResultModel)
                return GetResourceString(new Uri("pack://application:,,,/UmanyiSMS;component/Modules/Exams/Resources/ClassMarkList.xaml"));
            if (MyWorkObject is StudentExamResultModel)
                return GetResourceString(new Uri("pack://application:,,,/UmanyiSMS;component/Modules/Exams/Resources/Transcript.xaml"));

            throw new ArgumentException();
        }

        protected override int GetNoOfPages()
        {
            if (MyWorkObject is AggregateResultModel)
                return 1;
            if (MyWorkObject is ReportFormModel)
                return 1;
            if (MyWorkObject is ClassReportFormModel)
                return (MyWorkObject as ClassReportFormModel).Count;
            if (MyWorkObject is ClassTranscriptsModel)
                return (MyWorkObject as ClassTranscriptsModel).Entries.Count;
            if (MyWorkObject is ClassStudentsExamResultModel)
                return (MyWorkObject as ClassStudentsExamResultModel).Entries.Count;
            if (MyWorkObject is ClassExamResultModel)
            {
                var totalNoOfItems = (MyWorkObject as ClassExamResultModel).Entries.Rows.Count;
                return (totalNoOfItems % ItemsPerPage) != 0 ?
                                (totalNoOfItems / ItemsPerPage) + 1 : (totalNoOfItems / ItemsPerPage);
            }
            if (MyWorkObject is StudentExamResultModel)
                return 1;
            throw new ArgumentException();
        }

        protected override int GetItemsPerPage()
        {           
            if (MyWorkObject is ClassStudentsExamResultModel)
                return 37;
            if (MyWorkObject is ClassExamResultModel)
                return 37;

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

        #region Class Reportforms
        private void GenerateClassReportForms()
        {
            ClassReportFormModel si = MyWorkObject as ClassReportFormModel;

            int pageNo;
            for (pageNo = 0; pageNo < NoOfPages; pageNo++)
            {
                si[pageNo].SubjectEntries = new ObservableCollection<ReportFormSubjectModel>(si[pageNo].SubjectEntries.OrderBy(o => o.Code));
                AddRPMStudentID(si[pageNo].StudentID, pageNo);
                AddRPMName(si[pageNo].NameOfStudent, pageNo);
                AddRPMClassName(si[pageNo].NameOfClass, pageNo);
                AddRPMImage(si[pageNo].SPhoto, pageNo);
                AddRPMSubjectScores(si[pageNo].SubjectEntries, pageNo);
                AddRPMTotalMarks(si[pageNo].TotalMarks, pageNo);
                AddRPMOutOf(si[pageNo].SubjectEntries.Count * 100, pageNo);
                AddRPMAvgPts(si[pageNo].AvgPoints, pageNo);
                AddRPMTotalPts(si[pageNo].TotalPoints.ToString("N2"), pageNo);
                AddRPMMeanScore(si[pageNo].MeanScore, pageNo);
                AddRPMGrade(si[pageNo].MeanGrade, pageNo);
                AddRPMClassPOS(si[pageNo].ClassRank, pageNo);
                AddRPMCombinedClassPOS(si[pageNo].StreamRank, pageNo);
                AddRPMClassTRComments(si[pageNo].ClassTeacherComments, pageNo);
                AddRPMPrincipalComments(si[pageNo].PrincipalComments, pageNo);
                AddRPMOpening(si[pageNo].OpeningDay, pageNo);
                AddRPMClosing(si[pageNo].ClosingDay, pageNo);
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
        /*
        #region Report Form
        private void AddRPMClassTRComments(string classTRComments, int pageNo)
        {
            AddTextWithWrap(classTRComments, "Arial", 524, 30, 14, false, 0, Colors.Black, 250, 940, pageNo);
        }
        private void AddRPMPrincipalComments(string principalComments, int pageNo)
        {
            AddTextWithWrap(principalComments, "Arial", 524, 30, 14, false, 0, Colors.Black, 30, 1010, pageNo);
        }
        private void AddRPMOpening(DateTime opening, int pageNo)
        {
            AddText(opening.ToString("dd-MM-yyyy"), "Arial", 14, false, 0, Colors.Black, 350, 1055, pageNo);
        }
        private void AddRPMClosing(DateTime closing, int pageNo)
        {
            AddText(closing.ToString("dd-MM-yyyy"), "Arial", 14, false, 0, Colors.Black, 120, 1055, pageNo);
        }
        private void AddRPMStudentID(int studentID, int pageNo)
        {
            AddText(studentID.ToString(), "Arial", 14, false, 0, Colors.Black, 100, 135, pageNo);
        }
        private void AddRPMName(string nameOfStudent, int pageNo)
        {
            AddText(nameOfStudent, "Arial", 14, false, 0, Colors.Black, 255, 135, pageNo);
        }
        private void AddRPMClassName(string className, int pageNo)
        {
            AddText(className, "Arial", 14, true, 0, Colors.Black, 630, 135, pageNo);
        }
        
        private void AddRPMImage(byte[] image, int pageNo)
        {
            AddImage(image, double.NaN, double.NaN, 642, 10, 0, pageNo);
        }

        private void AddRPMClassPosition(string classPosition, int pageNo)
        {
            AddText(classPosition, "Arial", 14, true, 0, Colors.Black, 75, 580, pageNo);
        }
        private void AddRPMTotalMarks(decimal totalMarks, int pageNo)
        {
            AddText(totalMarks.ToString("N2"), "Arial", 14, true, 0, Colors.Black, 250, 580, pageNo);
        }
        private void AddRPMOverAllPosition(string meanGrade, int pageNo)
        {
            AddText(meanGrade, "Arial", 14, true, 0, Colors.Black, 650, 580, pageNo);
        }
        private void AddRPMMeanGrade(string meanGrade, int pageNo)
        {
            AddText(meanGrade, "Arial", 14, true, 0, Colors.Black, 410, 580, pageNo);
        }
        private void AddRPMMeanScore(decimal point, int pageNo)
        {
            AddText(point.ToString("N2"), "Arial", 14, true, 0, Colors.Black, 495, 580, pageNo);
        }
        private void AddRPMTotalPoints(decimal point, int pageNo)
        {
            AddText(point.ToString("N2"), "Arial", 14, true, 0, Colors.Black, 495, 580, pageNo);
        }
        private void AddRPMAvgPoints(decimal point, int pageNo)
        {
            AddText(point.ToString("N2"), "Arial", 14, true, 0, Colors.Black, 495, 580, pageNo);
        }

        private void AddRPMSubjectScore(ReportFormSubjectModel item, int itemIndex, int pageNo)
        {
            double fontsize = 14;
            int pageRelativeIndex = itemIndex;
            double yPos = 255 + pageRelativeIndex * 21;

            AddText(item.Code.ToString(), "Arial", 14, false, 0, Colors.Black, 40, yPos, pageNo);
            AddText(item.NameOfSubject, "Arial", 14, false, 0, Colors.Black, 100, yPos, pageNo);
            if (!string.IsNullOrWhiteSpace(item.Exam1Score))
                AddText(item.Exam1Score, "Arial", fontsize, false, 0, Colors.Black, 242, yPos, pageNo);
            if (!string.IsNullOrWhiteSpace(item.Exam2Score))
                AddText(item.Exam2Score, "Arial", fontsize, false, 0, Colors.Black, 302, yPos, pageNo);
            if (!string.IsNullOrWhiteSpace(item.Exam3Score))
                AddText(item.Exam3Score, "Arial", fontsize, false, 0, Colors.Black, 362, yPos, pageNo);
            AddText(item.MeanScore.ToString("N0"), "Arial", fontsize, false, 0, Colors.Black, 422, yPos, pageNo);
            AddText(item.Grade, "Arial", fontsize, false, 0, Colors.Black, 505, yPos, pageNo);
            AddText(item.StreamRank, "Arial", fontsize, false, 0, Colors.Black, 562, yPos, pageNo);
            AddText(item.Remarks, "Arial", fontsize, false, 0, Colors.Black, 620, yPos, pageNo);
        }
        
        private void AddRPMSubjectScores(IEnumerable<ReportFormSubjectModel> psi, int pageNo)
        {
            for (int i = 0; i <= psi.Count() - 1; i++)
                AddRPMSubjectScore(psi.ElementAt(i), i, pageNo);
        }
        
        private void GenerateReportForm()
        {
            ReportFormModel si = MyWorkObject as ReportFormModel;

            int pageNo;
            for (pageNo = 0; pageNo < NoOfPages; pageNo++)
            {
                si.SubjectEntries = new ObservableCollection<ReportFormSubjectModel>(si.SubjectEntries.OrderBy(o => o.Code));
                AddRPMStudentID(si.StudentID, pageNo);
                AddRPMName(si.NameOfStudent, pageNo);
                AddRPMClassName(si.NameOfClass, pageNo);
                AddRPMClassPosition(si.ClassRank, pageNo);
                AddRPMOverAllPosition(si.StreamRank, pageNo);
                AddRPMTotalMarks(si.TotalMarks, pageNo);
                AddRPMMeanScore(si.MeanScore, pageNo);
                AddRPMMeanGrade(si.MeanGrade, pageNo);
                AddRPMTotalPoints(si.TotalPoints, pageNo);
                AddRPMAvgPoints(si.AvgPoints, pageNo);                            
                AddRPMOpening(si.OpeningDay, pageNo);
                AddRPMClosing(si.ClosingDay, pageNo);
                AddRPMPrincipalComments(si.PrincipalComments, pageNo);
                AddRPMClassTRComments(si.ClassTeacherComments, pageNo);
                AddRPMImage(si.SPhoto, pageNo);
                AddRPMSubjectScores(si.SubjectEntries, pageNo);
            }
        }

        #endregion
        */
        #region Transcript3
       private void AddRPMStudentID(int studentID, int pageNo)
        {
            AddText(studentID.ToString(), "Arial", 14, true, 0, Colors.Black, 135, 270, pageNo);
        }
       private void AddRPMName(string nameOfStudent, int pageNo)
        {
            AddText(nameOfStudent, "Arial", 14, true, 0, Colors.Black, 120, 237, pageNo);
        }
       private void AddRPMClassName(string className, int pageNo)
        {
            AddText(className, "Arial", 14, true, 0, Colors.Black, 535, 237, pageNo);
        }
       private void AddRPMImage(byte[] image, int pageNo)
        {
            AddImage(image, 135, 150, 640, 17, 0, pageNo);
        }
        private void AddRPMSubjectScore(ReportFormSubjectModel item, int itemIndex, int pageNo)
        {
            double fontsize = 14;
            int pageRelativeIndex = itemIndex;
            double yPos = 337 + pageRelativeIndex * 25;
            
            AddText(item.NameOfSubject, "Segoe UI", 14, false, 0, Colors.Black, 50, yPos, pageNo);
            if (!string.IsNullOrWhiteSpace(item.Exam1Score))
                AddText(item.Exam1Score, "Arial", fontsize, false, 0, Colors.Black, 260, yPos, pageNo);
            if (!string.IsNullOrWhiteSpace(item.Exam2Score))
                AddText(item.Exam2Score, "Arial", fontsize, false, 0, Colors.Black, 310, yPos, pageNo);
            if (!string.IsNullOrWhiteSpace(item.Exam3Score))
                AddText(item.Exam3Score, "Arial", fontsize, false, 0, Colors.Black, 360, yPos, pageNo);
            AddText(item.MeanScore.ToString("N0"), "Arial", fontsize, false, 0, Colors.Black, 412, yPos, pageNo);
            AddText(item.StreamRank, "Arial", fontsize, false, 0, Colors.Black, 460, yPos, pageNo);

            AddText(item.Grade, "Segoe UI", fontsize, false, 0, Colors.Black, 510, yPos, pageNo);
            AddText(item.Remarks, "Segoe UI", fontsize, false, 0, Colors.Black, 567, yPos, pageNo);
        }
        private void AddRPMSubjectScores(ObservableCollection<ReportFormSubjectModel> psi, int pageNo)
        {
            for (int i = 0; i <= psi.Count - 1; i++)
                AddRPMSubjectScore(psi[i], i, pageNo);
        }

       private void AddRPMTotalMarks(decimal totalMarks, int pageNo)
        {
            AddText(totalMarks.ToString("N2"), "Arial", 14, true, 0, Colors.Black, 410, 615, pageNo);
        }
       private void AddRPMOutOf(decimal outOf, int pageNo)
        {
            AddText(outOf.ToString("N2"), "Arial", 14, true, 0, Colors.Black, 410, 640, pageNo);
        }

       private void AddRPMMeanScore(decimal meanScore, int pageNo)
        {
            AddText(meanScore.ToString("N2"), "Segoe UI", 14, true, 0, Colors.Black, 110, 692, pageNo);
        }
        private void AddRPMAvgPts(decimal term1Avgpts, int pageNo)
        {
            AddText(term1Avgpts > 0 ? term1Avgpts.ToString("N2") : "-", "Arial", 14, true, 0, Colors.Black, 225, 692, pageNo);
        }
        private void AddRPMTotalPts(string term1TotalPts, int pageNo)
        {
            AddText(term1TotalPts, "Arial", 14, true, 0, Colors.Black, 325, 692, pageNo);
        }
        private void AddRPMGrade(string term1Grade, int pageNo)
        {
            AddText(term1Grade, "Arial", 14, true, 0, Colors.Black, 430, 692, pageNo);
        }
       private void AddRPMClassPOS(string term1POS, int pageNo)
        {
            AddText(term1POS, "Arial", 14, true, 0, Colors.Black, 520, 692, pageNo);
        }
       private void AddRPMCombinedClassPOS(string term1POS, int pageNo)
        {
            AddText(term1POS, "Arial", 14, true, 0, Colors.Black, 630, 692, pageNo);
        }


       private void AddRPMClassTRComments(string classTRComments, int pageNo)
        {
            AddTextWithWrap(classTRComments, "Arial", 700, 70, 14, true, 0, Colors.Black, 50, 800, pageNo);
        }
       private void AddRPMPrincipalComments(string principalComments, int pageNo)
        {
            AddTextWithWrap(principalComments, "Arial", 445, 80, 14, true, 0, Colors.Black, 50, 945, pageNo);
        }
       private void AddRPMOpening(DateTime opening, int pageNo)
        {
            AddText(opening.ToString("dd MMM yyyy"), "Arial", 14, true, 0, Colors.Black, 545, 1017, pageNo);
        }
       private void AddRPMClosing(DateTime closing, int pageNo)
        {
            AddText(closing.ToString("dd MMM yyyy"), "Arial", 14, true, 0, Colors.Black, 545, 950, pageNo);
        }

       private void GenerateReportForm()
        {
            ReportFormModel si = MyWorkObject as ReportFormModel;

            int pageNo;
            for (pageNo = 0; pageNo < NoOfPages; pageNo++)
            {
                si.SubjectEntries = new ObservableCollection<ReportFormSubjectModel>(si.SubjectEntries.OrderBy(o => o.Code));
                AddRPMStudentID(si.StudentID, pageNo);
                AddRPMName(si.NameOfStudent, pageNo);
                AddRPMClassName(si.NameOfClass, pageNo);
                AddRPMImage(si.SPhoto, pageNo);
                AddRPMSubjectScores(si.SubjectEntries, pageNo);
                AddRPMTotalMarks(si.TotalMarks, pageNo);
                AddRPMOutOf(si.SubjectEntries.Count * 100, pageNo);
                AddRPMAvgPts(si.AvgPoints, pageNo);
                AddRPMTotalPts(si.TotalPoints.ToString("N2"), pageNo);
                AddRPMMeanScore(si.MeanScore, pageNo);
                AddRPMGrade(si.MeanGrade, pageNo);
                AddRPMClassPOS(si.ClassRank, pageNo);
                AddRPMCombinedClassPOS(si.StreamRank, pageNo);
                AddRPMClassTRComments(si.ClassTeacherComments, pageNo);
                AddRPMPrincipalComments(si.PrincipalComments, pageNo);
                AddRPMOpening(si.OpeningDay, pageNo);
                AddRPMClosing(si.ClosingDay, pageNo);
            }
        }

        #endregion

    }
}
