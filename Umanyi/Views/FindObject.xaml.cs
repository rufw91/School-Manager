using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Reflection;
using Helper.Controls;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.Collections;
using System.Windows.Input;
using System.Collections.Generic;
using System.Windows.Controls;

namespace UmanyiSMS.Views
{
    public partial class FindObject : CustomWindow, INotifyPropertyChanged
    {
        dynamic selectedItem;
        dynamic selectedItems;
        dynamic allItems;
        string searchText;
        Type searchModelType;
        private string searchProperty;
        Func<dynamic> getItemsFunction;

        public event PropertyChangedEventHandler PropertyChanged;
        private string title;
        private List<DataGridTextColumn> displayColumns;
        
        protected void NotifyPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null) { var e = new PropertyChangedEventArgs(propertyName); handler(this, e); }
        }
        public FindObject()
        {
            selectedItems = new ObservableCollection<dynamic>();
            InitializeComponent();
            SearchText = "";
            PropertyChanged += (o, e) =>
                {
                    if (e.PropertyName=="GetItemsFunction"&&GetItemsFunction!=null)
                    {
                        GetAllItems(GetItemsFunction, this.Dispatcher);
                    }
                    if (e.PropertyName == "DisplayColumns" && DisplayColumns != null&& DisplayColumns.Count>0)
                    {
                        foreach (var t in displayColumns)
                            dtGrid.Columns.Add(t);
                    }
                };
            
            AllItems = new ObservableCollection<dynamic>();

            dtGrid.SelectionChanged += (o, e) =>
            {
                this.selectedItems.Clear();
                this.selectedItem = dtGrid.SelectedItem;
                foreach (var i in dtGrid.SelectedItems)
                    this.SelectedItems.Add(i);
            };
            DataContext = this;
        }

        public Func<dynamic> GetItemsFunction
        {
            get { return getItemsFunction; }
             set
            {
                if (value != this.getItemsFunction)
                {
                    this.getItemsFunction = value;
                    NotifyPropertyChanged("GetItemsFunction");

                }
            }
        }

        private async void GetAllItems(Func<dynamic> getItemsDelegate,Dispatcher d)
        {
            await Task.Run(() =>
            {
                var t = getItemsDelegate.Invoke();
                foreach(var im in t)
                d.Invoke(() => { AllItems.Add(im); });
            });
        }

        private void Search()
        {
            dtGrid.Items.Filter = null;
            dtGrid.Items.Filter = new Predicate<object>((item) =>
            {
                if (item != null)
                {
                    string st = item.GetType().GetProperty(SearchProperty).GetValue(item).ToString();
                    return st.ToUpperInvariant().Contains(searchText.ToUpperInvariant());
                }
                else
                    return true;

            });
        }

        private void dtGrid_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
                FocusManager.SetFocusedElement(this, btnFinish);
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

        public dynamic AllItems
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

        public List<DataGridTextColumn> DisplayColumns
        {
            get { return displayColumns; }
            set
            {
                if (value != this.displayColumns)
                {
                    this.displayColumns = value;
                    NotifyPropertyChanged("DisplayColumns");

                }
            }
        }

        public dynamic SelectedItem
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

        public dynamic SelectedItems
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

        public string SearchProperty
        {
            get { return searchProperty; }
            set
            {
                if (value != this.searchProperty)
                {
                    this.searchProperty = value;
                    Search();
                }
            }
        }
    }
}
