using Helper;
using Helper.Models;
using System.Collections.ObjectModel;
using System.Security.Permissions;
using System.Windows;
using System.Windows.Input;

namespace UmanyiSMS.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "Accounts")]
    public class ModifyItemVM: ViewModelBase
    {
        ModifyItemModel item;
        bool isTaxable;
        ObservableCollection<ItemCategoryModel> allItemCategories;
        ObservableCollection<VATRateModel> allVats;
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
            IsTaxable = true;
            AllItemCategories = await DataAccess.GetAllItemCategoriesAsync();
            AllVats = await DataAccess.GetAllVatsAsync();
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
                bool res = await DataAccess.UpdateItemAsync(item);
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

        public ObservableCollection<VATRateModel> AllVats
        {
            get { return this.allVats; }

            private set
            {
                if (value != this.allVats)
                {
                    this.allVats = value;
                    NotifyPropertyChanged("AllVats");
                }
            }
        }


        public override void Reset()
        {            
            item.Reset();
            IsTaxable = true;
        }
    }
}
