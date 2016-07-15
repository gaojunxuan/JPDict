using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace JapaneseDict.GUI.Helpers
{
    public class StringEquationToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null || parameter == null)
                return false;
            string checkvalue = value.ToString();
            string targetvalue = parameter.ToString();
            bool r = checkvalue.Equals(targetvalue, StringComparison.OrdinalIgnoreCase);
            return r;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value == null || parameter == null)
                return null;
            bool usevalue = (bool)value;
            if (usevalue)
                return parameter.ToString();
            return null;
        }
    }
}
