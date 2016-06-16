using Helper;
using Helper.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UmanyiSMS.ViewModels
{
    public class PrepareBudgetVM:ViewModelBase
    {
        private BudgetModel newBudget;
        public PrepareBudgetVM()
        {
            InitVars();
            CreateCommands();
        }
        protected async override void InitVars()
        {
            Title = "PREPARE BUDGET";
            NewBudget = await DataAccess.GetCurrentBudgetAsync();
        }

        protected override void CreateCommands()
        {
        }

        public BudgetModel NewBudget
        {
            get { return this.newBudget; }

            set
            {
                if (value != this.newBudget)
                {
                    this.newBudget = value;
                    NotifyPropertyChanged("NewBudget");
                }
            }
        }

        public override void Reset()
        {
        }
    }
}
