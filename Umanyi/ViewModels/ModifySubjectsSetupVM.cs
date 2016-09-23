using Helper;
using Helper.Models;
using System.Collections.ObjectModel;
using System.Security.Permissions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace UmanyiSMS.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "Teacher")]
    public class ModifySubjectsSetupVM: ViewModelBase
    {
        int  selectedClassID;
        ObservableCollection<ClassModel> allClasses;
        private SubjectModel currentSubject;
        private ObservableCollection<SubjectModel> selectedSubjects;
        public ModifySubjectsSetupVM()
        {
            InitVars();
            CreateCommands();
        }

        protected async override void InitVars()
        {
            IsBusy = true;
            Title = "CLASS SUBJECTS SETUP";
            selectedSubjects = new ObservableCollection<SubjectModel>();
            AllClasses = await DataAccess.GetAllClassesAsync();

            PropertyChanged += async (o, e) =>
            {
                if (e.PropertyName == "SelectedClassID")
                {
                    if (selectedClassID <= 0)
                        return;
                    else
                        await RefreshSubjectEntries();
                }
            };
            IsBusy = false;
        }

        public ObservableCollection<SubjectModel> SelectedSubjects
        {
            get { return selectedSubjects; }
            private set
            {
                if (selectedSubjects != value)
                {
                    selectedSubjects = value;
                    NotifyPropertyChanged("SelectedSubjects");
                }
            }
        }

        public ObservableCollection<ClassModel> AllClasses
        {
            get { return allClasses; }
            private set
            {
                if (allClasses != value)
                {
                    allClasses = value;
                    NotifyPropertyChanged("AllClasses");
                }
            }
        }

       

        public SubjectModel CurrentSubject
        {
            get { return this.currentSubject; }

            set
            {
                if (value != currentSubject)
                {
                    currentSubject = value;
                    NotifyPropertyChanged("CurrentSubject");
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
                    NotifyPropertyChanged("SelectedClassID");
                }
            }
        }

        private async Task RefreshSubjectEntries()
        {
            selectedSubjects.Clear();
            if (selectedClassID == 0)
                return;
            var temp = await DataAccess.GetSubjectsRegistredToClassAsync(selectedClassID);
            
            foreach (SubjectModel sm in temp)
                selectedSubjects.Add(sm);
        }

        protected override void CreateCommands()
        {
            RemoveCommand = new RelayCommand(o =>
            {
                selectedSubjects.Remove(currentSubject);
            }, o => CanRemove()); 

            SetDefaultCommand = new RelayCommand(async o =>
            {
                selectedSubjects.Clear();
                if (selectedClassID == 0)
                    return;
                var temp = await DataAccess.GetSubjectsRegistredToInstitutionAsync();
                foreach (SubjectModel sm in temp)
                    selectedSubjects.Add(sm);
            }, o => CanReset()); 
            SaveCommand = new RelayCommand(async o =>
            {
                bool succ = await DataAccess.SaveNewSubjectSetupAsync(selectedClassID,selectedSubjects);
                MessageBox.Show(succ ? "Successfully updated details." : "Could not details at this time", "Information", MessageBoxButton.OK, 
                    succ ? MessageBoxImage.Information : MessageBoxImage.Warning);
                
            }, o => CanSave());
        }

        private bool CanRemove()
        {
            return currentSubject != null;
        }

        private bool CanReset()
        {
            return selectedClassID > 0;
        }

        private bool CanSave()
        {
            return selectedClassID > 0
                && currentSubject!=null;
        }

        public ICommand SaveCommand
        {
            get;
            private set;
        }

        public ICommand RemoveCommand
        {
            get;
            private set;
        }

        public ICommand SetDefaultCommand
        {
            get;
            private set;
        }

        public override void Reset()
        {
            
        }
    }
}
