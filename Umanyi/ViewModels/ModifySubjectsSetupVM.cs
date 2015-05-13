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
        SubjectsSetupModel subjectsSetup;
        SubjectsSetupEntryModel currentSubject;
        CombinedClassModel selectedCombinedClass;
        ObservableCollection<CombinedClassModel> allCombinedClasses;
        public ModifySubjectsSetupVM()
        {
            InitVars();
            CreateCommands();
        }

        protected async override void InitVars()
        {
            IsBusy = true;
            Title = "MODIFY SUBJECTS SETUP";
            subjectsSetup = new SubjectsSetupModel();
            AllCombinedClasses = await DataAccess.GetAllCombinedClassesAsync();

            PropertyChanged += async (o, e) =>
            {
                if (e.PropertyName == "SelectedCombinedClass")
                {
                    if ((selectedCombinedClass == null) || (selectedCombinedClass.Entries == subjectsSetup.Classes))
                        return;
                    else
                    {
                        if (selectedCombinedClass != null)
                            subjectsSetup.Classes = selectedCombinedClass.Entries;
                        await RefreshSubjectEntries();
                    }
                }
            };
            IsBusy = false;
        }

        public SubjectsSetupModel SubjectsSetup
        {
            get { return this.subjectsSetup; }
        }

        public ObservableCollection<CombinedClassModel> AllCombinedClasses
        {
            get { return allCombinedClasses; }
            private set
            {
                if (allCombinedClasses != value)
                {
                    allCombinedClasses = value;
                    NotifyPropertyChanged("AllCombinedClasses");
                }
            }
        }

        public SubjectsSetupEntryModel CurrentSubject
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

        private async Task RefreshSubjectEntries()
        {
            subjectsSetup.Entries.Clear();
            if (selectedCombinedClass == null || selectedCombinedClass.Entries.Count == 0)
                return;
            var temp = await DataAccess.GetSubjectsRegistredToClassAsync(selectedCombinedClass.Entries[0].ClassID);
            foreach (SubjectModel sm in temp)
                subjectsSetup.Entries.Add(new SubjectsSetupEntryModel(sm) { SubjectSetupID = 1 });
        }

        protected override void CreateCommands()
        {
            SaveCommand = new RelayCommand(async o =>
            {
                bool succ = await DataAccess.UpdateSubjectAsync(currentSubject);
                MessageBox.Show(succ ? "Successfully updated details." : "Could not details at this time", "Information", MessageBoxButton.OK, 
                    succ ? MessageBoxImage.Information : MessageBoxImage.Warning);
                
            }, o => CanSave());
        }

        private bool CanSave()
        {
            return subjectsSetup.Classes.Count > 0
                && currentSubject!=null;
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
