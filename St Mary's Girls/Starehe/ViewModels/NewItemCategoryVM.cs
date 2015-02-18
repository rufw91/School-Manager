using Helper;
using Helper.Models;
using System.Security.Permissions;
using System.Windows;
using System.Windows.Input;

namespace Starehe.ViewModels
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
            Title = "New Item Category";
            IsBusy = true;
            NewCategory = new ItemCategoryModel();
            IsBusy = false;
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
            return !string.IsNullOrWhiteSpace(category.Description);
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
