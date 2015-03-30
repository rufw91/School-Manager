using Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Starehe.ViewModels
{
    public class EventsReportVM:ViewModelBase
    {
        public EventsReportVM()
        {
            InitVars();
            CreateCommands();
        }
        protected override void InitVars()
        {
            Title = "EVENT REPORTS";
        }

        protected override void CreateCommands()
        {
            
        }

        public override void Reset()
        {
            
        }
    }
}
