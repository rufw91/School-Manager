﻿using Helper;
using Helper.Controls;
using Helper.Models;
using System;
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
    public partial class FindBooks : CustomWindow, INotifyPropertyChanged
    {
        BookModel selectedItem;
        ObservableImmutableList<BookModel> selectedItems;
        
        string searchText;
        public FindBooks()
        {
            selectedItems = new ObservableImmutableList<BookModel>();
            InitializeComponent();
            SearchText = "";
            GetAllItems(this.Dispatcher);
            AllItems = new ObservableImmutableList<BookModel>();
            
            dtGrid.SelectionChanged += (o, e) =>
                {
                    this.selectedItems.Clear();
                    this.selectedItem = dtGrid.SelectedItem as BookModel;
                    foreach (var i in dtGrid.SelectedItems)
                        this.SelectedItems.Add((i as BookModel));
                };
            DataContext = this;
        }

        private void Search()
        {
            dtGrid.Items.Filter = null;
            dtGrid.Items.Filter = new Predicate<object>((item) =>
            {
                BookModel findItem = item as BookModel;
                if (findItem != null)
                    return findItem.Title.ToUpperInvariant().Contains(searchText.ToUpperInvariant()) ||
                        findItem.Publisher.ToUpperInvariant().Contains(searchText.ToUpperInvariant()) ||
                        findItem.ISBN.ToUpperInvariant().Contains(searchText.ToUpperInvariant()) ||
                        (findItem.Author.ToString().ToUpperInvariant().Contains(searchText.ToUpperInvariant()));
                else
                    return true;

            });
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private ObservableImmutableList<BookModel> allItems;
        protected void NotifyPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null) { var e = new PropertyChangedEventArgs(propertyName); handler(this, e); }
        }
        public BookModel SelectedItem
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

        public ObservableImmutableList<BookModel> SelectedItems
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
            await Task.Factory.StartNew(() =>
            {
                string selectStr = "SELECT BookID," +
                                   "ISBN," +
                                   "[Name]," +
                                   "Author," +
                                   "Publisher," +
                                   "Price" +
                                   " FROM [Institution].[Book]";
                try
                {
                    using (SqlConnection DBConnection = DataAccessHelper.CreateConnection())
                    {
                        SqlCommand cmd = new SqlCommand();
                        cmd.CommandText = "USE UmanyiSMS\r\nSET DATEFORMAT DMY\r\n" + selectStr;
                        cmd.Connection = DBConnection;
                        SqlDataReader reader = cmd.ExecuteReader();
                        BookModel im;
                        while (reader.Read())
                        {
                            var dtr = (IDataRecord)reader;
                            im = new BookModel();
                            im.BookID = int.Parse(dtr[0].ToString());
                            im.ISBN = dtr[1].ToString();
                            im.Title = dtr[2].ToString();
                            im.Author = dtr[3].ToString();
                            im.Publisher = dtr[4].ToString();
                            im.Price = decimal.Parse(dtr[5].ToString());
                            allItems.Add(im);
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

        public ObservableImmutableList<BookModel> AllItems
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