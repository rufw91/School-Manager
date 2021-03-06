﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;

namespace UmanyiSMS.Lib.Converters
{
    public class ValidationErrorConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return value;
            if (value is string)
                return (string)value;

            if (value is List<String>)
            {
                string temp = "";
                var t=(List<string>)value;
                foreach(var f in t )
                    temp += " - " + f + "\r\n";
                temp=temp.Remove(temp.Length - 1);
                return temp;
            }
            
            return value;
        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
