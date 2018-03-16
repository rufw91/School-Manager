using System.Security.Permissions;
using UmanyiSMS.Lib;
using UmanyiSMS.Modules.Students.Models;

namespace UmanyiSMS.Modules.Students.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "Teacher")]
    public class StudentDetailsVM : ViewModelBase
    {
        StudentListModel currentStudent;
        public StudentDetailsVM()
        {
            InitVars();
            CreateCommands();
            
        }
        public StudentDetailsVM(StudentListModel sm)
        {
            InitVars();
            CreateCommands();
            CurrentStudent = sm;
        }
        protected override void InitVars()
        {
            Title = "STUDENT DETAILS";
            CurrentStudent = new StudentListModel();

        }
        protected override void CreateCommands()
        {
        }
        
        public StudentListModel CurrentStudent
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
        
        public override void Reset()
        {

        }

    }
}
