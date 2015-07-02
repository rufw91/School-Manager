using Helper;
using Helper.Models;
using System.Security.Permissions;
using System.Windows.Input;

namespace UmanyiSMS.ViewModels
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
            if (currentStudent!=null)
            {
                BoardingStatus = currentStudent.IsBoarder ? Boardingtype.Boarder : Boardingtype.DayScholar;
                NotifyPropertyChanged("BoardingStatus");
            }
        }
        protected override void InitVars()
        {
            Title = "STUDENT DETAILS";
            CurrentStudent = new StudentListModel();

        }
        protected override void CreateCommands()
        {
        }

        public Boardingtype BoardingStatus
        { get; private set; }

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
