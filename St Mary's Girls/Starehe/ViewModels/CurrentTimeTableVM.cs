using Helper;
using Helper.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace Starehe.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "User")]
    public class CurrentTimeTableVM: ViewModelBase
    {
        private ObservableCollection<TimetableClassModel> entries;
        public CurrentTimeTableVM()
            : base()
        {
            InitVars();
            CreateCommands();
        }
        protected override void CreateCommands()
        {
            
        }

        protected async override void InitVars()
        {            
            Title = "CURRENT TIMETABLE";
            Entries = await DataAccess.GetCurrentTimeTableAsync();
        }

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

        public override void Reset()
        {
            
        }

    }
}
