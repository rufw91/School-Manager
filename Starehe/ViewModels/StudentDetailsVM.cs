using Helper;
using Helper.Models;
using System.Security.Permissions;
using System.Windows.Input;

namespace UmanyiSMS.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "Teacher")]
    public class StudentDetailsVM : ViewModelBase
    {
        StudentModel currentStudent;
        private bool isReadonly;
        public StudentDetailsVM()
            : base()
        {
            InitVars();
            CreateCommands();
            CurrentStudent = new StudentModel();
        }

        public StudentModel CurrentStudent
        {
            get { return currentStudent; }

            set
            {
                if (currentStudent != value)
                {
                    currentStudent = value;
                    NotifyPropertyChanged("CurrentStudent");
                }
            }
        }

        public StudentDetailsVM(StudentModel sm)
            : base()
        {
            InitVars();
            CreateCommands();
            CurrentStudent = sm;
        }

        protected override void CreateCommands()
        {
            ModifyStudentCommand = new RelayCommand(o =>
            {
                IsReadonly = !IsReadonly;
            }, o => true);
        }

        protected override void InitVars()
        {
            Title = "STUDENT DETAILS";
            IsReadonly = true;
        }

        public override void Reset()
        {

        }
        public ICommand ModifyStudentCommand
        {
            get;
            private set;
        }

        public bool IsReadonly
        {
            get { return isReadonly; }
            private set
            {
                if (isReadonly != value)
                {
                    isReadonly = value;
                    NotifyPropertyChanged("IsReadonly");
                }
            }
        }
    }
}
