using System.Collections.Generic;

namespace UmanyiSMS.Modules.Exams.Sync.Models
{
    public class SyncTranscriptModel
    {
        public SyncTranscriptModel()
        {
        }

        public string Term1TotalScore { get; set; }

        public string Term2TotalScore { get; set; }

        public string Term3TotalScore { get; set; }

        public string Term1AvgPts { get; set; }

        public string Term2AvgPts { get; set; }

        public string Term3AvgPts { get; set; }

        public string PrevYearAvgPoints { get; set; }

        public string Term1PtsChange { get; set; }

        public string Term2PtsChange { get; set; }

        public string Term3PtsChange { get; set; }

        public string Term1TotalPoints { get; set; }

        public string Term2TotalPoints { get; set; }

        public string Term3TotalPoints { get; set; }

        public string Term1MeanScore { get; set; }

        public string Term2MeanScore { get; set; }

        public string Term3MeanScore { get; set; }

        public string Term1Grade { get; set; }

        public string Term2Grade { get; set; }

        public string Term3Grade { get; set; }

        public string Term1OverallPos { get; set; }

        public string Term2OverallPos { get; set; }

        public string Term3OverallPos { get; set; }

        public List<SyncTranscriptEntryModel> Term2Entries { get; set; }

        public List<SyncTranscriptEntryModel> Term1Entries { get; set; }

        public List<SyncTranscriptEntryModel> Term3Entries { get; set; }

        public List<SyncTranscriptEntryModel> PrevYearEntries { get; set; }

        public string ClassTeacherComments { get; set; }

        public string PrincipalComments { get; set; }

        public string OpeningDay { get; set; }

        public string ClosingDay { get; set; }

        public string MeanScore { get; set; }

        public string Term1Pos { get; set; }

        public string Term2Pos { get; set; }

        public string Term3Pos { get; set; }

        public string MeanGrade { get; set; }

        public string CAT1Grade { get; set; }

        public string CAT2Grade { get; set; }

        public string ExamGrade { get; set; }

        public int KCPEScore { get; set; }
    }
}
