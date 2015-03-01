﻿using Helper;
using Helper.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Input;

namespace Starehe.ViewModels
{
    public class AggregateResultsVM: ViewModelBase
    {
        private FixedDocument fd;
        private ObservableCollection<ExamModel> allExams;
        private ExamModel selectedExam;
        private ClassModel selectedClass;
        
        public AggregateResultsVM()
        {
            InitVars();
            CreateCommands();
            PropertyChanged += async (o, e) =>
                {
                    if ((e.PropertyName == "SelectedClass") && (selectedClass != null) && (selectedClass.ClassID > 0))
                    {
                        AllExams = await DataAccess.GetExamsByClass(selectedClass.ClassID);
                    }
                };
        }
        protected async override void InitVars()
        {
            Title = "AGGREGATE RESULTS";
            AllExams = new ObservableCollection<ExamModel>();
            AllClasses = await DataAccess.GetAllClassesAsync();
            NotifyPropertyChanged("AllClasses");
        }

        protected override void CreateCommands()
        {
            GenerateCommand = new RelayCommand(async o =>
            {
                AggregateResultModel fs = await DataAccess.GetAggregateResultAsync(selectedClass, selectedExam);
                Document = DocumentHelper.GenerateDocument(fs);
            },
                  o => CanGenerate());
        }

        private bool CanGenerate()
        {
            return selectedClass!=null && selectedExam!=null&&selectedClass.ClassID>0&&selectedExam.ExamID>0;
        }
        public ExamModel SelectedExam
        {
            get { return selectedExam; }

            set
            {
                if (value != selectedExam)
                {
                    selectedExam = value;
                    NotifyPropertyChanged("SelectedExam");
                }
            }
        }
        public ClassModel SelectedClass
        {
            get { return selectedClass; }

            set
            {
                if (value != selectedClass)
                {
                    selectedClass = value;
                    NotifyPropertyChanged("SelectedClass");
                    
                }
            }
        }
        public ObservableCollection<ExamModel> AllExams
        {
            get { return allExams; }

            private set
            {
                if (value != allExams)
                {
                    allExams = value;
                    NotifyPropertyChanged("AllExams");
                }
            }
        }
        public FixedDocument Document
        {
            get { return this.fd; }

            private set
            {
                if (value != this.fd)
                {
                    this.fd = value;
                    NotifyPropertyChanged("Document");
                }
            }
        }
        public ObservableCollection<ClassModel> AllClasses { get; private set; }
        public ICommand GenerateCommand
        {
            get;
            private set;
        }
        public override void Reset()
        {
        }
    }
}
