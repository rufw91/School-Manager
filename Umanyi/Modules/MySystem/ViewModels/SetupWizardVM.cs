using System.Collections.ObjectModel;
using System.Windows;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media;
using UmanyiSMS.Lib;
using UmanyiSMS.Lib.Models;
using UmanyiSMS.Lib.Presentation;
using UmanyiSMS.Modules.Institution.Models;
using System;
using System.Collections.Generic;
using UmanyiSMS.Lib.Controllers;
using System.Threading.Tasks;

namespace UmanyiSMS.Modules.MySystem.ViewModels
{
    public class SetupWizardVM : ViewModelBase
    { 
        ClassesSetupModel classesSetup;
        ClassesSetupEntryModel newClass;
        ViewModelBase source;
        ApplicationModel newSchool;
        ExamSettingsModel examSettings;
        private Color accentColor;
        private ObservableCollection<ClassModel> classes;
        private ObservableCollection<SubjectModel> subjects;
        private List<string> themes=new List<string> { "Light", "Dark" };

        public SetupWizardVM()
        {
            InitVars();
            CreateCommands();
        }
        public override void Reset()
        {
        }
        
        protected override void CreateCommands()
        {
            Page1Command = new RelayCommand(o =>
            {
                IsBusy = true;
                Source = new WelcomeVM();
                IsBusy = false;
            }, o => !IsBusy);
            Page2Command = new RelayCommand(o =>
            {
                IsBusy = true;
                Source = new InstitutionSetupWVM();
                IsBusy = false;
            }, o => !IsBusy);
            Page3Command = new RelayCommand(o =>
            {
                IsBusy = true;
                App.SaveInfo(newSchool);
                Source = new LogoVM();
                IsBusy = false;
            }, o => !IsBusy);
            Page4Command = new RelayCommand(o =>
            {
                IsBusy = true;
                App.SaveInfo(newSchool);
                Source = new ClassesSetupWVM();
                IsBusy = false;
            }, o => !IsBusy);
            Page5Command = new RelayCommand(o =>
            {
                IsBusy = true;
                Source = new SubjectsSetupWVM();
                IsBusy = false;
            }, o => !IsBusy);
            Page6Command = new RelayCommand(o =>
            {
                IsBusy = true;
                Source = new GradesSetupWVM();
                IsBusy = false;
            }, o => !IsBusy);
            Page7Command = new RelayCommand(async o =>
            {
                IsBusy = true;
                bool succ = await Institution.Controller.DataController.SaveNewExamSettingsAsync(examSettings);
                App.AppExamSettings.CopyFrom(examSettings);
                Source = new ColorsVM();
                IsBusy = false;
            }, o => !IsBusy&&CanSaveExamSettings());
            Page8Command = new RelayCommand(o =>
            {
                IsBusy = true;
                App.SaveInfo(newSchool);
                Source = new FinallyVM();
                IsBusy = false;
            }, o => !IsBusy);
            BrowseCommand = new RelayCommand(o =>
            {
                IsBusy = true;
                newSchool.SPhoto = FileHelper.BrowseImageAsByteArray();
                IsBusy = false;
            }, o => !IsBusy);
            AddNewClassCommand = new RelayCommand(async o =>
            {
                classesSetup.Entries.Add(newClass);
                bool succ = await Institution.Controller.DataController.SaveNewClassSetupAsync(classesSetup);
                if (succ)
                    await RefreshClassEntries();
                else
                    classesSetup.Entries.Remove(newClass);

                NewClass = new ClassesSetupEntryModel();
            }, o => CanAddClass());
            AddSubjectCommand = new RelayCommand(async o =>
            {
                subjects.Add(SelectedSubject);
                bool succ = await Institution.Controller.DataController.SaveNewInstitutionSubjectSetup(subjects);
                subjects = await Institution.Controller.DataController.GetInstitutionSubjectsAsync();
                NotifyPropertyChanged("SelectedSubjects");
            }, o => CanAddSubject());
            StartCommand = new RelayCommand(o =>
            {
                IsBusy = true;
                RegistryHelper.SetFirstRunComplete();
                if (CloseAction != null)
                    CloseAction.Invoke();
                IsBusy = false;
            }, o => !IsBusy);
        }

        protected async override void InitVars()
        {
            UserClosed = true;
            Source = new WelcomeVM();
            newSchool = new ApplicationModel();
            examSettings = new ExamSettingsModel();
            newSchool.CopyFrom(App.Info);
            AccentColor = newSchool.AccentColor;
            SelectedTheme = newSchool.Theme;
            examSettings.CopyFrom(App.AppExamSettings);
            classes = await Institution.Controller.DataController.GetAllClassesAsync();
            classesSetup = new ClassesSetupModel();
            newClass = new ClassesSetupEntryModel();
            await EnableDb();
            foreach (ClassModel c in classes)
                classesSetup.Entries.Add(new ClassesSetupEntryModel(c));
            subjects = await Institution.Controller.DataController.GetInstitutionSubjectsAsync();
            PropertyChanged += (o, e) =>
              {
                  if (e.PropertyName == "AccentColor")
                  {
                      newSchool.AccentColor = accentColor;
                      App.SetAccent(accentColor);
                  }

                  if (e.PropertyName == "SelectedTheme")
                  {
                      newSchool.Theme = selectedTheme;
                      App.SetTheme(selectedTheme);
                  }
              };
        }

