using System.Collections.ObjectModel;
using System.Security.Permissions;
using System.Threading.Tasks;
using UmanyiSMS.Lib;
using UmanyiSMS.Modules.Fees.Controller;
using UmanyiSMS.Modules.Fees.Models;
using UmanyiSMS.Modules.Institution.Models;

namespace UmanyiSMS.Modules.Fees.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "User")]
    public class ViewFeesStructureVM: ViewModelBase
    {
        CombinedClassModel selectedCombinedClass;
        ObservableCollection<CombinedClassModel> allCombinedClasses;
        FeesStructureModel currentStruct;
        private TermModel selectedTerm;
        private ObservableCollection<TermModel> allTerms;
        public ViewFeesStructureVM()
        {
            InitVars();
            CreateCommands();
        }

        protected async override void InitVars()
        {
            Title = "VIEW FEES STRUCTURE";
            currentStruct = new FeesStructureModel();
            allCombinedClasses = await DataController.GetAllCombinedClassesAsync();
            AllTerms = await DataController.GetAllTermsAsync();
            NotifyPropertyChanged("AllCombinedClasses");
            PropertyChanged += async (o, e) =>
                {
                    if ((e.PropertyName == "SelectedTerm" || e.PropertyName == "SelectedCombinedClass") &&selectedTerm!=null&& selectedCombinedClass != null && selectedCombinedClass.Entries.Count > 0)
                       CurrentStructure.Entries =  (await RefreshEntries()).Entries;
                };

        }

        protected override void CreateCommands()
        {
            
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
        public ObservableCollection<CombinedClassModel> AllCombinedClasses
        {
            get { return allCombinedClasses; }
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

        public FeesStructureModel CurrentStructure
        {
            get { return currentStruct; }
        }

        private async Task<FeesStructureModel> RefreshEntries()
        {
            return await DataController.GetFeesStructureAsync(selectedCombinedClass.Entries[0].ClassID, selectedTerm.StartDate);
        }

        public override void Reset()
        {
            
        }
    }
}
