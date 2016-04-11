using Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UmanyiSMS.ViewModels
{
    public class AdjustJournalVM : ViewModelBase
    {
        public AdjustJournalVM()
        {
            InitVars();
            CreateCommands();
        }
        public override void Reset()
        {
        }

        protected override void CreateCommands()
        {
        }

        protected override void InitVars()
        {
            Title = "ADJUST TRANSACTIONS";
        }
    }
}
