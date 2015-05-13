using Helper;
using Helper.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace UmanyiSMS.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "User")]
    public class CurrentTimeTableFullVM:ViewModelBase
    {
        TimeTableModel timeTable;
        public CurrentTimeTableFullVM()
        {
            InitVars();
            CreateCommands();
        }
        protected async override void InitVars()
        {
            Title = "CURRENT TIMETABLE (FULL)";
            IsBusy = true;
            CurrentTimeTable = await DataAccess.GetCurrentTimeTableFull();
            IsBusy = false;
        }

        protected override void CreateCommands()
        {
            RefreshCommand = new RelayCommand(async o =>
            {
                IsBusy = true;
                CurrentTimeTable = await DataAccess.GetCurrentTimeTableFull();
                IsBusy = false;
            }, o => true);
            PrintCommand = new RelayCommand(o =>
            {
                IsBusy = true;
                PrintHelper.PrintVisual(o as Visual);
                IsBusy = false;
            }, o => true);
        }

        public override void Reset()
        {
            
        }

        public TimeTableModel CurrentTimeTable
        {
            get { return timeTable; }
            private set
            {
                if (value != timeTable)
                {
                    timeTable = value;
                    NotifyPropertyChanged("CurrentTimeTable");
                }
            }
        }

        public ICommand RefreshCommand
        {
            get;private set;
        }

        public ICommand PrintCommand
        {
            get;
            private set;
        }
    }
}
