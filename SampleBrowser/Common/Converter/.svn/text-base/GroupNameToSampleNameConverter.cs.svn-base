using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace SampleBrowser.Common
{
    [ValueConversion(typeof(string), typeof(string))]
    public class GroupNameToSampleNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string result = string.Empty;
            if (targetType != typeof(string))
                return null;
            if (value != null && value.ToString() != string.Empty)
            {
                string[] str = value.ToString().Split(',');
                result = str[0];
            }
            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
