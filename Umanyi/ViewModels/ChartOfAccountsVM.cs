using Helper;
using Helper.Models;
using Helper.Presentation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UmanyiSMS.ViewModels
{
    public class ChartOfAccountsVM : ViewModelBase
    {
        private ObservableCollection<IAccount> chartOfAccounts;
        public ChartOfAccountsVM()
        {
            InitVars();
            CreateCommands();
        }
        public override void Reset()
        {
           
        }

        protected override void CreateCommands()
        {
           
        }

        protected async override void InitVars()
        {
            Title = "CHART OF ACCOUNTS";
            
            chartOfAccounts = await DataAccess.GetChartOfAccountsAsync();
            NotifyPropertyChanged("ChartOfAccounts");
        }

        public ObservableCollection<IAccount> ChartOfAccounts
        { get { return chartOfAccounts; } }
    }
}
