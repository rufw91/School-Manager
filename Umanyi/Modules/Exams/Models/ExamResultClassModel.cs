using System;
using System.Collections.ObjectModel;
using System.Data;

namespace UmanyiSMS.Modules.Exams.Models
{
    public class ExamResultClassModel : ExamResultBaseModel
    {
        private string nameOfClass;

        private int classID;

        private ObservableCollection<ExamResultStudentModel> entries;

        private DataTable resultTable;

        public ObservableCollection<ExamResultStudentModel> Entries
        {
            get
            {
                return this.entries;
            }
            set
            {
                if (value != this.entries)
                {
                    this.entries = value;
                    base.NotifyPropertyChanged("Entries");
                }
            }
        }

        public int ClassID
        {
            get
            {
                return this.classID;
            }
            set
            {
                if (value != this.classID)
                {
                    this.classID = value;
                    base.NotifyPropertyChanged("ClassID");
                }
            }
        }

        public string NameOfClass
        {
            get
            {
                return this.nameOfClass;
            }
            set
            {
                if (value != this.nameOfClass)
                {
                    this.nameOfClass = value;
                    base.NotifyPropertyChanged("NameOfClass");
                }
            }
        }

        public DataTable ResultTable
        {
            get
            {
                return this.resultTable;
            }
            set
            {
                if (value != this.resultTable)
                {
                    this.resultTable = value;
                    base.NotifyPropertyChanged("ResultTable");
                }
            }
        }

        public ExamResultClassModel()
        {
            this.ClassID = 0;
            this.NameOfClass = "";
            this.Entries = new ObservableCollection<ExamResultStudentModel>();
            this.ResultTable = new DataTable();
        }

        public override void Reset()
        {
            base.Reset();
            this.ClassID = 0;
            this.NameOfClass = "";
            this.Entries = new ObservableCollection<ExamResultStudentModel>();
            this.ResultTable = new DataTable();
        }
    }
}
