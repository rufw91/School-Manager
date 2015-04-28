using Helper;
using Helper.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace UmanyiSMS.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "Teacher")]
    public class DisciplineDetailsVM:ViewModelBase
    {
        DisciplineModel discipline;
        public DisciplineDetailsVM()
        {
            InitVars();
            CreateCommands();
        }

        public DisciplineDetailsVM(DisciplineModel discipline)
        {
            InitVars();
            CreateCommands();
            this.discipline.CopyFrom(discipline);
        }

        protected override void InitVars()
        {
            Title = "Discipline Recrd Details";
            discipline = new DisciplineModel();
        }

        protected override void CreateCommands()
        {            
        }

        public override void Reset()
        {
            
        }

        public DisciplineModel CurrentDiscipline
        {
            get { return discipline; }
        }
    }
}
