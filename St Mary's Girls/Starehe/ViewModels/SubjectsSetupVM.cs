using Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Starehe.ViewModels
{
    public class SubjectsSetupVM:ViewModelBase
    {
        public SubjectsSetupVM()
        {
            InitVars();
            CreateCommands();
        }
        protected override void InitVars()
        {
            Title = "SUBJECTS SETUP";
        }

        protected override void CreateCommands()
        {
        }

        public override void Reset()
        {
        }
    }
}
