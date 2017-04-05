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

namespace UmanyiSMS.Modules.MySystem.ViewModels
{
    public class SetupWizardVM : ViewModelBase
    {
        private ViewModelBase source;
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
                Source = new LogoVM();
                IsBusy = false;
            }, o => !IsBusy);
            Page4Command = new RelayCommand(o =>
            {
                IsBusy = true;
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
            Page7Command = new RelayCommand(o =>
            {
                IsBusy = true;
                Source = new ColorsVM();
                IsBusy = false;
            }, o => !IsBusy);
            Page8Command = new RelayCommand(o =>
            {
                IsBusy = true;
                Source = new FinallyVM();
                IsBusy = false;
            }, o => !IsBusy);
        }

        protected async override void InitVars()
        {
            Source = new WelcomeVM();
            newSchool = new ApplicationModel();
            examSettings = new ExamSettingsModel();
            newSchool.CopyFrom(App.Info);
            AccentColor = newSchool.AccentColor;
            SelectedTheme = newSchool.Theme;
            examSettings.CopyFrom(App.AppExamSettings);
            classes = await Institution.Controller.DataController.GetAllClassesAsync();
            subjects = await Institution.Controller.DataController.GetInstitutionSubjectsAsync();
            PropertyChanged += (o, e) =>
              {
                  if (e.PropertyName == "AccentColor")
                  { App.SetAccent(accentColor); }

                  if (e.PropertyName == "SelectedTheme")
                  { App.SetTheme(selectedTheme); }
                     
                  
              };
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

    }
}
