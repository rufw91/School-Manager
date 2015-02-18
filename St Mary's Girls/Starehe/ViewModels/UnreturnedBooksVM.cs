using Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Starehe.ViewModels
{
    public class UnreturnedBooksVM: ViewModelBase
    {
        public UnreturnedBooksVM()
        {
            InitVars();
            CreateCommands();
        }
        protected override void InitVars()
        {
            Title = "UNRETURNED BOOKS";
        }

        protected override void CreateCommands()
        {
            
        }

        public override void Reset()
        {
            
        }
    }
}
