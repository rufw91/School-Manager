using Helper;
using Helper.Models;
using System.Security.Permissions;
using System.Windows;
using System.Windows.Input;

namespace UmanyiSMS.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "Teacher")]
    public class NewEventVM: ViewModelBase
    {
        EventModel em;
        public NewEventVM()
            : base()
        {
            InitVars();
            CreateCommands();
        }
        protected override void CreateCommands()
        {
            SaveCommand = new RelayCommand(async o => 
            {
                if (MessageBoxResult.Yes != MessageBox.Show("This event has no Subject/Detail.\r\nAre you sure you woeld like to continue?", "Info",
                    MessageBoxButton.YesNo, MessageBoxImage.Information))
                    return;

                bool succ = await DataAccess.SaveNewEventAsync(em);
                if (succ)
                {
                    MessageBox.Show("Successfully saved details.","Success", MessageBoxButton.OK, MessageBoxImage.Information); 
                    Reset();
                }
                else
                    MessageBox.Show("Could not save details.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning); 
            },o=>CanSave());
            SaveAndShareCommand = new RelayCommand(async o =>
            {
                bool succ = await DataAccess.SaveNewEventAsync(em);
                if (succ)
                {
                    //succ = succ && await FaceBookHelper.ShareNewEvent(em);
                    if (succ)
                        Reset();
                }
            },o=>CanSave()&&false);
        }

        private bool CanSave()
        {
            return !string.IsNullOrWhiteSpace(em.Name) && em.EndDateTime > em.StartDateTime
                && !string.IsNullOrWhiteSpace(em.Location);
        }

        protected override void InitVars()
        {
            Title = "NEW EVENT";
            NewEvent = new EventModel();
        }
        public override void Reset()
        {
            NewEvent = new EventModel();
        }

        public EventModel NewEvent
        {
            get { return em; }

            private set
            {
                if (em != value)
                {
                    em = value;
                    NotifyPropertyChanged("NewEvent");
                }
            }
        }

        public ICommand SaveCommand
        {
            get;
            private set;
        }
        public ICommand SaveAndShareCommand
        {
            get;
            private set;
        }
    }
}
