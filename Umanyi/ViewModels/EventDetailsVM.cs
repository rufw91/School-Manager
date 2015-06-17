using Helper;
using Helper.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace UmanyiSMS.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "User")]
    public class EventDetailsVM:ViewModelBase
    {
        private EventModel em;
        public EventDetailsVM()
        {
            InitVars();
            CreateCommands();
        }
        protected override void InitVars()
        {
           
        }

        protected override void CreateCommands()
        {
            
        }
        public EventModel NewEvent
        {
            get { return em; }

            set
            {
                if (em != value)
                {
                    em = value;
                    NotifyPropertyChanged("NewEvent");
                }
            }
        }
        public override void Reset()
        {
           
        }
    }
}
