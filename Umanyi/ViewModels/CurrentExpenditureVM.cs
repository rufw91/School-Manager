using Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UmanyiSMS.ViewModels
{
    public class CurrentExpenditureVM:ViewModelBase
    {
        public CurrentExpenditureVM()
        {
            InitVars();
            CreateCommands();
        }
        protected override void InitVars()
        {
            Title = "CURRENT EXPENDITURE";
        }

        protected override void CreateCommands()
        {
        }

        public override void Reset()
        {
        }
    }
}
