using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Permissions;
using System.Windows;
using System.Windows.Input;
using UmanyiSMS.Lib;
using UmanyiSMS.Modules.Purchases.Models;
using UmanyiSMS.Modules.Purchases.Controller;
using UmanyiSMS.Lib.Presentation;

namespace UmanyiSMS.Modules.Purchases.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "Accounts")]
    public class NewItemVM: ViewModelBase
    {
        ItemModel item;
        ObservableCollection<ItemCategoryModel> allItemCategories;
        public NewItemVM()
        {
            InitVars();
            CreateCommands();
        }

        protected async override void InitVars()
        {
            Title = "NEW ITEM / SERVICE";
            IsBusy = true;
            NewItem = new ItemModel();
            var t = await DataController.GetAllItemCategoriesAsync();
            AllItemCategories = new ObservableCollection<ItemCategoryModel>(t);
            IsBusy = false;
        }

        protected override void CreateCommands()
        {
            
            SaveCommand = new RelayCommand(async o =>
            {
                IsBusy = true;
                if (!item.HasErrors)
                {
                    bool res = await DataController.SaveNewItemAsync(item);
                    if (res)
                    {
                        MessageBox.Show("Successfully saved new Item.");
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
            return item.ItemID > 0 && !string.IsNullOrWhiteSpace(item.Description)
                && item.Cost > 0 && item.ItemCategoryID > 0;
        }

        public ICommand SaveCommand
        { get; private set; }

        public ItemModel NewItem
        {
            get { return this.item; }

            private set
            {
                if (value != this.item)
                {
                    this.item = value;
                    NotifyPropertyChanged("NewItem");
                }
            }
        }
        
        public ObservableCollection<ItemCategoryModel> AllItemCategories
        {
            get { return this.allItemCategories; }

            private set
            {
                if (value != this.allItemCategories)
                {
                    this.allItemCategories = value;
                    NotifyPropertyChanged("AllItemCategories");
                }
            }
        }

        public override void Reset()
        {
            NewItem.Reset();
        }
    }
}
