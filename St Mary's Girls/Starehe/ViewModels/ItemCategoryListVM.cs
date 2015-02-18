using Helper;
using Helper.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Security.Permissions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Starehe.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "Accounts")]
    public class ItemCategoryListVM : ViewModelBase
    {
        ObservableCollection<ItemCategoryModel> currentItems;
        ObservableCollection<ItemCategoryModel> originalItems;
        bool isReadOnly;
        public ItemCategoryListVM()
        {
            InitVars();
            CreateCommands();
        }

        protected async override void InitVars()
        {
            IsBusy = true;
            IsReadOnly = true;
            Task<ObservableCollection<ItemCategoryModel>> pTemp = DataAccess.GetAllItemCategoriesAsync();
            Task<ObservableCollection<ItemCategoryModel>> cTemp = DataAccess.GetAllItemCategoriesAsync();

            await Task.WhenAll(new Task[] { pTemp, cTemp });
            originalItems = pTemp.Result;
            CurrentItems = cTemp.Result;
            IsBusy = false;
        }

        protected override void CreateCommands()
        {

            RefreshCommand = new RelayCommand(o => { IsBusy = true; Reset(); IsBusy = false; }, o => !IsBusy);

            SaveCommand = new RelayCommand(async o =>
            {
                IsBusy = true;
                ObservableCollection<int> updIndex = GetIndexesToUpdate();
                bool succ = await UpdateDatabaseFromIndexAsync(updIndex);
                if (succ)
                {
                    MessageBox.Show("Successfully updated records.");
                    RefreshCommand.Execute(null);
                }
                else
                    MessageBox.Show("Could not save changes.");
                IsBusy = false;
            }, o => { return !IsBusy&&(GetIndexesToUpdate().Count > 0); });
        }

        public ICommand SaveCommand
        {
            get;
            private set;
        }

        public ICommand RefreshCommand
        {
            get;
            private set;
        }

        public bool IsReadOnly
        {
            get { return this.isReadOnly; }

            set
            {
                if (value != this.isReadOnly)
                {
                    this.isReadOnly = value;
                    NotifyPropertyChanged("IsReadOnly");
                }
            }
        }

        public ObservableCollection<ItemCategoryModel> CurrentItems
        {
            get { return this.currentItems; }

            set
            {
                if (value != this.currentItems)
                {
                    this.currentItems = value;
                    NotifyPropertyChanged("CurrentItems");
                }
            }
        }

        private bool AreEqual(ItemCategoryModel p1, ItemCategoryModel p2)
        {
            return (p1.ItemCategoryID == p2.ItemCategoryID) &&
                (p1.Description == p2.Description);
        }

        private string CreateUpdateString(ItemCategoryModel plist)
        {
            string temp = "UPDATE [Sales].[ItemCategory] SET Description='" + plist.Description +
                "', ItemCategoryID="
                + plist.ItemCategoryID + " WHERE ItemCategoryID='" + plist.ItemCategoryID + "'";
            return temp;
        }

        private Task<bool> UpdateDatabaseFromIndexAsync(ObservableCollection<int> indexes)
        {

            string updateString = "";
            bool t = true;

            foreach (int index in indexes)
            {
                updateString = CreateUpdateString(originalItems[index]);
                t = t && DataAccessHelper.ExecuteNonQuery(updateString);
            }
            return null;
        }

        private ObservableCollection<int> GetIndexesToUpdate()
        {
            if ((originalItems == null) || (CurrentItems == null))
                return new ObservableCollection<int>();
            ObservableCollection<int> temp = new ObservableCollection<int>();
            for (int i = 0; i < originalItems.Count; i++)
            {
                if (!AreEqual(originalItems[i], CurrentItems[i]))
                {
                    temp.Add(i);
                }
            }

            return temp;
        }

        public async override void Reset()
        {
            IsReadOnly = true;
            Task<ObservableCollection<ItemCategoryModel>> pTemp = DataAccess.GetAllItemCategoriesAsync();
            Task<ObservableCollection<ItemCategoryModel>> cTemp = DataAccess.GetAllItemCategoriesAsync();

            await Task.WhenAll(new Task[] { pTemp, cTemp });
            originalItems = pTemp.Result;
            CurrentItems = cTemp.Result;
        }
    }
}

