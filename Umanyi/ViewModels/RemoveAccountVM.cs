using Helper;
using Helper.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace UmanyiSMS.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "Accounts")]
    public class RemoveAccountVM : ViewModelBase
    {
        AccountModel selectedAccount;
        private ObservableCollection<AccountModel> chartOfAccounts;
        public RemoveAccountVM()
        {
            InitVars();
            CreateCommands();
        }
        protected async override void InitVars()
        {
            Title = "REMOVE ACCOUNT";
            chartOfAccounts = await DataAccess.GetChartOfAccountsAsync();
            NotifyPropertyChanged("ChartOfAccounts");
        }

        protected override void CreateCommands()
        {
            DeleteCommand = new RelayCommand(async o =>
            {
                if (MessageBoxResult.Yes == MessageBox.Show("Are you sure you would like to delete this Account: \r\nAccount ID:"
                    + selectedAccount.AccountID + "\r\nName:" + selectedAccount.Name, "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning))
                {
                    bool succ = await DataAccess.RemoveAccountAsync(selectedAccount.AccountID);
                    MessageBox.Show(succ ? "Successfully removed account" : "Could not delete account. Account may be in use by Child accounts or some purchase records may be orphaned by this operation.", succ ? "Success" : "Error",
                            MessageBoxButton.OK, succ ? MessageBoxImage.Information : MessageBoxImage.Warning);
                    if (succ)
                        Reset();
                }

            }, o => CanDelete());
        }

        public ObservableCollection<AccountModel> ChartOfAccounts
        { get { return chartOfAccounts; } }

        private bool CanDelete()
        {
            return
            (selectedAccount != null) && (selectedAccount.AccountID > 0);
        }

        public ICommand DeleteCommand
        {
            get;
            private set;
        }

        public AccountModel SelectedAccount
        {
            get { return selectedAccount; }

            private set
            {
                if (value != selectedAccount)
                {
                    selectedAccount = value;
                    NotifyPropertyChanged("SelectedAccount");
                }
            }
        }

        public async override void Reset()
        {
            chartOfAccounts = await DataAccess.GetChartOfAccountsAsync();
            NotifyPropertyChanged("ChartOfAccounts");
            selectedAccount = null;
        }
    }
}
