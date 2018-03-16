

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
        ObservableCollection<string> swahiliGradeRemarks;
        private int best7Subjects;
        private int meanGradeCalculation;

        public ExamSettingsModel()
        {
            gradeRanges = new ObservableCollection<BasicPair<int,int>>();
            gradeRemarks = new ObservableCollection<string>();
            swahiliGradeRemarks = new ObservableCollection<string>();
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

            swahiliGradeRemarks.Add("HONGERA");
            swahiliGradeRemarks.Add("HONGERA");
            swahiliGradeRemarks.Add("VIZURI SANA");
            swahiliGradeRemarks.Add("VIZURI");
            swahiliGradeRemarks.Add("VIZURI");
            swahiliGradeRemarks.Add("WASTANI");
            swahiliGradeRemarks.Add("CHINI YA WASTANI");
            swahiliGradeRemarks.Add("TIA BIDII");
            swahiliGradeRemarks.Add("TIA BIDII");
            swahiliGradeRemarks.Add("AMKA");
            swahiliGradeRemarks.Add("PUNGUZA MZAHA");
            swahiliGradeRemarks.Add("ZINDUKA");
          
        }

        public ObservableCollection<BasicPair<int, int>> GradeRanges
        {
            get { return gradeRanges; }
        }

        public ObservableCollection<string> GradeRemarks
        {
            get { return gradeRemarks; }
        }

        public ObservableCollection<string> SwahiliGradeRemarks
        {
            get { return swahiliGradeRemarks; }
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
