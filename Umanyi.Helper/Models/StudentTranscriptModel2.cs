using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helper.Models
{
    public class StudentTranscriptModel2 : StudentTranscriptModel
    {
        public StudentTranscriptModel2()
        {
            Term1TotalScore = "";
            Term2TotalScore = "";
            Term3TotalScore = "";
            Term1AvgPts = 0m;
            Term2AvgPts = 0m;
            Term3AvgPts = 0m;

            Term1PtsChange = 0;
            Term2PtsChange = 0;
            Term3PtsChange = 0;

            Term1TotalPoints = "";
            Term2TotalPoints = "";
            Term3TotalPoints = "";
            Term1Score = 0;
            Term2Score = 0;
            Term3Score = 0;

            Term1Grade = "";
            Term2Grade = "";
            Term3Grade = "";

            Term1OverallPos = "";
            Term2OverallPos = "";
            Term3OverallPos = "";
        }
        public string Term1TotalScore { get; set; }

        public string Term2TotalScore { get; set; }

        public string Term3TotalScore { get; set; }

        public decimal Term3AvgPts { get; set; }

        public decimal Term2AvgPts { get; set; }

        public decimal Term1AvgPts { get; set; }

        public int Term1PtsChange { get; set; }

        public int Term2PtsChange { get; set; }

        public int Term3PtsChange { get; set; }

        public string Term1TotalPoints { get; set; }

        public string Term3TotalPoints { get; set; }

        public string Term2TotalPoints { get; set; }

        public decimal Term2Score { get; set; }

        public decimal Term3Score { get; set; }

        public decimal Term1Score { get; set; }

        public string Term2Grade { get; set; }

        public string Term3Grade { get; set; }

        public string Term1Grade { get; set; }

        public string Term1OverallPos { get; set; }

        public string Term2OverallPos { get; set; }

        public string Term3OverallPos { get; set; }

        public void CopyFrom(StudentTranscriptModel2 newTranscript)
        {
            base.CopyFrom(newTranscript);
            Term1TotalScore = newTranscript.Term1TotalScore;
            Term2TotalScore = newTranscript.Term2TotalScore;
            Term3TotalScore = newTranscript.Term3TotalScore;
            Term1AvgPts = newTranscript.Term1AvgPts;
            Term2AvgPts = newTranscript.Term2AvgPts;
            Term3AvgPts = newTranscript.Term3AvgPts;

            Term1PtsChange = newTranscript.Term1PtsChange;
            Term2PtsChange = newTranscript.Term2PtsChange;
            Term3PtsChange = newTranscript.Term3PtsChange;

            Term1TotalPoints = newTranscript.Term1TotalPoints;
            Term2TotalPoints = newTranscript.Term2TotalPoints;
            Term3TotalPoints = newTranscript.Term3TotalPoints;
            Term1Score = newTranscript.Term1Score;
            Term2Score = newTranscript.Term2Score;
            Term3Score = newTranscript.Term3Score;

            Term1Grade = newTranscript.Term1Grade;
            Term2Grade = newTranscript.Term2Grade;
            Term3Grade = newTranscript.Term3Grade;

            Term1OverallPos = newTranscript.Term1OverallPos;
            Term2OverallPos = newTranscript.Term2OverallPos;
            Term3OverallPos = newTranscript.Term3OverallPos;
        }

        public override void Clean()
        {
            base.Clean();
            Term1TotalScore = "";
            Term2TotalScore = "";
            Term3TotalScore = "";
            Term1AvgPts = 0m;
            Term2AvgPts = 0m;
            Term3AvgPts = 0m;

            Term1PtsChange = 0;
            Term2PtsChange = 0;
            Term3PtsChange = 0;

            Term1TotalPoints = "";
            Term2TotalPoints = "";
            Term3TotalPoints = ""; 
            Term1Score = 0;
            Term2Score = 0;
            Term3Score = 0;

            Term1Grade = "";
            Term2Grade = "";
            Term3Grade = "";

            Term1OverallPos = "";
            Term2OverallPos = "";
            Term3OverallPos = "";
        }

        
    }
}
