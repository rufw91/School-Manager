using Helper.Models;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Data;

namespace Helper.Converters
{
    public class ItemListToItemFindListConverter  : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ObservableCollection<ItemFindModel> res = new ObservableCollection<ItemFindModel>();
            var items = (ObservableCollection<ItemModel>)value;
            foreach (var i in items)
            {
                res.Add(new ItemFindModel(i));
            }
            return res;
        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ObservableCollection<ItemModel> res = new ObservableCollection<ItemModel>();
            var items = (ObservableCollection<ItemFindModel>)value;
            foreach (var i in items)
            {
                res.Add(i);
            }
            return res;
        }
    }
}
