using Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Starehe.ViewModels
{
    public class QuickBooksSyncVM:ViewModelBase
    {
        public QuickBooksSyncVM()
        {
            InitVars();
            CreateCommands();
        }
        protected override void InitVars()
        {
            Title = "QUICKBOOKS SYNC";
        }

        protected override void CreateCommands()
        {
           
        }

        public override void Reset()
        {
            
        }
    }
}
