﻿using Helper;
using Helper.Models;
using System.Collections.ObjectModel;
using System.Security.Permissions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Starehe.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "Accounts")]
    public class ItemListVM: ViewModelBase
    {
        ObservableCollection<ItemListModel> currentItems;
        ObservableCollection<ItemListModel> originalItems;
        bool isReadOnly;
        public ItemListVM()
        {
            InitVars();
            CreateCommands();
        }

        protected async override void InitVars()
        {
            IsBusy = true;
            Title = "Item List";
            IsReadOnly = true;
            Task<ObservableCollection<ItemListModel>> pTemp = DataAccess.GetAllItemsWithCurrentQuantityAsync();
            Task<ObservableCollection<ItemListModel>> cTemp = DataAccess.GetAllItemsWithCurrentQuantityAsync();

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

        public ObservableCollection<ItemListModel> CurrentItems
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

        private bool AreEqual(ItemModel p1, ItemModel p2)
        {
            return (p1.Cost == p2.Cost) &&
                (p1.Description == p2.Description) &&
                (p1.Price == p2.Price) &&
                (p1.ItemID == p2.ItemID);
        }

        private string CreateUpdateString(ItemModel plist)
        {
            string temp = "UPDATE Sales.Item SET Description='" + plist.Description + 
                "', ItemCategoryId='"
                + plist.ItemCategoryID + "', " + "Price='" + plist.Price.ToString() +
                "', Cost='" + plist.Cost.ToString() + "' WHERE ItemID='" + plist.ItemID + "'"; 
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
            if ((originalItems==null)|| (CurrentItems==null))
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
            Task<ObservableCollection<ItemListModel>> pTemp = DataAccess.GetAllItemsWithCurrentQuantityAsync();
            Task<ObservableCollection<ItemListModel>> cTemp = DataAccess.GetAllItemsWithCurrentQuantityAsync();

            await Task.WhenAll(new Task[] { pTemp, cTemp });
            originalItems = pTemp.Result;
            CurrentItems = cTemp.Result;
        }
    }
}