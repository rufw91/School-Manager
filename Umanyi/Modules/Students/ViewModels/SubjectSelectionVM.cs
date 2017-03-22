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
        private StudentSubjectSelectionModel selectedStudent; 
        public SubjectSelectionVM()
        {
            InitVars();
            CreateCommands();
        }
        protected async override void InitVars()
        {
            Title = "SUBJECT SELECTION";
            selectedStudent = new StudentSubjectSelectionModel();
            AllSubjects = new ObservableCollection<StudentSubjectSelectionEntryModel>();
            var ass= await Institution.Controller.DataController.GetInstitutionSubjectsAsync();
            foreach (var s in ass)
                AllSubjects.Add(new StudentSubjectSelectionEntryModel(s));
            selectedStudent.PropertyChanged += async (o, e) =>
            {
                
                if (e.PropertyName == "StudentID")
                {
                    foreach (var s in AllSubjects)
                        s.IsSelected = false;
                        selectedStudent.CheckErrors();
                    if (!selectedStudent.HasErrors)
                    {
                        var ty = (await DataController.GetStudentSubjectSelection(selectedStudent.StudentID)).Entries;
                        foreach (var s in ty)
                            AllSubjects.First(x => x.SubjectID == s.SubjectID).IsSelected = true;                        
                    }

                }
            };
            
        }

        protected override void CreateCommands()
        {
            SaveCommand = new RelayCommand(async o =>
                {
                    IsBusy = true;
                    selectedStudent.Entries = new ObservableCollection<StudentSubjectSelectionEntryModel>(AllSubjects.Where(x => x.IsSelected));
                    bool succ = await DataController.SaveNewSubjectSelection(selectedStudent);
                    MessageBox.Show(succ ? "Successfully saved details." : "An error occurred. Could not save details.", succ ? "Success" : "Error",
                        MessageBoxButton.OK, succ ? MessageBoxImage.Information : MessageBoxImage.Warning);
                    if (succ)
                        Reset();
                    IsBusy = false;
                }, o => CanSave());
        }
        private bool CanSave()
        {
            return selectedStudent.StudentID > 0 && !IsBusy && AllSubjects.Any(o=>o.IsSelected);
        }
        
        public StudentSubjectSelectionModel SelectedStudent
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
        
        public ObservableCollection<StudentSubjectSelectionEntryModel> AllSubjects { get; private set; }

        public ICommand SaveCommand
        { get; private set; }
        
        public override void Reset()
        {
            selectedStudent.Reset();
        }
    }
}
