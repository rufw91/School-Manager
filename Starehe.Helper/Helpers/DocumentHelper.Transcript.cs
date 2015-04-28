using Helper.Models;
using System.Collections.ObjectModel;
using System.Windows.Media;

namespace Helper
{
    public static partial class DocumentHelper
    {
        #region Transcript

        private static void AddTRStudentID(int studentID, int pageNo)
        {
            AddText(studentID.ToString(), "Arial", 14, false, 0, Colors.Black, 95, 170, pageNo);
        }
        private static void AddTRName(string nameOfStudent, int pageNo)
        {
            AddText(nameOfStudent, "Arial", 14, false, 0, Colors.Black, 247, 170, pageNo);
        }
        private static void AddTRClassName(string className, int pageNo)
        {
            AddText(className, "Arial", 14, true, 0, Colors.Black, 619, 170, pageNo);
        }
        private static void AddTRClassPosition(string classPosition, int pageNo)
        {
            AddText(classPosition, "Arial", 14, true, 0, Colors.Black, 75, 580, pageNo);
        }

        private static void AddTRTotalMarks(decimal totalMarks, int pageNo)
        {
            AddText(totalMarks.ToString(), "Arial", 14, true, 0, Colors.Black, 250, 580, pageNo);
        }
        private static void AddTRPointsPosition(int points, int pageNo)
        {
            AddText(points.ToString(), "Arial", 14, true, 0, Colors.Black, 570, 580, pageNo);
        }
        private static void AddTRMeanGrade(string meanGrade, int pageNo)
        {
            AddText(meanGrade, "Arial", 14, true, 0, Colors.Black, 410, 580, pageNo);
        }
        private static void AddTRExamName(string nameOfExam, int pageNo)
        {
            AddText(nameOfExam, "Arial", 14, true, 0, Colors.Black, 350, 145, pageNo);
        }
        private static void AddTRSubjectScore(StudentTranscriptSubjectModel item, int itemIndex, int pageNo)
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
        private static void AddTRSubjectScores(ObservableCollection<StudentTranscriptSubjectModel> psi, int pageNo)
        {
            for (int i = 0; i <= psi.Count - 1; i++)
                AddTRSubjectScore(psi[i], i, pageNo);
        }
        
        private static void GenerateTranscript()
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
    }
}
