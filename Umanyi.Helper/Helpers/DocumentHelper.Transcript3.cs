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

        #region Transcript3
        private static void AddTR3StudentID(int studentID, int pageNo)
        {
            AddText(studentID.ToString(), "Arial", 14, true, 0, Colors.Black, 135, 270, pageNo);
        }
        private static void AddTR3Name(string nameOfStudent, int pageNo)
        {
            AddText(nameOfStudent, "Arial", 14, true, 0, Colors.Black, 120, 237, pageNo);
        }
        private static void AddTR3ClassName(string className, int pageNo)
        {
            AddText(className, "Arial", 14, true, 0, Colors.Black, 535, 237, pageNo);
        }
        private static void AddTR3KCPEScore(int kcpeScore, int pageNo)
        {
            AddText(kcpeScore.ToString(), "Arial", 14, true, 0, Colors.Black, 580, 270, pageNo);
        }

        private static void AddTR3SubjectScore(StudentExamResultEntryModel item, int itemIndex, int pageNo)
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
        private static void AddTR3SubjectScores(ObservableCollection<StudentExamResultEntryModel> psi, int pageNo)
        {
            for (int i = 0; i <= psi.Count - 1; i++)
                AddTR3SubjectScore(psi[i], i, pageNo);
        }

        private static void AddTR3TotalMarks(decimal totalMarks, int pageNo)
        {
            AddText(totalMarks.ToString("N2"), "Arial", 14, true, 0, Colors.Black, 416, 616, pageNo);
        }
        private static void AddTR3OutOf(decimal outOf, int pageNo)
        {
            AddText(outOf.ToString("N2"), "Arial", 14, true, 0, Colors.Black, 416, 640, pageNo);
        }

        private static void AddTR3Term1TotalScore(string term1TotalScore, int pageNo)
        {
            AddText(term1TotalScore, "Arial", 14, true, 0, Colors.Black, 150, 692, pageNo);
        }
        private static void AddTR3Term2TotalScore(string term2TotalScore,  int pageNo)
        {
            AddText(term2TotalScore, "Arial", 14, true, 0, Colors.Black, 150, 712, pageNo);
        }
        private static void AddTR3Term3TotalScore(string term3TotalScore, int pageNo)
        {
            AddText(term3TotalScore, "Arial", 14, true, 0, Colors.Black, 150, 732, pageNo);
        }
        private static void AddTR3Term1AvgPts(decimal term1Avgpts, int pageNo)
        {
            AddText(term1Avgpts>0? term1Avgpts.ToString("N2"):"-", "Arial", 14, true, 0, Colors.Black, 267, 692, pageNo);
        }
        private static void AddTR3Term2AvgPts(decimal term2Avgpts, int pageNo)
        {
            AddText(term2Avgpts>0?term2Avgpts.ToString("N2"):"-", "Arial", 14, true, 0, Colors.Black, 267, 712, pageNo);
        }
        private static void AddTR3Term3AvgPts(decimal term3Avgpts, int pageNo)
        {
            AddText(term3Avgpts>0?term3Avgpts.ToString("N2"):"-", "Arial", 14, true, 0, Colors.Black, 267, 732, pageNo);
        }
        private static void AddTR3Term1PtsChange(int term1PtsChange, int pageNo)
        {
            if (term1PtsChange >= 0)
                AddText("+" + term1PtsChange.ToString(), "Arial", 14, true, 0, Colors.Black, 320, 692, pageNo);
            else
                AddText("-" + term1PtsChange.ToString(), "Arial", 14, true, 0, Colors.Black, 320, 692, pageNo);
        }
        private static void AddTR3Term2PtsChange(int term2PtsChange, int pageNo)
        {
            if (term2PtsChange >= 0)
                AddText("+"+term2PtsChange.ToString(), "Arial", 14, true, 0, Colors.Black, 320, 712, pageNo);
            else
                AddText("-"+term2PtsChange.ToString(), "Arial", 14, true, 0, Colors.Black, 320, 712, pageNo);
        }
        private static void AddTR3Term3PtsChange(int term3PtsChange, int pageNo)
        {
            if (term3PtsChange >= 0)
                AddText("+" + term3PtsChange.ToString(), "Arial", 14, true, 0, Colors.Black, 320, 732, pageNo);
            else
                AddText("-" + term3PtsChange.ToString(), "Arial", 14, true, 0, Colors.Black, 320, 732, pageNo);
        }
        private static void AddTR3Term1TotalPts(string term1TotalPts, int pageNo)
        {
            AddText(term1TotalPts, "Arial", 14, true, 0, Colors.Black, 380, 692, pageNo);
        }
        private static void AddTR3Term2TotalPts(string term2TotalPts, int pageNo)
        {
            AddText(term2TotalPts, "Arial", 14, true, 0, Colors.Black, 380, 712, pageNo);
        }
        private static void AddTR3Term3TotalPts(string term3TotalPts, int pageNo)
        {
            AddText(term3TotalPts, "Arial", 14, true, 0, Colors.Black, 380, 732, pageNo);
        }
        private static void AddTR3Term1Score(decimal term1Score, int pageNo)
        {
            AddText(term1Score.ToString("N2"), "Arial", 14, true, 0, Colors.Black, 485, 692, pageNo);
        }
        private static void AddTR3Term2Score(decimal term2Score, int pageNo)
        {
            AddText(term2Score.ToString("N2"), "Arial", 14, true, 0, Colors.Black, 485, 712, pageNo);
        }
        private static void AddTR3Term3Score(decimal term3Score, int pageNo)
        {
            AddText(term3Score.ToString("N2"), "Arial", 14, true, 0, Colors.Black, 485, 732, pageNo);
        }
        private static void AddTR3Term1Grade(string term1Grade, int pageNo)
        {
            AddText(term1Grade, "Arial", 14, true, 0, Colors.Black, 584, 692, pageNo);
        }
        private static void AddTR3Term2Grade(string term2Grade, int pageNo)
        {
            AddText(term2Grade, "Arial", 14, true, 0, Colors.Black, 584, 712, pageNo);
        }
        private static void AddTR3Term3Grade(string term3Grade, int pageNo)
        {
            AddText(term3Grade, "Arial", 14, true, 0, Colors.Black, 584, 732, pageNo);
        }
        private static void AddTR3Term1ClassPOS(string term1POS, int pageNo)
        {
            AddText(term1POS, "Arial", 14, true, 0, Colors.Black, 640, 692, pageNo);
        }
        private static void AddTR3Term2ClassPOS(string term2POS, int pageNo)
        {
            AddText(term2POS, "Arial", 14, true, 0, Colors.Black, 640, 712, pageNo);
        }
        private static void AddTR3Term3ClassPOS(string term3POS, int pageNo)
        {
            AddText(term3POS, "Arial", 14, true, 0, Colors.Black, 640, 732, pageNo);
        }
        private static void AddTR3Term1CombinedClassPOS(string term1POS, int pageNo)
        {
            AddText(term1POS, "Arial", 14, true, 0, Colors.Black, 705, 692, pageNo);
        }
        private static void AddTR3Term2CombinedClassPOS(string term1POS, int pageNo)
        {
            AddText(term1POS, "Arial", 14, true, 0, Colors.Black, 705, 712, pageNo);
        }
        private static void AddTR3Term3CombinedClassPOS(string term1POS, int pageNo)
        {
            AddText(term1POS, "Arial", 14, true, 0, Colors.Black, 705, 732, pageNo);
        }
        
        
        private static void AddTR3ClassTRComments(string classTRComments, int pageNo)
        {
            AddTextWithWrap(classTRComments, "Arial", 524, 30, 14, true, 0, Colors.Black, 50, 860, pageNo);
        }        
        private static void AddTR3PrincipalComments(string principalComments, int pageNo)
        {
            AddTextWithWrap(principalComments, "Arial", 445, 100, 14, true, 0, Colors.Black, 50, 945, pageNo);
        }
        private static void AddTR3Opening(DateTime opening, int pageNo)
        {
            AddText(opening.ToString("dd MMM yyyy"), "Arial", 14, true, 0, Colors.Black, 545, 1017, pageNo);
        }
        private static void AddTR3Closing(DateTime closing, int pageNo)
        {
            AddText(closing.ToString("dd MMM yyyy"), "Arial", 14, true, 0, Colors.Black, 545, 950, pageNo);
        }
       
        private static void GenerateTranscript3()
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

                AddTR3SubjectScores(si.Entries, pageNo);

                AddTR3TotalMarks(si.TotalMarks, pageNo);
                AddTR3OutOf(si.Entries.Count*100, pageNo);
                if (si.Term1AvgPts > 0)
                {
                    AddTR3Term1AvgPts(si.Term1AvgPts, pageNo);
                    AddTR3Term1TotalScore(si.Term1TotalScore, pageNo);
                    AddTR3Term1PtsChange(si.Term1PtsChange, pageNo);
                    AddTR3Term1TotalPts(si.Term1TotalPoints, pageNo);
                    AddTR3Term1Score(si.Term1Score, pageNo);
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
                    AddTR3Term2Score(si.Term2Score, pageNo);
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
                    AddTR3Term3Score(si.Term3Score, pageNo);
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
