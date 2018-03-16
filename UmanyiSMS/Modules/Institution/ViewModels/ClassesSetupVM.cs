using System.Collections.ObjectModel;
using System.Security.Permissions;
using System.Threading.Tasks;
using System.Windows.Input;
using UmanyiSMS.Lib;
using UmanyiSMS.Lib.Presentation;
using UmanyiSMS.Modules.Institution.Controller;
using UmanyiSMS.Modules.Institution.Models;

namespace UmanyiSMS.Modules.Institution.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "Deputy")]
    public class ClassesSetupVM : ViewModelBase
    {
        ClassesSetupModel classesSetup;
        ClassesSetupEntryModel newClass;
        
        public ClassesSetupVM()
        {
            InitVars();
            CreateCommands();
        }

        protected override async void InitVars()
        {
            IsBusy = true;
            Title = "CLASSES SETUP";
           
            classesSetup = new ClassesSetupModel();
            newClass = new ClassesSetupEntryModel();
            
            ObservableCollection<ClassModel> allClasses = await DataController.GetAllClassesAsync();
            foreach (ClassModel c in allClasses)
                classesSetup.Entries.Add(new ClassesSetupEntryModel(c));
            IsBusy = false;
        }
        
        protected override void CreateCommands()
        {
            AddNewClassCommand = new RelayCommand(async o =>
            {
                classesSetup.Entries.Add(newClass);
                bool succ = await DataController.SaveNewClassSetupAsync(classesSetup);
                if (succ)
                    await RefreshClassEntries();
                else
                    classesSetup.Entries.Remove(newClass);
                
                NewClass = new ClassesSetupEntryModel();
            }, o => CanAddClass());

           
        }

        public ClassesSetupModel ClassesSetup
        {
            get { return this.classesSetup; }
        }

        public ClassesSetupEntryModel NewClass
        {
            get { return this.newClass; }

            set
            {
                if (value != newClass)
                {
                    newClass = value;
                    NotifyPropertyChanged("NewClass");
                }
            }
        }
                
        private async Task RefreshClassEntries()
        {
            classesSetup.Entries.Clear();
            ObservableCollection<ClassModel> allClasses = await DataController.GetAllClassesAsync();
            foreach (ClassModel c in allClasses)
                classesSetup.Entries.Add(new ClassesSetupEntryModel(c));
        }

        public ICommand AddNewClassCommand
        {
            get;
            private set;
        }

        private bool CanAddClass()
        {
            int testInt;
            return !string.IsNullOrWhiteSpace(newClass.NameOfClass) && (newClass.NameOfClass.Length >= 6)
                && (newClass.NameOfClass.Substring(0, 5) == "FORM ")&&int.TryParse(newClass.NameOfClass.Substring(5,1),out testInt);
        }

        public override void Reset()
        {
            
        }
    }
}
