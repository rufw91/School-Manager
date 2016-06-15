using Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UmanyiSMS.ViewModels
{
    public class PrepareBudgetVM:ViewModelBase
    {
        public PrepareBudgetVM()
        {
            InitVars();
            CreateCommands();
        }
        protected override void InitVars()
        {
            Title = "PREPARE BUDGET";
        }

        protected override void CreateCommands()
        {
        }

        public override void Reset()
        {
        }
    }
}
