using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Permissions;
using System.Windows;
using System.Windows.Input;
using UmanyiSMS.Lib;
using UmanyiSMS.Lib.Presentation;
using UmanyiSMS.Modules.Institution.Models;
using UmanyiSMS.Modules.Institution.Controller;

namespace UmanyiSMS.Modules.Institution.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "Deputy")]
    public class InstitutionSubjectsSetupVM:ViewModelBase
    {
        private ObservableCollection<SubjectModel> selectedSubjects;
        int startCount;
        public InstitutionSubjectsSetupVM()
        {
            InitVars();
            CreateCommands();
        }
        protected async override void InitVars()
        {
            Title = "INSTITUTION SUBJECTS SETUP";
            selectedSubjects = new ObservableCollection<SubjectModel>();
            selectedSubjects = await DataController.GetInstitutionSubjectsAsync();
            startCount = selectedSubjects.Count;
                
            NotifyPropertyChanged("SelectedSubjects");
        }

        protected override void CreateCommands()
        {
            AddSubjectCommand = new RelayCommand(o =>
            {
                selectedSubjects.Add(SelectedSubject);
                selectedSubjects = new ObservableCollection<SubjectModel>(selectedSubjects.OrderBy(s => s.Code));
                NotifyPropertyChanged("SelectedSubjects");
            }, o => CanAdd());

            RemoveSubjectCommand = new RelayCommand(o =>
            {
                selectedSubjects.Remove(SelectedAddedSubject);
                selectedSubjects = new ObservableCollection<SubjectModel>(selectedSubjects.OrderBy(s => s.Code));
                NotifyPropertyChanged("SelectedSubjects");
            }, o => CanRemove());

            SaveCommand = new RelayCommand(async o =>
            {
                IsBusy = true;
                bool succ = await DataController.SaveNewInstitutionSubjectSetup(selectedSubjects);
                MessageBox.Show(succ ? "Successfully updated details." : "Could not details at this time", "Information", MessageBoxButton.OK,
                    succ ? MessageBoxImage.Information : MessageBoxImage.Warning);
                selectedSubjects = await DataController.GetInstitutionSubjectsAsync();
                startCount = selectedSubjects.Count;
                NotifyPropertyChanged("SelectedSubjects");
                IsBusy = false;
            }, o => CanSave());
        }

        private bool CanSave()
        {
            return selectedSubjects.Count > 0 && (selectedSubjects.Any(o => o.SubjectID == 0) || selectedSubjects.Count != startCount);
        }

        private bool CanRemove()
        {
            return SelectedAddedSubject != null;
        }

        private bool CanAdd()
        {
            return SelectedSubject != null && !selectedSubjects.Any(o => o.NameOfSubject == SelectedSubject.NameOfSubject);
        }

        public ObservableCollection<SubjectModel> SelectedSubjects
        {
            get { return selectedSubjects; }
        }

        public SubjectModel SelectedSubject
        {
            get;
            set;
        }

        public SubjectModel SelectedAddedSubject
        {
            get;
            set;
        }

        public ObservableCollection<SubjectModel> AllSubjects
        {
            get { return SubjectModel.AllSubjects; }
        }

        public ICommand AddSubjectCommand
        {
            get;
            private set;
        }

        public ICommand RemoveSubjectCommand
        {
            get;
            private set;
        }

        public ICommand SaveCommand
        {
            get;
            private set;
        }

        public override void Reset()
        {
            
        }
    }
}
