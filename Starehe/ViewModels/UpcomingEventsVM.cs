
using Helper;
using Helper.Models;
using System.Collections.ObjectModel;
using System.Security.Permissions;
namespace UmanyiSMS.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "User")]
    public class UpcomingEventsVM: ViewModelBase
    {
        ObservableCollection<EventModel> upcomingEvents;
        public UpcomingEventsVM()
        {
            InitVars();
            CreateCommands();
        }
        protected override void CreateCommands()
        {
           
        }

        protected async override void InitVars()
        {            
            Title = "UPCOMING EVENTS";
            UpcomingEvents = await DataAccess.GetUpcomingEvents();
        }

        public override void Reset()
        {
            
        }

        public ObservableCollection<EventModel> UpcomingEvents
        {
            get { return upcomingEvents; }
            
            private set
            {
                if (upcomingEvents != value)
                {
                    upcomingEvents = value;
                    NotifyPropertyChanged("UpcomingEvents");
                }
            }
        }
    }
}
