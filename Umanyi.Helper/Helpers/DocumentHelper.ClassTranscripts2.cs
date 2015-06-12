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
        #region ClassTranscripts2
        private static void GenerateClassTranscripts2()
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
    }
}
