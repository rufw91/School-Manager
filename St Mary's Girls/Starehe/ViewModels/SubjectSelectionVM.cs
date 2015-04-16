using Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Starehe.ViewModels
{
    public class SubjectSelectionVM:ViewModelBase
    {
        public SubjectSelectionVM()
        {
            InitVars();
            CreateCommands();
        }
        protected override void InitVars()
        {
            Title = "SUBJECT SELECTION";
        }

        protected override void CreateCommands()
        {
            
        }

        public override void Reset()
        {
           
        }
    }
}
