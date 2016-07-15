using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace JapaneseDict.GUI.Helpers
{
    public class BooleanToVisibilityNegateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var v = value is bool ? !(bool)value : value;
            return (v is bool && (bool)v) ? Visibility.Visible : Visibility.Collapsed;
        }

      
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            var v= Convert(value, targetType, parameter, language);
            return v is Visibility && (Visibility)v == Visibility.Visible;
        }

    }
}
