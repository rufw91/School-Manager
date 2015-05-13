using Helper.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helper
{
    public static partial class DocumentHelper
    {
        #region ClassTranscripts
        private static void GenerateClassTranscripts()
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
                TR2DrawGraph(DataAccess.CalculateGrade(decimal.Ceiling(Convert.ToDecimal(si.Entries[pageNo].KCPEScore) / 5m)),
                   si.Entries[pageNo].CAT1Grade,
                   si.Entries[pageNo].CAT2Grade,
                   si.Entries[pageNo].ExamGrade,
                   pageNo);
            }
        }
        #endregion
    }
}
