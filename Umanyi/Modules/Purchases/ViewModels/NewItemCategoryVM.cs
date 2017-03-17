using System.Security.Permissions;
using System.Windows;
using System.Windows.Input;
using UmanyiSMS.Lib;
using UmanyiSMS.Lib.Presentation;
using UmanyiSMS.Modules.Purchases.Controller;
using UmanyiSMS.Modules.Purchases.Models;

namespace UmanyiSMS.Modules.Purchases.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "Accounts")]
    public class NewItemCategoryVM : ViewModelBase
    {
        ItemCategoryModel category;
        public NewItemCategoryVM()
        {
            InitVars();
            CreateCommands();
        }

        protected override void InitVars()
        {
            Title = "NEW ACCOUNT";
            IsBusy = true;
            NewCategory = new ItemCategoryModel();
            IsBusy = false;
            NotifyPropertyChanged("ChartOfAccounts");
        }

        protected override void CreateCommands()
        {
            SaveCommand = new RelayCommand(async o =>
            {
                IsBusy = true;
                if (!category.HasErrors)
                {
                    bool res = await DataController.SaveNewItemCategoryAsync(category);
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
        
        public override void Reset()
        {
            NewCategory.Reset();
        }
    }
}
