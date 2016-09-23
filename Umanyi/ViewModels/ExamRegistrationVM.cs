using Helper;
using Helper.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace UmanyiSMS.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "Teacher")]
    public class ExamRegistrationVM : ViewModelBase
    {
        bool isInStudentMode;
        bool isInClassMode;
        bool isInCombinedMode;
        private ObservableImmutableList<ExamModel> allExams;
        private ExamRegistrationStudentModel selectedStudentRegistration;
        private int selectedClassID;
        private int selectedExamID;
        private CombinedClassModel selectedCombinedClass;

        public ExamRegistrationVM()
        {
            InitVars();
            CreateCommands();
        }
        public override void Reset()
        {
            selectedStudentRegistration.Reset();
            SelectedExamID = 0;
            SelectedCombinedClass = null;
            SelectedClassID = 0;
        }

        protected override void CreateCommands()
        {
            SaveCommand = new RelayCommand(async o =>
            {
                IsBusy = true;
                if (isInStudentMode)
                {

                    IsBusy = true;
                    bool succ = await DataAccess.SaveNewExamRegistrationAsync(selectedStudentRegistration);
                    MessageBox.Show(succ ? "Successfully saved details" : "Could not save details.", succ ? "Success" : "Error",
                                MessageBoxButton.OK, succ ? MessageBoxImage.Information : MessageBoxImage.Warning);
                    if (succ)
                        Reset();
                    IsBusy = false;
                }

                if (isInClassMode)
                {
                    IsBusy = true;
                    bool succ = await DataAccess.SaveNewClassExamRegistrationAsync(selectedClassID, selectedExamID);
                    MessageBox.Show(succ ? "Successfully saved details" : "Could not save details.", succ ? "Success" : "Error",
                                MessageBoxButton.OK, succ ? MessageBoxImage.Information : MessageBoxImage.Warning);
                    if (succ)
                        Reset();
                    IsBusy = false;
                }

                if (isInCombinedMode)
                {
                    IsBusy = true;
                    bool succ = await DataAccess.SaveNewCombinedClassExamRegistrationAsync(selectedCombinedClass, selectedExamID);
                    MessageBox.Show(succ ? "Successfully saved details" : "Could not save details.", succ ? "Success" : "Error",
                                MessageBoxButton.OK, succ ? MessageBoxImage.Information : MessageBoxImage.Warning);
                    if (succ)
                        Reset();
                    IsBusy = false;
                }
            }, o => CanSave());
        }

        private bool CanSave()
        {
            if (isInStudentMode)
            {
               selectedStudentRegistration.CheckErrors();
                return  selectedExamID > 0 &&
                      !selectedStudentRegistration.HasErrors && !IsBusy;
            }
            if (isInClassMode)
                return selectedExamID > 0 && selectedClassID > 0 && !IsBusy;
            if (isInCombinedMode)
                return selectedCombinedClass != null && selectedCombinedClass.Entries.Count > 0 &&
                   selectedExamID > 0 && !IsBusy;

            return false;
        }

        protected async override void InitVars()
        {
            Title = "EXAM REGISTRATION";
            allExams = new ObservableImmutableList<ExamModel>();
            selectedStudentRegistration = new ExamRegistrationStudentModel();
            selectedClassID = 0;
            selectedCombinedClass = new CombinedClassModel();
            selectedClassID = 0;
            IsInStudentMode = true;
            selectedStudentRegistration.PropertyChanged += async (o, e) =>
            {
                if (isInStudentMode)
                {
                    if (e.PropertyName == "StudentID")
                    {
                        selectedStudentRegistration.CheckErrors();

                        if ((selectedStudentRegistration.StudentID > 0) && (!selectedStudentRegistration.HasErrors))
                        { SelectedExamID = 0; await RefreshAllExams(); }
                        else {
                            SelectedExamID = 0;
                            allExams.Clear();
                        }
                    }
                    if (e.PropertyName == "ExamID")
                    {
                        selectedStudentRegistration.CheckErrors();
                    }
                }

            };

            this.PropertyChanged += OnPropertyChanged;

            AllClasses = await DataAccess.GetAllClassesAsync();
            NotifyPropertyChanged("AllClasses");
            AllCombinedClasses = await DataAccess.GetAllCombinedClassesAsync();
            NotifyPropertyChanged("AllCombinedClasses");

        }

        private async void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {

            if (isInClassMode)
                if (e.PropertyName == "SelectedClassID")              
                    await RefreshAllExams();
                

            if (e.PropertyName == "SelectedCombinedClass")
                if ((selectedCombinedClass != null) && (selectedCombinedClass.Entries.Count > 0))
                    await RefreshAllExams();
        }


        public int SelectedClassID
        {
            get { return selectedClassID; }

            set
            {
                if (value != selectedClassID)
                {
                    selectedClassID = value;
                    NotifyPropertyChanged("SelectedClassID");
                }
            }
        }

        public int SelectedExamID
        {
            get { return selectedExamID; }

            set
            {
                if (value != selectedExamID)
                {
                    selectedExamID = value;
                    selectedStudentRegistration.ExamID = value;
                    NotifyPropertyChanged("SelectedExamID");
                }
            }
        }

        public CombinedClassModel SelectedCombinedClass
        {
            get { return selectedCombinedClass; }

            set
            {
                if (value != selectedCombinedClass)
                {
                    selectedCombinedClass = value;
                    NotifyPropertyChanged("SelectedCombinedClass");
                }
            }
        }

        public ExamRegistrationStudentModel SelectedStudentRegistration
        {
            get { return selectedStudentRegistration; }

            set
            {
                if (value != selectedStudentRegistration)
                {
                    selectedStudentRegistration = value;
                    NotifyPropertyChanged("SelectedStudentRegistration");
                }
            }
        }

        public bool IsInStudentMode
        {
            get { return isInStudentMode; }

            set
            {
                if (value != isInStudentMode)
                {
                    isInStudentMode = value;
                    NotifyPropertyChanged("IsInStudentMode");
                    selectedStudentRegistration.Reset();
                    SelectedExamID = 0;
                    allExams.Clear();
                }
            }
        }

        public ObservableImmutableList<ExamModel> AllExams
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

        public bool IsInClassMode
        {
            get { return isInClassMode; }

            set
            {
                if (value != isInClassMode)
                {
                    isInClassMode = value;
                    NotifyPropertyChanged("IsInClassMode");
                    SelectedClassID = 0;
                    SelectedExamID = 0;
                    allExams.Clear();
                }
            }
        }

        public bool IsInCombinedMode
        {
            get { return isInCombinedMode; }

            set
            {
                if (value != isInCombinedMode)
                {
                    isInCombinedMode = value;
                    NotifyPropertyChanged("IsInCombinedMode");
                    SelectedExamID = 0;
                    allExams.Clear();
                }
            }
        }
        
        private async Task RefreshAllExams()
        {
            if (isInStudentMode)
            {
                if (selectedStudentRegistration.StudentID == 0)
                    return;
                int classID = await DataAccess.GetClassIDFromStudentID(selectedStudentRegistration.StudentID);
                AllExams = new ObservableImmutableList<ExamModel>(await DataAccess.GetExamsByClass(classID,null));
                return;
            }
            if (isInClassMode)
            {
                if (SelectedClassID == 0)
                    return;
                AllExams = new ObservableImmutableList<ExamModel>(await DataAccess.GetExamsByClass(selectedClassID,null));
                return;
            }
            if (isInCombinedMode)
            {
                AllExams = new ObservableImmutableList<ExamModel>(await DataAccess.GetExamsByClass(selectedCombinedClass.Entries[0].ClassID,null));
                return;
            }
        }

        public ICommand SaveCommand
        {
            get;
            private set;
        }

        public ObservableCollection<ClassModel> AllClasses { get; private set; }

        public ObservableCollection<CombinedClassModel> AllCombinedClasses { get; private set; }
    }
}
