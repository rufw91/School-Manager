
using System.Collections.ObjectModel;
using System.Security.Permissions;
using System.Windows.Documents;
using System.Windows.Input;
using UmanyiSMS.Lib;
using UmanyiSMS.Lib.Presentation;
using UmanyiSMS.Modules.Institution.Models;
using UmanyiSMS.Modules.Students.Controller;
using UmanyiSMS.Modules.Students.Models;

namespace UmanyiSMS.Modules.Projects.ViewModels
{    
    [PrincipalPermission(SecurityAction.Demand, Role = "Accounts")]
    public class NewDonorVM : ViewModelBase
    {
        CombinedClassModel selectedClass;
        ObservableCollection<CombinedClassModel> allClasses;
        private FixedDocument doc;
        public NewDonorVM()
        {
            InitVars();
            CreateCommands();
        }
        protected async override void InitVars()
        {
            Title = "COMBINED CLASS LISTS";
            AllClasses = await Institution.Controller.DataController.GetAllCombinedClassesAsync();
        }

        protected override void CreateCommands()
        {
            GenerateCommand = new RelayCommand(async o =>
            {
                IsBusy = true;
                ClassStudentListModel s = await DataController.GetCombinedClassStudentListAsync(selectedClass);
                Document = DocumentHelper.GenerateDocument(s);
                IsBusy = false;
            }, o => selectedClass != null);
        }

        public ObservableCollection<CombinedClassModel> AllClasses
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

        public FixedDocument Document
        {
            get { return doc; }
            private set
            {
                if (doc != value)
                    doc = value;
                NotifyPropertyChanged("Document");
            }
        }

        public ICommand GenerateCommand
        { get; private set; }

        public CombinedClassModel SelectedClass
        {
            get { return selectedClass; }
            set
            {
                if (value != selectedClass)
                {
                    selectedClass = value;
                    NotifyPropertyChanged("SelectedClass");
                }
            }
        }

        public override void Reset()
        {

        }
    }
}
