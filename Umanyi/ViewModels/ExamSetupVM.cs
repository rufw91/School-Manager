using Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UmanyiSMS.ViewModels
{
    public class ExamSetupVM:ViewModelBase
    {
        public ExamSetupVM()
        {
            InitVars();
            CreateCommands();
        }

        protected override void InitVars()
        {
            Title = "EXAMS & GRADES SETUP";
        }

        protected override void CreateCommands()
        {
        }

        public override void Reset()
        {
        }
    }
}
