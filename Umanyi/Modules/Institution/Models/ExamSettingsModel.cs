
using Helper.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UmanyiSMS.Lib;
using UmanyiSMS.Lib.Presentation;

namespace UmanyiSMS.Modules.Institution.Models
{
    public class ExamSettingsModel: ModelBase
    {
        ObservableCollection<BasicPair<int, int>> gradeRanges;
        ObservableCollection<string> gradeRemarks;
        private int best7Subjects;
        private int meanGradeCalculation;

        public ExamSettingsModel()
        {
            gradeRanges = new ObservableCollection<BasicPair<int,int>>();
            gradeRemarks = new ObservableCollection<string>();
            Best7Subjects = 3;
            MeanGradeCalculation = 1;
            gradeRanges.Add(new BasicPair<int, int>(85, 100));
            gradeRanges.Add(new BasicPair<int, int>(80, 84));
            gradeRanges.Add(new BasicPair<int, int>(75, 79));
            gradeRanges.Add(new BasicPair<int, int>(70, 74));
            gradeRanges.Add(new BasicPair<int, int>(65, 69));
            gradeRanges.Add(new BasicPair<int, int>(60, 64));
            gradeRanges.Add(new BasicPair<int, int>(55, 59));
            gradeRanges.Add(new BasicPair<int, int>(50, 54));
            gradeRanges.Add(new BasicPair<int, int>(45, 49));
            gradeRanges.Add(new BasicPair<int, int>(40, 44));
            gradeRanges.Add(new BasicPair<int, int>(35, 39));
            gradeRanges.Add(new BasicPair<int, int>(0, 34));

            gradeRemarks.Add("EXCELLENT");
            gradeRemarks.Add("VERY GOOD");
            gradeRemarks.Add("VERY GOOD");
            gradeRemarks.Add("GOOD");
            gradeRemarks.Add("ABOVE AVERAGE");
            gradeRemarks.Add("AVERAGE");
            gradeRemarks.Add("AVERAGE");
            gradeRemarks.Add("FAIR");
            gradeRemarks.Add("BELOW AVERAGE");
            gradeRemarks.Add("POOR");
            gradeRemarks.Add("VERY POOR");
            gradeRemarks.Add("WAKE UP");
            /*
            else
            {
                switch (num)
                {
                    case 1:
                        result = "ZINDUKA";
                        return result;
                    case 2:
                        result = "PUNGUZA MZAHA";
                        return result;
                    case 3:
                        result = "AMKA";
                        return result;
                    case 4:
                        result = "TIA BIDII";
                        return result;
                    case 5:
                        result = "TIA BIDII";
                        return result;
                    case 6:
                        result = "CHINI YA WASTANI";
                        return result;
                    case 7:
                        result = "WASTANI";
                        return result;
                    case 8:
                        result = "HEKO";
                        return result;
                    case 9:
                        result = "VIZURI";
                        return result;
                    case 10:
                        result = "VIZURI SANA";
                        return result;
                    case 11:
                        result = "PONGEZI";
                        return result;
                    case 12:
                        result = "HONGERA";
                        return result;/*/
        }

        public ObservableCollection<BasicPair<int, int>> GradeRanges
        {
            get { return gradeRanges; }
        }

        public ObservableCollection<string> GradeRemarks
        {
            get { return gradeRemarks; }
        }

        /// <summary>
        /// Select best 7 subjects
        /// 0 - Form 4 only,
        /// 1 - Form 3 and Form 4,
        /// 2 - All Classes,
        /// 3 - None,
        /// </summary>
        public int Best7Subjects
        {
            get { return best7Subjects; }
            set
            {
                if (value != this.best7Subjects)
                {
                    this.best7Subjects = value;
                    NotifyPropertyChanged("Best7Subjects");
                }
            }
        }

        /// <summary>
        /// Mean grade calculation
        /// 0 - Calculate using points
        /// 1 - Calculate using mean score
        /// </summary>

        public int MeanGradeCalculation
        {
            get { return meanGradeCalculation; }
            set
            {
                if (value != this.meanGradeCalculation)
                {
                    this.meanGradeCalculation = value;
                    NotifyPropertyChanged("MeanGradeCalculation");
                }
            }
        }

        public void CopyFrom(ExamSettingsModel source)
        {
            Reset();
            Best7Subjects = source.Best7Subjects;
            MeanGradeCalculation = source.MeanGradeCalculation;
            foreach (var v in new List<BasicPair<int,int>>(source.GradeRanges))
                gradeRanges.Add(v);
            foreach (var v in new List<string>(source.GradeRemarks))
                gradeRemarks.Add(v);
        }

        public override void Reset()
        {
            gradeRanges.Clear();
            gradeRemarks.Clear();
            Best7Subjects = 3;
            MeanGradeCalculation = 1;
        }
    }
}
