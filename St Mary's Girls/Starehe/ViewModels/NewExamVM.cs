using Helper;
using Helper.Models;
using System;
using System.Collections.ObjectModel;
using System.Security.Permissions;
using System.Windows.Input;

namespace Starehe.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "Deputy")]
    public class NewExamVM: ViewModelBase
    {
        int selectedClassID;
        ObservableCollection<ClassModel> allClasses;
        ExamModel newExam;
        public NewExamVM()
        {
            InitVars();
            CreateCommands();
        }
        protected override void CreateCommands()
        {
            RefreshCommand = new RelayCommand(o => { RefreshEntries(); }, o => selectedClassID > 0);
            SaveCommand = new RelayCommand(async o =>
            {                
              bool succ = await DataAccess.SaveNewExamAsync(newExam);
              if (succ)
                  Reset();
            },
            o =>
            {
                return SelectedClassID > 0 &&
                    !string.IsNullOrWhiteSpace(newExam.NameOfExam);
            });
        }

        protected async override void InitVars()
        {
            Title = "NEW EXAM";
            SelectedClassID=0;
            NewExam = new ExamModel();
            AllClasses = await DataAccess.GetAllClassesAsync();
        }

        public ObservableCollection<ClassModel> AllClasses
        {
            get { return allClasses; }
            set
            {
                if (allClasses != value)
                {
                    allClasses = value;
                    NotifyPropertyChanged("AllClasses");
                }
            }
        }

        public int SelectedClassID
        {
            get { return selectedClassID; }
            set
            {
                if (selectedClassID != value)
                {
                    selectedClassID = value;
                    newExam.ClassID = selectedClassID;   
                    RefreshEntries();
                }
            }
        }

        private async void RefreshEntries()
        {
            newExam.Entries.Clear();
            if (newExam.ClassID <= 0)
                return;
            var temp =
                await DataAccess.GetSubjectsRegistredToClassAsync(newExam.ClassID);
            foreach (SubjectModel sm in temp)
                newExam.Entries.Add(new ExamSubjectEntryModel(sm)); 
        }

        public ExamModel NewExam
        {
            get { return newExam; }
            private set
            {
                if (newExam != value)
                {
                    newExam = value;
                    NotifyPropertyChanged("NewExam");
                }
            }
        }

        public ICommand SaveCommand
        {
            get;
            private set;
        }

        public ICommand RefreshCommand
        {
            get;
            private set;
        }

        public async override void Reset()
        {
            SelectedClassID = 0;
            NewExam = new ExamModel();
            AllClasses = await DataAccess.GetAllClassesAsync();  
        }
    }
}
