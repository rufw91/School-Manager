﻿using Helper;
using Helper.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace UmanyiSMS.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "Teacher")]
    public class RemoveExamVM:ViewModelBase
    {
        private int selectedClassID;
        private int selectedExamID;
        private TermModel selectedTerm;
        private ObservableCollection<TermModel> allTerms;
        public RemoveExamVM()
        {
            InitVars();
            CreateCommands();
        }

        public int SelectedClassID
        {
            get { return selectedClassID; }
            set
            {
                if (value != this.selectedClassID)
                {
                    this.selectedClassID = value;
                    NotifyPropertyChanged("SelectedClassID");
                }
            }
        }

        public int SelectedExamID
        {
            get { return selectedExamID; }
            set
            {
                if (value != this.selectedExamID)
                {
                    this.selectedExamID = value;
                    NotifyPropertyChanged("SelectedExamID");
                }
            }
        } 

        protected async override void InitVars()
        {
            Title = "REMOVE EXAM";
            AllExams = new ObservableImmutableList<ExamModel>();
            AllClasses = await DataAccess.GetAllClassesAsync();
            AllTerms = await DataAccess.GetAllTermsAsync();
            NotifyPropertyChanged("AllClasses");
            PropertyChanged += async (o, e) =>
                {
                    if (e.PropertyName == "SelectedClassID" || e.PropertyName == "SelectedTerm")
                    {
                        AllExams.Clear();
                        SelectedExamID = 0;
                        if (SelectedClassID != 0&&selectedTerm!=null)
                        {
                            AllExams = new ObservableImmutableList<ExamModel>(await DataAccess.GetExamsByClass(selectedClassID,selectedTerm));
                            NotifyPropertyChanged("AllExams");
                        }
                        return;
                    }
                };
        }

        protected override void CreateCommands()
        {
            RemoveCommand = new RelayCommand(async o =>
            {
                if (MessageBoxResult.Yes==MessageBox.Show("This action is IRREVERSIBLE.\r\nAre you sure you would like to continue?","Information", MessageBoxButton.YesNo, MessageBoxImage.Warning))
                {
                    if (MessageBoxResult.Yes == MessageBox.Show("Are you ABSOLUTELY sure you would like to delete this exam? This will delete the results for all students who took the exam.", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Information))
                    {
                        bool succ = await DataAccess.RemoveExam(selectedExamID);
                        MessageBox.Show(succ ? "Successfully completed operation" : "Operation failed!", succ ? "Success" : "Error", MessageBoxButton.OK, succ ? MessageBoxImage.Information : MessageBoxImage.Error);
                        if (succ)
                            Reset();
                    }
                }
            }, o => CanRemove());
        }

        private bool CanRemove()
        {
            return selectedClassID>0&&selectedExamID > 0;
        }

        public ICommand RemoveCommand
        {
            get;
            private set;
        }

        public override void Reset()
        {
            SelectedClassID = 0;
            SelectedExamID = 0;
        }
        public ObservableCollection<TermModel> AllTerms
        {
            get { return this.allTerms; }

            private set
            {
                if (value != this.allTerms)
                {
                    this.allTerms = value;
                    NotifyPropertyChanged("AllTerms");
                }
            }
        }

        public TermModel SelectedTerm
        {
            get { return this.selectedTerm; }

            set
            {
                if (value != this.selectedTerm)
                {
                    this.selectedTerm = value;
                    NotifyPropertyChanged("SelectedTerm");
                }
            }
        }
        public ObservableCollection<ClassModel> AllClasses { get; private set; }

        public ObservableImmutableList<ExamModel> AllExams { get; set; }
    }
}
