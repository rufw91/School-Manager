using Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Starehe.ViewModels
{
    public class AllBooksVM: ViewModelBase
    {
        public AllBooksVM()
        {
            InitVars();
            CreateCommands();
        }
        protected override void InitVars()
        {
            Title = "ALL BOOKS";
        }

        protected override void CreateCommands()
        {
            
        }

        public override void Reset()
        {
            
        }
    }
}
