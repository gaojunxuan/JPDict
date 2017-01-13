using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace JapaneseDict.GUI.Helpers
{
    public class TimeSpanToDouble : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        { return (value == null) ? 0 : ((TimeSpan)value).TotalSeconds; }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            double seconds;
            Double.TryParse(value.ToString(), out seconds);
            return TimeSpan.FromSeconds(seconds);
        }
    }

    public class DurationToDouble : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        { return (value == null) ? 0 : ((Duration)value).TimeSpan.TotalSeconds; }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        { throw new NotImplementedException(); }
    }
}
