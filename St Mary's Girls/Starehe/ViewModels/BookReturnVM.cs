using Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Starehe.ViewModels
{
    public class BookReturnVM: ViewModelBase
    {
        public BookReturnVM()
        {
            InitVars();
            CreateCommands();
        }
        protected override void InitVars()
        {
            Title = "BOOK RETURN";
        }

        protected override void CreateCommands()
        {
            
        }

        public override void Reset()
        {
            
        }
    }
}
