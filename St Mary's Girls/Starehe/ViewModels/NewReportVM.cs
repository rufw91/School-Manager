using Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Starehe.ViewModels
{
    public class NewReportVM:ViewModelBase
    {
        public NewReportVM()
        {
            InitVars();
            CreateCommands();

        }
        protected override void InitVars()
        {
            Title = "SIMPLE REPORT";
        }

        protected override void CreateCommands()
        {
            
        }

        public override void Reset()
        {
           
        }
    }
}
