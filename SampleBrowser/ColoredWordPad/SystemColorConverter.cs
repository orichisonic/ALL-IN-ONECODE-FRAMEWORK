using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;
namespace ColoredWordPad
{
  public  class SystemColorConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null) { return ""; }
            try
            {
                Color color = (Color)value;
                return SystemNamedBrushes.ColorName(color);
            }
            catch
            {
                return "";
            }
           
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string name = value as string;
            return SystemNamedBrushes.Color(name);
        }

        #endregion
    }
}
