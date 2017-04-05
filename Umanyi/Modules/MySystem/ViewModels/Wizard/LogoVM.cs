using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UmanyiSMS.Lib;

namespace UmanyiSMS.Modules.MySystem.ViewModels
{
    public class LogoVM : ViewModelBase
    {
        public LogoVM()
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
            Title = "LOGO";
        }
    }
}
