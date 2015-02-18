using Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Starehe.ViewModels
{
    public class AggregateResultsVM: ViewModelBase
    {
        public AggregateResultsVM()
        {
            InitVars();
            CreateCommands();
        }
        protected override void InitVars()
        {
            Title = "AGGREGATE RESULTS";
        }

        protected override void CreateCommands()
        {
        }

        public override void Reset()
        {
        }
    }
}
