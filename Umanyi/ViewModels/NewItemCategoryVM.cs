using Helper;
using Helper.Models;
using System.Collections.ObjectModel;
using System.Security.Permissions;
using System.Windows;
using System.Windows.Input;

namespace UmanyiSMS.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "Accounts")]
    public class NewItemCategoryVM : ViewModelBase
    {
        ItemCategoryModel category;
        private ObservableCollection<AccountModel> chartOfAccounts;
        public NewItemCategoryVM()
        {
            InitVars();
            CreateCommands();
        }

        protected async override void InitVars()
        {
            Title = "NEW ACCOUNT";
            IsBusy = true;
            NewCategory = new ItemCategoryModel();
            IsBusy = false;
            chartOfAccounts = await DataAccess.GetChartOfAccountsAsync();
            NotifyPropertyChanged("ChartOfAccounts");
        }

        protected override void CreateCommands()
        {
            SaveCommand = new RelayCommand(async o =>
            {
                IsBusy = true;
                if (!category.HasErrors)
                {
                    bool res = await DataAccess.SaveNewItemCategoryAsync(category);
                    if (res)
                    {
                        MessageBox.Show("Successfully Completed Operation.");
                        Reset();
                    }
                    else
                        MessageBox.Show("Could not save new item.");
                }
                IsBusy = false;
            }, o => !IsBusy&&CanSave());
        }

        private bool CanSave()
        {
            return !string.IsNullOrWhiteSpace(category.Description) && category.ParentCategoryID > 0; 
        }

        public ICommand SaveCommand
        { get; private set; }

        public ItemCategoryModel NewCategory
        {
            get { return category; }
            private set
            {
                if (category != value)
                {
                    category = value;
                    NotifyPropertyChanged("NewCategory");
                }
            }
        }

        public ObservableCollection<AccountModel> ChartOfAccounts
        { get { return chartOfAccounts; } }

        public override void Reset()
        {
            NewCategory.Reset();
        }
    }
}
