using Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Starehe.ViewModels
{
    public class TimeTableSettingsVM: ViewModelBase
    {
        public TimeTableSettingsVM()
        {
            InitVars();
            CreateCommands();
        }
        protected override void InitVars()
        {
            Title = "SETTINGS";
        }

        protected override void CreateCommands()
        {
            
        }

        public override void Reset()
        {
           
        }
    }
}
