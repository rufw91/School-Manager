using Helper.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Helper
{
    public static partial class DocumentHelper
    {

        #region Transcript2
        private static void AddTR2Responsibilities(string responsibilities, int pageNo)
        {
            AddTextWithWrap(responsibilities, "Arial", 200, 40, 14, false, 0, Colors.Black, 30, 670, pageNo);
        }
        private static void AddTR2Clubs(string clubs, int pageNo)
        {
            AddTextWithWrap(clubs, "Arial", 200, 40, 14, false, 0, Colors.Black, 30, 740, pageNo);
        }
        private static void AddTR2Boarding(string boarding, int pageNo)
        {
            AddTextWithWrap(boarding, "Arial", 200, 60, 14, false, 0, Colors.Black, 30, 810, pageNo);
        }
        private static void AddTR2ClassTR(string classTR, int pageNo)
        {
            AddTextWithWrap(classTR, "Arial", 200, 30, 14, false, 0, Colors.Black, 30, 940, pageNo);
        }
        private static void AddTR2ClassTRComments(string classTRComments, int pageNo)
        {
            AddTextWithWrap(classTRComments, "Arial", 524, 30, 14, false, 0, Colors.Black, 250, 940, pageNo);
        }
        private static void AddTR2Principal(string principal, int pageNo)
        {
            AddTextWithWrap(principal, "Arial", 200, 30, 14, false, 0, Colors.Black, 30, 1010, pageNo);
        }
        private static void AddTR2PrincipalComments(string principalComments, int pageNo)
        {
            AddTextWithWrap(principalComments, "Arial", 524, 30, 14, false, 0, Colors.Black, 250, 1010, pageNo);
        }
        private static void AddTR2Opening(DateTime opening, int pageNo)
        {
            AddText(opening.ToString("dd-MM-yyyy"), "Arial", 14, false, 0, Colors.Black, 350, 1055, pageNo);
        }
        private static void AddTR2Closing(DateTime closing, int pageNo)
        {
            AddText(closing.ToString("dd-MM-yyyy"), "Arial", 14, false, 0, Colors.Black, 120, 1055, pageNo);
        }
        private static void AddTR2StudentID(int studentID, int pageNo)
        {
            AddText(studentID.ToString(), "Arial", 14, false, 0, Colors.Black, 100, 135, pageNo);
        }
        private static void AddTR2Name(string nameOfStudent, int pageNo)
        {
            AddText(nameOfStudent, "Arial", 14, false, 0, Colors.Black, 255, 135, pageNo);
        }
        private static void AddTR2ClassName(string className, int pageNo)
        {
            AddText(className, "Arial", 14, true, 0, Colors.Black, 630, 135, pageNo);
        }
        private static void AddTR2KCPEScore(int kcpeScore, int pageNo)
        {
            AddText(kcpeScore.ToString(), "Arial", 14, true, 0, Colors.Black, 135, 173, pageNo);
        }

        private static void AddTR2ClassPosition(string classPosition, int pageNo)
        {
            AddText(classPosition, "Arial", 14, true, 0, Colors.Black, 75, 580, pageNo);
        }
        private static void AddTR2TotalMarks(decimal totalMarks, int pageNo)
        {
            AddText(totalMarks.ToString("N2"), "Arial", 14, true, 0, Colors.Black, 250, 580, pageNo);
        }
        private static void AddTR2OverAllPosition(string meanGrade, int pageNo)
        {
            AddText(meanGrade, "Arial", 14, true, 0, Colors.Black, 650, 580, pageNo);
        }
        private static void AddTR2MeanGrade(string meanGrade, int pageNo)
        {
            AddText(meanGrade, "Arial", 14, true, 0, Colors.Black, 410, 580, pageNo);
        }
        private static void AddTR2ClustPoints(int point, int pageNo)
        {
            AddText(point.ToString(), "Arial", 14, true, 0, Colors.Black, 495, 580, pageNo);
        }

        private static void AddTR2SubjectScore(StudentExamResultEntryModel item, int itemIndex, int pageNo)
        {
            double fontsize = 14;
            int pageRelativeIndex = itemIndex;
            double yPos = 255 + pageRelativeIndex * 21;

            AddText(item.Code, "Arial", 14, false, 0, Colors.Black, 40, yPos, pageNo);
            AddText(item.NameOfSubject, "Arial", 14, false, 0, Colors.Black, 130, yPos, pageNo);
            AddText(item.Cat1Score.ToString(), "Arial", fontsize, false, 0, Colors.Black, 263, yPos, pageNo);
            AddText(item.Cat2Score.ToString(), "Arial", fontsize, false, 0, Colors.Black, 327, yPos, pageNo);
            AddText(item.ExamScore.ToString(), "Arial", fontsize, false, 0, Colors.Black, 390, yPos, pageNo);
            AddText((item.ExamScore + item.Cat1Score + item.Cat2Score) > 0 ? ((item.ExamScore + item.Cat1Score + item.Cat2Score) / 3).ToString("N0") :
            "0", "Arial", fontsize, false, 0, Colors.Black, 476, yPos, pageNo);
            AddText(item.Grade, "Arial", fontsize, false, 0, Colors.Black, 535, yPos, pageNo);
            AddText(item.Points.ToString(), "Arial", fontsize, false, 0, Colors.Black, 625, yPos, pageNo);
            AddText(item.Tutor, "Arial", fontsize, false, 0, Colors.Black, 705, yPos, pageNo);
        }
        private static void AddTR2SubjectScores(ObservableCollection<StudentExamResultEntryModel> psi, int pageNo)
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

        private static void GenerateTranscript2()
        {
            StudentTranscriptModel si = myWorkObject as StudentTranscriptModel;

            int pageNo;
            for (pageNo = 0; pageNo < noOfPages; pageNo++)
            {
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
                TR2DrawGraph(DataAccess.CalculateGrade(si.KCPEScore / 5),
                    DataAccess.CalculateGrade(si.Entries.Count > 0 ? (si.CAT1Score / si.Entries.Count) : 0),
                    DataAccess.CalculateGrade(si.Entries.Count > 0 ? (si.CAT2Score / si.Entries.Count) : 0),
                    DataAccess.CalculateGrade(si.Entries.Count > 0 ? (si.ExamScore / si.Entries.Count) : 0), pageNo);
            }
        }
        #endregion
    }
}
