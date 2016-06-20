using Helper.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Helper.Converters
{
    public class AccountToAccountIDConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return null;
            int accID = int.Parse(value.ToString());
            var name= DataAccess.GetAccountAsync(accID).Result;
            string param = parameter as string;
            if (param.Equals("nameOnly"))
                return name.Name;
            return name;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var val = value as AccountModel;
            if (val == null)
                return -1;
            return val.AccountID;
        }
    }
}
