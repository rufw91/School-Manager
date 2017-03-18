using System.Collections.ObjectModel;
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
    public class ModifyItemVM: ViewModelBase
    {
        ModifyItemModel item;
        ObservableCollection<ItemCategoryModel> allItemCategories;
        public ModifyItemVM()
        {
            InitVars();
            CreateCommands();
        }

        protected async override void InitVars()
        {
            Title = "MODIFY ITEM";
            IsBusy = true;
            NewItem = new ModifyItemModel();
            AllItemCategories = await DataController.GetAllItemCategoriesAsync();
            IsBusy = false;
            NewItem.PropertyChanged += (o, e) =>
                {
                    if (e.PropertyName=="ItemID")
                        item.CheckErrors();
                };
        }

        protected override void CreateCommands()
        {

            SaveCommand = new RelayCommand(async o =>
            {
                IsBusy = true;
                bool res = await DataController.UpdateItemAsync(item);
                if (res)
                {
                    MessageBox.Show("Successfully Updated Item.");
                    Reset();
                }
                else
                    MessageBox.Show("Could not update item.");

                IsBusy = false;
            }, o => !IsBusy && CanSave());

        }

        private bool CanSave()
        {            
            return !item.HasErrors && !string.IsNullOrWhiteSpace(item.Description)
                && item.Cost > 0 && item.Price > 0 && item.ItemCategoryID > 0;
        }

        public ICommand SaveCommand
        { get; private set; }

        public ModifyItemModel NewItem
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
            item.Reset();
        }
    }
}
