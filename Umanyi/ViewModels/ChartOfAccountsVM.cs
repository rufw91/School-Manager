using Helper;
using Helper.Models;
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
        private ObservableCollection<AccountModel> chartOfAccounts;
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

        public ObservableCollection<AccountModel> ChartOfAccounts
        { get { return chartOfAccounts; } }
    }
}
