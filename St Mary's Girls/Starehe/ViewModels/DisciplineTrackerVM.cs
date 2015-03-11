using Helper;
using Helper.Models;
using System.Security.Permissions;
using System.Windows;
using System.Windows.Input;

namespace Starehe.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "Teacher")]
    public class DisciplineTrackerVM: ViewModelBase
    {
        StudentSelectModel selectedStudent;
        DisciplineModel discipline;
        public DisciplineTrackerVM()
        {
            InitVars();
            CreateCommands();
        }
        protected override void InitVars()
        {
            Title = "DISCIPLINE TRACKER";
            SelectedStudent = new StudentSelectModel();
            CurrentDiscipline = new DisciplineModel();
            selectedStudent.PropertyChanged += (o, e) =>
                {
                    if (e.PropertyName=="StudentID")
                    selectedStudent.CheckErrors();                  
                };
        }

        protected override void CreateCommands()
        {
            BrowseCommand = new RelayCommand(o =>
            {
                discipline.SPhoto = FileHelper.BrowseImageAsByteArray();
            }, o => true);
            SaveCommand = new RelayCommand(async o =>
            {
                discipline.NameOfStudent = selectedStudent.NameOfStudent;
                discipline.StudentID = selectedStudent.StudentID;
                bool succ = await DataAccess.SaveNewDiscipline(discipline);
                MessageBox.Show(succ?"Successfully saved details":"Could not save details.",succ?"Success":"Error",MessageBoxButton.OK,succ?MessageBoxImage.Information:MessageBoxImage.Warning);
                if (succ)
                    Reset();
            }, o => CanSave());
        }

        private bool CanSave()
        {
            return !selectedStudent.HasErrors && !string.IsNullOrWhiteSpace(discipline.Issue);
        }

        public override void Reset()
        {
            selectedStudent.Reset();
            discipline.Reset();
        }
        public DisciplineModel CurrentDiscipline
        {
            get { return discipline; }
            set
            {
                if (value != discipline)
                    discipline = value;
                NotifyPropertyChanged("CurrentDiscipline");
            }
        }

        public ICommand BrowseCommand { get; private set; }

        public ICommand SaveCommand { get; private set; }

        public StudentSelectModel SelectedStudent
        {
            get { return selectedStudent; }
            set
            {
                if (value != selectedStudent)
                    selectedStudent = value;
                NotifyPropertyChanged("SelectedStudent");
            }
        }
    }
}
