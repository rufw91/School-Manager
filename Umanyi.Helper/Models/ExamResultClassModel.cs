using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;

namespace Helper.Models
{
    public class ExamResultClassModel : ExamResultBaseModel
    {
        private string nameOfClass;
        private int classID;
        ObservableCollection<ExamResultStudentModel> entries;
        private DataTable resultTable;

        public ExamResultClassModel()
        {
            ClassID = 0;
            NameOfClass = "";
            Entries = new ObservableCollection<ExamResultStudentModel>();
            ResultTable = new DataTable();
        }

        public ObservableCollection<ExamResultStudentModel> Entries
        {
            get { return entries; }

            set
            {
                if (value != this.entries)
                {
                    this.entries = value;
                    NotifyPropertyChanged("Entries");
                }
            }
        }

        public int ClassID
        {
            get { return this.classID; }

            set
            {
                if (value != this.classID)
                {
                    this.classID = value;
                    NotifyPropertyChanged("ClassID");
                }
            }
        }
        public string NameOfClass
        {
            get { return this.nameOfClass; }

            set
            {
                if (value != this.nameOfClass)
                {
                    this.nameOfClass = value;
                    NotifyPropertyChanged("NameOfClass");
                }
            }
        }

        public DataTable ResultTable
        {
            get { return this.resultTable; }

            set
            {
                if (value != this.resultTable)
                {
                    this.resultTable = value;
                    NotifyPropertyChanged("ResultTable");
                }
            }
        }

        public override void Reset()
        {
            base.Reset();
            ClassID = 0;
            NameOfClass = "";
            Entries = new ObservableCollection<ExamResultStudentModel>();
            ResultTable = new DataTable();
        }

    }
}

