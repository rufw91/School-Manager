


using System;
using System.Linq;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using UmanyiSMS.Lib.Controllers;
using UmanyiSMS.Lib.Controls;
using UmanyiSMS.Modules.Purchases.Models;

namespace UmanyiSMS.Modules.Purchases.Views
{
    public partial class FindItems : CustomWindow, INotifyPropertyChanged
    {
       string searchText;
        public FindItems()
        {
            InitializeComponent();
            SearchText = "";
            GetAllItems(this.Dispatcher);
            AllItems = new ObservableCollection<ItemFindModel>();
            
            DataContext = this;
        }

        private void Search()
        {
            dtGrid.Items.Filter = null;
            dtGrid.Items.Filter = (item) => Controller.DataController.SearchAllItemProperties(item as ItemFindModel, searchText);
        }
        
        public event PropertyChangedEventHandler PropertyChanged;
        private ObservableCollection<ItemFindModel> allItems;
        protected void NotifyPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null) { var e = new PropertyChangedEventArgs(propertyName); handler(this, e); }
        }
        
        public ObservableCollection<ItemFindModel> SelectedItems
        {
            get { return new ObservableCollection<ItemFindModel>(allItems.Where(o=>o.IsSelected)); }
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
            this.Visibility = Visibility.Collapsed;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
        }

        private async void GetAllItems(Dispatcher dispatcher)
        {
            await Task.Run(() =>
            {
                string selectStr = "SELECT ItemID," +
                                   "Description," +
                                   "DateAdded," +
                                   "ItemCategoryID," +
                                   "Cost" +
                                   " FROM [Item]";
                try
                {
                    using (SqlConnection DBConnection = DataAccessHelper.Helper.CreateConnection())
                    {
                        SqlCommand cmd = new SqlCommand();
                        cmd.CommandText = "USE UmanyiSMS\r\nSET DATEFORMAT DMY\r\n" + selectStr;
                        cmd.Connection = DBConnection;
                        SqlDataReader reader = cmd.ExecuteReader();
                        ItemFindModel im;
                        while (reader.Read())
                        {
                            var dtr = (IDataRecord)reader;
                            im = new ItemFindModel();
                            im.ItemID = long.Parse(dtr[0].ToString());
                            im.Description = dtr[1].ToString();
                            im.DateAdded = DateTime.Parse(dtr[2].ToString());
                            im.ItemCategoryID = int.Parse(dtr[3].ToString());
                            im.Cost = decimal.Parse(dtr[4].ToString());
                            im.IsSelected = false;
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
        
        public ObservableCollection<ItemFindModel> AllItems
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
