using Helper.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helper
{
    public static partial class DocumentHelper
    {
        #region ClassExamResults
        private static void GenerateClassExamResults()
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
    }
}
