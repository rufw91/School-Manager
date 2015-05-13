using Helper;
using Helper.Models;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Security.Permissions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace UmanyiSMS.ViewModels
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
            
            ObservableCollection<ClassModel> allClasses = await DataAccess.GetAllClassesAsync();
            foreach (ClassModel c in allClasses)
                classesSetup.Entries.Add(new ClassesSetupEntryModel(c));
            IsBusy = false;
        }
        
        protected override void CreateCommands()
        {
            AddNewClassCommand = new RelayCommand(async o =>
            {
                classesSetup.Entries.Add(newClass);
                bool succ = await DataAccess.SaveNewClassSetupAsync(classesSetup);
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
            ObservableCollection<ClassModel> allClasses = await DataAccess.GetAllClassesAsync();
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
            return !string.IsNullOrWhiteSpace(newClass.NameOfClass);
        }

        public override void Reset()
        {
            
        }
    }
}
