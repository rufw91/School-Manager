using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Permissions;
using System.Windows;
using System.Windows.Input;
using UmanyiSMS.Lib;
using UmanyiSMS.Modules.Institution.Models;
using UmanyiSMS.Modules.Students.Models;
using UmanyiSMS.Modules.Students.Controller;
using UmanyiSMS.Lib.Presentation;

namespace UmanyiSMS.Modules.Students.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "Teacher")]
    public class SubjectSelectionVM:ViewModelBase
    {
        private StudentSelectModel selectedStudent;
        private SubjectModel selectedSubject;        
        private StudentSubjectSelectionModel studentSubjectSelection;
        public SubjectSelectionVM()
        {
            InitVars();
            CreateCommands();
        }
        protected override void InitVars()
        {
            Title = "SUBJECT SELECTION";
            SelectedStudent = new StudentSelectModel();
            studentSubjectSelection = new StudentSubjectSelectionModel();
            AllSubjects = new ObservableCollection<SubjectModel>();
            selectedStudent.PropertyChanged += async (o, e) =>
            {
                
                if (e.PropertyName == "StudentID")
                {
                    AllSubjects.Clear();
                    selectedStudent.CheckErrors();
                    studentSubjectSelection.Reset();
                    studentSubjectSelection.StudentID = selectedStudent.StudentID;
                    studentSubjectSelection.NameOfStudent = selectedStudent.NameOfStudent;
                    if (!selectedStudent.HasErrors)
                    {
                        studentSubjectSelection.Entries = (await DataController.GetStudentSubjectSelection(studentSubjectSelection.StudentID)).Entries;
                        AllSubjects = await DataController.GetSubjectsRegistredToClassAsync(await DataController.GetClassIDFromStudentID(studentSubjectSelection.StudentID));
                        NotifyPropertyChanged("AllSubjects");
                    }

                }
            };
            
        }

        protected override void CreateCommands()
        {
            AddNewSubjectCommand = new RelayCommand(o =>
            {
                studentSubjectSelection.Entries.Add(new StudentSubjectSelectionEntryModel(selectedSubject));
            }, o => CanAdd());

            RemoveSubjectCommand = new RelayCommand(o =>
            {
                studentSubjectSelection.Entries.Remove(CurrentSubject);
            }, o => CurrentSubject != null);

            SaveCommand = new RelayCommand(async o =>
                {
                    IsBusy = true;
                    bool succ = await DataController.SaveNewSubjectSelection(studentSubjectSelection);
                    MessageBox.Show(succ ? "Successfully saved details." : "An error occurred. Could not save details.", succ ? "Success" : "Error",
                        MessageBoxButton.OK, succ ? MessageBoxImage.Information : MessageBoxImage.Warning);
                    if (succ)
                        Reset();
                    IsBusy = false;
                }, o => CanSave());
        }
        private bool CanSave()
        {
            return studentSubjectSelection.StudentID > 0 && !IsBusy && (studentSubjectSelection.Entries.Count > 0);
        }
        private bool CanAdd()
        {
            return selectedSubject != null && !studentSubjectSelection.Entries.Any(o => (o.NameOfSubject == selectedSubject.NameOfSubject) && 
                (o.SubjectID == selectedSubject.SubjectID));
        }

        public StudentSelectModel SelectedStudent
        {
            get { return selectedStudent; }
            set
            {
                if (selectedStudent != value)
                {
                    selectedStudent = value;
                    NotifyPropertyChanged("SelectedStudent");
                }

            }
        }

        public StudentSubjectSelectionModel StudentSubjectSelection
        {
            get { return studentSubjectSelection; }
            set
            {
                if (studentSubjectSelection != value)
                {
                    studentSubjectSelection = value;
                    NotifyPropertyChanged("StudentSubjectSelection");
                }

            }
        }

        public SubjectModel SelectedSubject
        {
            get { return selectedSubject; }
            set
            {
                if (selectedSubject != value)
                {
                    selectedSubject = value;
                    NotifyPropertyChanged("SelectedSubject");
                }

            }
        }

        public ObservableCollection<SubjectModel> AllSubjects { get; set; }

        public StudentSubjectSelectionEntryModel CurrentSubject { get; set; }

        public ICommand SaveCommand
        { get; private set; }

        public ICommand AddNewSubjectCommand
        { get; private set; }

        public ICommand RemoveSubjectCommand
        { get; private set; }
        
        public override void Reset()
        {
            selectedStudent.Reset();
            StudentSubjectSelection.Reset();
        }
    }
}
