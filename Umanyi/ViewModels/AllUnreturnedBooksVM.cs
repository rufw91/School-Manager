using Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UmanyiSMS.ViewModels
{
    public class AllUnreturnedBooksVM: ViewModelBase
    {
        public AllUnreturnedBooksVM()
        {
            InitVars();
            CreateCommands();
        }
        protected override void InitVars()
        {
            Title = "ALL UNRETURNED BOOKS";
        }

        protected override void CreateCommands()
        {
            
        }

        public override void Reset()
        {
            throw new NotImplementedException();
        }
    }
}
