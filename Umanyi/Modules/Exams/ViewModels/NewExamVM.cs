

using System;
using System.Collections.ObjectModel;
using System.Security.Permissions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using UmanyiSMS.Lib;
using UmanyiSMS.Lib.Presentation;
using UmanyiSMS.Modules.Exams.Models;
using UmanyiSMS.Modules.Exams.Controller;
using UmanyiSMS.Modules.Institution.Models;

namespace UmanyiSMS.Modules.Exams.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "Deputy")]
    public class NewExamVM : ViewModelBase
    {
        CombinedClassModel selectedCombinedClass;
        ObservableCollection<CombinedClassModel> allCombinedClasses;
        ExamModel newExam;
        public NewExamVM()
        {
            InitVars();
            CreateCommands();
        }
        protected override void CreateCommands()
        {
            RefreshCommand = new RelayCommand(async o => { await RefreshEntries(); }, o => selectedCombinedClass !=null);
            SaveCommand = new RelayCommand(async o =>
            {
                IsBusy = true;
                bool succ = await DataController.SaveNewExamAsync(newExam);

                MessageBox.Show(succ ? "Successfully saved details." : "Could not save details.", succ ? "Success" : "Error", MessageBoxButton.OK, 
                    succ ? MessageBoxImage.Information : MessageBoxImage.Warning);
                if (succ)
                    Reset();
                IsBusy = false;
            },
            o =>
            {
                return selectedCombinedClass != null && selectedCombinedClass.Entries.Count > 0&&
                    !string.IsNullOrWhiteSpace(newExam.NameOfExam) && newExam.Entries.Count > 0;
            });
        }

        protected async override void InitVars()
        {
            Title = "NEW EXAM";
            SelectedCombinedClass =null;
            NewExam = new ExamModel();
            AllCombinedClasses = await Institution.Controller.DataController.GetAllCombinedClassesAsync();
            PropertyChanged +=async (o, e) =>
                {
                    if (e.PropertyName=="SelectedCombinedClass")
                    {
                        if (selectedCombinedClass != null)
                        {
                            newExam.Classes = selectedCombinedClass.Entries;
                            await RefreshEntries();
                        }
                        else
                            newExam.Classes.Clear();
                    }
                };
        }

        public ObservableCollection<CombinedClassModel> AllCombinedClasses
        {
            get { return allCombinedClasses; }
            set
            {
                if (allCombinedClasses != value)
                {
                    allCombinedClasses = value;
                    NotifyPropertyChanged("AllCombinedClasses");
                }
            }
        }

        public CombinedClassModel SelectedCombinedClass
        {
            get { return selectedCombinedClass; }
            set
            {
                if (selectedCombinedClass != value)
                {
                    selectedCombinedClass = value;
                    NotifyPropertyChanged("SelectedCombinedClass");
                }
            }
        }
        private void SetExamClasses()
        {
            foreach (var c in selectedCombinedClass.Entries)
                newExam.Classes.Add(c);
        }
        private async Task RefreshEntries()
        {
            newExam.Entries.Clear();
            if (newExam.Classes.Count== 0)
                return;
            var temp =
                await Institution.Controller.DataController.GetInstitutionSubjectsAsync();
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
            SelectedCombinedClass = null;
            NewExam = new ExamModel();
            AllCombinedClasses = await Institution.Controller.DataController.GetAllCombinedClassesAsync();
        }
    }
}
