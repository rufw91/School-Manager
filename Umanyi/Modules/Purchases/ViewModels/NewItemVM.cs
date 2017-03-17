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
        bool isTaxable;
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
            IsTaxable = true;
            var t = await DataController.GetAllItemCategoriesAsync();
            var exp = t.First(o => o.Description.ToUpperInvariant() == "EXPENSES").ItemCategoryID;
            var accs = t.Where(o => o.ParentCategoryID == exp);
            AllItemCategories = new ObservableCollection<ItemCategoryModel>(accs);
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
                && item.Cost > 0 && item.Price > 0 && item.ItemCategoryID > 0;
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

        public bool IsTaxable
        {
            get { return this.isTaxable; }

            set
            {
                if (value != this.isTaxable)
                {
                    this.isTaxable = value;
                    if (!isTaxable)
                        NewItem.VatID = 0;
                    NotifyPropertyChanged("IsTaxable");
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

        public async override void Reset()
        {
            NewItem.Reset();
            IsTaxable = true;
            var t =DataController.GetAllItemCategoriesAsync().Result;
            var exp = t.First(o => o.Description.ToUpperInvariant() == "EXPENSES").ItemCategoryID;
            var accs = t.Where(o => o.ParentCategoryID == exp);
            AllItemCategories = new ObservableCollection<ItemCategoryModel>(accs);
        }
    }
}
