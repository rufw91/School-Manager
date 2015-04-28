
using Helper;
using Helper.Models;
using System.Security.Permissions;
using System.Windows.Input;
namespace UmanyiSMS.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "Accounts")]
    public class StaffDetailsVM : ViewModelBase
    {
        StaffModel currentStaff;
        private bool isReadonly;
        public StaffDetailsVM()
            : base()
        {
            InitVars();
            CreateCommands();
            CurrentStaff = new StaffModel();
        }

        public StaffModel CurrentStaff
        {
            get { return currentStaff; }

            set
            {
                if (currentStaff != value)
                {
                    currentStaff = value;
                    NotifyPropertyChanged("CurrentStaff");
                }
            }
        }

        public StaffDetailsVM(StaffModel sm)
            : base()
        {
            InitVars();
            CreateCommands();
            CurrentStaff = sm;
        }

        protected override void CreateCommands()
        {
            ModifyStaffCommand = new RelayCommand(o =>
            {
                IsReadonly = !IsReadonly;
            }, o => true);
        }

        protected override void InitVars()
        {
            Title = "STAFF DETAILS";
            IsReadonly = true;
        }

        public override void Reset()
        {

        }
        public ICommand ModifyStaffCommand
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
