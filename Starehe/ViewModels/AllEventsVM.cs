using Helper;
using Helper.Models;
using System.Collections.ObjectModel;
using System.Security.Permissions;

namespace UmanyiSMS.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "User")]
    public class AllEventsVM: ViewModelBase
    {
        ObservableCollection<EventModel> allEvents;
        public AllEventsVM()
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
            Title = "ALL EVENTS";
            AllEvents = await DataAccess.GetAllEvents();
        }

        public override void Reset()
        {

        }
        public ObservableCollection<EventModel> AllEvents
        {
            get { return allEvents; }

            private set
            {
                if (allEvents != value)
                {
                    allEvents = value;
                    NotifyPropertyChanged("AllEvents");
                }
            }
        }
    }
}
