using Helper;
using Helper.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Starehe.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "User")]
    public class CurrentTimeTableVM: ViewModelBase
    {
        DayOfWeek selectedDay;
        private ObservableCollection<TimetableClassModel> entries;
        public CurrentTimeTableVM()
            : base()
        {
            InitVars();
            CreateCommands();
        }
        protected override void CreateCommands()
        {
            RefreshCommand = new RelayCommand(async o =>
            {
                //Entries = await DataAccess.GetCurrentTimeTableAsync((int)selectedDay);
            }, o => true);
        }

        protected async override void InitVars()
        {            
            Title = "CURRENT TIMETABLE";
            DaysOfTheWeek = Enum.GetValues(typeof(DayOfWeek));
            NotifyPropertyChanged("DaysOfTheWeek");
            //Entries = await DataAccess.GetCurrentTimeTableAsync((int)selectedDay);
        }

        public Array DaysOfTheWeek
        { get; private set; }

        public ObservableCollection<TimetableClassModel> Entries
        {
            get { return this.entries; }

            private set
            {
                if (value != this.entries)
                {
                    this.entries = value;
                    NotifyPropertyChanged("Entries");
                }
            }
        }

        public DayOfWeek SelectedDay
        {
            get { return this.selectedDay; }

            set
            {
                if (value != this.selectedDay)
                {
                    this.selectedDay = value;
                    NotifyPropertyChanged("SelectedDay");
                }
            }
        }

        public ICommand RefreshCommand
        {
            get;
            private set;
        }

        public override void Reset()
        {
            
        }

    }
}
