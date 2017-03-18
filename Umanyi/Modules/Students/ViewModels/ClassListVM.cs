
using System.Collections.ObjectModel;
using System.Security.Permissions;
using System.Windows.Documents;
using System.Windows.Input;
using UmanyiSMS.Lib;
using UmanyiSMS.Lib.Presentation;
using UmanyiSMS.Modules.Institution.Models;
using UmanyiSMS.Modules.Students.Controller;
using UmanyiSMS.Modules.Students.Models;

namespace UmanyiSMS.Modules.Students.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "Teacher")]
    public class ClassListVM:ViewModelBase
    {
        ClassModel selectedClass;
        ObservableCollection<ClassModel> allClasses;
        private FixedDocument doc;
        public ClassListVM()
        {
            InitVars();
            CreateCommands();
        }
        protected async override void InitVars()
        {
            Title = "CLASS LISTS";
            AllClasses = await Institution.Controller.DataController.GetAllClassesAsync();
        }

        protected override void CreateCommands()
        {
            GenerateCommand = new RelayCommand(async o =>
            {
                IsBusy = true;
                ClassStudentListModel s = await DataController.GetClassStudentListAsync(selectedClass);
                Document = DocumentHelper.GenerateDocument(s);
                IsBusy = false;
            }, o => selectedClass != null);
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

        public ClassModel SelectedClass
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
