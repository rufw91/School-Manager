using Helper;
using Helper.Controls;
using Helper.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace UmanyiSMS.Views
{
    public partial class FindItems : CustomWindow, INotifyPropertyChanged
    {
        ItemFindModel selectedItem;
        ObservableCollection<ItemFindModel> selectedItems;

        string searchText;
        public FindItems()
        {
            selectedItems = new ObservableCollection<ItemFindModel>();
            InitializeComponent();
            SearchText = "";
            GetAllItems(this.Dispatcher);
            AllItems = new ObservableCollection<ItemModel>();

            dtGrid.SelectionChanged += (o, e) =>
            {
                this.selectedItems.Clear();
                this.selectedItem = new ItemFindModel(dtGrid.SelectedItem as ItemModel);
                foreach (var i in dtGrid.SelectedItems)
                    this.SelectedItems.Add(new ItemFindModel(i as ItemModel));
            };
            DataContext = this;
        }

        private void Search()
        {
            dtGrid.Items.Filter = null;
            dtGrid.Items.Filter = new Predicate<object>((item) =>
            {
                ItemFindModel findItem = item as ItemFindModel;
                if (findItem != null)
                    return findItem.Description.ToUpperInvariant().Contains(searchText.ToUpperInvariant()) ||
                        (findItem.ItemID.ToString().ToUpperInvariant().Contains(searchText.ToUpperInvariant()));
                else
                    return true;

            });
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private ObservableCollection<ItemModel> allItems;
        protected void NotifyPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null) { var e = new PropertyChangedEventArgs(propertyName); handler(this, e); }
        }
        public ItemFindModel SelectedItem
        {
            get { return selectedItem; }
            private set
            {
                if (value != this.selectedItem)
                {
                    this.selectedItem = value;
                    NotifyPropertyChanged("SelectedItems");
                }
            }
        }

        public ObservableCollection<ItemFindModel> SelectedItems
        {
            get { return selectedItems; }
        }

        public string SearchText
        {
            get { return searchText; }
            set
            {
                if (value != this.searchText)
                {
                    this.searchText = value;
                    Search();
                }
            }
        }

        private void btnFinish_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.SelectedItem = null;
            this.Visibility = System.Windows.Visibility.Collapsed;
        }

        private async void GetAllItems(Dispatcher dispatcher)
        {
            await Task.Run(() =>
            {
                string selectStr = "SELECT ItemID," +
                                   "Description," +
                                   "DateAdded," +
                                   "ItemCategoryID," +
                                   "Price," +
                                   "Cost," +
                                   "StartQuantity," +
                                   "VatID" +
                                   " FROM [Sales].[Item]";
                try
                {
                    using (SqlConnection DBConnection = DataAccessHelper.Helper.CreateConnection())
                    {
                        SqlCommand cmd = new SqlCommand();
                        cmd.CommandText = "USE UmanyiSMS\r\nSET DATEFORMAT DMY\r\n" + selectStr;
                        cmd.Connection = DBConnection;
                        SqlDataReader reader = cmd.ExecuteReader();
                        ItemModel im;
                        while (reader.Read())
                        {
                            var dtr = (IDataRecord)reader;
                            im = new ItemModel();
                            im.ItemID = long.Parse(dtr[0].ToString());
                            im.Description = dtr[1].ToString();
                            im.DateAdded = DateTime.Parse(dtr[2].ToString());
                            im.ItemCategoryID = int.Parse(dtr[3].ToString());
                            im.Price = decimal.Parse(dtr[4].ToString());
                            im.Cost = decimal.Parse(dtr[5].ToString());
                            im.StartQuantity = decimal.Parse(dtr[6].ToString());
                            im.VatID = int.Parse(dtr[7].ToString());
                            dispatcher.Invoke(() => { AllItems.Add(im); });
                        }

                        reader.Close();
                        cmd.Dispose();
                    }
                }
                catch (Exception e)
                {
                    throw e;
                }

            });
        }

        private void dtGrid_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Space)
                if (dtGrid.SelectedItem != null)
                    (dtGrid.SelectedItem as ItemFindModel).IsSelected = !(dtGrid.SelectedItem as ItemFindModel).IsSelected;
            if (e.Key == System.Windows.Input.Key.Enter)
                FocusManager.SetFocusedElement(this, btnFinish);
        }

        public ObservableCollection<ItemModel> AllItems
        {
            get { return allItems; }
            private set
            {
                if (value != this.allItems)
                {
                    this.allItems = value;
                    NotifyPropertyChanged("AllItems");

                }
            }
        }
    }
}