        private Task<bool> EnableDb()
        {
            return Task.Factory.StartNew(() =>
           {
               string cmdStr = "USE [master]\r\nALTER DATABASE[UmanyiSMS] SET READ_WRITE WITH NO_WAIT\r\nALTER LOGIN [sa] WITH PASSWORD='000002'\r\nALTER LOGIN[sa] ENABLE";
               return DataAccessHelper.Helper.ExecuteNonQuery(cmdStr);
           });
        }

        private bool CanSaveExamSettings()
        {
            bool succ = true;
            for (int i = 0; i <= 11; i++)
            {
                if (i == 0)
                    succ = examSettings.GradeRanges[i].Value == 100;
                if (i < 11)
                    succ = succ && (examSettings.GradeRanges[i].Key == (examSettings.GradeRanges[i + 1].Value + 1));
                else
                    succ = succ && examSettings.GradeRanges[i].Key == 0;
            }
            return succ;
        }
        

        private bool CanAddSubject()
        {
            return SelectedSubject != null && !subjects.Any(o => o.NameOfSubject == SelectedSubject.NameOfSubject);
        }

        private async Task RefreshClassEntries()
        {
            classesSetup.Entries.Clear();
            ObservableCollection<ClassModel> allClasses = await Institution.Controller.DataController.GetAllClassesAsync();
            foreach (ClassModel c in allClasses)
                classesSetup.Entries.Add(new ClassesSetupEntryModel(c));
        }

        private bool CanAddClass()
        {
            int testInt;
            return !string.IsNullOrWhiteSpace(newClass.NameOfClass) && (newClass.NameOfClass.Length >= 6)
                && (newClass.NameOfClass.Substring(0, 5) == "FORM ") && int.TryParse(newClass.NameOfClass.Substring(5, 1), out testInt);
        }

        private Color[] accentColors = new Color[]
        {
            Color.FromRgb(164, 196, 0),
            Color.FromRgb(96, 169, 23),
            Color.FromRgb(0, 138, 0),
            Color.FromRgb(0, 171, 169),
            Color.FromRgb(27, 161, 226),
            Color.FromRgb(0, 80, 239),
            Color.FromRgb(106, 0, 255),
            Color.FromRgb(44, 58, 73),
            Color.FromRgb(170, 0, 255),            
            Color.FromRgb(244, 114, 208),
            Color.FromRgb(216, 0, 115),
            Color.FromRgb(162, 0, 37),
            Color.FromRgb(229, 20, 0),
            Color.FromRgb(250, 104, 0),
            Color.FromRgb(240, 163, 10),
            Color.FromRgb(227, 200, 0),
            Color.FromRgb(130, 90, 44),
            Color.FromRgb(109, 135, 100),
            Color.FromRgb(100, 118, 135),
            Color.FromRgb(118, 96, 138),
            Color.FromRgb(135, 121, 78)
        };
        private string selectedTheme;
        public Action CloseAction
        { get; set; }
        public ObservableCollection<SubjectModel> AllSubjects
        {
            get { return SubjectModel.AllSubjects; }
        }

        public ObservableCollection<SubjectModel> SelectedSubjects
        {
            get { return subjects; }
        }

        public SubjectModel SelectedSubject
        {
            get;
            set;
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

        public Color[] AccentColors
        {
            get
            {
                return this.accentColors;
            }
        }

        public List<string> Themes
        {
            get
            {
                return this.themes;
            }
        }

        public string SelectedTheme
        {
            get
            {
                return this.selectedTheme;
            }

            set
            {
                if (value != this.selectedTheme)
                {
                    this.selectedTheme = value;
                    NotifyPropertyChanged("SelectedTheme");
                }
            }
        }


        public ViewModelBase Source
        {
            get { return this.source; }

            set
            {
                if (value != this.source)
                {
                    this.source = value;
                    NotifyPropertyChanged("Source");
                }
            }
        }

        public ApplicationModel NewSchool
        {
            get { return this.newSchool; }

            set
            {
                if (value != this.newSchool)
                {
                    this.newSchool = value;
                    NotifyPropertyChanged("NewSchool");
                }
            }
        }

        public ExamSettingsModel ExamSettings
        {
            get { return this.examSettings; }

            set
            {
                if (value != this.examSettings)
                {
                    this.examSettings = value;
                    NotifyPropertyChanged("ExamSettings");
                }
            }
        }

        public ObservableCollection<ClassModel> Classes
        {
            get { return this.classes; }

            set
            {
                if (value != this.classes)
                {
                    this.classes = value;
                    NotifyPropertyChanged("Classes");
                }
            }
        }

        public ObservableCollection<SubjectModel> Subjects
        {
            get { return this.subjects; }

            set
            {
                if (value != this.subjects)
                {
                    this.subjects = value;
                    NotifyPropertyChanged("Subjects");
                }
            }
        }

        public Color AccentColor
        {
            get { return this.accentColor; }

            set
            {                
                if (value != this.accentColor)
                {
                    this.accentColor = value;
                    NotifyPropertyChanged("AccentColor");
                }
            }
        }
                
        public ICommand Page1Command
        { get; private set; }        
        public ICommand Page2Command
        { get; private set; }
        public ICommand Page3Command
        { get; private set; }
        public ICommand Page4Command
        { get; private set; }
        public ICommand Page5Command
        { get; private set; }
        public ICommand Page6Command
        { get; private set; }
        public ICommand Page7Command
        { get; private set; }
        public ICommand Page8Command
        { get; private set; }
        public ICommand BrowseCommand
        { get; private set; }
        public ICommand AddNewClassCommand
        { get; private set; }
        public ICommand AddSubjectCommand
        {
            get;
            private set;
        }
        public ICommand StartCommand
        { get; private set; }
        public bool UserClosed { get; internal set; }
    }
}
